namespace AlbyOnContainers.ProductDataManager.Models;

using Abstract;

public class CategoryDescrType: Auditable
{
    public Guid CategoryId { get; set; }
    public Category Category { get; set; }
    
    public Guid DescrTypeId { get; set; }
    public DescrType DescrType { get; set; }
}