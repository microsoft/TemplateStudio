using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core
{
    public class Naming
    {
        private static readonly string[] ReservedNames = new string[] { "WebView", "Page", "BackgroundTask" };

        private const string ValidationPattern = @"^([a-zA-Z])([\w\-])*$";
        private const string InferInvalidPattern = @"[^a-zA-Z\d_\-]";

        public static string Infer(IEnumerable<string> existing, string suggestedName)
        {
            suggestedName = Regex.Replace(suggestedName, InferInvalidPattern, string.Empty);

            if (!Exist(existing, suggestedName))
            {
                return suggestedName;
            }

            for (int i = 1; i < 1000; i++)
            {
                var newName = $"{suggestedName}{i}";

                if (!Exist(existing, newName))
                {
                    return newName;
                }
            }

            throw new Exception("Unable to infer a name. Too much iterations");
        }

        public static ValidationResult Validate(IEnumerable<string> existing, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.Empty
                };
            }
            if (existing.Contains(value))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.AlreadyExists
                };
            }
            var m = Regex.Match(value, ValidationPattern);
            if (!m.Success)
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.BadFormat
                };
            }

            return new ValidationResult
            {
                IsValid = true,
                ErrorType = ValidationErrorType.None
            };
        }

        private static bool Exist(IEnumerable<string> existing, string suggestedName)
        {
            return ReservedNames.Contains(suggestedName) || existing.Contains(suggestedName);
        }
    }

    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public ValidationErrorType ErrorType { get; set; }
    }

    public enum ValidationErrorType
    {
        None,
        Empty,
        AlreadyExists,
        BadFormat
    }
}
