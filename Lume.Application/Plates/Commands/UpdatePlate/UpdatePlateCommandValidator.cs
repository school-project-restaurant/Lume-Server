using FluentValidation;

namespace Lume.Application.Plates.Commands.UpdatePlate;

public class UpdatePlateCommandValidator : AbstractValidator<UpdatePlateCommand>
{
    public UpdatePlateCommandValidator()
    {
        RuleFor(dto => dto.Price)
            .NotEmpty();

        RuleFor(dto => dto.Calories)
            .NotEmpty();

        RuleFor(dto => dto.Ingredients)
            .NotEmpty();
    }
}
