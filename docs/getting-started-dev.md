Getting Started for Developers
==============================

You can start working with Windows Template Studio by cloning [our repo](https://github.com/Microsoft/WindowsTemplateStudio) and working locally with the code and the available templates.

If you plan to contribute, please follow the [contribution guidelines](https://github.com/Microsoft/WindowsTemplateStudio/blob/master/contributing.md) and remeber that the Pull Requests must be done aganist the "[dev](https://github.com/Microsoft/WindowsTemplateStudio/tree/dev)" branch.


Our repo have the following root structure:
* _build -> powershell scripts for continuous integration purpose.
* code -> all solution code (tests and sources).
* docs -> documentation pages.
* templates -> templates repository sources.

We are currently targeting Visual Studio 2017 RC and the solution have three main pieces
* Templates --> Composite pieces of code that are used to generate the final code.
* Generation Wizard --> Allow the end-users to select a particular combination of templates to create a New Project 
* Visual Studio Extension --> The extension that allow to invoke the Wizard thru File -> New Project command from the Visual Studio IDE.

As a developer you may want to understand what is behind the code generation and template authoring, so lets drill down...

# Inside the code
## Main components
* [Core](../code/src): This assembly contains the core elements to enable the generation of templates. Deals with the location of the templates, the synchronization of the content and the  
* [Installer.2017](../code/src): This is the Visual Studio Extension project. Enables the access to the commands and the project templates and ensures that all required assets are deployed with it.
* [ProjectTemplates](../code/src): This folder contains the [Visual Studio Project Templates](https://msdn.microsoft.com/library/ms247121.aspx) deployed with the extension.
* [Wizard](../code/src): This project handles the generation orchestration as well as the UI dialogs required to handle the workflow.

## Test projects
There are different test projects in the solution:
* [Artifacts](../code/test): Common test elements. 
* [Core.Test](../code/test/): Contains unit test for Core assembly.
* [Templates.Test](../code/test/): Contains integration automated test for the Templates. This project scans the Templates folder and ensure that every template is generating and building properly.
* [VsEmulator](../code/test/): test application able to run End-To-End the generation using the local templates repository.

## VS Solutions
We have the following solutions to help get focused in the different development areas:
* Big.sln: contains all the projects including the test projects.
* Core.sln: contains the Core assembly and it's tests.
* Installer.sln: contains the assemblys needed by the extension to be able to run it with in the Visual Studio Experimental IDE.

## Build and Test
Clone the repo and you should can start working with Windows Template Studio. All projects must build and run without any special configuration.

We are targeting Visual Studio 2017 RC (Community or higher). 

There are two main entry points in the code (depending on what you want to launch, you need to set it as "StartUp Project" in Visual Studio):
* [VsEmulator](../code/test/): This is an emulator of Visual Studio environment to work with the wizard. This emulator use a fake shell for the Visual Studio interaction and allows to run End-To-End the generation using the local templates repository. The emulator reads the local Templates folder and generate code based on the contents. This test application is thought to be able to launch and interact with the extension UI without having to run the Visual Studio Experimental instance (in other words, much more lightweight). 

* [Installer.2017](../code/src): This is the Visual Studio Extension project. You can run it from your working Visual Studio instance and will launch the [Visual Studio Experimental instance](https://msdn.microsoft.com/library/bb166560(v=vs.140).aspx) with the extension deployed to it without interfering with your working instance. During the local execution, the extension is configured to run against local Templates folder. 

In order to launch and debug the extension within the Visual Studio Experimental extension you need to configure the "Start action" for "Debug" in the project properties as:
* Start external program: pointing to devenv.exe (i.e.: C:\Program Files (x86)\Microsoft Visual Studio\2017\<your edition>\Common7\IDE\devenv.exe)
* Command line arguments: "/RootSuffix Exp"

# Inside the templates
Templates are used to generate the code. There are four different types of templates: Frameworks, Projects, Pages and Features (developer or customer focused). 

For example, you may want to generate a project to create an App which use the Split View (hamburguer) menu, based on MVVM Light framework, with some pages (Home, Products -a master detail page-, Find Us -a map page-, etc. ) and which include some developer features. Or maybe you want to create the same project but avoiding to depend on a certain external framework, in other words, you want the same stuff but using a MVVM Basic framework.  

The Window Template Studio allow to combine the templates at your own convenience to generate the project you want, using your preferred framework and using the features you most like. 

# NxM Issue and Template Authoring principles
As mentioned, you can combine Frameworks with Project Types and add Pages and Features. That is, you can have "Count(Frameworks) * Count(Project Types)" combinations and all the pages and framewoks must fit for each combination. If we have 3 frameworsk and 3 project types, we will have 9 combinations. We need to support all the pages for this combinations, so if we have 6 types of pages, we will need to maintain 9x6 = 54 Pages (mainly with the same code). The same happen for Features. Having templates created linearly is unmanageble, this is what we call NxM issue.

To avoid the NxM issue, we have designed the Templates as composable items (frameworks, projects, pages and features), which make the Template authoring a bit complex, but infinite more maintaneable in long term.

We strong follow two principles for Template authoring:
* Principle #1: Templates are composable.
* Principle #2: Avoid code duplication.

#Templates repository structure
The [Templates Repository](../../templates) have the following structure:
* Projects --> Project type templates. This kind of templates define the navigation and global layout for the app.
* Frameworks --> Framework templates. This kind of templates define the base frameworks / coding style for the app.
* Pages --> Page Templates. This kind of templates define the different types of pages available for the apps.
* DevFeatures --> Developer features templates. Define developer focused features available.
* ConsumerFeatures --> Consumer feature templates. Define consumer focused features available.

At the end, a Template is just code with some metadata. The metadata will contain the template information: name, description, licensing, remarks, programming language, type, guids, etc. The templates definition is based on [dotnet Template Engine](https://github.com/dotnet/templating), you can visit their repo for deep understanding on how the Template Engine works.

In general, when you define a template, the template will have:
* The required code and structure
* The template metadata (display name, icon, descriptions...)
* The post actions required to finish the code generation 







