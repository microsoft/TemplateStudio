# Understanding the Templates

Templates are used to generate the code. In Windows Template Studio we have the following kinds of templates: Frameworks, Projects Types, Pages and Features.

For example, consider the following samples:
* **Sample #1**: you want to generate a project to create a target app which use the Split View (hamburger) menu, based on MVVM Light framework, with some pages (Home, Products -a master detail page-, Find Us -a map page-, etc. ) and including some extra features like local storage handling, background execution... 
* **Sample #2** you want to create as in *Sample #1* but avoiding to depend on an external framework and adding Live Tiles support.

The Window Template Studio allow you to combine different templates at your own convenience to generate the project you want, using your preferred framework and using the features you most like. Moreover, the templates available in Windows Template Studio are extensible.

## Interested in contributing

Do you want to contribute? Here are our [contribution guidelines](../contributing.md).

## Template Authoring

### The MxN Issue
Windows Template Studio works as a shopping basket where a developer can choose one of the available Frameworks and one of the available projects Project Types and then add the Pages and Features wanted for the target application. This leads to a complexity issue. Consider we have 3 frameworks (Fx) and 3 project types (Pj), then we will have 9 combinations, that is *Fx x Pj* app configurations. Now, consider we want to have 6 different types of Pages (P), all supported among the different app configurations, so we will need to maintain 9x6 = 54 pages, that is, *Fx x Pj x P* pages, with basically the same code. The same happens for Features (F), considering 6 types of features, we will have 9x6 = 54 features to maintain. 

Creating templates linearly is unmanageable, we would require to maintain Fx x Pj x (P + F) *[3 x 3 x (6 + 6) = 108]* different templates just to be able to combine all together under our preferences, but if the page types and/or features grow, then the number templates to maintain grow faster. This is what we call **The M*N issue**.

