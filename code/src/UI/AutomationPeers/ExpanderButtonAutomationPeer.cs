using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.AutomationPeers
{
    public class ExpanderButtonAutomationPeer : ExpanderAutomationPeer
    {
        public ExpanderButtonAutomationPeer(Expander owner) : base(owner)
        {
        }

        protected override AutomationControlType GetAutomationControlTypeCore()
        {
            return AutomationControlType.Button;
        }
    }
}
