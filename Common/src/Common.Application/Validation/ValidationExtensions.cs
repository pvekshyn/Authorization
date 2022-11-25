using FluentValidation;
using static Common.Application.Validation.Constants;

namespace Common.Application.Validation;
public static class ValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> WithDependency<T, TProperty>(this IRuleBuilderOptions<T, TProperty> rule)
    {
        return rule.WithState(x => DEPENDENCY);
    }
}
