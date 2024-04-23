using ProductDataManager.Components.Pages;
using ProductDataManager.Infrastructure.Domain;

namespace ProductDataManager.Exstensions;

public static class CategoryExtensions
{
    public static Categories.Data ConvertToData(this Category? parent, Func<Categories.Data, bool> selector)
    {
        var data = new Categories.Data(parent);

        foreach (var child in parent.Categories)
        {
            var current = ConvertToData(child, selector);

            if (selector(current)) data.Items.Add(current);
        }

        return data;
    }
}