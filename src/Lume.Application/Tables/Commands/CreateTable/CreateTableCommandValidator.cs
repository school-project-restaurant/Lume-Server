using FluentValidation;

namespace Lume.Application.Tables.Commands.CreateTable;

public class CreateTableCommandValidator : AbstractValidator<CreateTableCommand>
{
    public CreateTableCommandValidator()
    {
        RuleFor(dto => dto.Number)
            .NotEmpty();

        RuleFor(dto => dto.Seats)
            .NotEmpty();
    }
}
