Getting Started
===============
You can take advantage of UWP Community Templates by installing our Visual Studio Extension or by clonning the repo and working locally with the code and available templates. If you plan to contribute, please follow the [contribution guidelines](contributing.md).  

Currently you can install the Visual Studio Extension (pre-release nightly version) from [comming soon](). When ready, the official extension will be published to the [Visual Studio Gallery](https://visualstudiogallery.msdn.microsoft.com/).

We mainly handle the following areas or concepts:
* Templates
* Generation
* Templates Repository
* Visual Studio Extension

### What is a Template?
A template is just code with some metadata. The metadata will contain the template information: name, description, licensing, remarks, programming language, type, guids, etc. The template definition is based on [dotnet Template Engine](https://github.com/dotnet/templating).

There are three different types of templates:
* **Project Template**: to create Apps. Any project created with Visual Studio can be a template. Project templates must be working projects (must build and run).
* **Page Template**: will contain the files needed to add a page to a certain App.
* **Feature Templates**: will contain to add features to a certain App.

### What is the Generation?
The generation is the process executed to create actual code from a selected template. That is, the process to create the real Visual Studio project (thought to be an App), or to create the Xaml Page and its code behind, or the code to add/enable certain feature.

As well as templates, the generation is based on [dotnet Template Engine](https://github.com/dotnet/templating) code generation.

### What is the Template Repository?
The Templates Repository is the place where we will gather all templates and will make them available for the community (hosted on a CDN). We will have two repositories publicly available:
* Master: The stable and public version of the templates.
* VNext: The ongoing version of the templates.

### What is the Visual Studio Extension?
UWP App developers can take advantage of the UWP Community Templates by installing our Visual Studio extension. This extension will allow developers to create Apps, add pages and/or add features to exsisting apps based on the available Templates from the public Repository. 

The UWP Community Templates Visual Studio Extension (pre-release nightly build) version is published [here](). The stable public version will be published through the [Visual Studio Gallery](https://visualstudiogallery.msdn.microsoft.com/) when ready.

### Running the code locally
Clone the repo and you should be able to start working with UWP Community Templates. There are two main entry points in the code:
* [Wizard.TestApp](code/test/wizard.testapp): This is a test application project which is able to run End-To-End the generation using the local templates repository. It can read the Templates folder and generate code based on the contents. This test application is thought to be able to launch and interact with the extension UI without having to run the Visual Studio Experimental instance (in other words, much more lightweight). 
* [Vsix](code/src/vsix): This is the Visual Studio Extension project. You can run it from your working Visual Studio instance and will launch the [Visual Studio Experimental instance](https://msdn.microsoft.com/library/bb166560(v=vs.140).aspx) with the extension deployed to it without interfering with your working instance. The extension is configured to run against the CDN by default.

### Install the Visual Studio Extension (pre-release nightly build)
Coming soon.

### Install the Visual Studio Extension (official)
Coming soon.

### Authoring Templates
Coming soon.

