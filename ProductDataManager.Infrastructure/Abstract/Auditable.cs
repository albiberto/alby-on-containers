namespace ProductDataManager.Infrastructure.Abstract;

using System.ComponentModel.DataAnnotations;

public abstract class Auditable
{
    [Required]
    [ConcurrencyCheck]
    public DateTime Created { get; set; }
    
    [Required]
    [ConcurrencyCheck]
    public string CreatedBy { get; set; }

    [Required]
    [ConcurrencyCheck]
    public DateTime LastModified  { get; set; }
    
    [Required]
    [ConcurrencyCheck]
    public string LastModifiedBy  { get; set; }
}