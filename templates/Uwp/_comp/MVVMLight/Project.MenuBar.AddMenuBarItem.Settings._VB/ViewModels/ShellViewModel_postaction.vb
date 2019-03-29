Namespace ViewModels
    Public Class ShellViewModel
        Inherits ViewModelBase
        Private _keyboardAccelerators As IList(Of KeyboardAccelerator)
'^^
'{[{
        Private _menuFilewts.ItemNameCommand as ICommand
'}]}
        Private _menuFileExitCommand as ICommand
'^^
'{[{
        Public ReadOnly Property MenuFilewts.ItemNameCommand As ICommand
            Get
                If _menuFilewts.ItemNameCommand Is Nothing Then
                    _menuFilewts.ItemNameCommand = New RelayCommand(AddressOf OnMenuFilewts.ItemName)
                End If
                Return _menuFilewts.ItemNameCommand
            End Get
        End Property

'}]}
        Public ReadOnly Property MenuFileExitCommand As ICommand
            Get
                If _menuFileExitCommand Is Nothing Then
                    _menuFileExitCommand = New RelayCommand(AddressOf OnMenuFileExit)
                End If

                Return _menuFileExitCommand
            End Get
        End Property
'^^
'{[{
        Private Sub OnMenuFilewts.ItemName()
            MenuNavigationHelper.OpenInRightPane(GetType(Views.wts.ItemNamePage))
        End Sub

'}]}

    End Class
End Namespace
