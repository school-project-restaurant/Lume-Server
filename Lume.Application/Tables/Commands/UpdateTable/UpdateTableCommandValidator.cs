using FluentValidation;

namespace Lume.Application.Tables.Commands.UpdateTable;

public class UpdateTableCommandValidator : AbstractValidator<UpdateTableCommand>
{
    public UpdateTableCommandValidator()
    {
        RuleFor(dto => dto.Number)
            .NotEmpty();

        RuleFor(dto => dto.Seats)
            .NotEmpty();
    }
}
