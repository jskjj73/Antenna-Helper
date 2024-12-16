Public Class UserInputManager
    Private ReadOnly wireManager As WireManager
    Private snappingTolerance As Double = 0.5 ' Feet
    Private lockDirection As Boolean = False
    Private directionLockedAxis As String

    Public Sub New(ByVal manager As WireManager)
        Me.wireManager = manager
    End Sub

    Public Function ProcessMouseClick(ByVal x As Double, ByVal z As Double, ByVal firstClick As Boolean, ByRef startPoint As Point3D) As Point3D
        If firstClick Then
            ' Handle snapping
            Dim snappedPoint = wireManager.FindNearestEndpoint(x, z, snappingTolerance)
            startPoint = If(snappedPoint IsNot Nothing, snappedPoint, New Point3D(x, 0, z))
            Return Nothing
        Else
            Dim snappedPoint = wireManager.FindNearestEndpoint(x, z, snappingTolerance)
            Return If(snappedPoint IsNot Nothing, snappedPoint, New Point3D(x, 0, z))
        End If
    End Function

    Public Sub EnableLockDirection(ByVal dx As Double, ByVal dz As Double)
        If Not lockDirection Then
            lockDirection = True
            directionLockedAxis = If(Math.Abs(dx) > Math.Abs(dz), "Horizontal", "Vertical")
        End If
    End Sub

    Public Function GetLockedEndPoint(ByVal startPoint As Point3D, ByVal currentX As Double, ByVal currentZ As Double) As Point3D
        If Not lockDirection Then
            ' Determine lock direction on first movement
            Dim dx As Double = currentX - startPoint.X
            Dim dz As Double = currentZ - startPoint.Z
            lockDirection = True
            directionLockedAxis = If(Math.Abs(dx) > Math.Abs(dz), "Horizontal", "Vertical")
        End If

        ' Lock the endpoint based on the determined axis
        If directionLockedAxis = "Horizontal" Then
            Return New Point3D(currentX, 0, startPoint.Z) ' Lock Z
        ElseIf directionLockedAxis = "Vertical" Then
            Return New Point3D(startPoint.X, 0, currentZ) ' Lock X
        End If

        Return New Point3D(currentX, 0, currentZ) ' Default behavior
    End Function

End Class
