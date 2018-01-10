Imports System.Threading.Tasks

Namespace Activation

    Friend Class WebToAppLinkActivationHandler
        Inherits ActivationHandler(Of ProtocolActivatedEventArgs)

        ' See detailed documentation and samples about Web to App link
        ' https://docs.microsoft.com/en-us/windows/uwp/launch-resume/web-to-app-linking
        ' https://blogs.windows.com/buildingapps/2016/10/14/web-to-app-linking-with-appurihandlers/
        '
        ' TODO WTS: You must to update the Host Uri also on Package.appxmanifest XML (Right click > View Code)
        Private Const Host As String = "myapp.website.com"

        Private Const Section1 As String = "/MySection1"

        Private Const Section2 As String = "/MySection2"

        Protected Overrides Async Function HandleInternalAsync(args As ProtocolActivatedEventArgs) As Task
            ' TODO WTS: Use args.Uri.AbsolutePath to determinate the page you want to launch the application.
            ' Open the page in app that is equivalent to the section on the website.
            Select Case args.Uri.AbsolutePath
                Case Section1
                    ' Use NavigationService to Navigate to MySection1Page
                    Exit Select
                Case Section2
                    ' Use NavigationService to Navigate to MySection2Page
                    Exit Select
                Case Else
                    ' Launch the application with default page.
                    ' Use NavigationService to Navigate to MainPage
                    Exit Select
            End Select

            Await Task.CompletedTask
        End Function

        Protected Overrides Function CanHandleInternal(args As ProtocolActivatedEventArgs) As Boolean
            Return args?.Uri?.Host = Host
        End Function
    End Class
End Namespace
