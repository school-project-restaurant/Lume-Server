using FluentValidation;

namespace Lume.Application.Reservations.Commands.UpdateReservation;

public class UpdateReservationCommandValidator : AbstractValidator<UpdateReservationCommand>
{
    public UpdateReservationCommandValidator()
    {
        RuleFor(dto => dto.Id)
            .NotEmpty();

        RuleFor(dto => dto.Date)
            .NotEmpty();

        RuleFor(dto => dto.TableNumber)
            .NotEmpty();

        RuleFor(dto => dto.GuestCount)
            .NotEmpty();

        RuleFor(dto => dto.Status)
            .MaximumLength(20);

        RuleFor(dto => dto.Notes)
            .MaximumLength(150);
    }
}
