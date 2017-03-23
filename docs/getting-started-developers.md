Getting Started for Developers
==============================
If you are authoring templates for Windows Template Studio, or interested in contributing to this repo, then you are likely interested in how to use the latest version of this code. The required steps are outlined below.

Be sure you are running [Visual Studio 2017 RC](https://www.visualstudio.com/downloads/) (the Community edition -free- is ok!)

## Running the Extension Locally
* Step 1: Clone this repo to your local machine

* Step 2: Open the solution [Big.sln](../code/Big.sln)

* Step 3: Set the project "Installer.2017" as StartUp Project for the solution. This is the Windows Template Studio Visual Studio Extension project. 

* Step 4: Configure the "Installer.2017" project to launch the [Visual Studio Experimental instance](https://msdn.microsoft.com/library/bb166560(v=vs.140).aspx) when run.
    * Step 4.1: Open the "Installer.2017" project properties.
    * Step 4.2: Go to "Debug" properties.
    * Step 4.3: In "Start Action", select "Start external program" and browse for your Visual Studio executable (devenv.exe), tipically in the path "C:\Program Files (x86)\Microsoft Visual Studio\2017\<YOUR_VS_EDITION>\Common7\IDE\" 
    * Step 4.4: In the "Start options", for the "Command line arguments" set the following: "/RootSuffix Exp
    * Step 4.5: Save the chages.
    
    ![Installer.2017 Configuration](./resources/getting-started/Installer2017.Debug.Config.JPG)
    *The project configuration should looks like this*

* Step 5: Build the solution.

* Step 6: Now you can start debugging (F5) or start without debugging (Ctrl+F5).

With this steps, the Windows Template Studio Extension is deployed to a new instance of Visual Studio (the experimental instance). Now you can go to "File -> New Project..." to launch the Wizard.

## Repo Solutions
The repo have different solutions to aid developers get focused on certain development areas:

* **Big.sln**: This is the solution which contains all the projects available, including test projects.
* **Installer.sln**: This solution is focused on the Visual Studio extension, have the extension project and all the dependencies and is thought to run the extension in the Visual Studio Experimental IDE.
* **Wizard.sln**: This solution is focused in the Wizard itself. Using this solution (thru the VsEmulator project) you can launch the Windows Template Studio wizard in a more lightweight way since it does not deploy the extension to the VS experimental instance.
* **Core.sln**: This solution contains the Core assembly and it's unit tests. Use this solution when common core code is developed.
* **Test.sln**: This solution is used to work with the integration tests.




