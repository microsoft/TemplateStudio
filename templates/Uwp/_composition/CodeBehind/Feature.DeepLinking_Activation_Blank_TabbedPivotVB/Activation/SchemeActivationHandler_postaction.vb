Namespace Activation
    Friend Class SchemeActivationHandler
        Inherits ActivationHandler(Of ProtocolActivatedEventArgs)
        '{[{

        ' By default, this handler expects URIs of the format 'wtsapp:sample?secret={value}'
        Protected Overrides Async Function HandleInternalAsync(args As ProtocolActivatedEventArgs) As Task
            ' Create data from activation Uri in ProtocolActivatedEventArgs
            Dim data = New SchemeActivationData(args.Uri)
            If data.IsValid Then
                NavigationService.Navigate(data.PageType, data.Parameters)
            ElseIf args.PreviousExecutionState <> ApplicationExecutionState.Running Then
                NavigationService.Navigate(GetType(Views.PivotPage))
            End If
            Await Task.CompletedTask
        End Function
        '}]}
    End Class
End Namespace
