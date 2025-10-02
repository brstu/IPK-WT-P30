using System.ComponentModel.DataAnnotations;

namespace Task06.API.Models.V2;

public class ItemDto
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Range(0.01, 10000)]
    public decimal Price { get; set; }
    
    // Новые поля в v2
    [StringLength(50)]
    public string? Category { get; set; }
    
    [Range(0, 5)]
    public double? Rating { get; set; }
    
    public List<string> Tags { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateItemDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Range(0.01, 10000)]
    public decimal Price { get; set; }
    
    // Новые поля в v2
    [StringLength(50)]
    public string? Category { get; set; }
    
    [Range(0, 5)]
    public double? Rating { get; set; }
    
    public List<string> Tags { get; set; } = new();
}

public class UpdateItemDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [Range(0.01, 10000)]
    public decimal Price { get; set; }
    
    // Новые поля в v2
    [StringLength(50)]
    public string? Category { get; set; }
    
    [Range(0, 5)]
    public double? Rating { get; set; }
    
    public List<string> Tags { get; set; } = new();
}