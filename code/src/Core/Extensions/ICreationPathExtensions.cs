// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.Templates;

namespace Microsoft.Templates.Core.Gen
{
    public static class ICreationPathExtensions
    {
        public static string GetOutputPath(this ICreationPath cp, Dictionary<string, string> parameters)
        {
            var parameterReplacements = new FileRenameParameterReplacements(parameters);

            var newPath = parameterReplacements.ReplaceInPath(cp.Path);

            return newPath;
        }
    }
}
