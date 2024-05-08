using ProductDataManager.Components.Pages.Categories.Model;
using ProductDataManager.Domain.Aggregates.CategoryAggregate;

namespace ProductDataManager.Exstensions;

public static class CategoryExtensions
{
    public static Data ConvertToData(this Category category, Func<Data, bool> selector)
    {
        var data = new Data(category);
        var children = category.Categories
            .Select(child => ConvertToData(child, selector))
            .Where(selector);
        
        foreach (var child in children) data.Items.Add(child);

        return data;
    }
}