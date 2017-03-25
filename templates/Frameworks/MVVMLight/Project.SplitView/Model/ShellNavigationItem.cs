using System;

using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace uct.ItemName.Model
{
    public class ShellNavigationItem
    {
        public string Name { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar { get { return (char)Symbol; } }
        public string ViewModelName { get; set; }

        public ShellNavigationItem(string name, Symbol symbol, string viewModelName)
        {
            Name = name;
            Symbol = symbol;
            ViewModelName = viewModelName;
        }
    }
}