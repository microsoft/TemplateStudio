Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports wts.ItemName.Services
Imports wts.ItemName.Helpers

Namespace Views
    Partial Public NotInheritable Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private _selected As Object

        Public Property Selected As Object
            Get
                Return _selected
            End Get

            Set(value As Object)
                [Set](_selected, value)
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            DataContext = Me
            Initialize()
        End Sub

        Private Sub Initialize()
            NavigationService.Frame = shellFrame
            AddHandler NavigationService.Navigated, AddressOf Frame_Navigated
        End Sub

        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            Selected = navigationView.MenuItems.OfType(Of NavigationViewItem)().First(Function(menuItem) IsNavHelperForPageType(menuItem, e.SourcePageType))
        End Sub

        Private Function IsNavHelperForPageType(menuItem As NavigationViewItem, sourcePageType As Type) As Boolean
            Dim pageType = TryCast(menuItem.GetValue(NavHelper.NavigateToProperty), Type)
            Return pageType = sourcePageType
        End Function

        Private Sub OnItemInvoked(sender As NavigationView, args As NavigationViewItemInvokedEventArgs)
            Dim item = navigationView.MenuItems.OfType(Of NavigationViewItem)().First(Function(menuItem) CStr(menuItem.Content) = CStr(args.InvokedItem))
            Dim pageType = TryCast(item.GetValue(NavHelper.NavigateToProperty), Type)
            NavigationService.Navigate(pageType)
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
