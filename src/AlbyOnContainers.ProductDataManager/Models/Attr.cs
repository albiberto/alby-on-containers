namespace AlbyOnContainers.ProductDataManager.Models;

using System;
using Abstract;

public class Attr : Entity
{
    public string Value { get; set; }
    
    public Guid ProductId { get; set; }
    public Product Product { get; set; }
    
    public Guid AttrTypeId { get; set; }
    public AttrType AttrType { get; set; }

}