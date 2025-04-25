using FluentAssertions;
using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Lume.Application.Customers.Commands.CreateCustomer;
using Xunit;

namespace Lume.Application.Tests.Customers.Commands.CreateCustomer;

[TestSubject(typeof(CreateCustomerCommandValidator))]
public class CreateCustomerCommandValidatorTest
{

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new CreateCustomerCommand
        {
            Name = "John",
            Surname = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+1234567890123",
            PasswordHash = "qwerty1234"
        };
        var validator = new CreateCustomerCommandValidator();
        
        // act
        var result = validator.TestValidate(command);
        
        // assert
        result.ShouldNotHaveValidationErrorFor(nameof(command.Name));
        result.ShouldNotHaveValidationErrorFor(nameof(command.Surname));
        result.ShouldNotHaveValidationErrorFor(nameof(command.Email));
        result.ShouldNotHaveValidationErrorFor(nameof(command.PhoneNumber));
        result.ShouldNotHaveValidationErrorFor(nameof(command.PasswordHash));
    }
}