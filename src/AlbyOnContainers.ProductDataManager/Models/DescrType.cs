namespace AlbyOnContainers.ProductDataManager.Models;

using Abstract;

public class DescrType : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public ICollection<DescrValue> DescrValues { get; set; } = new HashSet<DescrValue>();
    public ICollection<CategoryDescrType> CategoryDescrTypes { get; set; } = new HashSet<CategoryDescrType>();
}