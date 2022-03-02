// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class CopyFilesToProjectPostAction : PostAction<TempGenerationResult>
    {
        private List<string> excludeFromOpeningExtensions = new List<string>() { ".png", ".jpg", ".bmp", ".ico", ".csproj", ".vbproj" };

        public CopyFilesToProjectPostAction(TempGenerationResult config)
            : base(config)
        {
        }

        internal override void ExecuteInternal()
        {
            var parentGenerationOutputPath = Directory.GetParent(GenContext.Current.GenerationOutputPath).FullName;
            var destinationParentPath = Directory.GetParent(GenContext.Current.DestinationPath).FullName;

            foreach (var file in Config.ModifiedFiles)
            {
                var sourceFile = Path.Combine(parentGenerationOutputPath, file);
                var destFilePath = Path.Combine(destinationParentPath, file);

                var destDirectory = Path.GetDirectoryName(destFilePath);
                Fs.SafeCopyFile(sourceFile, destDirectory, true);
            }

            foreach (var file in Config.NewFiles)
            {
                var sourceFile = Path.Combine(parentGenerationOutputPath, file);
                var destFilePath = Path.Combine(destinationParentPath, file);

                var destDirectory = Path.GetDirectoryName(destFilePath);
                Fs.SafeCopyFile(sourceFile, destDirectory, true);

                if (!excludeFromOpeningExtensions.Contains(Path.GetExtension(destFilePath)))
                {
                    GenContext.Current.FilesToOpen.Add(destFilePath);
                }
            }
        }
    }
}
