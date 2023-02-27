namespace AlbyOnContainers.ProductDataManager.Models;

using System;
using System.Collections.Generic;
using Abstract;

public class Product : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Guid? CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Attr> Attrs { get; set; } = new HashSet<Attr>();
    public ICollection<Descr> Descrs { get; set; } = new HashSet<Descr>();
}