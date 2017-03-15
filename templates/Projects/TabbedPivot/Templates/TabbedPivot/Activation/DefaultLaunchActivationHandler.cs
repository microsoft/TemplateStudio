using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace uct.TabbedPivotProject.Activation
{
    class DefaultLaunchActivationHandler : ActivationHandler<LaunchActivatedEventArgs>
    {
        private readonly Type _page;

        public DefaultLaunchActivationHandler(Type page)
        {
            _page = page;
        }

        private Frame RootFrame
        {
            get
            {
                var rootFrame = Window.Current.Content as Frame;
                if (rootFrame == null)
                {
                    throw new Exception("Window.Current.Content is not a frame");
                }
                return rootFrame;
            }
        }

        protected override async Task HandleInternalAsync(LaunchActivatedEventArgs args)
        {
            //TODO: NAVIGATE WITH NAVIGATION SERVICE?

            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            RootFrame.Navigate(_page, args.Arguments);

            await Task.FromResult(true).ConfigureAwait(false);
        }

        protected override bool CanHandleInternal(LaunchActivatedEventArgs args)
        {
            //None of the ActivationHandlers has handled the app activation
            return RootFrame.Content == null;
        }
    }
}
