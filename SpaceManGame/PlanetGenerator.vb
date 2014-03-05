Module PlanetGenerator
    Private P As Planet


    Public Sub populate_planet(ByVal current_Planet As Planet)
        P = current_Planet
        If P.type = planet_type_enum.Desert Then populate_desert()

    End Sub


    Sub populate_desert()

        'Set base
        For x = 0 To P.size.x
            For y = 0 To P.size.y
                If random(0, 20) = 20 Then
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Desert_Planet, desert_planet_sprite_enum.Pond, walkable_type_enum.Walkable)
                Else
                    'if random(0, 1) = 1 Then
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Desert_Planet, desert_planet_sprite_enum.Rough, walkable_type_enum.Walkable)
                    'Else
                    'P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Desert_Planet, planet_sprite_enum.Tree1, walkable_type_enum.Walkable)
                    'End If

                End If

            Next
        Next
        'Add resource Points
        For a = 1 To random(2, 4)
            Dim po As New PointI(random(1, 14), random(1, 14))
            If Not P.Resource_Points.ContainsKey(po) Then
                P.Resource_Points.Add(po, False)
            End If
        Next

        Create_Resource_Points()

        'P.Resource_Points(New PointI(0, 0)) = True
        Build_Mine(New PointI(0, 0))

        Build_AppartmentH(New PointI(32, 0))
        Build_AppartmentH(New PointI(32, 16))
        Build_AppartmentH(New PointI(32, 32))
        'Build_Mine(New PointI(P.Resource_Points.First.Key.x * 32, P.Resource_Points.First.Key.y * 32))
        Build_PubH(New PointI(0, 32))

        'Create_City()
        For x = 0 To 3
            Add_Crew(x, New Crew(0, New PointD(0, 0), 0, Officer_location_enum.Planet, 10, character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New crew_resource_type(0, 0)))
            P.crew_list(x).Worker_Type = Worker_Type_Enum.Worker
            P.crew_list(x).HomeBuilding = 1
            P.crew_list(x).HomeSpace = 0
            P.crew_list(x).WorkBuilding = 0
            P.crew_list(x).PubBuilding = 4
            P.crew_list(x).WorkShift = Work_Shift_Enum.Morning
        Next

    End Sub


    Sub Build_Mine(ByVal pos As PointI)
        Dim Building_tiles As HashSet(Of Build_Tiles)

        Building_tiles = Load_Building("Desert_Mine.bld")

        For Each t In Building_tiles
            P.tile_map(t.X + pos.x, t.Y + pos.y).sprite = t.Sprite
            P.tile_map(t.X + pos.x, t.Y + pos.y).sprite2 = t.Sprite2
            P.tile_map(t.X + pos.x, t.Y + pos.y).type = CType(t.Type, planet_tile_type_enum)
            P.tile_map(t.X + pos.x, t.Y + pos.y).walkable = CType(t.Walkable, walkable_type_enum)
        Next



        Dim ID As Integer
        For a = 0 To 100000
            If Not P.Building_List.ContainsKey(a) Then ID = a : Exit For
        Next

        P.Building_List.Add(ID, New Planet_Building(0, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Mine))
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 7, pos.y + 18, 10, 6))

        P.Building_List(ID).access_point.Add(New PointI(pos.x + 3, pos.y + 9), New Building_Access_Point_Type)
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 8, pos.y + 9), New Building_Access_Point_Type)
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 13, pos.y + 9), New Building_Access_Point_Type)
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 18, pos.y + 9), New Building_Access_Point_Type)

        For y = 12 To 23
            P.Item_Point.Add(New PointI(pos.x + 22, pos.y + y), New Item_Point_Type(ID))
            P.Item_Point.Add(New PointI(pos.x + 23, pos.y + y), New Item_Point_Type(ID))
            P.Item_Point.Add(New PointI(pos.x + 24, pos.y + y), New Item_Point_Type(ID))
            P.Item_Point.Add(New PointI(pos.x + 25, pos.y + y), New Item_Point_Type(ID))
            P.Item_Point.Add(New PointI(pos.x + 26, pos.y + y), New Item_Point_Type(ID))
            P.Item_Point.Add(New PointI(pos.x + 27, pos.y + y), New Item_Point_Type(ID))
        Next
    End Sub


    Sub Build_AppartmentH(ByVal pos As PointI)
        Dim Building_tiles As HashSet(Of Build_Tiles)

        Building_tiles = Load_Building("Desert_AppartmentH.bld")

        For Each t In Building_tiles
            P.tile_map(t.X + pos.x, t.Y + pos.y).sprite = t.Sprite
            P.tile_map(t.X + pos.x, t.Y + pos.y).sprite2 = t.Sprite2
            P.tile_map(t.X + pos.x, t.Y + pos.y).type = CType(t.Type, planet_tile_type_enum)
            P.tile_map(t.X + pos.x, t.Y + pos.y).walkable = CType(t.Walkable, walkable_type_enum)
        Next



        Dim ID As Integer
        For a = 0 To 100000
            If Not P.Building_List.ContainsKey(a) Then ID = a : Exit For
        Next

        P.Building_List.Add(ID, New Planet_Building(0, New Rectangle(pos.x, pos.y, 32, 16), building_type_enum.Apartment))
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 3, pos.y + 3, 7, 10))
        P.Building_List(ID).BuildingRect.Add(1, New Rectangle(pos.x + 9, pos.y + 3, 7, 10))
        P.Building_List(ID).BuildingRect.Add(2, New Rectangle(pos.x + 16, pos.y + 3, 7, 10))
        P.Building_List(ID).BuildingRect.Add(3, New Rectangle(pos.x + 22, pos.y + 3, 7, 10))

    End Sub



    Sub Build_PubH(ByVal pos As PointI)
        Dim Building_tiles As HashSet(Of Build_Tiles)

        Building_tiles = Load_Building("Desert_PubH.bld")

        For Each t In Building_tiles
            P.tile_map(t.X + pos.x, t.Y + pos.y).sprite = t.Sprite
            P.tile_map(t.X + pos.x, t.Y + pos.y).sprite2 = t.Sprite2
            P.tile_map(t.X + pos.x, t.Y + pos.y).type = CType(t.Type, planet_tile_type_enum)
            P.tile_map(t.X + pos.x, t.Y + pos.y).walkable = CType(t.Walkable, walkable_type_enum)
        Next


        Dim ID As Integer
        For a = 0 To 100000
            If Not P.Building_List.ContainsKey(a) Then ID = a : Exit For
        Next

        P.Building_List.Add(ID, New Planet_Building(0, New Rectangle(pos.x, pos.y, 32, 16), building_type_enum.Pub))
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 4, pos.y + 3, 24, 10))


        P.Building_List(ID).access_point.Add(New PointI(pos.x + 5, pos.y + 9), New Building_Access_Point_Type)
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 7, pos.y + 9), New Building_Access_Point_Type)
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 9, pos.y + 9), New Building_Access_Point_Type)
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 11, pos.y + 9), New Building_Access_Point_Type)
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 13, pos.y + 9), New Building_Access_Point_Type)

    End Sub




    Sub Create_Resource_Points()

        For Each item In P.Resource_Points
            'For x = item.x * 32 To item.x * 32 + 32
            'For y = item.y * 32 To item.y * 32 + 32
            'If random(0, 30) = 30 Then
            'P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Desert_Planet, planet_sprite_enum.Water, walkable_type_enum.Impassable)
            'End If
            'Next
            'Next
            For a = 0 To 100
                P.tile_map(random(item.Key.x * 32, 32), random(item.Key.y * 32, 32)) = New Planet_tile(planet_tile_type_enum.Desert_Planet, desert_planet_sprite_enum.Crystal, walkable_type_enum.Impassable)
            Next

        Next

    End Sub



    Sub Draw_Street(ByVal Pos As PointI, ByVal Size As PointI)

        For x = Pos.x To Pos.x + Size.x
            For y = Pos.y To Pos.y + 1
                If x >= 0 AndAlso x <= P.size.x AndAlso y >= 0 AndAlso y <= P.size.y Then
                    'P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next

        For x = Pos.x To Pos.x + Size.x
            For y = Pos.y + Size.y To Pos.y + Size.y - 1 Step -1
                If x >= 0 AndAlso x <= P.size.x AndAlso y >= 0 AndAlso y <= P.size.y Then
                    'P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next

        For x = Pos.x To Pos.x + 1
            For y = Pos.y To Pos.y + Size.y
                If x >= 0 AndAlso x <= P.size.x AndAlso y >= 0 AndAlso y <= P.size.y Then
                    'P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next

        For x = Pos.x + Size.x To Pos.x + Size.x - 1 Step -1
            For y = Pos.y To Pos.y + Size.y
                If x >= 0 AndAlso x <= P.size.x AndAlso y >= 0 AndAlso y <= P.size.y Then
                    'P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
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
        P.population = random(0, 256)
        Dim Capital As PointI
        Dim ShipyardCount As Integer = random(0, 1)
        Dim LargeShipyardCount As Integer = -1

        Capital = New PointI(random(2, 12), random(2, 12))
        'Capital = New PointI(2, 2)

        If P.population >= 4 Then ShipyardCount += 1
        If P.population >= 8 Then ShipyardCount += 1 : LargeShipyardCount += 1
        If P.population >= 12 Then ShipyardCount += 1 : LargeShipyardCount += 1



        Draw_Street(New PointI(Capital.x * 32, Capital.y * 32), New PointI(32, 32))
        P.Block_Map.Add(Capital)

        For Each item In P.Resource_Points
            For x = item.Key.x * 32 To item.Key.x * 32 + 32
                For y = item.Key.y * 32 To item.Key.y * 32 + 32
                    'If random(0, 1) = 1 Then
                    'P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Water, walkable_type_enum.Impassable)
                    'End If
                Next
            Next
        Next

        'Build_Item(planet_sprite_enum.Tree1, New PointI(2, 3), New PointI(0, 0))

        Build_Blocks(Capital, 9)

        For a = 0 To 10
            Dim blocks(P.Block_Map.Count - 1) As PointI
            P.Block_Map.CopyTo(blocks, 0)

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
                If x >= 0 AndAlso x < P.size.x \ 32 AndAlso y >= 0 AndAlso y < P.size.y \ 32 Then
                    If Not P.Block_Map.Contains(New PointI(x, y)) AndAlso Not P.Resource_Points.ContainsKey(New PointI(x, y)) Then
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


                        P.Block_Map.Add(New PointI(x, y))
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
            If Not P.Building_List.ContainsKey(a) Then ID = a : Exit For
        Next


        If Type = building_type_enum.House Then
            'external
            For y = 2 To 14
                For x = 2 To 14
                    P.tile_map(Pos.x + x, Pos.y + y).type = planet_tile_type_enum.House
                    P.tile_map(Pos.x + x, Pos.y + y).sprite = 4
                    P.tile_map(Pos.x + x, Pos.y + y).walkable = walkable_type_enum.Impassable
                Next
            Next

            P.tile_map(Pos.x + 2, Pos.y + 2).sprite = 5
            P.tile_map(Pos.x + 14, Pos.y + 2).sprite = 6

            For x = 3 To 13
                P.tile_map(Pos.x + x, Pos.y + 2).sprite = 7
            Next

            For x = 2 To 14
                P.tile_map(Pos.x + x, Pos.y + 12).sprite = 1
            Next

            For x = 2 To 14
                P.tile_map(Pos.x + x, Pos.y + 13).sprite = 0
            Next

            For x = 2 To 14
                P.tile_map(Pos.x + x, Pos.y + 14).sprite = 0
            Next

            For y = 3 To 11
                P.tile_map(Pos.x + 2, Pos.y + y).sprite = 8
            Next

            For y = 3 To 11
                P.tile_map(Pos.x + 14, Pos.y + y).sprite = 9
            Next

            P.tile_map(Pos.x + 8, Pos.y + 13).sprite = 3
            P.tile_map(Pos.x + 8, Pos.y + 14).sprite = 2
            For y = Pos.y + 3 To Pos.y + 13
                For x = Pos.x + 3 To Pos.x + 13
                    P.tile_map(x, y).walkable = walkable_type_enum.Walkable
                Next
            Next
            P.tile_map(Pos.x + 8, Pos.y + 14).walkable = walkable_type_enum.Walkable
            'internal
            For y = 2 To 13
                For x = 2 To 13
                    P.tile_map(Pos.x + x, Pos.y + y).sprite2 = 4
                Next
            Next

            P.tile_map(Pos.x + 2, Pos.y + 2).sprite2 = 10
            P.tile_map(Pos.x + 14, Pos.y + 2).sprite2 = 11

            P.tile_map(Pos.x + 2, Pos.y + 14).sprite2 = 12
            P.tile_map(Pos.x + 14, Pos.y + 14).sprite2 = 13

            For x = 3 To 13
                P.tile_map(Pos.x + x, Pos.y + 2).sprite2 = 14
            Next

            For x = 3 To 13
                P.tile_map(Pos.x + x, Pos.y + 14).sprite2 = 14
            Next

            For y = 3 To 13
                P.tile_map(Pos.x + 2, Pos.y + y).sprite2 = 15
            Next

            For y = 3 To 13
                P.tile_map(Pos.x + 14, Pos.y + y).sprite2 = 16
            Next

            P.tile_map(Pos.x + 8, Pos.y + 14).sprite2 = 17


            P.Building_List.Add(ID, New Planet_Building(0, New Rectangle(Pos.x + 2, Pos.y + 2, 13, 13), building_type_enum.House))
        End If





    End Sub


    Sub Build_Item(ByVal StartIndex As Integer, ByVal Size As PointI, ByVal Position As PointI)
        For y = 0 To Size.y
            For x = 0 To Size.x
                'P.tile_map(x + Position.x, y + Position.y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, CType(StartIndex + x + y * (Size.x + 1), planet_sprite_enum), walkable_type_enum.Impassable)
            Next
        Next

    End Sub


















End Module
