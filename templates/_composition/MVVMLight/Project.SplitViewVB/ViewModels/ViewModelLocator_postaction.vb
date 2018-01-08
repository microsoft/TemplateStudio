Namespace ViewModels
    Public Class ViewModelLocator
        Public Sub New()
            '^^
            '{[{
            SimpleIoc.[Default].Register(Of ShellViewModel)()
            '}]}
        End Sub
        '{[{

        Public ReadOnly Property ShellViewModel As ShellViewModel
            Get
                Return ServiceLocator.Current.GetInstance(Of ShellViewModel)()
            End Get
        End Property
        '}]}
    End Class
End Namespace
