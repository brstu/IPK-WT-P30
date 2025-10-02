namespace Task06.Api.Models;

public class ItemDtoV2
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty; // Новое поле в v2
    public DateTime CreatedAt { get; set; }
}