﻿using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.ViewModels;

public class ts.ItemNameViewModel : System.ComponentModel.INotifyPropertyChanged
{
    // TODO: Set the URI of the page to show by default
    private const string DefaultUrl = "https://docs.microsoft.com/windows/apps/";

    private readonly ISystemService _systemService;

    private string _source;
    private bool _isLoading = true;
    private bool _isShowingFailedMessage;
    private Visibility _isLoadingVisibility = Visibility.Visible;
    private Visibility _failedMesageVisibility = Visibility.Collapsed;
    private ICommand _refreshCommand;
    private System.Windows.Input.ICommand _browserBackCommand;
    private System.Windows.Input.ICommand _browserForwardCommand;
    private ICommand _openInBrowserCommand;
    private WebView2 _webView;

    public string Source
    {
        get { return _source; }
        set { Param_Setter(ref _source, value); }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            Param_Setter(ref _isLoading, value);
            IsLoadingVisibility = value ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public bool IsShowingFailedMessage
    {
        get => _isShowingFailedMessage;
        set
        {
            Param_Setter(ref _isShowingFailedMessage, value);
            FailedMesageVisibility = value ? Visibility.Visible : Visibility.Collapsed;
        }
    }

    public Visibility IsLoadingVisibility
    {
        get { return _isLoadingVisibility; }
        set { Param_Setter(ref _isLoadingVisibility, value); }
    }

    public Visibility FailedMesageVisibility
    {
        get { return _failedMesageVisibility; }
        set { Param_Setter(ref _failedMesageVisibility, value); }
    }

    public ICommand RefreshCommand => _refreshCommand ?? (_refreshCommand = new System.Windows.Input.ICommand(OnRefresh));

    public System.Windows.Input.ICommand BrowserBackCommand => _browserBackCommand ?? (_browserBackCommand = new System.Windows.Input.ICommand(() => _webView?.GoBack(), () => _webView?.CanGoBack ?? false));

    public System.Windows.Input.ICommand BrowserForwardCommand => _browserForwardCommand ?? (_browserForwardCommand = new System.Windows.Input.ICommand(() => _webView?.GoForward(), () => _webView?.CanGoForward ?? false));

    public ICommand OpenInBrowserCommand => _openInBrowserCommand ?? (_openInBrowserCommand = new System.Windows.Input.ICommand(OnOpenInBrowser));

    public ts.ItemNameViewModel(ISystemService systemService)
    {
        _systemService = systemService;
        Source = DefaultUrl;
    }

    public void Initialize(WebView2 webView)
    {
        _webView = webView;
    }

    public void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        IsLoading = false;
        if (e != null && !e.IsSuccess)
        {
            // Use `e.WebErrorStatus` to vary the displayed message based on the error reason
            IsShowingFailedMessage = true;
        }
    }

    private void OnRefresh()
    {
        IsShowingFailedMessage = false;
        IsLoading = true;
        _webView?.Reload();
    }

    private void OnOpenInBrowser()
        => _systemService.OpenInWebBrowser(Source);
}
