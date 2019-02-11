Imports Param_RootNamespace.Services

Namespace Activation
    Friend Class SchemeActivationHandler
        Inherits ActivationHandler(Of ProtocolActivatedEventArgs)

        Protected Overrides Function CanHandleInternal(args As ProtocolActivatedEventArgs) As Boolean
            ' If your app has multiple handlers of ProtocolActivationEventArgs
            ' use this method to determine which to use. (possibly checking args.Uri.Scheme)
            Return True
        End Function
    End Class
End Namespace
