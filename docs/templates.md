# Understanding the Templates

Templates are used to generate the code. In Windows Template Studio we have the following kind of templates: Frameworks, Projects Types, Pages and Features.

For example, consider the following samples:
* **Sample #1**: you want to generate a project to create a target app which use the Split View (hamburger) menu, based on MVVM Light framework, with some pages (Home, Products -a master detail page-, Find Us -a map page-, etc. ) and including some extra features like local storage handling, background execution... 
* **Sample #2** you want to create a like the *Sample #1* but avoiding to depend on a external frameworks and adding Live Tiles support.

The Window Template Studio allow you to combine different templates at your own convenience to generate the project you want, using your preferred framework and using the features you most like. Moreover, the templates available in Windows Template Studio are extensible.

## Interesting in contributing

Do you want to contribute? Here are our [contribution guidelines](../contributing.md).

## Template Authoring

### The MxN Issue
Windows Template Studio works as a shopping basket where a developer can combine the available Frameworks with the available projects Project Types and add the Pages and Features wanted for the target application. This leads to a complexity issue. Consider we have 3 frameworks (Fx) and 3 project types (Pj), then we will have 9 combinations, that is *Fx X Pj* app configurations. Now, consider we want to have 6 different types of Pages (P), all supported among the different app configurations, so we will need to maintain 9x6 = 54 pages, that is, *Fx X Pj X P* pages, with basically the same code. The same happen for Features (F), considering 6 types of features, we will have 9x6 = 54 features to maintain. 

Creating templates linearly is unmanageable, we would require to maintain Fx X Pj X (P + F) *[3 x 3 x (6 + 6)=108]* different templates just to be able to combine all together under our preferences, but if the page types and/or features grow, then the number templates to maintain grow faster. This is what we call **The M*N issue**.

