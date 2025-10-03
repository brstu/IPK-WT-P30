namespace TaskAPI.Models;

/// <summary>
/// Параметры запроса для пагинации, фильтрации и сортировки
/// </summary>
public class ItemQueryParameters
{
    /// <summary>
    /// Номер страницы (начиная с 1)
    /// </summary>
    /// <example>1</example>
    [Range(1, int.MaxValue, ErrorMessage = "Номер страницы должен быть положительным числом")]
    public int Page { get; set; } = 1;
    
    /// <summary>
    /// Размер страницы (1-100)
    /// </summary>
    /// <example>10</example>
    [Range(1, 100, ErrorMessage = "Размер страницы должен быть от 1 до 100")]
    public int PageSize { get; set; } = 10;
    
    /// <summary>
    /// Поле для сортировки (name, id, createdAt)
    /// </summary>
    /// <example>name</example>
    public string? Sort { get; set; }
    
    /// <summary>
    /// Направление сортировки (asc, desc)
    /// </summary>
    /// <example>asc</example>
    public string? SortDirection { get; set; } = "asc";
    
    /// <summary>
    /// Фильтр по названию
    /// </summary>
    /// <example>test</example>
    public string? Filter { get; set; }
    
    /// <summary>
    /// Фильтр по категории (только для v2)
    /// </summary>
    /// <example>Electronics</example>
    public string? Category { get; set; }
}