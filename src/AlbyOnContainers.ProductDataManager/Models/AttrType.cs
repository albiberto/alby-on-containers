namespace AlbyOnContainers.ProductDataManager.Models;

using System.Collections.Generic;
using Abstract;

public class AttrType : Entity
{
    public ICollection<Attr> Attrs { get; set; } = new HashSet<Attr>();
    public ICollection<CategoryAttrType> CategoryAttrTypes { get; set; } = new HashSet<CategoryAttrType>();
}
