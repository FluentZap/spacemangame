Public Class Form1

    <Serializable()> Class Build_Tiles
        Public X As Byte
        Public Y As Byte
        Public Walkable As Byte
        Public Type As Byte
        Public Sprite As Byte
        Public Sprite2 As Byte

        Sub New(ByVal x As Byte, ByVal y As Byte, ByVal Type As Byte, ByVal Sprite As Byte, ByVal Sprite2 As Byte, ByVal Walkable As Byte)
            Me.X = x
            Me.Y = y
            Me.Type = Type
            Me.Sprite = Sprite
            Me.Sprite2 = Sprite2
            Me.Walkable = Walkable
        End Sub

    End Class


    Public Tile_Map(31, 31) As Build_Tiles
    Public Tile_active(31, 31) As Boolean

    Public Tile_Boxes(31, 31) As Integer

    Public Building_Type As Byte
    Public WriteNumber As Byte    
    Public WriteType As Byte
    Public Walkable As Byte = 3
    Public tile_size As Integer = 32
    Public showSprite2 As Boolean
    Public Tile_Sprites As Image

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        For x = 0 To 31
            For y = 0 To 31
                Tile_Map(x, y) = New Build_Tiles(0, 0, 0, 0, 0, 0)
            Next
        Next
        Walkable = 3
    End Sub

    Private Sub TextBox3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox3.KeyDown
        If e.KeyCode = Keys.Right AndAlso WriteNumber < 200 Then WriteNumber = CByte(WriteNumber + 1)
        If e.KeyCode = Keys.Left AndAlso WriteNumber > 0 Then WriteNumber = CByte(WriteNumber - 1)

        If Tile_Sprites Is Nothing Then Exit Sub
        Dim g As Graphics = Graphics.FromHwnd(Me.Handle)

        g.DrawImage(Tile_Sprites, 0, 30)
        g.DrawRectangle(Pens.Green, New Rectangle(WriteNumber * tile_size, 30, tile_size - 1, tile_size - 1))        

    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        Dim X As Byte
        If Byte.TryParse(TextBox3.Text, X) = True Then
            If X >= 0 AndAlso X <= 255 Then
                WriteType = X
                TextBox3.BackColor = Color.LightGreen
            Else
                TextBox3.BackColor = Color.Red
            End If

        Else
            TextBox3.Text = ""
            TextBox3.BackColor = Color.White
        End If


    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        For x = 0 To 31
            For y = 0 To 31                
                Tile_Map(x, y) = New Build_Tiles(0, 0, 0, 0, 0, 0)
                Tile_active(x, y) = False
            Next
        Next
        Label2.BackColor = Color.LawnGreen
        Label4.BackColor = Color.White
        Walkable = 3
        WriteNumber = 0        
        WriteType = 0
        Me.Refresh()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

            Dim Tile_List As HashSet(Of Build_Tiles) = New HashSet(Of Build_Tiles)
            Dim stream As New IO.FileStream(OpenFileDialog1.FileName, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)

            Dim amount_Byte(3) As Byte
            stream.Read(amount_Byte, 0, 4)
            Dim Amount As Int32 = BitConverter.ToInt32(amount_Byte, 0)
            Dim ByteBuffer(Amount * 20) As Byte

            stream.Read(ByteBuffer, 0, Amount * 20)
            Dim p As Integer
            For x = 0 To Amount - 1
                p = x * 6
                Tile_List.Add(New Build_Tiles(ByteBuffer(p), ByteBuffer(p + 1), ByteBuffer(p + 2), ByteBuffer(p + 3), ByteBuffer(p + 4), ByteBuffer(p + 5)))
            Next
            stream.Close()


            Label2.BackColor = Color.LawnGreen
            Label4.BackColor = Color.White
            Walkable = 3
            WriteNumber = 0            
            WriteType = 0

            For x = 0 To 31
                For y = 0 To 31
                    Tile_Map(x, y) = New Build_Tiles(0, 0, 0, 0, 0, 0)
                    Tile_active(x, y) = False
                Next
            Next

            For Each tile In Tile_List
                Tile_Map(tile.X, tile.Y) = tile
                Tile_active(tile.X, tile.Y) = True
            Next
            Me.Refresh()

        End If
    End Sub







    Function Load_Building(ByVal Building As String) As HashSet(Of Build_Tiles)
        Dim binary_formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim stream As New IO.FileStream(Application.StartupPath + "/data/BuildingMap/" + Building, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
        Dim Tile_List As HashSet(Of Build_Tiles) = DirectCast(binary_formatter.Deserialize(stream), HashSet(Of Build_Tiles))

        stream.Close()
        Return Tile_List
    End Function


    Sub Save_Building(ByVal FileName As String, ByVal Building As HashSet(Of Build_Tiles))        
        Dim stream As New IO.FileStream(FileName, IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None)
        Dim Amount As Int32 = Building.Count
        Dim ByteBuffer(Amount * 20 + 3) As Byte
        Dim amount_Byte(3) As Byte
        amount_Byte = BitConverter.GetBytes(Amount)
        For x = 0 To 3
            ByteBuffer(x) = amount_Byte(x)
        Next
        Dim y As Integer = 4
        For Each item In Building
            ByteBuffer(y) = item.X
            ByteBuffer(y + 1) = item.Y
            ByteBuffer(y + 2) = item.Type
            ByteBuffer(y + 3) = item.Sprite
            ByteBuffer(y + 4) = item.Sprite2
            ByteBuffer(y + 5) = item.Walkable
            y += 6
        Next
        stream.Write(ByteBuffer, 0, Amount * 20 + 4)

        stream.Close()
    End Sub

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If Tile_Sprites Is Nothing Then Exit Sub
        Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
        Dim pos As Point
        pos.X = e.X \ 32
        
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If pos.X >= 0 AndAlso pos.X <= 40 AndAlso e.Y >= 30 AndAlso e.Y <= 30 + tile_size Then
                WriteNumber = CByte(pos.X)
            End If
        End If

        g.DrawImage(Tile_Sprites, 0, 30)
        g.DrawRectangle(Pens.Green, New Rectangle(WriteNumber * tile_size, 30, tile_size - 1, tile_size - 1))


        pos.X = e.X \ tile_size
        pos.Y = (e.Y - 70) \ tile_size
        If e.Y < 70 Then pos.Y = -1
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If pos.X >= 0 AndAlso pos.X <= 31 AndAlso pos.Y >= 0 AndAlso pos.Y <= 31 Then

                Tile_Map(pos.X, pos.Y).X = CByte(pos.X)
                Tile_Map(pos.X, pos.Y).Y = CByte(pos.Y)
                If showSprite2 = False Then
                    Tile_Map(pos.X, pos.Y).Sprite = WriteNumber
                Else
                    Tile_Map(pos.X, pos.Y).Sprite2 = WriteNumber
                End If
                Tile_Map(pos.X, pos.Y).Type = WriteType
                If Walkable < 100 Then Tile_Map(pos.X, pos.Y).Walkable = Walkable

                Tile_active(pos.X, pos.Y) = True


                If showSprite2 = False Then
                    g.DrawImage(Tile_Sprites, pos.X * tile_size, 70 + pos.Y * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                Else
                    g.DrawImage(Tile_Sprites, pos.X * tile_size, 70 + pos.Y * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite2 * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                End If

                If Tile_active(pos.X, pos.Y) = True Then
                    If Tile_Map(pos.X, pos.Y).Walkable = 0 Then g.DrawRectangle(Pens.Red, New Rectangle(pos.X * tile_size, (pos.Y * tile_size) + 70, tile_size - 1, tile_size - 1))
                    If Tile_Map(pos.X, pos.Y).Walkable = 3 Then g.DrawRectangle(Pens.Green, New Rectangle(pos.X * tile_size, (pos.Y * tile_size) + 70, tile_size - 1, tile_size - 1))
                    If Tile_Map(pos.X, pos.Y).Walkable = 4 Then g.DrawRectangle(Pens.Blue, New Rectangle(pos.X * tile_size, (pos.Y * tile_size) + 70, tile_size - 1, tile_size - 1))
                End If
                g.DrawString(Tile_Map(pos.X, pos.Y).Type.ToString, Me.Font, Brushes.Black, pos.X * 32, 70 + pos.Y * 32)
                g.Dispose()
            End If
        End If

        If e.Button = Windows.Forms.MouseButtons.Right Then
            If pos.X >= 0 AndAlso pos.X <= 31 AndAlso pos.Y >= 0 AndAlso pos.Y <= 31 Then
                Tile_active(pos.X, pos.Y) = False

                If showSprite2 = False Then
                    g.DrawImage(Tile_Sprites, pos.X * tile_size, 70 + pos.Y * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                Else
                    g.DrawImage(Tile_Sprites, pos.X * tile_size, 70 + pos.Y * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite2 * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                End If
                g.DrawString(Tile_Map(pos.X, pos.Y).Type.ToString, Me.Font, Brushes.Black, pos.X * 32, 70 + pos.Y * 32)
            End If
        End If
    End Sub


    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        If Tile_Sprites Is Nothing Then Exit Sub
        Dim pos As Point
        pos.X = e.X \ tile_size
        pos.Y = (e.Y - 70) \ tile_size
        If e.Y < 70 Then pos.Y = -1
        If e.Button = Windows.Forms.MouseButtons.Left Then
            If pos.X >= 0 AndAlso pos.X <= 31 AndAlso pos.Y >= 0 AndAlso pos.Y <= 31 Then

                Tile_Map(pos.X, pos.Y).X = CByte(pos.X)
                Tile_Map(pos.X, pos.Y).Y = CByte(pos.Y)

                If showSprite2 = False Then
                    Tile_Map(pos.X, pos.Y).Sprite = WriteNumber
                Else
                    Tile_Map(pos.X, pos.Y).Sprite2 = WriteNumber
                End If

                Tile_Map(pos.X, pos.Y).Type = WriteType
                If Walkable < 100 Then Tile_Map(pos.X, pos.Y).Walkable = Walkable

                Tile_active(pos.X, pos.Y) = True

                Dim g As Graphics = Graphics.FromHwnd(Me.Handle)                
                If showSprite2 = False Then
                    g.DrawImage(Tile_Sprites, pos.X * tile_size, 70 + pos.Y * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                Else
                    g.DrawImage(Tile_Sprites, pos.X * tile_size, 70 + pos.Y * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite2 * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                End If

                'g.DrawRectangle(Pens.Red, New Rectangle(pos.X * tile_size, (pos.Y * tile_size) + 70, tile_size - 1, tile_size - 1))

                If Tile_active(pos.X, pos.Y) = True Then
                    If Tile_Map(pos.X, pos.Y).Walkable = 0 Then g.DrawRectangle(Pens.Red, New Rectangle(pos.X * tile_size, (pos.Y * tile_size) + 70, tile_size - 1, tile_size - 1))
                    If Tile_Map(pos.X, pos.Y).Walkable = 3 Then g.DrawRectangle(Pens.Green, New Rectangle(pos.X * tile_size, (pos.Y * tile_size) + 70, tile_size - 1, tile_size - 1))
                    If Tile_Map(pos.X, pos.Y).Walkable = 4 Then g.DrawRectangle(Pens.Blue, New Rectangle(pos.X * tile_size, (pos.Y * tile_size) + 70, tile_size - 1, tile_size - 1))
                End If
                g.DrawString(Tile_Map(pos.X, pos.Y).Type.ToString, Me.Font, Brushes.Black, pos.X * 32, 70 + pos.Y * 32)
                g.Dispose()
            End If
        End If
        If e.Button = Windows.Forms.MouseButtons.Right Then
            If pos.X >= 0 AndAlso pos.X <= 31 AndAlso pos.Y >= 0 AndAlso pos.Y <= 31 Then
                Tile_active(pos.X, pos.Y) = False

                Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
                If showSprite2 = False Then
                    g.DrawImage(Tile_Sprites, pos.X * tile_size, 70 + pos.Y * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                Else
                    g.DrawImage(Tile_Sprites, pos.X * tile_size, 70 + pos.Y * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite2 * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                End If
                g.DrawString(Tile_Map(pos.X, pos.Y).Type.ToString, Me.Font, Brushes.Black, pos.X * 32, 70 + pos.Y * 32)
            End If
        End If
    End Sub

    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        If SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then

            Dim Tiles As HashSet(Of Build_Tiles) = New HashSet(Of Build_Tiles)

            For Each item In Tile_Map
                If item IsNot Nothing Then

                    If Tile_active(item.X, item.Y) = True Then
                        Tiles.Add(item)
                    End If

                End If
            Next
            Save_Building(SaveFileDialog1.FileName, Tiles)
        End If


    End Sub

    Private Sub LoadBitmapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadBitmapToolStripMenuItem.Click
        If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            Tile_Sprites = Bitmap.FromFile(OpenFileDialog1.FileName)
            'PictureBox1.Image = Tile_Sprites
            Me.Refresh()
        End If
    End Sub

    Private Sub Form1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        If Tile_Sprites IsNot Nothing Then
            For x = 0 To 31
                For y = 0 To 31
                    If showSprite2 = False Then
                        e.Graphics.DrawImage(Tile_Sprites, x * tile_size, 70 + y * tile_size, New RectangleF(Tile_Map(x, y).Sprite * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                    Else
                        e.Graphics.DrawImage(Tile_Sprites, x * tile_size, 70 + y * tile_size, New RectangleF(Tile_Map(x, y).Sprite2 * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                    End If


                    If Tile_active(x, y) = True Then
                        If Tile_Map(x, y).Walkable = 0 Then e.Graphics.DrawRectangle(Pens.Red, New Rectangle(x * tile_size, (y * tile_size) + 70, tile_size - 1, tile_size - 1))
                        If Tile_Map(x, y).Walkable = 3 Then e.Graphics.DrawRectangle(Pens.Green, New Rectangle(x * tile_size, (y * tile_size) + 70, tile_size - 1, tile_size - 1))
                        If Tile_Map(x, y).Walkable = 4 Then e.Graphics.DrawRectangle(Pens.Blue, New Rectangle(x * tile_size, (y * tile_size) + 70, tile_size - 1, tile_size - 1))
                    End If
                    e.Graphics.DrawString(Tile_Map(x, y).Type.ToString, Me.Font, Brushes.Black, x * 32, 70 + y * 32)

                Next
            Next
            e.Graphics.DrawImage(Tile_Sprites, 0, 30)
            e.Graphics.DrawRectangle(Pens.Green, New Rectangle(WriteNumber * tile_size, 30, tile_size - 1, tile_size - 1))
        End If        


    End Sub

    Private Sub Label2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label2.Click
        Walkable = 3
        Label2.BackColor = Color.LawnGreen
        Label4.BackColor = Color.Gray
        Label6.BackColor = Color.Gray
    End Sub

    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click
        Walkable = 0
        Label4.BackColor = Color.LawnGreen
        Label2.BackColor = Color.Gray
        Label6.BackColor = Color.Gray
    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click        
        showSprite2 = False
        Label1.BackColor = Color.Gray
        Label5.BackColor = Color.LawnGreen
        Me.Refresh()
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click
        showSprite2 = True
        Label1.BackColor = Color.LawnGreen
        Label5.BackColor = Color.Gray
        Me.Refresh()
    End Sub

    Private Sub Label6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label6.Click
        Walkable = 250
        Label2.BackColor = Color.Gray
        Label4.BackColor = Color.Gray
        Label6.BackColor = Color.LawnGreen
    End Sub
End Class
