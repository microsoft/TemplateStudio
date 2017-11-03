using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace DragAndDropExample.Models
{
    public class CustomItem
    {
        public CustomItem()
        {
            Id = Guid.NewGuid();            
        }

        public Guid Id { get; }
        public string Path { get; set; }

        public string FileName { get; set; }

        public BitmapImage Image { get; set; }

        public IStorageItem OriginalStorageItem { get; set; }
    }
}
