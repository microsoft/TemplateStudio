// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.UI.Styles;

namespace Microsoft.Templates.UI.Controls
{
    public partial class SequentialFlow : UserControl
    {
        public static SequentialFlow Instance { get; private set; }

        public int Step
        {
            get => (int)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(int), typeof(SequentialFlow), new PropertyMetadata(0));

        public object Steps
        {
            get => GetValue(StepsProperty);
            set => SetValue(StepsProperty, value);
        }

        public static readonly DependencyProperty StepsProperty = DependencyProperty.Register("Steps", typeof(object), typeof(SequentialFlow), new PropertyMetadata(null));

        private bool _listViewLoaded;
        private bool _canFocusFirstStep;

        public SequentialFlow()
        {
            Resources.MergedDictionaries.Add(AllStylesDictionary.GetMergeDictionary());

            Instance = this;
            InitializeComponent();
        }

        private void ListViewSequentialFlow_OnLoaded(object sender, RoutedEventArgs e)
        {
            _listViewLoaded = true;
            TryFocusFirstStep();
        }

        public void FocusFirstStep()
        {
            _canFocusFirstStep = true;
            TryFocusFirstStep();
        }

        private void TryFocusFirstStep()
        {
            if (_listViewLoaded && _canFocusFirstStep && listViewSequentialFlow.ItemContainerGenerator.ContainerFromIndex(0) is ListViewItem item)
            {
                item.Focus();
                Keyboard.Focus(item);
                _canFocusFirstStep = false;
            }
        }
    }
}
