'{[{
Imports Param_RootNamespace.ViewModels
'}]}
Namespace Views
    Public NotInheritable Partial Class LogInPage
        Inherits Page

'{[{
        Private ReadOnly Property ViewModel As LogInViewModel
            Get
                Return ViewModelLocator.Current.LogInViewModel
            End Get
        End Property
'}]}

        Public Sub New()
            InitializeComponent()
        End Sub
    End Class
End Namespace
