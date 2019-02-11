Imports Param_RootNamespace.Activation
Imports Windows.ApplicationModel.UserActivities
Imports Windows.UI

Namespace Services
    Public Class UserActivityData
        Public Property ActivityId As String

        Public Property ActivationData As SchemeActivationData

        Public Property DisplayText As String

        Public Property Description As String

        Public Property BackgroundColor As Color

        Public Sub New(activityId As String, activationData As SchemeActivationData, displayText As String, Optional backgroundColor As Color = Nothing, Optional description As String = Nothing)
            Me.ActivityId = activityId
            Me.ActivationData = activationData
            Me.DisplayText = displayText
            Me.BackgroundColor = backgroundColor
            Me.Description = If(description, String.Empty)
        End Sub

        Public Async Function ToUserActivity() As Task(Of UserActivity)
            If String.IsNullOrEmpty(ActivityId) Then
                Throw New ArgumentNullException(NameOf(ActivityId))
            ElseIf ActivationData Is Nothing Then
                Throw New ArgumentNullException(NameOf(ActivationData))
            ElseIf String.IsNullOrEmpty(DisplayText) Then
                Throw New ArgumentNullException(NameOf(DisplayText))
            End If

            Dim channel = UserActivityChannel.GetDefault()
            Dim activity = Await channel.GetOrCreateUserActivityAsync(ActivityId)
            activity.ActivationUri = ActivationData.Uri
            activity.VisualElements.DisplayText = DisplayText
            activity.VisualElements.BackgroundColor = BackgroundColor
            activity.VisualElements.Description = Description
            Return activity
        End Function

    End Class
End Namespace