using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;
using Windows.Storage;
using System.IO;
using System.Runtime.CompilerServices;

namespace ItemNamespace.Services
{
    public class StateService
    {
        private const string stateFilename = "pageState.json";

        internal async Task SaveStateAsync()
        {
            var pageState = new Dictionary<String, Object>();
            Type page = SaveState.Target.GetType();
            var saveStateArgs = new SaveStateEventArgs(pageState, page);

            SaveState?.Invoke(this, saveStateArgs);
            var stateData = JsonConvert.SerializeObject(saveStateArgs);

            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(stateFilename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(file, stateData);

        }

        internal async Task RestoreStateAsync(string arguments)
        {
            SaveStateEventArgs saveState;

            if (File.Exists(Path.Combine(ApplicationData.Current.LocalFolder.Path, stateFilename)))
            {
                var file = await ApplicationData.Current.LocalFolder.GetFileAsync(stateFilename);

                if (file != null)
                {
                    var saveStateData = await FileIO.ReadTextAsync(file);

                    saveState = JsonConvert.DeserializeObject<SaveStateEventArgs>(saveStateData);

                    if (saveState != null)
                    {
                        //Navigate to stored page 
                        NavigationService.Instance.Navigate(saveState.Page, arguments);

                        //Restore page state
                        RestoreState.Invoke(this, new RestoreStateEventArgs(saveState.PageState));
                    }
                }
            }
        }

        //TODO UWPTEMPLATES: Subscribe to this event in pages in OnNavigatedTo event handler
        //                   to save page data, unsubscribe in OnNavigatedFrom

        public event SaveStateEventHandler SaveState;
        
        public delegate void SaveStateEventHandler(object sender, SaveStateEventArgs e);


        //TODO UWPTEMPLATES: Subscribe to this event in pages in OnNavigatedTo event handler
        //                   to restore page data, unsubscribe in OnNavigatedFrom

        public event RestoreStateEventHandler RestoreState;

        public delegate void RestoreStateEventHandler(object sender, RestoreStateEventArgs e);

        

        public class RestoreStateEventArgs : EventArgs
        {
           
            public Object PageState { get; private set; }

            public RestoreStateEventArgs(Object pageState)
                : base()
            {
                this.PageState = pageState;
            }
        }

       
        public class SaveStateEventArgs : EventArgs
        {
            public Object PageState { get; set; }

            public Type Page { get; private set; }

            public SaveStateEventArgs(Object pageState, Type page)
                : base()
            {
                this.PageState = pageState;
                this.Page = page;
            }
        }
    }
}
