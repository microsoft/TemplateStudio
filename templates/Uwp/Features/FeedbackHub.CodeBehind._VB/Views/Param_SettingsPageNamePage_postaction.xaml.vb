'{**
' This code block adds the code to launch the Feedback Hub from the settings page
'**}
Namespace Views
    Public NotInheritable Partial Class Param_SettingsPageNamePage
        Inherits Page
        Implements INotifyPropertyChanged

        Private Sub Initialize()
            '^^
            '{[{

            If Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.IsSupported() Then
                FeedbackLink.Visibility = Visibility.Visible
            End If
            '}]}
        End Sub

        '^^
        '{[{
        Private Async Sub FeedbackLink_Click(sender As Object, e As RoutedEventArgs)
            ' This launcher is part of the Store Services SDK https://docs.microsoft.com/en-us/windows/uwp/monetize/microsoft-store-services-sdk
            Dim launcher = Microsoft.Services.Store.Engagement.StoreServicesFeedbackLauncher.GetDefault()
            Await launcher.LaunchAsync()
        End Sub
        '}]}
    End Class
End Namespace
