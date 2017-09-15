Imports Windows.ApplicationModel.Activation
Imports Windows.UI.Xaml
Imports wts.DefaultProject.Services

''' <summary>
''' Provides application-specific behavior to supplement the default Application class.
''' </summary>
NotInheritable Partial Class App
    Inherits Application
    Private _activationService As Lazy(Of ActivationService)
    Private ReadOnly Property ActivationService() As ActivationService
        Get
            Return _activationService.Value
        End Get
    End Property

    ''' <summary>
    ''' Initializes the singleton application object.  This is the first line of authored code
    ''' executed, and as such is the logical equivalent of main() or WinMain().
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        'Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
        _activationService = New Lazy(Of ActivationService)(AddressOf CreateActivationService)
    End Sub

    ''' <summary>
    ''' Invoked when the application is launched normally by the end user.  Other entry points
    ''' will be used such as when the application is launched to open a specific file.
    ''' </summary>
    ''' <param name="args">Details about the launch request and process.</param>
    Protected Overrides Async Sub OnLaunched(args As LaunchActivatedEventArgs)
        If Not args.PrelaunchActivated Then
            Await ActivationService.ActivateAsync(args)
        End If
    End Sub

    ''' <summary>
    ''' Invoked when the application is activated by some means other than normal launching.
    ''' </summary>
    ''' <param name="args">Event data for the event.</param>
    Protected Overrides Async Sub OnActivated(args As IActivatedEventArgs)
        Await ActivationService.ActivateAsync(args)
    End Sub
End Class
