# Installing / Using the extension

## Installing the Visual Studio Extension
### Prerequisites
Windows Template Studio installs into your existing Windows 10 app development environment. If you're missing any of the below, you can get everything from the [Windows Dev Center Download page](https://developer.microsoft.com/en-us/windows/downloads).
* Visual Studio 2017 Update 1 or higher (any edition)
* Windows 10 Creators Update SDK (version 10.0.15063.0 or later)

### Visual Studio Extension Feed URLs for Windows Template Studio

* **Public Release:** The official extension from the [Visual Studio Gallery](https://marketplace.visualstudio.com/items?itemName=WASTeamAccount.WindowsTemplateStudio)
* **Nightly:** <https://www.myget.org/F/windows-template-studio-nightly/vsix/>
* **Pre-release (stable):** <https://www.myget.org/F/windows-template-studio-prerelease/vsix/>

## Public Release instructions

1.	Install the Visual Studio Extension for Windows Template Studio by downloading the VSIX from the [Visual Studio Gallery](https://marketplace.visualstudio.com/items?itemName=WASTeamAccount.WindowsTemplateStudio). 
1.	Once installed, open Visual Studio 2017 and select File→ New Project→ C# → Windows Universal and you should see Windows Template Studio as an option. 


## Nightly / Pre-release instructions

The Pre-release build version allows you to get updates with stable features not officially released yet (so, they are not definitive and may change).  This build starts at 3AM GMT every night.

This feed will have stable extension versions so it is not thought to have breaking changes (and can be installed side by side with the official one), anyway, installing this extension is at your own risk.

Open Visual Studio 2017 and go to **Tools -> Extensions & Updates**, then click on **Change your Extensions and Updates settings** *(bottom left of the 'Extensions and Updates' window)* and create an Additional Extension Gallery.

![Configure Additional Extension Gallery](resources/vsix/configurefeed.PNG)

Then, go again to **Tools -> Extensions & Updates** and using the recently added online gallery *(added under the 'Online' node)*, download and install the Windows Template Studio extension. Note that you will likely need to restart Visual Studio for the install to complete.

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
