'{[{
Imports Param_RootNamespace.ViewModels
'}]}
Namespace Views
    Public NotInheritable Partial Class LogInPage
        Inherits Page

'{[{
        Public ReadOnly Property ViewModel As LogInViewModel = New LogInViewModel()
'}]}

        Public Sub New()
            InitializeComponent()
        End Sub
    End Class
End Namespace
