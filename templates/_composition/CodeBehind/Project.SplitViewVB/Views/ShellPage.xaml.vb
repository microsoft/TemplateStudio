Imports Microsoft.Toolkit.Uwp.UI.Controls
Imports wts.ItemName.Services
Imports wts.ItemName.Helpers

Imports Microsoft.Toolkit.Uwp.UI.Controls

Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Private Const PanoramicStateName As String = "PanoramicState"
        Private Const WideStateName As String = "WideState"
        Private Const NarrowStateName As String = "NarrowState"
        Private Const WideStateMinWindowWidth As Double = 640
        Private Const PanoramicStateMinWindowWidth As Double = 1024

        Private _isPaneOpen As Boolean

        Public Property IsPaneOpen As Boolean
            Get
                Return _isPaneOpen
            End Get
            Set
                [Set](_isPaneOpen, value)
            End Set
        End Property

        Private _selected As Object

        Public Property Selected As Object
            Get
                Return _selected
            End Get

            Set(value As Object)
                [Set](_selected, value)
            End Set
        End Property

        Private _displayMode As SplitViewDisplayMode = SplitViewDisplayMode.CompactInline

        Public Property DisplayMode As SplitViewDisplayMode
            Get
                Return _displayMode
            End Get
            Set
                [Set](_displayMode, value)
            End Set
        End Property

        Private _lastSelectedItem As Object

        Private _primaryItems As New ObservableCollection(Of ShellNavigationItem)()

        Public ReadOnly Property PrimaryItems As ObservableCollection(Of ShellNavigationItem)
            Get
                Return _primaryItems
            End Get
        End Property

        Private _secondaryItems As New ObservableCollection(Of ShellNavigationItem)()

        Public ReadOnly Property SecondaryItems As ObservableCollection(Of ShellNavigationItem)
            Get
                Return _secondaryItems
            End Get
        End Property

        Public Sub New()
            Me.InitializeComponent()
            DataContext = Me
            Initialize()
        End Sub

        Private Sub Initialize()
            NavigationService.Frame = shellFrame
            AddHandler NavigationService.Frame.Navigated, AddressOf Frame_Navigated
            PopulateNavItems()

            InitializeState(Window.Current.Bounds.Width)
        End Sub

        Private Sub InitializeState(windowWith As Double)
            If windowWith < WideStateMinWindowWidth Then
                GoToState(NarrowStateName)
            ElseIf windowWith < PanoramicStateMinWindowWidth Then
                GoToState(WideStateName)
            Else
                GoToState(PanoramicStateName)
            End If
        End Sub

        Private Sub PopulateNavItems()
            _primaryItems.Clear()
            _secondaryItems.Clear()

            ' TODO WTS: Change the symbols for each item as appropriate for your app
            ' More on Segoe UI Symbol icons: https://docs.microsoft.com/windows/uwp/style/segoe-ui-symbol-font
            ' Or to use an IconElement instead of a Symbol see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/projectTypes/navigationpane.vb.md
            ' Edit String/en-US/Resources.resw: Add a menu item title for each page
        End Sub

        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            Dim navigationItem = Nothing
            
            If PrimaryItems IsNot Nothing
                navigationitem = PrimaryItems.FirstOrDefault(Function(i as ShellNavigationItem) i.PageType.Equals(e.SourcePageType))
            End If
            
            If navigationItem Is Nothing AndAlso SecondaryItems IsNot Nothing Then
                navigationItem = SecondaryItems.FirstOrDefault(Function(i as ShellNavigationItem) i.PageType.Equals(e.SourcePageType))
            End If

            If navigationItem IsNot Nothing Then
                ChangeSelected(_lastSelectedItem, navigationItem)
                _lastSelectedItem = navigationItem
            End If
        End Sub

        Private Sub ChangeSelected(oldValue As Object, newValue As Object)
            If oldValue IsNot Nothing Then
                TryCast(oldValue, ShellNavigationItem).IsSelected = False
            End If
            If newValue IsNot Nothing Then
                TryCast(newValue, ShellNavigationItem).IsSelected = True
                Selected = newValue
            End If
        End Sub
        
        Private Sub Navigate(item As Object)
            Dim navigationItem = TryCast(item, ShellNavigationItem)
            If navigationItem IsNot Nothing Then
                NavigationService.Navigate(navigationItem.PageType)
            End If
        End Sub

        Private Sub ItemInvoked(sender As Object, e As HamburgetMenuItemInvokedEventArgs)
            If DisplayMode = SplitViewDisplayMode.CompactOverlay OrElse DisplayMode = SplitViewDisplayMode.Overlay Then
                IsPaneOpen = False
            End If
            Navigate(e.InvokedItem)
        End Sub

        Private Sub OpenPane_Click(sender As Object, e As RoutedEventArgs)
            IsPaneOpen = Not _isPaneOpen
        End Sub
    
        Private Sub WindowStates_CurrentStateChanged(sender As Object, e As VisualStateChangedEventArgs)
            GoToState(e.NewState.Name)
        End Sub

        Private Sub GoToState(stateName As String)
            Select Case stateName
                Case PanoramicStateName
                    DisplayMode = SplitViewDisplayMode.CompactInline
                    Exit Select
                Case WideStateName
                    DisplayMode = SplitViewDisplayMode.CompactInline
                    IsPaneOpen = False
                    Exit Select
                Case NarrowStateName
                    DisplayMode = SplitViewDisplayMode.Overlay
                    IsPaneOpen = False
                    Exit Select
                Case Else
                    Exit Select
            End Select
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
