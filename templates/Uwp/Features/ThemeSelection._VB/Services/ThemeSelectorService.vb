Imports Windows.ApplicationModel.Core
Imports Windows.Storage
Imports Windows.UI.Core

Imports Param_RootNamespace.Helpers

Namespace Services
    Public NotInheritable Class ThemeSelectorService
        Private Sub New()
        End Sub

        Private Const SettingsKey As String = "AppBackgroundRequestedTheme"

        Public Shared Property Theme As ElementTheme = ElementTheme.Default

        Public Shared Async Function InitializeAsync() As Task
            Theme = Await LoadThemeFromSettingsAsync()
        End Function

        Public Shared Async Function SetThemeAsync(theme As ElementTheme) As Task
            ThemeSelectorService.Theme = theme

            Await SetRequestedThemeAsync()
            Await SaveThemeInSettingsAsync(Theme)
        End Function

        Public Shared Async Function SetRequestedThemeAsync() As Task
            For Each view In CoreApplication.Views
                Await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                    Sub()
                        Dim frameworkElement = TryCast(Window.Current.Content, FrameworkElement)
                        If frameworkElement IsNot Nothing Then
                            frameworkElement.RequestedTheme = Theme
                        End If
                    End Sub)
            Next
        End Function

        Private Shared Async Function LoadThemeFromSettingsAsync() As Task(Of ElementTheme)
            Dim cacheTheme As ElementTheme = ElementTheme.[Default]
            Dim themeName As String = Await ApplicationData.Current.LocalSettings.ReadAsync(Of String)(SettingsKey)

            If Not String.IsNullOrEmpty(themeName) Then
                [Enum].TryParse(themeName, cacheTheme)
            End If

            Return cacheTheme
        End Function

        Private Shared Async Function SaveThemeInSettingsAsync(theme As ElementTheme) As Task
            Await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString())
        End Function
    End Class
End Namespace
