Imports Param_RootNamespace.Services

Namespace Activation
    ' TODO WTS: Open package.appxmanifest and change the declaration for the scheme (from the default of 'wtsapp') to what you want for your app.
    ' More details about this functionality can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/uri-scheme.md
    ' TODO WTS: Change the image in Assets/Logo.png to one for display if the OS asks the user which app to launch.
    Friend Class SchemeActivationHandler
        Inherits ActivationHandler(Of ProtocolActivatedEventArgs)

        Protected Overrides Function CanHandleInternal(args As ProtocolActivatedEventArgs) As Boolean
            ' If your app has multiple handlers of ProtocolActivationEventArgs
            ' use this method to determine which to use. (possibly checking args.Uri.Scheme)
            Return True
        End Function
    End Class
End Namespace
