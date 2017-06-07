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
    }
}
