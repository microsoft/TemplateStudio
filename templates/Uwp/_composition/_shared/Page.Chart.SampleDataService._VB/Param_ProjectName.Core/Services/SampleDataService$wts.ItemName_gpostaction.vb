'{**
' This code block adds the method `GetChartSampleData()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
        '^^
        '{[{

        ' TODO WTS: Remove this once your chart page is displaying real data.
        Public Function GetChartSampleData() As ObservableCollection(Of DataPoint)
            Dim data = AllOrders().[Select](Function(o) New DataPoint() With {
                .Category = o.Company,
                .Value = o.OrderTotal
            }).OrderBy(Function(dp) dp.Category)

            Return New ObservableCollection(Of DataPoint)(data)
        End Function
        '}]}
    End Module
End Namespace
