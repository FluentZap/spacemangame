

Public Class Building_Count_Type
    Public Mine As Integer
    Public Farm As Integer
    Public Factory As Integer
    Public Refinery As Integer
    Public Pub As Integer
    Public Special As Integer
    Public SpacePort As Integer

    Public CMine As Integer
    Public CFarm As Integer
    Public CFactory As Integer
    Public CRefinery As Integer
    Public CPub As Integer
    Public CSpecial As Integer
    Public CSpacePort As Integer

    Public CitizensBuildingLimit As Integer
    Public CitizensMax As Integer

    Sub SetLevel(ByVal Level As Planet_Level_Type)
        Select Case Level
            Case Is = Planet_Level_Type.Outpost
                Mine = 1 : Farm = 1 : Factory = 1 : Refinery = 1 : Pub = 1 : Special = 1
                CitizensBuildingLimit = 2
                CitizensMax = 3
            Case Is = Planet_Level_Type.Village
                Mine = 2 : Farm = 2 : Factory = 2 : Refinery = 2 : Pub = 2 : Special = 2
                CitizensBuildingLimit = 3
                CitizensMax = 4
            Case Is = Planet_Level_Type.Town
                Mine = 2 : Farm = 3 : Factory = 2 : Refinery = 2 : Pub = 3 : Special = 6
                CitizensBuildingLimit = 3
                CitizensMax = 6
            Case Is = Planet_Level_Type.City
                Mine = 3 : Farm = 4 : Factory = 3 : Refinery = 3 : Pub = 4 : Special = 7
                CitizensBuildingLimit = 4
                CitizensMax = 6
            Case Is = Planet_Level_Type.Metropolis
                Mine = 4 : Farm = 5 : Factory = 4 : Refinery = 4 : Pub = 5 : Special = 10
                CitizensBuildingLimit = 4
                CitizensMax = 8
            Case Is = Planet_Level_Type.Empire
                Mine = 4 : Farm = 6 : Factory = 5 : Refinery = 5 : Pub = 6 : Special = 14
                CitizensBuildingLimit = 5
                CitizensMax = 8
        End Select
    End Sub



    Function Building_Available() As Integer
        Dim Available As Integer
        Available += Mine - CMine
        Available += Farm - CFarm
        Available += Factory - CFactory
        Available += Refinery - CRefinery
        Available += Pub - CPub
        Available += Special - CSpecial
        Return Available
    End Function


End Class


Public Class Item_Point_Type
    Public Amount As Integer
    Public Item As Item_Enum

    Sub New(ByVal Amount As Integer, ByVal item As Item_Enum)
        Me.Amount = Amount
        Me.Item = item
    End Sub

    Sub New()
    End Sub
End Class


Public Enum Send_work_List_Enum
    Home
    Work
End Enum



Public Class Build_List_Type
    Public Tile_List As New Dictionary(Of PointI, Byte)()
    Public Build_Progress As Integer
    Public Builders As Integer
    Public Compleated As Boolean
    Public OwnerID As Integer
    Public Type As building_type_enum
    Public BlockType As Block_Return_Type_Enum
    Public Pos As PointI
End Class

Public Enum Block_Type_Enum
    Horizontal
    Vertical
    Small
    Large
    HorV
End Enum

Public Enum Block_Return_Type_Enum As Byte
    None = 0
    TopL
    TopR
    BotL
    BotR
    WholeTile
    HorizontalTop
    HorizontalBot
    VerticalL
    VerticalR
End Enum


Public Class Block_type
    Public TopL As Boolean
    Public TopR As Boolean
    Public BotL As Boolean
    Public BotR As Boolean

    Sub SetAll(ByVal Value As Boolean)
        TopL = Value
        TopR = Value
        BotL = Value
        BotR = Value
    End Sub


End Class


Public Class Block_Return_Type_Class
    Public Pos As PointI
    Public Type As Block_Return_Type_Enum

    Sub New(ByVal Pos As PointI, ByVal Type As Block_Return_Type_Enum)
        Me.Pos = Pos
        Me.Type = Type
    End Sub


End Class


Public Class Block_Map_Type
    Public Block As New Dictionary(Of PointI, Block_type)()

    Function Get_Block(ByVal Position As PointI, ByVal Type As Block_Type_Enum) As Block_Return_Type_Enum
        If Not Block.ContainsKey(Position) Then Block.Add(Position, New Block_type)
        Dim B As Block_type = Block(Position)

        Select Case Type
            Case Is = Block_Type_Enum.Small
                If B.TopL = False Then Return Block_Return_Type_Enum.TopL
                If B.TopR = False Then Return Block_Return_Type_Enum.TopR
                If B.BotL = False Then Return Block_Return_Type_Enum.BotL
                If B.BotR = False Then Return Block_Return_Type_Enum.BotR
            Case Is = Block_Type_Enum.Large
                If B.TopL = False AndAlso B.TopR = False AndAlso B.BotL = False AndAlso B.BotR = False Then Return Block_Return_Type_Enum.WholeTile
            Case Is = Block_Type_Enum.Horizontal
                If B.TopL = False AndAlso B.TopR = False Then Return Block_Return_Type_Enum.HorizontalTop
                If B.BotL = False AndAlso B.BotR = False Then Return Block_Return_Type_Enum.HorizontalBot
            Case Is = Block_Type_Enum.Vertical
                If B.TopL = False AndAlso B.BotL = False Then Return Block_Return_Type_Enum.VerticalL
                If B.TopR = False AndAlso B.BotR = False Then Return Block_Return_Type_Enum.VerticalR
            Case Is = Block_Type_Enum.HorV
                If random(0, 1) = 0 Then
                    If B.TopL = False AndAlso B.TopR = False Then Return Block_Return_Type_Enum.HorizontalTop
                    If B.BotL = False AndAlso B.BotR = False Then Return Block_Return_Type_Enum.HorizontalBot
                    If B.TopL = False AndAlso B.BotL = False Then Return Block_Return_Type_Enum.VerticalL
                    If B.TopR = False AndAlso B.BotR = False Then Return Block_Return_Type_Enum.VerticalR
                Else
                    If B.TopL = False AndAlso B.BotL = False Then Return Block_Return_Type_Enum.VerticalL
                    If B.TopR = False AndAlso B.BotR = False Then Return Block_Return_Type_Enum.VerticalR
                    If B.TopL = False AndAlso B.TopR = False Then Return Block_Return_Type_Enum.HorizontalTop
                    If B.BotL = False AndAlso B.BotR = False Then Return Block_Return_Type_Enum.HorizontalBot
                End If
        End Select

        Return Block_Return_Type_Enum.None
    End Function

End Class

Public Class Job_List_Type
    Public Type As Worker_Type_Enum    
    Public SlotID As Integer


    Sub New(ByVal Type As Worker_Type_Enum, ByVal SlotID As Integer)
        Me.Type = Type
        Me.SlotID = SlotID
    End Sub

End Class



Public Class Housing_List_Type    
    Public SlotID As Integer
    Public BuildingID As Integer

    Sub New(ByVal SlotID As Integer, ByVal BuildingID As Integer)
        Me.SlotID = SlotID
        Me.BuildingID = BuildingID
    End Sub


End Class

