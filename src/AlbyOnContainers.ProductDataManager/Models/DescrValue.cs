namespace AlbyOnContainers.ProductDataManager.Models;

using Abstract;

public class DescrValue : Entity
{
    public Guid DescrTypeId { get; set; }
    public DescrType DescrType { get; set; }
    
    public ICollection<Descr> Descrs { get; set; } = new HashSet<Descr>();
}