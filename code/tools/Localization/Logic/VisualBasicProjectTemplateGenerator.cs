// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;

namespace Localization
{
    internal class VisualBasicProjectTemplateGenerator : ProjectTemplateGeneratorBase
    {
        public VisualBasicProjectTemplateGenerator(string sourceDirPath, string destinationDirPath)
            : base(GetConfig(sourceDirPath, destinationDirPath))
        {
        }

        private static ProjectTemplateGeneratorConfig GetConfig(string sourceDirPath, string destinationDirPath)
        {
            return new ProjectTemplateGeneratorConfig()
            {
                SourceDir = Path.Combine(sourceDirPath, @"\code\src\ProjectTemplates\VBNet.UWP.VS2017.Solution"),
                SourceDirNamePattern = "VBNet.UWP.VS2017.Solution",
                SourceFileNamePattern = "VBNet.UWP.VS2017.Solution.vstemplate",
                DestinationDirNamePattern = "VBNet.UWP.VS2017.Solution",
                DestinationFileNamePattern = "VBNet.UWP.VS2017.Solution.{0}.vstemplate",
                DestinationDirPath = destinationDirPath
            };
        }
    }
}
