# Upgrade MVVM Toolkit to version 8.x

In versions 5.3 and below of the Template Studio extension created projects that used version 7.x of the `CommunityToolkit.MVVM` package.

Since then, the extension now uses version 8 of the package and makes use of SourceGenerator capability that it now includes.

After updating to the new version of the package (version 8.1.0 at the time of writing), you should be able to continue using the previously generated code without issue.

However, if you wish to switch to using the Source Generator capabilities, you can see examples of the changes to make in [their  announcement post](https://devblogs.microsoft.com/dotnet/announcing-the-dotnet-community-toolkit-800/#mvvm-toolkit-source-generators-%f0%9f%a4%96)

There is also more information about using MVVM Source Generators in the [official documentation](https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/generators/overview).
