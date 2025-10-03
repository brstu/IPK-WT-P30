namespace TaskAPI.Models.V2;

/// <summary>
/// Модель элемента для API v2.0 с дополнительными полями
/// </summary>
public class ItemDto
{
    /// <summary>
    /// Уникальный идентификатор элемента
    /// </summary>
    /// <example>1</example>
    public int Id { get; set; }
    
    /// <summary>
    /// Название элемента
    /// </summary>
    /// <example>Пример элемента v2</example>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Подробное описание элемента
    /// </summary>
    /// <example>Это расширенный тестовый элемент для демонстрации API v2</example>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Категория элемента
    /// </summary>
    /// <example>Electronics</example>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// Цена элемента
    /// </summary>
    /// <example>99.99</example>
    public decimal Price { get; set; }
    
    /// <summary>
    /// Теги для классификации
    /// </summary>
    public List<string> Tags { get; set; } = new();
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Дата последнего обновления
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}