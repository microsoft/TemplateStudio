using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace InkPoc.Controls
{
    public class MouseInkingInkOption :  InkOption
    {
        private const string MouseInkingLabel = "Mouse inking";

        public InkToolbarCustomToggleButton MouseInkingButton { get; set; }

        public MouseInkingInkOption()
        {
            MouseInkingButton = InkOptionHelper.BuildInkToolbarCustomToggleButton(MouseInkingLabel, "E962");
        }
    }
}
