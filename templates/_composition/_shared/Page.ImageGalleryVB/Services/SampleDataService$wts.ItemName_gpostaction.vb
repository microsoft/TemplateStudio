'{**
' This code block adds the method `GetSampleModelDataAsync()` to the SampleDataService of your project.
'**}
Namespace Services
    Public Module SampleDataService
        '^^
        '{[{

        ' TODO WTS: Remove this once your image gallery page is displaying real data
        Public Function GetGallerySampleData() As ObservableCollection(Of SampleImage)
            Dim data = New ObservableCollection(Of SampleImage)()
            For i As Integer = 1 To 10
                data.Add(New SampleImage() With {
                    .ID = $"{i}",
                    .Source = $"ms-appx:///Assets/SampleData/SamplePhoto{i}.png",
                    .Name = $"Image sample {i}"
                })
            Next

            Return data
        End Function
        '}]}
    End Module
End Namespace
