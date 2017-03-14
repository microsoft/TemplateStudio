using System;

using Windows.UI.Xaml.Controls;

namespace uct.ItemName.Shell
{
    public class ShellNavigationItem
    {
        public string Name { get; set; }
        public Symbol Symbol { get; set; }
        public char SymbolAsChar { get { return (char)Symbol; } }
        public Type PageType { get; set; }

        private ShellNavigationItem(string name, Symbol symbol, Type pageType)
        {
            this.Name = name;
            this.Symbol = symbol;
            this.PageType = pageType;
        }

        public static ShellNavigationItem FromType<T>(string name, Symbol symbol) where T : Page
        {
            return new ShellNavigationItem(name, symbol, typeof(T));
        }       
    }
}