using System;
using System.Linq;
using DragAndDropExample.ViewModels;

using Windows.UI.Xaml.Controls;
using DragAndDropExample.Models;
using Windows.ApplicationModel.DataTransfer;

namespace DragAndDropExample.Views
{
    public sealed partial class Scenario3Page : Page
    {
        public Scenario3ViewModel ViewModel { get; } = new Scenario3ViewModel();

        public Scenario3Page()
        {
            InitializeComponent();
        }
    }
}
