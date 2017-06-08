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

using System.Windows;

using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.NewItem;
using System.Windows.Input;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Controls;
using System.Windows.Controls;
using System;
using System.Linq;
using Microsoft.Templates.Core;

namespace Microsoft.Templates.UI.Views.NewItem
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainViewModel ViewModel { get; }
        public UserSelection Result { get; set; }

        public MainView(TemplateType templateType)
        {
            ViewModel = new MainViewModel(this);

            DataContext = ViewModel;

            Loaded += async (sender, e) =>
            {
                await ViewModel.InitializeAsync(templateType, overlayBox);
                NavigationService.Initialize(stepFrame, new NewItemSetupView());                
            };

            Unloaded += (sender, e) =>
            {
                ViewModel.UnsuscribeEventHandlers();
            };

            InitializeComponent();
        }
    }
}
