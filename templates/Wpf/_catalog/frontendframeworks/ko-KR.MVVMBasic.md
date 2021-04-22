**통지: MVVM 베이직은 MVVM 툴킷에 의해 대체되었으며 향후 버전의 Windows 템플릿 스튜디오에서 옵션으로 제거됩니다.**

MVVM pattern의 일반 버전입니다.  [Model-View-ViewModel 패턴](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel)은 XAML 플랫폼에서 사용합니다. 그 목적은 사용자 인터페이스 컨트롤과 논리 간의 고려 사항을 명확하게 분리하는 것입니다.

MVVM pattern의 3가지 핵심 구성 요소는 모델, 뷰, 뷰 모델입니다. 각기 서로 다른 역할을 수행합니다.

MVVM BasicMVVM Basic은 framework프레임워크는 아니지만 MVVM(Model-View-ViewModel) 패턴을 이용해 앱을 만드는 최소 기능을 제공합니다.
타사의 MVVM Framework프레임워크를 사용하고 싶지 않거나 사용할 수 없을 때 사용하십시오.

MVVM BasicMVVM Basic은 MVVM Framework프레임워크의 모든 기능이 제공되지 않으므로 다른 framework프레임워크에 있는 일부 기능이 없습니다. 이 중 ViewModel-first 탐색, IOC, 메시지 기능은 확실히 제공되지 않습니다. 필요하다면 이 기능을 지원하는 framework프레임워크를 선택하십시오.

MVVM BasicMVVM Basic로 만든 프로젝트는 `Observable` 및 `RelayCommand`라는 두 가지 중요 클래스가 있습니다.
**Observable**은 `INotifyPropertyChanged` 인터페이스를 구현하고 모든 뷰 모델에서 기본 클래스로 사용됩니다. 그러면 뷰의 바인딩 속성을 쉽게 업데이트할 수 있습니다.
**RelayCommand**는 UI 이벤트를 직접 처리하는 대신 ViewModel에서 뷰 호출 명령을 쉽게 만들도록 `ICommand` 인터페이스를 구현합니다.
