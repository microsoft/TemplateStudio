'{[{
Imports Param_RootNamespace.Behaviors
Imports Windows.UI.Xaml.Data
'}]}

Namespace Views

    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            '{[{
            SetNavigationViewHeaderContext()
            SetNavigationViewHeaderTemplate()
            '}]}
        End Sub

        '{[{
        Private Sub OnInkToolbarLoaded(sender As Object, e As RoutedEventArgs)
            Dim inkToolbar As InkToolbar = TryCast(sender, InkToolbar)
            If inkToolbar IsNot Nothing Then
                inkToolbar.TargetInkCanvas = inkCanvas
            End If
        End Sub

        Private Sub VisualStateGroup_CurrentStateChanged(sender As Object, e As VisualStateChangedEventArgs)
            SetNavigationViewHeaderTemplate()
        End Sub

        Private Sub SetNavigationViewHeaderTemplate()
            If visualStateGroup.CurrentState IsNot Nothing Then

                Select Case visualStateGroup.CurrentState.Name
                    Case "BigVisualState"
                        NavigationViewHeaderBehavior.SetHeaderTemplate(Me, TryCast(Resources("BigHeaderTemplate"), DataTemplate))
                        bottomCommandBar.Visibility = Visibility.Collapsed
                    Case "SmallVisualState"
                        NavigationViewHeaderBehavior.SetHeaderTemplate(Me, TryCast(Resources("SmallHeaderTemplate"), DataTemplate))
                        bottomCommandBar.Visibility = Visibility.Visible
                End Select
            End If
        End Sub

        Private Sub SetNavigationViewHeaderContext()
            Dim headerContextBinding As Binding = new Binding()
            headerContextBinding.Source = Me
            headerContextBinding.Mode = BindingMode.OneWay

            SetBinding(NavigationViewHeaderBehavior.HeaderContextProperty, headerContextBinding)
        End Sub
        '}]}
    End Class
End Namespace