'{**
' This code block adds the method `GetChartDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
'^^
'{[{

        ' TODO WTS: Remove this once your chart page is displaying real data.
        Public Async Function GetChartDataAsync() As Task(Of IEnumerable(Of DataPoint))
            Dim data = AllOrders().[Select](Function(o) New DataPoint() With {
                .Category = o.Company,
                .Value = o.OrderTotal
            }).OrderBy(Function(dp) dp.Category)

            Await Task.CompletedTask

            Return data
        End Function
'}]}
    End Module
End Namespace
