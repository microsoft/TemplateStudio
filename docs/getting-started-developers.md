Getting Started for Developers
==============================
If you are authoring templates for Windows Template Studio, or interested in contributing to this repo, then you are likely interested in how to use the latest version of this code. The required steps are outlined below.

If you just want to take advantage from the Windows Template Studio extension, check the [Getting Started with the Extension](getting-started-extension.md) page.

## Running the Extension Locally
First of all, be sure you are running [Visual Studio 2017 RC](https://www.visualstudio.com/downloads/) (the Community edition -free- is ok!)

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
* Step 6: Start debugging (F5) or start without debugging (Ctrl+F5).

With this steps, the Windows Template Studio Extension is deployed to a new instance of Visual Studio (the experimental instance). Now you can go to "File -> New Project..." to launch the Wizard.

The extension wizard, when runs locally, uses the local [templates folder](..\templates) as source for the Templates Repository. 

## Repo Solutions
Under the [code](../code/) folder, the repo have different solutions to aid developers get focused on certain development areas:

* **Big.sln**: This is the solution which contains all the projects available, including test projects.
* **Installer.sln**: This solution is focused on the Visual Studio extension, have the extension project and all the dependencies and is thought to run the extension in the Visual Studio Experimental IDE.
* **Wizard.sln**: This solution is focused in the Wizard itself. Using this solution (thru the VsEmulator project) you can launch the Windows Template Studio wizard in a more lightweight way since it does not deploy the extension to the VS experimental instance.
* **Core.sln**: This solution contains the Core assembly and it's unit tests. Use this solution when common core code is developed.
* **Test.sln**: This solution is used to work with the project integration tests.

## Using the Wizard.sln solution
To speed up the execution and development experience, we have created a [VsEmulator application](..\src\test) which can be used to launch and test the Windows Template Studio Wizard. This application, as well as the Wizard assembly, are available thru the Wizard.sln solution. To use it, follow this steps:

* Step 1: Open the Wizard.sln solution
* Step 2: Set the "test\VsEmulator" project as "StartUp"
* Step 3: Start debugging (F5) or start without debugging (Ctrl+F5).

Using this solution while authoring templates or improving the Wizard have the following advantages:
1. Speed up the development since it does not deploy the VSIX to the VS Experimental instance every time you build.
2. Simple and lightwight run / debug experience since it does not require to launch another instance of Visual Studio. 

So we encourage to use this solution for the general template authoring or code development and, once you are done, make some final local tests using the Installer.sln or Big.sln solution.

## Inside the Code folder
Follwing are described the contents for each folder:

* [_tools](../code/_tools): tooling required for testing / validations.
* [src](../code/src): solution source code
    * [Core](../code/src/core): Core VS Project for the solution Core clases, i.e.: enable the generation of code wrapping the "Template Engine generator", deals with templates source location and synchronization, provide the diagnostics infrastructure, etc .  
    * [Installer.2017](../code/src/Installer.2017): This is the Visual Studio Extension project. Enables the installation of the extension to enable the access to the Windows Template Studio Project Template and ensures that all required assets are deployed with it.
    * [ProjectTemplates](../code/src/ProjectTemplates): This folder contains the [Visual Studio Project Templates](https://msdn.microsoft.com/library/ms247121.aspx) deployed with the extension to enable the "File --> New Project..." experience (currently just one).
    * [Wizard](../code/src/Wizard): This project handles the generation as well as the UI dialogs required by the generation workflow.
* [test](../code/test)
    * [Artifacts](../code/test/Artifacts): Common test elements. 
    * [Core.Test](../code/test/Core.Test): Contains unit test for Core assembly.
    * [Templates.Test](../code/test/Templates.Test): Contains integration automated test for the Templates. This project scans the Templates folder and ensure that every template is generating and building properly.
    * [VsEmulator](../code/test/VsEmulator): test application able to run End-To-End the generation using the local templates repository without deploying the VSIX to VS Experimental instance.