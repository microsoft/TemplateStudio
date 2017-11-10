using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;

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

        public async Task ProcessComandsAsync(DataPackageView dataview)
        {
            if (OnDropDataViewCommand != null)
            {
                OnDropDataViewCommand.Execute(dataview);
            }


            if (dataview.Contains(StandardDataFormats.ApplicationLink) && OnDropApplicationLinkCommand != null)
            {
                var uri = await dataview.GetApplicationLinkAsync();
                OnDropApplicationLinkCommand.Execute(uri);
            }

            if (dataview.Contains(StandardDataFormats.Bitmap) && OnDropBitmapCommand != null)
            {
                var stream = await dataview.GetBitmapAsync();
                OnDropBitmapCommand.Execute(stream);
            }

            if (dataview.Contains(StandardDataFormats.Html) && OnDropHtmlCommand != null)
            {
                var html = await dataview.GetHtmlFormatAsync();
                OnDropHtmlCommand.Execute(html);
            }

            if (dataview.Contains(StandardDataFormats.Rtf) && OnDropRtfCommand != null)
            {
                var rtf = await dataview.GetRtfAsync();
                OnDropRtfCommand.Execute(rtf);
            }

            if (dataview.Contains(StandardDataFormats.StorageItems) && OnDropStorageItemsCommand != null)
            {
                var storageItems = await dataview.GetStorageItemsAsync();
                OnDropStorageItemsCommand.Execute(storageItems);
            }

            if (dataview.Contains(StandardDataFormats.Text) && OnDropTextCommand != null)
            {
                var text = await dataview.GetTextAsync();
                OnDropTextCommand.Execute(text);
            }

            if (dataview.Contains(StandardDataFormats.WebLink) && OnDropWebLinkCommand != null)
            {
                var uri = await dataview.GetWebLinkAsync();
                OnDropWebLinkCommand.Execute(uri);
            }
        }
    }
}
