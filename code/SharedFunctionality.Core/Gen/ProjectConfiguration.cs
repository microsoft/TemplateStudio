// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Gen
{
    public class ProjectConfiguration
    {
        public string Project { get; set; }

        public bool SetDeploy { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is ProjectConfiguration nugetReference)
            {
                return Project == nugetReference.Project;
            }

            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
