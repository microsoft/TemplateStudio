UWP Community Template Generator [![Build Status](https://travis-ci.org/Microsoft/UWPCommunityTemplates.png?branch=master)](https://travis-ci.org/Microsoft/UWPCommunityToolkit)
===========
The UWP Community Template Generator goal is to help developers with their File->New experience in Visual Studio.  It will generate a strong, generic foundation with the pages you need, but also integrate game changing features like Cortana, action center and background tasks from the start.  Any critical features will have code comments with links to MSDN, stack overflow and blogs to help unblock developers. Once the template is generated for the developer, it can provide base sample data and will be able to compile then run without issue.

Example scenario:
I need an app that uses MVVM Light, uses Cortana, Speech APIs, Ink on one of the pages and will need Azure mobile services.   It will need a background service that does a query every 5 minutes.

To reach our developers in an up-to-date fashion, the project is broken up into two primary parts, formulas and the generator.  The generator is a Visual Studio extension a developer will install while the formulas will be hosted on a CDN so we can update what is created independntly of the generator.  A formula is what is created by the generator.  It can be XAML, code behind, or something else, it does't matter.  Formulas use the [Mustache syntax](https://mustache.github.io/mustache.5.html) for generation.

## Getting Started
Please read the [getting Started with the UWP Template Community Generator](TBD) ???http://docs.uwpcommunitytemplates.com/en/master/Getting-Started/??? page for more detailed information about using the UWP Community Template Generator.

## Features

### Supported Application Types
 * Basic

### Supported Frameworks
 * Basic
 * [MVVM Light](http://www.mvvmlight.net/)

### Supported Pages
Coming soon

### Developer Options
Coming soon

### End User Options
Coming soon

## Feedback and Requests
Please use [GitHub issues](https://github.com/Microsoft/UWPCommunityTemplates/issues) for questions or comments.  If you have specific feature requests or would like to vote on what others are recommending, please go to the [GitHub issues](https://github.com/Microsoft/UWPCommunityTemplates/issues)section as well.  We would love to see what you are thinking.

## Contributing
Do you want to contribute? Here are our [contribution guidelines](https://github.com/Microsoft/UWPCommunityTemplates/blob/master/contributing.md).

## Principles
 * Principle #1: Generated templates will be kept simple.
 * Principle #2: Generated templates are a starting point, not a completed application.
 * Principle #3: Generated templates once generated, must be able to be compiled and run.
 * Principle #4: Generated templates should work on all device families.
 * Principle #5: Formulas should have comments to aid developers.  This includes links to singup pages for keys, MSDN, blogs and how-to's.  All guidance provide should be validated from either the framework/SDK/library’s creator.
 * Principle #6: All features will be supported for two Windows SDK for Windows 10 release cycles or until another principle supersedes it.
 * Principle #7: Templates released in production will try to adhere to the design language used in the current release of Windows 10.

This project has adopted the code of conduct defined by the [http://contributor-covenant.org/](Contributor Covenant) to clarify expected behavior in our community. 

## Roadmap
Read what we [plan for next iteration](https://github.com/Microsoft/UWPCommunityTemplates/issues?q=is%3Aopen+is%3Aissue+milestone%3Av0.1), and feel free to ask questions.

By adding this ([Gallery](TBD) ???https://dotnet.myget.org/gallery/UWPCommunityTemplates??? to your Visual Studio, you can also get pre-release packages of upcoming version.

## License
This code is distributed under the terms and conditions of the [MIT license](LICENSE). 
