Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Threading.Tasks
Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Core.Models

Namespace Core.Services
    Public Class MicrosoftGraphService
        ' For more information about Get-User Service, refer to the following documentation
        ' https://docs.microsoft.com/graph/api/user-get?view=graph-rest-1.0
        ' You can test calls to the Microsoft Graph with the Microsoft Graph Explorer
        ' https://developer.microsoft.com/graph/graph-explorer
        Private Const _graphAPIEndpoint As String = "https://graph.microsoft.com/v1.0/"
        Private Const _apiServiceMe As String = "me/"
        Private Const _apiServiceMePhoto As String = "me/photo/$value"

        Public Sub New()
        End Sub

        Public Async Function GetUserInfoAsync(accessToken As String) As Task(Of User)
            Dim user As User = Nothing
            Dim httpContent = Await GetDataAsync($"{_graphAPIEndpoint}{_apiServiceMe}", accessToken)

            If httpContent IsNot Nothing Then
                Dim userData = Await httpContent.ReadAsStringAsync()

                If Not String.IsNullOrEmpty(userData) Then
                    user = Await Json.ToObjectAsync(Of User)(userData)
                End If
            End If

            Return user
        End Function

        Public Async Function GetUserPhoto(accessToken As String) As Task(Of String)
            Dim httpContent = Await GetDataAsync($"{_graphAPIEndpoint}{_apiServiceMePhoto}", accessToken)

            If httpContent Is Nothing Then
                Return String.Empty
            End If

            Dim stream = Await httpContent.ReadAsStreamAsync()
            Return stream.ToBase64String()
        End Function

        Private Async Function GetDataAsync(url As String, accessToken As String) As Task(Of HttpContent)
            Try

                Using httpClient = New HttpClient()
                    Dim request = New HttpRequestMessage(HttpMethod.[Get], url)
                    request.Headers.Authorization = New AuthenticationHeaderValue("Bearer", accessToken)
                    Dim response = Await httpClient.SendAsync(request)

                    If response.IsSuccessStatusCode Then
                        Return response.Content
                    Else
                        ' TODO WTS: Please handle other status codes as appropriate to your scenario
                    End If
                End Using

            Catch hre As HttpRequestException
                ' TODO WTS: The request failed due to an underlying issue such as
                ' network connectivity, DNS failure, server certificate validation or timeout.
                ' Please handle this exception as appropriate to your scenario
            Catch ex As Exception
                ' TODO WTS: This call can fail please handle exceptions as appropriate to your scenario
            End Try

            Return Nothing
        End Function
    End Class
End Namespace
