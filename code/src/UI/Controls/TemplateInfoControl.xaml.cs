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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels;
using Microsoft.Templates.Core.Mvvm;

namespace Microsoft.Templates.UI.Controls
{
    public partial class TemplateInfoControl : UserControl
    {
        public TemplateInfoViewModel TemplateInfo
        {
            get { return (TemplateInfoViewModel)GetValue(TemplateInfoProperty); }
            set { SetValue(TemplateInfoProperty, value); }
        }
        public static readonly DependencyProperty TemplateInfoProperty = DependencyProperty.Register("TemplateInfo", typeof(TemplateInfoViewModel), typeof(TemplateInfoControl), new PropertyMetadata(null));

        public Visibility NoEditingContentVisibility
        {
            get { return (Visibility)GetValue(NoEditingContentVisibilityProperty); }
            set { SetValue(NoEditingContentVisibilityProperty, value); }
        }
        public static readonly DependencyProperty NoEditingContentVisibilityProperty = DependencyProperty.Register("NoEditingContentVisibility", typeof(Visibility), typeof(TemplateInfoControl), new PropertyMetadata(Visibility.Visible));

        public Visibility EditingContentVisibility
        {
            get { return (Visibility)GetValue(EditingContentVisibilityProperty); }
            set { SetValue(EditingContentVisibilityProperty, value); }
        }
        public static readonly DependencyProperty EditingContentVisibilityProperty = DependencyProperty.Register("EditingContentVisibility", typeof(Visibility), typeof(TemplateInfoControl), new PropertyMetadata(Visibility.Collapsed));

        public Visibility AddingVisibility
        {
            get { return (Visibility)GetValue(AddingVisibilityProperty); }
            set { SetValue(AddingVisibilityProperty, value); }
        }
        public static readonly DependencyProperty AddingVisibilityProperty = DependencyProperty.Register("AddingVisibility", typeof(Visibility), typeof(TemplateInfoControl), new PropertyMetadata(Visibility.Visible));

        public string NewTemplateName
        {
            get { return (string)GetValue(NewTemplateNameProperty); }
            set { SetValue(NewTemplateNameProperty, value); }
        }
        public static readonly DependencyProperty NewTemplateNameProperty = DependencyProperty.Register("NewTemplateName", typeof(string), typeof(TemplateInfoControl), new PropertyMetadata(String.Empty, OnNewTemplateNameChanged));
        private static void OnNewTemplateNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TemplateInfoControl;

            if (control.TemplateInfo.CanChooseItemName)
            {
                control.Validate(e.NewValue as string);
            }
        }

        public string ErrorMessage
        {
            get { return (string)GetValue(ErrorMessageProperty); }
            set { SetValue(ErrorMessageProperty, value); }
        }
        public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(TemplateInfoControl), new PropertyMetadata(String.Empty));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }
        public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register("IsValid", typeof(bool), typeof(TemplateInfoControl), new PropertyMetadata(true));

        public SolidColorBrush TitleForeground
        {
            get { return (SolidColorBrush)GetValue(TitleForegroundProperty); }
            set { SetValue(TitleForegroundProperty, value); }
        }
        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(SolidColorBrush), typeof(TemplateInfoControl), new PropertyMetadata(null));

        private ICommand _saveItemCommand;
        public ICommand SaveItemCommand => _saveItemCommand ?? (_saveItemCommand = new RelayCommand(OnSaveClicked));

        private ICommand _closeEditionCommand;
        public ICommand CloseEditionCommand => _closeEditionCommand ?? (_closeEditionCommand = new RelayCommand(SwichVisibilities));

        public TemplateInfoControl()
        {
            InitializeComponent();

            TitleForeground = FindResource("UIBlack") as SolidColorBrush;

            MainViewModel.Current.ProjectTemplates.UpdateTemplateAvailable += (sender, args) => CheckAddingStatus();
        }

        private void OnAddClicked(object sender, RoutedEventArgs e)
        {
            var names = MainViewModel.Current.ProjectTemplates.GetUsedNamesFunc.Invoke();

            if (TemplateInfo.CanChooseItemName)
            {
                NewTemplateName = Naming.Infer(names, TemplateInfo.Template.GetDefaultName());

                SwichVisibilities();

                templateName.Focus();
                templateName.SelectAll();
            }
            else
            {
                NewTemplateName = TemplateInfo.Template.GetDefaultName();
                MainViewModel.Current.ProjectTemplates.AddCommand.Execute((NewTemplateName, TemplateInfo.Template));

                CheckAddingStatus();
            }
        }

        private void OnSaveClicked()
        {
            if (IsValid)
            {
                MainViewModel.Current.ProjectTemplates.AddCommand.Execute((NewTemplateName, TemplateInfo.Template));

                SwichVisibilities();
                CheckAddingStatus();
            }
        }

        private void CheckAddingStatus()
        {
            if (TemplateInfo.MultipleInstances == false && IsAlreadyDefined)
            {
                AddingVisibility = Visibility.Collapsed;
                TitleForeground = FindResource("UIMiddleLightGray") as SolidColorBrush;
            }
            else
            {
                AddingVisibility = Visibility.Visible;
                TitleForeground = FindResource("UIBlack") as SolidColorBrush;
            }
        }

        private bool IsAlreadyDefined => MainViewModel.Current.ProjectTemplates.GetUsedTemplatesIdentitiesFunc.Invoke().Any(name => name == TemplateInfo.Template.Identity);

        private void SwichVisibilities()
        {
            if (EditingContentVisibility == Visibility.Collapsed)
            {
                EditingContentVisibility = Visibility.Visible;
                NoEditingContentVisibility = Visibility.Collapsed;
            }
            else
            {
                EditingContentVisibility = Visibility.Collapsed;
                NoEditingContentVisibility = Visibility.Visible;
            }
        }

        private void HandleValidation(Core.ValidationResult validationResult)
        {
            IsValid = validationResult.IsValid;
            ErrorMessage = String.Empty;

            if (!validationResult.IsValid)
            {
                ErrorMessage = StringRes.ResourceManager.GetString($"ValidationError_{validationResult.ErrorType}");

                if (string.IsNullOrWhiteSpace(ErrorMessage))
                {
                    ErrorMessage = "UndefinedError";
                }

                throw new Exception(ErrorMessage);
            }
        }

        public void Validate(string name)
        {
            var names = MainViewModel.Current.ProjectTemplates.GetUsedNamesFunc.Invoke();
            var validationResult = Core.Naming.Validate(names, name);

            HandleValidation(validationResult);
        }

        private void OnShowInfo(object sender, RoutedEventArgs e)
        {
            MainViewModel.Current.ProjectTemplates.ShowInfoCommand.Execute(TemplateInfo);
        }
    }
}
