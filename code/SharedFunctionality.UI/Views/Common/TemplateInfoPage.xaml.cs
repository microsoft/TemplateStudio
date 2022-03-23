// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Microsoft.Templates.UI.Converters;
using Microsoft.Templates.UI.Styles;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.VisualStudio.PlatformUI;

namespace Microsoft.Templates.UI.Views.Common
{
    public partial class TemplateInfoPage : Page
    {
        public BasicInfoViewModel ViewModel { get; }

        public TemplateInfoPage(BasicInfoViewModel basicInfoViewModel)
        {
            Resources.MergedDictionaries.Add(AllStylesDictionary.GetMergeDictionary());
            Resources.Add("HasItemsVisibilityConverter", new HasItemsVisibilityConverter());
            Resources.Add("StringVisibilityConverter", new StringVisibilityConverter());
            Resources.Add("BrushToColorConverter", new BrushToColorConverter());

            ViewModel = basicInfoViewModel;
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void TemplateInfoPage_OnPreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (e.NewFocus is Hyperlink hyperlink && hyperlink.Tag?.ToString() == nameof(Inline))
            {
                var hyperlinkHelpText = hyperlink.GetValue(AutomationProperties.NameProperty)?.ToString();
                if (!string.IsNullOrEmpty(hyperlinkHelpText))
                {
                    accessibleHyperlink.SetValue(AutomationProperties.NameProperty, hyperlinkHelpText);

                    // Inform a screen reader to read the hyperlink text
                    // https://devblogs.microsoft.com/dotnet/net-framework-4-7-1-accessibility-and-wpf-improvements/#uiautomation-liveregion-support
                    var peer = UIElementAutomationPeer.FromElement(accessibleTextBlock) ?? UIElementAutomationPeer.CreatePeerForElement(accessibleTextBlock);
                    if (peer is TextBlockAutomationPeer textBlockPeer)
                    {
                        textBlockPeer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
                    }
                }
            }
        }
    }
}
