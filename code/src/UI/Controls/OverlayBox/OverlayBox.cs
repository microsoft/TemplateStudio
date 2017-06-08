using Microsoft.Templates.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Microsoft.Templates.UI.Controls
{
    public sealed partial class OverlayBox : Control
    {
        static OverlayBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(OverlayBox), new FrameworkPropertyMetadata(typeof(OverlayBox)));
        }

        private async void UpdateVisible(bool visible)
        {
            if (visible)
            {
                Panel.SetZIndex(this, 2);
                await this.FadeInAsync();
            }
            else
            {
                Panel.SetZIndex(this, 0);
                await this.FadeOutAsync();
            }
        }
    }
}
