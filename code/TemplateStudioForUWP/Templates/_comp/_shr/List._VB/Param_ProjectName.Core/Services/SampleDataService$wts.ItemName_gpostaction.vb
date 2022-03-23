'{**
' This code block adds the method `GetListDetailsDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
'^^
'{[{

        ' Remove this once your ListDetails pages are displaying real data.
        Public Async Function GetListDetailsDataAsync() As Task(Of IEnumerable(Of SampleOrder))
            Await Task.CompletedTask

            Return AllOrders()
        End Function
'}]}
    End Module
End Namespace
