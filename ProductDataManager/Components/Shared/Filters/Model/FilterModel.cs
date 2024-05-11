using ProductDataManager.Components.Pages.Descriptions.Model;
using ProductDataManager.Components.Shared.Model;

namespace ProductDataManager.Components.Shared.Filters.Model;

public record FilterModel(Value Value, bool Checked = false)
{
    public string Tooltip => Value.GetTooltip();
    public string OutlinedIcon => Value.GetOutlinedIcon();
    public string FilledIcon => Value.GetFilledIcon();
    
    public bool Checked { get; private set; } = Checked;
    
    public void Set(bool @checked) => Checked = @checked;
}