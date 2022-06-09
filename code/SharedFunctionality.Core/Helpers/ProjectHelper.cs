// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core.Helpers
{
    public static class ProjectHelper
    {
        public static Dictionary<string, List<string>> ResolveProjectFiles(IEnumerable<string> itemsFullPath, bool workWithProjitemsFile = false)
        {
            Dictionary<string, List<string>> filesByProject = new Dictionary<string, List<string>>();
            foreach (var item in itemsFullPath)
            {
                var itemDirectory = Directory.GetParent(item).FullName;
                var projFile = Fs.FindFileAtOrAbove(itemDirectory, "*.*proj");
                if (string.IsNullOrEmpty(projFile))
                {
                    AppHealth.Current.Error.TrackAsync(string.Format(Resources.ExceptionProjectNotFound, item)).FireAndForget();
                }
                else
                {
                    if (workWithProjitemsFile && Path.GetExtension(projFile) == ".shproj")
                    {
                        projFile = projFile.Replace(".shproj", ".projitems");
                    }

                    if (!filesByProject.ContainsKey(projFile))
                    {
                        filesByProject.Add(projFile, new List<string>() { item });
                    }
                    else
                    {
                        filesByProject[projFile].Add(item);
                    }
                }
            }

            return filesByProject;
        }
    }
}
