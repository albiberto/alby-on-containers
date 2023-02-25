namespace AlbyOnContainers.ProductDataManager.Models;

using System;
using System.ComponentModel.DataAnnotations;
using Abstract;

public class Attr : Entity
{
    [ConcurrencyCheck]
    public Guid ProductId { get; set; }
    public Product Product { get; set; }

    [ConcurrencyCheck]
    public Guid AttrTypeId { get; set; }
    public AttrType AttrType { get; set; }

}