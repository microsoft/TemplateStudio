// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Automation.Peers;
using System.Windows.Controls;
using Microsoft.Templates.UI.AutomationPeers;

namespace Microsoft.Templates.UI.Controls
{
    public class ExpanderButtonAutomationPeerControl : Expander
    {
        protected override AutomationPeer OnCreateAutomationPeer()
        {
            return new ExpanderButtonAutomationPeer(this);
        }
    }
}
