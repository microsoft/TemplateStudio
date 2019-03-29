'{**
' This code block adds the method `GetContentGridData()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
'^^
'{[{

        Private _allOrders As IEnumerable(Of SampleOrder)

        ' TODO WTS: Remove this once your ContentGrid page is displaying real data.
        Public Function GetContentGridData() As ObservableCollection(Of SampleOrder)
            If _allOrders Is Nothing Then
                _allOrders = AllOrders()
            End If
            Return New ObservableCollection(Of SampleOrder)(_allOrders)
        End Function
'}]}
    End Module
End Namespace