Imports Param_ItemNamespace.Views
Imports Param_ItemNamespace.Helpers

Namespace Services
    ' For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/tree/master/docs/features/whats-new-prompt.md
    Public Module WhatsNewDisplayService
        Friend Async Function ShowIfAppropriateAsync() As Task
            Dim currentVersion = PackageVersionToReadableString(Package.Current.Id.Version)

            Dim lastVersion = Await Windows.Storage.ApplicationData.Current.LocalSettings.ReadAsync(Of String)(nameof(currentVersion))

            If lastVersion Is Nothing Then
                Await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(currentVersion), currentVersion)
            Else
                If currentVersion <> lastVersion Then
                    Await Windows.Storage.ApplicationData.Current.LocalSettings.SaveAsync(nameof(currentVersion), currentVersion)

                    Dim dialog = New WhatsNewDialog()
                    Await dialog.ShowAsync()
                End If
            End If
        End Function

        Private Function PackageVersionToReadableString(packageVersion As PackageVersion) As String
            Return $"{packageVersion.Major}.{packageVersion.Minor}.{packageVersion.Build}.{packageVersion.Revision}"
        End Function
    End Module
End Namespace
