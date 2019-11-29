Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services

Namespace Views
    ' TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/UWP/pages/settings-codebehind.vb.md
    ' TODO WTS: Change the URL for your privacy policy in the Resource File, currently set to https://YourPrivacyUrlGoesHere
    Public NotInheritable Partial Class SettingsPagePage
        Inherits Page
        Implements System.ComponentModel.INotifyPropertyChanged

        Private _elementTheme As ElementTheme = ThemeSelectorService.Theme

        Public Property ElementTheme As ElementTheme
            Get
                Return _elementTheme
            End Get

            Set
                [Param_Setter](_elementTheme, value)
            End Set
        End Property

        Private _versionDescription As String

        Public Property VersionDescription As String
            Get
                Return _versionDescription
            End Get

            Set
                [Param_Setter](_versionDescription, value)
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            Await InitializeAsync()
        End Sub

        Public Async Function InitializeAsync() As Task
            VersionDescription = GetVersionDescription()
            Await Task.CompletedTask
        End Function

        Private Function GetVersionDescription() As String
            Dim appName = "AppDisplayName".GetLocalized()
            Dim package = Windows.ApplicationModel.Package.Current
            Dim packageId = package.Id
            Dim version = packageId.Version

            Return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}"
        End Function

        Private Async Sub ThemeChanged_CheckedAsync(sender As Object, e As RoutedEventArgs)
            Dim param = TryCast(sender, RadioButton)

            If param IsNot Nothing AndAlso param.CommandParameter IsNot Nothing Then
                Await ThemeSelectorService.SetThemeAsync(DirectCast(param.CommandParameter, ElementTheme))
            End If
        End Sub
    End Class
End Namespace
