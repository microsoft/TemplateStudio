
Imports System
Imports System.Collections.Generic
Imports System.Net.Http
Imports System.Text
Imports System.Threading.Tasks
Imports Newtonsoft.Json

Namespace Core.Services
    ' This class provides a wrapper around common functionality for HTTP actions.
    ' Learn more at https://docs.microsoft.com/windows/uwp/networking/httpclient
    Public Class HttpDataService
        Private client As HttpClient
        Private ReadOnly responseCache As Dictionary(Of String, Object)

        Public Sub New(Optional defaultBaseUrl As String = "")
            client = New HttpClient()

            If Not String.IsNullOrEmpty(defaultBaseUrl) Then
                client.BaseAddress = New Uri($"{defaultBaseUrl}/")
            End If

            responseCache = New Dictionary(Of String, Object)()
        End Sub

        Public Async Function GetAsync(Of T)(uri As String, Optional forceRefresh As Boolean = False) As Task(Of T)
            Dim result As T = Nothing

            ' The responseCache is a simple store of past responses to avoid unnecessary requests for the same resource.
            ' Feel free to remove it or extend this request logic as appropraite for your app.
            If forceRefresh OrElse Not responseCache.ContainsKey(uri) Then
                Dim json = Await client.GetStringAsync(uri)
                result = Await Task.Run(Function() JsonConvert.DeserializeObject(Of T)(json))

                If responseCache.ContainsKey(uri) Then
                    responseCache(uri) = result
                Else
                    responseCache.Add(uri, result)
                End If
            Else
                result = CType(responseCache(uri), T)
            End If

            Return result
        End Function

        Public Async Function PostAsync(Of T)(uri As String, item As T) As Task(Of Boolean)
            If item Is Nothing Then
                Return False
            End If

            Dim serializedItem = JsonConvert.SerializeObject(item)
            Dim buffer = Encoding.UTF8.GetBytes(serializedItem)
            Dim byteContent = New ByteArrayContent(buffer)
            Dim response = Await client.PostAsync(uri, byteContent)
            Return response.IsSuccessStatusCode
        End Function

        Public Async Function PostAsJsonAsync(Of T)(uri As String, item As T) As Task(Of Boolean)
            If item Is Nothing Then
                Return False
            End If

            Dim serializedItem = JsonConvert.SerializeObject(item)
            Dim response = Await client.PostAsync(uri, New StringContent(serializedItem, Encoding.UTF8, "application/json"))
            Return response.IsSuccessStatusCode
        End Function

        Public Async Function PutAsync(Of T)(uri As String, item As T) As Task(Of Boolean)
            If item Is Nothing Then
                Return False
            End If

            Dim serializedItem = JsonConvert.SerializeObject(item)
            Dim buffer = Encoding.UTF8.GetBytes(serializedItem)
            Dim byteContent = New ByteArrayContent(buffer)
            Dim response = Await client.PutAsync(uri, byteContent)
            Return response.IsSuccessStatusCode
        End Function

        Public Async Function PutAsJsonAsync(Of T)(uri As String, item As T) As Task(Of Boolean)
            If item Is Nothing Then
                Return False
            End If

            Dim serializedItem = JsonConvert.SerializeObject(item)
            Dim response = Await client.PutAsync(uri, New StringContent(serializedItem, Encoding.UTF8, "application/json"))
            Return response.IsSuccessStatusCode
        End Function

        Public Async Function DeleteAsync(uri As String) As Task(Of Boolean)
            Dim response = Await client.DeleteAsync(uri)
            Return response.IsSuccessStatusCode
        End Function
    End Class
End Namespace
