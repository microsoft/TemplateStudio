// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.Core.Gen
{
    public class SdkReference
    {
        public string Project { get; set; }

        public string Name { get; set; }

        public string Sdk { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is SdkReference sdkReference)
            {
                return Project == sdkReference.Project
                    && Name == sdkReference.Name
                    && Sdk == sdkReference.Sdk;
            }

            return false;
        }

        public override int GetHashCode() => base.GetHashCode();
    }
}
