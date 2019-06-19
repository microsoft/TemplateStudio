'{[{
Imports Param_RootNamespace.ViewModels
'}]}
Namespace Services
    Friend Class ActivationService
        Private ReadOnly Property IdentityService As IdentityService
            Get
                Return Singleton(Of IdentityService).Instance
            End Get
        End Property
'{[{

        Public Shared ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return ViewModelLocator.Current.NavigationService
            End Get
        End Property
'}]}
'^^
'{[{

        Public Sub SetShell(shell As Lazy(Of UIElement))
            _shell = shell
        End Sub
'}]}
    End Class
End Namespace
