Imports Param_RootNamespace.Views
Imports Param_RootNamespace.Services

Imports Windows.ApplicationModel.Activation
Imports Windows.ApplicationModel.Core
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace Activation
    Friend Class CommandLineActivationHandler
        Inherits ActivationHandler(Of CommandLineActivatedEventArgs)

        ' Learn more about these EventArgs at https://docs.microsoft.com/en-us/uwp/api/windows.applicationmodel.activation.commandlineactivatedeventargs
        Protected Overrides Async Function HandleInternalAsync(args As CommandLineActivatedEventArgs) As Task
            Dim operation As CommandLineActivationOperation = args.Operation

            ' Because these are supplied by the caller, they should be treated as untrustworthy.
            Dim cmdLineString As String = operation.Arguments

            ' The directory where the command-line activation request was made.
            ' This is typically not the install location of the app itself, but could be any arbitrary path.
            Dim activationPath As String = operation.CurrentDirectoryPath

            ' TODO WTS: parse the cmdLineString to determine what to do.
            ' If doing anything async, get a deferral first.
            ' Using deferral = operation.GetDeferral()
            '     Await ParseCmdString(cmdLineString, activationPath)
            ' End Using
            '
            ' If the arguments warrant showing a different view on launch, that can be done here.
            ' NavigationService.Navigate(GetType(CmdLineActivationSamplePage), cmdLineString)
            ' If you do nothing, the app will launch like normal.

            Await Task.CompletedTask
        End Function

        Protected Overrides Function CanHandleInternal(args As CommandLineActivatedEventArgs) As Boolean
            ' Only handle a commandline launch if arguments are passed.
            Return If(args?.Operation.Arguments.Any(), False)
        End Function
    End Class
End Namespace
