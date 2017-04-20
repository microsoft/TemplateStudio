using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

        public ICommand AddCommand
        {
            get { return (ICommand)GetValue(AddCommandProperty); }
            set { SetValue(AddCommandProperty, value); }
        }
        public static readonly DependencyProperty AddCommandProperty = DependencyProperty.Register("AddCommand", typeof(ICommand), typeof(TemplateInfoControl), new PropertyMetadata(null));

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

        public Func<IEnumerable<string>> GetUsedNames
        {
            get { return (Func<IEnumerable<string>>)GetValue(GetUsedNamesProperty); }
            set { SetValue(GetUsedNamesProperty, value); }
        }
        public static readonly DependencyProperty GetUsedNamesProperty = DependencyProperty.Register("GetUsedNames", typeof(Func<IEnumerable<string>>), typeof(TemplateInfoControl), new PropertyMetadata(null));

        public string NewTemplateName
        {
            get { return (string)GetValue(NewTemplateNameProperty); }
            set { SetValue(NewTemplateNameProperty, value); }
        }
        public static readonly DependencyProperty NewTemplateNameProperty = DependencyProperty.Register("NewTemplateName", typeof(string), typeof(TemplateInfoControl), new PropertyMetadata(String.Empty));



        public TemplateInfoControl()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }

        private void OnAddClicked(object sender, RoutedEventArgs e)
        {
            var names = GetUsedNames.Invoke();
            NewTemplateName = Core.Naming.Infer(names, TemplateInfo.Name);
            SwichVisibilities();
        }

        private void OnSaveClicked(object sender, RoutedEventArgs e)
        {
            AddCommand.Execute((NewTemplateName, TemplateInfo.Template));
            SwichVisibilities();
        }

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

        private void OnMouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void OnCloseEdition(object sender, RoutedEventArgs e)
        {
            if (EditingContentVisibility == Visibility.Visible)
            {
                SwichVisibilities();
            }
        }
    }
}
