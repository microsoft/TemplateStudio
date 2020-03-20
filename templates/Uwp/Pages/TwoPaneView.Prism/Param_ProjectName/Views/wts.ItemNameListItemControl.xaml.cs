using System;
using Param_RootNamespace.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Views
{
    public sealed partial class wts.ItemNameListItemControl : UserControl
    {
        public SampleOrder Item
        {
            get { return (SampleOrder)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }

        public static readonly DependencyProperty ItemProperty = DependencyProperty.Register(nameof(Item), typeof(SampleOrder), typeof(wts.ItemNameListItemControl), new PropertyMetadata(null, OnItemPropertyChanged));

        public wts.ItemNameListItemControl()
        {
            this.InitializeComponent();
        }

        private static void OnItemPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }
    }
}
