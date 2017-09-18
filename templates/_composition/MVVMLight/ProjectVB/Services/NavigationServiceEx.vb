Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media.Animation
Imports Windows.UI.Xaml.Navigation

Namespace Services
    Public Class NavigationServiceEx

        Private ReadOnly _pages As New Dictionary(Of String, Type)()

        Private Shared _frame As Frame

        Public Property Frame() As Frame
            Get
              If _frame Is Nothing Then
                  _frame = TryCast(Window.Current.Content, Frame)
                    RegisterFrameEvents()
              End If

              Return _frame
            End Get
            Set
                UnregisterFrameEvents()
                _frame = value
                RegisterFrameEvents()
            End Set
        End Property

        Public ReadOnly Property CanGoBack() As Boolean
            Get
                Return Frame.CanGoBack
            End Get
        End Property

        Public ReadOnly Property CanGoForward() As Boolean
            Get
                Return Frame.CanGoForward
            End Get
        End Property

        Public Sub GoBack()
            Frame.GoBack()
        End Sub

        Public Sub GoForward()
            Frame.GoForward()
        End Sub

        Public Function Navigate(pageKey As String, Optional parameter As Object = Nothing, Optional infoOverride As NavigationTransitionInfo = Nothing) As Boolean
            SyncLock _pages
                If Not _pages.ContainsKey(pageKey) Then
                    Throw New ArgumentException("Page not found: {pageKey}. Did you forget to call NavigationService.Configure?", NameOf(pageKey))
                End If
                Dim navigationResult = Frame.Navigate(_pages(pageKey), parameter, infoOverride)
                Return navigationResult
            End SyncLock
        End Function

        Public Sub Configure(key As String, pageType As Type)
            SyncLock _pages
                If _pages.ContainsKey(key) Then
                    Throw New ArgumentException("The key {key} is already configured in NavigationService")
                End If

                If _pages.Any(Function(p) p.Value.Equals(pageType)) Then
                    Throw New ArgumentException("This type is already configured with key {_pages.First(p => p.Value == pageType).Key}")
                End If

                _pages.Add(key, pageType)
            End SyncLock
        End Sub

        Public Function GetNameOfRegisteredPage(page As Type) As String
            SyncLock _pages
                If _pages.ContainsValue(page) Then
                    Return _pages.FirstOrDefault(Function(p) p.Value.Equals(page)).Key
                Else
                    Throw New ArgumentException("The page '{page.Name}' is unknown by the NavigationService")
                End If
            End SyncLock
        End Function

        Private Shared Sub RegisterFrameEvents()
            If _frame IsNot Nothing Then
                AddHandler _frame.Navigated, AddressOf Frame_Navigated
                AddHandler _frame.NavigationFailed, AddressOf Frame_NavigationFailed
            End If
        End Sub

        Private Shared Sub UnregisterFrameEvents()
            If _frame IsNot Nothing Then
                RemoveHandler _frame.Navigated, AddressOf Frame_Navigated
                RemoveHandler _frame.NavigationFailed, AddressOf Frame_NavigationFailed
            End If
        End Sub

        Private Shared Sub Frame_NavigationFailed(sender As Object, e As NavigationFailedEventArgs)
            RaiseEvent NavigationFailed(sender, e)
        End Sub 
        
        Private Shared Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            RaiseEvent Navigated(sender, e)
        End Sub
    End Class
End Namespace
