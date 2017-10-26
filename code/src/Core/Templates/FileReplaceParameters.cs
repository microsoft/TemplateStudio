// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.Templates
{
    public static class FileReplaceParameters
    {
        public static Dictionary<string, string> Params { get; } = new Dictionary<string, string>()
            {
                { "Param_ProjectName", GenContext.Current.ProjectName }
            };

        public static string ReplaceInPath(string filePath)
        {
            // HACK: Template engine is not replacing fileRename parameters correctly in file names, when used together with sourceName
            var newPath = filePath;
            foreach (var param in Params)
            {
                newPath = newPath.Replace(param.Key, param.Value);
            }

            return newPath;
        }
    }
}
