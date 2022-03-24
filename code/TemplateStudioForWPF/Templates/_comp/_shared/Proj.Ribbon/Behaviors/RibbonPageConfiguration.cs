using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using Fluent;

namespace Param_RootNamespace.Behaviors
{
    public class RibbonPageConfiguration
    {
        public Collection<RibbonGroupBox> HomeGroups { get; set; } = new Collection<RibbonGroupBox>();

        public Collection<RibbonTabItem> Tabs { get; set; } = new Collection<RibbonTabItem>();

        public RibbonPageConfiguration()
        {
        }

        public void SetDataContext(System.ComponentModel.INotifyPropertyChanged viewModel, BindingMode bindingMode = BindingMode.OneWay)
        {
            foreach (var groups in HomeGroups)
            {
                groups.SetBinding(FrameworkElement.DataContextProperty, new Binding
                {
                    Source = viewModel,
                    Mode = bindingMode
                });
            }

            foreach (var tab in Tabs)
            {
                tab.SetBinding(FrameworkElement.DataContextProperty, new Binding
                {
                    Source = viewModel,
                    Mode = bindingMode
                });
            }
        }
    }
}
