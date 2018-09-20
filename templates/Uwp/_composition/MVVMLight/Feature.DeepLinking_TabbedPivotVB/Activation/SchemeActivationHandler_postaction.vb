'{[{
Imports Param_ItemNamespace.Views
'}]}
Namespace Activation    
    Friend Class SchemeActivationHandler        
        Inherits ActivationHandler(Of ProtocolActivatedEventArgs)
        '{[{

        Private ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return CommonServiceLocator.ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
            End Get
        End Property

        ' By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
        Protected Overrides Async Function HandleInternalAsync(args As ProtocolActivatedEventArgs) As Task
            ' Create data from activation Uri in ProtocolActivatedEventArgs
            Dim data = New SchemeActivationData(args.Uri)
            If data.IsValid Then
                Dim frame = TryCast(Window.Current.Content, Frame)
                Dim pivotPage = TryCast(frame.Content, PivotPage)
                If pivotPage IsNot Nothing Then
                    Await pivotPage.InitializeFromSchemeActivationAsync(data)
                Else
                    NavigationService.Navigate(GetType(ViewModels.PivotViewModel).FullName, data)
                End If
            ElseIf args.PreviousExecutionState <> ApplicationExecutionState.Running Then
                NavigationService.Navigate(GetType(ViewModels.PivotViewModel).FullName)
            End If
            Await Task.CompletedTask
        End Function
        '}]}
    End Class
End Namespace