Namespace ViewModels
    Public Class ViewModelLocator
        Public Sub New()
            '^^
            '{[{
            Register(Of PivotViewModel, PivotPage)()
            '}]}
        End Sub
'{[{

        Public ReadOnly Property PivotViewModel As PivotViewModel
            Get
                Return ServiceLocator.Current.GetInstance(Of PivotViewModel)()
            End Get
        End Property
'}]}
    End Class
End Namespace
