<Serializable()> Public MustInherit Class Tile
    Public walkable As walkable_type_enum

    Sub New(Optional ByVal set_walkable As walkable_type_enum = walkable_type_enum.Walkable)
        walkable = set_walkable
    End Sub

    Sub copy(ByVal other As Tile)
        walkable = other.walkable
        'type = other.type
    End Sub

End Class