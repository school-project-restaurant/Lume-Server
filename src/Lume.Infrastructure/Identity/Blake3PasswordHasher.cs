using System.Security.Cryptography;
using System.Text;
using Blake3;
using Lume.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Lume.Infrastructure.Identity;

/// <summary>
/// Custom implementation of the ASP.NET Core Identity password hasher using Blake3 algorithm
/// </summary>
public class Blake3PasswordHasher : IPasswordHasher<ApplicationUser>
{
    private const int SaltSize = 16;
    private const int KeySize = 32; // Blake3 produces 32 bytes output
    private const int Iterations = 10000; // Number of iterations for PBKDF2

    /// <summary>
    /// Hashes a password using Blake3 with a random salt, then applies PBKDF2 for additional security
    /// </summary>
    public string HashPassword(ApplicationUser user, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentNullException(nameof(password));
        }

        // Generate a random salt
        byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

        // Hash the password + salt with Blake3
        var hasher = Hasher.New();
        hasher.Update(Encoding.UTF8.GetBytes(password));
        hasher.Update(salt);
        byte[] blake3Hash = hasher.Finalize().AsSpan().ToArray();

        // Apply PBKDF2 for additional security
        using var pbkdf2 = new Rfc2898DeriveBytes(blake3Hash, salt, Iterations, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(KeySize);

        // Format: iterations + "." + salt + "." + hash
        return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    /// <summary>
    /// Verifies a password against a hash using Blake3
    /// </summary>
    public PasswordVerificationResult VerifyHashedPassword(ApplicationUser user, string hashedPassword, string providedPassword)
    {
        if (string.IsNullOrEmpty(hashedPassword) || string.IsNullOrEmpty(providedPassword))
        {
            return PasswordVerificationResult.Failed;
        }

        // Check the format of the hash
        string[] parts = hashedPassword.Split('.');
        if (parts.Length != 3)
        {
            return PasswordVerificationResult.Failed;
        }

        // Extract parts
        if (!int.TryParse(parts[0], out int iterations))
        {
            return PasswordVerificationResult.Failed;
        }

        byte[] salt;
        byte[] hash;

        try
        {
            salt = Convert.FromBase64String(parts[1]);
            hash = Convert.FromBase64String(parts[2]);
        }
        catch (FormatException)
        {
            return PasswordVerificationResult.Failed;
        }

        // Hash the provided password with the same salt using Blake3
        var hasher = Hasher.New();
        hasher.Update(Encoding.UTF8.GetBytes(providedPassword));
        hasher.Update(salt);
        byte[] blake3Hash = hasher.Finalize().AsSpan().ToArray();

        // Apply PBKDF2 with the same parameters
        using var pbkdf2 = new Rfc2898DeriveBytes(blake3Hash, salt, iterations, HashAlgorithmName.SHA256);
        byte[] providedHash = pbkdf2.GetBytes(KeySize);

        // Compare the hashes
        if (CryptographicOperations.FixedTimeEquals(hash, providedHash))
        {
            return PasswordVerificationResult.Success;
        }

        return PasswordVerificationResult.Failed;
    }
}
