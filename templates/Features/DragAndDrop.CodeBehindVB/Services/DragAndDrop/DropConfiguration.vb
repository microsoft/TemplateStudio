Imports System.Threading.Tasks
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.Storage.Streams
Imports Param_ItemNamespace.Models

Namespace Services.DragAndDrop

    Public Class DropConfiguration
        Inherits DependencyObject

        Public Shared ReadOnly DropBitmapActionProperty As DependencyProperty = DependencyProperty.Register("DropBitmapAction", GetType(Action(Of RandomAccessStreamReference)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropHtmlActionProperty As DependencyProperty = DependencyProperty.Register("DropHtmlAction", GetType(Action(Of String)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropRtfActionProperty As DependencyProperty = DependencyProperty.Register("DropRtfAction", GetType(Action(Of String)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropStorageItemsActionProperty As DependencyProperty = DependencyProperty.Register("DropStorageItemsAction", GetType(Action(Of IReadOnlyList(Of IStorageItem))), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropTextActionProperty As DependencyProperty = DependencyProperty.Register("DropTextAction", GetType(Action(Of String)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropApplicationLinkActionProperty As DependencyProperty = DependencyProperty.Register("DropApplicationLinkAction", GetType(Action(Of Uri)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropWebLinkActionProperty As DependencyProperty = DependencyProperty.Register("DropWebLinkAction", GetType(Action(Of Uri)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropDataViewActionProperty As DependencyProperty = DependencyProperty.Register("DropDataViewAction", GetType(Action(Of DataPackageView)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DragEnterActionProperty As DependencyProperty = DependencyProperty.Register("DragEnterAction", GetType(Action(Of DragDropData)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DragOverActionProperty As DependencyProperty = DependencyProperty.Register("DragOverAction", GetType(Action(Of DragDropData)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DragLeaveActionProperty As DependencyProperty = DependencyProperty.Register("DragLeaveAction", GetType(Action(Of DragDropData)), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Property DropBitmapAction As Action(Of RandomAccessStreamReference)
            Get
                Return CType(GetValue(DropBitmapActionProperty), Action(Of RandomAccessStreamReference))
            End Get

            Set(value As Action(Of RandomAccessStreamReference))
                SetValue(DropBitmapActionProperty, value)
            End Set
        End Property

        Public Property DropHtmlAction As Action(Of String)
            Get
                Return CType(GetValue(DropHtmlActionProperty), Action(Of String))
            End Get

            Set(value As Action(Of String))
                SetValue(DropHtmlActionProperty, value)
            End Set
        End Property

        Public Property DropRtfAction As Action(Of String)
            Get
                Return CType(GetValue(DropRtfActionProperty), Action(Of String))
            End Get

            Set(value As Action(Of String))
                SetValue(DropRtfActionProperty, value)
            End Set
        End Property

        Public Property DropStorageItemsAction As Action(Of IReadOnlyList(Of IStorageItem))
            Get
                Return CType(GetValue(DropStorageItemsActionProperty), Action(Of IReadOnlyList(Of IStorageItem)))
            End Get

            Set(value As Action(Of IReadOnlyList(Of IStorageItem)))
                SetValue(DropStorageItemsActionProperty, value)
            End Set
        End Property

        Public Property DropTextAction As Action(Of String)
            Get
                Return CType(GetValue(DropTextActionProperty), Action(Of String))
            End Get

            Set(value As Action(Of String))
                SetValue(DropTextActionProperty, value)
            End Set
        End Property

        Public Property DropApplicationLinkAction As Action(Of Uri)
            Get
                Return CType(GetValue(DropApplicationLinkActionProperty), Action(Of Uri))
            End Get

            Set(value As Action(Of Uri))
                SetValue(DropApplicationLinkActionProperty, value)
            End Set
        End Property

        Public Property DropWebLinkAction As Action(Of Uri)
            Get
                Return CType(GetValue(DropWebLinkActionProperty), Action(Of Uri))
            End Get

            Set(value As Action(Of Uri))
                SetValue(DropWebLinkActionProperty, value)
            End Set
        End Property

        Public Property DropDataViewAction As Action(Of DataPackageView)
            Get
                Return CType(GetValue(DropDataViewActionProperty), Action(Of DataPackageView))
            End Get

            Set(value As Action(Of DataPackageView))
                SetValue(DropDataViewActionProperty, value)
            End Set
        End Property

        Public Property DragEnterAction As Action(Of DragDropData)
            Get
                Return CType(GetValue(DragEnterActionProperty), Action(Of DragDropData))
            End Get

            Set(value As Action(Of DragDropData))
                SetValue(DragEnterActionProperty, value)
            End Set
        End Property

        Public Property DragOverAction As Action(Of DragDropData)
            Get
                Return CType(GetValue(DragOverActionProperty), Action(Of DragDropData))
            End Get

            Set(value As Action(Of DragDropData))
                SetValue(DragOverActionProperty, value)
            End Set
        End Property

        Public Property DragLeaveAction As Action(Of DragDropData)
            Get
                Return CType(GetValue(DragLeaveActionProperty), Action(Of DragDropData))
            End Get

            Set(value As Action(Of DragDropData))
                SetValue(DragLeaveActionProperty, value)
            End Set
        End Property

        Public Async Function ProcessComandsAsync(dataview As DataPackageView) As Task
            If DropDataViewAction IsNot Nothing Then
                DropDataViewAction.Invoke(dataview)
            End If

            If dataview.Contains(StandardDataFormats.ApplicationLink) AndAlso DropApplicationLinkAction IsNot Nothing Then
                Dim uri As Uri = Await dataview.GetApplicationLinkAsync()
                DropApplicationLinkAction.Invoke(uri)
            End If

            If dataview.Contains(StandardDataFormats.Bitmap) AndAlso DropBitmapAction IsNot Nothing Then
                Dim stream As RandomAccessStreamReference = Await dataview.GetBitmapAsync()
                DropBitmapAction.Invoke(stream)
            End If

            If dataview.Contains(StandardDataFormats.Html) AndAlso DropHtmlAction IsNot Nothing Then
                Dim html As String = Await dataview.GetHtmlFormatAsync()
                DropHtmlAction.Invoke(html)
            End If

            If dataview.Contains(StandardDataFormats.Rtf) AndAlso DropRtfAction IsNot Nothing Then
                Dim rtf As String = Await dataview.GetRtfAsync()
                DropRtfAction.Invoke(rtf)
            End If

            If dataview.Contains(StandardDataFormats.StorageItems) AndAlso DropStorageItemsAction IsNot Nothing Then
                Dim storageItems As IReadOnlyList(Of IStorageItem) = Await dataview.GetStorageItemsAsync()
                DropStorageItemsAction.Invoke(storageItems)
            End If

            If dataview.Contains(StandardDataFormats.Text) AndAlso DropTextAction IsNot Nothing Then
                Dim text As String = Await dataview.GetTextAsync()
                DropTextAction.Invoke(text)
            End If

            If dataview.Contains(StandardDataFormats.WebLink) AndAlso DropWebLinkAction IsNot Nothing Then
                Dim uri As Uri = Await dataview.GetWebLinkAsync()
                DropWebLinkAction.Invoke(uri)
            End If
        End Function
    End Class
End Namespace
