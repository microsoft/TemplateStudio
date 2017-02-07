#if (isBasic)
using Param_PageNS.Core;
#else if (isMVVMLight)
using GalaSoft.MvvmLight;
#endif

namespace Param_PageNS.BlankPage
{
    public class BlankPageViewModel : ViewModelBase
    {
        public BlankPageViewModel()
        {
        }        
    }
}