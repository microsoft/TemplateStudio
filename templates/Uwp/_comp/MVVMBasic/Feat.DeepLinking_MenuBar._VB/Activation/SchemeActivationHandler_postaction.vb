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
                MenuNavigationHelper.Navigate(data.PageType, data.Parameters)
            End If
            Await Task.CompletedTask
        End Function
'}]}
    End Class
End Namespace