To avoid the M*X issue, we have designed the Templates as composable items (frameworks, projects, pages and features), starting from the template definition from [dotnet Template Engine](https://github.com/dotnet/templating) and extending it  to allow to define compositions and post-actions to reduce the template proliferation. The drawback is that the generation becomes more complex, but infinitely more maintainable in the long term.

## Templates authoring principles
We follow these principles for template authoring:

* **Principle #1: Templates are composable**. In general, the templates should be composable with the existing frameworks and project types. That is, a certain page template should be available to be generated no matter the target framework and project type.  
* **Principle #2: Reduce code duplication as much as possible**. As far as possible, avoid to have templates with the same code for different frameworks and/or project types.
* **Principle #3: Balance between maintainability and complexity.** Avoiding code duplication benefits the maintainability in long term and is always a benefit. At the same time, reducing code duplication leads to more complexity to handle the composition and the required actions to finish a proper generation. We need to ensure that advanced developers are able to contribute authoring templates so, we need to ensure the right balance between code reusability and templates complexity. 

## Templates repository structure

Basically, a template is just code (some source files and folders structure) with some metadata which drives how the code is generated. The template metadata contains informational data (name, description, licensing, remarks, programming language, type, guids, etc.) as well as replacement data, used to replace matching text in the source content by the actual values (think of a class name). The templates definition is based on [dotnet Template Engine](https://github.com/dotnet/templating), you can visit their repo for a deep understanding on how the Template Engine works.

The [Templates Repository](../templates) has the following structure:

* [_catalog](../templates/_catalog): this folder contains the catalog of available Frameworks and Project Types, including the required information and metadata (descriptions, icons, images, etc.) to be displayed in the Wizard. You can consider all the content within the *_catalog* folder as metadata for frameworks and project types.

* [_composition](../templates/composition): this folder contains the partial code templates that will be generated when certain constraints are met, including framework specific templates.  

* [Projects](../templates/Projects): Project templates which define the actual folder structure, source files and auxiliary files to create a base project.

* [Pages](../templates/Pages): Page templates which define the source files needed to create a page of a certain type.

* [Features](../templates/Features): Feature templates with the sources required to add different features and / or capabilities to the target app.


## Anatomy of templates

As mentioned, a basic [dotnet Template Engine](https://github.com/dotnet/templating) template is defined by the following elements:
* **Metadata**: a json file within the *".template.config"* folder  information which defines the template and its properties. The metadata includes the replacements to be done.
* **Folder Structure**: A folder structure that will be maintained after the generation is done.
* **Files**: Text files, basically, the source code, where replacements are made.

The metadata drives how the generation is done, let's see a template content sample:

```
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
```
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

You can observe that the folder structure is maintained but in the source files the "BlankView" word has been replaced by "MyTest" (the actual value for the *sourceName* parameter).

The replacements are done based on the configuration established in the `template.json` file. Let's have a look to it:

```
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
  "sourceName": "BlankView",                      //The generation engine will replace any occurrence of "BlankView" by the parameter provided in the source file name.               
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

### Template Layouts

Project templates can define a default layout of pages to be considered in the wizard by adding    a `Layout.json` file within the `.template.config` folder. Using the layouts you can determine what pages are automatically added to a certain project type and if those pages are mandatory or can be removed. In other words, layout definition provides a way to pre-configure pages associated to a certain project type.

Layout.json
``` json
[
    {
        "name": "Main",
        "templateGroupIdentity": "wts.Page.Blank",
        "readonly": "true"
    }
]
```  

## Composable Templates

As we already have mentioned, templates can be composed to maximize the code reutilization. Consider a certain page (the Blank page, for example), the page source code will remain the same no matter the project type where it is embedded in. Moreover, there will be very few changes in the page source code depending on which framework we rely on. The idea behind having composable templates is to reuse as much as possible the existing code for a certain page or feature no matter the project type or framework used.

Creating composable templates is like when you are developing software and try to generalize something; it fits within the 80-20 rule, meaning that the 80% of the code is common among the callers and easy to be generalized, but the 20% have more dependencies, specific details, etc. and, by the way, it is more complex to be generalized. Considering this, we have two groups of templates in the repository: 
1. **Standard templates**: *the 80 part*, these templates are the common part of the source code, corresponding with the shared source code for projects, pages and features. This templates live in the `Projects`, `Pages` or `Features` folders of our Templates repository. Through the wizard, a user can select which project type, which pages and which features wants, those selections can be shown as a user adding items to a "generation basket".

2. **Composition templates**: *the 20 part*, these templates are thought to include the specific details required by a concrete template (a page or feature) which is going to be generated in a certain context. The context is determined by the combination of project type and framework selected by the user. Required composition templates are added to the "generation basket" automatically by the `Composer`. The composition templates lives in the project `_composition` folder of our Templates repository. 

The structure of files and folders within the `_composition` folder is just for organization, to exactly determine which *composition templates* are required to be added to the generation basket, the `Composer` evaluates all the templates available in the `_composition` folder, applying the **composition filter** defined in the `template.json` file (tag `wts.compositionFilter`). All the templates with composition filters resulting in positive matching will be added to generation basket. The following is a sample of composition filter.

```
  "tags": {
    "language": "C#",
    "type": "item",
    "wts.type": "composition",
    "wts.version": "1.0.0",
    "wts.compositionFilter": "$framework == CodeBehind|MVVMBasic & identity == wts.Proj.Blank"
 },
 ```

In this case, the template which have this configuration will be added to the generation basket when the following conditions are meet:
* The selected framework for the current generation is CodeBehind OR MVVMBasic
* There is a template within the generation basket whose `identity` property is "wts.Proj.Blank".

In other words, this template is designed to be added to the generation basket when we are generating a Blank Project Type with the CodeBehind or MVVMBasic framework.

We have a basic pseudo-language to define the composition filters. The composition filters must match the following structure:

```
  <operand field> <operator> <literal> [& <operand field> <operator> <literal options>[...]]

Where
* <operand field> := <queryable property> | <context parameter>
* <literal> := <literal> [|<literal>]
* <queryable property> -> template configuration property (`template.json`) among the following:
  - `name`
  - `identity`
  - `groupIdentity`
  - Any defined tag, i.e `language`, `type`, `language`, `wts.framework`, etc.
* <operators>
  - == -> Equals Equality
  - != -> Not equals
* <context parameter>
  - $framework -> current generation framework.
  - $projectType -> current generation project type.
```

Finally, all the templates, the *Standard* and the *Composition*, are generated by using the [dotnet Template Engine](https://github.com/dotnet/templating) standard generation. The standard generation does not support merging code from multiple files to one. In that case, we need to take advantage of other mechanism: the **Post-Actions**.

## Post Actions

Post-Actions are designed to complement the standard generation enabling certain actions to be executed over the generated files once the generation is done. 

Currently we support the following types of [Post-Actions](../code/src/Core/PostActions):
- **Merge**: merges the source code from one file into another. This Post-Action requires a special (_postaction) configuration in the templates files. 
- **Sort Usings**: this post action re-orders the `using` stements in the generated source files. 
- **Add Item To Project**: this post-action is executed to add the "PrimaryOutputs" to the target Visual Studio project (.csproj). The "PrimaryOutputs" are defined in the template.json file. 
- **Add Project To Solution**: this post-action is executed to add the a generated project to the current Visual Studio solution. 
- **Generate Test Certificate**: generate the test certificate for the UWP application and configure it in the application manifest.
- **Set Default Solution Configuration**: sets the default solution configuration in the Visual Studio sln file. With this post-action we change the default solution configuration from Debug|ARM to Debug|x86. 

### Merge Post-Action
If a template generates source files with the **_postaction** suffix, this means that the Post-Action engine needs to appear in the scene to finish the generation. 

For example, consider we are creating a SplitView project type with MVVM Basic framework, we add several pages to the project. At the end, all the pages must be registered in the navigation and added to the navigation menu, some of the final  generated code will look like:

``` csharp
//MVVM Basic, ShellViewModel.cs
...
        private void PopulateNavItems()
        {
            _primaryItems.Clear();
            _secondaryItems.Clear();

            // More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            // Edit String/en-US/Resources.resw: Add a menu item title for each page
            _primaryItems.Add(ShellNavigationItem.FromType<MainPage>("Shell_Main".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<MapPage>("Shell_Map".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<MasterDetailPage>("Shell_MasterDetail".GetLocalized(), Symbol.Document));
            _secondaryItems.Add(ShellNavigationItem.FromType<SettingsPage>("Shell_Settings".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<WebViewPage>("Shell_WebView".GetLocalized(), Symbol.Document));
            _primaryItems.Add(ShellNavigationItem.FromType<TabbedPage>("Shell_Tabbed".GetLocalized(), Symbol.Document));
        }
...
```
During the generation, each page must add the required code to register itself in the *navigation items*, even the SettingsPage knows that it must be added to the `_secondaryItems` collection instead of the `_primaryItems`. To achieve this, we relay on the Merge Post-Action, which after the generation, identify files that must be merged to generate the code we have above. Let see the details of the composition template defined for that purpose.

The `template.json` is defined as follows:
```
{
  "author": "Microsoft",
  "classifications": [
    "Universal"
  ],
  "name": "MVVMBasic.Project.SplitView.AddToSecondaryNavigationItems",
  "tags": {
    "language": "C#",
    "type": "item",
    "wts.type": "composition",
    "wts.version": "1.0.0",
    "wts.compositionFilter": "$framework == MVVMBasic & $projectType == SplitView & identity == wts.Page.Settings"
  },
```

As you can see in the composition filter, this template will be applied when the context framework is *MVVMBasic* and the project type is *SplitView* and there is a template in the generation basket with the identity equals to *wts.Page.Settings*

Here is the template layout:

```
├───.template.config
│       template.json
│
└───ViewModels
        ShellViewModel_postaction.cs //This indicates that the content of this file must be handled by the Merge Post-Action
```

Here is the content of the ShellViewModel_postaction.cs:

``` c#
using ItemNamespace.Views;
namespace ItemNamespace.ViewModels
{
    public class ShellViewModel : Observable
    {
        private void PopulateNavItems()
        {
            //^^
            _secondaryItems.Add(ShellNavigationItem.FromType<wts.ItemNamePage>("Shell_wts.ItemName".GetLocalized(), Symbol.Document));
        }
    }
}
```

The merge post action will do the following:
1. Locate a file called "ShellViewModel.cs" within the generated code.
2. Using a basic source code matching, the post-action will locate content in the `_postaction` file that is not included in the `ShellViewModel.cs` file and will insert it in the correct place. In this case:
    * Inserts the using *ItemNamespace.Views* as it is not in the ShellViewModel.cs file. Note that the token *ItemNamespace* has been replaced during the generation for the actual value. 
    * Locate the namespace for the item (matching with the generated namespace for the item)
    * Then a class with the name ShellViewModel inhering from Observable
    * Then the private method called PopularNavItems
    * The symbols `//^^` indicates that the merge must be done at the end, just before the closing `}`, without this directive the line would be inserted just below the opening `{`.
3. Once located the exactly place. The code is added in to the original source file. 

#### Merges Directives:
There are different merge directives to drive the code merging. Currently

* MacroBeforeMode `//^^`: Insert before the next match, instead of after the last match
* MacroStartGroup `//{[{` and MarcoEndGroup `}]}`: The content between `{[{` and `}]}` is considered as a block and inserted together.

## Table of Contents

* [Installing / Using the extension](getting-started-extension.md)
* [Using and extending your file->new](getting-started-endusers.md)
* [Concepts of Windows Template Studio](readme.md)
* [Getting started with the generator codebase](getting-started-developers.md)
* [**Authoring Templates**](templates.md)
