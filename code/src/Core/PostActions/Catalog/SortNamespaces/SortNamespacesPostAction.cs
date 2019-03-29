// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog.SortNamespaces
{
    public abstract class SortNamespacesPostAction : PostAction
    {
        public abstract string FilesToSearch { get; }

        public abstract bool SortMethod(List<string> classContent);

        internal override void ExecuteInternal()
        {
            var classFiles = Directory
                .EnumerateFiles(Path.GetDirectoryName(GenContext.Current.GenerationOutputPath), FilesToSearch, SearchOption.AllDirectories)
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
