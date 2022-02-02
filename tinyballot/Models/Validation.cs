using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace TinyBallot.Validation;

public class NotNullOrEmptyCollectionAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
	if (value == null)
	    return false;

	var collection = value as ICollection;
        if (collection != null)
        {
            return collection.Count != 0;
        }
	
        var enumerable = value as IEnumerable;
        return enumerable != null && enumerable.GetEnumerator().MoveNext();
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
	return IsValid(value) ?
	    ValidationResult.Success :
	    new ValidationResult($"The list {validationContext.DisplayName} requires at least one member.");
    }
}
