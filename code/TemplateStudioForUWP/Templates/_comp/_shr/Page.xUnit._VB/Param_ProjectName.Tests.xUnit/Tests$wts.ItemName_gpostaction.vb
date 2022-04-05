'{[{
Imports Param_RootNamespace.ViewModels
'}]}

Public Class Tests
    '^^
    '{[{

    ' TODO: Add tests for functionality you add to wts.ItemNameViewModel.
    <Fact>
    Public Sub Testwts.ItemNameViewModelCreation()
        ' This test is trivial. Add your own tests for the logic you add to the ViewModel.
        Dim vm = new wts.ItemNameViewModel()
        Assert.NotNull(vm)
    End Sub
    '}]}
End Class
