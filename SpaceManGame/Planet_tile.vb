Public Class Planet_tile
    Inherits Tile
    Public type As planet_tile_type_enum
    Public sprite As planet_sprite_enum

    Sub New(ByVal Type As planet_tile_type_enum, ByVal Sprite As planet_sprite_enum, ByVal Walkable As walkable_type_enum)
        Me.type = Type
        Me.sprite = Sprite
        Me.walkable = Walkable
    End Sub

End Class