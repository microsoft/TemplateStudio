'{[{
Imports Param_RootNamespace.Core.Helpers
'}]}

Namespace Services
    Friend Class ActivationService
        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
'{[{
            yield Singleton(Of wts.ItemNameActivationHandler).Instance
'}]}
        End Function

'^^
'{[{
        Friend Async Function ActivateFromShareTargetAsync(activationArgs As ShareTargetActivatedEventArgs) As Task
            Dim shareTargetHandler = GetActivationHandlers().FirstOrDefault(Function(h) h.CanHandle(activationArgs))
            If shareTargetHandler IsNot Nothing Then
                Await shareTargetHandler.HandleAsync(activationArgs)
            End If
        End Function
'}]}
    End Class
End Namespace
