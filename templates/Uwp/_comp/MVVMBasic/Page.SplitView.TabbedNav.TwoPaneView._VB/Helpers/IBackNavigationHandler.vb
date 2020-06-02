Namespace Helpers
    Interface IBackNavigationHandler
        Private Event OnPageCanGoBackChanged As EventHandler(Of Boolean)
        Sub GoBack()
    End Interface
End Namespace
