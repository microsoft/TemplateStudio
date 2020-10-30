// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Input;

namespace Microsoft.Templates.UI.Controls
{
    public partial class NotificationMarkdownBlock : UserControl
    {
        public NotificationMarkdownBlock()
        {
            InitializeComponent();

            CommandBindings.Add(new CommandBinding(NavigationCommands.GoToPage, (sender, e) => SafeNavigate(e.Parameter)));
        }

        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(NotificationMarkdownBlock), new PropertyMetadata(null));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private void SafeNavigate(object parameter)
        {
            if (parameter is string uri && Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            {
                Process.Start(uri);
            }
        }

        private void NotificationMarkdownBlock_OnLoaded(object sender, RoutedEventArgs e)
        {
            RaiseAutomationEventFlowDocument();
        }

        private void RaiseAutomationEventFlowDocument()
        {
            var peer = UIElementAutomationPeer.FromElement(flowDocumentScrollViewer) ?? UIElementAutomationPeer.CreatePeerForElement(flowDocumentScrollViewer);

            if (peer is FlowDocumentScrollViewerAutomationPeer flowPeer)
            {
                flowPeer.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            }
        }
    }
}
