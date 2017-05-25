using Microsoft.Templates.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.VsEmulator
{
    public class DirectoryExistsValidator : Validator<string>
    {
        public DirectoryExistsValidator(string config) : base(config)
        {

        }

        public override ValidationResult Validate(string suggestedName)
        {
            var existing = Directory.EnumerateDirectories(_config)
                                            .Select(d => new DirectoryInfo(d).Name)
                                            .ToList();

            
            if (existing.Contains(suggestedName))
            {
                return new ValidationResult()
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.AlreadyExists
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
