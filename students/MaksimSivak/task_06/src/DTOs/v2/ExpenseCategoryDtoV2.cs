using System.ComponentModel.DataAnnotations;

namespace FinancialAccounting.API.DTOs.v2;

public class ExpenseCategoryDtoV2
{
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public decimal MonthlyBudget { get; set; }
    public string ColorCode { get; set; } = "#000000";
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}