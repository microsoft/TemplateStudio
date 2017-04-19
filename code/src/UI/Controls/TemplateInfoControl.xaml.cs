using Microsoft.Templates.UI.ViewModels;
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



        public TemplateInfoControl()
        {
            InitializeComponent();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
        }
    }
}
