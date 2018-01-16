Imports Windows.Storage

Imports Param_RootNamespace.Activation
Imports Param_RootNamespace.Helpers

Namespace Services
    Friend Class SuspendAndResumeService
        Inherits ActivationHandler(Of LaunchActivatedEventArgs)

        ' TODO WTS: For more information regarding the application lifecycle and how to handle suspend and resume, please see:
        ' Documentation: https://docs.microsoft.com/windows/uwp/launch-resume/app-lifecycle

        Private Const StateFilename As String = "SuspendAndResumeState"

        ' TODO WTS: This event is fired just before the app enters in background. Subscribe to this event if you want to save your current state.
        Public Event OnBackgroundEntering As EventHandler(Of OnBackgroundEnteringEventArgs)

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

        Protected Overrides Async Function HandleInternalAsync(args As LaunchActivatedEventArgs) As Task
            Await RestoreStateAsync()
        End Function

        Protected Overrides Function CanHandleInternal(args As LaunchActivatedEventArgs) As Boolean
            Return args.PreviousExecutionState = ApplicationExecutionState.Terminated
        End Function

        Private Async Function RestoreStateAsync() As Task
            Dim saveState = Await ApplicationData.Current.LocalFolder.ReadAsync(Of OnBackgroundEnteringEventArgs)(StateFilename)
        End Function
    End Class
End Namespace
