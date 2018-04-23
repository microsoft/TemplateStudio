Namespace ViewModels
    Public Class WebViewPageViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

        ' TODO WTS: Set the URI of the page to show by default
        Private Const DefaultUrl As String = "https://developer.microsoft.com/en-us/windows/apps"

        Private _source As Uri

        Public Property Source As Uri
            Get
                Return _source
            End Get
            Set
                [Param_Setter](_source, value)
            End Set
        End Property

        Private _isLoading As Boolean

        Public Property IsLoading As Boolean
            Get
                Return _isLoading
            End Get

            Set
                If value Then
                    IsShowingFailedMessage = False
                End If

                [Param_Setter](_isLoading, value)
                IsLoadingVisibility = If(value, Visibility.Visible, Visibility.Collapsed)
            End Set
        End Property

        Private _isLoadingVisibility As Visibility

        Public Property IsLoadingVisibility As Visibility
            Get
                Return _isLoadingVisibility
            End Get
            Set
                [Param_Setter](_isLoadingVisibility, value)
            End Set
        End Property

        Private _isShowingFailedMessage As Boolean

        Public Property IsShowingFailedMessage As Boolean
            Get
                Return _isShowingFailedMessage
            End Get

            Set
                If value Then
                    IsLoading = False
                End If

                [Param_Setter](_isShowingFailedMessage, value)
                FailedMesageVisibility = If(value, Visibility.Visible, Visibility.Collapsed)
            End Set
        End Property

        Private _failedMesageVisibility As Visibility

        Public Property FailedMesageVisibility As Visibility
            Get
                Return _failedMesageVisibility
            End Get
            Set
                [Param_Setter](_failedMesageVisibility, value)
            End Set
        End Property

        Private _navCompleted As ICommand

        Public ReadOnly Property NavCompletedCommand As ICommand
            Get
                If _navCompleted Is Nothing Then
                    _navCompleted = New RelayCommand(Of WebViewNavigationCompletedEventArgs)(AddressOf NavCompleted)
                End If

                Return _navCompleted
            End Get
        End Property

        Private Sub NavCompleted(e As WebViewNavigationCompletedEventArgs)
            IsLoading = False
        End Sub

        Private _navFailed As ICommand

        Public ReadOnly Property NavFailedCommand As ICommand
            Get
                If _navFailed Is Nothing Then
                    _navFailed = New RelayCommand(Of WebViewNavigationFailedEventArgs)(AddressOf NavFailed)
                End If

                Return _navFailed
            End Get
        End Property

        Private Sub NavFailed(e As WebViewNavigationFailedEventArgs)
            ' Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = True
        End Sub

        Private _retryCommand As ICommand

        Public ReadOnly Property RetryCommand As ICommand
            Get
                If _retryCommand Is Nothing Then
                    _retryCommand = New RelayCommand(AddressOf Retry)
                End If

                Return _retryCommand
            End Get
        End Property

        Private Sub Retry()
            IsShowingFailedMessage = False
            IsLoading = True

            _webView?.Refresh()
        End Sub

        Private _browserBackCommand As ICommand

        Public ReadOnly Property BrowserBackCommand As ICommand
            Get
                If _browserBackCommand Is Nothing Then
                    _browserBackCommand = New RelayCommand(Sub() _webView?.GoBack(), Function() If(_webView?.CanGoBack, False))
                End If

                Return _browserBackCommand
            End Get
        End Property

        Private _browserForwardCommand As ICommand

        Public ReadOnly Property BrowserForwardCommand As ICommand
            Get
                If _browserForwardCommand Is Nothing Then
                    _browserForwardCommand = New RelayCommand(Sub() _webView?.GoForward(), Function() If(_webView?.CanGoForward, False))
                End If

                Return _browserForwardCommand
            End Get
        End Property

        Private _refreshCommand As ICommand

        Public ReadOnly Property RefreshCommand As ICommand
            Get
                If _refreshCommand Is Nothing Then
                    _refreshCommand = New RelayCommand(Sub() _webView?.Refresh())
                End If

                Return _refreshCommand
            End Get
        End Property

        Private _openInBrowserCommand As ICommand

        Public ReadOnly Property OpenInBrowserCommand As ICommand
            Get
                If _openInBrowserCommand Is Nothing Then
                    _openInBrowserCommand = New RelayCommand(Async Sub() Await Windows.System.Launcher.LaunchUriAsync(Source))
                End If

                Return _openInBrowserCommand
            End Get
        End Property

        Private _webView As WebView

        Public Sub New()
            IsLoading = True
            Source = New Uri(DefaultUrl)
        End Sub

        Public Sub Initialize(webView As WebView)
            _webView = webView
        End Sub
    End Class
End Namespace
