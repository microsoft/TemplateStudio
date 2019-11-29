Imports Param_RootNamespace.Views

Imports Microsoft.Toolkit.Uwp.Helpers

Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core

Namespace Services
    ' For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/features/whats-new-prompt.md
    Public Module WhatsNewDisplayService
        Dim shown As Boolean = False

        Friend Async Function ShowIfAppropriateAsync() As Task
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            Async Sub()
                If SystemInformation.IsAppUpdated AndAlso Not shown Then
                    shown = True
                    Dim dialog = New WhatsNewDialog()
                    Await dialog.ShowAsync()
                End If
            End Sub)
        End Function
    End Module
End Namespace
