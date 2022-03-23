using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace Param_RootNamespace.Services.DragAndDrop
{
    public class DropConfiguration : DependencyObject
    {
        public static readonly DependencyProperty DropBitmapCommandProperty =
            DependencyProperty.Register("DropBitmapCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropHtmlCommandProperty =
            DependencyProperty.Register("DropHtmlCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropRtfCommandProperty =
            DependencyProperty.Register("DropRtfCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropStorageItemsCommandProperty =
            DependencyProperty.Register("DropStorageItemsCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropTextCommandProperty =
            DependencyProperty.Register("DropTextCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropApplicationLinkCommandProperty =
            DependencyProperty.Register("DropApplicationLinkCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropWebLinkCommandProperty =
            DependencyProperty.Register("DropWebLinkCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropDataViewCommandProperty =
            DependencyProperty.Register("DropDataViewCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragEnterCommandProperty =
            DependencyProperty.Register("DragEnterCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragOverCommandProperty =
            DependencyProperty.Register("DragOverCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragLeaveCommandProperty =
            DependencyProperty.Register("DragLeaveCommand", typeof(ICommand), typeof(DropConfiguration), new PropertyMetadata(null));

        public ICommand DropBitmapCommand
        {
            get { return (ICommand)GetValue(DropBitmapCommandProperty); }
            set { SetValue(DropBitmapCommandProperty, value); }
        }

        public ICommand DropHtmlCommand
        {
            get { return (ICommand)GetValue(DropHtmlCommandProperty); }
            set { SetValue(DropHtmlCommandProperty, value); }
        }

        public ICommand DropRtfCommand
        {
            get { return (ICommand)GetValue(DropRtfCommandProperty); }
            set { SetValue(DropRtfCommandProperty, value); }
        }

        public ICommand DropStorageItemsCommand
        {
            get { return (ICommand)GetValue(DropStorageItemsCommandProperty); }
            set { SetValue(DropStorageItemsCommandProperty, value); }
        }

        public ICommand DropTextCommand
        {
            get { return (ICommand)GetValue(DropTextCommandProperty); }
            set { SetValue(DropTextCommandProperty, value); }
        }

        public ICommand DropApplicationLinkCommand
        {
            get { return (ICommand)GetValue(DropApplicationLinkCommandProperty); }
            set { SetValue(DropApplicationLinkCommandProperty, value); }
        }

        public ICommand DropWebLinkCommand
        {
            get { return (ICommand)GetValue(DropWebLinkCommandProperty); }
            set { SetValue(DropWebLinkCommandProperty, value); }
        }

        public ICommand DropDataViewCommand
        {
            get { return (ICommand)GetValue(DropDataViewCommandProperty); }
            set { SetValue(DropDataViewCommandProperty, value); }
        }

        public ICommand DragEnterCommand
        {
            get { return (ICommand)GetValue(DragEnterCommandProperty); }
            set { SetValue(DragEnterCommandProperty, value); }
        }

        public ICommand DragOverCommand
        {
            get { return (ICommand)GetValue(DragOverCommandProperty); }
            set { SetValue(DragOverCommandProperty, value); }
        }

        public ICommand DragLeaveCommand
        {
            get { return (ICommand)GetValue(DragLeaveCommandProperty); }
            set { SetValue(DragLeaveCommandProperty, value); }
        }

        public async Task ProcessComandsAsync(DataPackageView dataview)
        {
            if (DropDataViewCommand != null)
            {
                DropDataViewCommand.Execute(dataview);
            }

            if (dataview.Contains(StandardDataFormats.ApplicationLink) && DropApplicationLinkCommand != null)
            {
                Uri uri = await dataview.GetApplicationLinkAsync();
                DropApplicationLinkCommand.Execute(uri);
            }

            if (dataview.Contains(StandardDataFormats.Bitmap) && DropBitmapCommand != null)
            {
                RandomAccessStreamReference stream = await dataview.GetBitmapAsync();
                DropBitmapCommand.Execute(stream);
            }

            if (dataview.Contains(StandardDataFormats.Html) && DropHtmlCommand != null)
            {
                string html = await dataview.GetHtmlFormatAsync();
                DropHtmlCommand.Execute(html);
            }

            if (dataview.Contains(StandardDataFormats.Rtf) && DropRtfCommand != null)
            {
                string rtf = await dataview.GetRtfAsync();
                DropRtfCommand.Execute(rtf);
            }

            if (dataview.Contains(StandardDataFormats.StorageItems) && DropStorageItemsCommand != null)
            {
                IReadOnlyList<IStorageItem> storageItems = await dataview.GetStorageItemsAsync();
                DropStorageItemsCommand.Execute(storageItems);
            }

            if (dataview.Contains(StandardDataFormats.Text) && DropTextCommand != null)
            {
                string text = await dataview.GetTextAsync();
                DropTextCommand.Execute(text);
            }

            if (dataview.Contains(StandardDataFormats.WebLink) && DropWebLinkCommand != null)
            {
                Uri uri = await dataview.GetWebLinkAsync();
                DropWebLinkCommand.Execute(uri);
            }
        }
    }
}
