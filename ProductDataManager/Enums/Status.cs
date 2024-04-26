using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace ProductDataManager.Enums;

public enum Status
{
    Unchanged,
    Added,
    Modified,
    Deleted
}

public static class EntityStatusExtensions
{
    public static string GetOutlinedIcon(this Status status) => status switch
    {
        Status.Unchanged => Icons.Material.Outlined.Save,
        Status.Added => Icons.Material.Outlined.AddCircleOutline,
        Status.Modified => Icons.Material.Outlined.Edit,
        Status.Deleted => Icons.Material.Outlined.Delete,
        _ => throw new InvalidCastException("Invalid status")

    };
    
    public static string GetFilledIcon(this Status status) => status switch
    {
        Status.Unchanged => Icons.Material.Filled.Save,
        Status.Added => Icons.Material.Filled.AddCircle,
        Status.Modified => Icons.Material.Filled.Edit,
        Status.Deleted => Icons.Material.Filled.Delete,
        _ => throw new InvalidCastException("Invalid status")
    };
    
    public static string GetTooltip(this Status status) => status switch
    {
        Status.Unchanged => "Persisted",
        Status.Added => "Pending Insertion",
        Status.Modified => "Pending Update",
        Status.Deleted => "Pending Deletion",
        _ => throw new InvalidCastException("Invalid status")
    };
    
    public static Status  Map(this EntityState status) => status switch
    {
        EntityState.Unchanged => Status.Unchanged,
        EntityState.Added => Status.Added,
        EntityState.Modified => Status.Modified,
        EntityState.Deleted => Status.Deleted,
        _ => throw new InvalidCastException("Invalid status")
    };
}