namespace AlbyOnContainers.ProductDataManager.Models;

using Abstract;

public class Descr : Entity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    
    public Guid DescrValueId { get; set; }
    public DescrValue DescrValue { get; set; }
}