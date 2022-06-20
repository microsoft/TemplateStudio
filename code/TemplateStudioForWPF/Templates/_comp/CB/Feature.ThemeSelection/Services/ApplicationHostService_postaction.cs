﻿namespace Param_RootNamespace.Services;

public class ApplicationHostService : IHostedService
{
    private readonly INavigationService _navigationService;
//{[{
    private readonly IThemeSelectorService _themeSelectorService;
//}]}
    public ApplicationHostService(/*{[{*/IThemeSelectorService themeSelectorService/*}]}*/)
    {
//^^
//{[{
        _themeSelectorService = themeSelectorService;
//}]}
    }

    private async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
//^^
//{[{
            _themeSelectorService.InitializeTheme();
//}]}
            await Task.CompletedTask;
        }
    }
}
