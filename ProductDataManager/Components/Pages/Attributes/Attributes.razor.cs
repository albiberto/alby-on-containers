namespace ProductDataManager.Components.Pages.Attributes;

using Domain.Aggregates.AttributeAggregate;
using DynamicData;
using FluentValidation;
using Infrastructure;
using Microsoft.AspNetCore.Components;

public class AttributeValidator : AbstractValidator<Attribute>
{
    public AttributeValidator()
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
    }
}

public partial class Attributes : ComponentBase, IDisposable
{
    [Inject] public required ProductContext DbContext { get; set; }

    [Parameter] public required AttributeType Parent { get; set; }
    [Parameter] public required IReadOnlyObservableCollection<Model<AttributeType>> Types { get; set; }

    IReadOnlyObservableCollection<Model<Attribute>> Items { get; set; }

    protected override void OnInitialized()
    {
        var validator = new AttributeValidator();

        Items = DbContext.Changes
            .Connect(entry => entry.Entity is Attribute && entry.OriginalValues[nameof(Attribute.TypeId)] is Guid id && id == Parent.Id)
            .Transform(entry => new Model<Attribute>(entry, validator), (model, entry) => model.Update(entry))
            .ToObservableCollection();
    }

    void Add()
    {
        DbContext.Add(new Attribute
        {
            Name = "",
            Description = "",
            TypeId = Parent.Id
        });
    }
    
    public void Dispose()
    {
        Items.Dispose();
    }
}
