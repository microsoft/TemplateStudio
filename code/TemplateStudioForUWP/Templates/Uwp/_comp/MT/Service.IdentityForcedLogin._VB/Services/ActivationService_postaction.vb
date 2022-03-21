Namespace Services
    Friend Class ActivationService
'^^
'{[{

        Public Async Function RedirectLoginPageAsync() As Task
            Dim frame = New Frame()
            NavigationService.Frame = frame
            Window.Current.Content = frame
            Await ThemeSelectorService.SetRequestedThemeAsync()
            NavigationService.Navigate(Of Views.LogInPage)()
        End Function
'}]}
    End Class
End Namespace
