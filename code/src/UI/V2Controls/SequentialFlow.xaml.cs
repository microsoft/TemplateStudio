// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.V2Controls
{
    public partial class SequentialFlow : UserControl
    {
        public int Step
        {
            get => (int)GetValue(StepProperty);
            set => SetValue(StepProperty, value);
        }

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(int), typeof(SequentialFlow), new PropertyMetadata(0, OnStepPropertyChanged));

        public SequentialFlow()
        {
            InitializeComponent();
        }

        private static void OnStepPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as SequentialFlow;

            if (control != null)
            {
                control.UpdateStep((int)e.NewValue);
            }
        }

        private void UpdateStep(int newStep)
        {
            if (newStep == 0)
            {
                // 1. Project type
                SetSelected(projectTypeGrid, projectTypeTextBlock);
                SetUnSelected(frameworkGrid, frameworkTextBlock);
                SetUnSelected(pagesGrid, pagesTextBlock);
                SetUnSelected(featuresGrid, featuresTextBlock);
            }
            else if (newStep == 1)
            {
                // 2. Framework
                SetUnSelected(projectTypeGrid, projectTypeTextBlock);
                SetSelected(frameworkGrid, frameworkTextBlock);
                SetUnSelected(pagesGrid, pagesTextBlock);
                SetUnSelected(featuresGrid, featuresTextBlock);
            }
            else if (newStep == 2)
            {
                // 3. Pages
                SetUnSelected(projectTypeGrid, projectTypeTextBlock);
                SetUnSelected(frameworkGrid, frameworkTextBlock);
                SetSelected(pagesGrid, pagesTextBlock);
                SetUnSelected(featuresGrid, featuresTextBlock);
            }
            else if (newStep == 3)
            {
                // 4. Features
                SetUnSelected(projectTypeGrid, projectTypeTextBlock);
                SetUnSelected(frameworkGrid, frameworkTextBlock);
                SetUnSelected(pagesGrid, pagesTextBlock);
                SetSelected(featuresGrid, featuresTextBlock);
            }
        }

        private void SetSelected(Grid grid, TextBlock textBlock)
        {
            grid.Style = FindResource("WtsGridSequentialFlowActive") as Style;
            textBlock.Style = FindResource("WtsTextBlockSequentialFlowSelected") as Style;
        }

        private void SetUnSelected(Grid grid, TextBlock textBlock)
        {
            grid.Style = FindResource("WtsGridSequentialFlowInactive") as Style;
            textBlock.Style = FindResource("WtsTextBlockSequentialFlow") as Style;
        }
    }
}
