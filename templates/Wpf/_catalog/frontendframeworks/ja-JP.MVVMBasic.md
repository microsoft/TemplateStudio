これは MVVM patternの汎用バージョンです。[Model-View-ViewModel パターン](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) は、すべての XAML プラットフォームで使用できます。このパターンの目的は、ユーザー インターフェイス コントロールが関与する部分とそのロジックが関与する部分の区別を明確にすることです。

MVVM patternの中心的な構成要素は、モデル、ビュー、およびビュー モデルの 3 つです。それぞれが固有の異なる役割を果たします。

MVVM Basic は framework ではありませんが、Model-View-ViewModel (MVVM) パターンを使用してアプリを作成するための最小限の機能を提供します。
サード パーティの MVVM Framework を使用できない場合や、使用したくない場合に、これを使用してください。

MVVM Basic は、すべての機能を備えた MVVM Framework を意図したものではありません。したがって、他の framework にある機能の一部は含まれません。こうした機能の主なものとして、ViewModel ファーストのナビゲーション、IOC、メッセージングがあります。こうした機能が必要である場合は、こうした機能をサポートする framework を選択してください。

MVVM Basic で作成したプロジェクトには、Observable と RelayCommand という 2 つの重要なクラスが含まれます。
**Observable** は、INotifyPropertyChanged インターフェイスの実装を含み、すべてのビュー モデルの基本クラスとして使用されます。これにより、バインドされたプロパティのビューでの更新が簡単になります。
**RelayCommand** は、UI イベントを直接処理するのではなく、ViewModel でのビュー呼び出しコマンドを保持できるように、ICommand インターフェイスの実装を含みます。
