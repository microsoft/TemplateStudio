using System;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Views
{
    public sealed partial class MediaPlayerViewPage : Page
    {
        public MediaPlayerViewPage()
        {
            InitializeComponent();
            ViewModel.Initialize(mpe);
        }
    }
}
