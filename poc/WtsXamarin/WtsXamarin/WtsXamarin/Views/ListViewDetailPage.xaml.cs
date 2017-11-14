using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WtsXamarin.Models;
using WtsXamarin.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXamarin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListViewDetailPage : ContentPage
    {
        public ListViewDetailPage(object parameter)
        {
            InitializeComponent();
            BindingContext = new ListViewDetailViewModel(parameter as SampleOrder);
        }
    }
}
