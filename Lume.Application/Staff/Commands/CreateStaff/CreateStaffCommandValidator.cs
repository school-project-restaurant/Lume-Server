using FluentValidation;

namespace Lume.Application.Staff.Commands.CreateStaff;

public class CreateStaffCommandValidator : AbstractValidator<CreateStaffCommand>
{
    public CreateStaffCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .Length(2, 50);
        
        RuleFor(dto => dto.Surname)
            .NotEmpty()
            .Length(2, 50);
        
        RuleFor(dto => dto.Email)
            .EmailAddress();

        RuleFor(dto => dto.PhoneNumber)
           .Matches(@"^\+\d{13}$");
        
        RuleFor(dto => dto.PasswordHash)
            .NotEmpty()
            .Length(8, 32);
    }
}