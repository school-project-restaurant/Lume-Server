using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Lume.Application.Customers.Commands.UpdateCustomer;
using Xunit;

namespace Lume.Application.Tests.Customers.Commands.UpdateCustomer;

[TestSubject(typeof(UpdateCustomerCommandValidator))]
public class UpdateCustomerCommandValidatorTest
{
    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new UpdateCustomerCommand()
        {
            Name = "John",
            Surname = "Doe",
            Email = "john@example.com",
            PhoneNumber = "+1234567890123"
        };
        var validator = new UpdateCustomerCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveValidationErrorFor(nameof(command.Name));
        result.ShouldNotHaveValidationErrorFor(nameof(command.Surname));
        result.ShouldNotHaveValidationErrorFor(nameof(command.Email));
        result.ShouldNotHaveValidationErrorFor(nameof(command.PhoneNumber));
    }
}