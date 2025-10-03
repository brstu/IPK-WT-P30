using FluentValidation;
using Lab06_API.DTOs;

namespace Lab06_API.Validators
{
    public class ProductV2Validator : AbstractValidator<ProductV2>
    {
        public ProductV2Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Название продукта обязательно")
                .Length(2, 100).WithMessage("Название должно быть от 2 до 100 символов");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Цена должна быть больше 0");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("Категория обязательна");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Бренд обязателен");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Количество не может быть отрицательным");

            RuleFor(x => x.Rating)
                .InclusiveBetween(0, 5).WithMessage("Рейтинг должен быть между 0 и 5");
        }
    }
}