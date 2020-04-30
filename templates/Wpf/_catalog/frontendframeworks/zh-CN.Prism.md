Prism 是一种用于在 WPF、Windows 10 UWP 和 Xamarin Forms 中构建松散耦合、可维护且可测试的 XAML 应用程序的 framework。面向每个平台提供单独的版本，这些版本将按照独立的时间线进行开发。借助 Prism 可以实施一系列有助于编写结构良好的可维护 XAML 应用程序的 design pattern，包括 MVVM、依赖项注入、命令和 EventAggregator 等。Prism 的核心功能是面向上述平台的可移植类库中的共享代码库。需要指定平台的内容会在目标平台各自的库中实施。Prism 也提供这些模式和目标平台之间的良好集成。例如，适用于 UWP 和 Xamarin Forms 的 Prism 支持将抽象类而非平台概念和 API 上的层用作可以进行单元测试的导航，这样一来，你不但可以完全利用平台自身的功能，同时还能够以 MVVM 方式完成。

有关详细信息，请访问 [Prism GitHub Page] (https://github.com/PrismLibrary/Prism)。
