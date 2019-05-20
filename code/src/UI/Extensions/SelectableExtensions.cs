using System.Collections.Generic;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.Extensions
{
    public static class SelectableExtensions
    {
        public static void UnselectAll(this IEnumerable<Selectable> selectableItems)
        {
            foreach (var item in selectableItems)
            {
                item.IsSelected = false;
            }
        }
    }
}
