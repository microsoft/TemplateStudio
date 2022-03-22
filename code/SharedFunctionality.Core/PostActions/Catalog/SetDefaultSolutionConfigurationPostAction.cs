// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class SetDefaultSolutionConfigurationPostAction : PostAction
    {
        private const string Configuration = "Debug";
        private const string Platform = "x86";

        internal override void ExecuteInternal()
        {
            GenContext.ToolBox.Shell.Solution.SetDefaultSolutionConfiguration(Configuration, Platform, GenContext.Current.ProjectName);
        }
    }
}
