using System;
using XamarinUwpNative.Core;
using XamarinUwpNative.UWP.Helpers;

namespace XamarinUwpNative.UWP.ViewModels
{
    public class MainViewModel : Observable
    {
        public MainViewModel()
        {
            var model = new BasicModel(6);
        }
    }
}
