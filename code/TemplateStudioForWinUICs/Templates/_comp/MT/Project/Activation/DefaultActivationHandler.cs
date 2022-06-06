using System;
using System.Threading.Tasks;
using Microsoft.UI.Xaml;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.ViewModels;

namespace Param_RootNamespace.Activation;

public class DefaultActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
{
    private readonly INavigationService _navigationService;

    public DefaultActivationHandler(INavigationService navigationService)
    {
        _navigationService = navigationService;
    }

    protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
    {
        // None of the ActivationHandlers has handled the activation.
        return _navigationService.Frame.Content == null;
    }

    protected async override Task HandleInternalAsync(LaunchActivatedEventArgs args)
    {
        _navigationService.NavigateTo(typeof(Param_HomeNameViewModel).FullName, args.Arguments);

        await Task.CompletedTask;
    }
}
