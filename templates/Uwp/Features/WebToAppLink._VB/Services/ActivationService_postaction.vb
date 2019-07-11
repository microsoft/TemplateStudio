'{[{
Imports Param_RootNamespace.Core.Helpers
'}]}

Namespace Services
    Friend Class ActivationService
        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
'{[{
            Yield Singleton(Of WebToAppLinkActivationHandler).Instance
'}]}
        End Function
    End Class
End Namespace
