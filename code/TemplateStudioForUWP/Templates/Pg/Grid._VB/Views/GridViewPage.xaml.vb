﻿Namespace Views
    Public NotInheritable Partial Class GridViewPage
        Inherits Page

        ' TODO: Change the grid as appropriate to your app, adjust the column definitions on GridViewPage.xaml.
        ' For help see http://docs.telerik.com/windows-universal/controls/raddatagrid/gettingstarted
        ' You may also want to extend the grid to work with the RadDataForm http://docs.telerik.com/windows-universal/controls/raddataform/dataform-gettingstarted
        Public Sub New()
            InitializeComponent()
        End Sub

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)

            Await ViewModel.LoadDataAsync()
        End Sub
    End Class
End Namespace
