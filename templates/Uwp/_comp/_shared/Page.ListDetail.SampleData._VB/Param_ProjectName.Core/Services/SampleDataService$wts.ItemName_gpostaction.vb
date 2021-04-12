'{**
' This code block adds the method `GetListDetailDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
'^^
'{[{

        ' Remove this once your ListDetail pages are displaying real data.
        Public Async Function GetListDetailDataAsync() As Task(Of IEnumerable(Of SampleOrder))
            Await Task.CompletedTask

            Return AllOrders()
        End Function
'}]}
    End Module
End Namespace
