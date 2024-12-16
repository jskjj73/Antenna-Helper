<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.pbCanvas = New System.Windows.Forms.PictureBox()
        Me.btnAddWire = New System.Windows.Forms.Button()
        Me.btnExportMMAA = New System.Windows.Forms.Button()
        Me.dgvWires = New System.Windows.Forms.DataGridView()
        Me.cmbMaterial = New System.Windows.Forms.ComboBox()
        Me.txtDiameter = New System.Windows.Forms.TextBox()
        Me.lblLength = New System.Windows.Forms.Label()
        Me.MView = New System.Windows.Forms.Button()
        Me.chkLockDirection = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExportToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ExitToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.pbCanvas, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.dgvWires, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbCanvas
        '
        Me.pbCanvas.Location = New System.Drawing.Point(93, 12)
        Me.pbCanvas.Name = "pbCanvas"
        Me.pbCanvas.Size = New System.Drawing.Size(1096, 492)
        Me.pbCanvas.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize
        Me.pbCanvas.TabIndex = 0
        Me.pbCanvas.TabStop = False
        '
        'btnAddWire
        '
        Me.btnAddWire.Location = New System.Drawing.Point(12, 615)
        Me.btnAddWire.Name = "btnAddWire"
        Me.btnAddWire.Size = New System.Drawing.Size(75, 23)
        Me.btnAddWire.TabIndex = 1
        Me.btnAddWire.Text = "Wires"
        Me.btnAddWire.UseVisualStyleBackColor = True
        Me.btnAddWire.Visible = False
        '
        'btnExportMMAA
        '
        Me.btnExportMMAA.Location = New System.Drawing.Point(12, 573)
        Me.btnExportMMAA.Name = "btnExportMMAA"
        Me.btnExportMMAA.Size = New System.Drawing.Size(75, 23)
        Me.btnExportMMAA.TabIndex = 2
        Me.btnExportMMAA.Text = "Export"
        Me.btnExportMMAA.UseVisualStyleBackColor = True
        '
        'dgvWires
        '
        Me.dgvWires.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvWires.Location = New System.Drawing.Point(93, 510)
        Me.dgvWires.Name = "dgvWires"
        Me.dgvWires.Size = New System.Drawing.Size(1096, 153)
        Me.dgvWires.TabIndex = 3
        '
        'cmbMaterial
        '
        Me.cmbMaterial.FormattingEnabled = True
        Me.cmbMaterial.Location = New System.Drawing.Point(12, 48)
        Me.cmbMaterial.Name = "cmbMaterial"
        Me.cmbMaterial.Size = New System.Drawing.Size(75, 21)
        Me.cmbMaterial.TabIndex = 10
        '
        'txtDiameter
        '
        Me.txtDiameter.Location = New System.Drawing.Point(12, 92)
        Me.txtDiameter.Name = "txtDiameter"
        Me.txtDiameter.Size = New System.Drawing.Size(75, 20)
        Me.txtDiameter.TabIndex = 11
        '
        'lblLength
        '
        Me.lblLength.AutoSize = True
        Me.lblLength.Location = New System.Drawing.Point(12, 599)
        Me.lblLength.Name = "lblLength"
        Me.lblLength.Size = New System.Drawing.Size(39, 13)
        Me.lblLength.TabIndex = 12
        Me.lblLength.Text = "Label1"
        Me.lblLength.Visible = False
        '
        'MView
        '
        Me.MView.Location = New System.Drawing.Point(15, 544)
        Me.MView.Name = "MView"
        Me.MView.Size = New System.Drawing.Size(44, 23)
        Me.MView.TabIndex = 13
        Me.MView.Text = "View"
        Me.MView.UseVisualStyleBackColor = True
        Me.MView.Visible = False
        '
        'chkLockDirection
        '
        Me.chkLockDirection.AutoSize = True
        Me.chkLockDirection.Location = New System.Drawing.Point(11, 118)
        Me.chkLockDirection.Name = "chkLockDirection"
        Me.chkLockDirection.Size = New System.Drawing.Size(73, 17)
        Me.chkLockDirection.TabIndex = 14
        Me.chkLockDirection.Text = "H/V Lock"
        Me.chkLockDirection.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(44, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Material"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(9, 76)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 13)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Size (mm)"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1204, 24)
        Me.MenuStrip1.TabIndex = 17
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem, Me.ExportToolStripMenuItem, Me.ExitToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'ExportToolStripMenuItem
        '
        Me.ExportToolStripMenuItem.Name = "ExportToolStripMenuItem"
        Me.ExportToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.ExportToolStripMenuItem.Text = "Export"
        '
        'ExitToolStripMenuItem
        '
        Me.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem"
        Me.ExitToolStripMenuItem.Size = New System.Drawing.Size(108, 22)
        Me.ExitToolStripMenuItem.Text = "Exit"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1204, 675)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.chkLockDirection)
        Me.Controls.Add(Me.MView)
        Me.Controls.Add(Me.lblLength)
        Me.Controls.Add(Me.txtDiameter)
        Me.Controls.Add(Me.cmbMaterial)
        Me.Controls.Add(Me.dgvWires)
        Me.Controls.Add(Me.btnExportMMAA)
        Me.Controls.Add(Me.btnAddWire)
        Me.Controls.Add(Me.pbCanvas)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Form1"
        CType(Me.pbCanvas, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.dgvWires, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents pbCanvas As System.Windows.Forms.PictureBox
    Friend WithEvents btnAddWire As System.Windows.Forms.Button
    Friend WithEvents btnExportMMAA As System.Windows.Forms.Button
    Friend WithEvents dgvWires As System.Windows.Forms.DataGridView
    Friend WithEvents cmbMaterial As System.Windows.Forms.ComboBox
    Friend WithEvents txtDiameter As System.Windows.Forms.TextBox
    Friend WithEvents lblLength As System.Windows.Forms.Label
    Friend WithEvents MView As System.Windows.Forms.Button
    Friend WithEvents chkLockDirection As System.Windows.Forms.CheckBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExportToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ExitToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

End Class
