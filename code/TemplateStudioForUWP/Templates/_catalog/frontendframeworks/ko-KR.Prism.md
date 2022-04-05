Prism은 느슨하게 결합되고 유지관리와 테스트가 가능한 XAML 애플리케이션 개발을 위한 framework입니다. 각 플랫폼에 해당하는 개별 릴리스가 있어 독립적인 타임라인으로 개발할 수 있습니다. Prism은 design pattern 모음을 제공하고 MVVM, 종속성 삽입, 명령, EventAggregator 등을 사용하므로 잘 구조화되고 유지관리가 가능한 XAML 애플리케이션을 작성하는 데 유용합니다. Prism의 핵심 기능은 각 플랫폼의 Portable Class Library를 기반으로 한 코드 공유입니다. 특정 플랫폼에서 필요한 기능이 대상 플랫폼의 각각의 라이브러리에 구현됩니다. 또한 Prism은 대상 플랫폼에 이러한 패턴을 잘 통합할 수 있습니다. 예를 들면, UWP와 Xamarin Forms의 경우 Prism은 탐색의 추상 모델을 사용하여 단위 테스트가 가능합니다. 테스트는 탐색을 이용해 플랫폼 컨셉과 API의 계층에서 플랫폼 자체 기능을 종합적으로 활용하지만, 테스트 방식은 MVVM으로 진행됩니다.

자세한 내용은 [Prism GitHub 페이지]에서 알아보십시오. (https://github.com/PrismLibrary/Prism)

**참고.** Prism은 버전 7 릴리스에서 UWP를 지원하지 않습니다. Template Studio는 버전 6.3을 계속 사용합니다.
