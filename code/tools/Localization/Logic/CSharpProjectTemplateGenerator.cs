// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Localization
{
    internal class CSharpProjectTemplateGenerator : ProjectTemplateGeneratorBase
    {
        public CSharpProjectTemplateGenerator(string sourceDirPath, string destinationDirPath) : base(GetConfig(sourceDirPath, destinationDirPath))
        {
        }

        private static ProjectTemplateGeneratorConfig GetConfig(string sourceDirPath, string destinationDirPath)
        {
            return new ProjectTemplateGeneratorConfig()
            {
                SourceDir = Path.Combine(sourceDirPath, @"\code\src\ProjectTemplates\CSharp.UWP.2017.Solution"),
                SourceDirNamePattern = "CSharp.UWP.2017.Solution",
                SourceFileNamePattern = "CSharp.UWP.VS2017.Solution.vstemplate",
                DestinationDirNamePattern = "CSharp.UWP.2017.Solution",
                DestinationFileNamePattern = "CSharp.UWP.VS2017.Solution.{0}.vstemplate",
                DestinationDirPath = destinationDirPath
            };
        }
    }
}
