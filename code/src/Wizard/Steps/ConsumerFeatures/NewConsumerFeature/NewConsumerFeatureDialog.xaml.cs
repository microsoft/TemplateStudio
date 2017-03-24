// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.Steps.Pages;

namespace Microsoft.Templates.Wizard.Steps.ConsumerFeatures.NewConsumerFeature
{
    /// <summary>
    /// Interaction logic for NewDevFeatureDialog.xaml
    /// </summary>
    public partial class NewConsumerFeatureDialog : Window
    {
        public NewConsumerFeatureViewModel ViewModel { get; }
        public (string name, string templateName) Result { get; set; }

        public NewConsumerFeatureDialog(WizardContext context, IEnumerable<PageViewModel> selectedTemplates)
        {
            ViewModel = new NewConsumerFeatureViewModel(context, this, selectedTemplates);

            DataContext = ViewModel;
            Loaded += NewDevFeatureDialog_Loaded;
            InitializeComponent();
        }

        private async void NewDevFeatureDialog_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.InitializeAsync();
        }
    }
}
