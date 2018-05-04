using System;
using Xamarin.Forms;

namespace WtsXamarin.Controls
{
    public class LinkLabel : Label
    {
        public static readonly BindableProperty LinkProperty =
            BindableProperty.Create("Link", typeof(string), typeof(LinkLabel), null);

        public LinkLabel()
        {
            TapGestureRecognizer tapGesture = new TapGestureRecognizer();

            tapGesture.Tapped += (sender, args) =>
            {
                if (!String.IsNullOrWhiteSpace(Link))
                {
                    Device.OpenUri(new Uri(Link));
                }
            };

            GestureRecognizers.Add(tapGesture);
        }

        public string Link
        {
            set { SetValue(LinkProperty, value); }
            get { return (string)GetValue(LinkProperty); }
        }
    }
}