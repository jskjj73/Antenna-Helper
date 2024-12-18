﻿Public Class DrawingManager

    Private ReadOnly transformer As CoordinateTransformer

    Public Sub New(ByVal canvas As PictureBox, ByVal transformer As CoordinateTransformer)
        Me.canvas = canvas
        Me.transformer = transformer
    End Sub
    Private ReadOnly canvas As PictureBox
    Private mouseGuidelineEnd As Point3D
    Public startPoint As Point3D
    Private lockDirection As Boolean
    Private directionLockedAxis As String
    Private snapPoint As Point3D ' Stores the snapped endpoint
    Public Property ZoomFactor As Double = 1.0

    ' Expose guideline end as a read-only property
    Public ReadOnly Property GuidelineEnd As Point3D
        Get
            Return mouseGuidelineEnd
        End Get
    End Property

    Public ReadOnly Property CurrentSnapPoint As Point3D
        Get
            Return snapPoint
        End Get
    End Property

    ' Expose locked axis as a read-only property
    Public ReadOnly Property LockedAxis As String
        Get
            Return directionLockedAxis
        End Get
    End Property

    ' Constructor: Initialize with the canvas
    Public Sub New(ByVal canvas As PictureBox)
        Me.canvas = canvas
    End Sub

    ' Set the starting point for a wire
    Public Sub SetStartPoint(ByVal point As Point3D)
        startPoint = point
        lockDirection = False ' Reset lock state when starting a new wire
        directionLockedAxis = Nothing
    End Sub

    ' Set the guideline endpoint for V/H lock
    Public Sub SetGuidelineEnd(ByVal point As Point3D)
        mouseGuidelineEnd = point
    End Sub

    ' Handle mouse movement to update the guideline endpoint
    Public Function HandleMouseMove(ByVal mouseX As Double, ByVal mouseZ As Double, ByVal vHLockEnabled As Boolean) As Boolean
        If startPoint IsNot Nothing Then
            If vHLockEnabled Then
                ' Determine lock direction on first movement
                If Not lockDirection Then
                    Dim dx As Double = mouseX - startPoint.X
                    Dim dz As Double = mouseZ - startPoint.Z
                    lockDirection = True
                    directionLockedAxis = If(Math.Abs(dx) > Math.Abs(dz), "Horizontal", "Vertical")
                End If

                ' Lock endpoint based on the determined axis
                If directionLockedAxis = "Horizontal" Then
                    mouseGuidelineEnd = New Point3D(mouseX, 0, startPoint.Z) ' Lock Z
                ElseIf directionLockedAxis = "Vertical" Then
                    mouseGuidelineEnd = New Point3D(startPoint.X, 0, mouseZ) ' Lock X
                End If
            Else
                ' Free movement
                mouseGuidelineEnd = New Point3D(mouseX, 0, mouseZ)
            End If
            Return True ' Indicate canvas needs redrawing
        End If

        ' No active drawing, clear guideline
        mouseGuidelineEnd = Nothing
        Return False
    End Function



    ' Draw the grid on the canvas
    Public Sub DrawGrid(ByVal g As Graphics)
        'Debug.WriteLine("DrawGrid ZoomFactor (transformer): " & transformer.ZoomFactor)
        Dim gridSpacing As Integer = CInt(10 * transformer.ZoomFactor) ' Apply zoom factor to grid spacing
        Dim canvasWidth As Integer = canvas.Width
        Dim canvasHeight As Integer = canvas.Height
        ' Debug.WriteLine("Drawing grid with ZoomFactor: " & transformer.ZoomFactor)
        Dim gridPen As New Pen(Color.LightGray)

        ' Draw vertical grid lines
        For x As Integer = 0 To canvasWidth Step gridSpacing
            g.DrawLine(gridPen, x, 0, x, canvasHeight)
        Next

        ' Draw horizontal grid lines
        For y As Integer = 0 To canvasHeight Step gridSpacing
            g.DrawLine(gridPen, 0, y, canvasWidth, y)
        Next

        ' Draw center axis lines
        Dim axisPen As New Pen(Color.LightBlue, 2)
        g.DrawLine(axisPen, canvasWidth \ 2, 0, canvasWidth \ 2, canvasHeight) ' Vertical axis

        ' Draw the ground zero line
        Dim backendGroundZero = New Point3D(0, 0, 0) ' Ground zero in backend coordinates
        Dim groundPixelY = transformer.CoordinatesToPixels(backendGroundZero).Y - 20 ' Convert to canvas Y and apply transformation

        ' Check if the line is within the visible canvas bounds
        If groundPixelY >= 0 AndAlso groundPixelY <= canvasHeight Then
            g.DrawLine(Pens.Blue, 0, groundPixelY, canvasWidth, groundPixelY) ' Draw blue line
        Else
            Debug.WriteLine("Ground zero line is out of bounds!")
        End If
    End Sub








    ' Draw all wires on the canvas
    Public Sub DrawWires(ByVal g As Graphics, ByVal wires As List(Of Wire), ByVal selectedIndex As Integer)
        For i = 0 To wires.Count - 1
            Dim wire = wires(i)

            ' Convert wire coordinates to canvas space
            Dim startCanvasX = CInt((wire.StartPoint.X + (canvas.Width / 20)) * 10)
            Dim startCanvasY = CInt(canvas.Height - (wire.StartPoint.Z * 10))
            Dim endCanvasX = CInt((wire.EndPoint.X + (canvas.Width / 20)) * 10)
            Dim endCanvasY = CInt(canvas.Height - (wire.EndPoint.Z * 10))

            ' Select the pen color
            Dim wirePen As Pen = If(i = selectedIndex, New Pen(Color.Red, 2), New Pen(Color.Black, 2))

            ' Draw the wire
            g.DrawLine(wirePen, startCanvasX, startCanvasY, endCanvasX, endCanvasY)
            g.FillEllipse(Brushes.Black, startCanvasX - 3, startCanvasY - 3, 6, 6) ' Start point
            g.FillEllipse(Brushes.Black, endCanvasX - 3, endCanvasY - 3, 6, 6)     ' End point
        Next
    End Sub

    ' Draw the red guideline
    Public Sub DrawGuidelines(ByVal g As Graphics)
        If startPoint Is Nothing OrElse mouseGuidelineEnd Is Nothing Then Return

        ' Convert backend points to canvas coordinates
        Dim startPixel = transformer.CoordinatesToPixels(startPoint)
        Dim endPixel = transformer.CoordinatesToPixels(mouseGuidelineEnd)
        ' Use raw pixel values for alignment
        Dim startPixelX As Integer = transformer.CoordinatesToPixels(startPoint).X
        Dim startPixelY As Integer = transformer.CoordinatesToPixels(startPoint).Y
        Dim endPixelX As Integer = CInt(mouseGuidelineEnd.X) ' Use raw pixel X
        Dim endPixelY As Integer = CInt(mouseGuidelineEnd.Z) ' Use raw pixel Y


        ' Draw the guideline
        Dim guidelinePen As New Pen(Color.DarkRed, 1)
        g.DrawLine(guidelinePen, startPixel.X, startPixel.Y, endPixel.X, endPixel.Y)
    End Sub



    ' Draw snapping glow around the nearest endpoint
    Public Sub DrawSnapEffect(ByVal g As Graphics, ByVal snappedPoint As Point3D)
        If snappedPoint IsNot Nothing Then
            Dim snapCanvasX = CInt((snappedPoint.X + (canvas.Width / 20)) * 10)
            Dim snapCanvasY = CInt(canvas.Height - (snappedPoint.Z * 10))
            Dim glowPen As New Pen(Color.Red, 2)
            g.DrawEllipse(glowPen, snapCanvasX - 10, snapCanvasY - 10, 20, 20)
        End If
    End Sub

    ' Highlight the source wire
    Public Sub HighlightSourceWire(ByVal g As Graphics, ByVal wire As Wire, ByVal sourcePosition As String)
        If wire Is Nothing Then Return

        Dim x, y As Integer

        ' Calculate source highlight position
        Select Case sourcePosition
            Case "Beginning"
                x = CInt((wire.StartPoint.X + (canvas.Width / 20)) * 10)
                y = CInt(canvas.Height - (wire.StartPoint.Z * 10))
            Case "Center"
                x = CInt(((wire.StartPoint.X + wire.EndPoint.X) / 2 + (canvas.Width / 20)) * 10)
                y = CInt(canvas.Height - ((wire.StartPoint.Z + wire.EndPoint.Z) / 2 * 10))
            Case "End"
                x = CInt((wire.EndPoint.X + (canvas.Width / 20)) * 10)
                y = CInt(canvas.Height - (wire.EndPoint.Z * 10))
        End Select

        ' Draw the blue circle
        Dim sourcePen As New Pen(Color.Blue, 2)
        g.DrawEllipse(sourcePen, x - 5, y - 5, 10, 10)
    End Sub
    Public Sub SetSnapPoint(ByVal point As Point3D)
        If point IsNot Nothing Then
            ' Update snapPoint only if different
            If Not point.Equals(snapPoint) Then
                snapPoint = point
                canvas.Invalidate() ' Force canvas redraw
            End If
        Else
            ' Clear snapPoint and force canvas redraw
            If snapPoint IsNot Nothing Then
                snapPoint = Nothing
                canvas.Invalidate() ' Force canvas redraw
            End If
        End If
    End Sub






    Public Sub UpdateSnapEffect(ByVal mouseX As Double, ByVal mouseZ As Double, ByVal snapTolerance As Double, ByVal nearestPoint As Point3D)
        If nearestPoint IsNot Nothing Then
            ' If the nearest point is within snap tolerance, update snapPoint
            snapPoint = nearestPoint
        Else
            ' Clear snapPoint if no nearby point
            snapPoint = Nothing
        End If
    End Sub

    Public Sub DrawSnapEffect(ByVal g As Graphics)
        If snapPoint IsNot Nothing Then
            ' Calculate snapping effect coordinates
            Dim snapCanvasX = CInt((snapPoint.X + (canvas.Width / 20)) * 10)
            Dim snapCanvasY = CInt(canvas.Height - (snapPoint.Z * 10))

            ' Draw the red glow effect
            Dim glowPen As New Pen(Color.Red, 2)
            g.DrawEllipse(glowPen, snapCanvasX - 10, snapCanvasY - 10, 20, 20)
        End If
    End Sub


    Public Sub UpdateMouseGuidelineEnd(ByVal point As Point3D)
        mouseGuidelineEnd = point
    End Sub


End Class
