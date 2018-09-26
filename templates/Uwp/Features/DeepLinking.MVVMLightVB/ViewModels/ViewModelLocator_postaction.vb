Namespace ViewModels
    Public Class ViewModelLocator
        Private Sub New()
            '^^
            '{[{
            Register(Of SchemeActivationSampleViewModel, SchemeActivationSamplePage)()
            '}]}
        End Sub

'{[{

        Public ReadOnly Property SchemeActivationSampleViewModel As SchemeActivationSampleViewModel
            Get
                Return ServiceLocator.Current.GetInstance(Of SchemeActivationSampleViewModel)()
            End Get
        End Property
'}]}
    End Class
End Namespace
