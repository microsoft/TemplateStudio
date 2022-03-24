Namespace ViewModels
    Public Class ShellViewModel
        Inherits ObservableObject
        Private _keyboardAccelerators As IList(Of KeyboardAccelerator)
'^^
'{[{
        Private _menuFileswts.ItemNameCommand as ICommand
'}]}
        Private _menuFileExitCommand as ICommand
'^^
'{[{
        Public ReadOnly Property MenuFilewts.ItemNameCommand As ICommand
            Get
                If _menuFileswts.ItemNameCommand Is Nothing Then
                    _menuFileswts.ItemNameCommand = New RelayCommand(AddressOf OnMenuFilewts.ItemName)
                End If
                Return _menuFileswts.ItemNameCommand
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
            MenuNavigationHelper.OpenInRightPane(GetType(wts.ItemNamePage))
        End Sub

'}]}

    End Class
End Namespace
