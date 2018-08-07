namespace Microsoft.Templates.Core
{
    public class PageNameValidator : Validator
    {
        private const string PageSufix = "page";
        private const string ViewSufix = "view";

        public override ValidationResult Validate(string suggestedName)
        {
            if (suggestedName.ToLower().EndsWith(PageSufix))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.EndsWithPageSufix
                };
            }
            else if (suggestedName.ToLower().EndsWith(ViewSufix))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.EndsWithViewSufix
                };
            }
            else
            {
                return new ValidationResult()
                {
                    IsValid = true,
                    ErrorType = ValidationErrorType.None
                };
            }
        }
    }
}
