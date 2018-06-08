# Drag & Drop Service feature

:heavy_exclamation_mark: There is also a version of [this document with code samples in C#](./drag-and-drop.md) :heavy_exclamation_mark: |
---------------------------------------------------------------------------------------------------------------------------------------- |

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

```vbnet
Imports System.Collections.Generic
Imports Windows.Storage
Imports System.Windows.Input

...

Private _getStorageItemsCommand As ICommand

Public Property GetStorageItemsCommand As ICommand
    Get
        Return If(_getStorageItemsCommand, (__InlineAssignHelper(_getStorageItemsCommand, New RelayCommand(Of IReadOnlyList(Of IStorageItem))(OnGetStorageItem))))
    End Get
End Property

Public Sub OnGetStorageItem(ByVal items As IReadOnlyList(Of IStorageItem))
    For Each item In items
        ' TODO WTS: Process storage item
    Next
End Sub
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

```vbnet
Imports System.Collections.Generic
Imports Windows.Storage

...

Public Property GetStorageItem As Action(Of IReadOnlyList(Of IStorageItem))
    Get
        Return(Function(items) OnGetStorageItem(items))
    End Get
End Property

Public Sub OnGetStorageItem(ByVal items As IReadOnlyList(Of IStorageItem))
    For Each item In items
        ' TODO WTS: Process storage item
    Next
End Sub
```

### Escenario 2: Drag and drop enabled ListView

We will configure a ListView to allow drag items from it and drop items to it. We will also include some customizations.

We will need a class to be used class model during the drag and drop operations, for example the `CustomItem` class:

```vbnet
Public Class CustomItem

    Public ReadOnly Property Id As Guid = Guid.NewGuid()

    Public Property FileName As String

    Public Property OriginalStorageItem As IStorageItem
End Class
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

```vbnet
Private _getStorageItemsCommand As ICommand

Private _dragItemStartingCommand As ICommand

Private _dragItemCompletedCommand As ICommand

Public Property GetStorageItemsCommand As ICommand
    Get
        Return If(_getStorageItemsCommand, (__InlineAssignHelper(_getStorageItemsCommand, New RelayCommand(Of IReadOnlyList(Of IStorageItem))(OnGetStorageItem))))
    End Get
End Property

Public Property DragItemStartingCommand As ICommand
    Get
        Return If(_dragItemStartingCommand, (__InlineAssignHelper(_dragItemStartingCommand, New RelayCommand(Of DragDropStartingData)(OnDragItemStarting))))
    End Get
End Property

Public Property DragItemCompletedCommand As ICommand
    Get
        Return If(_dragItemCompletedCommand, (__InlineAssignHelper(_dragItemCompletedCommand, New RelayCommand(Of DragDropCompletedData)(OnDragItemCompleted))))
    End Get
End Property

Public Property Items As ObservableCollection(Of CustomItem) = New ObservableCollection(Of CustomItem)()

Private Sub OnGetStorageItem(ByVal items As IReadOnlyList(Of IStorageItem))
    For Each item As StorageFile In items
        Items.Add(New CustomItem With {.FileName = item.Name, .OriginalStorageItem = item})
    Next
End Sub

Private Sub OnDragItemStarting(ByVal startingData As DragDropStartingData)
    Dim items = startingData.Items.Cast(Of CustomItem)().[Select](Function(i) i.OriginalStorageItem)
    startingData.Data.SetStorageItems(items)
    startingData.Data.RequestedOperation = DataPackageOperation.Copy
End Sub

Private Sub OnDragItemCompleted(ByVal completedData As DragDropCompletedData)
    If completedData.DropResult <> DataPackageOperation.None Then
        Dim draggedItems = completedData.Items.Cast(Of CustomItem)()
        For Each item In draggedItems
            Items.Remove(item)
        Next
    End If
End Sub
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

```vbnet
Public Property GetStorageItems As Action(Of IReadOnlyList(Of IStorageItem))
    Get
        Return Function(items) OnGetStorageItems(items)
    End Get
End Property

Public Property DragItemStarting As Action(Of DragDropStartingData)
    Get
        Return Function(startingData) OnDragItemStarting(startingData)
    End Get
End Property

Public Property DragItemCompleted As Action(Of DragDropCompletedData)
    Get
        Return Function(completedData) OnDragItemCompleted(completedData)
    End Get
End Property

Public Property Items As ObservableCollection(Of CustomItem) = New ObservableCollection(Of CustomItem)()

Private Sub OnGetStorageItems(ByVal items As IReadOnlyList(Of IStorageItem))
    For Each item As StorageFile In items
        Items.Add(New CustomItem With {.FileName = item.Name, .OriginalStorageItem = item})
    Next
End Sub

Private Sub OnDragItemStarting(ByVal startingData As DragDropStartingData)
    Dim items = startingData.Items.Cast(Of CustomItem)().[Select](Function(i) i.OriginalStorageItem)
    startingData.Data.SetStorageItems(items)
    startingData.Data.RequestedOperation = DataPackageOperation.Copy
End Sub

Private Sub OnDragItemCompleted(ByVal completedData As DragDropCompletedData)
    If completedData.DropResult <> DataPackageOperation.None Then
        Dim draggedItems = completedData.Items.Cast(Of CustomItem)()
        For Each item In draggedItems
            Items.Remove(item)
        Next
    End If
End Sub
```
