Imports Param_ItemNamespace.ViewModels

Namespace Views
    ' TODO WTS: Remove this example page when/if it's not needed.
    ' This page is an example of how to launch a specific page in response to a protocol launch and pass it a value.
    ' It is expected that you will delete this page once you have changed the handling of a protocol launch to meet
    ' your needs and redirected to another of your pages.
    Public NotInheritable Partial Class SchemeActivationSamplePage
        Inherits Page

        Public ReadOnly Property ViewModel As New SchemeActivationSampleViewModel
    End Class
End Namespace
