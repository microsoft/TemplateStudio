#if (isBasic)
using SplitViewProject.Core;
#else if (isMVVMLight)
using GalaSoft.MvvmLight;
#endif

namespace SplitViewProject.Home
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
        }
    }
}
