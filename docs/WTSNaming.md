# Windows Template Studio Naming

## Page suffixes
For **Page** generation the suffixes 'Page' and 'ViewModel' will be added by WTS to Page and ViewModel files.

**Example**

A page named **Main** in the wizard will result in the generation of the following files:
 - MainPage.xaml
 - MainPage.xaml.cs
 - MainViewModel.cs (Not generated in CodeBehind design pattern).

**When choosing the name for a Page in the wizard you should not use suffixes like 'Page' or 'View'**

## Reserved names
WTS has some reserved names. You can't use those names for naming pages and features:
 - Page
 - BackgroundTask
 - Pivot
 - Shell
 - SharedDataWebLink
 - SharedDataStorageItems
 - Settings
 - 3DLauncher
 - DragAndDropFeature
 - FeedbackHubFeature
 - FirstRunPrompt
 - HubNotifications
 - LiveTile
 - MultiView
 - SampleDataService
 - SettingsStorage
 - ShareSource
 - ShareTarget
 - StoreNotifications
 - SuspendAndResume
 - ThemeSelection
 - UriScheme
 - VSAppCenter
 - WebToAppLink
 - WhatsNewPrompt