Public Class Planet_tile
    Inherits Tile
    Public type As planet_tile_type_enum
    Public sprite As Byte ' planet_sprite_enum
    Public sprite2 As Byte ' planet_sprite_enum

    Sub New(ByVal Type As planet_tile_type_enum, ByVal Sprite As Byte, ByVal Walkable As walkable_type_enum, Optional ByVal Sprite2 As Byte = 0)
        Me.type = Type
        Me.sprite = Sprite
        Me.sprite2 = Sprite2
        Me.walkable = Walkable
    End Sub

End Class