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
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Views.NewProject
{
    /// <summary>
    /// Interaction logic for InformationWindow.xaml
    /// </summary>
    public partial class InformationWindow : Window
    {
        public InformationViewModel ViewModel { get; private set; }

        public InformationWindow(ViewModels.NewProject.TemplateInfoViewModel template, Window mainWindow)
        {
            Init(template, mainWindow);
        }

        public InformationWindow(ViewModels.NewItem.TemplateInfoViewModel template, Window mainWindow)
        {
            Init(template, mainWindow);
        }

        public InformationWindow(ViewModels.NewProject.MetadataInfoViewModel  metadataInfo, Window mainWindow)
        {
            Init(metadataInfo, mainWindow);
        }

        private void Init(object info, Window mainWindow)
        {
            ViewModel = new InformationViewModel(this);
            DataContext = ViewModel;

            Owner = mainWindow;

            SetWindowSize(mainWindow);

            Loaded += (sender, e) =>
            {
                IntilizeViewModel(info);
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };

            InitializeComponent();
        }

        private void IntilizeViewModel(object info)
        {
            if (info is ViewModels.NewProject.MetadataInfoViewModel metadataInfo)
            {
                ViewModel.Initialize(metadataInfo);
            }
            else if(info is ViewModels.NewProject.TemplateInfoViewModel newProjectTemplateInfo)
            {
                ViewModel.Initialize(newProjectTemplateInfo);
            }
            else if (info is ViewModels.NewItem.TemplateInfoViewModel newItemTemplateInfo)
            {
                ViewModel.Initialize(newItemTemplateInfo);
            }
            else
            {
                throw new System.Exception($"{info.GetType().ToString()} is not expected as valid type for the Information Window.");
            }

            ViewModel.InformationVisibility = Visibility.Visible;
        }

        private void SetWindowSize(Window mainWindow)
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
        }
    }
}
