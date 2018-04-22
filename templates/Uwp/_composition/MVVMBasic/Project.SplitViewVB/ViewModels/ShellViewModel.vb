Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports wts.ItemName.Services
Imports wts.ItemName.Views
Imports wts.ItemName.Helpers

Namespace ViewModels

    Public Class ShellViewModel
        Inherits Observable

        Private _navigationView As NavigationView

        Private _selected As NavigationViewItem

        Private _itemInvokedCommand As ICommand

        Public Property Selected As NavigationViewItem
            Get
                Return _selected
            End Get

            Set(value As NavigationViewItem)
                [Set](_selected, value)
            End Set
        End Property

        Public ReadOnly Property ItemInvokedCommand As ICommand
            Get
                If _itemInvokedCommand Is Nothing Then
                    _itemInvokedCommand = New RelayCommand(Of NavigationViewItemInvokedEventArgs)(AddressOf OnItemInvoked)
                End If

                Return _itemInvokedCommand
            End Get
        End Property

        Public Sub Initialize(frame As Frame, navigationView As NavigationView)
            _navigationView = navigationView
            NavigationService.Frame = frame
            AddHandler NavigationService.Navigated, AddressOf Frame_Navigated
        End Sub

        Private Sub OnItemInvoked(args As NavigationViewItemInvokedEventArgs)
            Dim item = _navigationView.MenuItems.OfType(Of NavigationViewItem)().First(Function(menuItem) CStr(menuItem.Content) = CStr(args.InvokedItem))
            Dim pageType = TryCast(item.GetValue(NavHelper.NavigateToProperty), Type)
            NavigationService.Navigate(pageType)
        End Sub

        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            Selected = _navigationView.MenuItems.OfType(Of NavigationViewItem)().FirstOrDefault(Function(menuItem) IsMenuItemForPageType(menuItem, e.SourcePageType))
        End Sub

        Private Function IsMenuItemForPageType(menuItem As NavigationViewItem, sourcePageType As Type) As Boolean
            Dim pageType = TryCast(menuItem.GetValue(NavHelper.NavigateToProperty), Type)
            Return pageType = sourcePageType
        End Function
    End Class
End Namespace
