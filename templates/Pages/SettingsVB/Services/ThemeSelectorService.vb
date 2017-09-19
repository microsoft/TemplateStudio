Imports System.Threading.Tasks

Imports Windows.Storage
Imports Windows.UI.Xaml

Imports Param_RootNamespace.Helpers

Namespace Services
    Public NotInheritable Class ThemeSelectorService
        Private Sub New()
        End Sub
        Private Const SettingsKey As String = "RequestedTheme"

        Public Shared Property Theme As ElementTheme = ElementTheme.Default

        Public Shared Function InitializeAsync() As Task
            Theme = Await LoadThemeFromSettingsAsync()
		End Function

        Public Shared Function SetThemeAsync(theme As ElementTheme) As Task
            Me.Theme = theme

            SetRequestedTheme()
            Await SaveThemeInSettingsAsync(Me.Theme)
        End Function

        Public Shared Sub SetRequestedTheme()
            Dim frameworkElement = TryCast(Window.Current.Content, FrameworkElement)
            If frameworkElement IsNot Nothing Then
                frameworkElement.RequestedTheme = Theme
            End If
        End Sub

        Private Shared Function LoadThemeFromSettingsAsync() As Task(Of ElementTheme)
            Dim cacheTheme As ElementTheme = ElementTheme.Default
            Dim themeName As String = Await ApplicationData.Current.LocalSettings.ReadAsync(Of String)(SettingsKey)

			If Not String.IsNullOrEmpty(themeName) Then
                [Enum].TryParse(themeName, cacheTheme)
            End If

            Return cacheTheme
        End Function

        Private Shared Function SaveThemeInSettingsAsync(theme As ElementTheme) As Task
            Await ApplicationData.Current.LocalSettings.SaveAsync(SettingsKey, theme.ToString)
        End Function
    End Class
End Namespace
