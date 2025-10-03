using FluentValidation;
using Task06.Api.Models;

namespace Task06.Api.Validators;

public class CreateItemRequestValidator : AbstractValidator<CreateItemRequest>
{
    public CreateItemRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(3, 100).WithMessage("Name must be between 3 and 100 characters");
            
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0")
            .LessThanOrEqualTo(10000).WithMessage("Price must be less than or equal to 10000");
    }
}