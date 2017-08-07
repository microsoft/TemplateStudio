// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Localization
{
    internal class CSharpProjectTemplateGenerator : ProjectTemplateGeneratorBase
    {
        protected override string SourceDirRelPath => @"\code\src\ProjectTemplates\CSharp.UWP.2017.Solution";
        protected override string SourceDirNamePattern => "CSharp.UWP.2017.Solution";
        protected override string SourceFileNamePattern => "CSharp.UWP.VS2017.Solution.vstemplate";
        protected override string DestinationDirNamePattern => "CSharp.UWP.2017.Solution";
        protected override string DestinationFileNamePattern => "CSharp.UWP.VS2017.Solution.{0}.vstemplate";
        public CSharpProjectTemplateGenerator(string sourceDirPath, string destinationDirPath) : base(sourceDirPath, destinationDirPath)
        {
        }
    }
}
