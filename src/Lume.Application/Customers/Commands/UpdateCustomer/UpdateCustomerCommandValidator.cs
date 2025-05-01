using FluentValidation;

namespace Lume.Application.Customers.Commands.UpdateCustomer;

public class UpdateCustomerCommandValidator : AbstractValidator<UpdateCustomerCommand>
{
    public UpdateCustomerCommandValidator()
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
    }
}