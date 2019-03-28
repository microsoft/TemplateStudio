'{[{
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Views
'}]}
Namespace Activation
    Friend Class SchemeActivationHandler
        Inherits ActivationHandler(Of ProtocolActivatedEventArgs)
'{[{

        ' By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
        Protected Overrides Async Function HandleInternalAsync(args As ProtocolActivatedEventArgs) As Task
            ' Create data from activation Uri in ProtocolActivatedEventArgs
            Dim data = New SchemeActivationData(args.Uri)
            If data.IsValid Then
                MenuNavigationHelper.UpdateView(data.PageType, data.Parameters)
            ElseIf args.PreviousExecutionState <> ApplicationExecutionState.Running Then
                ' If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                NavigationService.Navigate(GetType(Views.Param_HomeNamePage))
            End If
            Await Task.CompletedTask
        End Function
'}]}
    End Class
End Namespace
