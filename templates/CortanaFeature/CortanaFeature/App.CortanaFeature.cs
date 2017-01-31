using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.VoiceCommands;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Feature_NS
{
    public partial class App
    {
        private async void RegisterCortanaFeatureCommands()
        {
            StorageFile commandsFile = await Package.Current.InstalledLocation.GetFileAsync(@"CortanaFeature\CortanaFeatureCommands.xml");
            await VoiceCommandDefinitionManager.InstallCommandDefinitionsFromStorageFileAsync(commandsFile);
        }

        private void CortanaFeatureAppActivation(IActivatedEventArgs args)
        {
            if (args.Kind == ActivationKind.VoiceCommand)
            {
                var commandArgs = args as VoiceCommandActivatedEventArgs;
                if (commandArgs != null)
                {
                    var speechRecognitionResult = commandArgs.Result;
                    // TODO: Manage command result information 
                }
                Frame rootFrame = Window.Current.Content as Frame;
                if (rootFrame == null)
                {
                    rootFrame = new Frame();
                    rootFrame.NavigationFailed += OnNavigationFailed;
                    Window.Current.Content = rootFrame;
                }
                // TODO: Navigate to page
                // rootFrame.Navigate(typeof(MainPage));
                Window.Current.Activate();
            }
        }
    }
}
