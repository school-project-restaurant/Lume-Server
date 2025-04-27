using FluentValidation;

namespace Lume.Application.Dishes.Commands.CreateDish;

public class CreateDishCommandValidator : AbstractValidator<CreateDishCommand>
{
    public CreateDishCommandValidator()
    {
        RuleFor(dto => dto.Price)
            .NotEmpty();

        RuleFor(dto => dto.Calories)
            .NotEmpty();

        RuleFor(dto => dto.Ingredients)
            .NotEmpty();
    }
}
