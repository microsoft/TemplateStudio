'{**
' This code block adds the code to launch the Feedback Hub from the settings page
'**}
Namespace ViewModels
    Public Class Param_SettingsPageNameViewModel
        Inherits Observable

        '{[{
        Public ReadOnly Property FeedbackLinkVisibility As Visibility
            Get
                Return If(Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported(), Visibility.Visible, Visibility.Collapsed)
            End Get
        End Property

        Private _launchFeedbackHubCommand As ICommand

        Public ReadOnly Property LaunchFeedbackHubCommand As ICommand
            Get
                If _launchFeedbackHubCommand Is Nothing Then
                    _launchFeedbackHubCommand = New RelayCommand(Async Sub() 
                            ' This launcher is part of the Store Services SDK https://docs.microsoft.com/en-us/windows/uwp/monetize/microsoft-store-services-sdk
                            Dim launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault()
                            Await launcher.LaunchAsync()
                        End Sub)
                End If

                Return _launchFeedbackHubCommand
            End Get
        End Property

        '}]}
    End Class
End Namespace
