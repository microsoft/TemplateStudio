# Concepts of Windows Template Studio

This section have the main concepts and definitions used in Windows Template Studio.

## Main concepts

Windows Template Studio is divided in the following main elements:

* **Windows Template Studio Extension**: This is a Visual Studio Extension project, which allows to install a new Visual Studio Project Template allowing the End-Users to have an improved experience when they want to create a new UWP App from the "File -> New Project...".
* **Generation Wizard**: Once the End-User select the "Windows Template Studio" project type in the Visual Studio "File -> New Project..." dialog, the Generation Wizard will guide him through some steps to create user's preferred project. The Generation Wizard allows the user to select among the available Project Types, Design Patterns, Pages and Features 
* **Templates**: This is the repository of code templates used from the Generation Wizard. The templates are pieces of code used to generate the final project templates.

### What is a Template

A template is just code with some metadata. The metadata will contain the template information: name, description, licensing, remarks, programming language, type, guids, etc. The template definition is based on [dotnet Template Engine](https://github.com/dotnet/templating).

There are four different types of templates:

* **Framework Templates**: Will contain the code used as infrastructure for the projects.
* **Project Template**: project templates define the type of App you are about to generate (Basic, Split View, Tabbed...).
* **Page Template**: will contain the files and code needed to add a page to a certain App.
* **Feature Templates**: will contain the files and code needed to add features to the target generated template.

### What is the Generation Wizard

The Generation Wizard guide the user through the available templates allowing the user to compose an App project of his preference.

The End-User can select among the different Design Patterns and Project Types to define the basic layout and base design pattern for his App. Then can add different Pages and Features to complete his app template. Once the user finish with the templates selections, the generation process is executed to create final App project template code.

The generation is made in a composite way, where Pages and Customer Features are Design Pattern and Project Type agnostics and is only at the generation time where the particularities are included.

As well as templates definition, the generation is based on [dotnet Template Engine](https://github.com/dotnet/templating) project.

### What is the Template Repository

The Templates Repository is the place where we will gather all templates and will make them available for the community (hosted on a CDN). We will have two repositories publicly available:

* **Master:** The stable and public version of the templates.
* **Dev:** The ongoing version of the templates.

## Table of Contents

* [Installing / Using the extension](getting-started-extension.md)
* [Using and extending your file->new](getting-started-endusers.md)
* [**Concepts of Windows Template Studio**](readme.md)
* [Getting started with the generator codebase](getting-started-developers.md)
* [Authoring Templates](templates.md)