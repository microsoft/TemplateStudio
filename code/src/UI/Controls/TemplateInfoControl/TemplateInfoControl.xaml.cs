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

using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels;

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
        public TemplateInfoControl()
        {
            InitializeComponent();

            TitleForeground = FindResource("UIBlack") as SolidColorBrush;

            MainViewModel.Current.ProjectTemplates.UpdateTemplateAvailable += (sender, args) => CheckAddingStatus();
            Loaded += (sender, args) => CheckAddingStatus();
        }

        private void OnAddItem()
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

        private void OnSaveItem()
        {
            if (IsValidName)
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

        private void OnShowItemInfo()
        {
            MainViewModel.Current.ProjectTemplates.ShowInfoCommand.Execute(TemplateInfo);
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
            IsValidName = validationResult.IsValid;
            ErrorMessage = String.Empty;

            if (!IsValidName)
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

        private static void OnNewTemplateNameChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as TemplateInfoControl;
            if (control.TemplateInfo.CanChooseItemName)
            {
                control.Validate(e.NewValue as string);
            }
        }
    }
}
