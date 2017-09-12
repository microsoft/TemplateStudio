// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.Views.NewProject
{
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

        public InformationWindow(ViewModels.NewProject.MetadataInfoViewModel metadataInfo, Window mainWindow)
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
            else if (info is ViewModels.NewProject.TemplateInfoViewModel newProjectTemplateInfo)
            {
                ViewModel.Initialize(newProjectTemplateInfo);
            }
            else if (info is ViewModels.NewItem.TemplateInfoViewModel newItemTemplateInfo)
            {
                ViewModel.Initialize(newItemTemplateInfo);
            }
            else
            {
                throw new System.Exception(string.Format(StringRes.InformationWindowInitializeViewModelMessage, info.GetType().ToString()));
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
