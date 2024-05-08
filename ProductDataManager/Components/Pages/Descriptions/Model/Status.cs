using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace ProductDataManager.Components.Pages.Descriptions.Model;

public class Status(Value value)
{
    public Status():this(Value.Unchanged) { }

    Value Original { get; set; } = value;
    public Value Current { get; private set; } = value;
    
    public bool IsAdded => Current == Value.Added;
    public bool IsDeleted => Current == Value.Deleted;
    public bool IsModified => Current == Value.Modified;
    public bool IsUnchanged => Current == Value.Unchanged;

    public void Save()
    {
        Current = Value.Unchanged;
        Original = Current;
    }

    public void Clear() => Current = Original;
    public void Unchanged() => Current = Value.Unchanged;
    public void Modified() => Current = Current == Value.Added ? Value.Added : Value.Modified;
    public void Added() => Current = Value.Added;
    public void Deleted() => Current = Value.Deleted;
}

public enum Value
{
    Unchanged, Modified, Added, Deleted 
}

public static class ValueExtensions
{
    public static string GetOutlinedIcon(this Value value) => value switch
    {
        Value.Unchanged => Icons.Material.Outlined.Save,
        Value.Added => Icons.Material.Outlined.AddCircleOutline,
        Value.Modified => Icons.Material.Outlined.Edit,
        Value.Deleted => Icons.Material.Outlined.Delete,
        _ => throw new InvalidCastException("Invalid status")
    };
    
    public static string GetFilledIcon(this Value value) => value switch
    {
        Value.Unchanged => Icons.Material.Filled.Save,
        Value.Added => Icons.Material.Filled.AddCircle,
        Value.Modified => Icons.Material.Filled.Edit,
        Value.Deleted => Icons.Material.Filled.Delete,
        _ => throw new InvalidCastException("Invalid status")
    };
    
    public static string GetTooltip(this Value value) => value switch
    {
        Value.Unchanged => "Persisted",
        Value.Added => "Pending Insertion",
        Value.Modified => "Pending Update",
        Value.Deleted => "Pending Deletion",
        _ => throw new InvalidCastException("Invalid status")
    };
    
    public static Value Map(EntityState status) => status switch
    {
        EntityState.Unchanged => Value.Unchanged,
        EntityState.Added => Value.Added,
        EntityState.Modified => Value.Modified,
        EntityState.Deleted => Value.Deleted,
        _ => throw new InvalidCastException("Invalid status")
    };
}
