using Microsoft.AspNetCore.Components;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Options;
using MudBlazor;
using ProductDataManager.Infrastructure.Domain;

namespace ProductDataManager.Components.Pages;

#nullable enable

public partial class Categories
{
    public class Data
    {
        public Data(Category category)
        {
            Id = category.Id;
            Name = category.Name;
            Description = category.Description;
            Created = category.Created;
            CreatedBy = category.CreatedBy;
            LastModified = category.LastModified;
            LastModifiedBy = category.LastModifiedBy;
            Items = [];
        }

        public Guid? Id { get; set; }

        public Data? Parent { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime LastModified { get; set; }
        public string LastModifiedBy { get; set; }

        public HashSet<Data> Items { get; init; } = [];

        public bool IsExpanded { get; set; } = true;
        public bool? IsChecked { get; set; } = false;
        public bool HasChild => Items.Count > 0;

        public int Total => Go(this);
        
        static int Go(Data data)
        {
            if (!data.HasChild) return 1;

            int total = 0;
            foreach (var child in data.Items)
            {
                total += Go(child);
            }

            return total;
        }
    }
}