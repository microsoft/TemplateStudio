// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels;
using Microsoft.Templates.UI.Controls;

namespace Microsoft.Templates.UI.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainViewModel ViewModel { get; }
        public UserSelection Result { get; set; }

        public MainView()
        {
            ViewModel = new MainViewModel(this);

            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                NavigationService.Initialize(stepFrame, new ProjectSetupView());
                await ViewModel.InitializeAsync(summaryPageGroups);
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };

            InitializeComponent();
        }

        private void OnPreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var control = e.OldFocus as TextBoxEx;
            var button = e.NewFocus as Button;
            string name = (button != null && button.Tag != null) ? button.Tag.ToString() : string.Empty;

            if (control != null && string.IsNullOrEmpty(name))
            {
                var templateInfo = control.Tag as TemplateInfoViewModel;
                if (templateInfo != null)
                {
                    templateInfo.CloseEdition();
                }
                var summaryItem = control.Tag as SavedTemplateViewModel;
                if (summaryItem != null)
                {
                    summaryItem.OnCancelRename();
                }
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            ViewModel.ProjectTemplates.SavedPages.ToList().ForEach(spg => spg.ToList().ForEach(p =>
            {
                if (p.IsEditionEnabled)
                {
                    p.ConfirmRenameCommand.Execute(p);
                    p.TryClose();
                }
            }));

            ViewModel?.ProjectTemplates?.SavedFeatures?.ToList()?.ForEach(f =>
            {
                if (f.IsEditionEnabled)
                {
                    f.ConfirmRenameCommand.Execute(f);
                    f.TryClose();
                }
            });
        }
    }
}
