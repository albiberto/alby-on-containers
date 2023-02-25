namespace AlbyOnContainers.ProductDataManager.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abstract;

public class Product : Entity
{
    [ConcurrencyCheck]
    public Guid? CategoryId { get; set; }
    public Category Category { get; set; }

    public ICollection<Attr> Attrs { get; set; } = new HashSet<Attr>();
    public ICollection<Descr> Descrs { get; set; } = new HashSet<Descr>();
}