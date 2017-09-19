Imports System.ComponentModel
Imports Param_ItemNamespace.Services
Imports Windows.ApplicationModel
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace Views
    NotInheritable Class SettingsPagePage
        Inherits Page
        Implements INotifyPropertyChanged

        '''/ TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings-codebehind.md
        '''/ TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private _elementTheme As ElementTheme = ThemeSelectorService.Theme

        Public Property ElementTheme As ElementTheme
            Get
                Return _elementTheme
            End Get
            Set(value As ElementTheme)
                _elementTheme = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ElementTheme)))
            End Set
        End Property

        Private _versionDescription As String

        Public Property VersionDescription As String
            Get
                Return _versionDescription
            End Get
            Set(value As String)
                _versionDescription = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(VersionDescription)))
            End Set
        End Property

        Private Sub Initialize()
            VersionDescription = GetVersionDescription()
        End Sub

        Private Function GetVersionDescription() As String
            Dim package = Package.Current
            Dim packageId = package.Id
            Dim version = packageId.Version

            Return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}"
        End Function

        Private Async Sub ThemeChanged_CheckedAsync(sender As Object, e As RoutedEventArgs)
            Dim param = TryCast(sender, RadioButton)?.CommandParameter

            If param IsNot Nothing Then
                Await ThemeSelectorService.SetThemeAsync(DirectCast(param, ElementTheme))
            End If
        End Sub
    End Class
End Namespace
