namespace TaskAPI.Models.V1;

/// <summary>
/// Модель элемента для API v1.0
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
    /// <example>Пример элемента</example>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Описание элемента
    /// </summary>
    /// <example>Это тестовый элемент для демонстрации API</example>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; set; }
}