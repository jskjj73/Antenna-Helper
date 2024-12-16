Public Class WireManager
    Public Property Wires As New List(Of Wire)()
    Public Property CurrentSourceWireIndex As Integer = -1 ' Default to no source
    Public Property SourcePosition As String = Nothing ' "Beginning", "Center", "End"

    ' Add a wire to the list
    Public Sub AddWire(ByVal wire As Wire)
        Wires.Add(wire)
    End Sub

    ' Calculate the length of a wire between two points
    Public Function CalculateLength(ByVal start As Point3D, ByVal [end] As Point3D) As Double
        Dim dx As Double = [end].X - start.X
        Dim dy As Double = [end].Y - start.Y
        Dim dz As Double = [end].Z - start.Z
        Return Math.Sqrt(dx * dx + dy * dy + dz * dz)
    End Function

    ' Find the nearest endpoint within a snap tolerance
    Public Function FindNearestEndpoint(ByVal mouseX As Double, ByVal mouseZ As Double, ByVal snapTolerance As Double) As Point3D
        Dim nearestPoint As Point3D = Nothing
        Dim minDistance As Double = Double.MaxValue

        For Each wire In Wires
            Dim points = {wire.StartPoint, wire.EndPoint}
            For Each point In points
                Dim distance As Double = Math.Sqrt((point.X - mouseX) ^ 2 + (point.Z - mouseZ) ^ 2)
                If distance < snapTolerance AndAlso distance < minDistance Then
                    minDistance = distance
                    nearestPoint = point
                End If
            Next
        Next

        Return nearestPoint
    End Function



    ' Export wires to MMANA format
    Public Function ExportWiresToMMANA() As List(Of String)
        Dim output As New List(Of String)()

        ' Placeholder header
        output.Add("*")
        output.Add("0.0") ' Frequency placeholder

        ' Wires section
        output.Add("***Wires***")
        output.Add(Wires.Count.ToString())

        For Each wire As Wire In Wires
            Dim startX = wire.StartPoint.X * 0.3048 ' Convert feet to meters
            Dim startY = wire.StartPoint.Y * 0.3048
            Dim startZ = wire.StartPoint.Z * 0.3048
            Dim endX = wire.EndPoint.X * 0.3048
            Dim endY = wire.EndPoint.Y * 0.3048
            Dim endZ = wire.EndPoint.Z * 0.3048
            Dim diameter = wire.Diameter * 0.3048 ' Convert diameter to meters

            ' Format: StartX, StartY, StartZ, EndX, EndY, EndZ, Diameter, SegmentCount
            output.Add(String.Format("{0:F3}, {1:F3}, {2:F3}, {3:F3}, {4:F3}, {5:F3}, {6:E3}, -1",
                                     startX, startY, startZ, endX, endY, endZ, diameter))
        Next

        ' Source section
        output.Add("***Source***")
        If CurrentSourceWireIndex >= 0 AndAlso CurrentSourceWireIndex < Wires.Count Then
            ' Add the source index and segment placeholder
            output.Add("1, 0")

            ' Add the wire source in the format w#b/c/e, 0.0, 10.0
            Dim sourceWireNumber = CurrentSourceWireIndex + 1 ' MMANA wire numbers are 1-based
            Dim sourcePoint As String = "b" ' Default to beginning
            Select Case SourcePosition
                Case "Beginning"
                    sourcePoint = "b"
                Case "Center"
                    sourcePoint = "c"
                Case "End"
                    sourcePoint = "e"
            End Select
            output.Add(String.Format("w{0}{1}, 0.0, 10.0", sourceWireNumber, sourcePoint))
        Else
            ' MMANA expects "0, 0" if no source exists
            output.Add("0, 0")
        End If

        ' Load section
        output.Add("***Load***")
        output.Add("0, 0")

        ' Segmentation section
        output.Add("***Segmentation***")
        output.Add("800, 80, 2.0, 2")

        ' Geometry/Header section
        output.Add("***G/H/M/R/AzEl/X***")
        output.Add("1, 0.0, 0, 50.0, 120, 60, 0.0")

        ' Comment section (optional)
        output.Add("###Comment###")
        output.Add(String.Format("Created by John, km4ldx {0}", DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt")))

        ' Save to file
        Dim saveFileDialog As New SaveFileDialog()
        saveFileDialog.Filter = "MMANA Files (*.maa)|*.maa"
        saveFileDialog.Title = "Export to MMANA"

        If saveFileDialog.ShowDialog() = DialogResult.OK Then
            Dim filePath As String = saveFileDialog.FileName
            System.IO.File.WriteAllLines(filePath, output)
            MessageBox.Show("File exported successfully!")
        End If

        Return output
    End Function







    Public Function ConvertToImperial(ByVal decimalFeet As Double) As String
        Dim feet As Integer = Math.Floor(decimalFeet) ' Extract the integer feet
        Dim inches As Integer = Math.Round((decimalFeet - feet) * 12) ' Convert decimal portion to inches

        ' Handle cases where inches round to 12
        If inches = 12 Then
            feet += 1
            inches = 0
        End If

        Return String.Format("{0}' {1}""", feet, inches)
    End Function


End Class
