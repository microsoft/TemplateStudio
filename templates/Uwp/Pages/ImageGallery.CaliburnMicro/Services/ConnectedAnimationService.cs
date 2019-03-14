using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Controls;

namespace Param_RootNamespace.Services
{
    public class ConnectedAnimationService : IConnectedAnimationService
    {
        private Frame _frame;

        public ConnectedAnimationService(Frame frame)
        {
            _frame = frame;
        }

        public void SetListDataItemForNextConnectedAnimation(object item)
        {
            _frame.SetListDataItemForNextConnectedAnimation(item);
        }
    }
}
