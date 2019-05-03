'{[{
Imports Param_RootNamespace.ViewModels
'}]}
Namespace Services

    Friend Class SuspendAndResumeService
        Inherits ActivationHandler(Of LaunchActivatedEventArgs)

        Private Const StateFilename As String = "SuspendAndResumeState"

        '{[{
        ' TODO WTS: Subscribe to the OnBackgroundEntering and OnDataRestored events from your current Page to save and restore the current App data.
        ' Only one Page should subscribe to OnBackgroundEntering and OnDataRestored at a time, as the App will navigate to that Page on resume.
        Public Event OnBackgroundEntering As EventHandler(Of SuspendAndResumeArgs)

        Public Event OnDataRestored As EventHandler(Of SuspendAndResumeArgs)

        ' TODO WTS: Subscribe to the OnResuming event from the current Page's ViewModel
        ' if you need to refresh online data when the App resumes without being terminated.
        Public Event OnResuming As EventHandler
        '}]}

        Protected Overrides Async Function HandleInternalAsync(args As LaunchActivatedEventArgs) As Task
            '^^
            '{[{
            If saveState?.Target IsNot Nothing Then
                ViewModelLocator.Current.NavigationService.Navigate(saveState.Target.FullName, saveState.SuspensionState)
            End If
            '}]}
        End Function
    End Class
End Namespace
