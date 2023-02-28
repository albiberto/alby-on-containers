namespace AlbyOnContainers.ProductDataManager.Pages.CategoriesComponent;

using System.Linq;
using System.Threading.Tasks;
using Models;
using Microsoft.EntityFrameworkCore;
using Radzen;

public partial class Categories
{
    protected override void OnInitialized() => Elements = Context.Categories
        .Include(c => c.Categories)
        .Where(e => e.ParentId == null);

    void LoadChildData(DataGridLoadChildDataEventArgs<Category> args)
    {
        foreach (var category in args.Item.Categories)
        {
            var children = Context.Categories.Where(e => e.ParentId == category.Id).ToList();
            category.Categories = children;

        }

        args.Data = args.Item.Categories;
    }
    
    void RowRender(RowRenderEventArgs<Category> args) => args.Expandable = args.Data.Categories.Any();
    
    async Task InsertRowAsync(Category category)
    {
        ToInsert = new();
        category.Categories.Add(ToInsert);

        await Grid.Reload();
        StateHasChanged();
    }
}