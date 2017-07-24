using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Param_ItemNamespace.Models;
using Param_ItemNamespace.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;

namespace Param_ItemNamespace.Views
{
    public sealed partial class MasterDetailPage : Page, System.ComponentModel.INotifyPropertyChanged
    {
        private Order _selected;

        public Order Selected
        {
            get { return _selected; }
            set { Set(ref _selected, value); }
        }

        public ObservableCollection<Order> SampleItems { get; private set; } = new ObservableCollection<Order>();

        public MasterDetailPage()
        {
            InitializeComponent();
        }

        private async Task LoadDataAsync()
        {
            SampleItems.Clear();

            var data = await SampleDataService.GetSampleModelDataAsync();

            foreach (var item in data)
            {
                SampleItems.Add(item);
            }

            Selected = SampleItems.First();
        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var item = e?.ClickedItem as Order;
            if (item != null)
            {
                if (WindowStates.CurrentState.Name != "NarrowState")
                {
                	foreach (var fonticon in GetAllVisualChildrenOfType<FontIcon>(MasterListView))
                    {
                        var element = (UIElement)fonticon;
                        bool foundIcon = (item.HashIdentIcon == (string)fonticon.Tag);
                        if (foundIcon)
                        {
                            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("companyIcon", element);
                        }
                    }

                    foreach (var textblock in GetAllVisualChildrenOfType<TextBlock>(MasterListView))
                    {
                        var element = (UIElement)textblock;
                        bool foundTitle = (item.HashIdentTitle == (string)textblock.Tag);
                        if (foundTitle)
                        {
                            ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("companyTitle", element);
                        }
                    }
                }

                if (WindowStates.CurrentState == NarrowState)
                {
                    NavigationService.Navigate<Views.MasterDetailDetailPage>(item);
                }
                else
                {
                    Selected = item;
                }
            }
        }

        private IEnumerable<T> GetAllVisualChildrenOfType<T>(DependencyObject obj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(obj, i);
                if (child != null && child is T)
                    yield return (T)child;
                else
                {
                    for (int j = 0; j < VisualTreeHelper.GetChildrenCount(child); j++)
                    {
                        IEnumerable<T> childrenOfChild = GetAllVisualChildrenOfType<T>(child);
                        foreach (T subChild in childrenOfChild)
                        {
                            yield return subChild;
                        }
                    }
                }
            }
        }
    }
}
