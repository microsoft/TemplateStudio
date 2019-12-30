# Prism

Prism is a framework for building loosely coupled, maintainable, and testable XAML applications. Separate releases are available for each platform and those will be developed on independent timelines. Prism provides an implementation of a collection of design patterns that are helpful in writing well-structured and maintainable XAML applications, including MVVM, dependency injection, commands, EventAggregator, and others. Prism's core functionality is a shared code base in a Portable Class Library targeting these platforms. Those things that need to be platform specific are implemented in the respective libraries for the target platform. Prism also provides great integration of these patterns with the target platform. For example, Prism for UWP and Xamarin Forms allows you to use an abstraction for navigation that is unit testable, but that layers on top of the platform concepts and APIs for navigation so that you can fully leverage what the platform itself has to offer, but done in the MVVM way.

For more information, visit the [Prism GitHub Page](https://github.com/PrismLibrary/Prism).

Prism also provide a number of [productivity tools](https://prismlibrary.github.io/docs/getting-started/productivity-tools.html) that include snippets, item templates, and tools for working in platforms other than UWP.

**Note.** Prism dropped support for UWP with their version 7 release. Windows Template Studio continues to use version 6.3 but the [uncertainty of the future of UWP and Prism](https://github.com/PrismLibrary/Prism/issues/1835) may be a factor in your choice to use it.
