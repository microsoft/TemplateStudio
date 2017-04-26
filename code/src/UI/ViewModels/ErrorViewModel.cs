using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels
{
    public class ErrorViewModel : Observable
    {
        private const double HostHeightCollapsed = 180;
        private const double HostHeightExpanded = 420;

        private readonly Window _host;

        public ErrorViewModel(Window host, Exception ex)
        {
            _host = host;

            MessageDetailVisibility = Visibility.Collapsed;

            if (ex == null)
            {
                Message = StringRes.NoData;
            }
            else
            {
                Message = ex.Message;
                MessageDetail = ex.ToString();
            }
        }

        public ICommand CloseCommand => new RelayCommand(() => _host.Close());
        public ICommand ToggleDetailVisibleCommand => new RelayCommand(() => ToggleDetailVisible());

        private double _hostHeight;
        public double HostHeight
        {
            get { return _hostHeight; }
            set { SetProperty(ref _hostHeight, value); }
        }

        private string _toggleText;
        public string ToggleText
        {
            get { return _toggleText; }
            set { SetProperty(ref _toggleText, value); }
        }

        private string _message;
        public string Message
        {
            get { return _message; }
            set { SetProperty(ref _message, value); }
        }

        private string _messageDetail;
        public string MessageDetail
        {
            get { return _messageDetail; }
            set { SetProperty(ref _messageDetail, value); }
        }

        private Visibility _messageDetailVisibility;
        public Visibility MessageDetailVisibility
        {
            get { return _messageDetailVisibility; }
            set
            {
                SetProperty(ref _messageDetailVisibility, value);

                if (value == Visibility.Visible)
                {
                    HostHeight = HostHeightExpanded;
                    ToggleText = StringRes.LessDetail;
                }
                else
                {
                    HostHeight = HostHeightCollapsed;
                    ToggleText = StringRes.MoreDetail;
                }
            }
        }

        private void ToggleDetailVisible()
        {
            if (MessageDetailVisibility == Visibility.Visible)
            {
                MessageDetailVisibility = Visibility.Collapsed;
            }
            else
            {
                MessageDetailVisibility = Visibility.Visible;
            }
        }
    }
}
