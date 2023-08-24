// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core.Naming
{
    public class NamingService
    {
        private const string InferInvalidPattern = @"[^\d\w\-]";

        public static string Infer(string suggestedName, IEnumerable<Validator> validators, string inferWith = "")
        {
            suggestedName = Regex.Replace(ToTitleCase(suggestedName), InferInvalidPattern, string.Empty);

            if (validators.All(v => v.Validate(suggestedName).IsValid))
            {
                return suggestedName;
            }

            for (int i = 1; i < 1000; i++)
            {
                var newName = $"{suggestedName}{inferWith}{i}";

                if (validators.All(v => v.Validate(newName).IsValid))
                {
                    return newName;
                }
            }

            throw new Exception(Resources.NamingInferMessage);
        }

        public static ValidationResult Validate(string value, IEnumerable<Validator> validators)
        {
            var result = new ValidationResult();

            foreach (var validator in validators)
            {
                var validationResult = validator.Validate(value);
                if (validationResult.IsValid == false)
                {
                    result.IsValid = false;
                    result.Errors.AddRange(validationResult.Errors);
                }
            }

            return result;
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
                result.Append(string.Concat(char.ToUpperInvariant(chunk[0]), chunk.Substring(1), string.Empty));
            }

            return result.ToString();
        }
    }
}
