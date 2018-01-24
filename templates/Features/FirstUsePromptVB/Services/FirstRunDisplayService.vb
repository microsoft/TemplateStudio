Imports Param_ItemNamespace.Views
Imports Microsoft.Toolkit.Uwp.Helpers

Namespace Services
    Public NotInheritable Class FirstRunDisplayService
        Shared shown As Boolean = false
        Private Sub New()
        End Sub

        Friend Shared Async Function ShowIfAppropriateAsync() As Task
            If SystemInformation.IsFirstRun And Not shown Then
                shown = true
                Dim dialog = New FirstRunDialog()
                Await dialog.ShowAsync()
            End If
        End Function
    End Class
End Namespace
