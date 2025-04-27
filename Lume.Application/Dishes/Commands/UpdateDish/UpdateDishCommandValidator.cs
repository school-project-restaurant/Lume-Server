using FluentValidation;

namespace Lume.Application.Dishes.Commands.UpdateDish;

public class UpdateDishCommandValidator : AbstractValidator<UpdateDishCommand>
{
    public UpdateDishCommandValidator()
    {
        RuleFor(dto => dto.Price)
            .NotEmpty();

        RuleFor(dto => dto.Calories)
            .NotEmpty();

        RuleFor(dto => dto.Ingredients)
            .NotEmpty(); // TODO add the list of possible ingredients
    }
}
