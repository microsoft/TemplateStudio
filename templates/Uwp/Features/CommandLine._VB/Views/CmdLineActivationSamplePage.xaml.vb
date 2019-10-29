
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation

Namespace Views

    ' TODO WTS: Remove this sample page when/if it's not needed.
    ' This page is an sample of how to launch a specific page in response to launching from the command line and passing arguments.
    ' It is expected that you will delete this page once you have changed the handling of command line launch to meet
    ' your needs and redirected to another of your pages.
    Public NotInheritable Partial Class CmdLineActivationSamplePage
        Inherits Page

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            PassedArguments.Text = e.Parameter?.ToString()
        End Sub
    End Class
End Namespace
