using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using RootNamespace.Extensions;
using Windows.Storage;

namespace ItemNamespace.Services
{
    public class StateService
    {
        //TODO UWPTEMPLATES: For more information regarding the application lifecycle and how to handle suspend and resume, please see: 
        //https://docs.microsoft.com/windows/uwp/launch-resume/app-lifecycle

        private const string stateFilename = "pageState.json";

        public static StateService Instance { get { return stateService.Value; } }

        private static readonly Lazy<StateService> stateService = new Lazy<StateService>(() => new StateService());

        //TODO UWPTEMPLATES: Subscribe to this event in pages in OnNavigatedTo event handler
        //to save page data, unsubscribe in OnNavigatedFrom
        public event SaveStateEventHandler SaveState;

        //TODO UWPTEMPLATES: Subscribe to this event in pages in OnNavigatedTo event handler
        //to restore page data, unsubscribe in OnNavigatedFrom
        public event RestoreStateEventHandler RestoreState;

        public async Task SaveStateAsync()
        {
            var pageState = new Object();
            var page = SaveState?.Target.GetType();
            var saveStateArgs = new SaveStateEventArgs(pageState, page);

            SaveState?.Invoke(this, saveStateArgs);
            await ApplicationData.Current.LocalFolder.SaveAsync<SaveStateEventArgs>(stateFilename, saveStateArgs);
        }

        public async Task RestoreStateAsync(ApplicationExecutionState previousState, string arguments)
        {
            if (previousState == ApplicationExecutionState.Terminated)
            {
                var saveState = await ApplicationData.Current.LocalFolder.ReadAsync<SaveStateEventArgs>(stateFilename);

                if (saveState != null && saveState.Page != null)
                {
                    //Navigate to page

                    //Restore page state
                    RestoreState?.Invoke(this, new RestoreStateEventArgs(saveState.PageState));
                }  
            }
        }
    }
}
