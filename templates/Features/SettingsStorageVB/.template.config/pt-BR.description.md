Setting storage is a class to simplify storing your application data.  It handles loading, saving, serializing your data and easy access to your application's data.

These are the main types of app data:

* Local: stored on the device, backed up in the cloud, and persists across updates
* LocalCache: persistent data that exists on the current device, not backed up, and persists across updates
* SharedLocal: persistent across all app users
* Roaming: exists on all devices where the user has installed the app
* Temporary: can be deleted by the system at any time

To find out more about storage, head to [docs.microsoft.com](https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata).