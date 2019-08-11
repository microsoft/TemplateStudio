        async protected override void OnActivated(IActivatedEventArgs args)
        {
            switch (args.Kind)
            {
                case ActivationKind.CommandLineLaunch:
                    CommandLineActivatedEventArgs cmdLineArgs = args as CommandLineActivatedEventArgs;
                    CommandLineActivationOperation operation = cmdLineArgs.Operation;

                    // The arguments supplied on command-line activation are available in the 
                    // CommandLineActivationOperation.Arguments property. Note that because these
                    // are supplied by the caller, they should be treated as untrustworthy.
                    string cmdLineString = operation.Arguments;

                    // The CommandLineActivationOperation.CurrentDirectoryPath is the directory
                    // current when the command-line activation request was made. This is typically
                    // not the install location of the app itself, but could be any arbitrary path.
                    string activationPath = operation.CurrentDirectoryPath;

                    // FINITE WORK PATTERN
                    // You can choose to do some finite work and then possibly exit.
                    // If you want to perform asynchronous work that must complete before returning to
                    // the caller, you should take a deferral before exiting.
                    //using (Deferral deferral = operation.GetDeferral())
                    //{
                    //    // Do any asynchronous work within the scope of a deferral.
                    //    for (int i = 0; i < 10; i++)
                    //    {
                    //        Debug.WriteLine("count = {0}", i);
                    //        await Task.Delay(1000);
                    //    }
                    //    // The app can supply an app-defined exit code to the caller, if required.
                    //    // This will be available to the caller when OnActivate returns or this app exits.
                    //    // If you don't set the ExitCode it defaults to zero.
                    //    operation.ExitCode = 1;
                    //}
                    // // If you don't want normal windowed execution, you can now exit.
                    // CoreApplication.Exit();

                    // REGULAR WINDOWS PATTERN
                    // You can choose to run your normal windowed operation.
                    // Ensure you have a main window set up, and optionally pass in the command-line arguments.
                    Frame rootFrame = Window.Current.Content as Frame;
                    if (rootFrame == null)
                    {
                        rootFrame = new Frame();
                        Window.Current.Content = rootFrame;
                    }
                    rootFrame.Navigate(typeof(MainPage), 
                        string.Format("CurrentDirectory={0}, Arguments={1}",
                        activationPath, cmdLineString));
                    Window.Current.Activate();

                    break;
            }
        }
