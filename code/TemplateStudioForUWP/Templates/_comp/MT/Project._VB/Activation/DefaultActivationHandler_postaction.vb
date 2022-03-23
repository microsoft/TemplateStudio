Namespace Activation
    Friend Class DefaultActivationHandler
        Inherits ActivationHandler(Of IActivatedEventArgs)
'{[{

        Private ReadOnly _navElement As Type

        Public Sub New(navElement As Type)
            _navElement = navElement
        End Sub
'}]}
    End Class
End Namespace
