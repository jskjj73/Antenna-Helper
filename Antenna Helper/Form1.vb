Public Class Form1
    ' Instances of other classes
    Private wireManager As New WireManager()
    'Private drawingManager As DrawingManager

    ' Variables for drawing logic
    Private firstClick As Boolean = True ' Tracks whether the first click was made
    Private selectedWireIndex As Integer = -1 ' Tracks the currently selected wire
    Private snappedPoint As Point3D = Nothing ' Stores the snapped endpoint for snapping effect
    Private lblMousePosition As Label
    'Private contextMenu As ContextMenuStrip
    Private txtLengthInput As TextBox
    Private transformer As CoordinateTransformer
    Private drawingManager As DrawingManager


    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Ensure pbCanvas dimensions are valid
        If pbCanvas IsNot Nothing Then
            '  transformer = New CoordinateTransformer(pbCanvas.Width, pbCanvas.Height)
            transformer = New CoordinateTransformer(pbCanvas.Width, pbCanvas.Height, 2.0)

            drawingManager = New DrawingManager(pbCanvas, transformer)
        End If
    End Sub


    ' Form Load Event
    Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Me.KeyPreview = True

        'Dim transformer As New CoordinateTransformer(pbCanvas.Width, pbCanvas.Height)

        'drawingManager = New DrawingManager(pbCanvas, transformer)
        '' Initialize transformer
        'transformer = New CoordinateTransformer(pbCanvas.Width, pbCanvas.Height)

        '' Pass transformer to DrawingManager
        'drawingManager = New DrawingManager(pbCanvas, transformer)

        ' Initialize context menu
        Dim dgvContextMenu As New ContextMenuStrip() ' Renamed from contextMenu

        ' Delete menu item
        Dim deleteMenuItem As New ToolStripMenuItem("Delete")
        AddHandler deleteMenuItem.Click, AddressOf DeleteMenuItem_Click
        dgvContextMenu.Items.Add(deleteMenuItem)

        ' Source menu items
        Dim sourceMenuItem As New ToolStripMenuItem("Source")
        Dim sourceBeginning As New ToolStripMenuItem("Beginning")
        Dim sourceCenter As New ToolStripMenuItem("Center")
        Dim sourceEnd As New ToolStripMenuItem("End")
        AddHandler sourceBeginning.Click, AddressOf SourceBeginning_Click
        AddHandler sourceCenter.Click, AddressOf SourceCenter_Click
        AddHandler sourceEnd.Click, AddressOf SourceEnd_Click
        sourceMenuItem.DropDownItems.AddRange({sourceBeginning, sourceCenter, sourceEnd})

        dgvContextMenu.Items.Add(sourceMenuItem)

        ' Attach context menu to DataGridView
        dgvWires.ContextMenuStrip = dgvContextMenu

        ' Initialize DataGridView for wire list
        dgvWires.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvWires.Columns.Clear()
        dgvWires.Columns.Add("StartX", "StartX")
        dgvWires.Columns.Add("StartY", "StartY")
        dgvWires.Columns.Add("StartZ", "StartZ")
        dgvWires.Columns.Add("EndX", "EndX")
        dgvWires.Columns.Add("EndY", "EndY")
        dgvWires.Columns.Add("EndZ", "EndZ")
        dgvWires.Columns.Add("Length", "Length")
        dgvWires.Columns.Add("Diameter", "Diameter")
        dgvWires.Columns.Add("Material", "Material")

        ' Refresh the wire list to reflect any initial data
        RefreshWireList()

        ' Initialize mouse position label
        lblMousePosition = New Label()
        lblMousePosition.AutoSize = True
        lblMousePosition.Font = New Font("Arial", 8, FontStyle.Regular)
        lblMousePosition.ForeColor = Color.Black
        lblMousePosition.BackColor = Color.White
        lblMousePosition.Visible = True
        pbCanvas.Controls.Add(lblMousePosition)

        ' Initialize length input textbox
        txtLengthInput = New TextBox()
        txtLengthInput.Visible = False
        txtLengthInput.Width = 50
        txtLengthInput.Font = New Font("Arial", 8)
        Me.Controls.Add(txtLengthInput)
        AddHandler txtLengthInput.KeyDown, AddressOf txtLengthInput_KeyDown
    End Sub


    ' Handles mouse clicks on the canvas
    Private Sub pbCanvas_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles pbCanvas.MouseDown
        Dim mouseX = (e.X / 10) - (pbCanvas.Width / 20)
        Dim mouseZ = (pbCanvas.Height - e.Y) / 10

        If e.Button = MouseButtons.Left Then
            If firstClick Then
                ' Set the starting point for the wire
                Dim startPoint = If(drawingManager.CurrentSnapPoint IsNot Nothing, drawingManager.CurrentSnapPoint, New Point3D(mouseX, 0, mouseZ))
                drawingManager.SetStartPoint(startPoint)
                firstClick = False
            Else
                ' Use the guideline endpoint if V/H Lock is enabled, else use snapPoint or mouse position
                Dim endPoint As Point3D

                If chkLockDirection.Checked Then
                    ' Use the locked guideline endpoint
                    endPoint = drawingManager.GuidelineEnd
                Else
                    ' Use snapPoint if snapping is active, otherwise use mouse position
                    endPoint = If(drawingManager.CurrentSnapPoint IsNot Nothing, drawingManager.CurrentSnapPoint, New Point3D(mouseX, 0, mouseZ))
                End If

                ' Finalize the wire
                Dim newWire As New Wire(drawingManager.StartPoint, endPoint, 0.1, "Copper")
                wireManager.AddWire(newWire)

                ' Reset for the next wire
                drawingManager.SetStartPoint(Nothing)
                firstClick = True
                snappedPoint = Nothing

                ' Refresh UI
                RefreshWireList()
                pbCanvas.Invalidate()
            End If
        ElseIf e.Button = MouseButtons.Right Then
            ' Cancel the current wire drawing process
            ResetDrawingState()
        End If
    End Sub



    ' Handles mouse movement for guideline updates and snapping
    Private Sub pbCanvas_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles pbCanvas.MouseMove
        If transformer Is Nothing Then Return ' Ensure transformer is not null

        ' Convert pixel coordinates to backend coordinates
        Dim backendPoint = transformer.PixelsToCoordinates(e.Location)

        Dim mouseX = backendPoint.X
        Dim mouseZ = backendPoint.Z

        ' Adjust mouseZ for ground zero and snap to zero for precision
        Dim adjustedMouseZ As Double = backendPoint.Z - transformer.groundOffset
        If Math.Abs(adjustedMouseZ) < 0.01 Then adjustedMouseZ = 0 ' Snap small values to zero

        ' Update mouse position label
        lblMousePosition.Text = String.Format("({0:F2}, {1:F2}) (Zoom: {2:F1})", mouseX, adjustedMouseZ, transformer.ZoomFactor)

        ' Ensure the label stays within the canvas boundaries
        Dim labelX As Integer = e.X + 10
        Dim labelY As Integer = e.Y + 10

        If labelX + lblMousePosition.Width > pbCanvas.Width Then
            labelX = pbCanvas.Width - lblMousePosition.Width - 5
        End If

        If labelY + lblMousePosition.Height > pbCanvas.Height Then
            labelY = pbCanvas.Height - lblMousePosition.Height - 5
        End If

        lblMousePosition.Location = New Point(labelX, labelY)

        ' Find nearest endpoint for snapping
        Dim nearestPoint = wireManager.FindNearestEndpoint(mouseX, mouseZ, 0.5) ' Snap tolerance of 0.5 feet

        ' Update snapping effect
        If nearestPoint IsNot Nothing Then
            drawingManager.SetSnapPoint(nearestPoint) ' Set the snapping point
        Else
            drawingManager.SetSnapPoint(Nothing) ' Clear the snapping point
        End If

        ' Update the guideline
        Dim needsRedraw = drawingManager.HandleMouseMove(mouseX, adjustedMouseZ, chkLockDirection.Checked)

        ' Display line length while drawing
        If Not firstClick AndAlso drawingManager.StartPoint IsNot Nothing Then
            Dim length As Double = wireManager.CalculateLength(drawingManager.StartPoint, New Point3D(mouseX, 0, adjustedMouseZ))
            lblMousePosition.Text &= String.Format(" | Length: {0:F2} ft", length)
        End If

        ' Show length input box for V/H lock
        If chkLockDirection.Checked AndAlso Not firstClick Then
            txtLengthInput.Location = New Point(e.X + 15, e.Y + 15)
            txtLengthInput.Visible = True
            txtLengthInput.BringToFront()
            txtLengthInput.Focus()
        Else
            txtLengthInput.Visible = False
        End If

        ' Redraw the canvas if necessary
        If needsRedraw OrElse nearestPoint IsNot Nothing Then
            pbCanvas.Invalidate()
        End If
    End Sub




    ' Handles the canvas redraw
    Private Sub pbCanvas_Paint(ByVal sender As Object, ByVal e As PaintEventArgs) Handles pbCanvas.Paint
        Debug.WriteLine("Paint event ZoomFactor: " & transformer.ZoomFactor)
        Dim g As Graphics = e.Graphics

        ' Draw the grid, wires, and any active effects
        drawingManager.DrawGrid(g)
        drawingManager.DrawWires(g, wireManager.Wires, selectedWireIndex)
        drawingManager.DrawGuidelines(g)



        ' Draw snapping effect if a snapped point exists
        drawingManager.DrawSnapEffect(g)

        ' Highlight the source wire if one is set
        If wireManager.CurrentSourceWireIndex >= 0 Then
            Dim sourceWire = wireManager.Wires(wireManager.CurrentSourceWireIndex)
            drawingManager.HighlightSourceWire(g, sourceWire, wireManager.SourcePosition)
        End If
    End Sub


    ' Handles wire selection in the DataGridView
    Private Sub dgvWires_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs) Handles dgvWires.SelectionChanged
        If dgvWires.SelectedRows.Count > 0 Then
            ' Update the selected wire index
            selectedWireIndex = dgvWires.SelectedRows(0).Index
        Else
            selectedWireIndex = -1
        End If

        ' Redraw the canvas to highlight the selected wire
        pbCanvas.Invalidate()
    End Sub

    ' Refreshes the DataGridView to reflect the current wire list
    Private Sub RefreshWireList()
        dgvWires.Rows.Clear()

        For Each wire As Wire In wireManager.Wires
            Dim length = wireManager.CalculateLength(wire.StartPoint, wire.EndPoint)
            dgvWires.Rows.Add(
                wire.StartPoint.X.ToString("F3"),
                wire.StartPoint.Y.ToString("F3"),
                (wire.StartPoint.Z - transformer.groundOffset).ToString("F3"),
                wire.EndPoint.X.ToString("F3"),
                wire.EndPoint.Y.ToString("F3"),
                (wire.EndPoint.Z - transformer.groundOffset).ToString("F3"),
                String.Format("{0:F3} ft", length),
                String.Format("{0:F3} ft", wire.Diameter),
                wire.Material()
            )
        Next
    End Sub


    ' Resets the drawing state
    Private Sub ResetDrawingState()
        firstClick = True
        drawingManager.SetStartPoint(Nothing)
        snappedPoint = Nothing
        pbCanvas.Invalidate()
    End Sub

    ' Handles export to MMANA
    Private Sub btnExportMMAA_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportMMAA.Click
        wireManager.ExportWiresToMMANA()
    End Sub

    ' Handles deleting a wire from the context menu
    Private Sub DeleteMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs)
        If dgvWires.SelectedRows.Count > 0 Then
            ' Get the selected row index
            Dim selectedRowIndex = dgvWires.SelectedRows(0).Index

            ' Ensure the selected row index is valid
            If selectedRowIndex >= 0 AndAlso selectedRowIndex < wireManager.Wires.Count Then
                ' Remove the wire
                wireManager.Wires.RemoveAt(selectedRowIndex)

                ' Reset CurrentSourceWireIndex if it points to the deleted wire
                If wireManager.CurrentSourceWireIndex = selectedRowIndex Then
                    wireManager.CurrentSourceWireIndex = -1 ' Reset to no source
                    wireManager.SourcePosition = Nothing
                ElseIf wireManager.CurrentSourceWireIndex > selectedRowIndex Then
                    ' Adjust the index if a wire before the source was deleted
                    wireManager.CurrentSourceWireIndex -= 1
                End If

                ' Refresh the UI
                RefreshWireList()
                pbCanvas.Invalidate()
            End If
        End If
    End Sub


    ' Handles length input for V/H lock
    Private Sub txtLengthInput_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            Dim inputLength As Double
            If Double.TryParse(txtLengthInput.Text, inputLength) Then
                If drawingManager.LockedAxis = "Horizontal" Then
                    drawingManager.SetGuidelineEnd(New Point3D(drawingManager.StartPoint.X + inputLength, 0, drawingManager.StartPoint.Z))
                ElseIf drawingManager.LockedAxis = "Vertical" Then
                    drawingManager.SetGuidelineEnd(New Point3D(drawingManager.StartPoint.X, 0, drawingManager.StartPoint.Z + inputLength))
                End If
                pbCanvas_MouseDown(Me, New MouseEventArgs(MouseButtons.Left, 1, 0, 0, 0))
            End If
            txtLengthInput.Visible = False
            e.SuppressKeyPress = True
        ElseIf e.KeyCode = Keys.Escape Then
            txtLengthInput.Visible = False
        End If
    End Sub

    ' Handles source wire assignment
    Private Sub SourceBeginning_Click(ByVal sender As Object, ByVal e As EventArgs)
        SetSource("Beginning")
    End Sub

    Private Sub SourceCenter_Click(ByVal sender As Object, ByVal e As EventArgs)
        SetSource("Center")
    End Sub

    Private Sub SourceEnd_Click(ByVal sender As Object, ByVal e As EventArgs)
        SetSource("End")
    End Sub

    ' Shared logic for setting the source wire
    Private Sub SetSource(ByVal position As String)
        If dgvWires.SelectedRows.Count > 0 Then
            Dim selectedRowIndex As Integer = dgvWires.SelectedRows(0).Index
            If selectedRowIndex >= 0 AndAlso selectedRowIndex < wireManager.Wires.Count Then
                wireManager.CurrentSourceWireIndex = selectedRowIndex
                wireManager.SourcePosition = position
                dgvWires.Refresh()
                pbCanvas.Invalidate()
            End If
        Else
            MessageBox.Show("No wire selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
    End Sub

    Private Sub ExportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExportToolStripMenuItem.Click
        wireManager.ExportWiresToMMANA()

    End Sub

    Private Sub Form1_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.Control AndAlso e.KeyCode = Keys.Oemplus Then
            transformer.ZoomFactor += 0.1
            Debug.WriteLine("ZoomFactor increased to: " & transformer.ZoomFactor)
            pbCanvas.Invalidate()
        ElseIf e.Control AndAlso e.KeyCode = Keys.OemMinus Then
            transformer.ZoomFactor = Math.Max(0.1, transformer.ZoomFactor - 0.1)
            Debug.WriteLine("ZoomFactor decreased to: " & transformer.ZoomFactor)
            pbCanvas.Invalidate()
        End If
    End Sub


    Private Sub pbCanvas_MouseWheel(ByVal sender As Object, ByVal e As MouseEventArgs) Handles pbCanvas.MouseWheel
        If e.Delta > 0 Then
            transformer.ZoomFactor += 0.1
        Else
            transformer.ZoomFactor = Math.Max(0.1, transformer.ZoomFactor - 0.1)
        End If
        pbCanvas.Invalidate() ' Redraw canvas
    End Sub

End Class
