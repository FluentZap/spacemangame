Public Class Planet
    Public orbits_planet As Boolean
    Public orbit_point As Integer
    Public orbit_distance As Integer
    Public radius As Integer
    Public population As Integer
    Public theta As Double
    Public size As PointI
    Public landed_ships As Dictionary(Of Integer, PointI) = New Dictionary(Of Integer, PointI)

    'trade route
    'available resources
    'regions
    Public tile_map(,) As Planet_tile
    'Dim crew_list As Crew_list

    Public type As planet_type_enum
    'ambient settings


    Public tech As HashSet(Of planet_tech_list_enum)
    Public special_tech As Planet_special_tech_enum

    Public crew_list As Dictionary(Of Integer, Crew) = New Dictionary(Of Integer, Crew)
    Public officer_list As Dictionary(Of Integer, Officer) = New Dictionary(Of Integer, Officer)


    Sub New(ByVal type As planet_type_enum, ByVal size As PointI, ByVal orbit_point As Integer, ByVal orbit_distance As Integer, ByVal orbits_planet As Boolean, ByVal theta_offset As Double)
        Me.type = type
        ReDim Me.tile_map(size.x, size.y)
        Me.size = size
        Me.orbit_point = orbit_point
        Me.orbit_distance = orbit_distance
        Me.orbits_planet = orbits_planet
        Me.theta = theta_offset
    End Sub




    Sub MoveOfficer(ByVal Id As Integer, ByRef vector As PointD)
        Dim Pos(3) As PointD
        vector.x *= Officer_List(Id).speed
        vector.y *= Officer_List(Id).speed

        '0:7,1
        '0:24,1
        '0:7,31
        '0:24,31

        'top left
        Pos(0).x = Math.Floor((Officer_List(Id).GetLocation.x + 8 + vector.x) * 0.03125)
        Pos(0).y = Math.Floor((Officer_List(Id).GetLocation.y + 27 + vector.y) * 0.03125)
        'top right
        Pos(1).x = Math.Floor((Officer_List(Id).GetLocation.x + 23 + vector.x) * 0.03125)
        Pos(1).y = Math.Floor((Officer_List(Id).GetLocation.y + 27 + vector.y) * 0.03125)
        'bottom left
        Pos(2).x = Math.Floor((Officer_List(Id).GetLocation.x + 8 + vector.x) * 0.03125)
        Pos(2).y = Math.Floor((Officer_List(Id).GetLocation.y + 31 + vector.y) * 0.03125)
        'bottom right
        Pos(3).x = Math.Floor((Officer_List(Id).GetLocation.x + 23 + vector.x) * 0.03125)
        Pos(3).y = Math.Floor((Officer_List(Id).GetLocation.y + 31 + vector.y) * 0.03125)

        Dim b As Integer = 0
        For a = 0 To 3
            If Pos(a).x >= 0 And Pos(a).x <= size.x And Pos(a).y >= 0 And Pos(a).y <= size.y Then
                If tile_map(Convert.ToInt32(Pos(a).x), Convert.ToInt32(Pos(a).y)).walkable = walkable_type_enum.Walkable Then
                    b = b + 1
                End If
            End If
        Next
        If b = 4 Then
            Officer_List(Id).Move(vector)
        ElseIf Not b = 0 Then
            If vector.x < 0 Then officer_list(Id).SetLocation(New PointD(Convert.ToInt32(Pos(0).x * 32 + 32 - 8), officer_list(Id).GetLocation.y))
            If vector.x > 0 Then officer_list(Id).SetLocation(New PointD(Convert.ToInt32(Pos(1).x * 32 - 32 + 8), officer_list(Id).GetLocation.y))
            If vector.y < 0 Then officer_list(Id).SetLocation(New PointD(officer_list(Id).GetLocation.x, Convert.ToInt32(Pos(0).y * 32 + 32 - 27)))
            If vector.y > 0 Then officer_list(Id).SetLocation(New PointD(officer_list(Id).GetLocation.x, Convert.ToInt32(Pos(2).y * 32 - 32)))
        End If
    End Sub



    Sub populate()
        If type = planet_type_enum.Forest Then populate_forest()





    End Sub

    Private Sub populate_forest()

        Dim City_Rect As Rectangle
        City_Rect.Width = 160
        City_Rect.Height = 160
        City_Rect.X = random(16, (512 - 16) - 160)
        City_Rect.Y = random(16, (512 - 16) - 160)

        Dim Shipyard_Rect As Rectangle
        Shipyard_Rect.Width = 80
        Shipyard_Rect.Height = 80
        'Fighter/Corvette facility 30x30
        'Frigate facility 40x60
        'Shipyard_Rect.X = random(10, 60) + City_Rect.X
        'Shipyard_Rect.Y = random(10, 60) + City_Rect.Y

        Shipyard_Rect.X = random(0, 1) * 80 + City_Rect.X
        Shipyard_Rect.Y = City_Rect.Y


        'Set base
        For x = 0 To size.x
            For y = 0 To size.y
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.CornerTL, walkable_type_enum.Walkable)
            Next
        Next

        'Set city rect
        For x = City_Rect.X To City_Rect.Right
            For y = City_Rect.Y To City_Rect.Bottom
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.CornerTR, walkable_type_enum.Walkable)
            Next
        Next


        For x = Shipyard_Rect.X To Shipyard_Rect.Right
            For y = Shipyard_Rect.Y To Shipyard_Rect.Bottom
                'Border
                If x >= Shipyard_Rect.X AndAlso x <= Shipyard_Rect.X + 2 Then _
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Shipyard, planet_sprite_enum.WallR, walkable_type_enum.Impassable)
                If x >= Shipyard_Rect.Right - 2 AndAlso x <= Shipyard_Rect.Right Then _
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Shipyard, planet_sprite_enum.WallL, walkable_type_enum.Impassable)
                If y >= Shipyard_Rect.Y AndAlso y <= Shipyard_Rect.Y + 2 Then _
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Shipyard, planet_sprite_enum.WallB, walkable_type_enum.Impassable)
                If y >= Shipyard_Rect.Bottom - 2 AndAlso y <= Shipyard_Rect.Bottom Then _
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Shipyard, planet_sprite_enum.WallT, walkable_type_enum.Impassable)


                'Large Ship Bay
                If x >= Shipyard_Rect.X + 4 AndAlso x <= Shipyard_Rect.X + 44 AndAlso y >= Shipyard_Rect.Y + 4 AndAlso y <= Shipyard_Rect.Y + 64 Then _
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Shipyard, planet_sprite_enum.Floor, walkable_type_enum.Walkable)

                If x >= Shipyard_Rect.X + 3 AndAlso x <= Shipyard_Rect.X + 44 AndAlso y = Shipyard_Rect.Y + 3 Then _
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Shipyard, planet_sprite_enum.CornerTL, walkable_type_enum.Walkable)



                'Fist small ship Bay
                If x >= Shipyard_Rect.X + 47 AndAlso x <= Shipyard_Rect.X + 77 AndAlso y >= Shipyard_Rect.Y + 4 AndAlso y <= Shipyard_Rect.Y + 34 Then _
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Shipyard, planet_sprite_enum.Floor, walkable_type_enum.Walkable)

                'Seccond small ship Bay
                If x >= Shipyard_Rect.X + 47 AndAlso x <= Shipyard_Rect.X + 77 AndAlso y >= Shipyard_Rect.Y + 38 AndAlso y <= Shipyard_Rect.Y + 68 Then _
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Shipyard, planet_sprite_enum.Floor, walkable_type_enum.Walkable)




            Next
        Next
        

    End Sub



    Function GetOfficer() As Dictionary(Of Integer, Officer)
        Return Me.officer_list
    End Function

End Class
