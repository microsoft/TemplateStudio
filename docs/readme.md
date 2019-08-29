# Concepts of Windows Template Studio

This section has the main concepts and definitions used in Windows Template Studio.

## Main concepts

Windows Template Studio is divided in the following main elements:

* **Windows Template Studio Extension**: This is a Visual Studio Extension project, which, once installed, allows developers to have an improved experience when creating a new UWP App from the "File -> New Project..." menu.
* **Generation Wizard**: After selecting the "Windows Template Studio" project type in the Visual Studio "File -> New Project..." dialog, the Generation Wizard will guide a person through some steps to create the desired project. The Generation Wizard allows the user to select from the available Project Types, Design Patterns, Pages, Features, Services, and Test Projects.
* **Templates**: This is the repository of code templates used from the Generation Wizard. The templates are pieces of code used to generate the final project.

### What is a Template

A template is just code with some metadata. The metadata contains the template information: name, description, licensing, remarks, programming language, type, guids, etc. The template definition is based on [dotnet Template Engine](https://github.com/dotnet/templating).

There are six different types of templates:

* **Framework Templates** contain the code used as infrastructure for the projects.
* **Project Template** define the type of App you are about to generate (Basic, Split View, Tabbed...).
* **Page Template** contain the files and code needed to add a page to a generated App.
* **Feature Templates** contain the files and code needed to add features to a generated app.
* **Services Templates** will contain the files and code needed to add services to a generated app.
* **Testing Projects Templates** contain the files and code needed to add test projects to the generated solution.

### What is the Generation Wizard

The Generation Wizard guides the user through the available templates allowing them to compose their desired project.

The End-User can select among the different Design Patterns and Project Types to define the basic layout and architecture for the App. They can then add different items (pages, features, services, and tests) to complete the app. Once selection is finished, the generation process is executed to create the App.

The generation is made in a composite way, where Pages and Customer Features are Design Pattern and Project Type agnostics and is only at the generation time where the specifics are included.

The templating functionality is based on the [dotnet Template Engine](https://github.com/dotnet/templating) project.

### What is the Template Repository

The Templates Repository is the place where all templates are made available (hosted on a CDN). There are two repositories publicly available:

* **Master:** The stable and public version of the templates.
* **Dev:** The ongoing version of the templates.

## Table of Contents

* [Installing / Using the extension](getting-started-extension.md)
* [Using and extending your file->new](getting-started-endusers.md)
* [**Concepts of Windows Template Studio**](readme.md)
* [Getting started with the generator codebase](getting-started-developers.md)
* [Authoring Templates](templates.md)
