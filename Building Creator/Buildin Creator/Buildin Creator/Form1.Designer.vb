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
        Me.SaveFileDialog1 = New System.Windows.Forms.SaveFileDialog
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RectVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RectHToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SmallToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LargeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label7 = New System.Windows.Forms.Label
        Me.WalkableBox = New System.Windows.Forms.ListBox
        Me.SpriteBox = New System.Windows.Forms.ListBox
        Me.TileBox = New System.Windows.Forms.ListBox
        Me.ScrollV = New System.Windows.Forms.VScrollBar
        Me.ScrollH = New System.Windows.Forms.HScrollBar
        Me.SetAll = New System.Windows.Forms.Button
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileToolStripMenuItem, Me.TypeToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1384, 24)
        Me.MenuStrip1.TabIndex = 4
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem})
        Me.FileToolStripMenuItem.Name = "FileToolStripMenuItem"
        Me.FileToolStripMenuItem.Size = New System.Drawing.Size(37, 20)
        Me.FileToolStripMenuItem.Text = "File"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.NewToolStripMenuItem.Text = "New"
        '
        'OpenToolStripMenuItem
        '
        Me.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem"
        Me.OpenToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.OpenToolStripMenuItem.Text = "Open"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'TypeToolStripMenuItem
        '
        Me.TypeToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RectVToolStripMenuItem, Me.RectHToolStripMenuItem, Me.SmallToolStripMenuItem, Me.LargeToolStripMenuItem})
        Me.TypeToolStripMenuItem.Name = "TypeToolStripMenuItem"
        Me.TypeToolStripMenuItem.Size = New System.Drawing.Size(45, 20)
        Me.TypeToolStripMenuItem.Text = "Type"
        '
        'RectVToolStripMenuItem
        '
        Me.RectVToolStripMenuItem.Name = "RectVToolStripMenuItem"
        Me.RectVToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.RectVToolStripMenuItem.Text = "RectV"
        '
        'RectHToolStripMenuItem
        '
        Me.RectHToolStripMenuItem.Name = "RectHToolStripMenuItem"
        Me.RectHToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.RectHToolStripMenuItem.Text = "RectH"
        '
        'SmallToolStripMenuItem
        '
        Me.SmallToolStripMenuItem.Name = "SmallToolStripMenuItem"
        Me.SmallToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.SmallToolStripMenuItem.Text = "Small"
        '
        'LargeToolStripMenuItem
        '
        Me.LargeToolStripMenuItem.Name = "LargeToolStripMenuItem"
        Me.LargeToolStripMenuItem.Size = New System.Drawing.Size(106, 22)
        Me.LargeToolStripMenuItem.Text = "Large"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(1084, 5)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(39, 13)
        Me.Label7.TabIndex = 19
        Me.Label7.Text = "Label7"
        '
        'WalkableBox
        '
        Me.WalkableBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.WalkableBox.FormattingEnabled = True
        Me.WalkableBox.ItemHeight = 24
        Me.WalkableBox.Items.AddRange(New Object() {"Walkable", "Impassable", "None"})
        Me.WalkableBox.Location = New System.Drawing.Point(1227, 27)
        Me.WalkableBox.Name = "WalkableBox"
        Me.WalkableBox.Size = New System.Drawing.Size(157, 76)
        Me.WalkableBox.TabIndex = 21
        '
        'SpriteBox
        '
        Me.SpriteBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 14.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SpriteBox.FormattingEnabled = True
        Me.SpriteBox.ItemHeight = 24
        Me.SpriteBox.Items.AddRange(New Object() {"Sprite", "Sprite2", "None"})
        Me.SpriteBox.Location = New System.Drawing.Point(1227, 109)
        Me.SpriteBox.Name = "SpriteBox"
        Me.SpriteBox.Size = New System.Drawing.Size(157, 76)
        Me.SpriteBox.TabIndex = 22
        '
        'TileBox
        '
        Me.TileBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TileBox.FormattingEnabled = True
        Me.TileBox.ItemHeight = 16
        Me.TileBox.Location = New System.Drawing.Point(1227, 191)
        Me.TileBox.Name = "TileBox"
        Me.TileBox.Size = New System.Drawing.Size(157, 356)
        Me.TileBox.TabIndex = 23
        '
        'ScrollV
        '
        Me.ScrollV.LargeChange = 2
        Me.ScrollV.Location = New System.Drawing.Point(0, 24)
        Me.ScrollV.Maximum = 102
        Me.ScrollV.Name = "ScrollV"
        Me.ScrollV.Size = New System.Drawing.Size(20, 951)
        Me.ScrollV.TabIndex = 24
        '
        'ScrollH
        '
        Me.ScrollH.LargeChange = 2
        Me.ScrollH.Location = New System.Drawing.Point(19, 975)
        Me.ScrollH.Maximum = 92
        Me.ScrollH.Name = "ScrollH"
        Me.ScrollH.Size = New System.Drawing.Size(1025, 20)
        Me.ScrollH.TabIndex = 25
        '
        'SetAll
        '
        Me.SetAll.Location = New System.Drawing.Point(1227, 553)
        Me.SetAll.Name = "SetAll"
        Me.SetAll.Size = New System.Drawing.Size(157, 37)
        Me.SetAll.TabIndex = 26
        Me.SetAll.Text = "Set All"
        Me.SetAll.UseVisualStyleBackColor = True
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1384, 994)
        Me.Controls.Add(Me.SetAll)
        Me.Controls.Add(Me.ScrollH)
        Me.Controls.Add(Me.ScrollV)
        Me.Controls.Add(Me.TileBox)
        Me.Controls.Add(Me.SpriteBox)
        Me.Controls.Add(Me.WalkableBox)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.MenuStrip1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Form1"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SaveFileDialog1 As System.Windows.Forms.SaveFileDialog
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents NewToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents OpenToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents TypeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RectVToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RectHToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SmallToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LargeToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents WalkableBox As System.Windows.Forms.ListBox
    Friend WithEvents SpriteBox As System.Windows.Forms.ListBox
    Friend WithEvents TileBox As System.Windows.Forms.ListBox
    Friend WithEvents ScrollV As System.Windows.Forms.VScrollBar
    Friend WithEvents ScrollH As System.Windows.Forms.HScrollBar
    Friend WithEvents SetAll As System.Windows.Forms.Button

End Class
