// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Gen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core
{
    public class Naming
    {
        private static readonly string[] ReservedNames = new string[] { "Page", "BackgroundTask", "Pivot" };

        private static readonly Lazy<string[]> _defaultNames = new Lazy<string[]>(() => GetDefaultNames());
        public static string[] DefaultNames => _defaultNames.Value;

        private static string[] GetDefaultNames()
        {
            return GenContext.ToolBox.Repo.Get(t => !t.GetItemNameEditable()).Select(n => n.GetDefaultName()).ToArray();
        }

        private const string ValidationPattern = @"^([a-zA-Z])([\w\-])*$";
        private const string InferInvalidPattern = @"[^a-zA-Z\d_\-]";

        public static string ResolveTemplateName(IEnumerable<string> existing, ITemplateInfo template)
        {
            var name = template.GetDefaultName();

            var inferMode = InferMode.ExcludeExistingAndReservedNames;

            if (template.GetItemNameEditable())
            {
                //avoid collisions with defaultnames
                inferMode = InferMode.ExcludeExistingReservedNamesAndDefaultNames;
            }

            var suggestedName = Infer(existing, name, inferMode);

            return suggestedName;
        }

        public static string Infer(IEnumerable<string> existing, string suggestedName, InferMode inferMode)
        {
            suggestedName = Regex.Replace(ToTitleCase(suggestedName), InferInvalidPattern, string.Empty);

            if (!Exist(existing, suggestedName, inferMode))
            {
                return suggestedName;
            }

            for (int i = 1; i < 1000; i++)
            {
                var newName = $"{suggestedName}{i}";

                if (!Exist(existing, newName, inferMode))
                {
                    return newName;
                }
            }

            throw new Exception("Unable to infer a name. Too much iterations");
        }



        public static ValidationResult Validate(IEnumerable<string> existing, string value, bool allowDefaultName)
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

            if (ReservedNames.Contains(value))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.ReservedName
                };
            }

            if (!allowDefaultName && DefaultNames.Contains(value))
            {
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorType = ValidationErrorType.ReservedName
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

        private static bool Exist(IEnumerable<string> existing, string suggestedName, InferMode inferMode)
        {
            switch (inferMode)
            {
                case InferMode.ExcludeExisting:
                    return existing.Contains(suggestedName);
                case InferMode.ExcludeExistingAndReservedNames:
                    return existing.Contains(suggestedName) || ReservedNames.Contains(suggestedName);
                case InferMode.ExcludeExistingReservedNamesAndDefaultNames:
                    return existing.Contains(suggestedName) || ReservedNames.Contains(suggestedName) || DefaultNames.Contains(suggestedName);
                default:
                    return existing.Contains(suggestedName); 
            }
        }
      

        private static string ToTitleCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var valueChunks = value.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (valueChunks.Length == 1)
            {
                return value;
            }

            var result = new StringBuilder();

            foreach (var chunk in valueChunks)
            {
                result.Append(string.Concat(char.ToUpper(chunk[0]), chunk.Substring(1), ""));
            }

            return result.ToString();
        }
    }


}
