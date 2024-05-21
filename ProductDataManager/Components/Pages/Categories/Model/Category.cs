using ProductDataManager.Domain.Aggregates.CategoryAggregate;

namespace ProductDataManager.Components.Pages.Categories.Model;

    public class Data(Category category)
    {
        public Guid? Id { get; } = category.Id;
        public Category Category { get; } = category;

        public string Name { get; set; } = category.Name;
        public string Description { get; set; } = category.Description;
        public Guid? ParentId { get; set; } = category.ParentId;
        
        public string ParentName { get; set; } = category.Parent?.Name ?? string.Empty;
        
        public bool NameIsDirty => !string.Equals(Category.Name, Name, StringComparison.InvariantCulture);
        public bool DescriptionIsDirty => !string.Equals(Category.Description, Description, StringComparison.InvariantCulture);
        public bool ParentIdIsDirty => Category.ParentId != ParentId;
        public bool IsDirty => NameIsDirty || DescriptionIsDirty || ParentIdIsDirty;
        
        public void CleanName() => Name = Category.Name;
        public void CleanDescription() => Description = Category.Description;
        public void CleanParentId() => ParentId = Category.ParentId;

        public HashSet<Data> Items { get; } = [];
        
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
        
        // TODO: Fix this
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