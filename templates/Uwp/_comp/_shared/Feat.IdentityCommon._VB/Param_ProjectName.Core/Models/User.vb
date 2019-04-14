Imports System.Collections.Generic

Namespace Core.Models
    ' This class contains user members to download user information from Microsoft Graph
    ' https://docs.microsoft.com/graph/api/resources/user?view=graph-rest-1.0
    Public Class User
        Public Property Id As String
        Public Property BusinessPhones As List(Of String)
        Public Property DisplayName As String
        Public Property GivenName As String
        Public Property JobTitle As Object
        Public Property Mail As String
        Public Property MobilePhone As String
        Public Property OfficeLocation As Object
        Public Property PreferredLanguage As String
        Public Property Surname As String
        Public Property UserPrincipalName As String
        Public Property Photo As String
    End Class
End Namespace
