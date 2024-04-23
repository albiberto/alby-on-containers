using ProductDataManager.Components.Pages;
using ProductDataManager.Infrastructure.Domain;

namespace ProductDataManager.Exstensions;

public static class CategoryExtensions
{
    public static Categories.Data ConvertToData(this Category category, Func<Categories.Data, bool> selector)
    {
        var data = new Categories.Data(category);
        var children = category.Categories
            .Select(child => ConvertToData(child, selector))
            .Where(selector);
        
        foreach (var child in children) data.Items.Add(child);

        return data;
    }
}