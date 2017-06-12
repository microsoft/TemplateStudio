using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public class CodeLineViewModel : Observable
    {
        public static double DefaultFontSize = 10;
        private string _line;
        public string Line
        {
            get => _line;
            set => SetProperty(ref _line, value);
        }

        private double _fontSize;
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }


        public CodeLineViewModel(string line)
        {
            Line = line;
            FontSize = DefaultFontSize;
        }
    }
}
