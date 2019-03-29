'{**
' This code block adds the method `GetGridSampleData()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
        '^^
        '{[{

        ' TODO WTS: Remove this once your grid page is displaying real data.
        Public Function GetGridSampleData() As ObservableCollection(Of SampleOrder)
            Return New ObservableCollection(Of SampleOrder)(AllOrders())
        End Function
        '}]}
    End Module
End Namespace
