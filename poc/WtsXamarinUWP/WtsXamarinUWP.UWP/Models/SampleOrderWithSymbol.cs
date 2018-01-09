using System;
using Windows.UI.Xaml.Controls;
using WtsXamarinUWP.Core.Models;

namespace WtsXamarinUWP.UWP.Models
{
    public class SampleOrderWithSymbol : SampleOrder
    {
        public SampleOrderWithSymbol(SampleOrder sampleOrder)
        {
            OrderId = sampleOrder.OrderId;
            OrderDate = sampleOrder.OrderDate;
            Company = sampleOrder.Company;
            ShipTo = sampleOrder.ShipTo;
            OrderTotal = sampleOrder.OrderTotal;
            Status = sampleOrder.Status;

            Enum.TryParse(sampleOrder.IconName, out Symbol symbol);
            Symbol = symbol;
        }

        public Symbol Symbol { get; set; }

        public char SymbolAsChar
        {
            get { return (char)Symbol; }
        }

        public string HashIdentIcon
        {
            get { return GetHashCode().ToString() + "-icon"; }
        }

        public string HashIdentTitle
        {
            get { return GetHashCode().ToString() + "-title"; }
        }
    }
}
