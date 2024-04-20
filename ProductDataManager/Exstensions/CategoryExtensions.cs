using ProductDataManager.Components.Pages;
using ProductDataManager.Infrastructure.Domain;

namespace ProductDataManager.Exstensions;

public static class CategoryExtensions
{
    public static Categories.Data ConvertToData(this Category parent)
    {
        var data = new Categories.Data(parent);

        foreach (var child in parent.Categories) data.Items.Add(ConvertToData(child));

        return data;
    }
    
    public static void AddToForest(this HashSet<Categories.Data> forest, Category category)
    {
        if (category.ParentId is null)
        {
            forest.Add(new(category));
        }
        else
        {
            forest.AddToTree(category);
        }
    }

    static void AddToTree(this HashSet<Categories.Data> tree, Category category)
    {
        foreach (var node in tree)
        {
            if (node.Id == category.ParentId)
            {
                node.Items.Add(new(category));
                return;
            }

            if (node.HasChild) node.Items.AddToTree(category);
        }
    }
    
    public static void RemoveFromForest(this HashSet<Categories.Data> forest, Category category)
    {
        var nodeToRemove = forest.FirstOrDefault(n => n.Id == category.Id);
        if (nodeToRemove != null)
        {
            forest.Remove(nodeToRemove);
        }
        else
        {
            forest.RemoveFromTree(category);
        }
    }

    static void RemoveFromTree(this HashSet<Categories.Data> tree, Category category)
    {
        foreach (var node in tree)
        {
            var childToRemove = node.Items.FirstOrDefault(n => n.Id == category.Id);
            if (childToRemove != null)
            {
                node.Items.Remove(childToRemove);
                return;
            }

            if (node.HasChild) node.Items.RemoveFromTree(category);
        }
    }
}