using FluentValidation;
using TaskAPI.Models;

namespace TaskAPI.Validators;

public class ItemQueryParametersValidator : AbstractValidator<ItemQueryParameters>
{
    public ItemQueryParametersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Номер страницы должен быть положительным числом");
        
        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Размер страницы должен быть от 1 до 100");
        
        RuleFor(x => x.Sort)
            .Must(BeAValidSortField).When(x => !string.IsNullOrEmpty(x.Sort))
            .WithMessage("Допустимые поля для сортировки: name, id, createdAt");
        
        RuleFor(x => x.SortDirection)
            .Must(BeAValidSortDirection).When(x => !string.IsNullOrEmpty(x.SortDirection))
            .WithMessage("Допустимые направления сортировки: asc, desc");
    }
    
    private static bool BeAValidSortField(string? sortField)
    {
        var validFields = new[] { "name", "id", "createdAt" };
        return validFields.Contains(sortField?.ToLower());
    }
    
    private static bool BeAValidSortDirection(string? direction)
    {
        var validDirections = new[] { "asc", "desc" };
        return validDirections.Contains(direction?.ToLower());
    }
}