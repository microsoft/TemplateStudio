# Understanding the Templates

Templates are used to generate the code. In *Template Studio* we have the following kinds of templates: Frameworks, Projects Types, Pages, Features, Services and Testing.

For example, consider the following scenarios:

- **Scenario #1**: you want to generate a project to create a target app which uses the Split View (hamburger) menu, is based on MVVM Light framework, with some pages (Home, Products -a list details page-, Find Us -a map page-, etc. ) and including some extra features like local storage handling, background execution...
- **Scenario #2** you want to create as in *Scenario #1* but without depending on an external framework and adding Live Tiles support.

The *Template Studio* allows you to combine different templates to generate the project you want, using your preferred framework, and using the features you most like. Moreover, the templates available in *Template Studio* are extensible.

## Interested in contributing

Do you want to contribute? Here are our [contribution guidelines](../CONTRIBUTING.md).

## Anatomy of Templates and Template Authoring

Before starting make sure you read how templates are defined, composed and generated at [Understanding the Templates (Core Template Studio)](https://github.com/microsoft/CoreTemplateStudio/tree/main/docs/templates.md)

## Templates repository structure

The [Templates Repository](../templates) has the following structure:

- [Uwp](../templates/Uwp): this folder contains all templates used for UWP platform projects
  - [_catalog](../templates/Uwp/_catalog): this folder contains the catalog of available Frameworks and Project Types, including the required information and metadata (descriptions, icons, images, etc.) to be displayed in the Wizard.
  - [_comp](../templates/Uwp/_comp): this folder contains the partial code templates that will be generated when certain constraints are met, including framework specific templates.
  - [Projects](../templates/Uwp/Projects): Project templates which define the actual folder structure, source files and auxiliary files to create a base project.
  - [Pages](../templates/Uwp/Pages): Page templates define the source files needed to create a page of a certain type.
  - [Features](../templates/Uwp/Features): Feature templates with the sources required to add different features and / or capabilities to the target app.
  - [Services](../templates/Uwp/Services): Service templates with the sources required to add different services to the target app.
  - [Testing](../templates/Uwp/Testing): Testing templates with the sources required to add testing projects to the target solution.

This template repository structure is the same for [WPF templates](../templates/Wpf) and [WinUI 3 templates](../templates/WinUI).


## Supporting VB.Net and C# versions of Templates

We aim to offer all functionality for apps created using C# and VB.Net. The exception to this rule is that we do not provide VB versions when a third party framework only offers documentation or support in C#.

The expectation is that the C# version of a template will be created first and the VB.Net version created after.

The script [List-CSharp-Templates-Without-VisualBasic-Equivalents.ps1](https://github.com/microsoft/TemplateStudio/blob/main/_utils/List-CSharp-Templates-Without-VisualBasic-Equivalents.ps1) can identify C# templates without VB.Net equivalents. For this to work it relies on the C# and VB versions having comparable template folder structures and that they follow the naming conventions already in use. This is particularly important for composition templates. Because VB.Net supports fewer frameworks it may be possible to produce the same output for the VB version of an item with fewer composition templates. This should be avoided as doing so will cause the above script to produce incorrect results.

It is assumed that non-code files used by different language versions of the same template will be identical. If one needs to be modified, change the one in the C# template and then run the script [Synchronize-Files-Used-By-VisualBasic-Templates.ps1](https://github.com/microsoft/TemplateStudio/blob/main/_utils/Synchronize-Files-Used-By-VisualBasic-Templates.ps1) and this will copy the file to the equivalent VB locations.

Additionally, there are automated test called `EnsureProjectsGeneratedWithDifferentLanguagesAreEquivalent_G1_Async` and `EnsureProjectsGeneratedWithDifferentLanguagesAreEquivalent_G2_Async` that will generate an app using both language versions of supported templates and then use reflection of the generated apps to check for differences.


## Testing and verifying template contents

The tool **TemplateValidator.exe** exists to help template authors verify their templates are correctly structured and to identify common errors. It can verify the contents of an individual `template.json` file or the contents of multiple directories containing templates.
It's use is encouraged to help avoid any problems or unintended consequences that may be missed during manual testing of a new template. It is not a substitute for thorough manual testing of new templates and the automated tests for generating and building projects using all templates.

### Testing individual template.json files

TemplateValidator must be passed the `-f` flag to put it in file mode and then the path to the file you wish to validate.
When validating an individual template file it will look for missing required elements, invalid values, and common typos.

![TemplateValidator used in file mode](./resources/tools/templateValidator-f.png)

If, as in the above image the file contains no issues the message "All looks good." will be displayed. If any issues are identified these will be listed.

### Testing directories containing template files

TemplateValidator must be passed the `-d` flag to put it in directory mode and then the paths to one or more directories containing templates.
When validating template directories, the contents of templates are analyzed individually and collectively. This allows for the identification of missing files, values reused across templates that should be unique, and missing dependencies.

![TemplateValidator used in directory mode](./resources/tools/templateValidator-d.png)

The above image shows the use of the tool to look at two root directories. It lists the values it was passed and any issues it finds or the "All looks good." message.
Any issues that start with "WARNING" are recommendations that should be addressed in any new templates.

### Automated testing of template validation

The Templates.Test project includes tests to run all the checks from the TemplateValidator tool as part of the automated tests for the solution.

## Authoring templates tooling

### Visual Studio Code Snippets

We've created some Visual Studio Code Snippets to help creating the template.json files.

#### Adding code Snippets to Visual Studio Code

- Open Preferences: Configure User Snippets (Ctrl + Shift + P, type snippets).
- Open Json.json on language files list.
- Open and copy the code snippets on [TS code snippets file](.//..//_utils//code-snippets.json).
- Paste those code snippets on the opened Json.json file.

#### Using the code snippets

After creating an empty template.json file, type wts.template and click Enter, the code snippet will add a template json with different variables to complete, you can use the tab key to navigate between them.
There are also code snippets to add Tags, PrimaryOutputs, Symbols and Post Actions.


## Learn more

- [Getting started with the TS codebase](./getting-started-developers.md)
- [Templates doc in Core Template Studio](https://github.com/microsoft/CoreTemplateStudio/tree/main/docs/templates.md)
- [Recording usage Telemetry](./telemetry.md)
- [Ensuring generated code is accessible](./accessibility.md)
- [All docs](./readme.md)
