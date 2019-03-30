'{[{
Imports Param_RootNamespace.ViewModels
'}]}
Namespace Activation
    Friend Class SchemeActivationHandler
        Inherits ActivationHandler(Of ProtocolActivatedEventArgs)
        '{[{

        Private ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property

        ' By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
        Protected Overrides Async Function HandleInternalAsync(args As ProtocolActivatedEventArgs) As Task
            ' Create data from activation Uri in ProtocolActivatedEventArgs
            Dim data = New SchemeActivationData(args.Uri)
            If data.IsValid Then
                NavigationService.Navigate(data.ViewModelName, data.Parameters)
            ElseIf args.PreviousExecutionState <> ApplicationExecutionState.Running Then
                ' If the app isn't running and not navigating to a specific page based on the URI, navigate to the home page
                NavigationService.Navigate(GetType(ViewModels.Param_HomeNameViewModel).FullName)
            End If
            Await Task.CompletedTask
        End Function
        '}]}
    End Class
End Namespace