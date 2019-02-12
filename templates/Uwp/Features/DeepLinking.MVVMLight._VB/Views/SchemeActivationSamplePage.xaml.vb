Imports Param_RootNamespace.ViewModels

Namespace Views
    ' TODO WTS: Remove this sample page when/if it's not needed.
    ' This page is an sample of how to launch a specific page in response to a protocol launch and pass it a value.
    ' It is expected that you will delete this page once you have changed the handling of a protocol launch to meet
    ' your needs and redirected to another of your pages.
    Public NotInheritable Partial Class SchemeActivationSamplePage
        Inherits Page

        Private ReadOnly Property ViewModel As SchemeActivationSampleViewModel
            Get
                Return ViewModelLocator.Current.SchemeActivationSampleViewModel
            End Get
        End Property

        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            If e.Parameter IsNot Nothing Then
                Dim parameters = TryCast(e.Parameter, Dictionary(Of String, String))

                If parameters IsNot Nothing Then
                    ViewModel.Initialize(parameters)
                End If
            End If
        End Sub
    End Class
End Namespace
