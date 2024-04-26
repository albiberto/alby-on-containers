using FluentValidation;
using ProductDataManager.Components.Shared.Dialogs;

namespace ProductDataManager.Validators;

public class CategoryValidator: AbstractValidator<CategoryDialog.Model>
{
    public CategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Deve essere specificato un nome")
            .Length(3,100)
            .WithMessage("Il nome deve essere lungo almeno 3 caratteri e non più di 100 caratteri");

        RuleFor(x => x.Description)
            .MaximumLength(100);
    }
    
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<CategoryDialog.Model>.CreateWithOptions((CategoryDialog.Model)model, x => x.IncludeProperties(propertyName)));
        
        return result.IsValid 
            ? Enumerable.Empty<string>() 
            : result.Errors.Select(e => e.ErrorMessage);
    };
}