using System.Collections.Generic;
using System.Threading.Tasks;

namespace Param_ItemNamespace.Helpers
{
    public interface IPivotActivationPage
    {
        Task OnPivotActivatedAsync(Dictionary<string, string> parameters);
    }
}
