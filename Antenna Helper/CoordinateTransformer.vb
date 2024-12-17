Public Class CoordinateTransformer
    Private ReadOnly canvasWidth As Integer
    Private ReadOnly canvasHeight As Integer
    Public groundOffset As Double  ' Offset to account for the new ground zero (in backend units)
    Public Property ZoomFactor As Double = 1.0
    Private transformer As CoordinateTransformer

    ' Constructor accepting an existing transformer
    Public Sub New(ByVal existingTransformer As CoordinateTransformer)
        transformer = existingTransformer
    End Sub



    Public Sub New(ByVal width As Integer, ByVal height As Integer, Optional ByVal groundOffset As Double = 0.0)
        canvasWidth = width
        canvasHeight = height
        Me.groundOffset = groundOffset
    End Sub

    ' Converts canvas pixels to backend coordinates (mouse movement)
    Public Function CoordinatesToPixels(ByVal point As Point3D) As Point
        Dim pixelX As Integer = CInt((point.X * 10 * ZoomFactor) + (canvasWidth / 2))
        Dim pixelY As Integer = CInt(canvasHeight - ((point.Z + groundOffset) * 10 * ZoomFactor))
        Return New Point(pixelX, pixelY)
    End Function

    Public Function PixelsToCoordinates(ByVal pixel As Point) As Point3D
        Dim coordX As Double = (pixel.X - (canvasWidth / 2)) / (10 * ZoomFactor)
        Dim coordZ As Double = ((canvasHeight - pixel.Y) / (10 * ZoomFactor)) - groundOffset
        Return New Point3D(coordX, 0, coordZ)
    End Function



    ' Adjusts backend coordinates for the ground offset
    Public Function AdjustForGroundOffset(ByVal point As Point3D) As Point3D
        Return New Point3D(point.X, point.Y, point.Z - groundOffset)
    End Function

End Class
