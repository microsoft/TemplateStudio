Imports Windows.Devices.Input
Imports Windows.UI.Core
Imports Windows.UI.Input.Inking

Namespace Services.Ink
    Public Class InkPointerDeviceService
        Private ReadOnly _inkCanvas As InkCanvas
        Private _enableMouse As Boolean
        Private _enablePen As Boolean
        Private _enableTouch As Boolean

        Public Sub New(inkCanvas As InkCanvas)
            _inkCanvas = inkCanvas
            _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse Or CoreInputDeviceTypes.Pen Or CoreInputDeviceTypes.Touch
            AddHandler _inkCanvas.InkPresenter.UnprocessedInput.PointerEntered, AddressOf UnprocessedInput_PointerEntered
        End Sub

        Public Event DetectPenEvent As EventHandler(Of EventArgs)

        Public Property EnableMouse As Boolean
            Get
                Return _enableMouse
            End Get
            Set(value As Boolean)
                _enableMouse = value
                UpdateInputDevice(CoreInputDeviceTypes.Mouse, value)
            End Set
        End Property

        Public Property EnablePen As Boolean
            Get
                Return _enablePen
            End Get
            Set(value As Boolean)
                _enablePen = value
                UpdateInputDevice(CoreInputDeviceTypes.Pen, value)
            End Set
        End Property

        Public Property EnableTouch As Boolean
            Get
                Return _enableTouch
            End Get
            Set(value As Boolean)
                _enableTouch = value
                UpdateInputDevice(CoreInputDeviceTypes.Touch, value)
            End Set
        End Property

        Private Sub UnprocessedInput_PointerEntered(sender As InkUnprocessedInput, e As PointerEventArgs)
            If e.CurrentPoint.PointerDevice.PointerDeviceType = PointerDeviceType.Pen Then
                RaiseEvent DetectPenEvent(Me, EventArgs.Empty)
            End If
        End Sub

        Private Sub UpdateInputDevice(inputDevice As CoreInputDeviceTypes, isEnabled As Boolean)
            _inkCanvas.InkPresenter.InputDeviceTypes = If(isEnabled, _inkCanvas.InkPresenter.InputDeviceTypes Or inputDevice, _inkCanvas.InkPresenter.InputDeviceTypes And Not inputDevice)
        End Sub
    End Class
End Namespace
