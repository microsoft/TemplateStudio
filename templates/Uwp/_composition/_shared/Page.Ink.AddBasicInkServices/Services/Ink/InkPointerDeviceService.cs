using System;
using Windows.Devices.Input;
using Windows.UI.Core;
using Windows.UI.Input.Inking;
using Windows.UI.Xaml.Controls;

namespace Param_ItemNamespace.Services.Ink
{
    public class InkPointerDeviceService
    {
        private readonly InkCanvas _inkCanvas;

        private bool enableMouse;
        private bool enablePen;
        private bool enableTouch;

        public InkPointerDeviceService(InkCanvas inkCanvas)
        {
            _inkCanvas = inkCanvas;
            _inkCanvas.InkPresenter.InputDeviceTypes = CoreInputDeviceTypes.Mouse |
                                                      CoreInputDeviceTypes.Pen |
                                                      CoreInputDeviceTypes.Touch;

            _inkCanvas.InkPresenter.UnprocessedInput.PointerEntered += UnprocessedInput_PointerEntered;
        }

        public event EventHandler<EventArgs> DetectPenEvent;

        public bool EnableMouse
        {
            get => enableMouse;
            set
            {
                enableMouse = value;
                UpdateInputDevice(CoreInputDeviceTypes.Mouse, value);
            }
        }

        public bool EnablePen
        {
            get => enablePen;
            set
            {
                enablePen = value;
                UpdateInputDevice(CoreInputDeviceTypes.Pen, value);
            }
        }

        public bool EnableTouch
        {
            get => enableTouch;
            set
            {
                enableTouch = value;
                UpdateInputDevice(CoreInputDeviceTypes.Touch, value);
            }
        }

        private void UnprocessedInput_PointerEntered(InkUnprocessedInput sender, PointerEventArgs e)
        {
            if (e.CurrentPoint.PointerDevice.PointerDeviceType == PointerDeviceType.Pen)
            {
                DetectPenEvent?.Invoke(this, EventArgs.Empty);
            }
        }

        private void UpdateInputDevice(CoreInputDeviceTypes inputDevice, bool isEnabled)
        {
            _inkCanvas.InkPresenter.InputDeviceTypes = isEnabled
                ? _inkCanvas.InkPresenter.InputDeviceTypes | inputDevice
                : _inkCanvas.InkPresenter.InputDeviceTypes & ~inputDevice;
        }
    }
}