namespace ProductDataManager.Components.Pages.Attributes;

using System.Linq.Expressions;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Domain.Aggregates.AttributeAggregate;
using DynamicData;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MudBlazor;
using ReactiveUI;
using Shared;
using Shared.Dialogs;
using Severity = MudBlazor.Severity;

public class AttributeTypeValidator : AbstractValidator<AttributeType>
{
    public AttributeTypeValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("{PropertyName} is required")
            .Length(3, 30)
            .WithMessage("{PropertyName} must be between {MinLength} and {MaxLength} characters");

        RuleFor(x => x.Description)
            .NotNull()
            .Length(0, 100)
            .WithMessage("{PropertyName} must be less than {MaxLength} characters");

        RuleForEach(x => x.Attributes)
            .SetValidator(new AttributeValidator());
    }
}

public class Model<T>(EntityEntry entry, AbstractValidator<T> validator)
    where T : class
{
    ValidationResult validationResult = validator.Validate((T)entry.Entity);

    public EntityState State 
        => entry.State;
    
    public T Entity
        => (T)entry.Entity;

    public void Clear() 
        => entry.DiscardChanges();
    
    public void Delete()
        => entry.Context.Remove(entry.Entity);
    
    public bool IsValid 
        => validationResult.IsValid;

    bool showErrors;

    public Model<T> Update(EntityEntry next)
    {
        entry = next;
        validationResult = validator.Validate(Entity);

        showErrors = true;
        
        return this;
    }

    public ValidationFailure? Validation(Expression<Func<T, object>> property)
    {
        if (!showErrors)
        {
            return null;
        }
        
        var member = property.Body as MemberExpression;
        var propertyName = member?.Member.Name ?? string.Empty;

        return validationResult.Errors.FirstOrDefault(error => error.PropertyName == propertyName);
    }
}

public sealed partial class AttributeTypes : ComponentBase, IDisposable
{
    [Inject] public required ProductContext DbContext { get; set; }
    [Inject] public required IDialogService DialogService { get; set; }
    [Inject] public required ISnackbar Snackbar { get; set; }
    [Inject] public required ILogger<Attributes> Logger { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }

    IDisposable? registration;

    IReadOnlyObservableCollection<Model<AttributeType>> Items { get; set; }

    IReactiveProperty<bool> DisableSave { get; set; }

    IReactiveProperty<bool> DisableClearAll { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _ = await DbContext.AttributeTypes.Include(type => type.Attributes).ToListAsync();

        var validator = new AttributeTypeValidator();

        var entries = DbContext.Changes
            .Connect(entry => entry.Entity is AttributeType)
            .Transform(entry => new Model<AttributeType>(entry, validator), (model, entry) => model.Update(entry))
            .SortBy(entry => entry.Entity.Name);

        Items = entries.ToObservableCollection();

        DisableClearAll = DbContext.HasChanges
            .Select(value => !value)
            .ToProperty(true);

        DisableSave = entries
            .ToCollection()
            .CombineLatest(DbContext.HasChanges, (models, hasChanges) => !hasChanges || models.Any(model => !model.IsValid))
            .ToProperty(true);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            registration = Navigation.RegisterLocationChangingHandler(OnLocationChanging);
        }
    }

    async ValueTask OnLocationChanging(LocationChangingContext context)
    {
        if (!DbContext.HasChanges.Value)
        {
            return;
        }

        var dialog = await DialogService.ShowAsync<NavigationDialog>("Leave page?", Constants.DialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            DbContext.ChangeTracker.Clear();
            return;
        }

        context.PreventNavigation();
    }

    void Add()
        => DbContext.AttributeTypes.Add(new AttributeType
        {
            Name = "",
            Description = ""
        });
    
    void ClearAll()
        => DbContext.DiscardChanges();

    async Task SaveAsync()
    {
        try
        {
            await DbContext.SaveChangesAsync();
            Snackbar.Add("Changes Saved!", Severity.Success);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error while saving attribute type");
            Snackbar.Add("Error while saving attribute type", Severity.Error);
        }
    }

    public void Dispose()
    {
        Snackbar.Dispose();
        
        Items.Dispose();
        DisableSave.Dispose();
        DisableClearAll.Dispose();
        
        registration?.Dispose();
    }
}
