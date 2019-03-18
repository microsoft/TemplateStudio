Namespace Views

        Public Sub New()
            InitializeComponent()
        End Sub
'{[{

        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            If e.Parameter IsNot Nothing Then
                Dim parameters = TryCast(e.Parameter, Dictionary(Of String, String))

                If parameters IsNot Nothing Then
                    ViewModel.Initialize(parameters)
                End If
            End If
        End Sub
'}]}
End Namespace
