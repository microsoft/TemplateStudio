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
