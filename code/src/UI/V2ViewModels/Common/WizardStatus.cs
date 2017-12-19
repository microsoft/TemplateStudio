// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Templates.UI.V2Services;

namespace Microsoft.Templates.UI.V2ViewModels.Common
{
    public class WizardStatus
    {
        public double Width { get; }

        public double Height { get; }

        public WizardStatus()
        {
            var size = SystemService.Instance.GetMainWindowSize();
            Width = size.width;
            Height = size.height;
        }
    }
}
