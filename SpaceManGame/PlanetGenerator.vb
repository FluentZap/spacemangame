Module PlanetGenerator
    'Private P As Planet


    Public Sub populate_planet(ByVal P As Planet)
        If P.Generated = True Then
            If P.type = planet_type_enum.Desert Then repopulate_desert(P)
        Else
            If P.type = planet_type_enum.Desert Then populate_desert(P)
        End If

    End Sub

    Sub repopulate_desert(ByVal P As Planet)
        For Each item In P.Building_List
            If item.Value.Generated = False Then
                item.Value.Generated = True
                Build_Building(item.Value.Type, P.GetEmptyBuildingID, item.Value.Owner, P)
            End If
        Next
    End Sub



    Sub populate_desert(ByVal P As Planet)        
        'Set base
        For x = 0 To P.size.x
            For y = 0 To P.size.y
                If random(0, 20) = 20 Then
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Desert_Planet, desert_planet_sprite_enum.Pond, walkable_type_enum.Walkable)
                Else
                    P.tile_map(x, y) = New Planet_tile(planet_tile_type_enum.Desert_Planet, desert_planet_sprite_enum.Rough, walkable_type_enum.Walkable)
                End If
            Next
        Next

        'Add resource Points
        For a = 1 To P.Resource_Count
            Dim po As New PointI(random(1, 14), random(1, 14))
            If P.Resource_Points.ContainsKey(po) OrElse P.Farm_Points.ContainsKey(po) Then
                a -= 1
            Else
                P.Resource_Points.Add(po, False)
            End If
        Next
        'Add farm Points
        For a = 1 To P.Farm_Count
            Dim po As New PointI(random(1, 14), random(1, 14))
            If P.Resource_Points.ContainsKey(po) OrElse P.Farm_Points.ContainsKey(po) Then
                a -= 1
            Else
                P.Farm_Points.Add(po, False)
            End If
        Next

        Create_Resource_Points(P)
        Create_Farm_Points(P)

        P.CapitalPoint = New PointI(8, 8)
        P.Building_List.Add(P.GetEmptyBuildingID, New Planet_Building(0, building_type_enum.Outpost))
        P.Building_List.Add(P.GetEmptyBuildingID, New Planet_Building(0, building_type_enum.Exchange))

        'Build_Building(building_type_enum.Outpost, P.GetEmptyBuildingID, 0, P, New Block_Return_Type_Class(New PointI(1, 1), Block_Return_Type_Enum.WholeTile))
        'Build_Building(building_type_enum.Exchange, P.GetEmptyBuildingID, 0, P)

        For Each item In P.Building_List
            item.Value.Generated = True
            Build_Building(item.Value.Type, item.Key, item.Value.Owner, P)

            For Each slot In item.Value.Worker_Slots
                Dim Pos As PointI
                Pos.x = slot.Key.x + item.Value.LandRect.X
                Pos.y = slot.Key.y + item.Value.LandRect.Y
                If slot.Value = -1 Then AddWorkers(Pos, slot.Value, P)
            Next

            For Each slot In item.Value.Guard_Slots
                Dim Pos As PointI
                Pos.x = slot.Key.x + item.Value.LandRect.X
                Pos.y = slot.Key.y + item.Value.LandRect.Y
                If slot.Value = -1 Then AddGuard(Pos, slot.Value, P)
            Next            
        Next

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
        P.Generated = True
    End Sub

    Sub AddWorkers(ByVal Slot As PointI, ByVal Id As Integer, ByVal P As Planet)
        Dim CrewId As Integer = P.GetEmptyCrewID
        Add_Crew(CrewId, New Crew(0, New PointD(Slot.x * 32, Slot.y * 32), P.PlanetID, Officer_location_enum.Planet, 1, character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New crew_resource_type(0, 0))) : Id = CrewId
    End Sub

    Sub AddGuard(ByVal Slot As PointI, ByVal Id As Integer, ByVal P As Planet)
        Dim CrewId As Integer = P.GetEmptyCrewID
        Add_Crew(CrewId, New Crew(0, New PointD(Slot.x * 32, Slot.y * 32), P.PlanetID, Officer_location_enum.Planet, 1, character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New crew_resource_type(0, 0))) : Id = CrewId
    End Sub

    Sub BuildRoad(ByVal P As Planet, ByVal Pos As PointI, ByVal Size As Block_Return_Type_Enum)
        Dim DrawRect As Rectangle
        Select Case Size
            Case Is = Block_Return_Type_Enum.WholeTile : DrawRect = New Rectangle(Pos.x, Pos.y, 31, 31)
            Case Is = Block_Return_Type_Enum.HorizontalTop : DrawRect = New Rectangle(Pos.x, Pos.y, 31, 15)
            Case Is = Block_Return_Type_Enum.VerticalL : DrawRect = New Rectangle(Pos.x, Pos.y, 15, 31)
            Case Is = Block_Return_Type_Enum.TopL : DrawRect = New Rectangle(Pos.x, Pos.y, 15, 15)
        End Select



        For y = DrawRect.Y To DrawRect.Bottom
            For X = DrawRect.X To DrawRect.Right
                If X = DrawRect.X OrElse X = DrawRect.Right OrElse y = DrawRect.Y OrElse y = DrawRect.Bottom Then
                    P.tile_map(X, y).sprite = 34
                    P.tile_map(X, y).walkable = walkable_type_enum.Walkable
                End If
            Next
        Next

    End Sub


    Sub AddPlanetBlock(ByVal Position As PointI, ByVal Type As Block_Return_Type_Enum, ByVal P As Planet)
        If Not P.Block_Map.Block.ContainsKey(Position) Then P.Block_Map.Block.Add(Position, New Block_type)
        Dim B As Block_type = P.Block_Map.Block(Position)

        Select Case Type
            Case Is = Block_Return_Type_Enum.WholeTile
                B.SetAll(True)
            Case Is = Block_Return_Type_Enum.TopL
                B.TopL = True
            Case Is = Block_Return_Type_Enum.TopR
                B.TopR = True
            Case Is = Block_Return_Type_Enum.BotL
                B.BotL = True
            Case Is = Block_Return_Type_Enum.BotR
                B.BotR = True
            Case Is = Block_Return_Type_Enum.HorizontalTop
                B.TopL = True
                B.TopR = True
            Case Is = Block_Return_Type_Enum.HorizontalBot
                B.BotL = True
                B.BotR = True
            Case Is = Block_Return_Type_Enum.VerticalL
                B.TopL = True
                B.BotL = True
            Case Is = Block_Return_Type_Enum.VerticalR
                B.TopR = True
                B.BotR = True
        End Select
    End Sub

    Sub Build_Mine(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Mine, Block_Return_Type_Enum.WholeTile, P, pos, False)        

        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Mine))
        '        P.Building_List(ID).PickupPoint = New PointI(pos.x + 24, pos.y + 10)
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 7, pos.y + 18, 10, 6))
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 32, 32)
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 3, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 8, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 13, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 18, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))

        For y = 1 To 4
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 3, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 4, pos.y + y), New Item_Slots_Type(False))

            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 8, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 9, pos.y + y), New Item_Slots_Type(False))

            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 13, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 14, pos.y + y), New Item_Slots_Type(False))

            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 18, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 19, pos.y + y), New Item_Slots_Type(False))
        Next

        For x = 23 To 26
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 4), New Item_Slots_Type(True))

            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 6), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 7), New Item_Slots_Type(True))
        Next

        'P.Item_Point.Add(New PointI(pos.x + 23, pos.y + 3), New Item_Point_Type(100, Item_Enum.Parts))

        'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 8, pos.y + 20), New Item_Slots_Type(True))
        'P.Item_Point.Add(New PointI(pos.x + 8, pos.y + 20), New Item_Point_Type(2000, Item_Enum.CrystalCoin))
        P.Building_List(ID).Worker_Slots.Add(New PointI(2, 2), -1)
        BuildRoad(P, pos, Block_Return_Type_Enum.WholeTile)
    End Sub


    Sub Build_Farm(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Farm, Block_Return_Type_Enum.WholeTile, P, pos, False)


        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Farm))

        '        P.Building_List(ID).PickupPoint = New PointI(pos.x + 24, pos.y + 10)

        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 21, pos.y + 8, 9, 10))
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 32, 32)
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 2, pos.y + 19), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 6, pos.y + 19), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 10, pos.y + 19), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 14, pos.y + 19), New Building_Access_Point_Type(BAP_Type.Worker))

        For y = 22 To 29
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 2, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 3, pos.y + y), New Item_Slots_Type(False))

            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 5, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 6, pos.y + y), New Item_Slots_Type(False))

            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 8, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 9, pos.y + y), New Item_Slots_Type(False))

            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 11, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 12, pos.y + y), New Item_Slots_Type(False))

            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 14, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 15, pos.y + y), New Item_Slots_Type(False))

            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 17, pos.y + y), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 18, pos.y + y), New Item_Slots_Type(False))

        Next

        'P.Item_Point.Add(New PointI(pos.x + 2, pos.y + 22), New Item_Point_Type(100, Item_Enum.Crystal))

        'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 22, pos.y + 9), New Item_Slots_Type(True))
        'P.Item_Point.Add(New PointI(pos.x + 22, pos.y + 9), New Item_Point_Type(2000, Item_Enum.CrystalCoin))

        P.Building_List(ID).Worker_Slots.Add(New PointI(2, 2), -1)

        BuildRoad(P, pos, Block_Return_Type_Enum.WholeTile)
    End Sub

    Sub Build_RefineryH(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Refinery, Block_Return_Type_Enum.HorizontalTop, P, pos)


        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Refinery))
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 32, 16)

        'P.Building_List(ID).PickupPoint = New PointI(pos.x + 11, pos.y + 8)

        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 14, pos.y + 6), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 17, pos.y + 6), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 14, pos.y + 10), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 17, pos.y + 10), New Building_Access_Point_Type(BAP_Type.Worker))


        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 20, pos.y + 8), New Building_Access_Point_Type(BAP_Type.Transporter))

        For x = 3 To 5
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 7), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 8), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 9), New Item_Slots_Type(True))
        Next

        For x = 26 To 28
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 7), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 8), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 9), New Item_Slots_Type(False))
        Next


        'P.Item_Point.Add(New PointI(pos.x + 3, pos.y + 7), New Item_Point_Type(100, Item_Enum.Crystal))

        'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 16, pos.y + 8), New Item_Slots_Type(True))
        'P.Item_Point.Add(New PointI(pos.x + 16, pos.y + 8), New Item_Point_Type(2000, Item_Enum.CrystalCoin))
        P.Building_List(ID).Worker_Slots.Add(New PointI(2, 2), -1)
        BuildRoad(P, pos, Block_Return_Type_Enum.HorizontalTop)
    End Sub

    Sub Build_RefineryV(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Refinery, Block_Return_Type_Enum.VerticalL, P, pos)

        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Refinery))
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 16, 32)

        'P.Building_List(ID).PickupPoint = New PointI(pos.x + 8, pos.y + 11)

        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 6, pos.y + 14), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 10, pos.y + 14), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 6, pos.y + 17), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 10, pos.y + 17), New Building_Access_Point_Type(BAP_Type.Worker))


        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 8, pos.y + 15), New Building_Access_Point_Type(BAP_Type.Transporter))

        For x = 7 To 9
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 4), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 5), New Item_Slots_Type(False))
        Next

        For x = 7 To 9
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 26), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 27), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 28), New Item_Slots_Type(True))
        Next


        'P.Item_Point.Add(New PointI(pos.x + 7, pos.y + 26), New Item_Point_Type(100, Item_Enum.Crystal))

        'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 8, pos.y + 20), New Item_Slots_Type(True))
        'P.Item_Point.Add(New PointI(pos.x + 8, pos.y + 20), New Item_Point_Type(2000, Item_Enum.CrystalCoin))

        P.Building_List(ID).Worker_Slots.Add(New PointI(2, 2), -1)

        BuildRoad(P, pos, Block_Return_Type_Enum.VerticalL)
    End Sub

    Sub Build_FactoryH(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Factory, Block_Return_Type_Enum.HorizontalTop, P, pos, False)
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 32, 16)

        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Factory))

        'P.Building_List(ID).PickupPoint = New PointI(pos.x + 15, pos.y + 7)

        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 9, pos.y + 8), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 13, pos.y + 8), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 18, pos.y + 8), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 22, pos.y + 8), New Building_Access_Point_Type(BAP_Type.Worker))


        For x = 1 To 3
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 1), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 2), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(True))
        Next

        For x = 28 To 30
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 1), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 2), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(False))
        Next
        'P.Item_Point.Add(New PointI(pos.x + 1, pos.y + 1), New Item_Point_Type(100, Item_Enum.Refined_Crystal))


        'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 10, pos.y + 10), New Item_Slots_Type(True))
        'P.Item_Point.Add(New PointI(pos.x + 10, pos.y + 10), New Item_Point_Type(2000, Item_Enum.CrystalCoin))

        P.Building_List(ID).Worker_Slots.Add(New PointI(2, 2), -1)
        BuildRoad(P, pos, Block_Return_Type_Enum.HorizontalTop)
    End Sub


    Sub Build_FactoryV(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Factory, Block_Return_Type_Enum.VerticalL, P, pos, False)
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 16, 32)

        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Factory))

        'P.Building_List(ID).PickupPoint = New PointI(pos.x + 15, pos.y + 7)

        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 4, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 11, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 4, pos.y + 22), New Building_Access_Point_Type(BAP_Type.Worker))
        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 11, pos.y + 22), New Building_Access_Point_Type(BAP_Type.Worker))


        For x = 3 To 5
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 14), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 15), New Item_Slots_Type(True))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 16), New Item_Slots_Type(True))
        Next

        For x = 10 To 14
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 14), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 15), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 16), New Item_Slots_Type(False))
        Next
        'P.Item_Point.Add(New PointI(pos.x + 3, pos.y + 14), New Item_Point_Type(100, Item_Enum.Refined_Crystal))


        'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 10, pos.y + 10), New Item_Slots_Type(True))
        'P.Item_Point.Add(New PointI(pos.x + 10, pos.y + 10), New Item_Point_Type(2000, Item_Enum.CrystalCoin))
        P.Building_List(ID).Worker_Slots.Add(New PointI(2, 2), -1)
        BuildRoad(P, pos, Block_Return_Type_Enum.VerticalL)
    End Sub

    Sub Build_Building(ByVal Type As building_type_enum, ByVal ID As Integer, ByVal OwnerID As Integer, ByVal P As Planet, Optional ByVal Block_Pos As Block_Return_Type_Class = Nothing)
        If Block_Pos Is Nothing Then Block_Pos = P.FindBestBuildingPos(Type)
        AddPlanetBlock(Block_Pos.Pos, Block_Pos.Type, P)
        Select Case Type
            Case Is = building_type_enum.Exchange : Build_Exchange(ID, OwnerID, Block_Pos.Pos * 32, P)
            Case Is = building_type_enum.Mine : Build_Mine(ID, OwnerID, Block_Pos.Pos * 32, P)
            Case Is = building_type_enum.Farm : Build_Farm(ID, OwnerID, Block_Pos.Pos * 32, P)
            Case Is = building_type_enum.Outpost : Build_Outpost(ID, OwnerID, Block_Pos.Pos * 32, P)
            Case Is = building_type_enum.Bank : Build_Bank(ID, OwnerID, Block_Pos.Pos * 32, P)

            Case Is = building_type_enum.Specials
                If Block_Pos.Type = Block_Return_Type_Enum.TopL Then Build_Special(ID, OwnerID, Block_Pos.Pos * 32, P)
                If Block_Pos.Type = Block_Return_Type_Enum.TopR Then Build_Special(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(16, 0), P)
                If Block_Pos.Type = Block_Return_Type_Enum.BotL Then Build_Special(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(0, 16), P)
                If Block_Pos.Type = Block_Return_Type_Enum.BotR Then Build_Special(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(16, 16), P)
            Case Is = building_type_enum.Apartment
                If Block_Pos.Type = Block_Return_Type_Enum.HorizontalTop Then Build_AppartmentH(ID, OwnerID, Block_Pos.Pos * 32, P)
                If Block_Pos.Type = Block_Return_Type_Enum.HorizontalBot Then Build_AppartmentH(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(0, 16), P)
                If Block_Pos.Type = Block_Return_Type_Enum.VerticalL Then Build_AppartmentV(ID, OwnerID, Block_Pos.Pos * 32, P)
                If Block_Pos.Type = Block_Return_Type_Enum.VerticalR Then Build_AppartmentV(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(16, 0), P)
            Case Is = building_type_enum.Refinery
                If Block_Pos.Type = Block_Return_Type_Enum.HorizontalTop Then Build_RefineryH(ID, OwnerID, Block_Pos.Pos * 32, P)
                If Block_Pos.Type = Block_Return_Type_Enum.HorizontalBot Then Build_RefineryH(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(0, 16), P)
                If Block_Pos.Type = Block_Return_Type_Enum.VerticalL Then Build_RefineryV(ID, OwnerID, Block_Pos.Pos * 32, P)
                If Block_Pos.Type = Block_Return_Type_Enum.VerticalR Then Build_RefineryV(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(16, 0), P)
            Case Is = building_type_enum.Factory
                If Block_Pos.Type = Block_Return_Type_Enum.HorizontalTop Then Build_FactoryH(ID, OwnerID, Block_Pos.Pos * 32, P)
                If Block_Pos.Type = Block_Return_Type_Enum.HorizontalBot Then Build_FactoryH(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(0, 16), P)
                If Block_Pos.Type = Block_Return_Type_Enum.VerticalL Then Build_FactoryV(ID, OwnerID, Block_Pos.Pos * 32, P)
                If Block_Pos.Type = Block_Return_Type_Enum.VerticalR Then Build_FactoryV(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(16, 0), P)
            Case Is = building_type_enum.Pub
                If Block_Pos.Type = Block_Return_Type_Enum.HorizontalTop Then Build_PubH(ID, OwnerID, Block_Pos.Pos * 32, P)
                If Block_Pos.Type = Block_Return_Type_Enum.HorizontalBot Then Build_PubH(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(0, 16), P)
                If Block_Pos.Type = Block_Return_Type_Enum.VerticalL Then Build_PubV(ID, OwnerID, Block_Pos.Pos * 32, P)
                If Block_Pos.Type = Block_Return_Type_Enum.VerticalR Then Build_PubV(ID, OwnerID, Block_Pos.Pos * 32 + New PointI(16, 0), P)

        End Select


    End Sub

    Sub Build_AppartmentH(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Apartment, Block_Return_Type_Enum.HorizontalTop, P, pos, False)
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 32, 16)
        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 16), building_type_enum.Apartment))
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 2, pos.y + 2, 6, 10))
        P.Building_List(ID).BuildingRect.Add(1, New Rectangle(pos.x + 8, pos.y + 2, 8, 7))
        P.Building_List(ID).BuildingRect.Add(2, New Rectangle(pos.x + 16, pos.y + 2, 8, 7))
        P.Building_List(ID).BuildingRect.Add(3, New Rectangle(pos.x + 24, pos.y + 2, 6, 10))

        P.Building_List(ID).Customer_Slots.Add(New PointI(3, 3), -1)
        P.Building_List(ID).Customer_Slots.Add(New PointI(9, 3), -1)
        P.Building_List(ID).Customer_Slots.Add(New PointI(17, 3), -1)
        P.Building_List(ID).Customer_Slots.Add(New PointI(25, 3), -1)
        BuildRoad(P, pos, Block_Return_Type_Enum.HorizontalTop)
    End Sub

    Sub Build_AppartmentV(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Apartment, Block_Return_Type_Enum.VerticalL, P, pos, False)
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 16, 32)
        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 16), building_type_enum.Apartment))
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 2, pos.y + 2, 11, 7))
        P.Building_List(ID).BuildingRect.Add(1, New Rectangle(pos.x + 2, pos.y + 11, 5, 10))
        P.Building_List(ID).BuildingRect.Add(2, New Rectangle(pos.x + 8, pos.y + 11, 5, 10))
        P.Building_List(ID).BuildingRect.Add(3, New Rectangle(pos.x + 2, pos.y + 23, 11, 7))

        BuildRoad(P, pos, Block_Return_Type_Enum.VerticalL)
    End Sub

    Sub Build_PubH(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Pub, Block_Return_Type_Enum.HorizontalTop, P, pos, False)
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 32, 16)
        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 16), building_type_enum.Pub))
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 2, pos.y + 2, 28, 11))

        '        P.Building_List(ID).PickupPoint = New PointI(pos.x, pos.y)

        For y = 5 To 13 Step 2
            For x = 5 To 27 Step 2

                'P.Building_List(ID).access_point.Add(New PointI(pos.x + x, pos.y + y), New Building_Access_Point_Type(BAP_Type.Customer))
                P.Building_List(ID).Customer_Slots.Add(New PointI(x, y), -1)
            Next
        Next

        P.Building_List(ID).Worker_Slots.Add(New PointI(4, 4), -1)
        
        BuildRoad(P, pos, Block_Return_Type_Enum.HorizontalTop)
    End Sub

    Sub Build_PubV(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Pub, Block_Return_Type_Enum.VerticalL, P, pos, False)
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 16, 32)
        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 16), building_type_enum.Pub))
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 2, pos.y + 2, 11, 28))
        'P.Building_List(ID).PickupPoint = New PointI(pos.x, pos.y)
        For y = 5 To 13 Step 2
            For x = 5 To 27 Step 2

                'P.Building_List(ID).access_point.Add(New PointI(pos.x + x, pos.y + y), New Building_Access_Point_Type(BAP_Type.Customer))
                P.Building_List(ID).Customer_Slots.Add(New PointI(x, y), -1)
            Next
        Next
        P.Building_List(ID).Worker_Slots.Add(New PointI(4, 4), -1)

        BuildRoad(P, pos, Block_Return_Type_Enum.VerticalL)
    End Sub

    Sub Build_Exchange(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Exchange, Block_Return_Type_Enum.WholeTile, P, pos, False)

        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 32, 32)
        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Exchange))


        'P.Building_List(ID).PickupPoint = New PointI(pos.x + 5, pos.y + 13)

        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 6, pos.y + 9), New Building_Access_Point_Type(BAP_Type.Worker))

        'P.Building_List(ID).access_point.Add(New PointI(pos.x + 1, pos.y + 1), New Building_Access_Point_Type(BAP_Type.Transporter))

        For x = 3 To 28
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 3), New Item_Slots_Type(False))
            'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + x, pos.y + 4), New Item_Slots_Type(False))
        Next

        'P.Item_Point.Add(New PointI(pos.x + 4, pos.y + 2), New Item_Point_Type(100, Item_Enum.Crystal))
        'P.Building_List(ID).Item_Slots.Add(New PointI(pos.x + 4, pos.y + 4), New Item_Slots_Type(True))
        'P.Item_Point.Add(New PointI(pos.x + 4, pos.y + 4), New Item_Point_Type(2000, Item_Enum.Refined_Crystal_Piece))
        P.Building_List(ID).Worker_Slots.Add(New PointI(2, 2), -1)
        P.Building_List(ID).Worker_Slots.Add(New PointI(4, 2), -1)
        P.Building_List(ID).Worker_Slots.Add(New PointI(6, 2), -1)
        P.Building_List(ID).Worker_Slots.Add(New PointI(8, 2), -1)


        P.Building_List(ID).Guard_Slots.Add(New PointI(2, 10), -1)
        P.Building_List(ID).Guard_Slots.Add(New PointI(4, 10), -1)
        P.Building_List(ID).Guard_Slots.Add(New PointI(6, 10), -1)
        P.Building_List(ID).Guard_Slots.Add(New PointI(7, 10), -1)
        BuildRoad(P, pos, Block_Return_Type_Enum.WholeTile)
    End Sub

    Sub Build_Outpost(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Outpost, Block_Return_Type_Enum.WholeTile, P, pos, False)
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 32, 32)
        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Outpost))
        'P.Building_List(ID).PickupPoint = New PointI(pos.x + 15, pos.y + 15)
        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 4, pos.y + 4, 24, 27))
        BuildRoad(P, pos, Block_Return_Type_Enum.WholeTile)
        P.Building_List(ID).Worker_Slots.Add(New PointI(8, 2), -1)
        P.Building_List(ID).Worker_Slots.Add(New PointI(10, 2), -1)
        P.Building_List(ID).Guard_Slots.Add(New PointI(12, 2), -1)
        P.Building_List(ID).Guard_Slots.Add(New PointI(14, 2), -1)
    End Sub


    Sub Build_Bank(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Bank, Block_Return_Type_Enum.WholeTile, P, pos, False)
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 32, 32)
        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 32, 32), building_type_enum.Bank))
        'P.Building_List(ID).PickupPoint = New PointI(pos.x + 15, pos.y + 15)

        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 4, pos.y + 4, 24, 24))
        P.Building_List(ID).Worker_Slots.Add(New PointI(8, 2), -1)
        P.Building_List(ID).Worker_Slots.Add(New PointI(10, 2), -1)
        P.Building_List(ID).Guard_Slots.Add(New PointI(12, 2), -1)
        P.Building_List(ID).Guard_Slots.Add(New PointI(14, 2), -1)
        BuildRoad(P, pos, Block_Return_Type_Enum.WholeTile)
    End Sub


    Sub Build_Special(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal pos As PointI, ByVal P As Planet)
        Build_From_File(building_type_enum.Specials, Block_Return_Type_Enum.TopL, P, pos, False)
        P.Building_List(ID).LandRect = New Rectangle(pos.x, pos.y, 16, 16)
        'P.Building_List.Add(ID, New Planet_Building(OwnerID, New Rectangle(pos.x, pos.y, 16, 16), building_type_enum.Specials))
        'P.Building_List(ID).PickupPoint = New PointI(pos.x + 0, pos.y + 0)

        P.Building_List(ID).BuildingRect.Add(0, New Rectangle(pos.x + 3, pos.y + 3, 10, 10))
        P.Building_List(ID).Worker_Slots.Add(New PointI(8, 2), -1)        
        BuildRoad(P, pos, Block_Return_Type_Enum.TopL)
    End Sub


    Public Function Build_From_File(ByVal Building As building_type_enum, ByVal Block_Type As Block_Return_Type_Enum, ByVal P As Planet, ByVal Pos As PointI, Optional ByVal ToBuffer As Boolean = False) As HashSet(Of Build_Tiles)
        Dim Building_tiles As New HashSet(Of Build_Tiles)
        Select Case Building
            Case Is = building_type_enum.Bank : Building_tiles = Load_Building("Desert_Bank.bld")
            Case Is = building_type_enum.Specials : Building_tiles = Load_Building("Desert_Special.bld")
            Case Is = building_type_enum.Outpost : Building_tiles = Load_Building("Desert_Outpost.bld")
            Case Is = building_type_enum.Factory
                If Block_Type = Block_Return_Type_Enum.HorizontalTop OrElse Block_Type = Block_Return_Type_Enum.HorizontalBot Then Building_tiles = Load_Building("Desert_FactoryH.bld")
                If Block_Type = Block_Return_Type_Enum.VerticalL OrElse Block_Type = Block_Return_Type_Enum.VerticalR Then Building_tiles = Load_Building("Desert_FactoryV.bld")
            Case Is = building_type_enum.Refinery
                If Block_Type = Block_Return_Type_Enum.HorizontalTop OrElse Block_Type = Block_Return_Type_Enum.HorizontalBot Then Building_tiles = Load_Building("Desert_RefineryH.bld")
                If Block_Type = Block_Return_Type_Enum.VerticalL OrElse Block_Type = Block_Return_Type_Enum.VerticalR Then Building_tiles = Load_Building("Desert_RefineryV.bld")
            Case Is = building_type_enum.Mine : Building_tiles = Load_Building("Desert_Mine.bld")
            Case Is = building_type_enum.Farm : Building_tiles = Load_Building("Desert_Farm.bld")
            Case Is = building_type_enum.Exchange : Building_tiles = Load_Building("Desert_Exchange.bld")
            Case Is = building_type_enum.Pub
                If Block_Type = Block_Return_Type_Enum.HorizontalTop OrElse Block_Type = Block_Return_Type_Enum.HorizontalBot Then Building_tiles = Load_Building("Desert_PubH.bld")
                If Block_Type = Block_Return_Type_Enum.VerticalL OrElse Block_Type = Block_Return_Type_Enum.VerticalR Then Building_tiles = Load_Building("Desert_PubV.bld")
            Case Is = building_type_enum.Apartment
                If Block_Type = Block_Return_Type_Enum.HorizontalTop OrElse Block_Type = Block_Return_Type_Enum.HorizontalBot Then Building_tiles = Load_Building("Desert_AppartmentH.bld")
                If Block_Type = Block_Return_Type_Enum.VerticalL OrElse Block_Type = Block_Return_Type_Enum.VerticalR Then Building_tiles = Load_Building("Desert_AppartmentV.bld")
        End Select

        If ToBuffer = False Then
            For Each t In Building_tiles
                P.tile_map(t.X + Pos.x, t.Y + Pos.y).sprite = t.Sprite
                P.tile_map(t.X + Pos.x, t.Y + Pos.y).sprite2 = t.Sprite2
                P.tile_map(t.X + Pos.x, t.Y + Pos.y).type = CType(t.Type, planet_tile_type_enum)
                P.tile_map(t.X + Pos.x, t.Y + Pos.y).walkable = CType(t.Walkable, walkable_type_enum)
            Next
        Else
            Return Building_tiles
        End If
        Return Nothing
    End Function





    Sub Create_Farm_Points(ByVal P As Planet)
        For Each item In P.Farm_Points
            For a = 0 To 100
                P.tile_map(random(item.Key.x * 32, 32), random(item.Key.y * 32, 32)) = New Planet_tile(planet_tile_type_enum.Desert_Planet, 3, walkable_type_enum.Walkable)
            Next
        Next
    End Sub

    Sub Create_Resource_Points(ByVal P As Planet)
        For Each item In P.Resource_Points
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
            'Dim blocks(P.Block_Map.Count - 1) As PointI
            '            P.Block_Map.CopyTo(blocks, 0)

            'For Each item In blocks
            ' Build_Blocks(item, 9, P)
            'Next
        Next

        Draw_Street(New PointI(0, 0), New PointI(16, 16), P)


        'Fighter/Corvette facility 30x30
        'Smallest Block size 32,32
        'Frigate facility 40x60
        'Shipyard_Rect.X = random(10, 60) + City_Rect.X
        'Shipyard_Rect.Y = random(10, 60) + City_Rect.Y







        'Set city rect

    End Sub


    Sub Build_Blocks(ByVal Pos As PointI, ByVal BuildMax As Integer, ByVal P As Planet)
        Dim Built As Integer
        Dim block_Type As Block_Type_Enum
        For x = Pos.x - 1 To Pos.x + 1
            For y = Pos.y - 1 To Pos.y + 1
                If x >= 0 AndAlso x < P.size.x \ 32 AndAlso y >= 0 AndAlso y < P.size.y \ 32 Then
                    'If Not P.Block_Map.Contains(New PointI(x, y)) AndAlso Not P.Resource_Points.ContainsKey(New PointI(x, y)) Then
                    block_Type = CType(random(0, 7), Block_Type_Enum)



                    'P.Block_Map.Add(New PointI(x, y))
                    Built += 1
                    If Built >= BuildMax Then Exit Sub
                End If
                'End If
            Next
        Next

    End Sub





    Sub Build_Item(ByVal StartIndex As Integer, ByVal Size As PointI, ByVal Position As PointI)
        For y = 0 To Size.y
            For x = 0 To Size.x
                'P.tile_map(x + Position.x, y + Position.y) = New Planet_tile(planet_tile_type_enum.Forest_Planet, CType(StartIndex + x + y * (Size.x + 1), planet_sprite_enum), walkable_type_enum.Impassable)
            Next
        Next

    End Sub


















End Module
