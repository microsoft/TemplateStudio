# Drag & Drop Service feature

:heavy_exclamation_mark: There is also a version of [this document with code samples in VB.Net](./drag-and-drop.vb.md) :heavy_exclamation_mark: |
----------------------------------------------------------------------------------------------------------------------------------------------- |

In Windows Template Studio, the Drag & Drop feature is a service that basically is a wrapper over the [standard UWP drag and drop functionallity](https://docs.microsoft.com/en-us/windows/uwp/design/input/drag-and-drop). This service simplify the required code to create drag and drop ready apps as well as follows the framework pattern used by your app.

While using Code-Behind framework, the events will be handled by using actions. When using frameworks based on MVVM pattern (MVVM Light, Calliburn.Micro or MVVM Basic) the events will be handled by using commands.

Following you find the actions, for code-behind framework, or commands, for MVVM based frameworks,
which returns the different object types supported by drag and drop within UWP:

|Command (MVVM pattern)     |Action (code-behind)       |Return Type                 |
|---------------------------|---------------------------|----------------------------|
|DropDataViewCommand		|DropDataViewAction			|DataPackageView             |
|DropApplicationLinkCommand	|DropApplicationLinkAction	|Uri                         |
|DropBitmapCommand			|DropBitmapAction			|RandomAccessStreamReference |
|DropHtmlCommand			|DropHtmlAction				|string                      |
|DropRtfCommand				|DropRtfAction				|string                      |
|DropStorageItemsCommand	|DropStorageItemsAction		|IReadOnlyList<IStorageItem> |
|DropTextCommand			|DropTextAction				|string                      |
|DropWebLinkCommand			|DropWebLinkAction			|Uri                         |


The service maps the following events too:

|Event     							|Command (MVVM pattern)	    |Action (code-behind)	|Return Type            |
|-----------------------------------|---------------------------|---------------------------|-----------------------|
|UIElement.DragEnter				|DragEnterCommand           |DragEnterAction            |DragDropData           |
|UIElement.DragOver					|DragOverCommand            |DragOverAction             |DragDropData           |
|UIElement.DragLeave				|DragLeaveCommand           |DragLeaveAction            |DragDropData           |
|ListViewBase.DragItemsStarting		|DragItemsStartingCommand   |DragItemsStartingAction    |DragDropStartingData   |
|ListViewBase.DragItemsCompleted    |DragItemsCompletedCommand  |DragItemsCompletedAction	|DragDropCompletedData  |

The feature provides three configuration types as well to easily setup the Drag & Drop service:

- *DropConfiguration*: Provides the drag and drop configuration properties for any element derived from UIElement.
- *ListViewDropConfiguration*: Provides the drag and drop configuration properties for any element derived from ListViewBase.
- *VisualDropConfiguration*: Provides the drag and drop configuration properties for visual customization.

## How to use the service

To use the service, you need to follow these steps:

1. Define your valid drag and drop areas: Use the elements AllowDrop and CanDrag properties to designate the elements of your app valid for dragging and dropping. (https://docs.microsoft.com/en-us/windows/uwp/design/input/drag-and-drop). In the ListView element you can use CanDragItems property.

2. Include the service reference within the page by adding the required namespace: `xmlns:dd="using:AppNameSpace.Services.DragAndDrop"`

3. Configure the service appropiately:
- For an UIElement:

``` xml
<Grid>
    <dd:DragDropService.Configuration>
        <dd:DropConfiguration  />
    </dd:DragDropService.Configuration>
</Grid>
```

- For a ListView:

``` xml
<ListView>
    <dd:DragDropService.Configuration>
        <dd:ListViewDropConfiguration  />
    </dd:DragDropService.Configuration>
</ListView>
```

- For visual customization:

``` xml
<Grid>
    <dd:DragDropService.VisualConfiguration>
        <dd:VisualDropConfiguration  />
    </dd:DragDropService.VisualConfiguration>
</Grid>
```

In the following section you can find how to setup a basic configuration.

## Drag and drop basic configuration

In this section you can find some code snippets representing how to setup a basic Drag and Drop service configuration for apps based on MVVM Basic or Code-Behind frameworks. For other MVVM pattern based frameworks, the definition is like MVVM Basic.

### Scenario 1 : Configure a grid to allow drop one or more files

#### MVVM Basic

- XAML Page

``` xml
<Grid Background="{ThemeResource SystemControlPageBackgroundChromeLowBrush}"
    AllowDrop="True">
    <dd:DragDropService.Configuration>
        <dd:DropConfiguration
            DropStorageItemsCommand="{x:Bind ViewModel.GetStorageItemsCommand}" />
    </dd:DragDropService.Configuration>
</Grid>
```

- ViewModel code

``` c#
using System.Collections.Generic;
using Windows.Storage;
using System.Windows.Input;

...

private ICommand _getStorageItemsCommand;
public ICommand GetStorageItemsCommand => _getStorageItemsCommand ?? (_getStorageItemsCommand = new RelayCommand<IReadOnlyList<IStorageItem>>(OnGetStorageItem));

public void OnGetStorageItem(IReadOnlyList<IStorageItem> items)
{
    foreach(var item in items)
    {
        //TODO WTS: Process storage item
    }
}
```

#### Code-Behind

- XAML Page

``` xml
<Grid Background="{ThemeResource SystemControlPageBackgroundChromeLowBrush}"
    AllowDrop="True">
    <dd:DragDropService.Configuration>
        <dd:DropConfiguration
            DropStorageItemsAction="{x:Bind GetStorageItem}"   />
    </dd:DragDropService.Configuration>
</Grid>
```

- ViewModel code

``` C#
using System.Collections.Generic;
using Windows.Storage;

...

public Action<IReadOnlyList<IStorageItem>> GetStorageItem => ((items) => OnGetStorageItem(items));
public void OnGetStorageItem(IReadOnlyList<IStorageItem> items)
{
    foreach(var item in items)
    {
        //TODO WTS: Process storage item
    }
}
```

### Escenario 2: Drag and drop enabled ListView

We will configure a ListView to allow drag items from it and drop items to it. We will also include some customizations.

We will need a class to be used class model during the drag and drop operations, for example the `CustomItem` class:

``` c#
public class CustomItem
{
    public Guid Id { get; } = Guid.NewGuid();
    public string FileName { get; set; }
    public IStorageItem OriginalStorageItem { get; set; }
}
```

#### MVVM Basic

- XAML Page

``` xml
<ListView
    AllowDrop="True"
    CanDragItems="True"
    ItemsSource="{x:Bind ViewModel.Items}">

    <dd:DragDropService.Configuration>
        <dd:ListViewDropConfiguration
            DropStorageItemsCommand="{x:Bind ViewModel.GetStorageItemsCommand}"
            DragItemsStartingCommand="{x:Bind ViewModel.DragItemStartingCommand}"
            DragItemsCompletedCommand="{x:Bind ViewModel.DragItemCompletedCommand}" />
    </dd:DragDropService.Configuration>

    <dd:DragDropService.VisualConfiguration>
        <dd:VisualDropConfiguration
            Caption="Custom text here"
            IsContentVisible="False"
            IsGlyphVisible="False"/>
    </dd:DragDropService.VisualConfiguration>
</ListView>
```

- ViewModel code

``` c#
private ICommand _getStorageItemsCommand;
private ICommand _dragItemStartingCommand;
private ICommand _dragItemCompletedCommand;

public ICommand GetStorageItemsCommand => _getStorageItemsCommand ?? (_getStorageItemsCommand = new RelayCommand<IReadOnlyList<IStorageItem>>(OnGetStorageItem));
public ICommand DragItemStartingCommand => _dragItemStartingCommand ?? (_dragItemStartingCommand = new RelayCommand<DragDropStartingData>(OnDragItemStarting));
public ICommand DragItemCompletedCommand => _dragItemCompletedCommand ?? (_dragItemCompletedCommand = new RelayCommand<DragDropCompletedData>(OnDragItemCompleted));

public ObservableCollection<CustomItem> Items { get; set; } = new ObservableCollection<CustomItem>();

private void OnGetStorageItem(IReadOnlyList<IStorageItem> items)
{
    foreach (StorageFile item in items)
    {
        Items.Add(new CustomItem
        {
            FileName = item.Name,
            OriginalStorageItem = item
        });
    }
}

private void OnDragItemStarting(DragDropStartingData startingData)
{
    var items = startingData.Items.Cast<CustomItem>().Select(i => i.OriginalStorageItem);
    startingData.Data.SetStorageItems(items);
    startingData.Data.RequestedOperation = DataPackageOperation.Copy;
}

private void OnDragItemCompleted(DragDropCompletedData completedData)
{
    if(completedData.DropResult != DataPackageOperation.None)
    {
        var draggedItems = completedData.Items.Cast<CustomItem>();
        foreach(var item in draggedItems)
        {
            Items.Remove(item);
        }
    }
}
```

#### Code-Behind

- XAML Page

``` xml
<ListView x:Name="listview"
            AllowDrop="True"
            CanDragItems="True"
            ItemsSource="{x:Bind Items}">

    <dd:DragDropService.Configuration>
        <dd:ListViewDropConfiguration
            DropStorageItemsAction="{x:Bind GetStorageItems}"
            DragItemsStartingAction="{x:Bind DragItemStarting}"
            DragItemsCompletedAction="{x:Bind DragItemCompleted}" />
    </dd:DragDropService.Configuration>

    <dd:DragDropService.VisualConfiguration>
        <dd:VisualDropConfiguration
            Caption="Custom text here"
            IsContentVisible="False"
            IsGlyphVisible="False"/>
    </dd:DragDropService.VisualConfiguration>
</ListView>
```

- ViewModel code

``` c#
public Action<IReadOnlyList<IStorageItem>> GetStorageItems => (items) => OnGetStorageItems(items);
public Action<DragDropStartingData> DragItemStarting => (startingData) => OnDragItemStarting(startingData);
public Action<DragDropCompletedData> DragItemCompleted => (completedData) => OnDragItemCompleted(completedData);

public ObservableCollection<CustomItem> Items { get; set; } = new ObservableCollection<CustomItem>();

private void OnGetStorageItems(IReadOnlyList<IStorageItem> items)
{
    foreach (StorageFile item in items)
    {
        Items.Add(new CustomItem
        {
            FileName = item.Name,
            OriginalStorageItem = item
        });
    }
}

private void OnDragItemStarting(DragDropStartingData startingData)
{
    var items = startingData.Items.Cast<CustomItem>().Select(i => i.OriginalStorageItem);
    startingData.Data.SetStorageItems(items);
    startingData.Data.RequestedOperation = DataPackageOperation.Copy;
}

private void OnDragItemCompleted(DragDropCompletedData completedData)
{
    if (completedData.DropResult != DataPackageOperation.None)
    {
        var draggedItems = completedData.Items.Cast<CustomItem>();
        foreach (var item in draggedItems)
        {
            Items.Remove(item);
        }
    }
}
```
