Namespace ViewModels
    Public Class ViewModelLocator
        Private Sub New()
            '^^
            '{[{
            Register(Of wts.ItemNameViewModel, wts.ItemNamePage)()
            '}]}
        End Sub

'{[{

        Public ReadOnly Property wts.ItemNameViewModel As wts.ItemNameViewModel
            Get
                ' A Guid is generated as a unique key for each instance as reusing the same VM instance in multiple MediaPlayerElement instances can cause playback errors
                Return SimpleIoc.[Default].GetInstance(Of wts.ItemNameViewModel)(Guid.NewGuid().ToString())
            End Get
        End Property
'}]}
    End Class
End Namespace
