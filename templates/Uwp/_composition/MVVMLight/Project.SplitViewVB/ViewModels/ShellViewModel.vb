Imports CommonServiceLocator
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
Imports wts.ItemName.Services
Imports wts.ItemName.Views
Imports wts.ItemName.Helpers

Namespace ViewModels
    Public Class ShellViewModel
        Inherits ViewModelBase

        Private _navigationView As NavigationView

        Private _selected As NavigationViewItem

        Private _itemInvokedCommand As ICommand

        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return CommonServiceLocator.ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
            End Get
        End Property

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
            Dim pageKey = TryCast(item.GetValue(NavHelper.NavigateToProperty), String)
            NavigationService.Navigate(pageKey)
        End Sub

        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            Selected = _navigationView.MenuItems.OfType(Of NavigationViewItem)().FirstOrDefault(Function(menuItem) IsMenuItemForPageType(menuItem, e.SourcePageType))
        End Sub

        Private Function IsMenuItemForPageType(menuItem As NavigationViewItem, sourcePageType As Type) As Boolean
            Dim navigatedPageKey = NavigationService.GetNameOfRegisteredPage(sourcePageType)
            Dim pageKey = TryCast(menuItem.GetValue(NavHelper.NavigateToProperty), String)
            Return pageKey = navigatedPageKey
        End Function
    End Class
End Namespace
