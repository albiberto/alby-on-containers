namespace ProductDataManager.Components.Pages.Descriptions.Model;

public class Value(Guid Id, string name, string description)
{
    public Guid Id { get; } = Id;
    public string Name { get; set; } = name;
    public string OriginalName { get; } = name;
    public string Description { get; set; } = description;
    public string OriginalDescription { get; } = description;
        
    public bool IsDirty => !string.Equals(Description, Name, StringComparison.InvariantCulture) || !string.Equals(Description, Description, StringComparison.InvariantCulture);

    public void Clear()
    {
        Name = OriginalName;
        Description = OriginalDescription;
    }
}