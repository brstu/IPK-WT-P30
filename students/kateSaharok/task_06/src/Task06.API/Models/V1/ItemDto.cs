using System.ComponentModel.DataAnnotations;

namespace Task06.API.Models.V1;

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
    
    public DateTime CreatedAt { get; set; }
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
}