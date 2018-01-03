Imports Param_ItemNamespace.Views
Imports Param_ItemNamespace.Helpers

Namespace Services
    Public NotInheritable Class FirstRunDisplayService
        Private Sub New()
        End Sub

        Friend Shared Async Function ShowIfAppropriateAsync() As Task
            Dim hasShownFirstRun As Boolean = False
            hasShownFirstRun = Await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync(Of Boolean)(nameof(hasShownFirstRun))

            If Not hasShownFirstRun Then
                Await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(hasShownFirstRun), True)
                Dim dialog = New FirstRunDialog()
                Await dialog.ShowAsync()
            End If
        End Function
    End Class
End Namespace
