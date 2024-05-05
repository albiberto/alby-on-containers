namespace ProductDataManager.Components.Pages.Descriptions.Model;

public record CategoryModel(Guid? Id, Guid CategoryId, string CategoryName)
{
    
    public void UpdateJoinId(Guid id) => Id = id;
    public Guid? Id { get; private set; } = Id;
};
