// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Templates.UI.V2Controls
{
    public class WizardStep
    {
        public int Order { get; set; }

        public bool IsCompleated { get; set; }

        public string Title { get; set; }

        public WizardStep(int order, bool isCompleated, string title)
        {
            Order = order;
            IsCompleated = isCompleated;
            Title = title;
        }
    }
}
