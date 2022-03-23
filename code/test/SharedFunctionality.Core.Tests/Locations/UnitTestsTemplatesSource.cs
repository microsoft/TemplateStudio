// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;

using Microsoft.Templates.Core.Locations;

namespace Microsoft.Templates.Core.Test.Locations
{
    public sealed class UnitTestsTemplatesSource : LocalTemplatesSource
    {
        public UnitTestsTemplatesSource(string installedPackagePath)
            : base(installedPackagePath)
        {
        }

        public override string Id => "UnitTest" + GetAgentName();

        protected override string Origin => $@"..\..\..\TestData\{TemplatesFolderName}";

        public override string GetContentRootFolder()
        {
            var dir = Path.GetDirectoryName(Environment.CurrentDirectory);
            dir = Path.Combine(dir, @"..\..\TestData\Templates\test");

            return dir;
        }
    }
}
