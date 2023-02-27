namespace AlbyOnContainers.ProductDataManager.Models;

using Abstract;

public class Descr : Entity
{
    public Descr()
    {
    }
    
    public Descr(Guid productId, Guid valueId, Guid typeId)
    {
        ProductId = productId;
        DescrValueId = valueId;
        DescrTypeId = typeId;
    }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    
    public Guid DescrValueId { get; set; }
    public DescrValue DescrValue { get; set; }
    
    public Guid DescrTypeId { get; set; }
    public DescrType DescrType { get; set; }
}