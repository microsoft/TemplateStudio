'{[{
Imports Microsoft.Practices.ServiceLocation
'}]}
Namespace Services
    Friend Class SuspendAndResumeService
        Inherits ActivationHandler(Of LaunchActivatedEventArgs)

        Private Async Function RestoreStateAsync() As Task
            '^^
            '{[{
            If saveState?.Target IsNot Nothing Then
                Dim navigationService = ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
                navigationService.Navigate(saveState.Target.FullName, saveState.SuspensionState)
            End If
            '}]}
        End Function
    End Class
End Namespace
