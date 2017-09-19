Imports Param_ItemNamespace.Views
Imports Param_ItemNamespace.Helpers
Imports System.Threading.Tasks
Imports Windows.ApplicationModel

Namespace Services
    ' For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/tree/master/docs/features/whats-new-prompt.md
    Public Class WhatsNewDisplayService
        Friend Shared Async Function ShowIfAppropriateAsync() As Task
            Dim currentVersion = PackageVersionToReadableString(Package.Current.Id.Version)
            Dim lastVersion = Await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync(Of String)(NameOf(currentVersion))

            If lastVersion Is Nothing Then
                Await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(NameOf(currentVersion), currentVersion)
            Else
                If currentVersion <> lastVersion Then
                    Await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(NameOf(currentVersion), currentVersion)

                    Dim dialog = New WhatsNewDialog
                    Await dialog.ShowAsync()
                End If
            End If
        End Function

        Private Shared Function PackageVersionToReadableString(packageVersion As PackageVersion) As String
            Return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}"
        End Function
    End Class
End Namespace
