// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.Templates
{
    public class FileRenameParameterReplacements
    {
        public Dictionary<string, string> FileRenameParams { get; }

        public FileRenameParameterReplacements(Dictionary<string, string> genParameters)
        {
            FileRenameParams = new Dictionary<string, string>()
            {
                { "Param_ProjectName", genParameters.SafeGet(GenParams.ProjectName) },
                { "Param_SourceName_Kebab", genParameters.SafeGet("ts.sourceName.casing.kebab") },
                { "Param_SourceName_Snake", genParameters.SafeGet("ts.sourceName.casing.snake") },
                { "Param_SourceName_Pascal", genParameters.SafeGet("ts.sourceName.casing.pascal") },
                { "Param_SourceName_Camel", genParameters.SafeGet("ts.sourceName.casing.camel") },
            };
        }

        public string ReplaceInPath(string filePath)
        {
            var newPath = filePath;
            foreach (var param in FileRenameParams)
            {
                newPath = newPath.Replace(param.Key, param.Value);
            }

            return newPath;
        }
    }
}
