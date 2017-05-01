# Installing / Using the extension

## Installing the Visual Studio Extension

### Visual Studio Extension Feed URLs for Windows Template Studio

* **Public Release (Coming soon):** The official extension from the [Visual Studio Gallery](https://visualstudiogallery.msdn.microsoft.com/)
* **Pre-release (stable):** <https://www.myget.org/F/windows-template-studio-prerelease/vsix/>
* **Nightly:** <https://www.myget.org/F/windows-template-studio-nightly/vsix/>

## Public Release

** Coming Soon **

## Pre-release build version

The Pre-release build version allows you to get updates with stable features not officially released yet (so, they are not definitive and may change).

This feed will have stable extension versions so it is not thought to have breaking changes (and can be installed side by side with the official one), anyway, installing this extension is at your own risk.

Open Visual Studio 2017 and go to **Tools -> Extensions & Updates**, then click on **Change your Extensions and Updates settings** and create an Additional Extension Gallery.

![Configure Additional Extension Gallery](resources/vsix/configurefeed.PNG)

Then, go again to **Tools -> Extensions & Updates** and using the recently added online gallery, install the Windows Template Studio extension.

![Install UWP Community Templates extension](resources/vsix/onlinefeed.PNG)

Once installed, you will see a new Project Template which allows you to access to the available templates: Pre-Release version uses the VNext Template Repository.

![File New Project](resources/vsix/fileNew.PNG)

You can start working with Windows Template Studio by cloning [our repo](https://github.com/Microsoft/WindowsTemplateStudio) and working locally with the code and the available templates.  If you plan to contribute, please follow the [contribution guidelines](../contributing.md)

## Nightly Dev-release

If you want to have updates from in-progress changes.  This feed will have the result of the daily dev-branch integration so expect some instability. This extension can be installed side by side with the official and pre-release, anyway, installing this extension is at your own risk.

## Table of Contents

* [**Installing / Using the extension**](getting-started-extension.md)
* [Using and extending your file->new](getting-started-endusers.md)
* [Concepts of Windows Template Studio](readme.md)
* [Getting started with the generator codebase](getting-started-developers.md)
* [Authoring Templates](templates.md)
