# Windows Template Studio (WinTS) Naming

## Project reserved names

WinTS has some reserved names. You can't use those names for naming your project:

### Universal Windows Platform

- Prism
- CaliburnMicro
- MVVMLight

### WPF

- Prism
- MVVMLight

## Page suffixes

For **Page** generation the suffixes 'Page' and 'ViewModel' will be added by WinTS to Page and ViewModel files.

**Example**

A page named **Main** in the wizard will result in the generation of the following files:

- MainPage.xaml
- MainPage.xaml.cs
- MainViewModel.cs (Not generated in UWP CodeBehind design pattern).

**When choosing the name for a Page in the wizard you should not use suffixes like 'Page' or 'View'**

## Pages and features reserved names

WinTS has some reserved names. You can't use those names for naming pages and features:

### Universal Windows Platform

- 3DLauncher
- BackgroundTask
- DragAndDropFeature
- FeedbackHubFeature
- FirstRunPrompt
- HubNotifications
- LiveTile
- MultiView
- Page
- Pivot
- SampleDataService
- Settings
- SettingsStorage
- SharedDataStorageItems
- SharedDataWebLink
- ShareSource
- ShareTarget
- Shell
- StoreNotifications
- SuspendAndResume
- ThemeSelection
- UriScheme
- VSAppCenter
- WebToAppLink
- WhatsNewPrompt

### WPF

- MultiView
- Page
- SampleDataService
- Settings
- Shell
- ThemeSelection
- MSIX Packaging
- Persist And Restore