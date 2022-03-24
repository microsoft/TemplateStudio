Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Core.Models
Imports Param_RootNamespace.Core.Services
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.ViewModels
Imports Windows.Storage

Namespace Services
    Public Class UserDataService
        Private Const _userSettingsKey As String = "IdentityUser"
        Private _user As UserViewModel

        Private ReadOnly Property IdentityService As IdentityService
            Get
                Return Singleton(Of IdentityService).Instance
            End Get
        End Property

        Private ReadOnly Property MicrosoftGraphService As MicrosoftGraphService
            Get
                Return Singleton(Of MicrosoftGraphService).Instance
            End Get
        End Property

        Public Event UserDataUpdated As EventHandler(Of UserViewModel)

        Public Sub New()
        End Sub

        Public Sub Initialize()
            AddHandler IdentityService.LoggedIn, AddressOf OnLoggedIn
            AddHandler IdentityService.LoggedOut, AddressOf OnLoggedOut
        End Sub

        Public Async Function GetUserAsync() As Task(Of UserViewModel)
            If _user Is Nothing Then
                _user = Await GetUserFromCacheAsync()

                If _user Is Nothing Then
                    _user = GetDefaultUserData()
                End If
            End If

            Return _user
        End Function

        Private Async Sub OnLoggedIn(sender As Object, e As EventArgs)
            _user = Await GetUserFromGraphApiAsync()
            RaiseEvent UserDataUpdated(Me, _user)
        End Sub

        Private Async Sub OnLoggedOut(sender As Object, e As EventArgs)
            _user = Nothing
            Await ApplicationData.Current.LocalFolder.SaveAsync(Of User)(_userSettingsKey, Nothing)
        End Sub

        Private Async Function GetUserFromCacheAsync() As Task(Of UserViewModel)
            Dim cacheData = Await ApplicationData.Current.LocalFolder.ReadAsync(Of User)(_userSettingsKey)
            Return Await GetUserViewModelFromData(cacheData)
        End Function

        Private Async Function GetUserFromGraphApiAsync() As Task(Of UserViewModel)
            Dim accessToken = Await IdentityService.GetAccessTokenForGraphAsync()

            If String.IsNullOrEmpty(accessToken) Then
                Return Nothing
            End If

            Dim userData = Await MicrosoftGraphService.GetUserInfoAsync(accessToken)

            If userData IsNot Nothing Then
                userData.Photo = Await MicrosoftGraphService.GetUserPhoto(accessToken)
                Await ApplicationData.Current.LocalFolder.SaveAsync(_userSettingsKey, userData)
            End If

            Return Await GetUserViewModelFromData(userData)
        End Function

        Private Async Function GetUserViewModelFromData(userData As User) As Task(Of UserViewModel)
            If userData Is Nothing Then
                Return Nothing
            End If

            Dim userPhoto = If(String.IsNullOrEmpty(userData.Photo), ImageHelper.ImageFromAssetsFile("DefaultIcon.png"), Await ImageHelper.ImageFromStringAsync(userData.Photo))
            Return New UserViewModel() With {
                .Name = userData.DisplayName,
                .UserPrincipalName = userData.UserPrincipalName,
                .Photo = userPhoto
            }
        End Function

        Private Function GetDefaultUserData() As UserViewModel
            Return New UserViewModel() With {
                .Name = IdentityService.GetAccountUserName(),
                .Photo = ImageHelper.ImageFromAssetsFile("DefaultIcon.png")
            }
        End Function
    End Class
End Namespace
