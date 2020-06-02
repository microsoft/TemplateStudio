Imports Windows.UI.Xaml

Namespace StateTriggers
    Public Class ControlSizeTrigger
        Inherits StateTriggerBase

        Private _currentWidth As Double
        Private _targetElement As FrameworkElement
        Public Property MinWidth As Double = -1

        Public Property TargetElement As FrameworkElement
            Get
                Return _targetElement
            End Get
            Set(value As FrameworkElement)

                If _targetElement IsNot Nothing Then
                    RemoveHandler _targetElement.SizeChanged, AddressOf OnTargetElementSizeChanged
                End If

                _targetElement = value
                AddHandler _targetElement.SizeChanged, AddressOf OnTargetElementSizeChanged
            End Set
        End Property

        Public Sub New()
        End Sub

        Private Sub OnTargetElementSizeChanged(sender As Object, e As SizeChangedEventArgs)
            _currentWidth = e.NewSize.Width
            UpdateTrigger()
        End Sub

        Private Sub UpdateTrigger()
            If _targetElement Is Nothing OrElse MinWidth = 0 Then
                SetActive(False)
            Else
                SetActive(_currentWidth >= MinWidth)
            End If
        End Sub
    End Class
End Namespace
