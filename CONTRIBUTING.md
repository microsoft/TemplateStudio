# Contributing

This project welcomes contributions and suggestions. Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

You can contribute to this project by contributing to:

* [Issues](https://github.com/microsoft/TemplateStudio/issues)
* [Discussions](https://github.com/microsoft/TemplateStudio/discussions)
* [Templates](#Templates)

If you intend to contribute code changes, learn how to [set up your development environment](#Set-up-your-development-environment).

When contributing template changes, [validate](#Validating-changes) your changes by generating projects with updated templates and running appropriate tests, then file a PR to trigger CI validation.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Set up your development environment

* Install [Visual Studio 2022 (.NET Desktop and Visual Studio Extension Workloads)](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&passive=false)
* Install [Visual Studio 2022 Extensibility Essentials](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.ExtensibilityEssentials2022)
* Install [Visual Studio Code](https://code.visualstudio.com/Download) for easier navigation of the repo and improved Markdown editor
* Clone the [repo](https://github.com/microsoft/TemplateStudio.git)

## Extensions

[TemplateStudio.sln](https://github.com/microsoft/TemplateStudio/blob/main/code/TemplateStudio.sln) is a multi-project solution that contains all of the extension projects and associated tests and assets. Each extension lives under a corresponding `TemplateStudioFor*` project in this solution.

There are also `*.slnf` files that load the subset of projects needed for a specific extension if you want a more focused and potentially more performant workspace:

* [TemplateStudio.WinUICs.slnf](https://github.com/microsoft/TemplateStudio/blob/main/code/TemplateStudio.WinUICs.slnf)
* [TemplateStudio.WinUICpp.slnf](https://github.com/microsoft/TemplateStudio/blob/main/code/TemplateStudio.WinUICpp.slnf)
* [TemplateStudio.WPF.slnf](https://github.com/microsoft/TemplateStudio/blob/main/code/TemplateStudio.WPF.slnf)
* [TemplateStudio.UWP.slnf](https://github.com/microsoft/TemplateStudio/blob/main/code/TemplateStudio.UWP.slnf)

Regardless of the solution you load, you need to make sure the extension project you want to work on is set as the startup project, and you need to ensure you select an appropriate Debug or Release configuration specific to that extension (e.g. `DebugWinUICs`/`ReleaseWinUICs`).

When you F5 to debug or start without debugging, Visual Studio will install the extensions in the Visual Studio Experimental Instance. Once loaded, you can test the extensions by creating a new project and selecting the appropriate `Template Studio for *` template.

You can also build and run the [VSEmulator](https://github.com/microsoft/TemplateStudio/tree/main/code/test/VsEmulator) to iterate faster on templates without needing to rebuild and redeploy the extensions. The VSEmulator reads template content directly from the local repository so will immediately honor any changes you make to the templates without needing to be rebuilt.

## Templates

The templates for a given Template Studio extension live within the `Templates` folder for that extension (e.g. [code/TemplateStudioForWinUICs/Templates](https://github.com/microsoft/TemplateStudio/tree/main/code/TemplateStudioForWinUICs/Templates) for the Template Studio for WinUI (C#) extension). If you are adding new options to the Template Studio wizard or fixing bugs in existing templates, you'll primarily be working within these folders. Below are three core concepts to understand when working on templates:

* [Template Structure](#Template-Structure)
* [Composition Templates](#Composition-Templates)
* [Modifying the Wizard](#Modifying-the-Wizard)

### Template Structure

To avoid the maintenance overhead and combinatorial complexity of static templates for every combination of options in the wizard, Template Studio templates are dynamically composed based on the selected options. For every project created with the wizard, there is a base project as well as additional templates that extend or modify the base project. Below are the various folders that make up these components:

* Proj - defines base Project templates
* Pg - defines base Page templates
* Ft - defines base Feature templates
* Serv - defines base Services templates
* Test - defines base Testing templates
* _comp - defines composition templates

Base templates contain the core part of the templates that are unaffected by other options selected in the wizard. Composition templates within _comp conditionally modify the base templates based on the options selected in the wizard (e.g. to add an MVVMToolkit ViewModel to base Page templates when the MVVMToolkit frontend framework is selected).

Each template has a `.template.config/template.json` file that defines metadata for the template as well as any conditionals that apply to their application, also known as composition filters. Template Studio templates are based on the [.NET Templating Engine](https://github.com/dotnet/templating/wiki/Reference-for-template.json), so the `template.json` format is inherited from .NET. Template Studio extends the .NET model to support composition filters.

### Composition Templates

Composition templates are just like other templates except that they include a `ts.compositionFilter` tag in `template.json` that qualifies when they will be applied.

```json
{
    "$schema": "http://json.schemastore.org/template",
    "author": "Microsoft",
    "classifications": [
      "Universal"
    ],
    "name": "ts.WinUI.Project.Blank",
    "shortName": "ts.WinUI.Project.Blank",
    "identity": "ts.WinUI.Project.Blank",
    "tags": {
      "language": "C#",
      "type": "item",
      "ts.type": "composition",
      "ts.platform": "WinUI",
      "ts.version": "1.0.0",
      "ts.compositionOrder": "0",
      "ts.compositionFilter": "$frontendframework == MVVMToolkit & $projectType == Blank & ts.type == project"
    },
    "sourceName": "Param_ItemName",
    "preferNameDirectory": true,
    "PrimaryOutputs": [
    ],
    "symbols": {
    }
  }
```

In the above example, you can see from the `ts.compositionFilter` tag that this template will be applied when the selected frontend framework (`$frontendframework`) is `MVVMToolkit`, the selected project type is `Blank` (`$projectType`), and the template being processed is of type `project` (`ts.type`). The matching template that satisfies these conditions is below.

```json
{
  "$schema": "http://json.schemastore.org/template",
  "author": "Microsoft",
  "classifications": [
    "Universal"
  ],
  "name": "ts.WinUI.Proj.Default",
  "shortName": "ts.WinUI.Proj.Default",
  "identity": "ts.WinUI.Proj.Default",
  "groupIdentity": "ts.WinUI.Proj.Default",
  "description": "",
  "tags": {
    "language": "C#",
    "type": "project",
    "ts.type": "project",
    "ts.projecttype": "Blank|SplitView|MenuBar",
    "ts.frontendframework": "MVVMToolkit",
    "ts.platform": "WinUI",
    "ts.appmodel": "Desktop",
    "ts.outputToParent": "true",
    "ts.version": "1.0.0",
    "ts.displayOrder": "0",
    "ts.licenses": "[Microsoft.WindowsAppSDK](https://www.nuget.org/packages/Microsoft.WindowsAppSDK/1.0.1/License)"
  },
  ...
}
```

The `ts.compositionFilter` indicates that the composition template will be applied to this base Project template when the conditions in the filter are met.

Below are the fields that can be evaluated in a composition filter:

* template.json fields
  * `name`
  * `identity`
  * `groupIdentity`
  * any `tag`
* context parameters
  * `$frontendframework`
  * `$projectType`

Conditionals can be combined with the `&` operator and fields can be compared to literal values with the `==` and `!=` operators.

New files can be contributed to the base Project template by adding them to the composition template following the folder structure of the base Project template.

Existing files can be modified with [merge post actions](https://github.com/microsoft/CoreTemplateStudio/blob/dev/docs/templates.md#merge-post-action). Merge post actions are what allow you to modify existing files in the template. For example, if you want to add or remove properties from the base Project template .csproj file, you can do so in the composition template with a merge post action applied to the .csproj file.

Merge post actions on a file are defined by adding a file to the composition template with the same name as the original file but with a _postaction suffix before the file extension (e.g. `Param_ProjectName_postaction.csproj`).

Merge post action files use pattern matching and special comments to identify a location within the original file and then either add or remove content at that location.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <EnablePreviewMsixTooling>true</EnablePreviewMsixTooling>
<!--{--{-->
    <WindowsPackageType>None</WindowsPackageType>
<!--}--}-->
  </PropertyGroup>
```

The above merge post action file would search for the `EnablePreviewMsixTooling` property and then remove `<WindowsPackageType>None</WindowsPackageType>` found after that location by surrounding the line to remove with XML comments that use the `{--{` and `}--}` syntax. This syntax indicates that the content within the comment should be removed from the original file. See https://github.com/microsoft/CoreTemplateStudio/blob/dev/docs/templates.md#post-actions for more details on merge post action syntax as well as other types of post actions that can change the output after generation has occurred.

### Modifying the Wizard

The Template Studio wizard enables developers to produce a custom project template based on the options they select. To add new options to the wizard or to modify the relationships or dependencies between options, you'll need to modify one or more of the following:

* Add a new project type to _catalog/projectTypes.json
  * Examples of project types include Blank, SplitView, and MenuBar.
* Add a new frontend framework to _catalog/frontendframeworks.json
  * Examples of frontend frameworks include MVVMToolkit and ReactiveUI.
* Add `ts.projecttype` and `ts.frontendframework` filters to base template.json files
  * Enables you to associate a template with specific project types and/or frontend frameworks
* Add `ts.dependencies` to base template.json files to associate dependencies between templates which will link them in the wizard
* Add a `Layout.json` file to a base Project template
  * Enables you to mark a feature as readonly

Note: Composition templates within the _comp folder do not alter the wizard. They only modify the base templates based on the options selected in the wizard.

## Updating the shared code

The displayed wizard and the logic that generates an app from the selected templates is in the `SharedFunctionality.UI` and `SharedFunctionality.Core` projects. If wanting to work on these it is important to be working in the `TemplateStudio.sln` file and not a `*.slnf` file. You should also use the `DebugAll` configuration as this ensures that all projects are compiled, which is important as these shared projects are included in all the extensions. Switch to using a configuration specific to the extension you are testing when wanting to debug any changes.

It may also be useful to note that because of the limited support for working with XAML files inside a shared project used by an extension, you may get misleading compilation errors if you have any of the `.xaml` files open in the editor when compiling. Simply close the file(s) and any spurious errors will go away.

## Validating changes

F5 or start without debugging to launch the extensions in the Visual Studio Experimental Instance and validate changes. Once the Experimental Instance is loaded, create a new project and select the appropriate `Template Studio for *` template.

Below is a checklist to follow when validating changes:

* Are changes to the new project wizard reflected as expected?
* Can you create new projects without errors?
* Does the generated code look as expected?
* Does the generated project build and deploy without errors?
* Do all relevant tests pass?

Once all changes pass basic validation, submit them for review by filing a pull request.

## Filing a pull request

All contributions are expected to be reviewed and merged via pull requests into the main branch.

In addition to ensuring all extensions build successfully, the CI pipelines run all tests in the Minimum* Groups. If any of these tests fail, the pull request will be blocked from merging.

The pull request template lists additional considerations when modifying templates. Ensure all considerations are accounted for in your changes and update the pull request description accordingly.

Once all required tests pass and the changes are approved, the pull request will be merged.
