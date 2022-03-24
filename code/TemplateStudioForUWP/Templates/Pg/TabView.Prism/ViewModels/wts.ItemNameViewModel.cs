using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Param_RootNamespace.Models;

using Prism.Commands;
using Prism.Windows.Mvvm;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace Param_RootNamespace.ViewModels
{
    public class wts.ItemNameViewModel : ViewModelBase
    {
        private ICommand _addTabCommand;
        private ICommand _closeTabCommand;

        public ICommand AddTabCommand => _addTabCommand ?? (_addTabCommand = new DelegateCommand(AddTab));

        public ICommand CloseTabCommand => _closeTabCommand ?? (_closeTabCommand = new DelegateCommand<WinUI.TabViewTabCloseRequestedEventArgs>(CloseTab));

        public ObservableCollection<TabViewItemData> Tabs { get; } = new ObservableCollection<TabViewItemData>()
        {
            new TabViewItemData()
            {
                Index = 1,
                Header = "Item 1",
                //// In this sample the content shown in the Tab is a string, set the content to the model you want to show
                Content = "This is the content for Item 1."
            },
            new TabViewItemData()
            {
                Index = 2,
                Header = "Item 2",
                Content = "This is the content for Item 2."
            },
            new TabViewItemData()
            {
                Index = 3,
                Header = "Item 3",
                Content = "This is the content for Item 3."
            }
        };

        public wts.ItemNameViewModel()
        {
        }

        private void AddTab()
        {
            int newIndex = Tabs.Any() ? Tabs.Max(t => t.Index) + 1 : 1;
            Tabs.Add(new TabViewItemData()
            {
                Index = newIndex,
                Header = $"Item {newIndex}",
                Content = $"This is the content for Item {newIndex}"
            });
        }

        private void CloseTab(WinUI.TabViewTabCloseRequestedEventArgs args)
        {
            if (args.Item is TabViewItemData item)
            {
                Tabs.Remove(item);
            }
        }
    }
}
