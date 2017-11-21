using System;
using System.ComponentModel;
using Windows.UI.Core;
using Windows.UI.ViewManagement;

namespace MultiViewFeaturePoC.Services
{
    public delegate void ViewReleasedHandler(Object sender, EventArgs e);
    
    public sealed class ViewLifetimeControl : INotifyPropertyChanged
    {
        CoreDispatcher dispatcher;
        CoreWindow window;
        string title;
        int refCount = 0;
        int viewId;
        bool released = false;
        bool consolidated = true;
        event ViewReleasedHandler InternalReleased;

        private ViewLifetimeControl(CoreWindow newWindow)
        {
            dispatcher = newWindow.Dispatcher;
            window = newWindow;
            viewId = ApplicationView.GetApplicationViewIdForWindow(window);
            RegisterForEvents();
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
                if (refCount == 0)
                {
                    justReleased = true;
                    released = true;
                }
            }

            if (justReleased)
            {
                UnregisterForEvents();
                InternalReleased(this, null);
            }
        }

        public static ViewLifetimeControl CreateForCurrentView()
        {
            return new ViewLifetimeControl(CoreWindow.GetForCurrentThread());
        }

        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                if (title != value)
                {
                    title = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Title"));
                }
            }
        }

        public CoreDispatcher Dispatcher
        {
            get
            {
                return dispatcher;
            }
        }

        public int Id
        {
            get
            {
                return viewId;
            }
        }

        public int StartViewInUse()
        {
            bool releasedCopy = false;
            int refCountCopy = 0;

            lock (this)
            {
                releasedCopy = this.released;
                if (!released)
                {
                    refCountCopy = ++refCount;
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
                releasedCopy = this.released;
                if (!released)
                {
                    refCountCopy = --refCount;
                    if (refCountCopy == 0)
                    {
                        var task = dispatcher.RunAsync(CoreDispatcherPriority.Low, FinalizeRelease);
                    }
                }
            }

            if (releasedCopy)
            {
                throw new InvalidOperationException("This view is being disposed");
            }
            return refCountCopy;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event ViewReleasedHandler Released
        {
            add
            {
                bool releasedCopy = false;
                lock (this)
                {
                    releasedCopy = released;
                    if (!released)
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
    }
}
