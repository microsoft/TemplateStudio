using Param_RootNamespace.Core.Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.TemplateSelectors
{
    public class SampleDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate CompanyTemplate { get; set; }

        public DataTemplate OrderTemplate { get; set; }

        public DataTemplate OrderDetailTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            return GetTemplate(item) ?? base.SelectTemplateCore(item);
        }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            return GetTemplate(item) ?? base.SelectTemplateCore(item, container);
        }

        private DataTemplate GetTemplate(object item)
        {
            switch (item)
            {
                case SampleCompany company:
                    return CompanyTemplate;
                case SampleOrder order:
                    return OrderTemplate;
                case SampleOrderDetail orderDetail:
                    return OrderDetailTemplate;
            }

            return null;
        }
    }
}
