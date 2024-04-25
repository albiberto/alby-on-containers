using System.ComponentModel.DataAnnotations;

namespace ProductDataManager.Domain.SeedWork;

public record Auditable
{
    [Required]
    public DateTime CreatedAt { get; set; }
    
    [Required]
    public string CreatedBy { get; set; } = null!;

    [Required]
    public DateTime UpdatedAt  { get; set; }
    
    [Required]
    public string UpdatedBy  { get; set; } = null!;
}