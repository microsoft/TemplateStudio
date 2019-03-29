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
            };
        }

        public string ReplaceInPath(string filePath)
        {
            // Workaround as template engine is not replacing fileRename parameters correctly in file names, when used together with sourceName,
            // working from template engine version 2.1.0, remove this workaround when using this version.
            var newPath = filePath;
            foreach (var param in FileRenameParams)
            {
                newPath = newPath.Replace(param.Key, param.Value);
            }

            return newPath;
        }
    }
}
