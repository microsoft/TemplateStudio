# MVVM Basic

MVVM Basic is not a framework but provides the minimum functionality necessary to create an app using the [Model-View-ViewModel (MVVM) pattern](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel). It is unique to projects generated with Windows Template Studio and was created for people who can't or don't want to use a 3rd party MVVM Framework such as MVVM Light or Prism.

MVVM Basic is not intended to be a fully featured MVVM Framework and does not include some features that other frameworks do as for example messaging.

MVVM Basic can also serve as a basis for developers who want to create their own MVVM implementation. By providing only the most basic of extra functionality but still following common conventions it should be the easiest option if you want to modify the generated code to meet your preferred way of working.

MVVM Basic uses the `Microsoft.Extensions.Hosting` NuGet Package as application host to provide configuration and dependency injection. For more information see [.NET Generic Host documentation](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/generic-host).