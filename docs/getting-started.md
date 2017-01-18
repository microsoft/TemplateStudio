Getting Started
===============
We mainly handle the following areas:
* Templates
* Generation
* Templates Repository
* Visual Studio Extension

### What is a Template?
A template is just code with some metadata. The metadata will contain the template information: name, description, licensing, remarks, programming language, type, guids, etc. The template definition is based on [dotnet Template Engine](https://github.com/dotnet/templating).

There are three different types of templates:
* Project Template: to create Apps. Any project created with Visual Studio can be a template. Project templates must be working projects (must build and run).
* Page Template: this templates will contains the files needed to add a page to a certain App.
* Feature Templates: to add features to a certain App.

### What is the Generation?
The generation is the process executed to create actual code from a selected template. That is, the process to create the real Visual Studio project (thought to be an App), or to create the Xaml Page and its codebehind, or the code to add/enable certain feature.

As well as templates, the generation is based on [dotnet Template Engine](https://github.com/dotnet/templating) code generation.

### What is the Template Repository?
The Templates Repository is the place where we will gather all templates and will make them available for the community (hosted on a CDN). We will have two repositories publicly available:
* Master: The stable and public version of the templates.
* VNext: The ongoing version of the templates.

### What is the Visual Studio Extension?
UWP App developers can take advantage of the UWP Community Templates by installing our Visual Studio extension. This extension will allow developers to create Apps, add pages and/or add features to exsisting apps based on the available Templates from the public Repository. 

### Running locally
Clone the repo and you should be able to start working. 
<TBD>

### Install the Visual Studio Extension (nightly build version)
The ongoing version of the 