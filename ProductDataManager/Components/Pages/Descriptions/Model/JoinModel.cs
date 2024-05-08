using System.Text.Json.Serialization;

namespace ProductDataManager.Components.Pages.Descriptions.Model;

[method: JsonConstructor]
public record JoinModel(Guid? Id, Guid CategoryId, string CategoryName, bool Checked)
{
    public void Update(Guid? id = default)
    {
        Id = id;
        Checked = id is not null;
    }

    public Guid? Id { get; private set; } = Id;
    public bool Checked { get; private set; } = Checked;
}