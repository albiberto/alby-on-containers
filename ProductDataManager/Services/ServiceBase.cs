namespace ProductDataManager.Services;

using System.Linq.Dynamic.Core;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using Radzen;

public abstract class ServiceBase
{
    protected ProductContext Context;
    protected NavigationManager Navigation;

    protected ServiceBase(ProductContext context, NavigationManager navigation)
    {
        Context = context;
        Navigation = navigation;
    }

    protected static void ApplyQuery<T>(ref IQueryable<T> items, Query? query = default)
    {
        if (query == null) return;
        
        if (!string.IsNullOrEmpty(query.Filter))
            items = query.FilterParameters != null
                ? items.Where(query.Filter, query.FilterParameters)
                : items.Where(query.Filter);

        if (!string.IsNullOrEmpty(query.OrderBy)) items = items.OrderBy(query.OrderBy);
        if (query.Skip.HasValue) items = Queryable.Skip(items, query.Skip.Value);
        if (query.Top.HasValue) items = Queryable.Take(items, query.Top.Value);
    }
}