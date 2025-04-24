using FluentValidation;

namespace Lume.Application.Staff.Commands.UpdateStaff;

public class UpdateStaffCommandValidator : AbstractValidator<UpdateStaffCommand>
{
    public UpdateStaffCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(dto => dto.Surname)
            .NotEmpty()
            .Length(2, 50);

        RuleFor(dto => dto.Salary)
            .NotEmpty();

        RuleFor(dto => dto.PhoneNumber)
            .Matches(@"^\+\d{13}$");
    }
}