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

using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.ViewModels;

using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Microsoft.Templates.UI.Controls
{
    public partial class TemplateInfoControl
    {
        #region TemplateInfo
        public TemplateInfoViewModel TemplateInfo
        {
            get => (TemplateInfoViewModel)GetValue(TemplateInfoProperty);
            set => SetValue(TemplateInfoProperty, value);
        }
        public static readonly DependencyProperty TemplateInfoProperty = DependencyProperty.Register("TemplateInfo", typeof(TemplateInfoViewModel), typeof(TemplateInfoControl), new PropertyMetadata(null));
        #endregion

        #region NoEditingContentVisibility
        public Visibility NoEditingContentVisibility
        {
            get => (Visibility)GetValue(NoEditingContentVisibilityProperty);
            set => SetValue(NoEditingContentVisibilityProperty, value);
        }
        public static readonly DependencyProperty NoEditingContentVisibilityProperty = DependencyProperty.Register("NoEditingContentVisibility", typeof(Visibility), typeof(TemplateInfoControl), new PropertyMetadata(Visibility.Visible));
        #endregion

        #region EditingContentVisibility
        public Visibility EditingContentVisibility
        {
            get => (Visibility)GetValue(EditingContentVisibilityProperty);
            set => SetValue(EditingContentVisibilityProperty, value);
        }
        public static readonly DependencyProperty EditingContentVisibilityProperty = DependencyProperty.Register("EditingContentVisibility", typeof(Visibility), typeof(TemplateInfoControl), new PropertyMetadata(Visibility.Collapsed));
        #endregion

        #region AddingVisibility
        public Visibility AddingVisibility
        {
            get => (Visibility)GetValue(AddingVisibilityProperty);
            set => SetValue(AddingVisibilityProperty, value);
        }
        public static readonly DependencyProperty AddingVisibilityProperty = DependencyProperty.Register("AddingVisibility", typeof(Visibility), typeof(TemplateInfoControl), new PropertyMetadata(Visibility.Visible));
        #endregion

        #region ErrorMessage
        public string ErrorMessage
        {
            get => (string)GetValue(ErrorMessageProperty);
            set => SetValue(ErrorMessageProperty, value);
        }
        public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(TemplateInfoControl), new PropertyMetadata(String.Empty));
        #endregion

        #region NewTemplateName
        public string NewTemplateName
        {
            get => (string)GetValue(NewTemplateNameProperty);
            set => SetValue(NewTemplateNameProperty, value);
        }
        public static readonly DependencyProperty NewTemplateNameProperty = DependencyProperty.Register("NewTemplateName", typeof(string), typeof(TemplateInfoControl), new PropertyMetadata(String.Empty, OnNewTemplateNameChanged));
        #endregion

        #region IsValidName
        public bool IsValidName
        {
            get => (bool)GetValue(IsValidNameProperty);
            set => SetValue(IsValidNameProperty, value);
        }
        public static readonly DependencyProperty IsValidNameProperty = DependencyProperty.Register("IsValidName", typeof(bool), typeof(TemplateInfoControl), new PropertyMetadata(true));
        #endregion

        #region TitleForeground
        public SolidColorBrush TitleForeground
        {
            get => (SolidColorBrush)GetValue(TitleForegroundProperty);
            set => SetValue(TitleForegroundProperty, value);
        }
        public static readonly DependencyProperty TitleForegroundProperty = DependencyProperty.Register("TitleForeground", typeof(SolidColorBrush), typeof(TemplateInfoControl), new PropertyMetadata(null));
        #endregion

        private ICommand _addItemCommand;
        public ICommand AddItemCommand => _addItemCommand ?? (_addItemCommand = new RelayCommand(OnAddItem));

        private ICommand _saveItemCommand;
        public ICommand SaveItemCommand => _saveItemCommand ?? (_saveItemCommand = new RelayCommand(OnSaveItem));

        private ICommand _showItemInfoCommand;
        public ICommand ShowItemInfoCommand => _showItemInfoCommand ?? (_showItemInfoCommand = new RelayCommand(OnShowItemInfo));

        private ICommand _closeEditionCommand;
        public ICommand CloseEditionCommand => _closeEditionCommand ?? (_closeEditionCommand = new RelayCommand(SwichVisibilities));
    }
}
