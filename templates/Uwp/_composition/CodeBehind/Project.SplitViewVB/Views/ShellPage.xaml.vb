Imports Windows.Foundation.Metadata
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports wts.ItemName.Services
Imports wts.ItemName.Helpers

Namespace Views
    ' TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    Partial Public NotInheritable Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private _selected As NavigationViewItem

        Public Property Selected As NavigationViewItem
            Get
                Return _selected
            End Get

            Set(value As NavigationViewItem)
                [Set](_selected, value)
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            HideNavViewBackButton()
            DataContext = Me
            Initialize()
        End Sub

        Private Sub Initialize()
            NavigationService.Frame = shellFrame
            AddHandler NavigationService.Navigated, AddressOf Frame_Navigated
            KeyboardAccelerators.Add(ActivationService.AltLeftKeyboardAccelerator)
            KeyboardAccelerators.Add(ActivationService.BackKeyboardAccelerator)
        End Sub

        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            Selected = navigationView.MenuItems.OfType(Of NavigationViewItem)().FirstOrDefault(Function(menuItem) IsMenuItemForPageType(menuItem, e.SourcePageType))
        End Sub

        Private Function IsMenuItemForPageType(menuItem As NavigationViewItem, sourcePageType As Type) As Boolean
            Dim pageType = TryCast(menuItem.GetValue(NavHelper.NavigateToProperty), Type)
            Return pageType = sourcePageType
        End Function

        Private Sub OnItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)
            Dim item = navigationView.MenuItems.OfType(Of NavigationViewItem)().First(Function(menuItem) CStr(menuItem.Content) = CStr(args.InvokedItem))
            Dim pageType = TryCast(item.GetValue(NavHelper.NavigateToProperty), Type)
            NavigationService.Navigate(pageType)
        End Sub

        Private Sub HideNavViewBackButton()
            If ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 6) Then
                navigationView.IsBackButtonVisible = NavigationViewBackButtonVisible.Collapsed
            End if
        End Sub

        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Private Sub [Set](Of T)(ByRef storage As T, value As T, <CallerMemberName> Optional propertyName As String = Nothing)
            If Equals(storage, value) Then
                Return
            End If

            storage = value
            OnPropertyChanged(propertyName)
        End Sub

        Private Sub OnPropertyChanged(propertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End Sub
    End Class
End Namespace
