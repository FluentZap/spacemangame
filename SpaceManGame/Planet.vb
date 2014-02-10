Public Class Planet
    Public orbits_planet As Boolean
    Public orbit_point As Integer
    Public orbit_distance As Integer
    Public radius As Integer
    Public population As Integer
    Public theta As Double
    Public size As PointI
    Public landed_ships As Dictionary(Of Integer, PointI) = New Dictionary(Of Integer, PointI)

    Public Block_Map As HashSet(Of PointI) = New HashSet(Of PointI)
    Public Resource_Points As HashSet(Of PointI) = New HashSet(Of PointI)
    Public Building_List As Dictionary(Of Integer, Planet_Building) = New Dictionary(Of Integer, Planet_Building)

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


    Public Projectiles As HashSet(Of Projectile) = New HashSet(Of Projectile)

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


    Public Sub DoEvents()

        Update_Officers()

        Handle_Projectiles()

        Run_animations()

    End Sub


    Sub Run_animations()
        For Each item In officer_list
            item.Value.Update_Sprite()
        Next

        For Each item In crew_list
            item.Value.Update_Sprite()
        Next

    End Sub

    Sub Update_Officers()
        For Each item In Officer_List
            If item.Value.input_flages.walking = True Then
                Select Case item.Value.input_flages.Facing

                    Case Is = Move_Direction.Left
                        If item.Value.input_flages.MoveX = Move_Direction.Left Then MoveOfficer(item.Key, New PointD(-1, 0))
                        If item.Value.input_flages.MoveX = Move_Direction.Right Then MoveOfficer(item.Key, New PointD(0.2, 0))
                        If item.Value.input_flages.MoveY = Move_Direction.Up Then MoveOfficer(item.Key, New PointD(0, -DIAGONAL_SPEED_MODIFIER))
                        If item.Value.input_flages.MoveY = Move_Direction.Down Then MoveOfficer(item.Key, New PointD(0, DIAGONAL_SPEED_MODIFIER))
                    Case Is = Move_Direction.Right
                        If item.Value.input_flages.MoveX = Move_Direction.Left Then MoveOfficer(item.Key, New PointD(-0.2, 0))
                        If item.Value.input_flages.MoveX = Move_Direction.Right Then MoveOfficer(item.Key, New PointD(1, 0))
                        If item.Value.input_flages.MoveY = Move_Direction.Up Then MoveOfficer(item.Key, New PointD(0, -DIAGONAL_SPEED_MODIFIER))
                        If item.Value.input_flages.MoveY = Move_Direction.Down Then MoveOfficer(item.Key, New PointD(0, DIAGONAL_SPEED_MODIFIER))
                    Case Is = Move_Direction.Up
                        If item.Value.input_flages.MoveX = Move_Direction.Left Then MoveOfficer(item.Key, New PointD(-DIAGONAL_SPEED_MODIFIER, 0))
                        If item.Value.input_flages.MoveX = Move_Direction.Right Then MoveOfficer(item.Key, New PointD(DIAGONAL_SPEED_MODIFIER, 0))
                        If item.Value.input_flages.MoveY = Move_Direction.Up Then MoveOfficer(item.Key, New PointD(0, -1))
                        If item.Value.input_flages.MoveY = Move_Direction.Down Then MoveOfficer(item.Key, New PointD(0, 0.2))
                    Case Is = Move_Direction.Down
                        If item.Value.input_flages.MoveX = Move_Direction.Left Then MoveOfficer(item.Key, New PointD(-DIAGONAL_SPEED_MODIFIER, 0))
                        If item.Value.input_flages.MoveX = Move_Direction.Right Then MoveOfficer(item.Key, New PointD(DIAGONAL_SPEED_MODIFIER, 0))
                        If item.Value.input_flages.MoveY = Move_Direction.Up Then MoveOfficer(item.Key, New PointD(0, -0.2))
                        If item.Value.input_flages.MoveY = Move_Direction.Down Then MoveOfficer(item.Key, New PointD(0, 1))

                End Select
            End If


        Next
    End Sub


    Sub Handle_Projectiles()
        Dim remove_List As List(Of Projectile) = New List(Of Projectile)
        For Each item In Me.Projectiles
            item.Location.x += item.vector_velocity.x
            item.Location.y += item.vector_velocity.y
            item.Life -= 1
            If item.Life <= 0 Then
                If item.Second_Stage_Life <= 0 Then
                    remove_List.Add(item)
                Else
                    item.Life = item.Second_Stage_Life
                    item.Second_Stage_Life = 0
                    Dim vec As PointD

                    vec.x = Math.Cos(item.Rotation - 1.57079633)
                    vec.y = Math.Sin(item.Rotation - 1.57079633)

                    item.vector_velocity.x += vec.x * 20
                    item.vector_velocity.y += vec.y * 20
                End If
            End If


            Dim tile As PointI = New PointI(item.Location.intX \ 32, item.Location.intY \ 32)
            If tile.x >= 0 AndAlso tile.x <= size.x AndAlso tile.y >= 0 AndAlso tile.y <= size.y Then
                If (Not tile_map(tile.x, tile.y).walkable = walkable_type_enum.Walkable) AndAlso (Not tile_map(tile.x, tile.y).walkable = walkable_type_enum.OpenDoor) Then
                    Detonate_Local_Projectile(item)
                    remove_List.Add(item)
                End If
            End If

        Next

        For Each item In remove_List
            Me.Projectiles.Remove(item)
        Next

    End Sub


    Sub Detonate_Local_Projectile(ByVal Pro As Projectile)
        Dim contact_Point As PointI = New PointI(Pro.Location.intX \ 32, Pro.Location.intY \ 32)
        'tile_map(contact_Point.x, contact_Point.y).color = Color.Red

        For y = contact_Point.y - 2 To contact_Point.y + 2
            For x = contact_Point.x - 2 To contact_Point.x + 2
                If x >= 0 AndAlso x <= size.x AndAlso y >= 0 AndAlso y <= size.y Then
                    'tile_map(x, y).color = Color.Red

                    For Each Crew In crew_list
                        If Crew.Value.find_tile = New PointI(x, y) Then
                            Crew.Value.Health.Damage_All_Limbs(3)
                        End If

                    Next

                End If
            Next
        Next

    End Sub


    Private Sub populate_forest()
        'Set base
        For x = 0 To size.x
            For y = 0 To size.y
                Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Grass, walkable_type_enum.Walkable)
            Next
        Next

        For a = 1 To random(2, 4)
            Resource_Points.Add(New PointI(random(1, 14), random(1, 14)))
        Next


        Create_City()



    End Sub


    Sub Draw_Street(ByVal Pos As PointI, ByVal Size As PointI)

        For x = Pos.x To Pos.x + Size.x
            For y = Pos.y To Pos.y + 1
                If x >= 0 AndAlso x <= Me.size.x AndAlso y >= 0 AndAlso y <= Me.size.y Then
                    Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next

        For x = Pos.x To Pos.x + Size.x
            For y = Pos.y + Size.y To Pos.y + Size.y - 1 Step -1
                If x >= 0 AndAlso x <= Me.size.x AndAlso y >= 0 AndAlso y <= Me.size.y Then
                    Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next

        For x = Pos.x To Pos.x + 1
            For y = Pos.y To Pos.y + Size.y
                If x >= 0 AndAlso x <= Me.size.x AndAlso y >= 0 AndAlso y <= Me.size.y Then
                    Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next

        For x = Pos.x + Size.x To Pos.x + Size.x - 1 Step -1
            For y = Pos.y To Pos.y + Size.y
                If x >= 0 AndAlso x <= Me.size.x AndAlso y >= 0 AndAlso y <= Me.size.y Then
                    Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next
    End Sub


    Enum Block_Type_enum
        Big
        Quad
        DoubleH
        DoubleV
        Three1
        Three2
        Three3
        Three4
    End Enum

    Enum building_size_enum
        Big
        Small
        WideH
        WideV
    End Enum


    


    Sub Create_City()
        population = random(0, 256)
        Dim Capital As PointI
        Dim ShipyardCount As Integer = random(0, 1)
        Dim LargeShipyardCount As Integer = -1

        Capital = New PointI(random(2, 12), random(2, 12))
        'Capital = New PointI(2, 2)
        If population >= 4 Then ShipyardCount += 1
        If population >= 8 Then ShipyardCount += 1 : LargeShipyardCount += 1
        If population >= 12 Then ShipyardCount += 1 : LargeShipyardCount += 1



        Draw_Street(New PointI(Capital.x * 32, Capital.y * 32), New PointI(32, 32))
        Block_Map.Add(Capital)

        For Each item In Resource_Points
            For x = item.x * 32 To item.x * 32 + 32
                For y = item.y * 32 To item.y * 32 + 32
                    'If random(0, 1) = 1 Then
                    Me.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Water, walkable_type_enum.Impassable)
                    'End If
                Next
            Next
        Next

        'Build_Item(planet_sprite_enum.Tree1, New PointI(2, 3), New PointI(0, 0))

        Build_Blocks(Capital, 9)

        For a = 0 To 10
            Dim blocks(Block_Map.Count - 1) As PointI
            Block_Map.CopyTo(blocks, 0)

            For Each item In blocks
                Build_Blocks(item, 9)
            Next
        Next

        Draw_Street(New PointI(0, 0), New PointI(16, 16))
        Build_Building(New PointI(0, 0), building_type_enum.House)

        'Fighter/Corvette facility 30x30
        'Smallest Block size 32,32
        'Frigate facility 40x60
        'Shipyard_Rect.X = random(10, 60) + City_Rect.X
        'Shipyard_Rect.Y = random(10, 60) + City_Rect.Y







        'Set city rect

    End Sub


    Sub Build_Blocks(ByVal Pos As PointI, ByVal BuildMax As Integer)
        Dim Built As Integer
        Dim block_Type As Block_Type_enum
        For x = Pos.x - 1 To Pos.x + 1
            For y = Pos.y - 1 To Pos.y + 1
                If x >= 0 AndAlso x < size.x \ 32 AndAlso y >= 0 AndAlso y < size.y \ 32 Then
                    If Not Block_Map.Contains(New PointI(x, y)) AndAlso Not Resource_Points.Contains(New PointI(x, y)) Then
                        block_Type = CType(random(0, 7), Block_Type_enum)

                        Select Case block_Type
                            Case Is = Block_Type_enum.Big
                                Draw_Street(New PointI(x * 32, y * 32), New PointI(32, 32))
                            Case Is = Block_Type_enum.DoubleH
                                Draw_Street(New PointI(x * 32, y * 32), New PointI(32, 16))
                                Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(32, 16))
                            Case Is = Block_Type_enum.DoubleV
                                Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 32))
                                Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 32))
                            Case Is = Block_Type_enum.Quad
                                Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 16))
                                Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 16))
                                Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(16, 16))
                                Draw_Street(New PointI(x * 32 + 16, y * 32 + 16), New PointI(16, 16))

                                Build_Building(New PointI(x * 32, y * 32), building_type_enum.House)
                                Build_Building(New PointI(x * 32 + 16, y * 32), building_type_enum.House)
                                Build_Building(New PointI(x * 32, y * 32 + 16), building_type_enum.House)
                                Build_Building(New PointI(x * 32 + 16, y * 32 + 16), building_type_enum.House)

                            Case Is = Block_Type_enum.Three1
                                Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 32))
                                Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 16))
                                Draw_Street(New PointI(x * 32 + 16, y * 32 + 16), New PointI(16, 16))

                                Build_Building(New PointI(x * 32 + 16, y * 32), building_type_enum.House)
                                Build_Building(New PointI(x * 32 + 16, y * 32 + 16), building_type_enum.House)
                            Case Is = Block_Type_enum.Three2
                                Draw_Street(New PointI(x * 32, y * 32), New PointI(32, 16))
                                Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(16, 16))
                                Draw_Street(New PointI(x * 32 + 16, y * 32 + 16), New PointI(16, 16))

                                Build_Building(New PointI(x * 32, y * 32 + 16), building_type_enum.House)
                                Build_Building(New PointI(x * 32 + 16, y * 32 + 16), building_type_enum.House)
                            Case Is = Block_Type_enum.Three3
                                Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 32))
                                Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 16))
                                Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(16, 16))

                                Build_Building(New PointI(x * 32, y * 32), building_type_enum.House)
                                Build_Building(New PointI(x * 32, y * 32 + 16), building_type_enum.House)
                            Case Is = Block_Type_enum.Three4
                                Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(32, 16))
                                Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 16))
                                Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 16))

                                Build_Building(New PointI(x * 32, y * 32), building_type_enum.House)
                                Build_Building(New PointI(x * 32 + 16, y * 32), building_type_enum.House)
                        End Select


                        Block_Map.Add(New PointI(x, y))
                        Built += 1
                        If Built >= BuildMax Then Exit Sub
                    End If
                End If
            Next
        Next

    End Sub



    Sub Build_Building(ByVal Pos As PointI, ByVal Type As building_type_enum)
        Dim ID As Integer
        For a = 0 To 100000
            If Not Building_List.ContainsKey(a) Then ID = a : Exit For
        Next


        If Type = building_type_enum.House Then
            'external
            For y = 2 To 14
                For x = 2 To 14
                    Me.tile_map(Pos.x + x, Pos.y + y).type = planet_tile_type_enum.House
                    Me.tile_map(Pos.x + x, Pos.y + y).sprite = 4
                    Me.tile_map(Pos.x + x, Pos.y + y).walkable = walkable_type_enum.Impassable
                Next
            Next

            Me.tile_map(Pos.x + 2, Pos.y + 2).sprite = 5
            Me.tile_map(Pos.x + 14, Pos.y + 2).sprite = 6

            For x = 3 To 13
                Me.tile_map(Pos.x + x, Pos.y + 2).sprite = 7
            Next

            For x = 2 To 14
                Me.tile_map(Pos.x + x, Pos.y + 12).sprite = 1
            Next

            For x = 2 To 14
                Me.tile_map(Pos.x + x, Pos.y + 13).sprite = 0
            Next

            For x = 2 To 14
                Me.tile_map(Pos.x + x, Pos.y + 14).sprite = 0
            Next

            For y = 3 To 11
                Me.tile_map(Pos.x + 2, Pos.y + y).sprite = 8
            Next

            For y = 3 To 11
                Me.tile_map(Pos.x + 14, Pos.y + y).sprite = 9
            Next

            Me.tile_map(Pos.x + 8, Pos.y + 13).sprite = 3
            Me.tile_map(Pos.x + 8, Pos.y + 14).sprite = 2
            For y = Pos.y + 3 To Pos.y + 13
                For x = Pos.x + 3 To Pos.x + 13
                    Me.tile_map(x, y).walkable = walkable_type_enum.Walkable
                Next
            Next
            Me.tile_map(Pos.x + 8, Pos.y + 14).walkable = walkable_type_enum.Walkable
            'internal
            For y = 2 To 13
                For x = 2 To 13
                    Me.tile_map(Pos.x + x, Pos.y + y).sprite2 = 4
                Next
            Next

            Me.tile_map(Pos.x + 2, Pos.y + 2).sprite2 = 10
            Me.tile_map(Pos.x + 14, Pos.y + 2).sprite2 = 11

            Me.tile_map(Pos.x + 2, Pos.y + 14).sprite2 = 12
            Me.tile_map(Pos.x + 14, Pos.y + 14).sprite2 = 13

            For x = 3 To 13
                Me.tile_map(Pos.x + x, Pos.y + 2).sprite2 = 14
            Next

            For x = 3 To 13
                Me.tile_map(Pos.x + x, Pos.y + 14).sprite2 = 14
            Next

            For y = 3 To 13
                Me.tile_map(Pos.x + 2, Pos.y + y).sprite2 = 15
            Next

            For y = 3 To 13
                Me.tile_map(Pos.x + 14, Pos.y + y).sprite2 = 16
            Next

            Me.tile_map(Pos.x + 8, Pos.y + 14).sprite2 = 17


            Building_List.Add(ID, New Planet_Building(0, New Rectangle(Pos.x + 2, Pos.y + 2, 13, 13), building_type_enum.House))
        End If

        



    End Sub


    Sub Build_Item(ByVal StartIndex As Integer, ByVal Size As PointI, ByVal Position As PointI)
        For y = 0 To Size.y
            For x = 0 To Size.x
                Me.tile_map(x + Position.x, y + Position.y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, CType(StartIndex + x + y * (Size.x + 1), planet_sprite_enum), walkable_type_enum.Impassable)
            Next
        Next

    End Sub


    Function GetOfficer() As Dictionary(Of Integer, Officer)
        Return Me.officer_list
    End Function










End Class