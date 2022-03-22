// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.UI.Styles;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Views.Common
{
    public partial class QuestionDialogWindow : Window
    {
        public QuestionDialogWindow(QuestionDialogViewModel vm)
        {
            Resources.MergedDictionaries.Add(AllStylesDictionary.GetMergeDictionary());
            DataContext = vm;
            vm.CloseAction = () => Close();

            InitializeComponent();
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseLeftButtonDown(e);
            DragMove();
        }
    }
}
