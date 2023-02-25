namespace AlbyOnContainers.ProductDataManager.Extensions;

using Radzen;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Models;

public static class QueryableExtensions
{
    public static async Task<(ICollection<T> Entities, int Count)> LoadDataAsync<T>(this IQueryable<T> query, LoadDataArgs args, Func<string, Expression<Func<Category, bool>>> selector = default)
    {
        if (!string.IsNullOrEmpty(args.Filter)) query = selector is null ? query.Where(args.Filter) : query.Where(selector(args.Filter));
        if (!string.IsNullOrEmpty(args.OrderBy)) query = query.OrderBy(args.OrderBy);

        var count = await query.CountAsync();
        
        if (args.Skip is not null) query = query.Skip(args.Skip.Value);
        if (args.Top is not null) query = query.Take(args.Top.Value);

        var entities = await query.ToListAsync();
        return (entities, count);
    }
}