Public Class CoordinateTransformer
    Private ReadOnly canvasWidth As Integer
    Private ReadOnly canvasHeight As Integer
    Public groundOffset As Double ' Offset to account for the new ground zero (in backend units)

    Public Sub New(ByVal width As Integer, ByVal height As Integer, Optional ByVal groundOffset As Double = 0.0)
        canvasWidth = width
        canvasHeight = height
        Me.groundOffset = groundOffset
    End Sub

    ' Converts canvas pixels to backend coordinates (mouse movement)
    Public Function PixelsToCoordinates(ByVal x As Integer, ByVal y As Integer) As Point3D
        Dim backendX = (x / 10) - (canvasWidth / 20)
        Dim backendZ = ((canvasHeight - y) / 10) ' No ground offset here for clean backend Z
        Return New Point3D(backendX, 0, backendZ)
    End Function

    ' Converts backend coordinates to canvas pixels (grid drawing)
    Public Function CoordinatesToPixels(ByVal point As Point3D) As Point
        Dim verticalBuffer As Integer = 20 ' Shift the grid upwards slightly
        Dim pixelX = CInt((point.X + (canvasWidth / 20)) * 10)
        Dim pixelY = CInt(canvasHeight - ((point.Z + groundOffset) * 10 + 10)) ' Apply ground offset for visuals
        Return New Point(pixelX, pixelY)
    End Function
End Class
