Imports System
Imports System.Threading.Tasks
Imports Param_RootNamespace.Core.Helpers
Imports Windows.UI.Popups

Namespace Helpers
    Friend Module AuthenticationHelper
        Friend Async Function ShowLoginErrorAsync(loginResult As LoginResultType) As Task
            Select Case loginResult
                Case LoginResultType.NoNetworkAvailable
                    Await New MessageDialog("DialogNoNetworkAvailableContent".GetLocalized(), "DialogAuthenticationTitle".GetLocalized()).ShowAsync()
                Case LoginResultType.UnknownError
                    Await New MessageDialog("DialogStatusUnknownErrorContent".GetLocalized(), "DialogAuthenticationTitle".GetLocalized()).ShowAsync()
            End Select
        End Function
    End Module
End Namespace
