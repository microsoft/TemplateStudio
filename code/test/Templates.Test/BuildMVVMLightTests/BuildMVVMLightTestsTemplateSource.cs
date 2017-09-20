// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Core.Packaging;

namespace Microsoft.Templates.Test
{
    public sealed class BuildMVVMLightTestTemplatesSource : LocalTemplatesSource
    {
        public override string Id => "TestBuildMVVMLight";
    }
}
