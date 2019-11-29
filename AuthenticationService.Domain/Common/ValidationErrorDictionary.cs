using AuthenticationService.Domain.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AuthenticationService.Domain.Common
{
    public class ValidationErrorDictionary : Dictionary<string, object>
    {
        public ValidationErrorDictionary()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public ValidationErrorDictionary(ModelStateDictionary modelState)
            : this()
        {
            foreach (var keyModelStatePair in modelState)
            {
                var key = keyModelStatePair.Key;
                key = key.ToCamelCaseFromPascalCase();

                var errors = keyModelStatePair.Value.Errors;
                if (errors != null && errors.Count > 0)
                {
                    var errorMessages = errors.Select(error => error.ErrorMessage).ToArray();
                    Add(key, errorMessages);
                }
            }
        }
    }
}
