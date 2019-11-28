// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Naming;
using Microsoft.Templates.UI.Threading;
using Newtonsoft.Json;

namespace Microsoft.Templates.UI.Services
{
    public static class ValidationService
    {
        private static Func<IEnumerable<string>> _getNames;
        private static Func<IEnumerable<string>> _getPageNames;
        private static ItemNameService _itemValidationService;

        public static void Initialize(Func<IEnumerable<string>> getNames, Func<IEnumerable<string>> getPageNames)
        {
            _getNames = getNames;
            _getPageNames = getPageNames;
            _itemValidationService = new ItemNameService(GenContext.ToolBox.Repo.ItemNameValidationConfig, _getNames);
        }

        public static ValidationResult ValidateTemplateName(string templateName)
        {
           return _itemValidationService.Validate(templateName);
        }

        public static string InferTemplateName(string templateName)
        {
            return _itemValidationService.Infer(templateName);
        }

        public static bool HasAllPagesViewSuffix(bool fromNewTemplate, string newName)
        {
            var names = _getPageNames.Invoke();
            if (!names.Any())
            {
                return false;
            }

            foreach (var name in names)
            {
                if (!name.EndsWith("view", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }

            if (fromNewTemplate && !newName.EndsWith("view", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
