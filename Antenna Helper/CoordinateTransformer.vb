Public Class CoordinateTransformer
    Private ReadOnly canvasWidth As Integer
    Private ReadOnly canvasHeight As Integer
    Private Const ScaleFactor As Double = 10 ' 1 foot = 10 pixels

    ' Initialize with canvas dimensions
    Public Sub New(ByVal width As Integer, ByVal height As Integer)
        canvasWidth = width
        canvasHeight = height
    End Sub

    ' Convert canvas pixel coordinates to backend coordinates
    Public Function PixelsToCoordinates(ByVal pixelX As Integer, ByVal pixelY As Integer) As Point3D
        Dim backendX As Double = (pixelX / ScaleFactor) - (canvasWidth / (2 * ScaleFactor))
        Dim backendZ As Double = (canvasHeight - pixelY) / ScaleFactor
        Return New Point3D(backendX, 0, backendZ)
    End Function

    ' Convert backend coordinates to canvas pixel coordinates
    Public Function CoordinatesToPixels(ByVal point As Point3D) As Point
        Dim pixelX As Integer = CInt((point.X + (canvasWidth / (2 * ScaleFactor))) * ScaleFactor)
        Dim pixelY As Integer = CInt(canvasHeight - (point.Z * ScaleFactor))
        Return New Point(pixelX, pixelY)
    End Function

    ' Apply a vertical offset for "ground zero" adjustments
    Public Function AdjustForGroundOffset(ByVal point As Point3D, ByVal groundOffset As Double) As Point3D
        Return New Point3D(point.X, point.Y, point.Z + groundOffset)
    End Function
End Class
