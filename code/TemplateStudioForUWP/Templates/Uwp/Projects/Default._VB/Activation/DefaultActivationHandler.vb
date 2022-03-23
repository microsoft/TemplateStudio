Imports Param_RootNamespace.Services

Namespace Activation
    Friend Class DefaultActivationHandler
        Inherits ActivationHandler(Of IActivatedEventArgs)

        Protected Overrides Async Function HandleInternalAsync(args As IActivatedEventArgs) As Task
            ' When the navigation stack isn't restored, navigate to the first page and configure
            ' the new page by passing required information in the navigation parameter
            Dim arguments As Object = Nothing
            Dim launchArgs = TryCast(args, LaunchActivatedEventArgs)
            If launchArgs IsNot Nothing Then
                arguments = launchArgs.Arguments
            End If

            NavigationService.Navigate(_navElement, arguments)
            Await Task.CompletedTask
        End Function

        Protected Overrides Function CanHandleInternal(args As IActivatedEventArgs) As Boolean
            ' None of the ActivationHandlers has handled the app activation
            Return NavigationService.Frame.Content Is Nothing
        End Function
    End Class
End Namespace
