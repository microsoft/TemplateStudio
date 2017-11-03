using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace DragAndDropExample.Services
{
    public class DropConfiguration
    {
        public ICommand OnDropBitmapCommand { get; set; }
        public ICommand OnDropHtmlCommand { get; set; }
        public ICommand OnDropRtfCommand { get; set; }
        public ICommand OnDropStorageItemsCommand { get; set; }
        public ICommand OnDropTextCommand { get; set; }
        public ICommand OnDropApplicationLinkCommand { get; set; }
        public ICommand OnDropWebLinkCommand { get; set; }


        public ICommand OnDropDataViewCommand { get; set; }


        public DataPackageOperation AcceptedOperation { get; set; } = DataPackageOperation.Copy;

        public string Caption { get; set; } = string.Empty;
        public bool IsCaptionVisible { get; set; } = true;
        public bool IsContentVisible { get; set; } = true;
        public bool IsGlyphVisible { get; set; } = true;
        public ImageSource StartingDragImage { get; set; }
        public ImageSource DropOverImage { get; set; }

    }
}
