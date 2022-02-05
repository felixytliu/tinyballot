using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TinyBallot.Extensions;

public static class ValidationUtilities
{
    public static string ValidationErrorLog(this ModelStateDictionary ms)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var v in ms.Values)
        {
            sb.Append($"Value: {v.AttemptedValue}\n");
            foreach (var e in v.Errors)
            {
                sb.Append($"  {e.ErrorMessage}\n");
            }
        }
        return sb.ToString();
    }
}
