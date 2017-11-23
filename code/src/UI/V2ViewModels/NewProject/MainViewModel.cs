// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Threading;
using Microsoft.Templates.UI.V2ViewModels.Common;
using System.Windows.Input;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.V2Services;
using Microsoft.Templates.UI.V2Views.NewProject;

namespace Microsoft.Templates.UI.V2ViewModels.NewProject
{
    public class MainViewModel : BaseMainViewModel
    {
        private static MainViewModel _instance;

        public static MainViewModel Instance => _instance ?? (_instance = new MainViewModel());

        private DispatcherTimer _timer;
        private int _step;
        private ICommand _openDetailCommand;

        public int Step
        {
            get => _step;
            set => SetProperty(ref _step, value);
        }

        public ICommand OpenDetailCommand => _openDetailCommand ?? (_openDetailCommand = new RelayCommand(OnOpenDetail));

        private MainViewModel()
        {
            _timer = new DispatcherTimer() { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += TimerTick;
            _timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (Step == 0)
            {
                Step = 1;
            }
            else if (Step == 1)
            {
                Step = 2;
            }
            else if (Step == 2)
            {
                Step = 3;
            }
            else if (Step == 3)
            {
                Step = 0;
            }
        }

        private void OnOpenDetail()
        {
            NavigationService.NavigateMainFrame(new TemplateInfoPage());
        }
    }
}
