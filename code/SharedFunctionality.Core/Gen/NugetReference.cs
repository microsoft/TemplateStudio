// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Core.Gen
{
    [DebuggerDisplay("{PackageId}, v{Version}")]
    public class NugetReference
    {
        public string Project { get; set; }

        public string PackageId { get; set; }

        public string Version { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is NugetReference nugetReference)
            {
                return Project == nugetReference.Project
                    && PackageId == nugetReference.PackageId
                    && Version == nugetReference.Version;
            }

            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
