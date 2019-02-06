namespace Param_RootNamespace.Services
{
    internal class ActivationService
    {
        public async Task ActivateAsync(object activationArgs)
        {
            if (IsInteractive(activationArgs))
            {
                if (Window.Current.Content == null)
                {
                    if (_shell?.Value == null)
                    {
                    }
                    else
                    {
                    }
//{[{

                    _container.RegisterInstance(typeof(IConnectedAnimationService), nameof(IConnectedAnimationService), new ConnectedAnimationService(Window.Current.Content as Frame));
//}]}
                }
            }
        }
    }
}
