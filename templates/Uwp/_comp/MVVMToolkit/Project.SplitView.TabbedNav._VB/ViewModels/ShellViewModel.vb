Imports WinUI = Microsoft.UI.Xaml.Controls
Imports Windows.System
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports Param_RootNamespace.Helpers
Imports Param_RootNamespace.Services
Imports Microsoft.Toolkit.Mvvm.ComponentModel
Imports Microsoft.Toolkit.Mvvm.Input

Namespace ViewModels

    Public Class ShellViewModel
        Inherits ObservableObject

        Private ReadOnly _altLeftKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu)
        Private ReadOnly _backKeyboardAccelerator As KeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack)

        Private _isBackEnabled As Boolean
        Private _keyboardAccelerators As IList(Of KeyboardAccelerator)
        Private _navigationView As WinUI.NavigationView

        Private _selected As WinUI.NavigationViewItem

        Private _loadedCommand As ICommand
        Private _itemInvokedCommand As ICommand

        Public Property IsBackEnabled As Boolean
            Get
                Return _isBackEnabled
            End Get

            Set(value As Boolean)
                SetProperty(_isBackEnabled, value)
            End Set
        End Property

        Public Property Selected As WinUI.NavigationViewItem
            Get
                Return _selected
            End Get

            Set(value As WinUI.NavigationViewItem)
                SetProperty(_selected, value)
            End Set
        End Property

        Public ReadOnly Property LoadedCommand As ICommand
            Get
                If _loadedCommand Is Nothing Then
                    _loadedCommand = New RelayCommand(AddressOf OnLoaded)
                End If

                Return _loadedCommand
            End Get
        End Property

        Public ReadOnly Property ItemInvokedCommand As ICommand
            Get
                If _itemInvokedCommand Is Nothing Then
                    _itemInvokedCommand = New RelayCommand(Of WinUI.NavigationViewItemInvokedEventArgs)(AddressOf OnItemInvoked)
                End If

                Return _itemInvokedCommand
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Sub Initialize(frame As Frame, navigationView As WinUI.NavigationView, keyboardAccelerators As IList(Of KeyboardAccelerator))
            _navigationView = navigationView
            _keyboardAccelerators = keyboardAccelerators
            NavigationService.Frame = frame
            AddHandler NavigationService.NavigationFailed, Function(sender, e)
                                                                Throw e.Exception
                                                            End Function
            AddHandler NavigationService.Navigated, AddressOf Frame_Navigated
            AddHandler _navigationView.BackRequested, AddressOf OnBackRequested
        End Sub

        Private Async Sub OnLoaded()
            ' Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            ' More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            _keyboardAccelerators.Add(_altLeftKeyboardAccelerator)
            _keyboardAccelerators.Add(_backKeyboardAccelerator)
            Await Task.CompletedTask
        End Sub

        Private Sub OnBackRequested(sender As WinUI.NavigationView, args As WinUI.NavigationViewBackRequestedEventArgs)
            NavigationService.GoBack()
        End Sub

        Private Sub OnItemInvoked(args As WinUI.NavigationViewItemInvokedEventArgs)
            Dim selectedItem As WinUI.NavigationViewItem = TryCast(args.InvokedItemContainer, WinUI.NavigationViewItem)
            Dim pageType = TryCast(selectedItem.GetValue(NavHelper.NavigateToProperty), Type)
            NavigationService.Navigate(pageType, Nothing, args.RecommendedNavigationTransitionInfo)
        End Sub

        Private Sub Frame_NavigationFailed(sender As Object, e As NavigationFailedEventArgs)
            Throw e.Exception
        End Sub

        Public Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            IsBackEnabled = NavigationService.CanGoBack
            Dim selectedItem = GetSelectedItem(_navigationView.MenuItems, e.SourcePageType)
            If selectedItem IsNot Nothing Then
                Selected = selectedItem
            End If
        End Sub

        Private Function GetSelectedItem(menuItems As IEnumerable(Of Object), pageType As Type) As WinUI.NavigationViewItem
            For Each item In menuItems.OfType(Of WinUI.NavigationViewItem)()

                If IsMenuItemForPageType(item, pageType) Then
                    Return item
                End If

                Dim selectedChild = GetSelectedItem(item.MenuItems, pageType)

                If selectedChild IsNot Nothing Then
                    Return selectedChild
                End If
            Next

            Return Nothing
        End Function

        Private Function IsMenuItemForPageType(menuItem As WinUI.NavigationViewItem, sourcePageType As Type) As Boolean
            Dim pageType = TryCast(menuItem.GetValue(NavHelper.NavigateToProperty), Type)
            Return pageType = sourcePageType
        End Function

        Private Shared Function BuildKeyboardAccelerator(key As VirtualKey, Optional modifiers As VirtualKeyModifiers? = Nothing) As KeyboardAccelerator
            Dim keyboardAccelerator = New KeyboardAccelerator() With {
                .Key = key
            }

            If modifiers.HasValue Then
                keyboardAccelerator.Modifiers = modifiers.Value
            End If

            AddHandler keyboardAccelerator.Invoked, AddressOf OnKeyboardAcceleratorInvoked
            Return keyboardAccelerator
        End Function

        Private Shared Sub OnKeyboardAcceleratorInvoked(sender As KeyboardAccelerator, args As KeyboardAcceleratorInvokedEventArgs)
            Dim result = NavigationService.GoBack()
            args.Handled = result
        End Sub
    End Class
End Namespace