Public Class Planet
    Public PlanetID As Integer
    Public orbits_planet As Boolean
    Public orbit_point As Integer
    Public orbit_distance As Integer
    Public radius As Integer

    Public population As Integer
    Public populationMax As Integer

    Public theta As Double
    Public size As PointI
    Public landed_ships As New Dictionary(Of Integer, PointI)()

    Public Animation_Glow As Single
    Public Animation_Glow_subtract As Boolean


    Public Block_Map As New Block_Map_Type()

    Public Resource_Points As New Dictionary(Of PointI, Boolean)() 'Is true is resource point is taken
    Public Farm_Points As New Dictionary(Of PointI, Boolean)() 'Is true is farm point is taken

    Public Building_List As New Dictionary(Of Integer, Planet_Building)()

    Public Send_List As New Dictionary(Of KeyValuePair(Of Integer, Crew), Send_work_List_Enum)()

    Public Item_Point As New Dictionary(Of PointI, Item_Point_Type)()


    Public Free_Job_List As New Dictionary(Of Integer, HashSet(Of Job_List_Type))()

    Public Free_Housing_List As New HashSet(Of Housing_List_Type)()

    Public Free_Job_List_Count As Integer



    Private path_find As Pathfind.Pathfind

    Private BuilderSpawnCounter As Integer = 100
    Private BuilderCount As Integer
    Public Builder_List As New Dictionary(Of Integer, Integer)()

    Public Build_List As New Dictionary(Of Integer, Build_List_Type)()

    Public Citizens As Integer
    Public CitizensMax As Integer
    Public Building_Count As New Building_Count_Type()
    'trade route
    'available resources
    'regions
    Public tile_map(,) As Planet_tile
    'Dim crew_list As Crew_list

    Public Exchange As New PlanetExchange

    Public CapitalPoint As PointI

    Public type As planet_type_enum
    'ambient settings


    Public tech As HashSet(Of planet_tech_list_enum)
    Public special_tech As Planet_special_tech_enum

    Public crew_list As New Dictionary(Of Integer, Crew)()
    Public officer_list As New Dictionary(Of Integer, Officer)()

    Public Projectiles As HashSet(Of Projectile) = New HashSet(Of Projectile)




    Sub New(ByVal ID As Integer, ByVal type As planet_type_enum, ByVal size As PointI, ByVal orbit_point As Integer, ByVal orbit_distance As Integer, ByVal orbits_planet As Boolean, ByVal theta_offset As Double)
        Me.PlanetID = ID
        Me.type = type
        ReDim Me.tile_map(size.x, size.y)
        Me.size = size
        Me.orbit_point = orbit_point
        Me.orbit_distance = orbit_distance
        Me.orbits_planet = orbits_planet
        Me.theta = theta_offset

        'Add Block Map (Tells weather the block is built on or not)        
    End Sub

    Sub Set_Population()
        population = crew_list.Count - BuilderCount
        populationMax = 0
        For Each item In Building_List
            If item.Value.Type = building_type_enum.Apartment Then populationMax += 4
        Next
    End Sub

    Sub Build_Tile(ByVal T As Build_Tiles)
        tile_map(T.X, T.Y).sprite = T.Sprite
        tile_map(T.X, T.Y).sprite2 = T.Sprite2
        tile_map(T.X, T.Y).type = CType(T.Type, planet_tile_type_enum)
        tile_map(T.X, T.Y).walkable = CType(T.Walkable, walkable_type_enum)
    End Sub


    Sub Start_Building_Constuction(ByVal ID As Integer, ByVal OwnerID As Integer, ByVal Type As building_type_enum, ByVal Pos As PointI, ByVal Block_Type As Block_Return_Type_Enum)
        If Not Build_List.ContainsKey(ID) Then Build_List.Add(ID, New Build_List_Type)
        Pos = Pos * 32

        Dim Adjusted_Pos As PointI = Pos
        Select Case Block_Type
            Case Is = Block_Return_Type_Enum.WholeTile : Adjusted_Pos = Pos
            Case Is = Block_Return_Type_Enum.HorizontalTop : Adjusted_Pos = Pos
            Case Is = Block_Return_Type_Enum.HorizontalBot : Adjusted_Pos = Pos + New PointI(0, 16)
            Case Is = Block_Return_Type_Enum.VerticalL : Adjusted_Pos = Pos
            Case Is = Block_Return_Type_Enum.VerticalR : Adjusted_Pos = Pos + New PointI(16, 0)
            Case Is = Block_Return_Type_Enum.TopL : Adjusted_Pos = Pos
            Case Is = Block_Return_Type_Enum.TopR : Adjusted_Pos = Pos + New PointI(16, 0)
            Case Is = Block_Return_Type_Enum.BotL : Adjusted_Pos = Pos + New PointI(0, 16)
            Case Is = Block_Return_Type_Enum.BotR : Adjusted_Pos = Pos + New PointI(16, 16)
        End Select

        Dim tiles As HashSet(Of Build_Tiles) = Build_From_File(Type, Block_Type, Me, Adjusted_Pos, True)
        Build_List(ID).Build_Progress = tiles.Count
        Build_List(ID).Type = Type
        Build_List(ID).OwnerID = OwnerID
        Build_List(ID).Pos = Pos
        Build_List(ID).BlockType = Block_Type

        For Each item In tiles
            Build_List(ID).Tile_List.Add(New PointI(item.X + Adjusted_Pos.x, item.Y + Adjusted_Pos.y), 0)
        Next

    End Sub




    Sub Citizen_Build_Building(ByVal Owner As Integer)
        Dim Mine, Farm, Factory, Refinery, Pub, Special As Double

        Select Case officer_list(Owner).Personality.Dominant
            Case Is = Personality_Type_Type.Aggressive
                Mine += 6 : Farm += 1 : Factory += 4 : Refinery += 5 : Pub += 2 : Special += 3
            Case Is = Personality_Type_Type.Sly
                Mine += 2 : Farm += 1 : Factory += 4 : Refinery += 3 : Pub += 5 : Special += 6
            Case Is = Personality_Type_Type.Conqueror
                Mine += 3 : Farm += 1 : Factory += 6 : Refinery += 5 : Pub += 2 : Special += 4
            Case Is = Personality_Type_Type.Political
                Mine += 1 : Farm += 5 : Factory += 2 : Refinery += 3 : Pub += 4 : Special += 6
            Case Is = Personality_Type_Type.Militaristic
                Mine += 4 : Farm += 1 : Factory += 5 : Refinery += 6 : Pub += 2 : Special += 3
        End Select

        Mine *= 1.5 : Farm *= 1.5 : Factory *= 1.5 : Refinery *= 1.5 : Pub *= 1.5 : Special *= 1.5

        Select Case officer_list(Owner).Personality.Recessive
            Case Is = Personality_Type_Type.Aggressive
                Mine += 6 : Farm += 1 : Factory += 4 : Refinery += 5 : Pub += 2 : Special += 3
            Case Is = Personality_Type_Type.Sly
                Mine += 2 : Farm += 1 : Factory += 4 : Refinery += 3 : Pub += 5 : Special += 6
            Case Is = Personality_Type_Type.Conqueror
                Mine += 3 : Farm += 1 : Factory += 6 : Refinery += 5 : Pub += 2 : Special += 4
            Case Is = Personality_Type_Type.Political
                Mine += 1 : Farm += 5 : Factory += 2 : Refinery += 3 : Pub += 4 : Special += 6
            Case Is = Personality_Type_Type.Militaristic
                Mine += 4 : Farm += 1 : Factory += 5 : Refinery += 6 : Pub += 2 : Special += 3
        End Select
        Dim list As New Dictionary(Of Integer, Double)()
        Dim Highest As Integer = -1
        list.Add(-1, -1)

        If Building_Count.CMine < Building_Count.Mine AndAlso Building_Count.CMine < Resource_Points.Count Then list.Add(0, Mine)
        If Building_Count.CFarm < Building_Count.Farm AndAlso Building_Count.CFarm < Farm_Points.Count Then list.Add(1, Farm)
        If Building_Count.CFactory < Building_Count.Factory Then list.Add(2, Factory)
        If Building_Count.CRefinery < Building_Count.Refinery Then list.Add(3, Refinery)
        If Building_Count.CPub < Building_Count.Pub Then list.Add(4, Pub)

        ''If Building_Count.CSpecial < Special Then list.Add(5, Special) : Highest = 5

        For Each item In list
            If item.Value > list(Highest) Then Highest = item.Key
        Next

        Dim Return_Type As Block_Return_Type_Class
        Dim Id As Integer = GetEmptyBuildingID()
        Select Case Highest
            Case Is = -1

            Case Is = 0
                Return_Type = FindBestBuildingPos(building_type_enum.Mine)
                AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.CMine += 1
                Start_Building_Constuction(Id, Owner, building_type_enum.Mine, Return_Type.Pos, Return_Type.Type)

            Case Is = 1
                Return_Type = FindBestBuildingPos(building_type_enum.Farm)
                AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.CFarm += 1
                Start_Building_Constuction(Id, Owner, building_type_enum.Farm, Return_Type.Pos, Return_Type.Type)

            Case Is = 2
                Return_Type = FindBestBuildingPos(building_type_enum.Factory)
                AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.CFactory += 1
                Start_Building_Constuction(Id, Owner, building_type_enum.Factory, Return_Type.Pos, Return_Type.Type)

            Case Is = 3
                Return_Type = FindBestBuildingPos(building_type_enum.Refinery)
                AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.CRefinery += 1
                Start_Building_Constuction(Id, Owner, building_type_enum.Refinery, Return_Type.Pos, Return_Type.Type)

            Case Is = 4
                Return_Type = FindBestBuildingPos(building_type_enum.Pub)
                AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.CPub += 1
                Start_Building_Constuction(Id, Owner, building_type_enum.Pub, Return_Type.Pos, Return_Type.Type)

            Case Is = 5
        End Select


    End Sub




    Function FindBestBuildingPos(ByVal Building As building_type_enum) As Block_Return_Type_Class
        Dim Return_Type As New Block_Return_Type_Class(New PointI(0, 0), Block_Return_Type_Enum.None)
        Select Case Building
            Case Is = building_type_enum.Mine
                For Each item In Resource_Points
                    If item.Value = False Then Return_Type.Pos = item.Key : Exit For
                Next
                Resource_Points(Return_Type.Pos) = True
                Return_Type.Type = Block_Return_Type_Enum.WholeTile
                Return Return_Type

            Case Is = building_type_enum.Farm                
                For Each item In Farm_Points
                    If item.Value = False Then Return_Type.Pos = item.Key : Exit For
                Next
                Farm_Points(Return_Type.Pos) = True
                Return_Type.Type = Block_Return_Type_Enum.WholeTile
                Return Return_Type

            Case Is = building_type_enum.Factory
                Return FindEmptyTile(CapitalPoint, Block_Type_Enum.HorV)
            Case Is = building_type_enum.Refinery
                Return FindEmptyTile(CapitalPoint, Block_Type_Enum.HorV)
            Case Is = building_type_enum.Pub
                Return FindEmptyTile(CapitalPoint, Block_Type_Enum.HorV)
            Case Is = building_type_enum.Exchange
                Return FindEmptyTile(CapitalPoint, Block_Type_Enum.Large)
            Case Is = building_type_enum.Apartment
                Return FindEmptyTile(CapitalPoint, Block_Type_Enum.HorV)
            Case Is = building_type_enum.Bank
                Return FindEmptyTile(CapitalPoint, Block_Type_Enum.Large)
            Case Is = building_type_enum.Specials
                Return FindEmptyTile(CapitalPoint, Block_Type_Enum.Small)
        End Select

        Return Return_Type
    End Function


    Function FindEmptyTile(ByVal Position As PointI, ByVal Type As Block_Type_Enum) As Block_Return_Type_Class
        Dim pos As PointI
        For a = 1 To 8
            For y = -a To a
                For x = -a To a
                    pos.x = Position.x + x
                    pos.y = Position.y + y
                    If pos.x >= 0 AndAlso pos.x <= size.x AndAlso pos.y >= 0 AndAlso pos.y <= size.y Then
                        Dim Return_Type As Block_Return_Type_Enum = Block_Map.Get_Block(pos, Type)
                        If Not Return_Type = Block_Return_Type_Enum.None AndAlso Not Resource_Points.ContainsKey(pos) AndAlso Not Farm_Points.ContainsKey(pos) Then                            
                            Return New Block_Return_Type_Class(pos, Return_Type)
                        End If
                    End If
                Next
            Next
        Next
        Return New Block_Return_Type_Class(New PointI(0, 0), Block_Return_Type_Enum.None)
    End Function

    Function GetEmptyBuildingID() As Integer
        Dim ID As Integer
        For a = 0 To 100000
            If Not Build_List.ContainsKey(a) AndAlso Not Building_List.ContainsKey(a) Then ID = a : Exit For
        Next
        Return ID
    End Function

    Function GetEmptyCrewID() As Integer
        Dim ID As Integer
        For a = 0 To 100000
            If Not crew_list.ContainsKey(a) Then ID = a : Exit For
        Next
        Return ID
    End Function


    Sub MoveOfficer(ByVal Id As Integer, ByRef vector As PointD)
        Dim Pos(3) As PointD
        vector.x *= officer_list(Id).speed
        vector.y *= officer_list(Id).speed

        '0:7,1
        '0:24,1
        '0:7,31
        '0:24,31

        'top left
        Pos(0).x = Math.Floor((officer_list(Id).GetLocation.x + 8 + vector.x) * 0.03125)
        Pos(0).y = Math.Floor((officer_list(Id).GetLocation.y + 27 + vector.y) * 0.03125)
        'top right
        Pos(1).x = Math.Floor((officer_list(Id).GetLocation.x + 23 + vector.x) * 0.03125)
        Pos(1).y = Math.Floor((officer_list(Id).GetLocation.y + 27 + vector.y) * 0.03125)
        'bottom left
        Pos(2).x = Math.Floor((officer_list(Id).GetLocation.x + 8 + vector.x) * 0.03125)
        Pos(2).y = Math.Floor((officer_list(Id).GetLocation.y + 31 + vector.y) * 0.03125)
        'bottom right
        Pos(3).x = Math.Floor((officer_list(Id).GetLocation.x + 23 + vector.x) * 0.03125)
        Pos(3).y = Math.Floor((officer_list(Id).GetLocation.y + 31 + vector.y) * 0.03125)

        Dim b As Integer = 0
        For a = 0 To 3
            If Pos(a).x >= 0 And Pos(a).x <= size.x And Pos(a).y >= 0 And Pos(a).y <= size.y Then
                If tile_map(Convert.ToInt32(Pos(a).x), Convert.ToInt32(Pos(a).y)).walkable = walkable_type_enum.Walkable Then
                    b = b + 1
                End If
            End If
        Next
        If b = 4 Then
            officer_list(Id).Move(vector)
        ElseIf Not b = 0 Then
            If vector.x < 0 Then officer_list(Id).SetLocation(New PointD(Convert.ToInt32(Pos(0).x * 32 + 32 - 8), officer_list(Id).GetLocation.y))
            If vector.x > 0 Then officer_list(Id).SetLocation(New PointD(Convert.ToInt32(Pos(1).x * 32 - 32 + 8), officer_list(Id).GetLocation.y))
            If vector.y < 0 Then officer_list(Id).SetLocation(New PointD(officer_list(Id).GetLocation.x, Convert.ToInt32(Pos(0).y * 32 + 32 - 27)))
            If vector.y > 0 Then officer_list(Id).SetLocation(New PointD(officer_list(Id).GetLocation.x, Convert.ToInt32(Pos(2).y * 32 - 32)))
        End If
    End Sub



    Sub populate()
        populate_planet(Me)
        Load_Pathfinding()
    End Sub


    Public Sub DoEvents()
        Process_crew()
        Process_Work()

        If GSTFrequency = 0 Then Process_Planet()
        If GSTFrequency = 0 Then Process_Citizens()

        Update_Officers()

        Handle_Projectiles()
        run_crew_scrips(crew_list)
        Run_animations()

    End Sub


    Public Sub DoPassiveEvents()





    End Sub


    Sub Process_Citizens()
        If GST = 1500 OrElse GST = 0 Then


            For Each Cit In officer_list
                Dim BuildBuilding As Boolean = False
                If Cit.Value.Citizen_Rights.Contains(PlanetID) Then
                    If Cit.Value.Owned_Buildings.ContainsKey(PlanetID) Then
                        If Cit.Value.Owned_Buildings(PlanetID).Count < Building_Count.CitizensBuildingLimit Then
                            BuildBuilding = True
                        End If
                    Else
                        Cit.Value.Owned_Buildings.Add(PlanetID, New HashSet(Of Integer))
                        BuildBuilding = True
                    End If
                End If

                If BuildBuilding = True AndAlso Building_Count.Building_Available > 0 Then

                    If Cit.Value.Item_List.ContainsKey(Item_Enum.Refined_Crystal_Piece) Then
                        If Cit.Value.Item_List(Item_Enum.Refined_Crystal_Piece) >= 10000 Then
                            Cit.Value.Item_List(Item_Enum.Refined_Crystal_Piece) -= 10000
                            Citizen_Build_Building(Cit.Key)
                        End If
                    End If

                End If

            Next
        End If
    End Sub


    Sub Check_Citizen()
        If Citizens < CitizensMax Then
            Dim Id As Integer
            For Id = 0 To 10000
                If Not u.Officer_List.ContainsKey(Id) Then Exit For
            Next
            Add_Officer(Id, New Officer(0, Id.ToString, Officer_location_enum.Planet, 0, New PointD(Building_List(0).PickupPoint.x * 32, Building_List(0).PickupPoint.y * 32), 1, 1, New Officer.sprite_list(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head)))
            u.Officer_List(Id).Item_List.Add(Item_Enum.Refined_Crystal_Piece, 10000)
            u.Officer_List(Id).Citizen_Rights.Add(PlanetID)
            Citizens += 1
        End If
    End Sub


    Sub Check_Workers()
        Dim count = 0

        If Free_Job_List.Count > 0 AndAlso population < populationMax AndAlso Free_Housing_List.Count > 0 Then
            If Free_Job_List_Count > Free_Job_List.Count - 1 Then Free_Job_List_Count = 0
            count = 0
            'Dim Slots As New KeyValuePair(Of Integer, HashSet(Of Job_List_Type))()
            Dim Id As Integer
            For Each item In Free_Job_List
                If count = Free_Job_List_Count Then Id = item.Key : Exit For
                count += 1
            Next



            Dim CrewID As Integer = GetEmptyCrewID()
            Dim SpawnPoint = New PointD(Building_List(0).PickupPoint.x * 32, Building_List(0).PickupPoint.y * 32)
            Building_List(Id).Work_Slots.Slots(Free_Job_List(Id).First.SlotID).ID = CrewID            
            'CrewAdd
            crew_list.Add(CrewID, New Crew(0, SpawnPoint, 0, Officer_location_enum.Planet, 10, character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New crew_resource_type(0, 0)))
            crew_list(CrewID).Worker_Type = Free_Job_List(Id).First.Type
            crew_list(CrewID).HomeBuilding = Free_Housing_List.First.BuildingID
            crew_list(CrewID).HomeSpace = Free_Housing_List.First.SlotID
            crew_list(CrewID).WorkShift = Work_Shift_Enum.Night  'CType(random(0, 2), Work_Shift_Enum)
            crew_list(CrewID).PubBuilding = -1
            crew_list(CrewID).WorkBuilding = Id
            'Remove House listing
            Building_List(Free_Housing_List.First.BuildingID).Tennent_Slots.Slots(Free_Housing_List.First.SlotID).CrewID = CrewID
            Free_Job_List_Count += 1
        End If


        'RebuildJobList / Housing List
        Free_Job_List.Clear()
        For Each Item In Building_List            
            count = 0
            For Each slot In Item.Value.Work_Slots.Slots
                If slot.ID = -1 Then
                    If Not Free_Job_List.ContainsKey(Item.Key) Then Free_Job_List.Add(Item.Key, New HashSet(Of Job_List_Type))
                    Free_Job_List(Item.Key).Add(New Job_List_Type(slot.Type, count))
                End If
                count += 1
            Next
        Next

        Free_Housing_List.Clear()
        For Each Item In Building_List
            For Each slot In Item.Value.Tennent_Slots.Slots
                If slot.CrewID = -1 Then
                    Free_Housing_List.Add(New Housing_List_Type(slot.SlotID, Item.Key))
                End If
            Next
        Next


    End Sub

    Sub Check_Builders()
        If BuilderSpawnCounter <= 0 AndAlso BuilderCount < 4 Then
            BuilderCount += 1
            Dim Id As Integer = GetEmptyCrewID()
            crew_list.Add(Id, New Crew(0, New PointD((CapitalPoint.x * 32 + 15) * 32, (CapitalPoint.y * 32 + 15) * 32), 0, Officer_location_enum.Planet, 10, character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head, New crew_resource_type(0, 0)))
            crew_list(Id).Worker_Type = Worker_Type_Enum.Builder
            Builder_List.Add(Id, -1)
        End If
        If BuilderSpawnCounter > 0 Then BuilderSpawnCounter -= 1


        'Assign new builders
        Dim FreeBuilders As New HashSet(Of Integer)()
        'Get builder count
        For Each builder In Builder_List
            If builder.Value = -1 Then FreeBuilders.Add(builder.Key)
        Next

        If FreeBuilders.Count > 0 Then
            Dim BuildID As Integer = -1
            For Each Item In Build_List
                If Item.Value.Compleated = False AndAlso Item.Value.Builders = 0 Then
                    BuildID = Item.Key
                    Exit For
                End If
            Next

            If BuildID = -1 Then
                For Each Item In Build_List
                    If Item.Value.Compleated = False AndAlso Item.Value.Builders = 1 Then
                        BuildID = Item.Key
                        Exit For
                    End If
                Next
            End If

            If BuildID > -1 Then

                Dim crew As Crew = crew_list(FreeBuilders.First)
                Dim Last As PointI = crew.find_tile()
                Dim tile_time As Integer = CInt(500 / Build_List(BuildID).Tile_List.Count)
                crew.command_queue.Clear()

                For Each Tile In If((Build_List(BuildID).Builders Mod 2) = 0, Build_List(BuildID).Tile_List, Build_List(BuildID).Tile_List.Reverse)
                    MoveCrewTo(Last, New PointI(Tile.Key.x, Tile.Key.y), crew)
                    Last = New PointI(Tile.Key.x, Tile.Key.y)
                    crew.command_queue.Enqueue(New Crew.Command_Builder_Build_Tile(tile_time, BuildID, Tile.Key))
                Next
                crew.command_queue.Enqueue(New Crew.Command_Builder_Start_Work())
                Builder_List(FreeBuilders.First) = BuildID
                Build_List(BuildID).Builders += 1
            End If

        End If
    End Sub

    Sub Process_Planet()
        Set_Population()
        Check_Citizen()
        Check_Workers()
        Check_Builders()
        
        'Remove_Compleated_Buildings
        Dim Remove_List As New HashSet(Of Integer)()
        For Each Item In Build_List
            If Item.Value.Compleated = True Then
                PlanetGenerator.Build_Building(Item.Value.Type, Item.Key, Item.Value.OwnerID, Me, New Block_Return_Type_Class(Item.Value.Pos \ 32, Item.Value.BlockType))
                Remove_List.Add(Item.Key)
            End If
        Next
        For Each Item In Remove_List
            Build_List.Remove(Item)
        Next

    End Sub






    Sub Process_Work()
        If GSTFrequency = 0 AndAlso GST Mod 10 = 0 Then
            For Each buiding In Building_List
                'Mine
                If buiding.Value.Type = building_type_enum.Mine Then Process_Mine(buiding)
                If buiding.Value.Type = building_type_enum.Refinery Then Process_Refinery(buiding)
                If buiding.Value.Type = building_type_enum.Factory Then Process_Factory(buiding)
            Next
        End If
    End Sub



    Sub Process_Mine(ByVal Building As KeyValuePair(Of Integer, Planet_Building))

        Dim Safe_Point As Item_Point_Type = Nothing
        Dim Safe_Location As PointI

        For Each ipoint In Building.Value.Item_Slots
            If ipoint.Value.Input_Slot = True AndAlso Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Item = Item_Enum.Refined_Crystal_Piece Then Safe_Point = Item_Point(ipoint.Key) : Safe_Location = ipoint.Key : Exit For
        Next

        'Need better safty
        If Safe_Point Is Nothing Then Exit Sub

        Dim Free_Slots As Double
        For Each ipoint In Building.Value.Item_Slots
            If ipoint.Value.Input_Slot = False Then
                If Item_Point.ContainsKey(ipoint.Key) Then
                    If Item_Point(ipoint.Key).Item = Item_Enum.Crystal AndAlso Item_Point(ipoint.Key).Amount < 100 Then
                        Free_Slots += 100 - Item_Point(ipoint.Key).Amount
                    End If
                Else
                    Free_Slots += 100
                End If
            End If
        Next


        'Pay workers/Remove resources

        For Each CrewID In Building.Value.Working_crew_list
            Dim processed As Boolean = False
            If Free_Slots >= 1.5 Then
                For Each ipoint In Building.Value.Item_Slots
                    If ipoint.Value.Input_Slot = True AndAlso Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Item = Item_Enum.Parts Then
                        If Item_Point(ipoint.Key).Amount >= 1 Then
                            If Safe_Point.Amount >= 1 Then
                                'Safe_Point.Amount -= 1                                
                                Building.Value.Item_Build_Progress += 150
                                Free_Slots -= 1.5
                                Item_Point(ipoint.Key).Amount -= 1
                                If Item_Point(ipoint.Key).Amount = 0 Then Item_Point.Remove(ipoint.Key)
                            End If
                            processed = True
                            Exit For
                        End If
                    End If
                Next
                If processed = False Then
                    If Safe_Point.Amount >= 1 Then
                        'Safe_Point.Amount -= 1                        
                        Building.Value.Item_Build_Progress += 30
                        Free_Slots -= 0.3
                    End If
                End If

            End If
        Next


        'Add crystal        
        Do Until Building.Value.Item_Build_Progress < 100
            For Each ipoint In Building.Value.Item_Slots

                If ipoint.Value.Input_Slot = False Then
                    If Not Item_Point.ContainsKey(ipoint.Key) Then Item_Point.Add(ipoint.Key, New Item_Point_Type)
                    If (Item_Point(ipoint.Key).Item = Item_Enum.None OrElse Item_Point(ipoint.Key).Item = Item_Enum.Crystal) Then
                        If Item_Point(ipoint.Key).Amount < 100 Then
                            Item_Point(ipoint.Key).Item = Item_Enum.Crystal
                            Item_Point(ipoint.Key).Amount += 1
                            Exit For
                        End If
                    End If
                End If

            Next
            Building.Value.Item_Build_Progress -= 100
        Loop



        'Process Transporters Buy and Sell
        Dim Parts As Integer = Check_Resources(Building.Key, Item_Enum.Parts)
        Dim Crystal As Integer = Check_Resources(Building.Key, Item_Enum.Crystal)

        'Need to sell
        Dim NeedToSell As Boolean = False
        Dim NeedToBuy As Boolean = False

        If Crystal >= 100 Then NeedToSell = True
        If Parts <= 400 AndAlso Safe_Point.Amount > 500 Then NeedToBuy = True

        Dim Buy_amount As Integer = 800 - Parts '800 is max hold amount
        Dim Can_Affort As Integer = (Safe_Point.Amount - 500) \ 10 '10 is part cost
        If Can_Affort < Buy_amount Then Buy_amount = Can_Affort

        If NeedToSell = True AndAlso NeedToBuy = True Then
            If Building.Value.Available_Transporters.Count > 0 AndAlso Buy_amount >= 0 Then
                Dim Id As Integer = GetNearistBuilding(building_type_enum.Exchange, Safe_Location)
                Dim Avilable As Integer = Exchange.Check_Exchange(Item_Enum.Parts)
                If Avilable < Buy_amount Then Buy_amount = Avilable
                If Id > -1 Then
                    If Buy_amount > 100 Then
                        Send_Transporter_BuySell(crew_list(Building.Value.Available_Transporters.First), Item_Enum.Crystal, (Crystal \ 100) * 100, Buy_amount * 10, Item_Enum.Parts, Buy_amount, Safe_Location, Building_List(Id).PickupPoint, Building.Value.PickupPoint, Building.Key)
                        Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                    Else
                        Send_Transporter_Sell(crew_list(Building.Value.Available_Transporters.First), Item_Enum.Crystal, (Crystal \ 100) * 100, Building_List(Id).PickupPoint, Safe_Location, Building.Value.PickupPoint, Building.Key)
                        Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                    End If
                End If
            End If
        ElseIf NeedToSell = True Then
            If Building.Value.Available_Transporters.Count > 0 Then
                Dim Id As Integer = GetNearistBuilding(building_type_enum.Exchange, Safe_Location)
                If Id > -1 Then
                    Send_Transporter_Sell(crew_list(Building.Value.Available_Transporters.First), Item_Enum.Crystal, (Crystal \ 100) * 100, Building_List(Id).PickupPoint, Safe_Location, Building.Value.PickupPoint, Building.Key)
                    Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                End If
            End If
        ElseIf NeedToBuy = True Then
            If Building.Value.Available_Transporters.Count > 0 AndAlso Buy_amount >= 0 Then
                Dim Id As Integer = GetNearistBuilding(building_type_enum.Exchange, Safe_Location)
                Dim Avilable As Integer = Exchange.Check_Exchange(Item_Enum.Parts)
                If Avilable < Buy_amount Then Buy_amount = Avilable
                If Id > -1 AndAlso Buy_amount > 100 Then
                    Send_Transporter_Buy(crew_list(Building.Value.Available_Transporters.First), Buy_amount * 10, Item_Enum.Parts, Buy_amount, Safe_Location, Building_List(Id).PickupPoint, Building.Value.PickupPoint, Building.Key)
                    Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                End If
            End If
        End If


    End Sub


    Sub Process_Refinery(ByVal Building As KeyValuePair(Of Integer, Planet_Building))

        Dim Safe_Point As Item_Point_Type = Nothing
        Dim Safe_Location As PointI

        For Each ipoint In Building.Value.Item_Slots
            If ipoint.Value.Input_Slot = True AndAlso Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Item = Item_Enum.Refined_Crystal_Piece Then Safe_Point = Item_Point(ipoint.Key) : Safe_Location = ipoint.Key : Exit For
        Next




        'Need better safty
        If Safe_Point Is Nothing Then Exit Sub


        Dim Free_Slots As Double
        For Each ipoint In Building.Value.Item_Slots
            If ipoint.Value.Input_Slot = False Then
                If Item_Point.ContainsKey(ipoint.Key) Then
                    If Item_Point(ipoint.Key).Item = Item_Enum.Refined_Crystal AndAlso Item_Point(ipoint.Key).Amount < 100 Then
                        Free_Slots += 100 - Item_Point(ipoint.Key).Amount
                    End If
                Else
                    Free_Slots += 100
                End If
            End If
        Next

        'Convert Refined Crystal to Refined Crystal Peices
        'If Safe_Point.Amount < 1500 Then
        'For Each ipoint In Building.Value.Item_Slots
        'If Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Item = Item_Enum.Refined_Crystal Then
        'For a = 1 To 10
        'If Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Amount >= 1 Then
        'Item_Point(ipoint.Key).Amount -= 1
        'If Item_Point(ipoint.Key).Amount = 0 Then Item_Point.Remove(ipoint.Key)
        'Safe_Point.Amount += 10
        'End If
        'Next a
        'If Safe_Point.Amount >= 100 Then Exit For
        'End If
        'Next
        'End If

        'Pay workers/Remove resources
        For Each CrewID In Building.Value.Working_crew_list
            If Free_Slots >= 1 Then
                For Each ipoint In Building.Value.Item_Slots
                    If (ipoint.Value.Input_Slot = True AndAlso Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Item = Item_Enum.Crystal) OrElse Building.Value.Resource_Credit >= 1 Then
                        If Building.Value.Resource_Credit >= 1 OrElse Item_Point(ipoint.Key).Amount >= 1 Then

                            If Safe_Point.Amount >= 1 Then
                                'Safe_Point.Amount -= 1
                                Building.Value.Item_Build_Progress += 100
                                Free_Slots -= 1
                                If Building.Value.Resource_Credit >= 1 Then
                                    Building.Value.Resource_Credit -= 1
                                Else
                                    Item_Point(ipoint.Key).Amount -= 1
                                    Building.Value.Resource_Credit += 1
                                    If Item_Point(ipoint.Key).Amount = 0 Then Item_Point.Remove(ipoint.Key)
                                End If
                            End If
                            Exit For

                        End If
                    End If
                Next
            End If
        Next


        'Add crystal
        Do Until Building.Value.Item_Build_Progress < 100
            For Each ipoint In Building.Value.Item_Slots

                If ipoint.Value.Input_Slot = False Then
                    If Not Item_Point.ContainsKey(ipoint.Key) Then Item_Point.Add(ipoint.Key, New Item_Point_Type)
                    If (Item_Point(ipoint.Key).Item = Item_Enum.None OrElse Item_Point(ipoint.Key).Item = Item_Enum.Refined_Crystal) Then
                        If Item_Point(ipoint.Key).Amount < 100 Then
                            Item_Point(ipoint.Key).Item = Item_Enum.Refined_Crystal
                            Item_Point(ipoint.Key).Amount += 1
                            Exit For
                        End If
                    End If
                End If

            Next
            Building.Value.Item_Build_Progress -= 100
        Loop



        'Process Transporters Buy and Sell
        Dim Crystal As Integer = Check_Resources(Building.Key, Item_Enum.Crystal)
        Dim RefinedCrystal As Integer = Check_Resources(Building.Key, Item_Enum.Refined_Crystal)


        'Need to sell
        Dim NeedToSell As Boolean = False
        Dim NeedToBuy As Boolean = False

        If RefinedCrystal >= 100 Then NeedToSell = True
        If Crystal <= 400 AndAlso Safe_Point.Amount > 500 Then NeedToBuy = True

        Dim Buy_amount As Integer = 800 - Crystal '800 is max hold amount
        Dim Can_Affort As Integer = (Safe_Point.Amount - 500) \ 10 '10 is part cost
        If Can_Affort < Buy_amount Then Buy_amount = Can_Affort

        If NeedToSell = True AndAlso NeedToBuy = True Then
            If Building.Value.Available_Transporters.Count > 0 AndAlso Buy_amount >= 0 Then
                Dim Id As Integer = GetNearistBuilding(building_type_enum.Exchange, Safe_Location)
                Dim Avilable As Integer = Exchange.Check_Exchange(Item_Enum.Crystal)
                If Avilable < Buy_amount Then Buy_amount = Avilable
                If Id > -1 Then
                    If Buy_amount > 100 Then
                        Send_Transporter_BuySell(crew_list(Building.Value.Available_Transporters.First), Item_Enum.Refined_Crystal, (RefinedCrystal \ 100) * 100, Buy_amount * 10, Item_Enum.Crystal, Buy_amount, Safe_Location, Building_List(Id).PickupPoint, Building.Value.PickupPoint, Building.Key)
                        Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                    Else
                        Send_Transporter_Sell(crew_list(Building.Value.Available_Transporters.First), Item_Enum.Refined_Crystal, (RefinedCrystal \ 100) * 100, Building_List(Id).PickupPoint, Safe_Location, Building.Value.PickupPoint, Building.Key)
                        Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                    End If
                End If
            End If
        ElseIf NeedToSell = True Then
            If Building.Value.Available_Transporters.Count > 0 Then
                Dim Id As Integer = GetNearistBuilding(building_type_enum.Exchange, Safe_Location)
                If Id > -1 Then
                    Send_Transporter_Sell(crew_list(Building.Value.Available_Transporters.First), Item_Enum.Refined_Crystal, (RefinedCrystal \ 100) * 100, Building_List(Id).PickupPoint, Safe_Location, Building.Value.PickupPoint, Building.Key)
                    Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                End If
            End If
        ElseIf NeedToBuy = True Then
            If Building.Value.Available_Transporters.Count > 0 AndAlso Buy_amount >= 0 Then
                Dim Id As Integer = GetNearistBuilding(building_type_enum.Exchange, Safe_Location)
                Dim Avilable As Integer = Exchange.Check_Exchange(Item_Enum.Crystal)
                If Avilable < Buy_amount Then Buy_amount = Avilable
                If Id > -1 AndAlso Buy_amount > 100 Then
                    Send_Transporter_Buy(crew_list(Building.Value.Available_Transporters.First), Buy_amount * 10, Item_Enum.Crystal, Buy_amount, Safe_Location, Building_List(Id).PickupPoint, Building.Value.PickupPoint, Building.Key)
                    Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                End If
            End If
        End If




    End Sub


    Sub Process_Factory(ByVal Building As KeyValuePair(Of Integer, Planet_Building))

        Dim Safe_Point As Item_Point_Type = Nothing
        Dim Safe_Location As PointI

        For Each ipoint In Building.Value.Item_Slots
            If ipoint.Value.Input_Slot = True AndAlso Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Item = Item_Enum.Refined_Crystal_Piece Then Safe_Point = Item_Point(ipoint.Key) : Safe_Location = ipoint.Key : Exit For
        Next


        'Need better safty
        If Safe_Point Is Nothing Then Exit Sub


        Dim Free_Slots As Double
        For Each ipoint In Building.Value.Item_Slots
            If ipoint.Value.Input_Slot = False Then
                If Item_Point.ContainsKey(ipoint.Key) Then
                    If Item_Point(ipoint.Key).Item = Item_Enum.Parts AndAlso Item_Point(ipoint.Key).Amount < 100 Then
                        Free_Slots += 100 - Item_Point(ipoint.Key).Amount
                    End If
                Else
                    Free_Slots += 100
                End If
            End If
        Next

        'Pay workers/Remove resources
        For Each CrewID In Building.Value.Working_crew_list
            If Free_Slots >= 1 Then
                For Each ipoint In Building.Value.Item_Slots
                    If (ipoint.Value.Input_Slot = True AndAlso Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Item = Item_Enum.Refined_Crystal) OrElse Building.Value.Resource_Credit >= 1 Then
                        If Building.Value.Resource_Credit >= 1 OrElse Item_Point(ipoint.Key).Amount >= 1 Then

                            If Safe_Point.Amount >= 1 Then
                                'Safe_Point.Amount -= 1
                                Building.Value.Item_Build_Progress += 100
                                Free_Slots -= 1
                                If Building.Value.Resource_Credit >= 1 Then
                                    Building.Value.Resource_Credit -= 1
                                Else
                                    Item_Point(ipoint.Key).Amount -= 1
                                    Building.Value.Resource_Credit += 1
                                    If Item_Point(ipoint.Key).Amount = 0 Then Item_Point.Remove(ipoint.Key)
                                End If
                            End If
                            Exit For

                        End If
                    End If
                Next
            End If
        Next


        'Add crystal
        Do Until Building.Value.Item_Build_Progress < 100
            For Each ipoint In Building.Value.Item_Slots

                If ipoint.Value.Input_Slot = False Then
                    If Not Item_Point.ContainsKey(ipoint.Key) Then Item_Point.Add(ipoint.Key, New Item_Point_Type)
                    If (Item_Point(ipoint.Key).Item = Item_Enum.None OrElse Item_Point(ipoint.Key).Item = Item_Enum.Parts) Then
                        If Item_Point(ipoint.Key).Amount < 100 Then
                            Item_Point(ipoint.Key).Item = Item_Enum.Parts
                            Item_Point(ipoint.Key).Amount += 1
                            Exit For
                        End If
                    End If
                End If

            Next
            Building.Value.Item_Build_Progress -= 100
        Loop


        'Process Transporters Buy and Sell
        Dim RefinedCrystal As Integer = Check_Resources(Building.Key, Item_Enum.Refined_Crystal)
        Dim Parts As Integer = Check_Resources(Building.Key, Item_Enum.Parts)



        'Need to sell
        Dim NeedToSell As Boolean = False
        Dim NeedToBuy As Boolean = False

        If Parts >= 100 Then NeedToSell = True
        If RefinedCrystal <= 400 AndAlso Safe_Point.Amount > 500 Then NeedToBuy = True

        Dim Buy_amount As Integer = 800 - RefinedCrystal '800 is max hold amount
        Dim Can_Affort As Integer = (Safe_Point.Amount - 500) \ 10 '10 is part cost
        If Can_Affort < Buy_amount Then Buy_amount = Can_Affort

        If NeedToSell = True AndAlso NeedToBuy = True Then
            If Building.Value.Available_Transporters.Count > 0 AndAlso Buy_amount >= 0 Then
                Dim Id As Integer = GetNearistBuilding(building_type_enum.Exchange, Safe_Location)
                Dim Avilable As Integer = Exchange.Check_Exchange(Item_Enum.Refined_Crystal)
                If Avilable < Buy_amount Then Buy_amount = Avilable
                If Id > -1 Then
                    If Buy_amount > 100 Then
                        Send_Transporter_BuySell(crew_list(Building.Value.Available_Transporters.First), Item_Enum.Parts, (Parts \ 100) * 100, Buy_amount * 10, Item_Enum.Refined_Crystal, Buy_amount, Safe_Location, Building_List(Id).PickupPoint, Building.Value.PickupPoint, Building.Key)
                        Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                    Else
                        Send_Transporter_Sell(crew_list(Building.Value.Available_Transporters.First), Item_Enum.Parts, (Parts \ 100) * 100, Building_List(Id).PickupPoint, Safe_Location, Building.Value.PickupPoint, Building.Key)
                        Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                    End If
                End If
            End If
        ElseIf NeedToSell = True Then
            If Building.Value.Available_Transporters.Count > 0 Then
                Dim Id As Integer = GetNearistBuilding(building_type_enum.Exchange, Safe_Location)
                If Id > -1 Then
                    Send_Transporter_Sell(crew_list(Building.Value.Available_Transporters.First), Item_Enum.Parts, (Parts \ 100) * 100, Building_List(Id).PickupPoint, Safe_Location, Building.Value.PickupPoint, Building.Key)
                    Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                End If
            End If
        ElseIf NeedToBuy = True Then
            If Building.Value.Available_Transporters.Count > 0 AndAlso Buy_amount >= 0 Then
                Dim Id As Integer = GetNearistBuilding(building_type_enum.Exchange, Safe_Location)
                Dim Avilable As Integer = Exchange.Check_Exchange(Item_Enum.Refined_Crystal)
                If Avilable < Buy_amount Then Buy_amount = Avilable
                If Id > -1 AndAlso Buy_amount > 100 Then
                    Send_Transporter_Buy(crew_list(Building.Value.Available_Transporters.First), Buy_amount * 10, Item_Enum.Refined_Crystal, Buy_amount, Safe_Location, Building_List(Id).PickupPoint, Building.Value.PickupPoint, Building.Key)
                    Building.Value.Available_Transporters.Remove(Building.Value.Available_Transporters.First)
                End If
            End If
        End If


    End Sub



    Function GetNearistBuilding(ByVal Building_Type As building_type_enum, ByVal Point As PointI) As Integer
        Dim list As New Dictionary(Of Integer, Integer)()
        For Each building In Building_List
            If building.Value.Type = Building_Type Then list.Add(building.Key, CInt(Math.Sqrt(((building.Value.PickupPoint.x - Point.x) ^ 2) + (building.Value.PickupPoint.y - Point.y) ^ 2)))
        Next

        Dim Lowest As Integer = 1000000
        Dim ID As Integer
        For Each item In list
            If item.Value < Lowest Then Lowest = item.Value : ID = item.Key
        Next
        If Building_List.Count = 0 Then Return -1
        Return ID
    End Function


    Function Check_Resources(ByVal BuildingID As Integer, ByVal Item As Item_Enum) As Integer
        Dim amount As Integer
        If Building_List.ContainsKey(BuildingID) Then

            Dim B As Planet_Building = Building_List(BuildingID)

            For Each ipoint In B.Item_Slots
                If Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Item = Item Then
                    amount += Item_Point(ipoint.Key).Amount
                End If
            Next
            Return amount

        End If
        Return 0
    End Function


    Sub Process_crew()
        If GSTFrequency = 0 Then
            'Send Crew to work
            'Send Crew to bank if needed
            'Send Crew to Pub
            'Send Crew home
            'al la repeto


            'Send to work
            If GST = 800 Then
                For Each Crew In crew_list
                    If Crew.Value.WorkBuilding > -1 AndAlso Crew.Value.WorkShift = Work_Shift_Enum.Morning Then
                        Send_List.Add(Crew, Send_work_List_Enum.Work)
                    End If
                Next
            ElseIf GST = 1800 Then
                For Each Crew In crew_list
                    If Crew.Value.WorkBuilding > -1 AndAlso Crew.Value.WorkShift = Work_Shift_Enum.Mid Then
                        Send_List.Add(Crew, Send_work_List_Enum.Work)
                    End If
                Next
            ElseIf GST = 2800 Then
                For Each Crew In crew_list
                    If Crew.Value.WorkBuilding > -1 AndAlso Crew.Value.WorkShift = Work_Shift_Enum.Night Then
                        Send_List.Add(Crew, Send_work_List_Enum.Work)
                    End If
                Next
            End If

            'Send crew home
            If GST = 1000 Then
                For Each Crew In crew_list
                    If Crew.Value.WorkBuilding > -1 AndAlso Crew.Value.WorkShift = Work_Shift_Enum.Night Then
                        Send_List.Add(Crew, Send_work_List_Enum.Home)
                    End If
                Next
            ElseIf GST = 2000 Then
                For Each Crew In crew_list
                    If Crew.Value.WorkBuilding > -1 AndAlso Crew.Value.WorkShift = Work_Shift_Enum.Morning Then
                        Send_List.Add(Crew, Send_work_List_Enum.Home)
                    End If
                Next
            ElseIf GST = 3000 Then
                For Each Crew In crew_list
                    If Crew.Value.WorkBuilding > -1 AndAlso Crew.Value.WorkShift = Work_Shift_Enum.Mid Then
                        Send_List.Add(Crew, Send_work_List_Enum.Home)
                    End If
                Next
            End If


            If Send_List.Count > 0 Then

                Select Case Send_List.First.Value

                    Case Is = Send_work_List_Enum.Work
                        Send_Crew_ToWork(Send_List.First.Key)
                        Send_List.Remove(Send_List.First.Key)

                    Case Is = Send_work_List_Enum.Home
                        Send_Crew_Home(Send_List.First.Key)
                        Send_List.Remove(Send_List.First.Key)

                End Select

            End If

        End If

    End Sub





    Sub Send_Crew_ToWork(ByVal Crew As KeyValuePair(Of Integer, Crew))
        Dim work_Point As PointI
        Dim Found_AP As Boolean = False

        'Not already working/going to work
        If Not Building_List(Crew.Value.WorkBuilding).Working_crew_list.Contains(Crew.Key) AndAlso Not Building_List(Crew.Value.WorkBuilding).Assigned_crew_list.Contains(Crew.Key) Then
            If Crew.Value.WorkBuilding > -1 Then


                'Worker
                If Crew.Value.Worker_Type = Worker_Type_Enum.Worker Then
                    'Find AP in building
                    For Each AP In Building_List(Crew.Value.WorkBuilding).access_point
                        If AP.Value.Type = BAP_Type.Worker AndAlso AP.Value.Used = False AndAlso AP.Value.NextUp = False Then
                            work_Point = AP.Key
                            Found_AP = True
                            Exit For
                        End If
                    Next

                    If Found_AP = False Then
                        For Each AP In Building_List(Crew.Value.WorkBuilding).access_point
                            If AP.Value.Type = BAP_Type.Worker AndAlso AP.Value.NextUp = False Then
                                work_Point = AP.Key
                                Found_AP = True
                                Exit For
                            End If
                        Next
                    End If


                    'If Open access point is found send crew to work
                    If Found_AP = True Then
                        Found_AP = False
                        'Set NextUp to work to true
                        Building_List(Crew.Value.WorkBuilding).access_point(work_Point).NextUp = True
                        'Pathfind

                        'If Not tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit For

                        Dim tile As PointI = Crew.Value.find_tile


                        path_find.Standard_Pathfind(tile, work_Point)
                        If path_find.Found_State = Pathfind.Pathfind.PFState.Found Then

                            If Crew.Value.command_queue.Any Then
                                Crew.Value.command_queue.Clear()
                            End If
                            Building_List(Crew.Value.WorkBuilding).Assigned_crew_list.Add(Crew.Key)
                            Dim list As LinkedList(Of PointI)
                            list = path_find.Path

                            'Need to add wait for finish
                            Crew.Value.command_queue.Clear()
                            For Each dest In list
                                If dest = work_Point Then
                                    Crew.Value.command_queue.Enqueue(New Crew.Command_Try_Work(work_Point))
                                End If
                                Crew.Value.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
                            Next
                            Crew.Value.command_queue.Enqueue(New Crew.Command_Start_Work(work_Point))
                        End If

                    End If
                End If






                If Crew.Value.Worker_Type = Worker_Type_Enum.Transporter Then

                    Dim point As PointI = Building_List(Crew.Value.WorkBuilding).PickupPoint
                    'If Not tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit For


                    Dim tile As PointI = Crew.Value.find_tile

                    path_find.Standard_Pathfind(tile, point)

                    If path_find.Found_State = Pathfind.Pathfind.PFState.Found Then
                        Dim list As LinkedList(Of PointI)
                        list = path_find.Path

                        'Need to add wait for finish                                
                        For Each dest In list
                            Crew.Value.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
                        Next
                        Crew.Value.command_queue.Enqueue(New Crew.Command_Trans_Start())


                    End If
                End If


            End If
        End If

    End Sub


    Sub Send_Crew_Home(ByVal crew As KeyValuePair(Of Integer, Crew))
        Dim Home_Point As PointI
        Dim Home_rect As Rectangle


        If crew.Value.HomeBuilding > -1 Then

            'Send workers home after errands
            If crew.Value.Worker_Type = Worker_Type_Enum.Worker Then

                Home_rect = Building_List(crew.Value.HomeBuilding).BuildingRect(crew.Value.HomeSpace)
                Home_Point.x = CInt(Home_rect.X + Home_rect.Width / 2)
                Home_Point.y = CInt(Home_rect.Y + Home_rect.Height / 2)
                'Find AP in building                    
                'Pathfind

                If Not tile_map(crew.Value.find_tile.x, crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Sub
                Dim tile As PointI = crew.Value.find_tile

                crew.Value.command_queue.Clear()
                Dim End_Point As PointI
                End_Point = Send_Crew_Errands(crew.Value, crew.Key)

                If Building_List(crew.Value.WorkBuilding).Working_crew_list.Contains(crew.Key) Then Building_List(crew.Value.WorkBuilding).Working_crew_list.Remove(crew.Key)
                If Building_List(crew.Value.WorkBuilding).access_point.ContainsKey(tile) Then Building_List(crew.Value.WorkBuilding).access_point(tile).Used = False
                MoveCrewTo(End_Point, Home_Point, crew.Value)

            End If

            'Send transporters home after Errands
            If crew.Value.Worker_Type = Worker_Type_Enum.Transporter Then

                Home_rect = Building_List(crew.Value.HomeBuilding).BuildingRect(crew.Value.HomeSpace)
                Home_Point.x = CInt(Home_rect.X + Home_rect.Width / 2)
                Home_Point.y = CInt(Home_rect.Y + Home_rect.Height / 2)
                'Find AP in building                    
                'Pathfind
                crew.Value.RemoveWhenDone = True
                'If Not tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit For
                crew.Value.command_queue.Enqueue(New Crew.Command_Trans_Start())


                Dim End_Point As PointI
                End_Point = Send_Crew_Errands(crew.Value, crew.Key)

                MoveCrewTo(End_Point, Home_Point, crew.Value)


                'path_find.Standard_Pathfind(End_Point, Home_Point)
                'If path_find.Found_State = Pathfind.Pathfind.PFState.Found Then
                'Dim list As LinkedList(Of PointI)
                'list = path_find.Path
                'Need to add sleep and stuuuuf

                'For Each dest In list
                'crew.Value.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
                'Next
                'End If
            End If




        End If


    End Sub


    Function Send_Crew_Errands(ByVal Crew As Crew, ByVal ID As Integer) As PointI

        If Not tile_map(Crew.find_tile.x, Crew.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Function
        Dim tile As PointI
        If Crew.Worker_Type = Worker_Type_Enum.Worker Then tile = Crew.find_tile
        If Crew.Worker_Type = Worker_Type_Enum.Transporter Then tile = Building_List(Crew.WorkBuilding).PickupPoint

        Dim GotoBank As Boolean
        Dim Pub_Time As Integer
        'If Crew.Wealth > 500 Then GotoBank = True

        Dim Found_Bank_AP As Boolean
        Dim Bank_Point As PointI

        Dim Home_Point As PointI
        Dim Home_rect As Rectangle

        'SetHomeBuildings
        If Crew.BankBuilding = -1 Then
            Home_rect = Building_List(Crew.HomeBuilding).BuildingRect(Crew.HomeSpace)
            Home_Point.x = CInt(Home_rect.X + Home_rect.Width / 2)
            Home_Point.y = CInt(Home_rect.Y + Home_rect.Height / 2)
            Crew.BankBuilding = GetNearistBuilding(building_type_enum.Bank, Home_Point)
        End If

        If Crew.PubBuilding = -1 Then
            Home_rect = Building_List(Crew.HomeBuilding).BuildingRect(Crew.HomeSpace)
            Home_Point.x = CInt(Home_rect.X + Home_rect.Width / 2)
            Home_Point.y = CInt(Home_rect.Y + Home_rect.Height / 2)
            Crew.PubBuilding = GetNearistBuilding(building_type_enum.Pub, Home_Point)
        End If





        If GotoBank = True Then
            'Need to add find new bank code
            If Crew.BankBuilding > -1 Then
                'If crew has a bank
                For Each AP In Building_List(Crew.BankBuilding).access_point
                    If AP.Value.Type = BAP_Type.Customer AndAlso AP.Value.Used = False Then Bank_Point = AP.Key : Found_Bank_AP = True : Exit For
                Next

                If Found_Bank_AP = True Then
                    Found_Bank_AP = False
                    Building_List(Crew.BankBuilding).access_point(Bank_Point).Used = True

                    MoveCrewTo(tile, Bank_Point, Crew)
                    'Add Bank Script
                    'Crew.command_queue.Enqueue(New Crew.Command_Pub_Start(Bank_Point, Pub_Time))
                    tile = Bank_Point
                End If
            End If
        End If

        'Need to add find new pub code
        If Crew.PubBuilding > -1 Then
            Pub_Time = random(100, 200)
            Dim Found_AP As Boolean
            Dim Pub_Point As PointI
            For Each AP In Building_List(Crew.PubBuilding).access_point
                If AP.Value.Type = BAP_Type.Customer AndAlso AP.Value.Used = False Then Pub_Point = AP.Key : Found_AP = True : Exit For
            Next

            If Found_AP = True Then
                Found_AP = False
                'Set NextUp to work to true
                Building_List(Crew.PubBuilding).access_point(Pub_Point).Used = True
                'Pathfind
                MoveCrewTo(tile, Pub_Point, Crew)
                Crew.command_queue.Enqueue(New Crew.Command_Pub_Start(Pub_Point, Pub_Time))
                tile = Pub_Point

            End If
        End If

        Return tile


    End Function

    Sub Send_Transporter_Buy(ByVal Crew As Crew, ByVal PickupCash As Integer, ByVal BuyType As Item_Enum, ByVal BuyAmount As Integer, ByVal Safe_Point As PointI, ByVal Exchange_Point As PointI, ByVal Building_Point As PointI, ByVal BuildingID As Integer)


        'Goto Safe Point
        If Not tile_map(Crew.find_tile.x, Crew.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Sub
        Dim tile As PointI = Crew.find_tile

        Crew.command_queue.Clear()

        MoveCrewTo(tile, Safe_Point, Crew)

        'Pickup Money
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup(PickupCash))

        'Send To Buy Point
        MoveCrewTo(Safe_Point, Exchange_Point, Crew)

        'Buy Goods
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Buy(BuyAmount, BuyType))

        'Pickup Any Exchange money
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Exchange(BuildingID))

        'Send To Drop Off Point
        MoveCrewTo(Exchange_Point, Building_Point, Crew)

        'Drop Off Goods
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff(BuildingID, BuyAmount, BuyType))

        'Send To Safe Point
        MoveCrewTo(Building_Point, Safe_Point, Crew)

        'Dropoff Money
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff_Money(BuildingID))

        'Add to workers
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Start())
    End Sub



    Sub Send_Transporter_Sell(ByVal Crew As Crew, ByVal SellType As Item_Enum, ByVal SellAmount As Integer, ByVal Exchange_Point As PointI, ByVal Safe_Point As PointI, ByVal Building_Point As PointI, ByVal BuildingID As Integer)


        'Goto Safe Point
        If Not tile_map(Crew.find_tile.x, Crew.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Sub
        Dim tile As PointI = Crew.find_tile

        Crew.command_queue.Clear()

        'Send To Point to pickup
        MoveCrewTo(tile, Building_Point, Crew)

        'Pickup Goods
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Goods(BuildingID, SellAmount, SellType))

        'Send To Buy Point
        MoveCrewTo(Building_Point, Exchange_Point, Crew)

        'Sell Goods
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Sell(BuildingID, SellAmount, SellType))

        'Pickup Any Exchange money
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Exchange(BuildingID))

        'Send To Safe Point
        MoveCrewTo(Exchange_Point, Safe_Point, Crew)

        'Dropoff Money
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff_Money(BuildingID))

        'Send To Drop Off Point
        MoveCrewTo(Safe_Point, Building_Point, Crew)

        'Add to workers
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Start())
    End Sub


    Sub Send_Transporter_BuySell(ByVal Crew As Crew, ByVal SellType As Item_Enum, ByVal SellAmount As Integer, ByVal PickupCash As Integer, ByVal BuyType As Item_Enum, ByVal BuyAmount As Integer, ByVal Safe_Point As PointI, ByVal Exchange_Point As PointI, ByVal Building_Point As PointI, ByVal BuildingID As Integer)


        'Goto Safe Point
        If Not tile_map(Crew.find_tile.x, Crew.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Sub
        Dim tile As PointI = Crew.find_tile

        Crew.command_queue.Clear()

        MoveCrewTo(tile, Safe_Point, Crew)

        'Pickup Money
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup(PickupCash))


        'Send To Point to pickup
        MoveCrewTo(Safe_Point, Building_Point, Crew)

        'Pickup Goods
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Goods(BuildingID, SellAmount, SellType))

        'Send To Buy Point
        MoveCrewTo(Building_Point, Exchange_Point, Crew)

        'Buy Goods
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Buy(BuyAmount, BuyType))

        'Sell Goods
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Sell(BuildingID, SellAmount, SellType))

        'Pickup Any Exchange money
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Exchange(BuildingID))

        'Send To Drop Off Point
        MoveCrewTo(Exchange_Point, Building_Point, Crew)

        'Drop Off Goods
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff(BuildingID, BuyAmount, BuyType))

        'Send To Safe Point
        MoveCrewTo(Building_Point, Safe_Point, Crew)

        'Dropoff Money
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff_Money(BuildingID))

        'Send To Drop Off Point
        MoveCrewTo(Safe_Point, Building_Point, Crew)

        'Add to workers
        Crew.command_queue.Enqueue(New Crew.Command_Trans_Start())
    End Sub


    Sub Load_Pathfinding()
        path_find = New Pathfind.Pathfind(tile_map, New Rectangle(0, 0, size.x, size.y))
        'path_find = New Pathfind.Pathfind(tile_map, New Rectangle(0, 0, size.x \ 4, size.y \ 4))


    End Sub


    Sub Run_animations()
        For Each item In officer_list
            item.Value.Update_Sprite()
        Next

        For Each item In crew_list
            item.Value.Update_Sprite()
        Next

        If Animation_Glow_subtract = True Then
            Animation_Glow -= 0.001F
        Else
            Animation_Glow += 0.001F
        End If

        If Animation_Glow >= 1 Then Animation_Glow = 1 : Animation_Glow_subtract = True
        If Animation_Glow <= 0.8 Then Animation_Glow = 0.8 : Animation_Glow_subtract = False

    End Sub

    Sub Update_Officers()
        For Each item In officer_list
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
        Dim remove_List As New List(Of Integer)()

        For y = contact_Point.y - 4 To contact_Point.y + 4
            For x = contact_Point.x - 4 To contact_Point.x + 4
                If x >= 0 AndAlso x <= size.x AndAlso y >= 0 AndAlso y <= size.y Then
                    'tile_map(x, y).sprite = 0

                    For Each Crew In crew_list
                        If Crew.Value.find_tile = New PointI(x, y) Then
                            Crew.Value.Health.Damage_All_Limbs(3)
                            If Crew.Value.Health.Torso = 0 Then remove_List.Add(Crew.Key)
                        End If
                    Next
                End If
            Next
        Next

        For Each Crew In remove_List
            crew_list.Remove(Crew)
            For Each building In Building_List
                If building.Value.Working_crew_list.Contains(Crew) Then building.Value.Working_crew_list.Remove(Crew)
                If building.Value.Assigned_crew_list.Contains(Crew) Then building.Value.Assigned_crew_list.Remove(Crew)
                If building.Value.Available_Transporters.Contains(Crew) Then building.Value.Available_Transporters.Remove(Crew)
            Next
        Next
        remove_List.Clear()

    End Sub




    Function GetOfficer() As Dictionary(Of Integer, Officer)
        Return Me.officer_list
    End Function




    Function MoveCrewTo(ByVal StartPoint As PointI, ByVal EndPoint As PointI, ByVal Crew As Crew) As Boolean

        path_find.Standard_Pathfind(StartPoint, EndPoint)
        If path_find.Found_State = Pathfind.Pathfind.PFState.Found Then
            Dim list As LinkedList(Of PointI)
            list = path_find.Path

            For Each dest In list
                Crew.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
            Next
        Else
            Return False
        End If
        Return True
    End Function





    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class