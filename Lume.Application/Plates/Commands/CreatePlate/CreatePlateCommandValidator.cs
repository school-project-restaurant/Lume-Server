using FluentValidation;

namespace Lume.Application.Plates.Commands.CreatePlate;

public class CreatePlateCommandValidator : AbstractValidator<CreatePlateCommand>
{
    public CreatePlateCommandValidator()
    {
        RuleFor(dto => dto.Price)
            .NotEmpty();

        RuleFor(dto => dto.Calories)
            .NotEmpty();

        RuleFor(dto => dto.Ingredients)
            .NotEmpty();
    }
}
