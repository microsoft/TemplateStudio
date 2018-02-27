# Getting started with the generator codebase

If you are authoring templates for Windows Template Studio, or interested in [contributing](../CONTRIBUTING.md) to this repo, then you are likely interested in how to use the latest version of this code. The required steps are outlined below.

If you just want to take advantage from Windows Template Studio extension, check the [Getting Started with the Extension](getting-started-extension.md) page.

## Repo Solutions

Under the [code](../code/) folder, the repo have different solutions to aid developers get focused on certain development areas:

* **Big.sln**: This is the solution which contains all the projects available, including test projects.
* **Installer.sln**: This solution is focused on the Visual Studio extension, have the extension project and all the dependencies and is thought to run the extension in the Visual Studio Experimental IDE.
* **UI.sln**: This solution is focused in the user interface, that is, the Wizard itself. Using this solution (by executing the VsEmulator project) you can launch the Windows Template Studio wizard in a more lightweight way since it does not deploy the extension to the VS experimental instance.
* **Core.sln**: This solution contains the Core assembly and it's unit tests. Use this solution when common core code is developed.
* **Test.sln**: This solution is used to work with the project integration tests.

## Running the Extension Locally

First of all, be sure you are running [Visual Studio 2017](https://www.visualstudio.com/downloads/) (Any version works)

1. Clone this repo to your local machine
1. Open the solution [Big.sln](../code/)
1. Set the project "Installer.2017" as StartUp Project for the solution. This is the Visual Studio Extension project for Windows Template Studio.
1. Configure the "Installer.2017" project to launch the [Visual Studio Experimental instance](https://msdn.microsoft.com/library/bb166560(v=vs.140).aspx) when run.
   1. Open the "Installer.2017" project properties.
   1. Go to "Debug" properties.
   1. In "Start Action", select "Start external program" and browse for your Visual Studio executable (devenv.exe), typically in the path "C:\Program Files (x86)\Microsoft Visual Studio\2017\*YOUR_VS_EDITION*\Common7\IDE\"
   1. In the "Start options", for the "Command line arguments" set the following: "/RootSuffix Exp
   1. Save the changes.
    ![Installer.2017 Configuration](./resources/getting-started/Installer2017.Debug.Config.JPG)
    *The project configuration should looks like this*
1. Build the solution.
1. Start debugging (F5) or start without debugging (Ctrl+F5).

With this steps, the Windows Template Studio Extension is deployed to a new instance of Visual Studio (the experimental instance). Now you can go to "File -> New Project..." to launch the Wizard.

The extension wizard, when runs locally, uses the local [templates folder](../templates) as source for the Templates Repository.

## Using the UI.sln solution

To speed up the execution and development experience, we have created a [VsEmulator application](../src/test) which can be used to launch and test the Windows Template Studio Wizard. This application, as well as the Wizard assembly, are available thru the UI.sln solution. To use it, follow this steps:

1. Open the UI.sln solution
1. Set the "test\VsEmulator" project as "StartUp"
1. Start debugging (F5) or start without debugging (Ctrl+F5).

Using this solution while authoring templates or improving the Wizard have the following advantages:

1. Speed up the development since it does not deploy the VSIX to the VS Experimental instance every time you build.
1. Simple and lightweight run / debug experience since it does not require to launch another instance of Visual Studio.

So we encourage to use this solution for the general template authoring or code development and, once you are done, make some final local tests using the Installer.sln or Big.sln solution.

### Accesible UI


Both the UI and the templates (generated code) must be accesible by definition. If you are going to collaborate in this space, please, be sure you have verified all accesibility rules defined in [Accessibility checklist](accesibility.md).

## Inside the Code folder

Following are described the contents for each folder:

* [_tools](../code/_tools): tooling required for testing / validations.
* [src](../code/src): solution source code
  * [Core](../code/src/core): Core VS Project for the solution Core classes, i.e.: enable the generation of code wrapping the "Template Engine generator", deals with templates source location and synchronization, provide the diagnostics infrastructure, etc .
  * [Installer.2017](../code/src/Installer.2017): This is the Visual Studio Extension project. Enables the installation of the extension to enable the access to the Windows Template Studio Project Template and ensures that all required assets are deployed with it.
  * [ProjectTemplates](../code/src/ProjectTemplates): This folder contains the [Visual Studio Project Templates](https://msdn.microsoft.com/library/ms247121.aspx) deployed with the extension to enable the "File --> New Project..." experience. There are separate templates for the C# and Visual basic versions of the template.
  * [UI](../code/src/UI): This project handles the generation as well as the UI dialogs required by the generation workflow.
* [test](../code/test)
  * [Fakes](../code/test/Fakes): Common test elements.
  * [Core.Test](../code/test/Core.Test): Contains unit test for Core assembly.
  * [Templates.Test](../code/test/Templates.Test): Contains integration automated test for the Templates. This project scans the Templates folder and ensure that every template is generating and building properly.
  * [UI.Test](../code/test/UI.Test): Contains unit test for UI assembly.
  * [VsEmulator](../code/test/VsEmulator): test application able to run End-To-End the generation using the local templates repository without deploying the VSIX to VS Experimental instance.

## Test execution

The following list shows which tests are executed in which build. Within the Templates.Test project we use the trait ExecutionSet to specify which tests are run. 

* AppVeyor 'CIBuild' Build (CI):	
  * Core.Tests	
  * UI.Test	
  * Templates.Tests	
    * ExecutionSet=BuildMinimum
    * ExecutionSet=BuildStyleCop
    * ExecutionSet=TemplateValidation
		
* VSO 'Templates.Test.Gen' Build (Gen Tests):	
  * Templates.Test 
    * ExecutionSet=Generation

* VSO 'Templates.Test.Full'	Build (Full Tests):
  * Core.Tests	
  *	UI.Tests	
  *	Templates.Test
    * ExecutionSet=BuildMVVMBasic
    * ExecutionSet=BuildCodeBehind 
    * ExecutionSet=BuildMVVMLight
    * ExecutionSet=BuildCaliburnMicro
    * ExecutionSet=BuildPrism
    * ExecutionSet=BuildStyleCop
    * ExecutionSet=TemplateValidation
    * ExecutionSet=BuildRightClickWithLegacy

* VSO 'Templates.Test.Wack'	Build (Wack Tests):
  * Templates.Test
    * ExecutionSet=LongRunning

To shorten test execution time traits in Templates.Test are run parallel using this [script](../_build/ParallelTestExecution.ps1).
To execute this script locally use the following powershell command:

`<wts directory>\_build\ParallelTestExecution.ps1 -testRunner <wts directory>\Code\packages\xunit.runner.console.2.2.0\tools\xunit.console.exe -testLibrary <wts directory>\Code\test\Templates.Test\bin\Analyze\Microsoft.Templates.Test.dll -traits 'ExecutionSet=BuildMinimum', 'ExecutionSet=BuildStyleCop', 'ExecutionSet=TemplateValidation' -outputDir <output directory>`

where

* `<wts directory>` : Directory where WTS is cloned
* `<output directory>`: Directory where test xml result files will be generated

## Table of Contents

* [Installing / Using the extension](getting-started-extension.md)
* [Using and extending your file->new](getting-started-endusers.md)
* [**Getting started with the generator codebase**](getting-started-developers.md)
* [Authoring Templates](templates.md)
* [Concepts of Windows Template Studio](readme.md)
