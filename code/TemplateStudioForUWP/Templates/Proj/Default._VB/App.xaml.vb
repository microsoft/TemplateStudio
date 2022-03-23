Imports Param_RootNamespace.Services

NotInheritable Partial Class App
    Inherits Application

    Private _activationService As Lazy(Of ActivationService)

    Private ReadOnly Property ActivationService As ActivationService
        Get
            Return _activationService.Value
        End Get
    End Property

    Public Sub New()
        InitializeComponent()
        AddHandler UnhandledException, AddressOf OnAppUnhandledException

        ' Deferred execution until used. Check https://docs.microsoft.com/dotnet/api/system.lazy-1 for further info on Lazy<T> class.
        _activationService = New Lazy(Of ActivationService)(AddressOf CreateActivationService)
    End Sub

    Protected Overrides Async Sub OnLaunched(args As LaunchActivatedEventArgs)
        If Not args.PrelaunchActivated Then
            Await ActivationService.ActivateAsync(args)
        End If
    End Sub

    Protected Overrides Async Sub OnActivated(args As IActivatedEventArgs)
        Await ActivationService.ActivateAsync(args)
    End Sub

    Private Sub OnAppUnhandledException(sender As Object, e As Windows.UI.Xaml.UnhandledExceptionEventArgs)
        ' TODO WTS: Please log and handle the exception as appropriate to your scenario
        ' For more info see https://docs.microsoft.com/uwp/api/windows.ui.xaml.application.unhandledexception
    End Sub
End Class
