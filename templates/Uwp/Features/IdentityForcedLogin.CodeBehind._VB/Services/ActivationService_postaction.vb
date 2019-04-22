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

        Public Sub New(app As App, defaultNavItem As Type, Optional shell As Lazy(Of UIElement) = Nothing)
'^^
'{[{
            AddHandler IdentityService.LoggedIn, AddressOf OnLoggedIn
'}]}
        End Sub

        Public Async Function ActivateAsync(activationArgs As Object) As Task
            If IsInteractive(activationArgs) Then
                Await InitializeAsync()
'{[{
                UserDataService.Initialize()
                IdentityService.InitializeWithAadAndPersonalMsAccounts()
                Dim silentLoginSuccess = Await IdentityService.AcquireTokenSilentAsync()

                If Not silentLoginSuccess OrElse Not IdentityService.IsAuthorized() Then
                    Await RedirectLoginPageAsync()
                End If
'}]}
            End If
'{--{
            Await HandleActivationAsync(activationArgs)
'}--}
'^^
'{[{

            If IdentityService.IsLoggedIn() Then
                Await HandleActivationAsync(activationArgs)
            End If
'}]}

            _lastActivationArgs = activationArgs
        End Function

'{[{

        Private Async Sub OnLoggedIn(sender As Object, e As EventArgs)
            If _shell?.Value IsNot Nothing Then
                Window.Current.Content = _shell.Value
            Else
                Dim frame = New Frame()
                Window.Current.Content = frame
                NavigationService.Frame = frame
            End If

            Await ThemeSelectorService.SetRequestedThemeAsync()
            Await HandleActivationAsync(_lastActivationArgs)
        End Sub

        Public Async Function RedirectLoginPageAsync() As Task
            Dim frame = New Frame()
            NavigationService.Frame = frame
            Window.Current.Content = frame
            Await ThemeSelectorService.SetRequestedThemeAsync()
            NavigationService.Navigate(GetType(Views.LogInPage))
        End Function
'}]}
    End Class
End Namespace

