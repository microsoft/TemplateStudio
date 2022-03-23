using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer.ShareTarget;

namespace Param_RootNamespace.ViewModels
{
    public class SharedDataViewModelBase : System.ComponentModel.INotifyPropertyChanged
    {
        private string _dataFormat;

        public string DataFormat
        {
            get => _dataFormat;
            set => Param_Setter(ref _dataFormat, value);
        }

        private string _pageTitle;

        public string PageTitle
        {
            get => _pageTitle;
            set => Param_Setter(ref _pageTitle, value);
        }

        private string _title;

        public string Title
        {
            get => _title;
            set => Param_Setter(ref _title, value);
        }

        public SharedDataViewModelBase()
        {
        }

        public virtual async Task LoadDataAsync(ShareOperation shareOperation)
        {
            Title = shareOperation.Data.Properties.Title;
            await Task.CompletedTask;
        }
    }
}
