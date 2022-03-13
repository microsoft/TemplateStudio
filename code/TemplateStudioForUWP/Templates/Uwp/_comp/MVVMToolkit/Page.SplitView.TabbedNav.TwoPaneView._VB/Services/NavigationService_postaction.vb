'{[{
Imports Param_RootNamespace.Helpers
'}]}

Namespace Services
    Module NavigationService
'^^
'{[{
        Public Event OnCurrentPageCanGoBackChanged As EventHandler(Of Boolean)

'}]}
        Public Event NavigationFailed As NavigationFailedEventHandler
        Private _lastParamUsed As Object
'{[{
        Private _canCurrentPageGoBack As Boolean
'}]}

        Function GoBack() As Boolean
'{[{
            If _canCurrentPageGoBack Then
                Dim element As FrameworkElement = TryCast(Frame.Content, FrameworkElement)

                If element IsNot Nothing Then
                    Dim navigationHandler As IBackNavigationHandler = TryCast(element.DataContext, IBackNavigationHandler)

                    If navigationHandler IsNot Nothing Then
                        navigationHandler.GoBack()
                        Return True
                    End If
                End If
            End If
'}]}
        End Function

        Private Sub RegisterFrameEvents()
            If _frame IsNot Nothing Then
                AddHandler _frame.Navigated, AddressOf Frame_Navigated
'{[{
                AddHandler _frame.Navigating, AddressOf Frame_Navigating
'}]}
            End If
        End Sub

        Private Sub UnregisterFrameEvents()
            If _frame IsNot Nothing Then
                RemoveHandler _frame.Navigated, AddressOf Frame_Navigated
'{[{
                RemoveHandler _frame.Navigating, AddressOf Frame_Navigating
'}]}
            End If
        End Sub

        Public Sub Frame_Navigated(sender As Object, e As NavigationEventArgs)
'^^
'{[{
            Dim element As FrameworkElement = TryCast(Frame.Content, FrameworkElement)

            If element IsNot Nothing Then
                Dim backNavigationHandler As IBackNavigationHandler = TryCast(element.DataContext, IBackNavigationHandler)

                If backNavigationHandler IsNot Nothing Then
                    AddHandler backNavigationHandler.OnPageCanGoBackChanged, AddressOf OnPageCanGoBackChanged
                End If
            End If

'}]}
            RaiseEvent Navigated(sender, e)
        End Sub
'^^
'{[{

        Private Sub Frame_Navigating(sender As Object, e As NavigatingCancelEventArgs)
            Dim element As FrameworkElement = TryCast(Frame.Content, FrameworkElement)

            If element IsNot Nothing Then
                Dim backNavigationHandler As IBackNavigationHandler = TryCast(element.DataContext, IBackNavigationHandler)

                If backNavigationHandler IsNot Nothing Then
                    RemoveHandler backNavigationHandler.OnPageCanGoBackChanged, AddressOf OnPageCanGoBackChanged
                    _canCurrentPageGoBack = False
                End If
            End If
        End Sub

        Private Sub OnPageCanGoBackChanged(sender As Object, canCurrentPageGoBack As Boolean)
            _canCurrentPageGoBack = canCurrentPageGoBack
            RaiseEvent OnCurrentPageCanGoBackChanged(sender, canCurrentPageGoBack)
        End Sub
'}]}
    End Module
End Namespace
