using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Extensions;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Controls
{
    public class StatusBox : Control
    {
        public StatusViewModel Status
        {
            get { return (StatusViewModel)GetValue(StatusProperty); }
            set { SetValue(StatusProperty, value); }
        }
        public static readonly DependencyProperty StatusProperty = DependencyProperty.Register("Status", typeof(StatusViewModel), typeof(StatusBox), new PropertyMetadata(null, OnStatusPropertyChanged));

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
        public static readonly DependencyProperty CloseCommandProperty = DependencyProperty.Register("CloseCommand", typeof(ICommand), typeof(StatusBox), new PropertyMetadata(null));

        private static async void OnStatusPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as StatusBox;
            if (control != null)
            {
                await control.UpdateStatusAsync(e.NewValue as StatusViewModel);
            }
        }

        public StatusBox()
        {
            DefaultStyleKey = typeof(StatusBox);
            CloseCommand = new RelayCommand(OnClose);
        }

        private async void OnClose()
        {
            await UpdateVisibilityAsync(false);
        }

        private async Task UpdateStatusAsync(StatusViewModel statusViewModel)
        {
            var isVisible = statusViewModel != null && statusViewModel.Status != StatusType.Empty;
            await UpdateVisibilityAsync(isVisible);
        }

        private async Task UpdateVisibilityAsync(bool isVisible)
        {
            if (isVisible)
            {
                Panel.SetZIndex(this, 2);
                await this.FadeInAsync();
            }
            else
            {
                await this.FadeOutAsync();
                Panel.SetZIndex(this, 0);
            }
        }
    }
}
