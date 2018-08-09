using System.Threading.Tasks;

namespace Param_ItemNamespace.Helpers
{
    public interface IPivotPage
    {
        Task OnPivotSelectedAsync();

        Task OnPivotUnselectedAsync();
    }
}
