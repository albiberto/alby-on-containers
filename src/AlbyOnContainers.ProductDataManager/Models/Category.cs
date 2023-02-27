namespace AlbyOnContainers.ProductDataManager.Models;

using System;
using System.Collections.Generic;
using Abstract;

public class Category : Entity
{
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Guid? ParentId { get; set; }
    public Category Parent { get; set; }

    public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    public ICollection<Category> Categories { get; set; } = new HashSet<Category>();
    public ICollection<CategoryDescrType> CategoryDescrTypes { get; set; } = new HashSet<CategoryDescrType>();
    public ICollection<CategoryAttrType> CategoryAttrTypes { get; set; } = new HashSet<CategoryAttrType>();
}