using FluentValidation;
using FinancialAccounting.API.DTOs.v2;

namespace FinancialAccounting.API.Validators;

public class ExpenseCategoryValidator : AbstractValidator<ExpenseCategoryDtoV2>
{
    public ExpenseCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Название категории обязательно")
            .MaximumLength(100).WithMessage("Название не должно превышать 100 символов");
            
        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Описание не должно превышать 500 символов");
            
        RuleFor(x => x.MonthlyBudget)
            .GreaterThanOrEqualTo(0).WithMessage("Бюджет не может быть отрицательным");
            
        RuleFor(x => x.ColorCode)
            .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("Неверный формат цвета (должен быть #RRGGBB)");
    }
}