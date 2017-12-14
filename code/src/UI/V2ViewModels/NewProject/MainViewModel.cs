// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Input;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2ViewModels.Common;
using Microsoft.Templates.UI.V2Views.NewProject;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class MainViewModel : BaseMainViewModel
    {
        private static MainViewModel _instance;

        public static MainViewModel Instance => _instance ?? (_instance = new MainViewModel());

        private int _step;
        private string _summaryImage;

        public int Step
        {
            get => _step;
            set => SetProperty(ref _step, value);
        }

        public string SummaryImage
        {
            get => _summaryImage;
            set => SetProperty(ref _summaryImage, value);
        }

        public ProjectTypeViewModel ProjectType { get; } = new ProjectTypeViewModel();

        public DesignPatternViewModel DesignPattern { get; } = new DesignPatternViewModel();

        private MainViewModel()
        {
            SummaryImage = "/Microsoft.Templates.UI;component/Assets/PaneStep1-2.png";
            ProjectType.ProjectTypeChanged += OnProjectTypeChanged;
        }

        protected override void OnCancel()
        {
            WizardShell.Current.Close();
        }

        protected override void OnGoBack()
        {
            switch (Step)
            {
                case 1: // GoBack button Clicked on DesignPatternPage
                    NavigationService.GoBackMainFrame();
                    Step--;
                    SetCanGoBack(false);
                    break;
                case 2: // GoBack button Clicked on PagesPage
                    NavigationService.GoBackMainFrame();
                    SummaryImage = "/Microsoft.Templates.UI;component/Assets/PaneStep1-2.png";
                    Step--;
                    break;
                case 3: // GoBack button Clicked on FeaturesPage
                    NavigationService.GoBackMainFrame();
                    Step--;
                    SummaryImage = "/Microsoft.Templates.UI;component/Assets/PaneStep3.png";
                    SetCanGoForward(true);
                    break;
            }
        }

        protected override void OnGoForward()
        {
            switch (Step)
            {
                case 0: // GoForward button Clicked on ProjectTypePage
                    NavigationService.NavigateSecondaryFrame(new DesignPatternPage());
                    Step++;
                    SetCanGoBack(true);
                    break;
                case 1: // GoForward button Clicked on DesignPatternPage
                    NavigationService.NavigateSecondaryFrame(new PagesPage());
                    SummaryImage = "/Microsoft.Templates.UI;component/Assets/PaneStep3.png";
                    Step++;
                    break;
                case 2: // GoForward button Clicked on PagesPage
                    NavigationService.NavigateSecondaryFrame(new FeaturesPage());
                    SummaryImage = "/Microsoft.Templates.UI;component/Assets/PaneStep4.png";
                    Step++;
                    SetCanGoForward(false);
                    break;
            }
        }

        protected override void OnFinish()
        {
        }

        private void OnProjectTypeChanged(object sender, BasicInfoViewModel e)
        {
            DesignPattern.LoadData(e.Name);
        }
    }
}
