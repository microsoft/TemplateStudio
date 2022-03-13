using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;

namespace Param_RootNamespace
{
    public static class Program
    {
        // This project includes DISABLE_XAML_GENERATED_MAIN in the build properties,
        // which prevents the build system from generating the default Main method:
        // static void Main(string[] args)
        // {
        //     global::Windows.UI.Xaml.Application.Start((p) => new App());
        // }
        // TODO WTS: Update the logic in this method if you want to control the launching of multiple instances.
        // You may find the `AppInstance.GetActivatedEventArgs()` useful for your app-defined logic.
        public static void Main(string[] args)
        {
            // If the platform indicates a recommended instance, use that.
            if (AppInstance.RecommendedInstance != null)
            {
                AppInstance.RecommendedInstance.RedirectActivationTo();
            }
            else
            {
                // Update the logic below as appropriate for your app.
                // Multiple instances of an app are registered using keys.
                // Creating a unique key (as below) allows a new instance to always be created.
                // Always using the same key will mean there's only one ever one instance.
                // Or you can use your own logic to launch a new instance or switch to an existing one.
                var key = Guid.NewGuid().ToString();
                var instance = AppInstance.FindOrRegisterInstanceForKey(key);

                if (instance.IsCurrentInstance)
                {
                    // If successfully registered this instance, do normal XAML initialization.
                    global::Windows.UI.Xaml.Application.Start((p) => new App());
                }
                else
                {
                    // Some other instance has registered for this key, redirect activation to that instance.
                    instance.RedirectActivationTo();
                }
            }
        }
    }
}
