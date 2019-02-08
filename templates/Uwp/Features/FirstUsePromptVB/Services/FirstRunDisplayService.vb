Imports Param_RootNamespace.Views
Imports Microsoft.Toolkit.Uwp.Helpers

Namespace Services
    Public NotInheritable Class FirstRunDisplayService
        Shared shown As Boolean = False

        Private Sub New()
        End Sub

        Friend Shared Async Function ShowIfAppropriateAsync() As Task
            If SystemInformation.IsFirstRun AndAlso Not shown Then
                shown = true
                Dim dialog = New FirstRunDialog()
                Await dialog.ShowAsync()
            End If
        End Function
    End Class
End Namespace
