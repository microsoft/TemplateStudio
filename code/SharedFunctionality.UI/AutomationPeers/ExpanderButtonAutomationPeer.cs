// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.AutomationPeers
{
    public class ExpanderButtonAutomationPeer : ExpanderAutomationPeer
    {
        public ExpanderButtonAutomationPeer(Expander owner)
            : base(owner)
        {
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }
    }
}
