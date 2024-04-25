﻿using FluentValidation;
using ProductDataManager.Components.Shared;

namespace ProductDataManager.Components.Validators;

public class DescriptionValidator: AbstractValidator<DescriptionDialog.Model>
{
    public DescriptionValidator()
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
        var result = await ValidateAsync(ValidationContext<DescriptionDialog.Model>.CreateWithOptions((DescriptionDialog.Model)model, x => x.IncludeProperties(propertyName)));
        
        return result.IsValid 
            ? Enumerable.Empty<string>() 
            : result.Errors.Select(e => e.ErrorMessage);
    };
}