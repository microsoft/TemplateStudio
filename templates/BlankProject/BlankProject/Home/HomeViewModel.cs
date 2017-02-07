#if (isBasic)
using BlankProject.Core;
#else if(isMVVMLight)
using GalaSoft.MvvmLight;
#endif

namespace BlankProject.Home
{
    public class HomeViewModel : ViewModelBase
    {
        public HomeViewModel()
        {
        }
    }
}
