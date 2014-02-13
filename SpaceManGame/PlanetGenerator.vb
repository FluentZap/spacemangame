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
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Desert_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                Else
                    'If random(0, 1) = 1 Then
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Desert_Planet, planet_sprite_enum.Grass, walkable_type_enum.Walkable)
                    'Else
                    'P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Desert_Planet, planet_sprite_enum.Tree1, walkable_type_enum.Walkable)
                    'End If

                End If

            Next
        Next

        For a = 1 To random(2, 4)
            P.Resource_Points.Add(New PointI(random(1, 14), random(1, 14)))
        Next

        Create_Resource_Points()


        'Create_City()



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
                P.tile_map(random(item.x * 32, 32), random(item.y * 32, 32)) = New Planet_tile(planet_tile_type_enum.Desert_Planet, planet_sprite_enum.Water, walkable_type_enum.Impassable)
            Next

        Next

    End Sub



    Sub Draw_Street(ByVal Pos As PointI, ByVal Size As PointI)

        For x = Pos.x To Pos.x + Size.x
            For y = Pos.y To Pos.y + 1
                If x >= 0 AndAlso x <= P.size.x AndAlso y >= 0 AndAlso y <= P.size.y Then
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next

        For x = Pos.x To Pos.x + Size.x
            For y = Pos.y + Size.y To Pos.y + Size.y - 1 Step -1
                If x >= 0 AndAlso x <= P.size.x AndAlso y >= 0 AndAlso y <= P.size.y Then
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next

        For x = Pos.x To Pos.x + 1
            For y = Pos.y To Pos.y + Size.y
                If x >= 0 AndAlso x <= P.size.x AndAlso y >= 0 AndAlso y <= P.size.y Then
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
                End If
            Next
        Next

        For x = Pos.x + Size.x To Pos.x + Size.x - 1 Step -1
            For y = Pos.y To Pos.y + Size.y
                If x >= 0 AndAlso x <= P.size.x AndAlso y >= 0 AndAlso y <= P.size.y Then
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Wood, walkable_type_enum.Walkable)
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
            For x = item.x * 32 To item.x * 32 + 32
                For y = item.y * 32 To item.y * 32 + 32
                    'If random(0, 1) = 1 Then
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, planet_sprite_enum.Water, walkable_type_enum.Impassable)
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
                    If Not P.Block_Map.Contains(New PointI(x, y)) AndAlso Not P.Resource_Points.Contains(New PointI(x, y)) Then
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
                P.tile_map(x + Position.x, y + Position.y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, CType(StartIndex + x + y * (Size.x + 1), planet_sprite_enum), walkable_type_enum.Impassable)
            Next
        Next

    End Sub


















End Module
