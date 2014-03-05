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
        Me.LoadBitmapToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.OpenToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.TypeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RectVToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RectHToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SmallToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.LargeToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label3 = New System.Windows.Forms.Label
        Me.TextBox3 = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
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
        Me.MenuStrip1.Size = New System.Drawing.Size(1194, 24)
        Me.MenuStrip1.TabIndex = 4
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'FileToolStripMenuItem
        '
        Me.FileToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.LoadBitmapToolStripMenuItem, Me.OpenToolStripMenuItem, Me.SaveToolStripMenuItem})
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
        'LoadBitmapToolStripMenuItem
        '
        Me.LoadBitmapToolStripMenuItem.Name = "LoadBitmapToolStripMenuItem"
        Me.LoadBitmapToolStripMenuItem.Size = New System.Drawing.Size(141, 22)
        Me.LoadBitmapToolStripMenuItem.Text = "Load Bitmap"
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
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(139, 7)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(31, 13)
        Me.Label3.TabIndex = 13
        Me.Label3.Text = "Type"
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(179, 4)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.Size = New System.Drawing.Size(100, 20)
        Me.TextBox3.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.BackColor = System.Drawing.Color.LawnGreen
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(1030, 116)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(74, 20)
        Me.Label2.TabIndex = 14
        Me.Label2.Text = "Walkable"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BackColor = System.Drawing.Color.Gray
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(1030, 136)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(91, 20)
        Me.Label4.TabIndex = 15
        Me.Label4.Text = "Impassable"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.BackColor = System.Drawing.Color.Gray
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(1030, 254)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(60, 20)
        Me.Label1.TabIndex = 17
        Me.Label1.Text = "Sprite2"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BackColor = System.Drawing.Color.LawnGreen
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(1030, 234)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 20)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Sprite"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BackColor = System.Drawing.Color.Gray
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(1030, 156)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(47, 20)
        Me.Label6.TabIndex = 18
        Me.Label6.Text = "None"
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
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1194, 1064)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.TextBox3)
        Me.Controls.Add(Me.MenuStrip1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "Form1"
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
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents LoadBitmapToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label

End Class
