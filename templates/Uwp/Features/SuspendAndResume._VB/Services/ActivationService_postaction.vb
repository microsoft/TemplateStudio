'{**
'This code block includes the SuspendAndResumeService Instance in the method
'`GetActivationHandlers()` in the ActivationService of your project.
'**}

'{[{
Imports Param_RootNamespace.Core.Helpers
'}]}

Namespace Services
    Friend Class ActivationService
        Public Async Function ActivateAsync(activationArgs As Object) As Task
            If IsInteractive(activationArgs) Then
            End If

            If IsInteractive(activationArgs) Then
'{[{
                Dim Activation = TryCast(activationArgs, IActivatedEventArgs)
                If Activation IsNot Nothing AndAlso Activation.PreviousExecutionState = ApplicationExecutionState.Terminated Then
                    Await Singleton(Of SuspendAndResumeService).Instance.RestoreSuspendAndResumeData()
                End If

'}]}
            End If
        End Function

        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
'{[{
            Yield Singleton(Of SuspendAndResumeService).Instance
'}]}
'{--{
            Exit Function
'}--}
        End Function
    End Class
End Namespace
