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

When contributing template changes, [validate](#Validating-changes) your changes by generating projects with updated templates, then file a PR to trigger CI validation.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Set up your development environment

* Install [Visual Studio 2022 (.NET Desktop and Visual Studio Extension Workloads)](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&channel=Release&version=VS2022&source=VSLandingPage&passive=false)
* Install [Visual Studio Code](https://code.visualstudio.com/Download)
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

## Templates

The templates for a given Template Studio extension live within the Templates folder for that extension (e.g. [code/TemplateStudioForWinUICs/Templates](https://github.com/microsoft/TemplateStudio/tree/main/code/TemplateStudioForWinUICs/Templates) for the Template Studio for WinUI (C#) extension). If you are adding new options to the Template Studio wizard or fixing bugs in existing templates, you'll primarily be working within these folders.

### Template Structure

To avoid the maintenance overhead and combinatorial complexity of static templates for every combination of options in the wizard, Template Studio templates are dynamically composed based on the selected options. For every project created with the wizard, there is a base project as well as additional templates that extend or modify the base project. Below are the various folders that make up these components:

* Proj - defines base project templates
* Pg - defines base Page templates
* Ft - defines base Feature templates
* Serv - defines base Services templates
* Test - defines base Testing templates
* _comp - defines composition fragments

Base templates contain the core part of the templates that are unaffected by other options selected in the wizard. Composition fragments within _comp conditionally modify the base templates based on the options selected in the wizard (e.g. to add an MVVMToolkit ViewModel to base Page templates when the MVVMToolkit frontend framework is selected).

Each template has a `.template.config/template.json` file that defines metadata for the template as well as any conditionals that apply to their application, also known as composition filters. Template Studio templates are based on the .NET Templating Engine, so the `template.json` format is inherited from .NET. Template Studio extended the .NET model to support composition filters.

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

Note: Composition filters within the _comp folder do not alter the wizard. They only modify the base templates based on the options selected in the wizard.

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

Once all required tests pass and the request is approved, the pull request can be merged.