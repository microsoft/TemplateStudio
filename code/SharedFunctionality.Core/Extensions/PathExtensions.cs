// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.Extensions
{
    public static class PathExtensions
    {
        public static string GetPathRelativeToGenerationParentPath(this string filePath)
        {
            var generationParentDirectory = Directory.GetParent(GenContext.Current.GenerationOutputPath).FullName;
            return GetRelativePath(filePath, generationParentDirectory);
        }

        public static string GetPathRelativeToGenerationPath(this string filePath)
        {
            var generationDirectory = GenContext.Current.GenerationOutputPath;
            return GetRelativePath(filePath, generationDirectory);
        }

        public static string GetPathRelativeToDestinationParentPath(this string filePath)
        {
            var generationParentDirectory = Directory.GetParent(GenContext.Current.DestinationPath).FullName;
            return GetRelativePath(filePath, generationParentDirectory);
        }

        public static string GetDestinationPath(this string filePath)
        {
            var parentGenerationOutputPath = Directory.GetParent(GenContext.Current.GenerationOutputPath).FullName;
            var parentDestinationPath = Directory.GetParent(GenContext.Current.DestinationPath).FullName;

            return filePath.Replace(parentGenerationOutputPath, parentDestinationPath);
        }

        public static string GetGenerationPath(this string filePath)
        {
            var parentGenerationOutputPath = Directory.GetParent(GenContext.Current.GenerationOutputPath).FullName;
            var parentDestinationPath = Directory.GetParent(GenContext.Current.DestinationPath).FullName;

            return filePath.Replace(parentDestinationPath, parentGenerationOutputPath);
        }

        private static string GetRelativePath(this string filePath, string folderName)
        {
            if (filePath.Contains(folderName))
            {
                return filePath.Replace(folderName + Path.DirectorySeparatorChar, string.Empty);
            }

            return filePath;
        }
    }
}
