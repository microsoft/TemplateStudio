**注意：MVVM 基本版已被 MVVM 工具包取代，並將作為選項在未來版本的 Windows 範本工作室中刪除**

這是 MVVM 模式的一般版本。[Model-View-ViewModel 模式](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) 可在所有 XAML 平台上使用。其目的是為了清楚分割使用者介面控制項與其邏輯的考量。

MVVM 模式中有三個核心元件：模型、檢視及檢視模型。每一個元件各有其獨特的作用。

MVVM Basic 不是framework，但是會提供使用 Model-View-ViewModel (MVVM) 模式建立應用程式所需的最少功能。
如果您無法或不想使用第三方 MVVM Framework，請使用 MVVM Basic。

MVVM Basic 不是具備完整功能的 MVVM Framework，也不包含其他framework所擁有的部分功能。ViewModel 優先導覽、IOC 和傳訊是其中最顯而易見的功能。如果您想要這些功能，請選擇支援這些功能的framework。

使用 MVVM Basic 建立的專案包含兩個重要類別：`Observable` 和 `RelayCommand`。
**Observable** 包含 `INotifyPropertyChanged` 介面的實作，而且會當做所有檢視模型的基底類別使用。如此可讓您輕鬆更新檢視上的繫結屬性。
**RelayCommand** 包含 `ICommand` 介面的實作，好讓您輕鬆地在 ViewModel 上擁有檢視呼叫命令，而不用直接處理 UI 事件。
