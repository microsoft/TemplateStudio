这是 MVVM 模式的通用版本。[Model-View-ViewModel 模式](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel)可在所有的 XAML 平台上使用。其目的是清晰划分用户界面控件及其逻辑之间的关注点。

MVVM 模式中有三个核心组成部分:模型、视图和视图模型。每个部分都有各自不同的角色。

MVVM Basic 并非 framework，但提供最基本的功能，用于通过 Model-View-ViewModel (MVVM)模式创建应用。
当你无法或不希望使用第三方 MVVM Framework 时，可以使用它。

MVVM Basic 并非功能完备的 MVVM Framework，也不包括其他 framework 所具有的某些功能。ViewModel 优先导航、IOC 和消息传送是最显著的功能。如果你需要这些功能，请选择支持它们的 framework。

使用 MVVM Basic 创建的项目包含两个重要的类: "Observable" 和 "RelayCommand"。
**Observable** 包含“INotifyPropertyChanged”接口的实现，用作所有视图模型的基类。这样，便可在“视图”中轻松地更新绑定属性。
**RelayCommand** 包含“ICommand”接口的实现，可轻松地对 ViewModel 使用 View 调用命令，而不直接处理 UI 事件。
