using System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
namespace MultiViewFeaturePoC.Services
{
    public delegate void ViewReleasedHandler(Object sender, EventArgs e);

    public sealed class ViewLifetimeControl
    {
        private bool _released = false;
        private int _refCount = 0;
        private bool consolidated = true;
        private event ViewReleasedHandler _internalReleased;

        public readonly ApplicationView View;

        public readonly CoreWindow Window;

        public readonly CoreDispatcher Dispatcher;

        public string Title { get; set; }

        public int ViewId { get; private set; }

        public Action OnCloseAction;

        private ViewLifetimeControl(CoreWindow newWindow)
        {
            Dispatcher = newWindow.Dispatcher;
            Window = newWindow;
            View = ApplicationView.GetForCurrentView();
            ViewId = View.Id;
            // ApplicationView.GetApplicationViewIdForWindow(Window);
            RegisterForEvents();
        }

        public static ViewLifetimeControl CreateForCurrentView()
        {
            return new ViewLifetimeControl(CoreWindow.GetForCurrentThread());
        }

        public int StartViewInUse()
        {
            bool releasedCopy = false;
            int refCountCopy = 0;
            lock (this)
            {
                releasedCopy = _released;
                if (!_released)
                {
                    _refCount++;
                    refCountCopy = _refCount;
                }
            }

            if (releasedCopy)
            {
                throw new InvalidOperationException("This view is being disposed");
            }

            return refCountCopy;
        }

        public int StopViewInUse()
        {
            int refCountCopy = 0;
            bool releasedCopy = false;

            lock (this)
            {
                releasedCopy = this._released;
                if (!_released)
                {
                    _refCount--;
                    refCountCopy = _refCount;
                    if (refCountCopy == 0)
                    {
                        var task = Dispatcher.RunAsync(CoreDispatcherPriority.Low, FinalizeRelease);
                    }
                }
            }

            if (releasedCopy)
            {
                throw new InvalidOperationException("This view is being disposed");
            }
            return refCountCopy;
        }

        public event ViewReleasedHandler Released
        {
            add
            {
                bool releasedCopy = false;
                lock (this)
                {
                    releasedCopy = _released;
                    if (!_released)
                    {
                        _internalReleased += value;
                    }
                }

                if (releasedCopy)
                {
                    throw new InvalidOperationException("This view is being disposed");
                }
            }

            remove
            {
                lock (this)
                {
                    _internalReleased -= value;
                }
            }
        }

        private void RegisterForEvents()
        {
            ApplicationView.GetForCurrentView().Consolidated += ViewConsolidated;
        }

        private void UnregisterForEvents()
        {
            ApplicationView.GetForCurrentView().Consolidated -= ViewConsolidated;
        }

        private void ViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs e) => StopViewInUse();

        private void FinalizeRelease()
        {
            bool justReleased = false;
            lock (this)
            {
                if (_refCount == 0)
                {
                    justReleased = true;
                    _released = true;
                }
            }
            if (justReleased)
            {
                UnregisterForEvents();
                _internalReleased?.Invoke(this, null);
                OnCloseAction?.Invoke();
            }
        }
    }
}
