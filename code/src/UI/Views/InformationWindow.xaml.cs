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

using System.Windows;

using Microsoft.Templates.UI.ViewModels;

namespace Microsoft.Templates.UI.Views
{
    /// <summary>
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        public InformationViewModel ViewModel { get; }

        public InformationWindow(TemplateInfoViewModel template, Window mainWindow)
        {
            ViewModel = new InformationViewModel(this);
            DataContext = ViewModel;

            Loaded += (sender, e) =>
            {
                ViewModel.Initialize(template);
                CenterWindow(mainWindow);
                ViewModel.InformationVisibility = Visibility.Visible;
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };

            InitializeComponent();
        }        

        public InformationWindow(MetadataInfoViewModel  metadataInfo, Window mainWindow)
        {
            ViewModel = new InformationViewModel(this);
            DataContext = ViewModel;

            Loaded += (sender, e) =>
            {
                ViewModel.Initialize(metadataInfo);
                CenterWindow(mainWindow);
                ViewModel.InformationVisibility = Visibility.Visible;
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };

            InitializeComponent();
        }

        private void CenterWindow(Window mainWindow)
        {            
            if (mainWindow.Width < 1200)
            {
                Width = mainWindow.Width * 0.8;
            }
            else
            {
                Width = mainWindow.Width * 0.6;
            }
            if (mainWindow.Height < 700)
            {
                Height = mainWindow.Height * 0.8;
            }
            else
            {
                Height = mainWindow.Height * 0.6;
            }

            Left = mainWindow.Left + (mainWindow.Width - ActualWidth) / 2;
            Top = mainWindow.Top + (mainWindow.Height - ActualHeight) / 2;
        }
    }
}
