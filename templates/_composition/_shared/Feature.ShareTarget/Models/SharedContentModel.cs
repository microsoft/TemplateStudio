using System;
using System.Collections.ObjectModel;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace Param_ItemNamespace.Models
{
    public class SharedContentModel
    {
        public string DataFormat { get; set; }

        public string Title { get; set; }

        public Uri Uri { get; set; }

        public ObservableCollection<ImageSource> Images { get; } = new ObservableCollection<ImageSource>();

        public SharedContentModel()
        {
        }
    }
}
