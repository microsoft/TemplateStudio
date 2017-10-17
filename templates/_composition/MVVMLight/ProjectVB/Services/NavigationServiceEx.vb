Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Media.Animation

Namespace Services
    Public Class NavigationServiceEx

        Private ReadOnly _pages As New Dictionary(Of String, Type)()

        Private Shared _frame As Frame

        Public Property Frame() As Frame
            Get
              If _frame Is Nothing Then
                  _frame = TryCast(Window.Current.Content, Frame)
              End If

              Return _frame
            End Get
            Set
                _frame = value
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
            Dim page As Type
            SyncLock _pages
                If Not _pages.TryGetValue(pageKey, page) Then
                    Throw New ArgumentException("Page not found: {pageKey}. Did you forget to call NavigationService.Configure?", "pageKey")
                End If
            End SyncLock
            Dim navigationResult = Frame.Navigate(page, parameter, infoOverride)
            Return navigationResult
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
    End Class
End Namespace
