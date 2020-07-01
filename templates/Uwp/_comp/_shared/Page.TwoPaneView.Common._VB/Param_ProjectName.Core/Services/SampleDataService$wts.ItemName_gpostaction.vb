'{**
' This code block adds the method `GetTwoPaneViewDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
'^^
'{[{

        ' TODO WTS: Remove this once your TwoPaneView pages are displaying real data.
        Async Function GetTwoPaneViewDataAsync() As Task(Of IEnumerable(Of SampleOrder))
            Await Task.CompletedTask
            Return AllOrders()
        End Function
'}]}
    End Module
End Namespace
