Imports Param_ItemNamespace.Views
Imports Param_ItemNamespace.Helpers
Imports System.Threading.Tasks
Imports Windows.ApplicationModel

Namespace Services
    Public Class FirstRunDisplayService
        Friend Shared Function ShowIfAppropriateAsync() As Task
            Dim hasShownFirstRun As Boolean = False
            hasShownFirstRun = Await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync(Of Boolean)(NameOf(hasShownFirstRun))

			If Not hasShownFirstRun Then
                Await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(NameOf(hasShownFirstRun), True)
                Dim dialog = New FirstRunDialog()
                Await dialog.ShowAsync()
            End If
        End Function
    End Class
End Namespace
