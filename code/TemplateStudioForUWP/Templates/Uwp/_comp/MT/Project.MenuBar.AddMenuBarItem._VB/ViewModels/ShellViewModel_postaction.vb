Namespace ViewModels
    Public Class ShellViewModel
        Inherits ObservableObject
        Private _keyboardAccelerators As IList(Of KeyboardAccelerator)
'^^
'{[{
        Private _menuViewswts.ItemNameCommand As ICommand
'}]}
        Private _menuFileExitCommand As ICommand
'^^
'{[{
        Public ReadOnly Property MenuViewswts.ItemNameCommand As ICommand
            Get
                If _menuViewswts.ItemNameCommand Is Nothing Then
                    _menuViewswts.ItemNameCommand = New RelayCommand(AddressOf OnMenuViewswts.ItemName)
                End If

                Return _menuViewswts.ItemNameCommand
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
        Private Sub OnMenuViewswts.ItemName()
            MenuNavigationHelper.UpdateView(GetType(wts.ItemNamePage))
        End Sub

'}]}
    End Class
End Namespace
