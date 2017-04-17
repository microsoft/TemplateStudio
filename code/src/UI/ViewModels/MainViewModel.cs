using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels
{
    public class MainViewModel : Observable
    {
        private MainView _mainView;

        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        private bool _canGoBack;
        public bool CanGoBack
        {
            get { return _canGoBack; }
            set { SetProperty(ref _canGoBack, value); }
        }

        private ICommand _cancelCommand;
        private ICommand _backCommand;
        private ICommand _nextCommand;

        public ICommand CancelCommand => _cancelCommand ?? new RelayCommand(OnCancel);
        public ICommand BackCommand => _backCommand ?? new RelayCommand(OnBack, () => CanGoBack);
        public ICommand NextCommand => _nextCommand ?? new RelayCommand(OnNext);

        public MainViewModel(MainView mainView)
        {
            _mainView = mainView;
        }

        public async Task IniatializeAsync()
        {
            Title = StringRes.Step1Title;
        }

        public void UnsuscribeEventHandlers()
        {
        }

        private void OnCancel()
        {
            _mainView.DialogResult = false;
            _mainView.Result = null;
            _mainView.Close();
        }

        private void OnBack()
        {
        }

        private void OnNext()
        {
        }
    }
}
