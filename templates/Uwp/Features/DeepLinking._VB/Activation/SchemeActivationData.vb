Imports System.Web

Namespace Activation
    Public Class SchemeActivationData
        ' TODO WTS: Open package.appxmanifest and change the declaration for the scheme (from the default of 'wtsapp') to what you want for your app.
        ' More details about this functionality can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/features/deep-linking.md
        ' TODO WTS: Change the image in Assets/Logo.png to one for display if the OS asks the user which app to launch.
        ' Also update this protocol name with the same value as package.appxmanifest.
        Private Const ProtocolName As String = "wtsapp"

        Public Property PageType As Type

        Public Property Uri As Uri

        Public Property Parameters As Dictionary(Of String, String) = New Dictionary(Of String, String)()

        Public ReadOnly Property IsValid As Boolean
            Get
                Return PageType IsNot Nothing
            End Get
        End Property

        Public Sub New(activationUri As Uri)
            PageType = SchemeActivationConfig.GetPage(activationUri.AbsolutePath)

            If Not IsValid OrElse String.IsNullOrEmpty(activationUri.Query) Then
                Return
            End If

            Dim uriQuery = HttpUtility.ParseQueryString(activationUri.Query)

            For Each paramKey In uriQuery.AllKeys
                Parameters.Add(paramKey, uriQuery.[Get](paramKey))
            Next
        End Sub

        Public Sub New(pageType As Type, Optional parameters As Dictionary(Of String, String) = Nothing)
            Me.PageType = pageType
            Me.Parameters = parameters
            Me.Uri = BuildUri()
        End Sub

        Private Function BuildUri() As Uri
            Dim pageKey = SchemeActivationConfig.GetPageKey(PageType)
            Dim uriBuilder = New UriBuilder($"{ProtocolName}:{pageKey}")
            Dim query = HttpUtility.ParseQueryString(String.Empty)

            For Each parameter In Parameters
                query.[Set](parameter.Key, parameter.Value)
            Next

            uriBuilder.Query = query.ToString()
            Return New Uri(uriBuilder.ToString())
        End Function
    End Class
End Namespace