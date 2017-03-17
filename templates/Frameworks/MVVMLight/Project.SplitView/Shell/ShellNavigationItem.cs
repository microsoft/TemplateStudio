using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Controls;

namespace uct.ItemName.Shell
{
    public class ShellNavigationItem
    {
        public string Name { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar { get { return (char)Symbol; } }
        public string ViewModelName { get; set; }

        public ShellNavigationItem(string name, Symbol symbol, string viewModelName)
        {
            this.Name = name;
            this.Symbol = symbol;
            this.ViewModelName = viewModelName;
        }      
    }
}