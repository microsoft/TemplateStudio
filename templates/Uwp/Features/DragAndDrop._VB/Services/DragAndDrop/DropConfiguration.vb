Imports System.Threading.Tasks
Imports Windows.ApplicationModel.DataTransfer
Imports Windows.Storage
Imports Windows.Storage.Streams

Namespace Services.DragAndDrop

    Public Class DropConfiguration
        Inherits DependencyObject

        Public Shared ReadOnly DropBitmapCommandProperty As DependencyProperty = DependencyProperty.Register("DropBitmapCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropHtmlCommandProperty As DependencyProperty = DependencyProperty.Register("DropHtmlCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropRtfCommandProperty As DependencyProperty = DependencyProperty.Register("DropRtfCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropStorageItemsCommandProperty As DependencyProperty = DependencyProperty.Register("DropStorageItemsCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropTextCommandProperty As DependencyProperty = DependencyProperty.Register("DropTextCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropApplicationLinkCommandProperty As DependencyProperty = DependencyProperty.Register("DropApplicationLinkCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropWebLinkCommandProperty As DependencyProperty = DependencyProperty.Register("DropWebLinkCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DropDataViewCommandProperty As DependencyProperty = DependencyProperty.Register("DropDataViewCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DragEnterCommandProperty As DependencyProperty = DependencyProperty.Register("DragEnterCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DragOverCommandProperty As DependencyProperty = DependencyProperty.Register("DragOverCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Shared ReadOnly DragLeaveCommandProperty As DependencyProperty = DependencyProperty.Register("DragLeaveCommand", GetType(ICommand), GetType(DropConfiguration), New PropertyMetadata(Nothing))

        Public Property DropBitmapCommand As ICommand
            Get
                Return CType(GetValue(DropBitmapCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DropBitmapCommandProperty, value)
            End Set
        End Property

        Public Property DropHtmlCommand As ICommand
            Get
                Return CType(GetValue(DropHtmlCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DropHtmlCommandProperty, value)
            End Set
        End Property

        Public Property DropRtfCommand As ICommand
            Get
                Return CType(GetValue(DropRtfCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DropRtfCommandProperty, value)
            End Set
        End Property

        Public Property DropStorageItemsCommand As ICommand
            Get
                Return CType(GetValue(DropStorageItemsCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DropStorageItemsCommandProperty, value)
            End Set
        End Property

        Public Property DropTextCommand As ICommand
            Get
                Return CType(GetValue(DropTextCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DropTextCommandProperty, value)
            End Set
        End Property

        Public Property DropApplicationLinkCommand As ICommand
            Get
                Return CType(GetValue(DropApplicationLinkCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DropApplicationLinkCommandProperty, value)
            End Set
        End Property

        Public Property DropWebLinkCommand As ICommand
            Get
                Return CType(GetValue(DropWebLinkCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DropWebLinkCommandProperty, value)
            End Set
        End Property

        Public Property DropDataViewCommand As ICommand
            Get
                Return CType(GetValue(DropDataViewCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DropDataViewCommandProperty, value)
            End Set
        End Property

        Public Property DragEnterCommand As ICommand
            Get
                Return CType(GetValue(DragEnterCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DragEnterCommandProperty, value)
            End Set
        End Property

        Public Property DragOverCommand As ICommand
            Get
                Return CType(GetValue(DragOverCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DragOverCommandProperty, value)
            End Set
        End Property

        Public Property DragLeaveCommand As ICommand
            Get
                Return CType(GetValue(DragLeaveCommandProperty), ICommand)
            End Get

            Set(value As ICommand)
                SetValue(DragLeaveCommandProperty, value)
            End Set
        End Property

        Public Async Function ProcessComandsAsync(dataview As DataPackageView) As Task
            If DropDataViewCommand IsNot Nothing Then
                DropDataViewCommand.Execute(dataview)
            End If

            If dataview.Contains(StandardDataFormats.ApplicationLink) AndAlso DropApplicationLinkCommand IsNot Nothing Then
                Dim uri As Uri = Await dataview.GetApplicationLinkAsync()
                DropApplicationLinkCommand.Execute(uri)
            End If

            If dataview.Contains(StandardDataFormats.Bitmap) AndAlso DropBitmapCommand IsNot Nothing Then
                Dim stream As RandomAccessStreamReference = Await dataview.GetBitmapAsync()
                DropBitmapCommand.Execute(stream)
            End If

            If dataview.Contains(StandardDataFormats.Html) AndAlso DropHtmlCommand IsNot Nothing Then
                Dim html As String = Await dataview.GetHtmlFormatAsync()
                DropHtmlCommand.Execute(html)
            End If

            If dataview.Contains(StandardDataFormats.Rtf) AndAlso DropRtfCommand IsNot Nothing Then
                Dim rtf As String = Await dataview.GetRtfAsync()
                DropRtfCommand.Execute(rtf)
            End If

            If dataview.Contains(StandardDataFormats.StorageItems) AndAlso DropStorageItemsCommand IsNot Nothing Then
                Dim storageItems As IReadOnlyList(Of IStorageItem) = Await dataview.GetStorageItemsAsync()
                DropStorageItemsCommand.Execute(storageItems)
            End If

            If dataview.Contains(StandardDataFormats.Text) AndAlso DropTextCommand IsNot Nothing Then
                Dim text As String = Await dataview.GetTextAsync()
                DropTextCommand.Execute(text)
            End If

            If dataview.Contains(StandardDataFormats.WebLink) AndAlso DropWebLinkCommand IsNot Nothing Then
                Dim uri As Uri = Await dataview.GetWebLinkAsync()
                DropWebLinkCommand.Execute(uri)
            End If
        End Function
    End Class
End Namespace
