Module PlanetGenerator
    'Private P As Planet


    Public Sub populate_planet(ByVal P As Planet)

        If P.type = planet_type_enum.Desert Then populate_desert(P)

    End Sub


    Sub populate_desert(ByVal P As Planet)

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
        P.Resource_Points.Clear()

        'P.Resource_Points.Add(New PointI(1, 1), False)

        'P.Resource_Points.Add(New PointI(3, 1), False)

        P.Resource_Points.Add(New PointI(5, 1), False)
        P.Farm_Points.Add(New PointI(4, 4), False)

        Create_Resource_Points(P)


        Build_Outpost(P.GetEmptyBuildingID, 0, New PointI(32 * 2, 32 * 2), P, True)
        P.CapitalPoint = New PointI(2, 2)

        'Build_AppartmentH(New PointI(32, 0))
        'Build_PubH(New PointI(32, 16))

        'Dim CID As Integer
        'Dim BID As Integer = 2
        'For Each Point In P.Resource_Points

        'Build_Mine(New PointI(Point.Key.x * 32, Point.Key.y * 32))
        'CID = AddTestWorkforce(BID, CID)
        'BID += 1

        'Build_RefineryH(New PointI(Point.Key.x * 32 + 32, Point.Key.y * 32))
        'CID = AddTestWorkforce(BID, CID)
        'BID += 1

        'Build_FactoryH(New PointI(Point.Key.x * 32 + 32, Point.Key.y * 32 + 16))
        'CID = AddTestWorkforce(BID, CID)
        'BID += 1


        'Build_RefineryH(New PointI(Point.Key.x * 32, Point.Key.y * 32 + 32))
        'CID = AddTestWorkforce(BID, CID)
        'BID += 1

        'Build_FactoryH(New PointI(Point.Key.x * 32, Point.Key.y * 32 + 48))
        'CID = AddTestWorkforce(BID, CID)
        'BID += 1


        'Next
        'Build_Exchange(New PointI(32 * 3, 32 * 1))

        'Build_Mine(New PointI(0, 0))

        'Build_AppartmentH(New PointI(32, 0))

        'Build_Mine(New PointI(P.Resource_Points.First.Key.x * 32, P.Resource_Points.First.Key.y * 32))




        'Build_RefineryH(New PointI(0, 32))

        'Build_FactoryH(New PointI(32, 32))

        'Create_City()

        P.CitizensMax = 100
        P.Citizens = 0
        P.Building_Count.SetLevel(Planet_Level_Type.Outpost)
    End Sub

    Function AddTestWorkforce(ByVal BID As Integer, ByVal CID As Integer, ByVal P As Planet) As Integer
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Morning, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Morning, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Morning, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Morning, P) : CID += 1

        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Mid, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Mid, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Mid, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Mid, P) : CID += 1

        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Night, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Night, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Night, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Worker, 0, 0, BID, 1, Work_Shift_Enum.Night, P) : CID += 1

        QuickAddCrew(CID, Worker_Type_Enum.Transporter, 0, 0, BID, 1, Work_Shift_Enum.Morning, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Transporter, 0, 0, BID, 1, Work_Shift_Enum.Mid, P) : CID += 1
        QuickAddCrew(CID, Worker_Type_Enum.Transporter, 0, 0, BID, 1, Work_Shift_Enum.Night, P) : CID += 1
        Return CID
    End Function


    Sub QuickAddCrew(ByVal ID As Integer, ByVal WorkerType As Worker_Type_Enum, ByVal HomeBuilding As Integer, ByVal HomeSpace As Integer, ByVal WorkBuilding As Integer, ByVal PubBuilding As Integer, ByVal WorkShift As Work_Shift_Enum, ByVal P As Planet)
        Add_Crew(ID, New Crew(0, New PointD(0, 0), 0, Officer_location_enum.Planet, 10, character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New crew_resource_type(0, 0)))
        P.crew_list(ID).Worker_Type = WorkerType
        P.crew_list(ID).HomeBuilding = HomeBuilding
        P.crew_list(ID).HomeSpace = HomeSpace
        P.crew_list(ID).WorkBuilding = WorkBuilding
        P.crew_list(ID).PubBuilding = PubBuilding
        P.crew_list(ID).WorkShift = WorkShift
    End Sub


    Sub AddPlanetBlock(ByVal Pos As PointI, ByVal P As Planet)
        If P.Block_Map.ContainsKey(Pos) Then
            P.Block_Map(Pos) = True
        Else
            P.Block_Map.Add(Pos, True)
        End If
    End Sub

    Sub Build_Mine(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet, ByVal BuildTiles As Boolean)
        If BuildTiles = True Then Build_From_File(building_type_enum.Mine, P, pos, False)
        AddPlanetBlock(pos \ 32, P)
        P.Building_Count.CMine += 1

        P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Mine))

        P.Building_List(ID).PickupPoint = New PointI(pos.x + 24, pos.y + 10)

        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 7, pos.y + 18, 10, 6))

        P.Building_List(ID).access_point.Add(New PointI(pos.x + 3, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 8, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 13, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 18, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))

        For y = 1 To 4
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 3, pos.y + y), New Item_Slots_Type(False))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 4, pos.y + y), New Item_Slots_Type(False))

            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 8, pos.y + y), New Item_Slots_Type(False))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 9, pos.y + y), New Item_Slots_Type(False))

            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 13, pos.y + y), New Item_Slots_Type(False))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 14, pos.y + y), New Item_Slots_Type(False))

            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 18, pos.y + y), New Item_Slots_Type(False))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 19, pos.y + y), New Item_Slots_Type(False))
        Next

        For x = 23 To 26
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(True))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 4), New Item_Slots_Type(True))

            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 6), New Item_Slots_Type(True))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 7), New Item_Slots_Type(True))
        Next

        P.Item_Point.Add(New PointI(pos.x + 23, pos.y + 3), New Item_Point_Type(100, Item_Enum.Parts))

        P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 8, pos.y + 20), New Item_Slots_Type(True))
        P.Item_Point.Add(New PointI(pos.x + 8, pos.y + 20), New Item_Point_Type(2000, Item_Enum.Refined_Crystal_Piece))

    End Sub

    Sub Build_RefineryH(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet, ByVal BuildTiles As Boolean)
        If BuildTiles = True Then Build_From_File(building_type_enum.Refinery, P, pos)
        AddPlanetBlock(pos \ 32, P)
        P.Building_Count.CRefinery += 1

        P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Refinery))


        P.Building_List(ID).PickupPoint = New PointI(pos.x + 15, pos.y + 8)

        P.Building_List(ID).access_point.Add(New PointI(pos.x + 6, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 14, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 17, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 25, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))


        P.Building_List(ID).access_point.Add(New PointI(pos.x + 1, pos.y + 1), New Building_Access_Point_Type(BAP_Type.Transporter))

        For x = 4 To 27
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 2), New Item_Slots_Type(True))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(False))
        Next

        P.Item_Point.Add(New PointI(pos.x + 4, pos.y + 2), New Item_Point_Type(100, Item_Enum.Crystal))

        P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 4, pos.y + 4), New Item_Slots_Type(True))
        P.Item_Point.Add(New PointI(pos.x + 4, pos.y + 4), New Item_Point_Type(2000, Item_Enum.Refined_Crystal_Piece))

    End Sub

    Sub Build_FactoryH(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet, ByVal BuildTiles As Boolean)
        If BuildTiles = True Then Build_From_File(building_type_enum.Factory, P, pos, False)
        AddPlanetBlock(pos \ 32, P)
        P.Building_Count.CFactory += 1

        P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Factory))

        P.Building_List(ID).PickupPoint = New PointI(pos.x + 15, pos.y + 7)

        P.Building_List(ID).access_point.Add(New PointI(pos.x + 9, pos.y + 8), New Building_Access_Point_Type(BAP_Type.Worker))
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 13, pos.y + 8), New Building_Access_Point_Type(BAP_Type.Worker))
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 18, pos.y + 8), New Building_Access_Point_Type(BAP_Type.Worker))
        P.Building_List(ID).access_point.Add(New PointI(pos.x + 22, pos.y + 8), New Building_Access_Point_Type(BAP_Type.Worker))


        For x = 1 To 3
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 1), New Item_Slots_Type(True))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 2), New Item_Slots_Type(True))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(True))
        Next

        For x = 28 To 30
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 1), New Item_Slots_Type(False))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 2), New Item_Slots_Type(False))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(False))
        Next
        P.Item_Point.Add(New PointI(pos.x + 1, pos.y + 1), New Item_Point_Type(100, Item_Enum.Refined_Crystal))


        P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 10, pos.y + 10), New Item_Slots_Type(True))
        P.Item_Point.Add(New PointI(pos.x + 10, pos.y + 10), New Item_Point_Type(2000, Item_Enum.Refined_Crystal_Piece))

    End Sub

    Sub Build_AppartmentH(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet, ByVal BuildTiles As Boolean)
        If BuildTiles = True Then Build_From_File(building_type_enum.Apartment, P, pos, False)
        AddPlanetBlock(pos \ 32, P)

        P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 16), building_type_enum.Apartment))
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 3, pos.y + 3, 7, 10))
        P.Building_List(ID).BuildingRect.Add(1, New Rectangle(pos.x + 9, pos.y + 3, 7, 10))
        P.Building_List(ID).BuildingRect.Add(2, New Rectangle(pos.x + 16, pos.y + 3, 7, 10))
        P.Building_List(ID).BuildingRect.Add(3, New Rectangle(pos.x + 22, pos.y + 3, 7, 10))

    End Sub

    Sub Build_PubH(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet, ByVal BuildTiles As Boolean)
        If BuildTiles = True Then Build_From_File(building_type_enum.Pub, P, pos, False)
        AddPlanetBlock(pos \ 32, P)
        P.Building_Count.CPub += 1

        P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 16), building_type_enum.Pub))
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 4, pos.y + 3, 24, 10))

        For y = 5 To 13 Step 2
            For x = 5 To 27 Step 2

                P.Building_List(ID).access_point.Add(New PointI(pos.x + x, pos.y + y), New Building_Access_Point_Type(BAP_Type.Customer))

            Next
        Next

    End Sub

    Sub Build_Exchange(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet, ByVal BuildTiles As Boolean)
        If BuildTiles = True Then Build_From_File(building_type_enum.Exchange, P, pos, False)
        AddPlanetBlock(pos \ 32, P)

        P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Exchange))


        P.Building_List(ID).PickupPoint = New PointI(pos.x + 5, pos.y + 13)

        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 6, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))

        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 1, pos.y + 1), New Building_Access_Point_Type(BAP_Type.Transporter))

        For x = 3 To 28
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(False))
            P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 4), New Item_Slots_Type(False))
        Next

        'P.Item_Point.Add(New PointI(pos.x + 4, pos.y + 2), New Item_Point_Type(100, Item_Enum.Crystal))
        'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 4, pos.y + 4), New Item_Slots_Type(True))
        'P.Item_Point.Add(New PointI(pos.x + 4, pos.y + 4), New Item_Point_Type(2000, Item_Enum.Refined_Crystal_Piece))

    End Sub

    Sub Build_Outpost(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet, ByVal BuildTiles As Boolean)
        If BuildTiles = True Then Build_From_File(building_type_enum.Outpost, P, pos, False)
        AddPlanetBlock(pos \ 32, P)


        P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Outpost))
        P.Building_List(ID).PickupPoint = New PointI(pos.x + 15, pos.y + 15)
    End Sub




    Public Function Build_From_File(ByVal Building As building_type_enum, ByVal P As Planet, ByVal Pos As PointI, Optional ByVal ToBuffer As Boolean = False) As HashSet(Of Build_Tiles)
        Dim Building_tiles As New HashSet(Of Build_Tiles)
        Select Case Building
            Case Is = building_type_enum.Outpost : Building_tiles = Load_Building("Desert_Outpost.bld")
            Case Is = building_type_enum.Factory : Building_tiles = Load_Building("Desert_FactoryH.bld")
            Case Is = building_type_enum.Refinery : Building_tiles = Load_Building("Desert_RefineryH.bld")
            Case Is = building_type_enum.Mine : Building_tiles = Load_Building("Desert_Mine.bld")
            Case Is = building_type_enum.Exchange : Building_tiles = Load_Building("Desert_Exchange.bld")
            Case Is = building_type_enum.Pub : Building_tiles = Load_Building("Desert_PubH.bld")
            Case Is = building_type_enum.Apartment : Building_tiles = Load_Building("Desert_AppartmentH.bld")
        End Select

        If ToBuffer = False Then
            For Each t In Building_tiles
                P.tile_map(t.X + Pos.x, t.Y + Pos.y).sprite = t.Sprite
                P.tile_map(t.X + Pos.x, t.Y + Pos.y).sprite2 = t.Sprite2
                P.tile_map(t.X + Pos.x, t.Y + Pos.y).type = CType(t.Type, planet_tile_type_enum)
                P.tile_map(t.X + Pos.x, t.Y + Pos.y).walkable = CType(t.Walkable, walkable_type_enum)
            Next
        Else

            For index = 0 To Building_tiles.Count - 1
                Building_tiles(index).X += CByte(Pos.x)
                Building_tiles(index).Y += CByte(Pos.y)
            Next
            Return Building_tiles
        End If
        Return Nothing
    End Function











    Sub Create_Resource_Points(ByVal P As Planet)

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



    Sub Draw_Street(ByVal Pos As PointI, ByVal Size As PointI, ByVal P As Planet)

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


    Sub Create_City(ByVal P As Planet)
        P.population = random(0, 256)
        Dim Capital As PointI
        Dim ShipyardCount As Integer = random(0, 1)
        Dim LargeShipyardCount As Integer = -1

        Capital = New PointI(random(2, 12), random(2, 12))
        'Capital = New PointI(2, 2)

        If P.population >= 4 Then ShipyardCount += 1
        If P.population >= 8 Then ShipyardCount += 1 : LargeShipyardCount += 1
        If P.population >= 12 Then ShipyardCount += 1 : LargeShipyardCount += 1



        Draw_Street(New PointI(Capital.x * 32, Capital.y * 32), New PointI(32, 32), P)
        'P.Block_Map.Add(Capital)

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

        Build_Blocks(Capital, 9, P)

        For a = 0 To 10
            Dim blocks(P.Block_Map.Count - 1) As PointI
            '            P.Block_Map.CopyTo(blocks, 0)

            For Each item In blocks
                Build_Blocks(item, 9, P)
            Next
        Next

        Draw_Street(New PointI(0, 0), New PointI(16, 16), P)
        Build_Building(New PointI(0, 0), building_type_enum.House, P)

        'Fighter/Corvette facility 30x30
        'Smallest Block size 32,32
        'Frigate facility 40x60
        'Shipyard_Rect.X = random(10, 60) + City_Rect.X
        'Shipyard_Rect.Y = random(10, 60) + City_Rect.Y







        'Set city rect

    End Sub


    Sub Build_Blocks(ByVal Pos As PointI, ByVal BuildMax As Integer, ByVal P As Planet)
        Dim Built As Integer
        Dim block_Type As Block_Type_enum
        For x = Pos.x - 1 To Pos.x + 1
            For y = Pos.y - 1 To Pos.y + 1
                If x >= 0 AndAlso x < P.size.x \ 32 AndAlso y >= 0 AndAlso y < P.size.y \ 32 Then
                    'If Not P.Block_Map.Contains(New PointI(x, y)) AndAlso Not P.Resource_Points.ContainsKey(New PointI(x, y)) Then
                    block_Type = CType(random(0, 7), Block_Type_enum)

                    Select Case block_Type
                        Case Is = Block_Type_enum.Big
                            Draw_Street(New PointI(x * 32, y * 32), New PointI(32, 32), P)
                        Case Is = Block_Type_enum.DoubleH
                            Draw_Street(New PointI(x * 32, y * 32), New PointI(32, 16), P)
                            Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(32, 16), P)
                        Case Is = Block_Type_enum.DoubleV
                            Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 32), P)
                            Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 32), P)
                        Case Is = Block_Type_enum.Quad
                            Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 16), P)
                            Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 16), P)
                            Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(16, 16), P)
                            Draw_Street(New PointI(x * 32 + 16, y * 32 + 16), New PointI(16, 16), P)

                            Build_Building(New PointI(x * 32, y * 32), building_type_enum.House, P)
                            Build_Building(New PointI(x * 32 + 16, y * 32), building_type_enum.House, P)
                            Build_Building(New PointI(x * 32, y * 32 + 16), building_type_enum.House, P)
                            Build_Building(New PointI(x * 32 + 16, y * 32 + 16), building_type_enum.House, P)

                        Case Is = Block_Type_enum.Three1
                            Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 32), P)
                            Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 16), P)
                            Draw_Street(New PointI(x * 32 + 16, y * 32 + 16), New PointI(16, 16), P)

                            Build_Building(New PointI(x * 32 + 16, y * 32), building_type_enum.House, P)
                            Build_Building(New PointI(x * 32 + 16, y * 32 + 16), building_type_enum.House, P)
                        Case Is = Block_Type_enum.Three2
                            Draw_Street(New PointI(x * 32, y * 32), New PointI(32, 16), P)
                            Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(16, 16), P)
                            Draw_Street(New PointI(x * 32 + 16, y * 32 + 16), New PointI(16, 16), P)

                            Build_Building(New PointI(x * 32, y * 32 + 16), building_type_enum.House, P)
                            Build_Building(New PointI(x * 32 + 16, y * 32 + 16), building_type_enum.House, P)
                        Case Is = Block_Type_enum.Three3
                            Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 32), P)
                            Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 16), P)
                            Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(16, 16), P)

                            Build_Building(New PointI(x * 32, y * 32), building_type_enum.House, P)
                            Build_Building(New PointI(x * 32, y * 32 + 16), building_type_enum.House, P)
                        Case Is = Block_Type_enum.Three4
                            Draw_Street(New PointI(x * 32, y * 32 + 16), New PointI(32, 16), P)
                            Draw_Street(New PointI(x * 32, y * 32), New PointI(16, 16), P)
                            Draw_Street(New PointI(x * 32 + 16, y * 32), New PointI(16, 16), P)

                            Build_Building(New PointI(x * 32, y * 32), building_type_enum.House, P)
                            Build_Building(New PointI(x * 32 + 16, y * 32), building_type_enum.House, P)
                    End Select


                    'P.Block_Map.Add(New PointI(x, y))
                    Built += 1
                    If Built >= BuildMax Then Exit Sub
                End If
                'End If
            Next
        Next

    End Sub



    Sub Build_Building(ByVal Pos As PointI, ByVal Type As building_type_enum, ByVal P As Planet)
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
