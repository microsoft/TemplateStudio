'{[{
Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Core.Services
'}]}

Namespace Services
    Friend Class ActivationService
        Private _lastActivationArgs As Object
        '{[{

        Private ReadOnly Property IdentityService As IdentityService
            Get
                Return Singleton(Of IdentityService).Instance
            End Get
        End Property

        Private ReadOnly Property UserDataService As UserDataService
            Get
                Return Singleton(Of UserDataService).Instance
            End Get
        End Property
        '}]}

        Public Async Function ActivateAsync(activationArgs As Object) As Task
            If IsInteractive(activationArgs) Then
                Await InitializeAsync()
                '{[{
                UserDataService.Initialize()
                IdentityService.InitializeWithAadAndPersonalMsAccounts()
                Await IdentityService.AcquireTokenSilentAsync()
                '}]}
            End If

        End Function
    End Class
End Namespace
