Prism 是一個framework，可在 WPF、Windows 10 UWP 和 Xamarin Forms 中，用來建置相依性低、可維護和可測試的 XAML 應用程式。每個平台適用不同的版本，而且這些版本將會根據獨立的時間表加以開發。Prism 可實作一套design pattern，這在撰寫結構良好且可維護的 XAML 應用程式 (包括 MVVM、相依性插入、命令、EventAggregator 等等) 時非常實用。Prism 的核心功能是鎖定這些平台的可攜式類別庫中的共用程式碼庫。必須是平台專屬的事項會在目標平台的個別程式庫中實作。Prism 也會將這些模式與目標平台完美整合。例如，UWP 和 Xamarin Forms 適用的 Prism 可讓您對能夠進行單元測試的導覽使用抽象概念，而不是在平台概念和 API 之上用於導覽的層級，讓您可以充分運用平台本身提供的功能，以 MVVM 的方式完成。

如需詳細資訊，請瀏覽 [Prism GitHub Page](https://github.com/PrismLibrary/Prism)。
