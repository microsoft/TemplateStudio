# ActivationService & ActivationHandlers

:heavy_exclamation_mark: There is also a version of [this document with code samples in VB.Net](./activation.vb.md) :heavy_exclamation_mark: |
-------------------------------------------------------------------------------------------------------------------------------------------- |

## ActivationService

The ActivationService is in charge of handling the applications initialization and activation.

With the method `ActivateAsync()` it has one common entry point that is called from the app lifecycle events `OnLaunched`, `OnActivated` and `OnBackgroundActivated`.
For more information on application lifecycle and its events see [Windows 10 universal Windows platform (UWP) app lifecycle](https://docs.microsoft.com/en-us/windows/uwp/launch-resume/app-lifecycle).

## ActivationHandlers

For choosing the concrete type of activation the ActivationService relies on ActivationHandlers, that are registered in the method `GetActivationHandlers()`.

Each class in the application that can handle application activation should derive from the abstract class `ActivationHandler<T>` (T is the type of ActivationEventArguments the class can handle) and implement the method HandleInternalAsync().
The method `HandleInternalAsync()` is where the actual activation takes place.
The virtual method `CanHandleInternal()` checks if the incoming activation arguments are of the type the ActivationHandler can manage. It can be overwritten to establish further conditions based on the ActivationEventArguments.

### ActivationHandlers sample

We'll have look at the SchemeActivationHandler, added by DeepLink feature, to see how activation works in detail:

```csharp
protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
{
    // If your app has multiple handlers of ProtocolActivationEventArgs
    // use this method to determine which to use. (possibly checking args.Uri.Scheme)
    return true;
}

// By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
{
    // Create data from activation Uri in ProtocolActivatedEventArgs
    var data = new SchemeActivationData(args.Uri);
    if (data.IsValid)
    {
        NavigationService.Navigate(data.PageType, data.Parameters);
    }

    await Task.CompletedTask;
}
```

The `CanHandleInternal()` method was overwritten here and it returns true by default, devs could use args to add extra validations in scenarios with multiple ProtocolActivationEventArgs.

The `HandleInternalAsync()` method gets the ActivationData from argument's Uri and uses the PageType and Parameters to navigate.

## Activation in depth

### Activation flow

The following flowchart shows the Activation proccess that starts with an app lifecycle event and ends with StartupAsync method call.

**ActivateAsync**

Activation starts from an app lifecycle event: `OnLaunched`, `OnActivated` or `OnBackgroundActivated`.

![](resources/activation/AppLifecycleEvent.png)

The flowchart shows that the first in ActivateAsync is to call InitializeAsync and do the ShellCreation, both actions are excluded from background running (IsInteractive check). If you have added an Identity feature to your app, this block also will include code for Identity configuration and SilentLogin.

After this first block, the flowchart calls to HandleActivation (explained below).

![](resources/activation/ActivateAsync.png)

**IsInteractive**

All interactions with the app window and navigations are only available when the activation arguments extend from IActivatedEventArgs, this allows ActivationService runs activation code on running background task without activating the window.

**InitializeAsync**

InitializeAsync contains static or singleton services initialization for services that are going to be used as ActivationHandler. This method is called before the window is activated. The code in this method runs while the splash screen is shown, initializations from classes different from ActivationHandlers should be placed at StartupAsync method.

**StartupAsync**

StartupAsync contains initializations of some classes and start's processes that will be run after the Window is activated.

**HandleActivation**

HandleActivation method gets the first ActivationHandler that can handle the arguments of the current activation. Before that creates a DefaultActivationHandler and execute if it possible (when no one ActivationHandler was selected or the selected ActivationHandler does not realize a Navigation).

![](resources/activation/HandleActivation.png)

## Sample: Add activation from File Association

We are going now trying to create a new ActivationHandler to understand how to extend the ActivationService in our project. In this case, we are going to add a markdown files (.md) reader in our app.

The following code is thougt to be added in a WTS MVVM Basic app.



For viewing the markdown a MarkdownTextBlock from the [Windows Community Toolkit](https://github.com/Microsoft/WindowsCommunityToolkit) was added.



### Add Page and ViewModel to show the opened file


**MarkdownPage.xaml**

```xml
<Page
    x:Class="YourAppName.Views.MarkdownPage"
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
    xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
    xmlns:controls="using:Microsoft.Toolkit.Uwp.UI.Controls"
    mc:Ignorable="d">
    <Grid
        x:Name="ContentArea">

        <Grid.RowDefinitions>
            <RowDefinition x:Name="TitleRow" Height="48"/>
            <RowDefinition Height="*"/>
        </Grid.RowDefinitions>

        <TextBlock
            x:Name="TitlePage"
            x:Uid="Markdown_Title"
            FontSize="28" FontWeight="SemiLight"
            TextTrimming="CharacterEllipsis"
            TextWrapping="NoWrap" VerticalAlignment="Center"
            Margin="24,0,24,7"/>

        <Grid Grid.Row="1" >
            <ScrollViewer
                Margin="12"
                BorderBrush="{ThemeResource AppBarBorderThemeBrush}"
                BorderThickness="2"
                HorizontalScrollBarVisibility="Disabled"
                VerticalScrollBarVisibility="Visible">
                <controls:MarkdownTextBlock
                    x:Name="UiMarkdownText"
                    Padding="24,12,24,12"
                    Foreground="Black"
                    Background="White"
                    LinkClicked="UiMarkdownText_LinkClicked"
                    Text="{x:Bind ViewModel.Text, Mode=TwoWay}" />
            </ScrollViewer>
        </Grid>
    </Grid>
</Page>
```

**MarkdownPage.xaml.cs**

```csharp
using System;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

using YourAppName.ViewModels;

namespace YourAppName.Views
{
    public sealed partial class MarkdownPage : Page
    {
        public MarkdownViewModel ViewModel { get; } = new MarkdownViewModel();

        public MarkdownPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Parameter?.ToString()))
            {
                var text = await FileIO.ReadTextAsync(e.Parameter as StorageFile);
                ViewModel.Text = text;
            }
        }

        private async void UiMarkdownText_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
```

**MarkdownViewModel.cs**

```csharp
using System;
using YourAppName.Helpers;

namespace YourAppName.ViewModels
{
    public class MarkdownViewModel : Observable
    {
        private string _text;

        public string Text
        {
            get { return _text; }
            set { Set(ref _text, value); }
        }

        public MarkdownViewModel()
        {
        }
    }
}
```

**Resources.resw**

You also should add a string resource for Markdown Title.

```xml
<data name="Markdown_Title.Text" xml:space="preserve">
    <value>Markdown</value>
</data>
```


### Set up File Association Activation

Now we are going to configure the file association activation, first we have to add a file type association declaration in the application manifest, allowing the App to be shown as a default handler for markdown files.

![](resources/activation/DeclarationFileAssociation.PNG)

Further we have to handle the file activation event by implementing the override of OnFileActivated:

**App.xaml.cs**

```csharp
protected override async void OnFileActivated(FileActivatedEventArgs args)
{
    await ActivationService.ActivateAsync(args);
}
```

Then we need a service that handles this new type of activation. We'll call it FileAssociationService, it derives from `ApplicationHandler<T>`.
As it manages activation by File​Activated​Event​Args the signature would be:

**FileAssociationService**

```csharp
internal class FileAssociationService : ActivationHandler<File​Activated​Event​Args>
{

}
```

Next, we'll implement HandleInternalAsync(), to evaluate the event args, and take action:

```csharp
protected override async Task HandleInternalAsync(File​Activated​Event​Args args)
{
    var file = args.Files.FirstOrDefault();

    NavigationService.Navigate(typeof(MarkdownPage), file);

    await Task.CompletedTask;
}
```

### Add the FileAssociationService to ActivationService

Last but not least, we'll have to add our new FileAssociationService to the ActivationHandlers registered in the ActivationService:

**ActivationService**

```csharp
private IEnumerable<ActivationHandler> GetActivationHandlers()
{
    // Add this new FileAssociationService
    yield return Singleton<FileAssociationService>.Instance;
}
```
