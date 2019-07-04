Imports Param_RootNamespace.Core.Models

Namespace TemplateSelectors
    Public Class SampleDataTemplateSelector
        Inherits DataTemplateSelector

        Public Property CompanyTemplate As DataTemplate

        Public Property OrderTemplate As DataTemplate

        Public Property OrderDetailTemplate As DataTemplate

        Protected Overrides Function SelectTemplateCore(item As Object) As DataTemplate
            If item IsNot Nothing Then
                Return GetTemplate(item)
            End If
            Return MyBase.SelectTemplateCore(item)
        End Function

        Protected Overrides Function SelectTemplateCore(item As Object, container As DependencyObject) As DataTemplate
            If item IsNot Nothing Then
                Return GetTemplate(item)
            End If
            Return MyBase.SelectTemplateCore(item, container)
        End Function

        Private Function GetTemplate(item As Object) As DataTemplate
            Select Case item.GetType()
                Case GetType(SampleCompany)
                    Return CompanyTemplate
                Case GetType(SampleOrder)
                    Return OrderTemplate
                Case GetType(SampleOrderDetail)
                    Return OrderDetailTemplate
            End Select

            Return Nothing
        End Function
    End Class
End Namespace