// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class AddContextItemsToSolutionAndProjectPostAction : PostAction
    {
        internal override void ExecuteInternal()
        {
            GenContext.ToolBox.Shell.AddContextItemsToSolution(GenContext.Current.ProjectInfo);
        }
    }
}
