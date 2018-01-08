Namespace Activation
    Friend Class DefaultLaunchActivationHandler
        Inherits ActivationHandler(Of LaunchActivatedEventArgs)
'{[{

        Private ReadOnly _navElement As Type

        Public Sub New(navElement As Type)
            _navElement = navElement
        End Sub
'}]}
    End Class
End Namespace
