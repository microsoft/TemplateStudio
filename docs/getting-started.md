Getting Started
===============
You can take advantage of Windows Template Studio by installing our Visual Studio Extension or by cloning the repo and working locally with the code and the available templates. If you plan to contribute, please follow the [contribution guidelines](https://github.com/Microsoft/WindowsTemplateStudio/blob/master/contributing.md).  

You can install the Windows Template Studio Visual Studio Extension (pre-release nightly version) configuring the following extensions feed https://www.myget.org/F/vsixextensions/vsix/. Follow detailed installation instructions [below](#the-project).

You can download the official extension from the [Visual Studio Gallery](https://visualstudiogallery.msdn.microsoft.com/) (coming soon).

We are currently targeting Visual Studio 2017 RC.

We mainly handle the following areas or concepts:
* Templates
* Generation
* Templates Repository
* Visual Studio Extension

## Main concepts
### What is a Template?
A template is just code with some metadata. The metadata will contain the template information: name, description, licensing, remarks, programming language, type, guids, etc. The template definition is based on [dotnet Template Engine](https://github.com/dotnet/templating).

There are four different types of templates:
* **Framework Templates**: Will contain the code used as infrastructure for the projects.
* **Project Template**: project templates define the type of App you are about to generate (Basic, Split View, Tabbed...). 
* **Page Template**: will contain the files and code needed to add a page to a certain App.
* **Feature Templates**: will contain the files and code needed to add features to a certain App.

### What is the Generation process?
The generation is the process executed to create actual code from a selected template. That is, the process to create the real Visual Studio project (thought to be an App), or to create the XAML Page and its code behind, or the code to add/enable certain feature.

As well as templates, the generation is based on [dotnet Template Engine](https://github.com/dotnet/templating) code generation.

### What is the Template Repository?
The Templates Repository is the place where we will gather all templates and will make them available for the community (hosted on a CDN). We will have two repositories publicly available:
* Master: The stable and public version of the templates.
* VNext: The ongoing version of the templates.

### What is the Visual Studio Extension?
UWP App developers can take advantage of the Windows Template Studio by installing our Visual Studio extension. This extension will allow developers to create Apps, add pages and/or add features to existing apps based on the available Templates from the public Repository. 

The Windows Template Studio Visual Studio Extension (pre-release nightly build) version is published [here](). The stable public version will be published through the [Visual Studio Gallery](https://visualstudiogallery.msdn.microsoft.com/) when ready.

## Installing the Visual Studio Extension
### Pre-release nightly build version
Open Visual Studio 2017 and go to Tools -> Extensions & Updates, then click on "Change your Extensions and Updates settings" and create an Additional Extension Gallery using https://www.myget.org/F/vsixextensions/vsix/ as Url.

![Configure Additional Extension Gallery](resources/vsix/configurefeed.jpg)

Then, go again to Tools -> Extensions & Updates and using the recently added online gallery, install the Windows Template Studio Extension.

![Install Windows Template Studio extension](resources/vsix/onlinefeed.jpg)

Once installed, you will see a new Project Template which allows you to access to the available templates: Pre-Release version uses the VNext Template Repository.

![File New Project](resources/vsix/filenew.jpg)

### Official release version.
Coming soon.

