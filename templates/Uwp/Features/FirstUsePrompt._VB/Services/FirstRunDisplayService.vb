Imports Param_RootNamespace.Views

Imports Microsoft.Toolkit.Uwp.Helpers

Imports Windows.ApplicationModel.Core
Imports Windows.UI.Core

Namespace Services
    Public NotInheritable Class FirstRunDisplayService
        Shared shown As Boolean = False

        Private Sub New()
        End Sub

        Friend Shared Async Function ShowIfAppropriateAsync() As Task
            Await CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            Async Sub()
                If SystemInformation.IsFirstRun AndAlso Not shown Then
                    shown = True
                    Dim dialog = New FirstRunDialog()
                    Await dialog.ShowAsync()
                End If
            End Sub)
        End Function
    End Class
End Namespace
