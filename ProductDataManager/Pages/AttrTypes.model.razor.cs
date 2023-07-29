namespace ProductDataManager.Pages;

using Infrastructure.Domain;

public partial class AttrTypes
{
    public class Model(KeyValuePair<AttrType, IEnumerable<Category>>? join = default)
    {
        public string Name { get; set; } = join.HasValue ? join.Value.Key.Name : string.Empty;
        public string Description { get; set; } = join.HasValue ? join.Value.Key.Description : string.Empty;

        public IEnumerable<Category> Categories { get; set; } = join.HasValue ? join.Value.Value : Enumerable.Empty<Category>();

        public IEnumerable<CategoryAttrType> Build()
        {
            return Categories.Select(category => new CategoryAttrType
            {
                CategoryId = category.Id,
                Category = category,
                Created = DateTime.UtcNow,
                CreatedBy = "Alberto",
                LastModified = DateTime.UtcNow,
                LastModifiedBy = "Alberto",
                Type = new()
                {
                    Id = join.HasValue ? join.Value.Key.Id : Guid.Empty,
                    Name = Name,
                    Description = Description,
                    Created = DateTime.UtcNow,
                    CreatedBy = "Alberto",
                    LastModified = DateTime.UtcNow,
                    LastModifiedBy = "Alberto"
                }
            });
        }
    }
}