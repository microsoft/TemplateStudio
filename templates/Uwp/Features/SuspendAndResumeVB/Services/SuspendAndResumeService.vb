Imports Windows.Storage

Imports Param_RootNamespace.Activation
Imports Param_RootNamespace.Helpers

Namespace Services

    ' The SuspendAndResumeService allows you to the save App data before the app is being suspended (or enters in background state).
    ' In case the App is terminated during suspension the data is restored during app launch using this ActivationHandler.
    ' In case the App is resumed without being terminated a resume event is fired that allows you to refresh App data that might
    ' be outdated (e.g data from online feed)
    ' Documentation:
    '     * How to implement and test: https://github.com/Microsoft/WindowsTemplateStudio/blob/dev/docs/features/suspend-and-resume.md
    '     * Application Lifecycle: https://docs.microsoft.com/windows/uwp/launch-resume/app-lifecycle
    Friend Class SuspendAndResumeService
        Inherits ActivationHandler(Of LaunchActivatedEventArgs)

        Private Const StateFilename As String = "SuspendAndResumeState"

        ' This method saves the application state before entering background. It fires the event OnBackgroundEntering to collect
        ' state data from the current subscriber and saves it the local storage.
        Public Async Function SaveStateAsync() As Task
            Dim suspensionState = New SuspensionState() With {
                .SuspensionDate = DateTime.Now
            }

            Dim target As Type = Nothing
            
            If OnBackgroundEnteringEvent IsNot Nothing Then
                target = OnBackgroundEnteringEvent.Target.GetType
            End If

            Dim onBackgroundEnteringArgs = New OnBackgroundEnteringEventArgs(suspensionState, target)

            RaiseEvent OnBackgroundEntering(Me, onBackgroundEnteringArgs)

            Await ApplicationData.Current.LocalFolder.SaveAsync(StateFilename, onBackgroundEnteringArgs)
        End Function

        ' This method allows subscribers to refesh data that might be outdated after App is resumed from suspension
        ' If the App was terminated during suspension this event will not fire, data restore is handled by method HandleInternalAsync
        Public Sub ResumeApp()
            RaiseEvent OnResuming(Me, EventArgs.Empty)         
        End Sub


       ' This method restores state when the App is launched after termination, it navigates to the stored Page passing the recovered state data
        Protected Overrides Async Function HandleInternalAsync(args As LaunchActivatedEventArgs) As Task
            Dim saveState = Await ApplicationData.Current.LocalFolder.ReadAsync(Of OnBackgroundEnteringEventArgs)(StateFilename)
        End Function

        Protected Overrides Function CanHandleInternal(args As LaunchActivatedEventArgs) As Boolean
            ' State has only to be restored if the App was terminated during suspension, no App data should be lost in this case
            Return args.PreviousExecutionState = ApplicationExecutionState.Terminated
        End Function

    End Class
End Namespace
