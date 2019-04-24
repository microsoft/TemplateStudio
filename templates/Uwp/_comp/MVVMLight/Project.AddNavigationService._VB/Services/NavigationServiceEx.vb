Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media.Animation
Imports Windows.UI.Xaml.Navigation
Imports Param_RootNamespace.Helpers

Namespace Services
    Public Class NavigationServiceEx
        Public Event Navigated As NavigatedEventHandler

        Public Event NavigationFailed As NavigationFailedEventHandler

        Private ReadOnly _pages As Dictionary(Of String, Type) = New Dictionary(Of String, Type)()
        Private _frame As Frame
        Private _lastParamUsed As Object

        Public Property Frame As Frame
            Get

                If _frame Is Nothing Then
                    _frame = TryCast(Window.Current.Content, Frame)
                    RegisterFrameEvents()
                End If

                Return _frame
            End Get
            Set(value As Frame)
                UnregisterFrameEvents()
                _frame = value
                RegisterFrameEvents()
            End Set
        End Property

        Public ReadOnly Property CanGoBack As Boolean
            Get
                Return Frame.CanGoBack
            End Get
        End Property

        Public ReadOnly Property CanGoForward As Boolean
            Get
                Return Frame.CanGoForward
            End Get
        End Property

        Public Function GoBack() As Boolean
            If CanGoBack Then
                Frame.GoBack()
                Return True
            End If

            Return False
        End Function

        Public Sub GoForward()
            Frame.GoForward()
        End Sub

        Public Function Navigate(pageKey As String, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Boolean
            Dim page As Type = Nothing

            SyncLock _pages

                If Not _pages.TryGetValue(pageKey, page) Then
                    Throw New ArgumentException(String.Format("ExceptionNavigationServiceExPageNotFound".GetLocalized(), pageKey), NameOf(pageKey))
                End If
            End SyncLock

            If Frame.Content?.[GetType]() <> page OrElse (parameter IsNot Nothing AndAlso Not parameter.Equals(_lastParamUsed)) Then
                Dim navigationResult = Frame.Navigate(page, parameter, infoOverride)

                If navigationResult Then
                    _lastParamUsed = parameter
                End If

                Return navigationResult
            Else
                Return False
            End If
        End Function

        Public Sub Configure(key As String, pageType As Type)
            SyncLock _pages

                If _pages.ContainsKey(key) Then
                    Throw New ArgumentException(String.Format("ExceptionNavigationServiceExKeyIsInNavigationService".GetLocalized(), key))
                End If

                If _pages.Any(Function(p) p.Value = pageType) Then
                    Throw New ArgumentException(String.Format("ExceptionNavigationServiceExTypeAlreadyConfigured".GetLocalized(), _pages.First(Function(p) p.Value = pageType).Key))
                End If

                _pages.Add(key, pageType)
            End SyncLock
        End Sub

        Public Function GetNameOfRegisteredPage(page As Type) As String
            SyncLock _pages

                If _pages.ContainsValue(page) Then
                    Return _pages.FirstOrDefault(Function(p) p.Value = page).Key
                Else
                    Throw New ArgumentException(String.Format("ExceptionNavigationServiceExPageUnknown".GetLocalized(), page.Name))
                End If
            End SyncLock
        End Function

        Private Sub RegisterFrameEvents()
            If _frame IsNot Nothing Then
                AddHandler _frame.Navigated, AddressOf Frame_Navigated
                AddHandler _frame.NavigationFailed, AddressOf Frame_NavigationFailed
            End If
        End Sub

        Private Sub UnregisterFrameEvents()
            If _frame IsNot Nothing Then
                RemoveHandler _frame.Navigated, AddressOf Frame_Navigated
                RemoveHandler _frame.NavigationFailed, AddressOf Frame_NavigationFailed
            End If
        End Sub

        Public Sub Frame_NavigationFailed(sender As Object, e As NavigationFailedEventArgs)
            RaiseEvent NavigationFailed(sender, e)
        End Sub

        Public Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            RaiseEvent Navigated(sender, e)
        End Sub
    End Class
End Namespace
