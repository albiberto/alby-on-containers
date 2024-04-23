﻿using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using MudBlazor;
using ProductDataManager.Infrastructure.Domain;

namespace ProductDataManager.Components.Pages;

#nullable enable

public partial class Categories
{
    public class Data(Category category)
    {
        public Guid? Id { get; } = category.Id;

        public string Name { get; set; } = category.Name ?? string.Empty;
        public string Description { get; set; } = category.Description ?? string.Empty;
        public Guid? ParentId { get; set; } = category.ParentId;

        public HashSet<Data> Items { get; set; } = new();
        public bool HasChild => Items.Any();
        
        public bool IsExpanded { get; set; } = true;
        
        public int Total => Go(this);
        
        static int Go(Data data)
        {
            if (!data.HasChild) return 1;

            int total = 0;
            foreach (var child in data.Items) total += Go(child);

            return total;
        }
        
        public HashSet<Guid?> LeafIds => GetLeafIds(this).Append(Id).ToHashSet();
        
        private static IEnumerable<Guid?> GetLeafIds(Data data)
        {
            if (!data.HasChild) yield return data.Id;

            foreach (var child in data.Items)
            {
                foreach (var id in GetLeafIds(child))
                {
                    yield return id;
                }
            }
        }
    }

    public record Parent(Guid? Id, string Name);
}