// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Localization
{
    internal class VisualBasicProjectTemplateGenerator : ProjectTemplateGeneratorBase
    {
        protected override string SourceDirRelPath => @"\code\src\ProjectTemplates\VBNet.UWP.VS2017.Solution";
        protected override string SourceDirNamePattern => "VBNet.UWP.VS2017.Solution";
        protected override string SourceFileNamePattern => "VBNet.UWP.VS2017.Solution.vstemplate";
        protected override string DestinationDirNamePattern => "VBNet.UWP.VS2017.Solution";
        protected override string DestinationFileNamePattern => "VBNet.UWP.VS2017.Solution.{0}.vstemplate";
        public VisualBasicProjectTemplateGenerator(string sourceDirPath, string destinationDirPath) : base(sourceDirPath, destinationDirPath)
        {
        }
    }
}
