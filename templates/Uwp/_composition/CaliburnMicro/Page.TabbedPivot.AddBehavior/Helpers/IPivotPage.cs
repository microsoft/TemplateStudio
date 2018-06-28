using System.Threading.Tasks;

namespace Param_ItemNamespace.Helpers
{
    interface IPivotPage
    {
        Task OnPivotSelectedAsync();

        Task OnPivotUnselectedAsync();
    }
}
