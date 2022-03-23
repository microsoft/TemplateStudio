'{[{
Imports Param_RootNamespace.Core.Helpers
'}]}
Namespace Services
    Friend Class ActivationService
        Private Iterator Function GetActivationHandlers() As IEnumerable(Of ActivationHandler)
            '{[{
            Yield Singleton(Of SchemeActivationHandler).Instance
            '}]}
'{--{
            Return
'}--}
        End Function
    End Class
End Namespace
