Public Class Wire
    Public Property StartPoint As Point3D
    Public Property EndPoint As Point3D
    Public Property Diameter As Double
    Public Property Material As String

    Public Sub New(ByVal startPoint As Point3D, ByVal endPoint As Point3D, ByVal diameter As Double, ByVal material As String)
        Me.StartPoint = startPoint
        Me.EndPoint = endPoint
        Me.Diameter = diameter
        Me.Material = material
    End Sub
End Class
