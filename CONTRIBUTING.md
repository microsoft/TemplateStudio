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
* [Shared Code](#Shared-Code)
* [Edit Project Menu](#Edit-Project-Menu)
* [Localization](#Localization)

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

When you F5 to debug or start without debugging, Visual Studio will install the extensions in the Visual Studio Experimental Instance. Once loaded, you can test the extensions by creating a new project and selecting the appropriate `Template Studio` project template.

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

Below is a checklist to follow when adding new templates:

* Copy an existing similar template as a starting point
* Set descriptive `name` and `shortName` fields in `.template.config/template.json`
* Set unique `identity` and `groupIdentity` fields in `.template.config/template.json`
  * By default, `groupIdentity` should be the same as `identity` unless you have a good reason to change it
* Update other metadata in `.template.config/template.json` as appropriate
  * See [Modifying the Wizard](#Modifying-the-Wizard) and [template.json reference](https://github.com/dotnet/templating/wiki/Reference-for-template.json)
* Update `.template.config/icon.xaml` as appropriate
  * Requests for new icons can be filed at https://aka.ms/d3icons
* Modify the content of the template
* Set `Include in VSIX` to `true` in the Properties window for all the files in the template

To determine whether code should be part of a base template vs. a composition template, ask yourself these questions:

* Is the code the same regardless of other options selected in the Template Studio wizard?
  * Yes? It should be included in a base template.
  * No? It should be included in a composition template.
* Is the code dependent on the frontend framework selected in the Template Studio wizard?
  * Yes? It should be included in a composition template under `_comp/<FRONTENDFRAMEWORK>`.
  * No? It should be included in a composition template under `_comp/_shared`.

Determining what code should be part of a base template vs. a composition template is the most important and challenging step when adding new templates. Once this is established, implementing the templates becomes straightforward.

When proposing non-trivial changes to templates, creating a git repository from a fully generated Template Studio project and then committing changes can help track and communicate the differences. The git diffs can then be used to identify net new content that should be added to base templates and changes to existing content that should be added to composition templates.

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

Conditionals can be combined with the `&` operator and fields can be compared to literal values with the `==` and `!=` operators. Literal values can be separated by `|` to achieve OR logic.

New files can be contributed to the base Project template by adding them to the composition template following the folder structure of the base Project template.

Existing files can be modified with [merge post actions](https://github.com/microsoft/CoreTemplateStudio/blob/dev/docs/templates.md#merge-post-action). Merge post actions are what allow you to modify existing files in the template. For example, if you want to add or remove properties from the base Project template .csproj file, you can do so in the composition template with a merge post action applied to the .csproj file.

Merge post actions on a file are defined by adding a file to the composition template with the same name as the original file but with a _postaction suffix before the file extension (e.g. `Param_ProjectName_postaction.csproj`).

Merge post action files use pattern matching and special comments to identify a location within the original file and then either add or remove content at that location.

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <EnableMsixTooling>true</EnableMsixTooling>
<!--{--{-->
    <WindowsPackageType>None</WindowsPackageType>
<!--}--}-->
  </PropertyGroup>
```

The above merge post action file would search for the `EnableMsixTooling` property and then remove `<WindowsPackageType>None</WindowsPackageType>` found after that location by surrounding the line to remove with XML comments that use the `{--{` and `}--}` syntax. This syntax indicates that the content within the comment should be removed from the original file. See https://github.com/microsoft/CoreTemplateStudio/blob/dev/docs/templates.md#post-actions for more details on merge post action syntax as well as other types of post actions that can change the output after generation has occurred. Note that the special comments should use the comment syntax corresponding to the file being modified. `.xaml` and `.csproj` files should use XML comment syntax and `.cs` files should use C# comment syntax.

### Modifying the Wizard

The Template Studio wizard enables developers to produce a custom project template based on the options they select. To add new options to the wizard or to modify the relationships or dependencies between options, you'll need to modify one or more of the following:

* Add a new project type to _catalog/projectTypes.json
  * Examples of project types include Blank, SplitView, and MenuBar.
* Add a new frontend framework to _catalog/frontendframeworks.json
  * Examples of frontend frameworks include MVVMToolkit and ReactiveUI.
* Add `ts.projecttype` and `ts.frontendframework` filters to base template.json files
  * Enables you to associate a template with specific project types and/or frontend frameworks
* Add `ts.dependencies` to base template.json files to associate dependencies between templates which will link them in the wizard
* Add `ts.group` to base template.json files to group features in the wizard
  * This requires adding a corresponding `TemplateGroup_<GROUPNAME>` string resource in SharedResources/Resources.resx.
* Add `ts.displayOrder` to base template.json files to change the display order in the wizard
* Add `description.md` and `icon.xaml` files to base .template.config folders to update the description and icon presented in the wizard
* Add a `Layout.json` file to a base Project template
  * Enables you to mark a feature as readonly

Each template contains a `.template.config/icon.xaml` file that defines the icon that is displayed in the wizard for that template. Visual Studio designers are available to help recommend icons from their [existing icon library](https://d3assets.azurewebsites.net/icons/vswin2022) or to design new icons. Requests for new icons can be filed at https://aka.ms/d3icons.

Note: Composition templates within the _comp folder do not alter the wizard. They only modify the base templates based on the options selected in the wizard.

## Shared Code

The displayed wizard and the logic that generates an app from the selected templates is in the `SharedFunctionality.UI` and `SharedFunctionality.Core` projects. When working on these it is important to be working in the `TemplateStudio.sln` file and not a `*.slnf` file. You should also use the `DebugAll` configuration as this ensures that all projects are compiled, which is important as these shared projects are included in all the extensions. Switch to using a configuration specific to the extension you are testing when wanting to debug any changes.

Note that because of the limited support for working with XAML files inside a shared project used by an extension, you may get misleading compilation errors if you have any of the `.xaml` files open in the editor when compiling. Simply close the file(s) and any spurious errors will go away.

## Edit Project Menu

Developers can add Template Studio templates to an existing project by opening the Project context menu and selecting an option in the `Add -> New Item (Template Studio)` submenu. In order for this menu to be presented, the project must [match the project type](https://docs.microsoft.com/visualstudio/extensibility/how-to-use-rule-based-ui-context-for-visual-studio-extensions) supported by the extension, and the project must contain Template Studio metadata in either `Package.appxmanifest` or `TemplateStudio.xml` at the root of the project.

The layout and location of this menu are defined in [`.vsct`](code/TemplateStudioForWinUICs/Commands/TemplateStudioForWinuiPackage.vsct) files per extension. See [Author .vsct files](https://docs.microsoft.com/visualstudio/extensibility/internals/authoring-dot-vsct-files) for details. To change the location of the menu, change the `id` of the root `CommandPlacement` element to the ID of the Visual Studio Menu or Group that should contain the menu. These IDs are defined within `C:\Program Files\Microsoft Visual Studio\2022\Community\VSSDK\VisualStudioIntegration\Common\Inc\vsshlids.h`. The easiest way to identify these IDs from the Visual Studio UI is to install Visual Studio 2019 and the [Command Explorer extension](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.CommandExplorer).

Note: If the parent Visual Studio item is a Menu (i.e. begins with `IDM_`), then the root element in the `.vsct` should be a Group. If the parent Visual Studio item is a Group (i.e. begins with `IDG_`), then the root element in the `.vsct` should be a Menu.

The handlers for the menu items are defined in files like [TemplateStudioForWinUICsPackage.cs](code/TemplateStudioForWinUICs/TemplateStudioForWinUICsPackage.cs) per extension. These handlers include the function to invoke when the menu item is clicked as well as a `RightClickAvailable` function to determine whether to show each of the menu items.

Templates contain a `ts.rightClickEnabled` tag in `template.json` that controls whether they are presented in the wizard when invoked via the Edit Project menu. Set this tag to `true` to enable the template in the wizard and `false` to disable it.

The below extensions may be useful when debugging changes to the Edit Project menu:

* [Command Explorer](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.CommandExplorer) for identifying Visual Studio Menu and Group IDs
* [Component Diagnostics](https://marketplace.visualstudio.com/items?itemName=PaulHarrington.ComponentDiagnosticsDev17) for checking if the Menu package is registered properly in the Package Manager

Note: The Experimental Instance often doesn't recognize changes to the `.vsct` file which can result in the menu not showing up or UI changes not being reflected. Running `. "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\devenv.exe" /RootSuffix Exp /setup` from PowerShell will trigger the Experimental Instance to rebuild its menus. If that doesn't work, then uninstalling all versions of the extension and [resetting the Experimental Instance](#Validating-changes) sometimes helps. The most reliable way to test menu changes is to build a Release build of the VSIX and install it into the main instance of Visual Studio.

## Localization

Template Studio uses a Microsoft service called TDBuild to handle localization. The [localization pipeline](_build/pipelines/localization.yml) submits English resources to TDBuild on a nightly basis which queues them up for localization. The pipeline publishes localized resources as artifacts, so once localization is finished on the backend, future pipeline runs will include the resources in the published artifacts. Download the `TDBuild.tar.gz` artifact from the pipeline, run `tar -xf TDBuild.tar.gz` to extract the localized resources to a `TDBuild` folder, then from within the repo run [Import-TDBuild](_build/Import-TDBuild.ps1) with the path to the `TDBuild` folder to import the files for submission.

The resources that are submitted to TDBuild include:

* `**/FrontendFrameworks/FrontendFrameworks.json`
* `**/ProjectTypes/ProjectTypes.json`
* `**/.template.config/localize/templatestrings.json`
* `**/SharedResources/Resources.resx`

JSON files use the `resjson` format which includes comments describing the resources that should be localized and how they are presented in the UI. This provides context to translators which helps improve the quality of the translations. For any strings that should not be localized, the comment should be `"{Locked}"`.

Templates are localized using the [pattern](https://github.com/dotnet/templating/wiki/Localization) defined by the .NET Templating Engine. English strings are loaded from `template.json`, but the localizable strings must also be defined in `localize/templatestrings.json` in order for TDBuild to localize them.

Other files that can be localized but are not integrated with TDBuild include:

* `**/*.md`
* `**/*.vsct`
* `**/*.vsixmanifest`
* `**/*.vstemplate`

Either TDBuild doesn't support these file formats, or the strings are already localized and not expected to change very frequently.

## Validating changes

F5 or start without debugging to launch the extensions in the Visual Studio Experimental Instance and validate changes. Once the Experimental Instance is loaded, create a new project and select the appropriate `Template Studio` project template.

Below is a checklist to follow when validating changes:

* Are changes to the new project wizard reflected as expected?
* Can you create new projects without errors?
* Does the generated code look as expected?
* Does the generated project build and deploy without errors?
* Do all relevant tests pass?

Once all changes pass basic validation, submit them for review by filing a pull request.

When developing extensions, the Experimental Instance of Visual Studio can sometimes become corrupt, preventing deployment and debugging of the extension. This usually surfaces as deployment errors when trying to deploy the extension or as silent failures that result in the Template Studio project templates not showing up in the New Project dialog. To recover from this state, you can reset the Experimental Instance by closing all instances of Visual Studio, then running `Reset the Visual Studio 2022 Experimental Instance` from the `Visual Studio 2022` folder in the Start Menu. If this doesn't work, uninstalling and reinstalling Visual Studio will always return you to a working state.

## Filing a pull request

All contributions are expected to be reviewed and merged via pull requests into the main branch.

In addition to ensuring all extensions build successfully, the CI pipelines run all tests in the Minimum* Groups. If any of these tests fail, the pull request will be blocked from merging.

The pull request template lists additional considerations when modifying templates. Ensure all considerations are accounted for in your changes and update the pull request description accordingly.

Once all required tests pass and the changes are approved, the pull request will be merged.
