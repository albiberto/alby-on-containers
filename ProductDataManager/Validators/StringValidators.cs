using FluentValidation;

namespace ProductDataManager.Validators;

public static class StringValidators
{
    public static ValueValidator<string> Value(int min = 1, int max = 100) => new(rule => rule
        .NotEmpty()
        .WithMessage("Value is required")
        .Length(min, max)
        .WithMessage($"Value must be between {min} and {max} characters"));
    
    public static ValueValidator<string> Name(int min = 3, int max = 30) => new(rule => rule
        .NotEmpty()
        .WithMessage("Name is required")
        .Length(min, max)
        .WithMessage($"Name must be between {min} and {max} characters"));
    
    public static ValueValidator<string> Description(int max = 100) => new(rule => rule
        .Length(0, max)
        .WithMessage($"Description must be less than {max} characters"));
}