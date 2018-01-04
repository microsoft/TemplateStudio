Imports Windows.ApplicationModel.DataTransfer
Imports Windows.UI.Xaml.Media.Imaging
Imports Param_RootNamespace.Models

Namespace Services.DragAndDrop

    ' For instructions on testing this service see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/drag-and-drop.md
    Public Class DragDropService

        Private Shared configurationProperty As DependencyProperty = DependencyProperty.RegisterAttached("Configuration", GetType(DropConfiguration), GetType(DragDropService), New PropertyMetadata(Nothing, AddressOf OnConfigurationPropertyChanged))

        Private Shared visualConfigurationProperty As DependencyProperty = DependencyProperty.RegisterAttached("VisualConfiguration", GetType(VisualDropConfiguration), GetType(DragDropService), New PropertyMetadata(Nothing, AddressOf OnVisualConfigurationPropertyChanged))

        Public Shared Sub SetConfiguration(dependencyObject As DependencyObject, value As DropConfiguration)
            If dependencyObject IsNot Nothing Then
                dependencyObject.SetValue(configurationProperty, value)
            End If
        End Sub

        Public Shared Function GetConfiguration(dependencyObject As DependencyObject) As DropConfiguration
            Return CType(dependencyObject.GetValue(configurationProperty), DropConfiguration)
        End Function

        Public Shared Sub SetVisualConfiguration(dependencyObject As DependencyObject, value As VisualDropConfiguration)
            If dependencyObject IsNot Nothing Then
                dependencyObject.SetValue(visualConfigurationProperty, value)
            End If
        End Sub

        Public Shared Function GetVisualConfiguration(dependencyObject As DependencyObject) As VisualDropConfiguration
            Return CType(dependencyObject.GetValue(visualConfigurationProperty), VisualDropConfiguration)
        End Function

        Private Shared Sub OnConfigurationPropertyChanged(dependencyObject As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim element = TryCast(dependencyObject, UIElement)
            Dim configuration = GetConfiguration(element)
            ConfigureUIElement(element, configuration)
            Dim listview = TryCast(element, ListViewBase)
            Dim listviewconfig = TryCast(configuration, ListViewDropConfiguration)
            If listview IsNot Nothing AndAlso listviewconfig IsNot Nothing Then
                ConfigureListView(listview, listviewconfig)
            End If
        End Sub

        Private Shared Sub ConfigureUIElement(element As UIElement, configuration As DropConfiguration)
        End Sub

        Private Shared Sub ConfigureListView(listview As ListViewBase, configuration As ListViewDropConfiguration)
        End Sub

        Private Shared Sub OnVisualConfigurationPropertyChanged(dependencyObject As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim element = TryCast(dependencyObject, UIElement)
            Dim configuration = GetVisualConfiguration(element)
            AddHandler element.DragStarting, Sub(sender, args)
                If configuration.DropOverImage IsNot Nothing Then
                    args.DragUI.SetContentFromBitmapImage(TryCast(configuration.DragStartingImage, BitmapImage))
                End If
            End Sub
            AddHandler element.DragOver, Sub(sender, args)
                args.DragUIOverride.Caption = configuration.Caption
                args.DragUIOverride.IsCaptionVisible = configuration.IsCaptionVisible
                args.DragUIOverride.IsContentVisible = configuration.IsContentVisible
                args.DragUIOverride.IsGlyphVisible = configuration.IsGlyphVisible
                If configuration.DropOverImage IsNot Nothing Then
                    args.DragUIOverride.SetContentFromBitmapImage(TryCast(configuration.DropOverImage, BitmapImage))
                End If
            End Sub
        End Sub
    End Class
End Namespace
