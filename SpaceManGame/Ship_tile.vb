<Serializable()> Public Class Ship_tile
    Inherits Tile
    Public type As tile_type_enum
    Public temperature As Byte
    Public integrity As Byte
    Public integrityMax As Byte
    Public sprite As room_sprite_enum
    Public roomID As Byte
    Public access_point As tile_accesspoint_enum
    Public device_tile As Device_tile

    Public explored As Boolean
    Public viewed As tile_view_level

    Public light_level As Byte = 255
    Public color As Color = Drawing.Color.White


    Function adj_color() As Color

        Return Drawing.Color.FromArgb(Convert.ToInt32(Me.color.A / 255 * light_level), Convert.ToInt32(Me.color.R / 255 * light_level), Convert.ToInt32(Me.color.G / 255 * light_level), Convert.ToInt32(Me.color.B / 255 * light_level))
    End Function


    Sub New(ByVal roomId As Byte, ByVal type As tile_type_enum, ByVal temperature As Byte, ByVal integrity As Byte, ByVal sprite As room_sprite_enum, Optional ByVal set_walkable As walkable_type_enum = walkable_type_enum.Walkable)
        MyBase.New(set_walkable)
        Me.roomID = roomId
        Me.type = type
        Me.temperature = temperature
        Me.integrity = integrity
        Me.sprite = sprite
        Me.walkable = walkable
    End Sub

    Public Overloads Sub copy(ByVal other As Ship_tile)
        temperature = other.temperature
        integrity = other.integrity
        integrityMax = other.integrityMax
        sprite = other.sprite
        'type = tile_type_enum.other
        roomID = other.roomID
        access_point = other.access_point
        device_tile = other.device_tile
        light_level = other.light_level
    End Sub

    Function Has_Device_Tile() As Boolean
        If device_tile IsNot Nothing Then Return True
        Return False
    End Function

End Class



<Serializable()> Public Class Device_tile    
    Public sprite As Integer
    Public rotate As rotate_enum
    Public flip As flip_enum
    Public spriteAni As Integer
    Public type As device_tile_type_enum
    Public IDhash As New HashSet(Of Integer)()  'Used For Device Base

    Sub New(ByVal device_ID As Integer, ByVal sprite As Integer, ByVal rotate As rotate_enum, ByVal flip As flip_enum, ByVal spriteAni As Integer, ByVal SpriteTileSet As device_tile_type_enum)        
        Me.sprite = sprite
        Me.rotate = rotate
        Me.flip = flip
        Me.spriteAni = spriteAni
        Me.type = SpriteTileSet
        Me.IDhash.Add(device_ID)
    End Sub


    Function ContainsID(ByVal ID As Integer) As Boolean
        If device_ID = ID Then Return True
        If IDhash IsNot Nothing AndAlso IDhash.Contains(ID) Then Return True
        Return False
    End Function

    Function device_ID() As Integer
        If IDhash.Count > 0 Then
            Return IDhash.First
        Else
            Return -1
        End If
    End Function

End Class