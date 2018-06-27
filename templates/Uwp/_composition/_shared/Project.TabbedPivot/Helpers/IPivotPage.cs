using System.Threading.Tasks;

namespace wts.ItemName.Helpers
{
    interface IPivotPage
    {
        Task OnPivotSelectedAsync();

        Task OnPivotUnselectedAsync();
    }
}
