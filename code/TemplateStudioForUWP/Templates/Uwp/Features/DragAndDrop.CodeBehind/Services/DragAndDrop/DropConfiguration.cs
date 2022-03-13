using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

using Param_RootNamespace.Models;

namespace Param_RootNamespace.Services.DragAndDrop
{
    public class DropConfiguration : DependencyObject
    {
        public static readonly DependencyProperty DropBitmapActionProperty =
            DependencyProperty.Register("DropBitmapAction", typeof(Action<RandomAccessStreamReference>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropHtmlActionProperty =
            DependencyProperty.Register("DropHtmlAction", typeof(Action<string>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropRtfActionProperty =
            DependencyProperty.Register("DropRtfAction", typeof(Action<string>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropStorageItemsActionProperty =
            DependencyProperty.Register("DropStorageItemsAction", typeof(Action<IReadOnlyList<IStorageItem>>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropTextActionProperty =
            DependencyProperty.Register("DropTextAction", typeof(Action<string>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropApplicationLinkActionProperty =
            DependencyProperty.Register("DropApplicationLinkAction", typeof(Action<Uri>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropWebLinkActionProperty =
            DependencyProperty.Register("DropWebLinkAction", typeof(Action<Uri>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DropDataViewActionProperty =
            DependencyProperty.Register("DropDataViewAction", typeof(Action<DataPackageView>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragEnterActionProperty =
            DependencyProperty.Register("DragEnterAction", typeof(Action<DragDropData>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragOverActionProperty =
            DependencyProperty.Register("DragOverAction", typeof(Action<DragDropData>), typeof(DropConfiguration), new PropertyMetadata(null));

        public static readonly DependencyProperty DragLeaveActionProperty =
            DependencyProperty.Register("DragLeaveAction", typeof(Action<DragDropData>), typeof(DropConfiguration), new PropertyMetadata(null));

        public Action<RandomAccessStreamReference> DropBitmapAction
        {
            get { return (Action<RandomAccessStreamReference>)GetValue(DropBitmapActionProperty); }
            set { SetValue(DropBitmapActionProperty, value); }
        }

        public Action<string> DropHtmlAction
        {
            get { return (Action<string>)GetValue(DropHtmlActionProperty); }
            set { SetValue(DropHtmlActionProperty, value); }
        }

        public Action<string> DropRtfAction
        {
            get { return (Action<string>)GetValue(DropRtfActionProperty); }
            set { SetValue(DropRtfActionProperty, value); }
        }

        public Action<IReadOnlyList<IStorageItem>> DropStorageItemsAction
        {
            get { return (Action<IReadOnlyList<IStorageItem>>)GetValue(DropStorageItemsActionProperty); }
            set { SetValue(DropStorageItemsActionProperty, value); }
        }

        public Action<string> DropTextAction
        {
            get { return (Action<string>)GetValue(DropTextActionProperty); }
            set { SetValue(DropTextActionProperty, value); }
        }

        public Action<Uri> DropApplicationLinkAction
        {
            get { return (Action<Uri>)GetValue(DropApplicationLinkActionProperty); }
            set { SetValue(DropApplicationLinkActionProperty, value); }
        }

        public Action<Uri> DropWebLinkAction
        {
            get { return (Action<Uri>)GetValue(DropWebLinkActionProperty); }
            set { SetValue(DropWebLinkActionProperty, value); }
        }

        public Action<DataPackageView> DropDataViewAction
        {
            get { return (Action<DataPackageView>)GetValue(DropDataViewActionProperty); }
            set { SetValue(DropDataViewActionProperty, value); }
        }

        public Action<DragDropData> DragEnterAction
        {
            get { return (Action<DragDropData>)GetValue(DragEnterActionProperty); }
            set { SetValue(DragEnterActionProperty, value); }
        }

        public Action<DragDropData> DragOverAction
        {
            get { return (Action<DragDropData>)GetValue(DragOverActionProperty); }
            set { SetValue(DragOverActionProperty, value); }
        }

        public Action<DragDropData> DragLeaveAction
        {
            get { return (Action<DragDropData>)GetValue(DragLeaveActionProperty); }
            set { SetValue(DragLeaveActionProperty, value); }
        }

        public async Task ProcessComandsAsync(DataPackageView dataview)
        {
            if (DropDataViewAction != null)
            {
                DropDataViewAction.Invoke(dataview);
            }

            if (dataview.Contains(StandardDataFormats.ApplicationLink) && DropApplicationLinkAction != null)
            {
                Uri uri = await dataview.GetApplicationLinkAsync();
                DropApplicationLinkAction.Invoke(uri);
            }

            if (dataview.Contains(StandardDataFormats.Bitmap) && DropBitmapAction != null)
            {
                RandomAccessStreamReference stream = await dataview.GetBitmapAsync();
                DropBitmapAction.Invoke(stream);
            }

            if (dataview.Contains(StandardDataFormats.Html) && DropHtmlAction != null)
            {
                string html = await dataview.GetHtmlFormatAsync();
                DropHtmlAction.Invoke(html);
            }

            if (dataview.Contains(StandardDataFormats.Rtf) && DropRtfAction != null)
            {
                string rtf = await dataview.GetRtfAsync();
                DropRtfAction.Invoke(rtf);
            }

            if (dataview.Contains(StandardDataFormats.StorageItems) && DropStorageItemsAction != null)
            {
                IReadOnlyList<IStorageItem> storageItems = await dataview.GetStorageItemsAsync();
                DropStorageItemsAction.Invoke(storageItems);
            }

            if (dataview.Contains(StandardDataFormats.Text) && DropTextAction != null)
            {
                string text = await dataview.GetTextAsync();
                DropTextAction.Invoke(text);
            }

            if (dataview.Contains(StandardDataFormats.WebLink) && DropWebLinkAction != null)
            {
                Uri uri = await dataview.GetWebLinkAsync();
                DropWebLinkAction.Invoke(uri);
            }
        }
    }
}
