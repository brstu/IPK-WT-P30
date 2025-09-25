using System.ComponentModel.DataAnnotations;

namespace FinancialAccounting.API.DTOs.v1;

public class ExpenseCategoryDtoV1
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public DateTime CreatedAt { get; set; }
}
