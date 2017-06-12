using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.ViewModels.NewItem
{
    public enum LineStatus { Default = 0, New = 1, Deleted = 2 };
    public class CodeLineViewModel : Observable
    {
        public static double DefaultFontSize = 10;

        private LineStatus _status;
        public LineStatus Status
        {
            get => _status;
            private set => SetProperty(ref _status, value);
        }

        private uint _number;
        public uint Number
        {
            get => _number;
            private set => SetProperty(ref _number, value);
        }
        public string NumberString
        {
            get => _number.ToString("000");
        }

        private string _line;
        public string Line
        {
            get => _line;
            private set => SetProperty(ref _line, value);
        }

        private double _fontSize;
        public double FontSize
        {
            get => _fontSize;
            set => SetProperty(ref _fontSize, value);
        }


        public CodeLineViewModel(uint number, string line, LineStatus status = LineStatus.Default)
        {
            Number = number;
            Line = line;
            Status = status;
            FontSize = DefaultFontSize;
        }

        public CodeLineViewModel(uint number, CodeLineViewModel codeLine, LineStatus status = LineStatus.Default)
        {
            Number = number;
            Line = codeLine.Line;
            Status = status;
        }
    }
}
