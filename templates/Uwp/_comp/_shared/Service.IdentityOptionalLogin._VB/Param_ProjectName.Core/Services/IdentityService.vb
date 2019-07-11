Imports System.Configuration
Imports System.Linq
Imports System.Net.NetworkInformation
Imports System.Threading.Tasks
Imports Microsoft.Identity.Client
Imports Param_RootNamespace.Core.Helpers

Namespace Services
    Public Class IdentityService
        ' For more information about using Identity, see
        ' https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/identity.md
        '
        ' Read more about Microsoft Identity Client here
        ' https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki
        ' https://docs.microsoft.com/azure/active-directory/develop/v2-overview
        Private ReadOnly _scopes As String() = {"user.read"}
        Private _integratedAuthAvailable As Boolean
        Private _client As IPublicClientApplication
        Private _authenticationResult As AuthenticationResult

        ' TODO WTS: The IdentityClientId in App.config is provided to test the project in development environments.
        ' Please, follow these steps to create a new one with Azure Active Directory and replace it before going to production.
        ' https://docs.microsoft.com/azure/active-directory/develop/quickstart-register-app
        Private _clientId As String = ConfigurationManager.AppSettings("IdentityClientId")

        Public Event LoggedIn As EventHandler

        Public Event LoggedOut As EventHandler

        Public Sub InitializeWithAadAndPersonalMsAccounts()
            _integratedAuthAvailable = False
            _client = PublicClientApplicationBuilder.Create(_clientId).WithAuthority(AadAuthorityAudience.AzureAdAndPersonalMicrosoftAccount).Build()
        End Sub

        Public Sub InitializeWithAadMultipleOrgs(Optional integratedAuth As Boolean = False)
            _integratedAuthAvailable = integratedAuth
            _client = PublicClientApplicationBuilder.Create(_clientId).WithAuthority(AadAuthorityAudience.AzureAdMultipleOrgs).Build()
        End Sub

        Public Sub InitializeWithAadSingleOrg(tenant As String, Optional integratedAuth As Boolean = False)
            _integratedAuthAvailable = integratedAuth
            _client = PublicClientApplicationBuilder.Create(_clientId).WithAuthority(AzureCloudInstance.AzurePublic, tenant).Build()
        End Sub

        Public Function IsLoggedIn() As Boolean
            Return _authenticationResult IsNot Nothing
        End Function

        Public Async Function LoginAsync() As Task(Of LoginResultType)
            If Not NetworkInterface.GetIsNetworkAvailable() Then
                Return LoginResultType.NoNetworkAvailable
            End If

            Try
                Dim accounts = Await _client.GetAccountsAsync()
                _authenticationResult = Await _client.AcquireTokenInteractive(_scopes).WithAccount(accounts.FirstOrDefault()).ExecuteAsync()
                RaiseEvent LoggedIn(Me, EventArgs.Empty)
                Return LoginResultType.Success
            Catch ex As MsalClientException

                If ex.ErrorCode = "authentication_canceled" Then
                    Return LoginResultType.CancelledByUser
                End If

                Return LoginResultType.UnknownError
            Catch __unusedException2__ As Exception
                Return LoginResultType.UnknownError
            End Try
        End Function

        Public Function IsAuthorized() As Boolean
            ' TODO WTS: You can also add extra authorization checks here.
            ' i.e.: Checks permisions of _authenticationResult.Account.Username in a database.
            Return True
        End Function

        Public Function GetAccountUserName() As String
            Return _authenticationResult?.Account?.Username
        End Function

        Public Async Function LogoutAsync() As Task
            Try
                Dim accounts = Await _client.GetAccountsAsync()
                Dim account = accounts.FirstOrDefault()

                If account IsNot Nothing Then
                    Await _client.RemoveAsync(account)
                End If

                _authenticationResult = Nothing
                RaiseEvent LoggedOut(Me, EventArgs.Empty)
            Catch ex As MsalException
                ' TODO WTS: LogoutAsync can fail please handle exceptions as appropriate to your scenario
                ' For more info on MsalExceptions see
                ' https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/exceptions
            End Try
        End Function

        Public Async Function GetAccessTokenAsync() As Task(Of String)
            Dim acquireTokenSuccess = Await AcquireTokenSilentAsync()

            If acquireTokenSuccess Then
                Return _authenticationResult.AccessToken
            Else
                Try
                    ' Interactive authentication is required
                    Dim accounts = Await _client.GetAccountsAsync()
                    _authenticationResult = Await _client.AcquireTokenInteractive(_scopes).WithAccount(accounts.FirstOrDefault()).ExecuteAsync()
                    Return _authenticationResult.AccessToken
                Catch ex As MsalException
                    ' AcquireTokenSilent and AcquireTokenInteractive failed, the session will be closed.
                    _authenticationResult = Nothing
                    RaiseEvent LoggedOut(Me, EventArgs.Empty)
                    Return String.Empty
                End Try
            End If
        End Function

        Public Async Function AcquireTokenSilentAsync() As Task(Of Boolean)
            If Not NetworkInterface.GetIsNetworkAvailable() Then
                Return False
            End If

            Dim retryWithUI As Boolean = False

            Try
                Dim accounts = Await _client.GetAccountsAsync()
                _authenticationResult = Await _client.AcquireTokenSilent(_scopes, accounts.FirstOrDefault()).ExecuteAsync()
                Return True
            Catch ex As MsalUiRequiredException

                If _integratedAuthAvailable Then
                    retryWithUI = True
                Else
                    ' Interactive authentication is required
                    Return False
                End If

            Catch ex As MsalException
                ' TODO WTS: Silentauth failed, please handle this exception as appropriate to your scenario
                ' For more info on MsalExceptions see
                ' https://github.com/AzureAD/microsoft-authentication-library-for-dotnet/wiki/exceptions
                Return False
            End Try

            If retryWithUI Then
                Try
                    _authenticationResult = Await _client.AcquireTokenByIntegratedWindowsAuth(_scopes).ExecuteAsync()
                    Return True
                Catch ex As MsalUiRequiredException
                    ' Interactive authentication is required
                    Return False
                End Try
            Else
                Return false
            End If
        End Function
    End Class
End Namespace
