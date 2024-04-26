using FluentValidation;

namespace ProductDataManager.Validators;

public class ValueValidator<T> : AbstractValidator<T>
{
    public ValueValidator(Action<IRuleBuilderInitial<T, T>> rule) => rule(RuleFor(x => x));

    IEnumerable<string> ValidateValue(T value)
    {
        var result = Validate(value);
        
        return result.IsValid 
            ? Enumerable.Empty<string>() 
            : result.Errors.Select(e => e.ErrorMessage);
    }

    public Func<T, IEnumerable<string>> Validation => ValidateValue;
}