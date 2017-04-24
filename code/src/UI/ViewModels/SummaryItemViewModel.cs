using Microsoft.Templates.Core.Mvvm;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace Microsoft.Templates.UI.ViewModels
{
    public class SummaryItemViewModel : Observable
    {
        public string Identity { get; set; }
        public string ItemName { get; set; }
        public string TemplateName { get; set; }
        public string Author { get; set; }
        public bool IsRemoveEnabled { get; set; }
        public string DisplayText { get => $"{ItemName} [{TemplateName}]"; }

        private FontWeight _itemFontWeight = FontWeights.Normal;        

        private Brush _itemForeground = new SolidColorBrush(Color.FromRgb(1,122,243));
        public Brush ItemForeground
        {
            get => _itemForeground;
            set => SetProperty(ref _itemForeground, value);
        }

        private DispatcherTimer dt;
        public SummaryItemViewModel()
        {
            dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(2);
            dt.Tick += OnTimerTick;
            dt.Start();
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            ItemForeground = new SolidColorBrush(Colors.Black);
            dt.Stop();
        }
    }
}
