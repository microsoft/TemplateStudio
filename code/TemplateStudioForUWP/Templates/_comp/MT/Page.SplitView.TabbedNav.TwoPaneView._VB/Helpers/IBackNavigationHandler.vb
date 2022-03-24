Namespace Helpers
    Interface IBackNavigationHandler
        Event OnPageCanGoBackChanged As EventHandler(Of Boolean)

        Sub GoBack()
    End Interface
End Namespace
