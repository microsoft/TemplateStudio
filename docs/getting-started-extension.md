Getting Started with the Extension
==================================

## Installing the Visual Studio Extension

You can download the official extension from the [Visual Studio Gallery](https://visualstudiogallery.msdn.microsoft.com/) (coming soon).

### Pre-release build version
The Pre-release build version allows you to get updates with stable features not officially released yet (so, they are not definitive and may change).

This feed will have stable extension versions so it is not thought to have breaking changes (and can be installed side by side with the official one), anyway, installing this extension is at your own risk. 

Open Visual Studio 2017 and go to Tools -> Extensions & Updates, then click on "Change your Extensions and Updates settings" and create an Additional Extension Gallery using https://www.myget.org/F/prerelease/vsix/ as Url.

![Configure Additional Extension Gallery](resources/vsix/configurefeed.jpg)

Then, go again to Tools -> Extensions & Updates and using the recently added online gallery, install the Uwp Community Extension.

![Install UWP Community Templates extension](resources/vsix/onlinefeed.jpg)

Once installed, you will see a new Project Template which allows you to access to the available templates: Pre-Release version uses the VNext Template Repository.

![File New Project](resources/vsix/filenew.jpg)

You can start working with Windows Template Studio by cloning [our repo](https://github.com/Microsoft/WindowsTemplateStudio) and working locally with the code and the available templates.

If you plan to contribute, please follow the [contribution guidelines](https://github.com/Microsoft/WindowsTemplateStudio/blob/master/contributing.md) and remeber that the Pull Requests must be done aganist the "[dev](https://github.com/Microsoft/WindowsTemplateStudio/tree/dev)" branch.

## Nighlty Dev-release  
If you want to have updates from in-progress changes, you can confiure the Nightly-Dev feed using this url: https://www.myget.org/F/vsixextensions/vsix/ 

This feed will have the result of the daily dev-branch integration so expect some instability. This extension can be installed side by side with the offical and pre-release, anyway, installing this extension is at your own risk.