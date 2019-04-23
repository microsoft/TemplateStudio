Imports Microsoft.Xaml.Interactivity
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Windows.UI.Xaml.Navigation
Imports WinUI = Microsoft.UI.Xaml.Controls

Namespace Behaviors
    Public Class NavigationViewHeaderBehavior
        Inherits Behavior(Of WinUI.NavigationView)

        Private Shared _current As NavigationViewHeaderBehavior
        Private _currentPage As Page

        Public Property DefaultHeaderTemplate As DataTemplate

        Public Property DefaultHeader As Object
            Get
                Return GetValue(DefaultHeaderProperty)
            End Get
            Set(value As Object)
                SetValue(DefaultHeaderProperty, value)
            End Set
        End Property

        Public Shared ReadOnly DefaultHeaderProperty As DependencyProperty = DependencyProperty.Register("DefaultHeader", GetType(Object), GetType(NavigationViewHeaderBehavior), New PropertyMetadata(Nothing, Sub(d, e) _current.UpdateHeader()))

        Public Shared Function GetHeaderMode(item As Page) As NavigationViewHeaderMode
            Return CType(item.GetValue(HeaderModeProperty), NavigationViewHeaderMode)
        End Function

        Public Shared Sub SetHeaderMode(item As Page, value As NavigationViewHeaderMode)
            item.SetValue(HeaderModeProperty, value)
        End Sub

        Public Shared ReadOnly HeaderModeProperty As DependencyProperty = DependencyProperty.RegisterAttached("HeaderMode", GetType(Boolean), GetType(NavigationViewHeaderBehavior), New PropertyMetadata(NavigationViewHeaderMode.Always, Sub(d, e) _current.UpdateHeader()))

        Public Shared Function GetHeaderContext(item As Page) As Object
            Return item.GetValue(HeaderContextProperty)
        End Function

        Public Shared Sub SetHeaderContext(item As Page, value As Object)
            item.SetValue(HeaderContextProperty, value)
        End Sub

        Public Shared ReadOnly HeaderContextProperty As DependencyProperty = DependencyProperty.RegisterAttached("HeaderContext", GetType(Object), GetType(NavigationViewHeaderBehavior), New PropertyMetadata(Nothing, Sub(d, e) _current.UpdateHeader()))

        Public Shared Function GetHeaderTemplate(item As Page) As DataTemplate
            Return CType(item.GetValue(HeaderTemplateProperty), DataTemplate)
        End Function

        Public Shared Sub SetHeaderTemplate(item As Page, value As DataTemplate)
            item.SetValue(HeaderTemplateProperty, value)
        End Sub

        Public Shared ReadOnly HeaderTemplateProperty As DependencyProperty = DependencyProperty.RegisterAttached("HeaderTemplate", GetType(DataTemplate), GetType(NavigationViewHeaderBehavior), New PropertyMetadata(Nothing, Sub(d, e) _current.UpdateHeaderTemplate()))

        Protected Overrides Sub OnAttached()
            MyBase.OnAttached()
            _current = Me
        End Sub

        Protected Overrides Sub OnDetaching()
            MyBase.OnDetaching()
            RemoveHandler NavigationService.Navigated, AddressOf OnNavigated
        End Sub

        Private Sub OnNavigated(sender As Object, e As NavigationEventArgs)
            Dim frame = TryCast(sender, Frame)
            Dim page = TryCast(frame.Content, Page)

            If page IsNot Nothing Then
                _currentPage = page
                UpdateHeader()
                UpdateHeaderTemplate()
            End If
        End Sub

        Private Sub UpdateHeader()
            If _currentPage IsNot Nothing Then
                Dim headerMode = GetHeaderMode(_currentPage)

                If headerMode = NavigationViewHeaderMode.Never Then
                    AssociatedObject.Header = Nothing
                    AssociatedObject.AlwaysShowHeader = False
                Else
                    Dim headerFromPage = GetHeaderContext(_currentPage)

                    If headerFromPage IsNot Nothing Then
                        AssociatedObject.Header = headerFromPage
                    Else
                        AssociatedObject.Header = DefaultHeader
                    End If

                    If headerMode = NavigationViewHeaderMode.Always Then
                        AssociatedObject.AlwaysShowHeader = True
                    Else
                        AssociatedObject.AlwaysShowHeader = False
                    End If
                End If
            End If
        End Sub

        Private Sub UpdateHeaderTemplate()
            If _currentPage IsNot Nothing Then
                Dim headerTemplate = GetHeaderTemplate(_currentPage)
                AssociatedObject.HeaderTemplate = If(headerTemplate, DefaultHeaderTemplate)
            End If
        End Sub
    End Class
End Namespace
