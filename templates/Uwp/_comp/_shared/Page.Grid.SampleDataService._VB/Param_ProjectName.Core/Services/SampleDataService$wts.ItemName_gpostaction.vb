'{**
' This code block adds the method `GetGridDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
'^^
'{[{

        ' TODO WTS: Remove this once your grid page is displaying real data.
        Public Async Function GetGridDataAsync() As Task(Of IEnumerable(Of SampleOrder))
            Await Task.CompletedTask

            Return AllOrders()
        End Function
'}]}
    End Module
End Namespace
