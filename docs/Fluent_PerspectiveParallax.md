# Perspective Parllax

## Overview and Purpose

This page describes how to enable Fluent Design parallax features in a
Windows Template Studio enabled application.

## Prerequisites

Perspective Parallax as a control is available as of the Fall Creators Update,
which enables all of the new Fluent features. This method also utilizes
conditional XAML, a Creators update feature, which allows the selective
enabling or disabling of features at runtime which aren't available on the
target computer.

This tutorial requires and assumes the following of your project and development environment
- An updated Visual Studio 2017 development environment
- The most current (Fall Creators Update) Windows 10 SDK
- Project Max Version set to Fall Creators Update or above (Release XXXXX)
- Project Min Version set to Creators Update (Release 15063)

## Conditional XAML

### What and Why
Conditional XAML is a set of directives for the XAML parser that allow you to selectively instantiate objects in markup at runtime based on if the relevant API exists on the target machine. As long as the project is compiled using a recent enough SDK that the "newest" controls exist in that API, then you as a developer can enable backwards compatability as far back at the Creators Update.

### How to Enable

The equivalent of the if statement is the conditional namespace.
``` XML
xmlns:myNamespace="schema?conditionalMethod(parameter)
```
The delimiter `?` separates the namespace on the left and the conditional on the right. At runtime, the namespace is evaluated as true or false based on the condition. By prefixing XAML objects with this namespace, if at runtime, it evaluates to true, the object is instantiated and if it evaluates to false, it is ignored.

In our case, the ParallaxView control is new for the Fall Creators Update. This update introduces the 5th version of the Universal API Contract. The condition to check for such a version is as follows

``` XML
IsApiContractPresent(ContractName, VersionNumber)
```

Therefore, we could create a namespace that checks for the 5th version of the contract in the following way. It would be added to our page in the following way

``` XML
xmlns:fcu="http://schemas.microsoft.com/winfx/2006/xaml/presentation?IsApiContractPresent(Windows.Foundation.UniversalApiContract,5)"

<UserControl
    x:Class="XXXXX"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    ...
    xmlns:fcu="http://schemas.microsoft.com/winfx/2006/xaml/presentation?IsApiContractPresent(Windows.Foundation.UniversalApiContract,5)"

```

We can then conditionally instantiate controls based on the API contract as follows. In this example, the ColorPicker (a control only introduced in the Fall Creators Update) is only created at runtime when the machine it is running on has the right APIs.

``` XML
<fcu:ColorPicker x:Name="colorPicker" Grid.Column="1"/>
```

## Parallax Controls

### What is Parallax

Parallax is a visual effect where items closer to the viewer move faster than items in the background. Parallax creates a feeling of depth, perspective, and movement. In a UWP app, you can use the ParallaxView control to create a parallax effect. Parallax is a Fluent Design System component that adds motion, depth, and scale to your app.

ParallaxView becomes generally available starting in the Fall Creator's Update

### Appropriate Use

Microsoft gives the following general recommendations for where Parallax is appropriate for use.
+ Use parallax in lists with a background image
+ Consider using parallax in ListViewItems when ListViewItems contain an image
+ Don’t use it everywhere, overuse can diminish its impact

### How to Enable

To create a parallax effect, you use the ParallaxView control. This control ties the scroll position of a foreground element, such as a list, to a background element, such as an image. As you scroll through the foreground element, it animates the background element to create a parallax effect.
To use the ParallaxView control, you provide a Source element and a background element.

As in the above section, we are going to selectively create a ParallaxView control only when the APIs exist.

``` XML
<Grid>
  <fcu:ParallaxView Source="{x:Bind ForegroundElement}">
    <fcu:Image x:Name="BackgroundImage" Source="/Assets/XXXX.jpg"/>
  </fcu:ParallaxView>

  <ScrollViewer Name="ForegroundElement".../>
  ...
</Grid>

```
Add the above snippet of code into the Content grid at the root level. Ensure the content you want to be captured inside the parallax (ie the scrollviewer) is named ForegroundElement and is also at the root of the grid.

## Further Reading

1. [Parallax and the Fluent Design System](https://docs.microsoft.com/en-us/windows/uwp/style/parallax)
2. [Conditional XAML](https://docs.microsoft.com/en-us/windows/uwp/debug-test-perf/conditional-xaml)
3. [Further reading on ParallaxView](https://docs.microsoft.com/en-us/uwp/api/Windows.UI.Xaml.Controls.Parallaxview)
