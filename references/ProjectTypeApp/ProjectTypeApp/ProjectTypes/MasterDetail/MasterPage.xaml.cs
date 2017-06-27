using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace ProjectTypeApp.ProjectTypes.MasterDetail
{
    public partial class MasterPage : ContentPage
    {
        public ListView ListView;

        public MasterPage()
        {
	        InitializeComponent();

	        BindingContext = new MasterPageViewModel();
	        ListView = MenuItemsListView;
        }

        class MasterPageViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MenuItem> MenuItems { get; set; }

            public MasterPageViewModel()
            {
	            MenuItems = new ObservableCollection<MenuItem>(new[]
	            {
					new MenuItem { Id = 0, Title = "Page 1" },
					new MenuItem { Id = 1, Title = "Page 2" },
					new MenuItem { Id = 2, Title = "Page 3" },
					new MenuItem { Id = 3, Title = "Page 4" },
					new MenuItem { Id = 4, Title = "Page 5" },
				});
            }

            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
	            if (PropertyChanged == null)
		            return;

	            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}