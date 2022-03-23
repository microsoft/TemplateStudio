// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces
{
    public abstract class SortNamespacesPostAction : PostAction
    {
        public SortNamespacesPostAction(List<string> paths)
        {
            Paths = paths;
        }

        public List<string> Paths { get; set; }

        public abstract string FilesToSearch { get; }

        public abstract bool SortMethod(List<string> classContent);

        internal override void ExecuteInternal()
        {
            foreach (var path in Paths)
            {
                var classFiles = Directory
                .EnumerateFiles(path, FilesToSearch, SearchOption.AllDirectories)
                .ToList();

                foreach (var classFile in classFiles)
                {
                    var fileContent = File.ReadAllLines(classFile).ToList();
                    var sortResult = SortMethod(fileContent);

                    if (sortResult)
                    {
                        File.WriteAllLines(classFile, fileContent, Encoding.UTF8);
                    }
                }
            }
        }
    }
}
