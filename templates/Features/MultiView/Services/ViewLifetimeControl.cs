using System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace Param_ItemNamespace.Services
{
    public delegate void ViewReleasedHandler(Object sender, EventArgs e);
    
    public sealed class ViewLifetimeControl
    {
        private CoreWindow _window;
        private int _refCount = 0;
        private bool _released = false;
        private bool _consolidated = true;
        private event ViewReleasedHandler InternalReleased;

        public CoreDispatcher Dispatcher { get; private set; }

        public int Id { get; private set; }

        public string Title { get; set; }

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
                        InternalReleased += value;
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
                    InternalReleased -= value;
                }
            }
        }

        private ViewLifetimeControl(CoreWindow newWindow)
        {
            Dispatcher = newWindow.Dispatcher;
            _window = newWindow;
            Id = ApplicationView.GetApplicationViewIdForWindow(_window);
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
                releasedCopy = this._released;
                if (!_released)
                {
                    refCountCopy = ++_refCount;
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
                    refCountCopy = --_refCount;
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

        private void RegisterForEvents()
        {
            ApplicationView.GetForCurrentView().Consolidated += ViewConsolidated;
        }

        private void UnregisterForEvents()
        {
            ApplicationView.GetForCurrentView().Consolidated -= ViewConsolidated;
        }

        private void ViewConsolidated(ApplicationView sender, ApplicationViewConsolidatedEventArgs e)
        {
            StopViewInUse();
        }

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
                InternalReleased(this, null);
            }
        }
    }
}