To avoid the M*X issue, we have designed the Templates as composable items (frameworks, projects, pages and features), starting from the template definition from [dotnet Template Engine](https://github.com/dotnet/templating) and extending it  to allow to define compositions and post-actions to reduce the template proliferation. The drawback here is that we have had to design a couple of software elements (the Composer and the Post-Actions), which makes the templates authoring a bit complex, but infinite more maintenable in long term.

### Templates authoring principles
We follow these principles for template authoring:

* **Principle #1: Templates are composable**. In general, the templates should be composable with the existing frameworks and projecte types, that is, a certain page template should be available to be generated no matter the target framework and project type.  
* **Principle #2: Reduce code duplication as much as possible**. As far as posible avoid to have templates with the same code for different frameworks and/or project types.
* **Principle #3: Balance between maintenability and complexity.** Avoiding code duplication benefits the maintenability in long term and it is always a benefit. At the same time, reducing code duplication would require much more complexity in the Composer and the Post-Actions. We need to ensure that advance developers are able to contribute authoring templates so, we need to ensure the right balance between code reusability and templates complexity. 


## Templates repository structure

Basically, a template is just code (some source files and folders structure) with some metadata which drives how the code is generated. The template metadata contains informational data (name, description, licensing, remarks, programming language, type, guids, etc.) as well as replacement data, used to replace matching text in the source content by the actual values (think of a class name). The templates definition is based on [dotnet Template Engine](https://github.com/dotnet/templating), you can visit their repo for a deep understanding on how the Template Engine works.

The [Templates Repository](../templates) have the following structure:

* [_catalog](../templates/_catalog): this folder contains the Frameworks and Project Types available catalog, including the required information and metadata (descriptions, icons, images, etc.) to be displayed in the Wizard. You can consider all the content within the *_catalog* folder as metadata for framworks and project types.

* [_composition](../templates/composition): this folder contains the partial code templates that will be generated when certain constraints are meet, including shared or specific framwork templates.  

* [Projects](../templates/Projects): Project templates which define the actual folder structure, source files and auxiliary files to create a base project.

* [Pages](../templates/Pages): Page templates which define the source files needed to create a page of a certain type.

* [Features](../templates/Features): Feature templates whith the sources required to add different features and / or capabilities to the target app.


## Anatomy of templates

As mentioned, a basic [dotnet Template Engine](https://github.com/dotnet/templating) template is defined by the following elements:
* **Metadata**: a json file within the *".template.config"* folder  information which defines the template and its properties. The metadata includes the replacements to be done.
* **Folder Structure**: A folder structure that will be maintained afer the generation is done.
* **Files**: Text files, basically, the source code, where replacements are made.

The metadata drives how the generation is done, let's see a template content sample:

``` c#
├───.template.config
│       description.md      //Rich template description in markdown. Displayed in the wizard.
│       icon.xaml           //SVG XAML definition for the template icon (.png or .jpg are accepted as well).
│       template.json       //Template definition json file
│
├───Strings
│   └───en-us
│           Resources_postaction.resw //Post-Action to be applied after main generation of this template.
│
├───ViewModels
│       BlankViewViewModel.cs         //Source file
│
└───Views
        BlankViewPage.xaml            //Source file
        BlankViewPage.xaml.cs         //Source file
```

If we generate this template using "MyTest" as page name, the result will be as follows:
``` c#
├───Strings
│   └───en-us
│           Resources.resw
│
├───ViewModels
│       MyTestViewModel.cs
│
└───Views
        MyTestPage.xaml
        MyTestPage.xaml.cs
```

Yo can observe that the folder structure is maintained but in the source files the "BlankView" word has been replaced by "MyTest" (the actual value for the *sourceName* parameter).

The replacements are done based on the configuration established in the *template.json* file. Let's have a look to it:

``` json
{
  "author": "Microsoft",
  "classifications": [
    "Universal"
  ],
  "name": "Blank",
  "groupIdentity": "wts.Page.Blank",              //Used for filtering and grouping in the wizard
  "identity": "wts.Page.Blank",         
  "shortName": "Blank Page",                      //This is the displayed name in the wizard.
  "description": "This is the most basic page.",  //This is the short description displayed in the wizard.
  "tags": {                                       //Tags are used to filter and handled the composition
    "language": "C#",
    "type": "item",
    "wts.type": "page",
    "wts.framework": "MVVMBasic|MVVMLight",       //Frameworks where this template can be used.
    "wts.version": "1.0.0",
    "wts.order": "1"
  },
  "sourceName": "BlankView",                      //The generation engine will replace any occurence of "BlankView" by the parameter provided in the source file name.               
  "preferNameDirectory": true,
  "PrimaryOutputs": [                             //The primary outputs are the list of items that are returned to the caller after the generation.
    {
      "path": ".\\Views\\BlankViewPage.xaml"
    },
    {
      "path": ".\\Views\\BlankViewPage.xaml.cs"
    },
    {
      "path": ".\\ViewModels\\BlankViewViewModel.cs"
    }
  ],
  "symbols": {                                    //Symbols define a collection of replacements to be done while generating.        
    "rootNamespace": {
      "type": "parameter",
      "replaces": "RootNamespace"                //Each instance of "RootNamespace" in the source files will be replaced by the actual value passed in the "rootNamespace" parameter.
    },
    "itemNamespace": {
      "type": "parameter",
      "replaces": "ItemNamespace"
    },
    "baseclass": {
      "type": "parameter",
      "replaces": "System.ComponentModel.INotifyPropertyChanged"
    },
    "setter": {
      "type": "parameter",
      "replaces": "Set"
    }
  }
}
```

Pages, DevFeatures and CustomerFeatures templates are Framework and Project Type agnostic. We use two mechanisms to be able to compose and configure the templates to create actual projects, Compositions and Post-Actions.


TBD: Templates and Generation
TBD: Mention Templates Composer && Post Actions


## Composable Generation

*TODO:* Explain how the composable generation works

mention the composer

## Post Actions

Before drilling down in Framework and Project Type templates, there is an important concept we use to be able to maximize the code reuse and be able to apply particular code requirements to the generated projects. As mentioned before, Page, DevFeatures and CustomerFeatures are Framework and Project Type agnostic. Post-Actions are designed to complement the agnostic generation and get the expected final results. Imagine we have a certain page, the final code needed to be generated for this page is slightly different depending on which framework are we are using (i.e. namespaces required) and depending on which project type we are using (i.e. the navigation).

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

            // More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            // Edit String/en-US/Resources.resw: Add a menu item title for each page
            _navigationItems.Add(ShellNavigationItem.FromType<MainPage>("Shell_Main".GetLocalized(), Symbol.Document));
            _navigationItems.Add(ShellNavigationItem.FromType<ProductsPage>("Shell_Products".GetLocalized(), Symbol.Document));
            _navigationItems.Add(ShellNavigationItem.FromType<FindUsPage>("Shell_FindUs".GetLocalized(), Symbol.Document));
            _navigationItems.Add(ShellNavigationItem.FromType<SettingsPage>("Shell_Settings".GetLocalized(), Symbol.Document));
        }
...
```

But the Pages templates are Framework and Project type agnostics, and more over, pages itself have no conscience of ShellViewModel so, how this code can be generated?. Here is when the post-actions take place. 

After the code is generated, the [PostActionFactory]() review all the generated files and, for those which the name contains "_postaction", execute the post-action defined in its content.

### Post-Action Types

*TODO:* complete types of postactions

## Frameworks Templates

This templates contains all the generation information required by specific frameworks. Basically, contains the metadata and the post actions required to apply to a certain project or page during the generation to ensure the Framework infrastructure is added.

This kind of templates have the following structure:

* **Metadata Folder** (.metadata). Information about the framework. This information is displayed in the wizard.
* **Code and folders structure**. The folder structure for this kind of templates is really important as it defines which specific code will be generated and which post-actions will be applied (as well as the post-action targets)when the user selects this kind of framework. 


## Project Type Templates

*TODO:* documentation is pretty sparce below here

### Layouts

## Basic Authoring

*TODO:* Probably in a different Page

### Add a new page

### Add a new feature


After the code is generated, the [PostActionFactory]() review all the generated files and, for those which the name contains "_postaction", execute the post-action defined in its content.

### Post-Action Types

## Frameworks Templates

This templates contains all the generation information required by specific frameworks. Basically, contains the metadata and the post actions required to apply to a certain project or page during the generation to ensure the Framework infrastructure is added.

This kind of templates have the following structure:

* **Metadata Folder** (.metadata). Information about the framework. This information is displayed in the wizard.
* **Code and folders structure**. The folder structure for this kind of templates is really important as it defines which specific code will be generated and which post-actions will be applied (as well as the post-action targets)when the user selects this kind of framework. 


## Project Type Templates

### Layouts

## Basic Authoring

*TODO:* Probably in a different Page

### Add a new page

### Add a new feature

## Advance Authoring

*TODO:* Probably in a different Page

### Add custom post-actions??

### Add new framework



## Table of Contents

* [Installing / Using the extension](getting-started-extension.md)
* [Using and extending your file->new](getting-started-endusers.md)
* [Concepts of Windows Template Studio](readme.md)
* [Getting started with the generator codebase](getting-started-developers.md)
* [**Authoring Templates**](templates.md)