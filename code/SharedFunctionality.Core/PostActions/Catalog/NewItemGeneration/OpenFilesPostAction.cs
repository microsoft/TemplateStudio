﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class OpenFilesPostAction : PostAction
    {
        internal override void ExecuteInternal()
        {
            GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(Resources.StatusOpeningItems);
            GenContext.ToolBox.Shell.UI.OpenItems(GenContext.Current.FilesToOpen.ToArray());
            GenContext.Current.FilesToOpen.Clear();
        }
    }
}
