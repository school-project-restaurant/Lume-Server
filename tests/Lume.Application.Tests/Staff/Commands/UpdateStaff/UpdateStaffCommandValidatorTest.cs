using FluentValidation.TestHelper;
using JetBrains.Annotations;
using Lume.Application.Staff.Commands.UpdateStaff;
using Xunit;

namespace Lume.Application.Tests.Staff.Commands.UpdateStaff;

[TestSubject(typeof(UpdateStaffCommandValidator))]
public class UpdateStaffCommandValidatorTest
{

    [Fact]
    public void Validator_ForValidCommand_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new UpdateStaffCommand()
        {
            Name = "John",
            Surname = "Doe",
            PhoneNumber = "+1234567890123"
        };
        var validator = new UpdateStaffCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveValidationErrorFor(nameof(command.Name));
        result.ShouldNotHaveValidationErrorFor(nameof(command.Surname));
        result.ShouldNotHaveValidationErrorFor(nameof(command.PhoneNumber));
    }
    
    [Theory]
    [InlineData("J", "D", "+123")]
    [InlineData("51chars00000000000000000000000000000000000000000000",
        "51chars00000000000000000000000000000000000000000000", "1234567890123")]
    [InlineData("", "", "")]
    public void Validator_ForInvalidCommand_ShouldHaveValidationErrors(string name, string surname,
        string phoneNumber)
    {
        // arrange
        var command = new UpdateStaffCommand()
        {
            Name = name,
            Surname = surname,
            PhoneNumber = phoneNumber
        };
        var validator = new UpdateStaffCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldHaveValidationErrorFor(nameof(command.Name));
        result.ShouldHaveValidationErrorFor(nameof(command.Surname));
        result.ShouldHaveValidationErrorFor(nameof(command.PhoneNumber));
    }
    
    [Fact]
    public void Validator_ForBoundaryValues_ShouldNotHaveValidationErrors()
    {
        // arrange
        var command = new UpdateStaffCommand()
        {
            Name = new string('A', 50),
            Surname = new string('B', 50),
            PhoneNumber = "+1234567890123"
        };
        var validator = new UpdateStaffCommandValidator();

        // act
        var result = validator.TestValidate(command);

        // assert
        result.ShouldNotHaveValidationErrorFor(nameof(command.Name));
        result.ShouldNotHaveValidationErrorFor(nameof(command.Surname));
        result.ShouldNotHaveValidationErrorFor(nameof(command.PhoneNumber));
    }
}
