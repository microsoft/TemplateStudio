// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
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

        public static readonly DependencyProperty StepProperty = DependencyProperty.Register("Step", typeof(int), typeof(SequentialFlow), new PropertyMetadata(0));

        public object Steps
        {
            get => GetValue(StepsProperty);
            set => SetValue(StepsProperty, value);
        }

        public static readonly DependencyProperty StepsProperty = DependencyProperty.Register("Steps", typeof(object), typeof(SequentialFlow), new PropertyMetadata(GetSteps()));

        public SequentialFlow()
        {
            InitializeComponent();
        }

        private static List<string> GetSteps()
        {
            return new List<string>()
            {
                "1. Project type",
                "2. Design pattern",
                "3. Pages",
                "4. Features"
            };
        }
    }
}
