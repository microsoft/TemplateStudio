'{**
' This code block adds the method `GetTreeViewDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
'^^
'{[{

        ' TODO WTS: Remove this once your TreeView page is displaying real data.
        Public Async Function GetTreeViewDataAsync() As Task(Of IEnumerable(Of SampleCompany))
            Await Task.CompletedTask

            Return AllCompanies()
        End Function
'}]}
    End Module
End Namespace