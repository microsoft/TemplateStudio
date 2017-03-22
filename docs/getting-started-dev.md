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

# Templates repository structure
The [Templates Repository](../../templates) have the following structure:
* Projects --> Project type templates. This kind of templates define the navigation and global layout for the app.
* Frameworks --> Framework templates. This kind of templates define the base frameworks / coding style for the app.
* Pages --> Page Templates. This kind of templates define the different types of pages available for the apps.
* DevFeatures --> Developer features templates. Define developer focused features available.
* ConsumerFeatures --> Consumer feature templates. Define consumer focused features available.

At the end, a Template is just code with some metadata. The metadata will contain the template information: name, description, licensing, remarks, programming language, type, guids, etc. The templates definition is based on [dotnet Template Engine](https://github.com/dotnet/templating), you can visit their repo for deep understanding on how the Template Engine works.

## Templates anatomy
In general, a template will have a folder and files structure which defines itself as a template. Depending on the kind of template the internal structure may differ but, in general, all the Templates define some metatada and a folder structure with files where the Template Engine apply the replacements.

TBD: Templates and Generation
TBD: Mention Templates Composer && Post Actions

### Pages, DevFeatures and CustomerFeatures Templates
This kind of templates have the following structure:
* **Metadata Folder** (.template.config). Here we can find the template definition file (this is the [dotnet Template Engine](https://github.com/dotnet/templating) definition file). In this file is defined the metadata used while the engine make the code generation.
* **Code and folders structure**. This structure will be maintaned once generated.

Let examine the template anatomy for the ["Blank" Page](../../templates/Pages/Blank):
* .template.config: Folder containing the Template metada configuration.
    * icon.png: icon that will be shown in the Wizard.
    * template.json: this is the template definition file itself. The template definition file determine which replacements will be done and which information will be returned to the generation engine. For example, the template definition defines "Blank" as "name" for the template. This means that "Blank" will be replaced by the parametrized name during the generation. 
    * View: Folder containing the source code files for the page view (.xaml, and .xaml.cs)
    * ViewModel: Folder containing the source files for the view model. 

Following you can see some details from the template.json file.

Templates Properties:

``` json
{
  "author": "Microsoft",
  "classifications": [
    "Universal"
  ],
  "name": "Blank",
  "groupIdentity": "Microsoft.UWPTemplates.BlankPage",
  "identity": "Microsoft.UWPTemplates.BlankPage.CSharp",
  ...
```

Custom tags (starting with 'utc.') that are used to organize the templates information in the Wizard and drive the generation :

``` json
    "tags": {
        "language": "C#",
        "type": "item",
        "uct.type": "page",
        "uct.framework": "Basic|MVVMLight",
        "uct.version": "1.0.0",
        "uct.order": "1"
    },
    ...
```

If we generate this template using the name "MyPage", the result will be a folder structure as follow:

* View (Folder)
    * MyPageView.xaml
    * MyPageView.xaml.cs
* ViewModel (Folder)
    * MyPageViewModel.cs

Pages, DevFeatures and CustomerFeatures templates are Framework and Project Type agnostic. Once generated, some Post-Actions are executed to adjust the generation to the framework and project type select by the user. 

## Composable Generation

<TBD> Explain how the composable generation works

mention the composer

## Post Actions
Befor drilling down in Framework and Projecty Type templates, there is an important concept we use to be able to maximize the code reuse and be able to apply particular code requirements to the generated projects. As metinoned before, Page, DevFeatures and CustomerFeatures are Framework and Project Type agnostic. Post-Actions are designed to complement the agnostic generation and get the expected final results. Imagine we have a certain page, the final code needed to be generated for this page is slightly different depending on which framework are we are using (i.e. namespaces required) and depending on which project type we are using (i.e. the navigation). 

For example, consider we are creating a SplitView project type with MVVM Basic framework, we add several pages to the project. At the end, all the pages must be registered in the navigation and added to the navigation menu, the generated code will looks like:

MVVM Basic, ShellViewModel.cs

``` csharp
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using App147.Model;
using App147.View;
using App147.Mvvm;
using App147.Services;
...
        private void PopulateNavItems()
        {
            _navigationItems.Clear();

            //More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            //Edit String/en-US/Resources.resw: Add a menu item title for each page
            _navigationItems.Add(ShellNavigationItem.FromType<MainPage>("Shell_Main".GetLocalized(), Symbol.Document));
            _navigationItems.Add(ShellNavigationItem.FromType<ProductsPage>("Shell_Products".GetLocalized(), Symbol.Document));
            _navigationItems.Add(ShellNavigationItem.FromType<FindUsPage>("Shell_FindUs".GetLocalized(), Symbol.Document));
            _navigationItems.Add(ShellNavigationItem.FromType<SettingsPage>("Shell_Settings".GetLocalized(), Symbol.Document));
        }
...
```  

But the Pages templates are Framework and Project type agnostics, and more over, pages itself have no conscience of ShellViewModel so, how this code can be generated?. Here is when the post-actions take place. 
<<<<<<< HEAD

After the code is generated, the [PostActionFactory]() review all the generated files and, for those which the name contains "_postaction", execute the post-action defined in its content.

### Post-Action Types
TBD: complete types of postactions
### 

## Frameworks Templates
This templates contains all the generation information required by specific frameworks. Basically, contains the metadata and the post actions required to apply to a certain project or page during the generation to ensure the Framework infrastructure is added.

This kind of templates have the following structure:
* **Metadata Folder** (.metadata). Information about the framework. This information is displayed in the wizard.
* **Code and folders structure**. The folder structure for this kind of templates is really important as it defines which specific code will be generated and which post-actions will be applied (as well as the post-action targets)when the user selects this kind of framewok. 


## Project Type Templates
### Layouts

## Basic Authoring
TBD -> Probably in a differnt Page

### Add a new page

### Add a new feature

=======

After the code is generated, the [PostActionFactory]() review all the generated files and, for those which the name contains "_postaction", execute the post-action defined in its content.

### Post-Action Types

### 

## Frameworks Templates
This templates contains all the generation information required by specific frameworks. Basically, contains the metadata and the post actions required to apply to a certain project or page during the generation to ensure the Framework infrastructure is added.

This kind of templates have the following structure:
* **Metadata Folder** (.metadata). Information about the framework. This information is displayed in the wizard.
* **Code and folders structure**. The folder structure for this kind of templates is really important as it defines which specific code will be generated and which post-actions will be applied (as well as the post-action targets)when the user selects this kind of framewok. 


## Project Type Templates
### Layouts

## Basic Authoring
TBD -> Probably in a differnt Page

### Add a new page

### Add a new feature

>>>>>>> b3995124d2106e965090226beb662b32b7bfda7c
## Advance Authoring
TBD -> 
### Add custom post-actions??

### Add new framework



