using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WtsXamarin.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckBox : ContentView
    {
        public event EventHandler<bool> CheckedChanged;

        public static readonly BindableProperty TextProperty =
            BindableProperty.Create("Text", typeof(string), typeof(CheckBox), null);

        public static readonly BindableProperty TextColorProperty =
            BindableProperty.Create("TextColor", typeof(Color), typeof(CheckBox), Color.Default);

        public static readonly BindableProperty FontSizeProperty =
            BindableProperty.Create("FontSize", typeof(double), typeof(CheckBox),
                Device.GetNamedSize(NamedSize.Default, typeof(Label)));

        public static readonly BindableProperty FontAttributesProperty =
            BindableProperty.Create("FontAttributes", typeof(FontAttributes), typeof(CheckBox), FontAttributes.None);

        public static readonly BindableProperty IsCheckedProperty =
            BindableProperty.Create("IsChecked", typeof(bool), typeof(CheckBox), false, BindingMode.TwoWay,
                propertyChanged: (bindable, oldValue, newValue) =>
                {
                    CheckBox checkbox = (CheckBox)bindable;
                    checkbox.CheckedChanged?.Invoke(checkbox, (bool)newValue);
                });

        public CheckBox()
        {
            InitializeComponent();
        }

        public string Text
        {
            set { SetValue(TextProperty, value); }
            get { return (string)GetValue(TextProperty); }
        }

        public Color TextColor
        {
            set { SetValue(TextColorProperty, value); }
            get { return (Color)GetValue(TextColorProperty); }
        }

        [TypeConverter(typeof(FontSizeConverter))]
        public double FontSize
        {
            set { SetValue(FontSizeProperty, value); }
            get { return (double)GetValue(FontSizeProperty); }
        }

        public FontAttributes FontAttributes
        {
            set { SetValue(FontAttributesProperty, value); }
            get { return (FontAttributes)GetValue(FontAttributesProperty); }
        }

        public bool IsChecked
        {
            set { SetValue(IsCheckedProperty, value); }
            get { return (bool)GetValue(IsCheckedProperty); }
        }

        private void OnCheckBoxTapped(object sender, EventArgs args)
        {
            IsChecked = !IsChecked;
        }
    }
}

