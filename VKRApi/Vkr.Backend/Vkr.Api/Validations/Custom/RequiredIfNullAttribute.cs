using System.ComponentModel.DataAnnotations;

namespace Vkr.API.Validations.Custom;

public class RequiredIfNullAttribute(string otherProperty) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var otherPropertyInfo = validationContext.ObjectType.GetProperty(otherProperty);
        var otherValue = otherPropertyInfo?.GetValue(validationContext.ObjectInstance);

        if (value == null && otherValue == null)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}