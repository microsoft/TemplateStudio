Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports Windows.Foundation
Imports Windows.UI.Input.Inking
Imports Windows.UI.Input.Inking.Analysis
Imports Windows.UI.Xaml
Imports Windows.UI.Xaml.Controls
Imports Param_ItemNamespace.EventHandlers.Ink

Namespace Services.Ink
    ' TODO WTS: InkAnalyzer requires installation of HandwritingRecognition in the active input language to be able to recognize words.
    ' For more info see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/ink.md#install-handwriting-recognition
    Public Class InkAsyncAnalyzer
        Private Const IdleWaitingTime As Double = 400
        Private ReadOnly _inkCanvas As InkCanvas
        Private ReadOnly _strokesService As InkStrokesService
        Private ReadOnly dispatcherTimer As DispatcherTimer

        Public Sub New(inkCanvas As InkCanvas, strokesService As InkStrokesService)
            _inkCanvas = inkCanvas
            _inkCanvas.InkPresenter.StrokeInput.StrokeStarted += Function(s, e) StopTimer()
            _inkCanvas.InkPresenter.StrokesErased += Function(s, e) RemoveStrokes(e.Strokes)
            _inkCanvas.InkPresenter.StrokesCollected += Function(s, e) AddStrokes(e.Strokes)
            _strokesService = strokesService
            _strokesService.AddStrokeEvent += AddressOf StrokesService_AddStrokeEvent
            _strokesService.RemoveStrokeEvent += AddressOf StrokesService_RemoveStrokeEvent
            _strokesService.MoveStrokesEvent += AddressOf StrokesService_MoveStrokesEvent
            _strokesService.CutStrokesEvent += AddressOf StrokesService_CutStrokesEvent
            _strokesService.PasteStrokesEvent += AddressOf StrokesService_PasteStrokesEvent
            _strokesService.ClearStrokesEvent += AddressOf StrokesService_ClearStrokesEvent
            _strokesService.LoadInkFileEvent += AddressOf StrokesService_LoadInkFileEvent
            dispatcherTimer = New DispatcherTimer()
            dispatcherTimer.Tick += AddressOf DispatcherTimer_Tick
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(IdleWaitingTime)
        End Sub

        Public Property InkAnalyzer As InkAnalyzer = New InkAnalyzer()

        Public ReadOnly Property IsAnalyzing As Boolean
            Get
                Return InkAnalyzer.IsAnalyzing
            End Get
        End Property

        Public Async Function AnalyzeAsync(Optional clean As Boolean = False) As Task(Of Boolean)
            StopTimer()

            If IsAnalyzing Then
                ' Ink analyzer is busy. Wait a while and try again.
                StartTimer()
                Return False
            End If

            If clean = True Then
                InkAnalyzer.ClearDataForAllStrokes()
                InkAnalyzer.AddDataForStrokes(_strokesService.GetStrokes())
            End If

            Dim result = Await InkAnalyzer.AnalyzeAsync()
            Return result.Status = InkAnalysisStatus.Updated
        End Function

        Public Function FindHitNode(position As Point) As IInkAnalysisNode
            ' Start with smallest scope
            Dim node = FindHitNodeByKind(position, InkAnalysisNodeKind.InkWord)

            If node Is Nothing Then
                node = FindHitNodeByKind(position, InkAnalysisNodeKind.InkBullet)

                If node Is Nothing Then
                    node = FindHitNodeByKind(position, InkAnalysisNodeKind.InkDrawing)
                End If
            End If

            Return node
        End Function

        Public Sub AddStroke(stroke As InkStroke)
            StopTimer()
            InkAnalyzer.AddDataForStroke(stroke)
            StartTimer()
        End Sub

        Public Sub AddStrokes(strokes As IReadOnlyList(Of InkStroke))
            StopTimer()
            InkAnalyzer.AddDataForStrokes(strokes)
            StartTimer()
        End Sub

        Public Sub RemoveStroke(stroke As InkStroke)
            StopTimer()
            InkAnalyzer.RemoveDataForStroke(stroke.Id)
            StartTimer()
        End Sub

        Public Sub RemoveStrokes(strokes As IReadOnlyList(Of InkStroke))
            StopTimer()

            For Each stroke In strokes
                ' Remove strokes from InkAnalyzer
                InkAnalyzer.RemoveDataForStroke(stroke.Id)
            Next

            StartTimer()
        End Sub

        Public Sub ReplaceStroke(stroke As InkStroke)
            InkAnalyzer.ReplaceDataForStroke(stroke)
        End Sub

        Public Sub ClearAnalysis()
            StopTimer()
            InkAnalyzer.ClearDataForAllStrokes()
        End Sub

        Public Sub StartTimer()
            Return dispatcherTimer.Start()
        End Sub

        Public Sub StopTimer()
            Return dispatcherTimer.[Stop]()
        End Sub

        Private Function FindHitNodeByKind(position As Point, kind As InkAnalysisNodeKind) As IInkAnalysisNode
            Dim nodes = InkAnalyzer.AnalysisRoot.FindNodes(kind)

            For Each node In nodes

                If RectHelper.Contains(node.BoundingRect, position) Then
                    Return node
                End If
            Next

            Return Nothing
        End Function

        Private Sub StrokesService_AddStrokeEvent(sender As Object, e As AddStrokeEventArgs)
            Return AddStroke(e.NewStroke)
        End Sub

        Private Sub StrokesService_RemoveStrokeEvent(sender As Object, e As RemoveEventArgs)
            Return RemoveStroke(e.RemovedStroke)
        End Sub

        Private Sub StrokesService_ClearStrokesEvent(sender As Object, e As EventArgs)
            Return ClearAnalysis()
        End Sub

        Private Async Sub StrokesService_MoveStrokesEvent(sender As Object, e As MoveStrokesEventArgs)
            For Each stroke In e.Strokes
                ReplaceStroke(stroke)
            Next

            ' Strokes are moved and the analysis result is not valid anymore.
            Await AnalyzeAsync(True)
        End Sub

        Private Sub StrokesService_PasteStrokesEvent(sender As Object, e As CopyPasteStrokesEventArgs)
            For Each stroke In e.Strokes
                AddStroke(stroke)
            Next
        End Sub

        Private Sub StrokesService_CutStrokesEvent(sender As Object, e As CopyPasteStrokesEventArgs)
            For Each stroke In e.Strokes
                RemoveStroke(stroke)
            Next
        End Sub

        Private Async Sub StrokesService_LoadInkFileEvent(sender As Object, e As EventArgs)
            Return Await AnalyzeAsync(True)
        End Sub

        Private Async Sub DispatcherTimer_Tick(sender As Object, e As Object)
            Return Await AnalyzeAsync()
        End Sub
    End Class
End Namespace
