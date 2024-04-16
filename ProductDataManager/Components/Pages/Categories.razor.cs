using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using ProductDataManager.Infrastructure;
using ProductDataManager.Infrastructure.Domain;


namespace ProductDataManager.Components.Pages;

public partial class Categories
{
    [Inject] private ProductContext Context { get; set; } = null!;
    
    private List<Category> Elements = [];

    

    protected override async Task OnInitializedAsync()
    {
        Elements = await Context.Categories
            .Where(c => c.ParentId == null)
            .Include(c => c.Categories)
            .ThenInclude(c => c.Categories)
            .ToListAsync();
    }
}