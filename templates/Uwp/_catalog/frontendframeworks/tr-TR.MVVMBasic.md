**Bildirim: MVVM Basic, MVVM Araç Seti tarafından değiştirilmiştir ve Windows Template Studio'nun gelecekteki bir sürümünde bir seçenek olarak kaldırılacaktır.**

Bu, MVVM deseninin genel bir sürümü.  [Model-View-ViewModel deseni](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) tüm XAML platformlarında kullanılabilir. Bunun amacı, kullanıcı arabirimi denetimleri ve bunların mantığı arasındaki endişeleri açık bir şekilde ayırmaktır.

MVVM deseninde üç temel bileşen bulunmaktadır: model, görünüm ve görünüm modeli. Her biri farklı bir rol oynar.

MVVM Basic bir framework değildir ancak Model-View-ViewModel (MVVM) deseni kullanarak bir uygulama oluşturmak için minimum işlevsellik sağlar.
Üçüncü taraf MVVM Framework kullanamıyorsanız veya kullanmak istemiyorsanız bunu kullanabilirsiniz.

MVVM Basic tam özellikli bir MVVM Framework olarak tasarlanmamıştır ve diğer framework'lerde bulunan bazı özellikleri içermez. Bu özelliklerin başlıcaları ViewModel öncelikli gezinti, IOC ve mesajlaşmadır. Bu özellikleri istiyorsanız bunları destekleyen bir framework seçin.

MVVM Basic ile oluşturulan projeler "Observable" ve "RelayCommand" olmak üzere iki önemli sınıf içerir.
**Observable**, "INotifyPropertyChanged" arabiriminin bir uygulamasını içerir ve tüm görüntüleme modelleri için temel sınıf olarak kullanılır. Bu da Görünüm üzerindeki bağlı özellikleri güncelleştirmeyi kolaylaştırır.
**RelayCommand** kullanıcı arabirimi olaylarını doğrudan işlemek yerine, ViewModel üzerinde Görünüm çağrı komutlarını bulundurmayı kolaylaştıran "ICommand" arabirimi uygulamasını içerir.
