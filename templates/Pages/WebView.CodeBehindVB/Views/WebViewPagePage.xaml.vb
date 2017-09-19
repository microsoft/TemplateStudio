Imports System.ComponentModel
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls

Namespace Views
    NotInheritable Class WebViewPagePage
        Inherits Page
        Implements INotifyPropertyChanged

        ' TODO WTS: Set your hyperlink default here
        Private Const DefaultUrl As String = "https://developer.microsoft.com/en-us/windows/apps"

        Private _source As Uri

        Public Property Source As Uri
            Get
                Return _source
            End Get
            Set(value As Uri)
                _source = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(Source)))
            End Set
        End Property

        Private _isLoading As Boolean

        Public Property IsLoading As Boolean
            Get
                Return _isLoading
            End Get
            Set(value As Boolean)
                If value Then
                    IsShowingFailedMessage = False
                End If
                _isLoading = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsLoading)))
                IsLoadingVisibility = If(value, Visibility.Visible, Visibility.Collapsed)
            End Set
        End Property

        Private _isLoadingVisibility As Visibility

        Public Property IsLoadingVisibility As Visibility
            Get
                Return _isLoadingVisibility
            End Get
            Set(value As Visibility)
                _isLoadingVisibility = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsLoadingVisibility)))
            End Set
        End Property

        Private _isShowingFailedMessage As Boolean

        Public Property IsShowingFailedMessage() As Boolean
            Get
                Return _isShowingFailedMessage
            End Get
            Set(value As Boolean)
                If value Then
                    IsLoading = False
                End If
                _isShowingFailedMessage = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(IsShowingFailedMessage)))
                FailedMesageVisibility = If(value, Visibility.Visible, Visibility.Collapsed)
            End Set
        End Property

        Private _failedMesageVisibility As Visibility
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Public Property FailedMesageVisibility As Visibility
            Get
                Return _failedMesageVisibility
            End Get
            Set(value As Visibility)
                _failedMesageVisibility = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(FailedMesageVisibility)))
            End Set
        End Property

        Private Sub OnNavigationCompleted(sender As WebView, args As WebViewNavigationCompletedEventArgs)
            IsLoading = False
            OnPropertyChanged(NameOf(IsBackEnabled))
            OnPropertyChanged(NameOf(IsForwardEnabled))
        End Sub

        Private Sub OnNavigationFailed(sender As Object, e As WebViewNavigationFailedEventArgs)
            ' Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = True
        End Sub

        Private Sub OnRetry(sender As Object, e As RoutedEventArgs)
            IsShowingFailedMessage = False
            IsLoading = True

            webView.Refresh()
        End Sub

        Public ReadOnly Property IsBackEnabled As Boolean
            Get
                Return webView.CanGoBack
            End Get
        End Property

        Public ReadOnly Property IsForwardEnabled As Boolean
            Get
                Return webView.CanGoForward
            End Get
        End Property

        Private Sub OnGoBack(sender As Object, e As RoutedEventArgs)
            webView.GoBack()
        End Sub

        Private Sub OnGoForward(sender As Object, e As RoutedEventArgs)
            webView.GoForward()
        End Sub

        Private Sub OnRefresh(sender As Object, e As RoutedEventArgs)
            webView.Refresh()
        End Sub

        Private Async Sub OnOpenInBrowser(sender As Object, e As RoutedEventArgs)
            Await Windows.System.Launcher.LaunchUriAsync(webView.Source)
        End Sub

        Public Sub New()
            Source = New Uri(DefaultUrl)
            InitializeComponent()
            IsLoading = True
        End Sub
    End Class
End Namespace
