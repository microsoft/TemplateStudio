// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Templates.Core.Naming;
using Xunit;

namespace Microsoft.Templates.Core.Test
{
    [Collection("Unit Test Templates")]
    [Trait("Group", "Minimum")]
    [Trait("Type", "Naming")]
    public class ProjectNameServiceTests
    {
        [Fact]
        public void Infer_SuccessfullyAccountsForReservedNames()
        {
            var config = new ProjectNameValidationConfig()
            {
                ReservedNames = new string[]
                {
                    "Prism",
                    "CaliburnMicro",
                    "MVVMLight",
                },
            };

            var validationService = new ProjectNameService(config, null);
            var result = validationService.Infer("Prism");

            Assert.Equal("Prism1", result);
        }

        [Fact]
        public void Infer_SuccessfullyAccountsForExistingNames()
        {
            var existingNames = new List<string>() { "App" };

            Func<IEnumerable<string>> getExistingNames = () => { return existingNames; };

            var config = new ProjectNameValidationConfig()
            {
                ValidateExistingNames = true,
            };

            var validationService = new ProjectNameService(config, getExistingNames);
            var result = validationService.Infer("App");

            Assert.Equal("App1", result);
        }

        [Fact]
        public void Validate_SuccessfullyAccountsForRegex()
        {
            var config = new ProjectNameValidationConfig()
            {
                Regexs = new RegExConfig[]
                {
                    new RegExConfig()
                    {
                        Name = "projectStartWith$",
                        Pattern = "^[^\\$]",
                    },
                },
            };

            var validationService = new ProjectNameService(config, null);
            var result = validationService.Validate("$App");

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.ErrorType == ValidationErrorType.Regex);
            Assert.Contains(result.Errors, e => e.ValidatorName == "projectStartWith$");
        }
    }
}
