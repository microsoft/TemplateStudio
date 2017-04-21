using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.UI.ViewModels
{
    public class SummaryItemViewModel : Observable
    {
        public string ItemName { get; set; }
        public string TemplateName { get; set; }
        public string Author { get; set; }
        public bool IsRemoveEnabled { get; set; }
        public string DisplayText { get => $"{ItemName} [{TemplateName}]"; }
    }
}
