Imports Microsoft.Practices.ServiceLocation
Imports GalaSoft.MvvmLight
Imports GalaSoft.MvvmLight.Command
Imports wts.ItemName.Services
Imports wts.ItemName.Views
Imports Microsoft.Toolkit.Uwp.UI.Controls

Namespace ViewModels
    Public Class ShellViewModel
        Inherits ViewModelBase

        Private Const PanoramicStateName As String = "PanoramicState"
        Private Const WideStateName As String = "WideState"
        Private Const NarrowStateName As String = "NarrowState"
        Private Const WideStateMinWindowWidth As Double = 640
        Private Const PanoramicStateMinWindowWidth As Double = 1024

        Public ReadOnly Property NavigationService As NavigationServiceEx
            Get
                Return Microsoft.Practices.ServiceLocation.ServiceLocator.Current.GetInstance(Of NavigationServiceEx)()
            End Get
        End Property

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

        Private _openPaneCommand As ICommand

        Public ReadOnly Property OpenPaneCommand As ICommand
            Get
                If _openPaneCommand Is Nothing Then
                    _openPaneCommand = New RelayCommand(Sub()
                        IsPaneOpen = Not _isPaneOpen
                                                        End Sub)
                End If

                Return _openPaneCommand
            End Get
        End Property

        Private _itemSelected As ICommand

        Public ReadOnly Property ItemSelectedCommand As ICommand
            Get
                If _itemSelected Is Nothing Then
                    _itemSelected = New RelayCommand(Of HamburgetMenuItemInvokedEventArgs)(AddressOf ItemSelected)
                End If

                Return _itemSelected
            End Get
        End Property

        Private _stateChangedCommand As ICommand

        Public ReadOnly Property StateChangedCommand As ICommand
            Get
                If _stateChangedCommand Is Nothing Then
                    _stateChangedCommand = New RelayCommand(Of Windows.UI.Xaml.VisualStateChangedEventArgs)(Sub(args) GoToState(args.NewState.Name))
                End If

                Return _stateChangedCommand
            End Get
        End Property

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

        Public Sub Initialize(frame As Frame)
            NavigationService.Frame = frame
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

        Private Sub ItemSelected(args As HamburgetMenuItemInvokedEventArgs)
            If DisplayMode = SplitViewDisplayMode.CompactOverlay OrElse DisplayMode = SplitViewDisplayMode.Overlay Then
                IsPaneOpen = False
            End If
            Navigate(args.InvokedItem)
        End Sub

        Private Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
            If e IsNot Nothing Then
                Dim vm = NavigationService.GetNameOfRegisteredPage(e.SourcePageType)
                Dim navigationItem = PrimaryItems.FirstOrDefault(Function(i) i.ViewModelName = vm)
                If navigationItem Is Nothing Then
                    navigationItem = SecondaryItems.FirstOrDefault(Function(i) i.ViewModelName = vm)
                End If

                If navigationItem IsNot Nothing Then
                    ChangeSelected(_lastSelectedItem, navigationItem)
                    _lastSelectedItem = navigationItem
                End If
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
                NavigationService.Navigate(navigationItem.ViewModelName)
            End If
        End Sub
    End Class
End Namespace
