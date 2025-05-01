using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Lume.Application.Staff.Commands.CreateStaff;
using Xunit;

namespace Lume.Application.Tests.Staff.Commands.CreateStaff;

[TestSubject(typeof(CreateStaffCommandValidator))]
public class CreateStaffCommandValidatorTest
{

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new CreateStaffCommand
        {
            Name = "John",
            Surname = "Doe",
            PhoneNumber = "+1234567890123",
            PasswordHash = "qwerty1234"
        };
        var validator = new CreateStaffCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveValidationErrorFor(nameof(command.Name));
        result.ShouldNotHaveValidationErrorFor(nameof(command.Surname));
        result.ShouldNotHaveValidationErrorFor(nameof(command.PhoneNumber));
        result.ShouldNotHaveValidationErrorFor(nameof(command.PasswordHash));
    }

    [Theory]
    [InlineData("J", "D", "+123", "123")]
    [InlineData("51chars00000000000000000000000000000000000000000000",
        "51chars00000000000000000000000000000000000000000000", "1234567890123", "33chars00000000000000000000000000")]
    [InlineData("", "", "", "")]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors(string name, string surname,
        string phoneNumber, string passwordHash)
    {
        // arrange
        var command = new CreateStaffCommand
        {
            Name = name,
            Surname = surname,
            PhoneNumber = phoneNumber,
            PasswordHash = passwordHash
        };
        var validator = new CreateStaffCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveValidationErrorFor(nameof(command.Name));
        result.ShouldHaveValidationErrorFor(nameof(command.Surname));
        result.ShouldHaveValidationErrorFor(nameof(command.PhoneNumber));
        result.ShouldHaveValidationErrorFor(nameof(command.PasswordHash));
    }

    [Fact]
    public void Validator_ForBoundaryValues_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new CreateStaffCommand
        {
            Name = new string('A', 50),
            Surname = new string('B', 50),
            PhoneNumber = "+1234567890123",
            PasswordHash = new string('C', 8)
        };
        var validator = new CreateStaffCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveValidationErrorFor(nameof(command.Name));
        result.ShouldNotHaveValidationErrorFor(nameof(command.Surname));
        result.ShouldNotHaveValidationErrorFor(nameof(command.PhoneNumber));
        result.ShouldNotHaveValidationErrorFor(nameof(command.PasswordHash));
    }
}
