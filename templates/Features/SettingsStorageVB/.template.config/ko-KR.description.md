설정 저장소는 애플리케이션의 데이터 저장을 단순화해주는 클래스입니다.  여기에서는 데이터를 로딩하여 저장하고 직렬화하며 애플리케이션의 데이터에 쉽게 액세스합니다.

다음은 앱 데이터의 주요 유형입니다:

* Local: 장치에 저장된 데이터로 클라우드에 백업되어 업데이트 후에도 유지됩니다.
* LocalCache: 백업되지 않으며 기존의 장치에 저장된 데이터로 업데이트 후에도 유지됩니다.
* SharedLocal: 모든 앱 사용자에게 유지되는 데이터입니다.
* Roaming: 앱이 설치된 장치에 저장된 데이터입니다.
* Temporary: 언제라도 시스템에서 삭제 가능한 데이터입니다.

저장소에 대한 자세한 내용은 [docs.microsoft.com]에서 알아보십시오. (https://docs.microsoft.com/en-us/uwp/api/windows.storage.applicationdata)
