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


    Public Tile_Map(127, 127) As Build_Tiles
    Public Tile_active(127, 127) As Boolean

    Public Tile_Boxes(127, 127) As Integer

    Public Building_Type As Byte
    Public WriteNumber As Integer
    Public WriteType As Byte
    Public Walkable As Byte = 3
    Public tile_size As Integer = 32
    Public showSprite2 As Boolean    
    Public Tile_Sprites As Image()

    Private Sub Form1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Right AndAlso WriteNumber < 200 Then WriteNumber = CByte(WriteNumber + 1)
        If e.KeyCode = Keys.Left AndAlso WriteNumber > 0 Then WriteNumber = CByte(WriteNumber - 1)

        If Tile_Sprites Is Nothing Then Exit Sub
        Dim g As Graphics = Graphics.FromHwnd(Me.Handle)

        g.DrawImage(Tile_Sprites(WriteType), 0, 30)
        g.DrawRectangle(Pens.Green, New Rectangle(WriteNumber * tile_size, 30, tile_size - 1, tile_size - 1))

    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        For x = 0 To 127
            For y = 0 To 127
                Tile_Map(x, y) = New Build_Tiles(0, 0, 0, 0, 0, 0)
            Next
        Next        
        For x = 0 To 255
            TileBox.Items.Add(CType(x, planet_tile_type_enum))
        Next
        Load_Images()
        WalkableBox.SelectedIndex = 0
        TileBox.SelectedIndex = 0
        SpriteBox.SelectedIndex = 0
    End Sub

    Private Sub NewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewToolStripMenuItem.Click
        For x = 0 To 127
            For y = 0 To 127
                Tile_Map(x, y) = New Build_Tiles(0, 0, 0, 0, 0, 0)
                Tile_active(x, y) = False
            Next
        Next
        Reset_Selections()
        Me.Refresh()
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenToolStripMenuItem.Click

        OpenFileDialog1.InitialDirectory = "C:\Users\Toad\Documents\Visual Studio 2008\Projects\SpaceManGame\SpaceManGame\bin\x86\Debug\Data\BuildingMap\"

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

            Reset_Selections()
            
            For x = 0 To 127
                For y = 0 To 127
                    'Tile_Map(x, y) = New Build_Tiles(0, 0, 0, 0, 0, 0)
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

    Sub Reset_Selections()
        WalkableBox.SelectedIndex = 0
        SpriteBox.SelectedIndex = 0
        TileBox.SelectedIndex = 0
        WriteNumber = 0
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
        SetTile(e)
    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        SetTile(e)
    End Sub


    Sub SetTile(ByVal e As MouseEventArgs)
        Dim g As Graphics = Graphics.FromHwnd(Me.Handle)
        Dim pos As Point
        pos.X = (e.X - 20) \ tile_size + ScrollH.Value
        pos.Y = (e.Y - 70) \ tile_size + ScrollV.Value
        Label7.Text = (pos.X).ToString + " / " + (pos.Y).ToString
        'Draw Mouse pos

        If e.Y < 70 Then pos.Y = -1

        If e.Button = Windows.Forms.MouseButtons.Left Then
            If pos.X - ScrollH.Value >= 0 AndAlso pos.X - ScrollH.Value <= 40 AndAlso e.Y >= 30 AndAlso e.Y <= 30 + tile_size Then
                WriteNumber = CByte(pos.X - ScrollH.Value)
                g.DrawImage(Tile_Sprites(WriteType), 20, 30)
                g.DrawRectangle(Pens.White, New Rectangle(WriteNumber * tile_size + 20, 30, tile_size - 1, tile_size - 1))
            End If
        End If

        If e.Button = Windows.Forms.MouseButtons.Left Then
            If pos.X >= ScrollH.Value AndAlso pos.X <= ScrollH.Value + 36 AndAlso pos.Y >= ScrollV.Value AndAlso pos.Y <= ScrollV.Value + 26 Then


                If WriteNumber > -1 Then

                    Tile_Map(pos.X, pos.Y).X = CByte(pos.X)
                    Tile_Map(pos.X, pos.Y).Y = CByte(pos.Y)

                    If showSprite2 = False Then
                        Tile_Map(pos.X, pos.Y).Sprite = CByte(WriteNumber)
                    Else
                        Tile_Map(pos.X, pos.Y).Sprite2 = CByte(WriteNumber)
                    End If
                    Tile_Map(pos.X, pos.Y).Type = WriteType
                End If


                If Walkable < 100 Then Tile_Map(pos.X, pos.Y).Walkable = Walkable
                Tile_active(pos.X, pos.Y) = True


                If showSprite2 = False Then
                    g.DrawImage(Tile_Sprites(Tile_Map(pos.X, pos.Y).Type), 20 + (pos.X - ScrollH.Value) * tile_size, 70 + (pos.Y - ScrollV.Value) * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                Else
                    g.DrawImage(Tile_Sprites(Tile_Map(pos.X, pos.Y).Type), 20 + (pos.X - ScrollH.Value) * tile_size, 70 + (pos.Y - ScrollV.Value) * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite2 * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                End If

                If (pos.X + 1) / 16 = 0 Then g.DrawRectangle(Pens.LightBlue, New Rectangle((pos.X - ScrollH.Value) * tile_size + 20, (pos.Y * tile_size) + 70, tile_size - 1, tile_size - 1))
                If (pos.Y + 1) / 16 = 0 Then g.DrawRectangle(Pens.LightBlue, New Rectangle((pos.X - ScrollH.Value) * tile_size + 20, (pos.Y * tile_size) + 70, tile_size - 1, tile_size - 1))

                If Tile_active(pos.X, pos.Y) = True Then
                    If Tile_Map(pos.X, pos.Y).Walkable = 0 Then g.DrawRectangle(Pens.Red, New Rectangle((pos.X - ScrollH.Value) * tile_size + 20, ((pos.Y - ScrollV.Value) * tile_size) + 70, tile_size - 1, tile_size - 1))
                    If Tile_Map(pos.X, pos.Y).Walkable = 3 Then g.DrawRectangle(Pens.Green, New Rectangle((pos.X - ScrollH.Value) * tile_size + 20, ((pos.Y - ScrollV.Value) * tile_size) + 70, tile_size - 1, tile_size - 1))
                    If Tile_Map(pos.X, pos.Y).Walkable = 4 Then g.DrawRectangle(Pens.Blue, New Rectangle((pos.X - ScrollH.Value) * tile_size + 20, ((pos.Y - ScrollV.Value) * tile_size) + 70, tile_size - 1, tile_size - 1))
                End If
                g.DrawString(Tile_Map(pos.X, pos.Y).Type.ToString, Me.Font, Brushes.Black, 20 + (pos.X - ScrollH.Value) * 32, 70 + (pos.Y - ScrollV.Value) * 32)
            End If

        End If

        If e.Button = Windows.Forms.MouseButtons.Right Then
            If pos.X >= ScrollH.Value AndAlso pos.X <= ScrollH.Value + 36 AndAlso pos.Y >= ScrollV.Value AndAlso pos.Y <= ScrollV.Value + 26 Then
                Tile_Map(pos.X, pos.Y).Sprite = 0
                Tile_Map(pos.X, pos.Y).Sprite2 = 0

                Tile_active(pos.X, pos.Y) = False
                If showSprite2 = False Then
                    g.DrawImage(Tile_Sprites(Tile_Map(pos.X, pos.Y).Type), 20 + (pos.X - ScrollH.Value) * tile_size, 70 + (pos.Y - ScrollV.Value) * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                Else
                    g.DrawImage(Tile_Sprites(Tile_Map(pos.X, pos.Y).Type), 20 + (pos.X - ScrollH.Value) * tile_size, 70 + (pos.Y - ScrollV.Value) * tile_size, New RectangleF(Tile_Map(pos.X, pos.Y).Sprite2 * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                End If

                g.DrawString(Tile_Map(pos.X, pos.Y).Type.ToString, Me.Font, Brushes.Black, 20 + (pos.X - ScrollH.Value) * 32, 70 + (pos.Y - ScrollV.Value) * 32)

            End If
        End If
        g.Dispose()

    End Sub


    Sub Load_Images()
        Dim Base As String = "C:\Users\Toad\Documents\Visual Studio 2008\Projects\SpaceManGame\SpaceManGame\bin\x86\Debug\Data\Textures\Tiles\"
        ReDim Tile_Sprites(3)
        Tile_Sprites(0) = Bitmap.FromFile(Base + "Planet1.png")
        Tile_Sprites(1) = Bitmap.FromFile(Base + "Desert_Planet.png")
        Tile_Sprites(2) = Bitmap.FromFile(Base + "Desert_Mine.png")
        Tile_Sprites(3) = Bitmap.FromFile(Base + "Desert_Capitol.png")


    End Sub




    Private Sub SaveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveToolStripMenuItem.Click
        SaveFileDialog1.InitialDirectory = "C:\Users\Toad\Documents\Visual Studio 2008\Projects\SpaceManGame\SpaceManGame\bin\x86\Debug\Data\BuildingMap\"
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

    Private Sub LoadBitmapToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'OpenFileDialog1.InitialDirectory = "C:\Users\Toad\Documents\Visual Studio 2008\Projects\SpaceManGame\SpaceManGame\bin\x86\Debug\Data\Textures\Tiles\"
        'If OpenFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
        'Tile_Sprites = Bitmap.FromFile(OpenFileDialog1.FileName)
        'PictureBox1.Image = Tile_Sprites
        'Me.Refresh()
        'End If
        Load_Images()
    End Sub

    Private Sub Form1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Me.Paint
        If Tile_Sprites IsNot Nothing Then
            Dim Tile As Point
            For x = 0 To 36
                For y = 0 To 26
                    Tile.X = x + ScrollH.Value
                    Tile.Y = y + ScrollV.Value
                    If showSprite2 = False Then
                        e.Graphics.DrawImage(Tile_Sprites(Tile_Map(Tile.X, Tile.Y).Type), 20 + x * tile_size, 70 + y * tile_size, New RectangleF(Tile_Map(Tile.X, Tile.Y).Sprite * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                    Else
                        e.Graphics.DrawImage(Tile_Sprites(Tile_Map(Tile.X, Tile.Y).Type), 20 + x * tile_size, 70 + y * tile_size, New RectangleF(Tile_Map(Tile.X, Tile.Y).Sprite2 * 32, 0, tile_size, tile_size), GraphicsUnit.Pixel)
                    End If

                    If (Tile.X + 1) Mod 16 = 0 Then e.Graphics.DrawRectangle(Pens.Blue, New Rectangle(x * tile_size + 20, (y * tile_size) + 70, tile_size - 1, tile_size - 1))
                    If (Tile.Y + 1) Mod 16 = 0 Then e.Graphics.DrawRectangle(Pens.Blue, New Rectangle(x * tile_size + 20, (y * tile_size) + 70, tile_size - 1, tile_size - 1))

                    If Tile_active(Tile.X, Tile.Y) = True Then
                        If Tile_Map(Tile.X, Tile.Y).Walkable = 0 Then e.Graphics.DrawRectangle(Pens.Red, New Rectangle(x * tile_size + 20, (y * tile_size) + 70, tile_size - 1, tile_size - 1))
                        If Tile_Map(Tile.X, Tile.Y).Walkable = 3 Then e.Graphics.DrawRectangle(Pens.Green, New Rectangle(x * tile_size + 20, (y * tile_size) + 70, tile_size - 1, tile_size - 1))
                        If Tile_Map(Tile.X, Tile.Y).Walkable = 4 Then e.Graphics.DrawRectangle(Pens.Blue, New Rectangle(x * tile_size + 20, (y * tile_size) + 70, tile_size - 1, tile_size - 1))
                    End If

                    e.Graphics.DrawString(Tile_Map(Tile.X, Tile.Y).Type.ToString, Me.Font, Brushes.Black, 20 + x * 32, 70 + y * 32)

                Next
            Next
            e.Graphics.DrawImage(Tile_Sprites(WriteType), 20, 30)
            e.Graphics.DrawRectangle(Pens.Green, New Rectangle(WriteNumber * tile_size + 20, 30, tile_size - 1, tile_size - 1))
        End If        


    End Sub

    Private Sub WalkableBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WalkableBox.SelectedIndexChanged
        If WalkableBox.SelectedIndex = 0 Then Walkable = 3
        If WalkableBox.SelectedIndex = 1 Then Walkable = 0
        If WalkableBox.SelectedIndex = 2 Then Walkable = 250
    End Sub

    Private Sub SpriteBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SpriteBox.SelectedIndexChanged
        If SpriteBox.SelectedIndex = 0 Then showSprite2 = False : Me.Refresh()
        If SpriteBox.SelectedIndex = 1 Then showSprite2 = True : Me.Refresh()
        If SpriteBox.SelectedIndex = 2 Then showSprite2 = False : WriteNumber = -1 : Me.Refresh()
    End Sub

    Private Sub TileBox_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TileBox.SelectedIndexChanged
        WriteType = CByte(TileBox.SelectedIndex)
        If WriteType > Tile_Sprites.Length - 1 Then WriteType = 0
        Me.Refresh()
    End Sub

    Private Sub ScrollV_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles ScrollV.Scroll
        Me.Refresh()
    End Sub

    Private Sub ScrollH_Scroll(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles ScrollH.Scroll
        Me.Refresh()
    End Sub

    Private Sub SetAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetAll.Click
        For x = 0 To 127
            For y = 0 To 127
                Tile_Map(x, y) = New Build_Tiles(0, 0, WriteType, CByte(WriteNumber), 0, 0)
            Next
        Next
        Me.Refresh()
    End Sub

    Private Sub MenuStrip1_ItemClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolStripItemClickedEventArgs) Handles MenuStrip1.ItemClicked

    End Sub
End Class

Public Enum planet_tile_type_enum As Byte
    Forest_Planet = 0
    Desert_Planet = 1
    Desert_Mine = 2
    Desert_Capitol = 3
    Road
    Shipyard
    House
    House_Inside

    BuildingBuildOverlay
    empty = 255
End Enum