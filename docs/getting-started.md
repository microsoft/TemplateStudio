Getting Started with the Windows Template Studio
===============
This section have the main concepts and definitions used in Windows Template Studio.

To start working with the latest version of this code, check [Getting Started for Developers](getting-started-developers.md)

To start using the Windows Template Studio extension, [Getting Started with the Extension](getting-started-extension.md)

To start authoring templates, check [Understanding the Templates](docs/templates.md)

If you plan to contribute, please follow the [contribution guidelines](https://github.com/Microsoft/WindowsTemplateStudio/blob/master/contributing.md) and remeber that the Pull Requests must be done aganist the "[dev](https://github.com/Microsoft/WindowsTemplateStudio/tree/dev)" branch.

## Main concepts
Windows Template Studio is divided in the following main elements:
* **Windows Template Studio Extension** --> This is a Visual Studio Extension project, which allows to install a new Visual Studio Project Template allowing the End-Users to have an improved experience when they want to create a new UWP App from the "File -> New Project...".  
* **Generation Wizard** --> Once the End-User select the "Windows Template Studio" project type in the Visual Studio "File -> New Project..." dialog, the Generation Wizard will guide him through some steps to create users's preferred project. The Generation Wizard allows the user to select among the available Project Types, Frameworks, Pages, Developer Features and Customer Features 
* **Templates** --> This is the repository of code templates used from the Generation Wizard. The templates are pieces of code used to generate the final project templates.

### What is a Template?
A template is just code with some metadata. The metadata will contain the template information: name, description, licensing, remarks, programming language, type, guids, etc. The template definition is based on [dotnet Template Engine](https://github.com/dotnet/templating).

There are five different types of templates:
* **Framework Templates**: Will contain the code used as infrastructure for the projects.
* **Project Template**: project templates define the type of App you are about to generate (Basic, Split View, Tabbed...). 
* **Page Template**: will contain the files and code needed to add a page to a certain App.
* **Developer Feature Templates**: will contain the files and code needed to add features focused in developer experience.
* **Customer Feature Templates**: will contain the files and code needed to add features focused in the final-user.

### What is the Generation Wizard
The Generation Wizard guide the user through the available templates allowing the user to compose an App project of his preference.

The End-User can select among the different Frameworks and Project Types to define the basic layaout and base framework for his App. Then can add different Pages, Developer Features and Customer Features to complete his app. Once the user finish with the templates selections, the generation process is executed to create final App project code. 

The generation is made in a composite way, where the Pages, Dev Features and Customer Features are Framework and Project Type agnostics and is only at the generation time where the particularities are included.  

As well as templates, the generation is based on [dotnet Template Engine](https://github.com/dotnet/templating) code generation.

### What is the Template Repository?
The Templates Repository is the place where we will gather all templates and will make them available for the community (hosted on a CDN). We will have two repositories publicly available:
* Master: The stable and public version of the templates.
* VNext: The ongoing version of the templates.

### What is the Visual Studio Extension?
UWP App developers can take advantage of Windows Template Studio by installing our Visual Studio extension. This extension will allow developers to create Apps, with pages and/or features based on the Templates available in the public Repository. 

## Getting Started for Developers
To start working with the latest version of this code, check [Getting Started for Developers](getting-started-developers.md)

## Getting Started with the Exension
To start using the Windows Template Studio extension, [Getting Started with the Extension](getting-started-extension.md)