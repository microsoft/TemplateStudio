Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits System.ComponentModel.INotifyPropertyChanged

'^^
'{[{
        Private _isGoBackButtonVisible As Boolean
        Private _goBackCommand As ICommand
'}]}
        Private _selected As SampleOrder

'^^
'{[{

        Public Property IsGoBackButtonVisible As Boolean
            Get
                Return _isGoBackButtonVisible
            End Get
            Set(value As Boolean)
                [Param_Setter](_isGoBackButtonVisible, value)
            End Set
        End Property
'}]}

        Public Property SampleItems As ObservableCollection(Of SampleOrder) = New ObservableCollection(Of SampleOrder)()

'^^
'{[{
        Public ReadOnly Property GoBackCommand As ICommand
            Get
                If _goBackCommand Is Nothing Then
                    _goBackCommand = New RelayCommand(AddressOf OnGoBack)
                End If

                Return _goBackCommand
            End Get
        End Property
'}]}

        Public Function TryCloseDetail() As Boolean
            If TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2 Then
'^^
'{[{
                RefreshIsGoBackButtonVisible()
'}]}
                Return True
            End If
        End Function

        Private Sub OnItemClick()
            If _twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane Then
'^^
'{[{
                RefreshIsGoBackButtonVisible()
'}]}
            End If
        End Sub

        Private Sub OnModeChanged(twoPaneView As WinUI.TwoPaneView)
'^^
'{[{
            RefreshIsGoBackButtonVisible()
'}]}
        End Sub
'^^
'{[{

        Private Sub OnGoBack()
            TryCloseDetail()
        End Sub

        Private Sub RefreshIsGoBackButtonVisible()
            IsGoBackButtonVisible = _twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane AndAlso TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2
        End Sub
'}]}
    End Class
End Namespace
