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
        private Dictionary<string, UIElement> dict = new Dictionary<string, UIElement>();

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
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("companyIcon", dict[item.HashIdentIcon]);
                    ConnectedAnimationService.GetForCurrentView().PrepareToAnimate("companyTitle", dict[item.HashIdentTitle]);

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

        private void registerAllElemens(DependencyObject root)
        {
            int childCount = VisualTreeHelper.GetChildrenCount(root);
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(root, i);
                if (child != null && child is FontIcon)
                {
                    FontIcon elem = (FontIcon)child;
                    if (elem.Tag != null && !dict.ContainsKey((string)elem.Tag))
                    {
                        dict.Add((string)elem.Tag, elem);
                    }
                }
                else if (child != null && child is TextBlock)
                {
                    TextBlock elem = (TextBlock)child;
                    if (elem.Tag != null && !dict.ContainsKey((string)elem.Tag))
                    {
                        dict.Add((string)elem.Tag, elem);
                    }
                }
                else
                {
                    for (int j = 0; j < childCount; j++)
                    {
                        registerAllElemens(child);
                    }
                }
            }
        }

        private void ListView_Loaded(object sender, RoutedEventArgs e)
        {
            registerAllElemens(MasterListView);
        }
    }
}
