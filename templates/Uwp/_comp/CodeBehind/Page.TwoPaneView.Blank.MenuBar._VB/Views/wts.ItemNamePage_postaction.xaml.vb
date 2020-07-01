'{[{
Imports Windows.UI.Xaml
'}]}

Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged

'^^
'{[{
        Private _isGoBackButtonVisible As Boolean
'}]}
        Private _selected As SampleOrder

'^^
'{[{
        Public Property IsGoBackButtonVisible As Boolean
            Get
                Return _isGoBackButtonVisible
            End Get
            Set(value As Boolean)
                [Set](_isGoBackButtonVisible, value)
            End Set
        End Property
'}]}

        Public Property SampleItems As ObservableCollection(Of SampleOrder) = New ObservableCollection(Of SampleOrder)()

        Public Function TryCloseDetail() As Boolean
            If TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2 Then
'^^
'{[{
                RefreshIsGoBackButtonVisible()
'}]}
                Return True
            End If
        End Function

        Private Sub OnItemClick(sender As Object, e As ItemClickEventArgs)
            If twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane Then
'^^
'{[{
                RefreshIsGoBackButtonVisible()
'}]}
            End If
        End Sub

        Private Sub OnModeChanged(sender As WinUI.TwoPaneView, args As Object)
'^^
'{[{
            RefreshIsGoBackButtonVisible()
'}]}
        End Sub

'^^
'{[{
        Private Sub OnGoBack(sender As Object, e As RoutedEventArgs)
            TryCloseDetail()
        End Sub

        Private Sub RefreshIsGoBackButtonVisible()
            IsGoBackButtonVisible = twoPaneView.Mode = WinUI.TwoPaneViewMode.SinglePane AndAlso TwoPanePriority = WinUI.TwoPaneViewPriority.Pane2
        End Sub
'}]}
    End Class
End Namespace
