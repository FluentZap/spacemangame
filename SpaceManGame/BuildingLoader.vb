Public Module BuildingLoader

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





    Function Load_Building(ByVal Building As String) As HashSet(Of Build_Tiles)

        Dim Tile_List As HashSet(Of Build_Tiles) = New HashSet(Of Build_Tiles)


        Dim stream As New IO.FileStream(Application.StartupPath + "/data/BuildingMap/" + Building, IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)

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
        Return Tile_List


    End Function














End Module
