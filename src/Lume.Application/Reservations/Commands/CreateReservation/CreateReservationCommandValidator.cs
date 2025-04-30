using FluentValidation;

namespace Lume.Application.Reservations.Commands.CreateReservation;

public class CreateReservationCommandValidator : AbstractValidator<CreateReservationCommand>
{
    public CreateReservationCommandValidator()
    {
        RuleFor(dto => dto.CustomerId)
            .NotEmpty();

        RuleFor(dto => dto.Date)
            .NotEmpty();

        RuleFor(dto => dto.TableNumber)
            .NotEmpty();

        RuleFor(dto => dto.GuestCount)
            .NotEmpty();

        RuleFor(dto => dto.Status)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(dto => dto.Notes)
            .MaximumLength(150);
    }
}
