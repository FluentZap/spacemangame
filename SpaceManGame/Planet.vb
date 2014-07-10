Public Class Building_Count_Type
    Public Mine As Integer
    Public Farm As Integer
    Public Factory As Integer
    Public Refinery As Integer
    Public Pub As Integer
    Public Special As Integer
    Public SpacePort As Integer
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


Public Class Planet
    'Planet 
    Public PlanetID As Integer
    Public Generated As Boolean
    Public landed_ships As New Dictionary(Of Integer, PointI)()
    Public Block_Map As New Block_Map_Type()
    Public External_Block_Map As New Block_Map_Type()
    Public Building_List As New Dictionary(Of Integer, Planet_Building)()

    'Location
    Public orbits_planet As Boolean
    Public orbit_point As Integer
    Public orbit_distance As Integer
    Public radius As Integer
    Public theta As Double
    Public size As PointI


    'Ai
    Public Exchange As New PlanetExchange
    Public population As Integer
    Public populationMax As Integer    
    Public Citizens As Integer
    Public CitizensMax As Integer
    Public Resource_Count As Integer
    Public Farm_Count As Integer
    Public Resource_Points As New Dictionary(Of PointI, Boolean)() 'Is true if resource point is taken
    Public Farm_Points As New Dictionary(Of PointI, Boolean)() 'Is true if farm point is taken
    Public Building_Count As New Building_Count_Type()


    'Graphics
    Public Animation_Glow As Single
    Public Animation_Glow_subtract As Boolean    

    



    'Public Send_List As New Dictionary(Of KeyValuePair(Of Integer, Crew), Send_work_List_Enum)()
    Public Item_Point As New Dictionary(Of PointI, Item_Point_Type)()

    'Public Free_Job_List As New Dictionary(Of Integer, HashSet(Of Job_List_Type))()
    'Public Free_Housing_List As New HashSet(Of Housing_List_Type)()
    'Public Free_Job_List_Count As Integer
    'Private BuilderSpawnCounter As Integer = 100
    'Private BuilderCount As Integer
    'Public Builder_List As New Dictionary(Of Integer, Integer)()
    'Public Build_List As New Dictionary(Of Integer, Build_List_Type)()



    Private path_find As Pathfind.Pathfind

    
    Public tile_map(,) As Planet_tile
        

    Public CapitalPoint As PointI

    Public type As planet_type_enum
    'ambient settings


    Public tech As HashSet(Of planet_tech_list_enum)
    Public special_tech As Planet_special_tech_enum

    Public crew_list As New Dictionary(Of Integer, Crew)()
    Public officer_list As New Dictionary(Of Integer, Officer)()

    Public Projectiles As HashSet(Of Projectile) = New HashSet(Of Projectile)




    Sub New(ByVal ID As Integer, ByVal type As planet_type_enum, ByVal size As PointI, ByVal orbit_point As Integer, ByVal orbit_distance As Integer, ByVal orbits_planet As Boolean, ByVal theta_offset As Double, ByVal CivSize As Planet_Level_Type)
        Me.PlanetID = ID
        Me.type = type
        ReDim Me.tile_map(size.x, size.y)
        Me.size = size
        Me.orbit_point = orbit_point
        Me.orbit_distance = orbit_distance
        Me.orbits_planet = orbits_planet
        Me.theta = theta_offset
        Initialise(CivSize)
    End Sub

    Sub Set_Population()
        'population = crew_list.Count - BuilderCount
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
        'If Not Build_List.ContainsKey(ID) Then Build_List.Add(ID, New Build_List_Type)
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
        'Build_List(ID).Build_Progress = tiles.Count
        'Build_List(ID).Type = Type
        'Build_List(ID).OwnerID = OwnerID
        'Build_List(ID).Pos = Pos
        'Build_List(ID).BlockType = Block_Type

        For Each item In tiles
            'Build_List(ID).Tile_List.Add(New PointI(item.X + Adjusted_Pos.x, item.Y + Adjusted_Pos.y), 0)
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
        'Negative if building is already built
        Mine -= Building_Count.Mine * 2
        Farm -= Building_Count.Farm * 2
        Factory -= Building_Count.Factory * 2
        Refinery -= Building_Count.Refinery * 2
        Pub -= Building_Count.Pub * 2

        Dim list As New Dictionary(Of Integer, Double)()
        Dim Highest As Integer = -1
        list.Add(-1, -1)
        If Building_Count.Mine < Resource_Count Then list.Add(0, Mine)
        If Building_Count.Farm < Farm_Count Then list.Add(1, Farm)
        list.Add(2, Factory)
        list.Add(3, Refinery)
        list.Add(4, Pub)

        'If Building_Count.CSpecial < Special Then list.Add(5, Special) : Highest = 5

        For Each item In list
            If item.Value > list(Highest) Then Highest = item.Key
        Next

        'Dim Return_Type As Block_Return_Type_Class
        Dim Id As Integer = GetEmptyBuildingID()
        Select Case Highest
            Case Is = -1

            Case Is = 0
                'Return_Type = FindBestBuildingPos(building_type_enum.Mine)
                'AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.Mine += 1                
                Building_List.Add(GetEmptyBuildingID, New Planet_Building(Owner, building_type_enum.Mine))

            Case Is = 1
                'Return_Type = FindBestBuildingPos(building_type_enum.Farm)
                'AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.Farm += 1
                Building_List.Add(GetEmptyBuildingID, New Planet_Building(Owner, building_type_enum.Farm))

            Case Is = 2
                'Return_Type = FindBestBuildingPos(building_type_enum.Factory)
                'AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.Factory += 1
                Building_List.Add(GetEmptyBuildingID, New Planet_Building(Owner, building_type_enum.Factory))

            Case Is = 3
                'Return_Type = FindBestBuildingPos(building_type_enum.Refinery)
                'AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.Refinery += 1
                Building_List.Add(GetEmptyBuildingID, New Planet_Building(Owner, building_type_enum.Refinery))

            Case Is = 4
                'Return_Type = FindBestBuildingPos(building_type_enum.Pub)
                'AddPlanetBlock(Return_Type.Pos, Return_Type.Type, Me)
                Building_Count.Pub += 1
                Building_List.Add(GetEmptyBuildingID, New Planet_Building(Owner, building_type_enum.Pub))

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

            Case Is = building_type_enum.Outpost
                Return FindEmptyTile(CapitalPoint, Block_Type_Enum.Large)
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
            If Not Building_List.ContainsKey(a) Then ID = a : Exit For
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
        Dim X, Y As Single
        vector.x *= Officer_List(Id).speed
        vector.y *= Officer_List(Id).speed

        Do
            If vector.x > 0 Then
                If vector.x > 1 Then vector.x -= 1 : X = 1 Else X = vector.sngX : vector.x = 0
            Else
                If vector.x < -1 Then vector.x += 1 : X = -1 Else X = vector.sngX : vector.x = 0
            End If


            If vector.y > 0 Then
                If vector.y > 1 Then vector.y -= 1 : Y = 1 Else Y = vector.sngY : vector.y = 0
            Else
                If vector.y < -1 Then vector.y += 1 : Y = -1 Else Y = vector.sngY : vector.y = 0
            End If

            If X > 0 Then MoveOfficerStep(Id, Move_Direction.Right, Math.Abs(X))
            If X < 0 Then MoveOfficerStep(Id, Move_Direction.Left, Math.Abs(X))
            If Y < 0 Then MoveOfficerStep(Id, Move_Direction.Up, Math.Abs(Y))
            If Y > 0 Then MoveOfficerStep(Id, Move_Direction.Down, Math.Abs(Y))

        Loop Until vector.x = 0 AndAlso vector.y = 0

    End Sub

    Private Sub MoveOfficerStep(ByVal Id As Integer, ByVal Direction As Move_Direction, ByVal Amount As Single)

        Dim R, B As RectangleF
        '0:7,1
        '0:24,1
        '0:7,31
        '0:24,31
        B = New RectangleF(Officer_List(Id).GetLocationD.sngX + 9, Officer_List(Id).GetLocationD.sngY + 27, 14, 5)
        R = New RectangleF(Officer_List(Id).GetLocationD.sngX + 9, Officer_List(Id).GetLocationD.sngY + 27, 14, 5)
        If Direction = Move_Direction.Up Then R.Y -= Amount
        If Direction = Move_Direction.Down Then R.Y += Amount
        If Direction = Move_Direction.Left Then R.X -= Amount
        If Direction = Move_Direction.Right Then R.X += Amount
        Dim CantMove As Boolean

        Select Case Direction
            Case Is = Move_Direction.Up                
                If Not Get_Tile(CInt(Math.Floor(R.Left / 32)), CInt(Math.Floor(R.Top / 32))).walkable = walkable_type_enum.Walkable Then CantMove = True
                If Not Get_Tile(CInt(Math.Floor((R.Right - 1) / 32)), CInt(Math.Floor(R.Top / 32))).walkable = walkable_type_enum.Walkable Then CantMove = True
            Case Is = Move_Direction.Down
                If Not Get_Tile(CInt(Math.Floor(R.Left / 32)), CInt(Math.Floor(R.Bottom / 32))).walkable = walkable_type_enum.Walkable Then CantMove = True
                If Not Get_Tile(CInt(Math.Floor((R.Right - 1) / 32)), CInt(Math.Floor(R.Bottom / 32))).walkable = walkable_type_enum.Walkable Then CantMove = True
            Case Is = Move_Direction.Left
                If Not Get_Tile(CInt(Math.Floor(R.Left / 32)), CInt(Math.Floor(R.Top / 32))).walkable = walkable_type_enum.Walkable Then CantMove = True
                If Not Get_Tile(CInt(Math.Floor(R.Left / 32)), CInt(Math.Floor((R.Bottom - 1) / 32))).walkable = walkable_type_enum.Walkable Then CantMove = True
            Case Is = Move_Direction.Right
                If Not Get_Tile(CInt(Math.Floor(R.Right / 32)), CInt(Math.Floor(R.Top / 32))).walkable = walkable_type_enum.Walkable Then CantMove = True
                If Not Get_Tile(CInt(Math.Floor(R.Right / 32)), CInt(Math.Floor((R.Bottom - 1) / 32))).walkable = walkable_type_enum.Walkable Then CantMove = True
        End Select

        If CantMove = False Then
            Select Case Direction
                Case Is = Move_Direction.Up : Officer_List(Id).Move(New PointD(0, -Amount))
                Case Is = Move_Direction.Down : Officer_List(Id).Move(New PointD(0, Amount))
                Case Is = Move_Direction.Left : Officer_List(Id).Move(New PointD(-Amount, 0))
                Case Is = Move_Direction.Right : Officer_List(Id).Move(New PointD(Amount, 0))
            End Select
        Else
            Select Case Direction
                Case Is = Move_Direction.Up : Officer_List(Id).location.y = (CInt(B.Top) \ 32) * 32 - (B.Top - Officer_List(Id).location.y)
                Case Is = Move_Direction.Down : Officer_List(Id).location.y = (CInt(Math.Floor(B.Bottom) / 32)) * 32 - (B.Bottom - Officer_List(Id).location.y)


                Case Is = Move_Direction.Left : Officer_List(Id).location.x = (CInt(B.Left) \ 32) * 32 - (B.Left - Officer_List(Id).location.x)
                Case Is = Move_Direction.Right : Officer_List(Id).location.x = (CInt(Math.Floor(B.Right) / 32)) * 32 + (Officer_List(Id).location.x - B.Right)

            End Select
        End If
    End Sub


    Function Get_Tile(ByVal x As Integer, ByVal y As Integer) As Planet_tile
        If x >= 0 AndAlso x <= size.x AndAlso y >= 0 AndAlso y <= size.y Then
            Return tile_map(x, y)
        Else
            Return New Planet_tile(planet_tile_type_enum.empty, 0, walkable_type_enum.Impassable)
        End If
    End Function

    Function Get_Tile(ByVal Point As PointI) As Planet_tile
        If Point.x >= 0 AndAlso Point.x <= size.x AndAlso Point.y >= 0 AndAlso Point.y <= size.y Then
            Return tile_map(Point.x, Point.y)
        Else
            Return New Planet_tile(planet_tile_type_enum.empty, 0, walkable_type_enum.Impassable)
        End If
    End Function

    Sub Initialise(ByVal Size As Planet_Level_Type)
        'Set Resource/Farm count
        Me.Resource_Count = random(2, 4)
        Me.Farm_Count = random(2, 4)
        Select Case Size
            Case Is = Planet_Level_Type.Barren
                population = 0 : populationMax = 0 : CitizensMax = 0
            Case Is = Planet_Level_Type.Outpost
                population = 10 : populationMax = 20 : CitizensMax = 5
            Case Is = Planet_Level_Type.Village
                population = 20 : populationMax = 40 : CitizensMax = 10
            Case Is = Planet_Level_Type.Town
                population = 30 : populationMax = 60 : CitizensMax = 15
            Case Is = Planet_Level_Type.City
                population = 40 : populationMax = 80 : CitizensMax = 20
            Case Is = Planet_Level_Type.Metropolis
                population = 50 : populationMax = 100 : CitizensMax = 25
            Case Is = Planet_Level_Type.Empire
                population = 75 : populationMax = 150 : CitizensMax = 30
        End Select

        'Create Citizens
        For a = 1 To CitizensMax
            Dim Id As Integer = GetEmptyOfficerID()
            Add_Officer(Id, Me, New Officer(0, Id.ToString, Officer_location_enum.Planet, PlanetID, New PointD(0, 0), 1, 1, New Officer.sprite_list(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head)))
            u.Officer_List(Id).Item_List.Add(Item_Enum.CrystalCoin, 10000)
            u.Officer_List(Id).Citizen_Rights.Add(PlanetID)
            Citizens += 1
        Next

        For Each Cit In officer_list
            Citizen_Build_Building(Cit.Key)
        Next

    End Sub


    Sub populate()
        populate_planet(Me)
        Load_Pathfinding()
    End Sub

    Public Sub DoEvents()
        'Process_crew()
        'Process_Work()

        'If GSTFrequency = 0 Then Process_Planet()
        'If GSTFrequency = 0 Then Process_Citizens()
        Process_Buildings()
        Process_Citizens(True)
        Update_Officers()
        CrewRandomTalking()

        Handle_Projectiles()
        run_crew_scrips(crew_list)
        Run_animations()
    End Sub


    Sub CrewRandomTalking()
        If GSTFrequency = 0 Then
            For Each Crew In crew_list
                If random(0, 100) = 100 Then
                    If Crew.Value.Speech.Speech.Count = 0 Then Crew.Value.Speech.Add(AiSpeech.GetAiSpeech(Crew.Value), 20)
                End If
                Crew.Value.Speech.Advance()
            Next
        End If


        For Each Officer In officer_list            
            If Officer.Value.Greeted = False AndAlso Officer.Value.InBuilding > -1 Then
                If Building_List(Officer.Value.InBuilding).GetWorker > -1 Then
                    GetAiSpeech(crew_list(Building_List(Officer.Value.InBuilding).GetWorker), Speech_Enum.Shopkeeper_Greeting)                    
                    Officer.Value.Greeted = True
                End If
            End If
        Next
    End Sub


    Public Sub DoPassiveEvents()
        Process_Buildings()
        Process_Citizens(False)


    End Sub


    Sub Process_Citizens(ByVal Active As Boolean)

    End Sub


    Sub Check_Citizen()
        If Citizens < CitizensMax Then
            Dim Id As Integer = GetEmptyOfficerID()
            Add_Officer(Id, Me, New Officer(0, Id.ToString, Officer_location_enum.Planet, 0, New PointD(0, 0), 1, 1, New Officer.sprite_list(character_sprite_set_enum.Human_Renagade_1, character_sprite_enum.Head)))
            u.Officer_List(Id).Item_List.Add(Item_Enum.CrystalCoin, 10000)
            u.Officer_List(Id).Citizen_Rights.Add(PlanetID)
            Citizens += 1
        End If
    End Sub


    Sub Process_Planet()
        Set_Population()
        Check_Citizen()        

        'Remove_Compleated_Buildings
        Dim Remove_List As New HashSet(Of Integer)()
        'For Each Item In Build_List
        'If Item.Value.Compleated = True Then
        'PlanetGenerator.Build_Building(Item.Value.Type, Item.Key, Item.Value.OwnerID, Me, New Block_Return_Type_Class(Item.Value.Pos \ 32, Item.Value.BlockType))
        'Remove_List.Add(Item.Key)
        'End If
        'Next
        'For Each Item In Remove_List
        'Build_List.Remove(Item)
        'Next

    End Sub


#Region "Process Buildings"

    Sub Process_Buildings()
        If GSTFrequency = 0 AndAlso GST Mod 10 = 0 Then
            For Each building In Building_List
                If building.Value.Type = building_type_enum.Mine Then Process_Mine(building)
                If building.Value.Type = building_type_enum.Refinery Then Process_Refinery(building)
                If building.Value.Type = building_type_enum.Factory Then Process_Factory(building)
                If building.Value.Type = building_type_enum.Farm Then Process_Farm(building)
                If building.Value.Type = building_type_enum.Pub Then Process_Pub(building)
            Next
        End If
    End Sub

    Sub Building_Sell_Item(ByVal Item As Item_Enum, ByVal B As Planet_Building, ByVal ID As Integer, ByVal Threshold As Integer)
        If B.Get_Item(Item) >= Threshold Then
            Exchange.List_Item(Item, 100, ID)
            B.Remove_Item(Item, 100)
        End If
    End Sub

    Sub Building_Buy_Item(ByVal Item As Item_Enum, ByVal B As Planet_Building, ByVal Threshold As Integer)
        If B.Get_Item(Item) < Threshold Then
            If Exchange.Check_Exchange(Item) >= 100 Then
                Exchange.Buy_Item(Item, 100)
                B.Add_Item(Item, 100)
            End If
        End If
    End Sub

    Sub Process_Mine(ByVal Building As KeyValuePair(Of Integer, Planet_Building))
        Dim B As Planet_Building = Building.Value
        Dim ID As Integer = Building.Key
        'Make
        If B.Get_Item(Item_Enum.Parts) > 0 Then
            'Has Parts
            B.Remove_Item(Item_Enum.Parts, 1)
            B.Add_Item(Item_Enum.Crystal, 3)
            B.Add_Item(Item_Enum.Carbon, 3)
            B.Add_Item(Item_Enum.Water, 3)
            B.Add_Item(Item_Enum.Minerals, 3)
            If Me.type = planet_type_enum.Desert Then B.Add_Item(Item_Enum.Crystal, 7)
        Else
            'No Parts
            If Me.type = planet_type_enum.Desert Then B.Add_Item(Item_Enum.Crystal, 7)
        End If

        'Sell
        Building_Sell_Item(Item_Enum.Crystal, B, ID, 500)
        Building_Sell_Item(Item_Enum.Carbon, B, ID, 500)
        Building_Sell_Item(Item_Enum.Water, B, ID, 500)
        Building_Sell_Item(Item_Enum.Minerals, B, ID, 500)

        'Buy
        Building_Buy_Item(Item_Enum.Parts, B, 500)

    End Sub

    Sub Process_Refinery(ByVal Building As KeyValuePair(Of Integer, Planet_Building))
        Dim B As Planet_Building = Building.Value
        Dim ID As Integer = Building.Key
        'Make
        If B.Get_Item(Item_Enum.Crystal) >= 5 Then
            B.Remove_Item(Item_Enum.Crystal, 5)
            B.Add_Item(Item_Enum.Refined_Crystal, 10)
        End If

        'Sell
        Building_Sell_Item(Item_Enum.Refined_Crystal, B, ID, 500)

        'Buy
        Building_Buy_Item(Item_Enum.Crystal, B, 500)
    End Sub

    Sub Process_Factory(ByVal Building As KeyValuePair(Of Integer, Planet_Building))
        Dim B As Planet_Building = Building.Value
        Dim ID As Integer = Building.Key
        'Make
        If B.Get_Item(Item_Enum.Refined_Crystal) >= 5 Then
            B.Remove_Item(Item_Enum.Refined_Crystal, 5)
            B.Add_Item(Item_Enum.Parts, 10)
        End If

        'Sell
        Building_Sell_Item(Item_Enum.Parts, B, ID, 500)

        'Buy
        Building_Buy_Item(Item_Enum.Refined_Crystal, B, 500)
    End Sub

    Sub Process_Farm(ByVal Building As KeyValuePair(Of Integer, Planet_Building))
        Dim B As Planet_Building = Building.Value
        Dim ID As Integer = Building.Key
        'Make
        If B.Get_Item(Item_Enum.Parts) > 0 Then
            'Has Parts
            B.Remove_Item(Item_Enum.Parts, 1)
            B.Add_Item(Item_Enum.RawDesertProduce, 10)
        Else
            'No Parts
            B.Add_Item(Item_Enum.RawDesertProduce, 5)
        End If

        'Sell
        Building_Sell_Item(Item_Enum.RawDesertProduce, B, ID, 500)

        'Buy
        Building_Buy_Item(Item_Enum.Parts, B, 500)
    End Sub

    Sub Process_Pub(ByVal Building As KeyValuePair(Of Integer, Planet_Building))
        Dim B As Planet_Building = Building.Value
        Dim ID As Integer = Building.Key
        'Make
        If B.Get_Item(Item_Enum.RawDesertProduce) > 0 Then
            'Has Parts
            B.Remove_Item(Item_Enum.RawDesertProduce, 1)
            B.Add_Item(Item_Enum.DesertFood, 5)
        End If

        'Sell
        Building_Sell_Item(Item_Enum.DesertFood, B, ID, 500)

        'Buy
        Building_Buy_Item(Item_Enum.RawDesertProduce, B, 500)
    End Sub

#End Region

    Function GetNearistBuilding(ByVal Building_Type As building_type_enum, ByVal Point As PointI) As Integer
        Dim list As New Dictionary(Of Integer, Integer)()
        For Each building In Building_List
            If building.Value.Type = Building_Type Then list.Add(building.Key, CInt(Math.Sqrt(((building.Value.LandRect.Left + building.Value.LandRect.Width \ 2) ^ 2) + (building.Value.LandRect.Top + building.Value.LandRect.Height \ 2) ^ 2)))
        Next

        Dim Lowest As Integer = 1000000
        Dim ID As Integer
        For Each item In list
            If item.Value < Lowest Then Lowest = item.Value : ID = item.Key
        Next
        If Building_List.Count = 0 Then Return -1
        Return ID
    End Function

    Function GetNearistCustomerSlot(ByVal Building_Type As building_type_enum, ByVal Point As PointI, ByVal CustomerID As Integer) As Slot_Return_Type
        Dim list As New Dictionary(Of Integer, Integer)()
        For Each building In Building_List
            If building.Value.EmptyCustomerSlots > 0 Then
                If building.Value.Type = Building_Type Then list.Add(building.Key, CInt(Math.Sqrt(((building.Value.LandRect.Left + building.Value.LandRect.Width \ 2) ^ 2) + (building.Value.LandRect.Top + building.Value.LandRect.Height \ 2) ^ 2)))
            End If
        Next

        Dim Lowest As Integer = 1000000
        Dim BuildingID As Integer
        For Each item In list
            If item.Value < Lowest Then Lowest = item.Value : BuildingID = item.Key
        Next
        If Building_List.Count = 0 OrElse list.Count = 0 Then Return New Slot_Return_Type()

        Dim slot As New Slot_Return_Type(Building_List(BuildingID).GetEmptyCustomerSlot, BuildingID)
        Building_List(BuildingID).Customer_Slots(slot.Point) = CustomerID        
        Return slot
    End Function



    Function Check_Resources(ByVal BuildingID As Integer, ByVal Item As Item_Enum) As Integer
        Dim amount As Integer
        If Building_List.ContainsKey(BuildingID) Then

            Dim B As Planet_Building = Building_List(BuildingID)

            'For Each ipoint In B.Item_Slots
            'If Item_Point.ContainsKey(ipoint.Key) AndAlso Item_Point(ipoint.Key).Item = Item Then
            'amount += Item_Point(ipoint.Key).Amount
            'End If
            'Next
            Return amount

        End If
        Return 0
    End Function


    Sub Process_crew()
           'Send Crew to work
        'Send Crew to bank if needed
        'Send Crew to Pub
        'Send Crew home
        'al la repeto


        'Send to work            

        'Send crew home



        'If Send_List.Count > 0 Then

        'Select Case Send_List.First.Value

        'Case Is = Send_work_List_Enum.Work
        'Send_Crew_ToWork(Send_List.First.Key)
        'Send_List.Remove(Send_List.First.Key)

        'Case Is = Send_work_List_Enum.Home
        'Send_Crew_Home(Send_List.First.Key)
        'Send_List.Remove(Send_List.First.Key)

        'End Select

        'End If

    End Sub





    Sub Send_Crew_ToWork(ByVal Crew As KeyValuePair(Of Integer, Crew))
        Dim work_Point As PointI
        Dim Found_AP As Boolean = False

        'Not already working/going to work
        'If Not Building_List(Crew.Value.WorkBuilding).Working_crew_list.Contains(Crew.Key) AndAlso Not Building_List(Crew.Value.WorkBuilding).Assigned_crew_list.Contains(Crew.Key) Then
        'If Crew.Value.WorkBuilding > -1 Then


        'Worker
        'If Crew.Value.Worker_Type = Worker_Type_Enum.Worker Then
        'Find AP in building
        'For Each AP In Building_List(Crew.Value.WorkBuilding).access_point
        'If AP.Value.Type = BAP_Type.Worker AndAlso AP.Value.Used = False AndAlso AP.Value.NextUp = False Then
        'work_Point = AP.Key
        'Found_AP = True
        'Exit For
        'End If
        'Next

        If Found_AP = False Then
            'For Each AP In Building_List(Crew.Value.WorkBuilding).access_point
            'If AP.Value.Type = BAP_Type.Worker AndAlso AP.Value.NextUp = False Then
            'work_Point = AP.Key
            'Found_AP = True
            'Exit For
        End If
        'Next
        'End If


        'If Open access point is found send crew to work
        If Found_AP = True Then
            Found_AP = False
            'Set NextUp to work to true
            'Building_List(Crew.Value.WorkBuilding).access_point(work_Point).NextUp = True
            'Pathfind

            'If Not tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit For

            'Dim tile As PointI = Crew.Value.find_tile


            'path_find.Standard_Pathfind(tile, work_Point)
            If path_find.Found_State = Pathfind.Pathfind.PFState.Found Then

                If Crew.Value.command_queue.Any Then
                    Crew.Value.command_queue.Clear()
                End If
                'Building_List(Crew.Value.WorkBuilding).Assigned_crew_list.Add(Crew.Key)
                Dim list As LinkedList(Of PointI)
                list = path_find.Path

                'Need to add wait for finish
                Crew.Value.command_queue.Clear()
                For Each dest In list
                    If dest = work_Point Then
                        'Crew.Value.command_queue.Enqueue(New Crew.Command_Try_Work(work_Point))
                    End If
                    'Crew.Value.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
                Next
                'Crew.Value.command_queue.Enqueue(New Crew.Command_Start_Work(work_Point))
            End If

        End If
        'End If






        'If Crew.Value.Worker_Type = Worker_Type_Enum.Transporter Then

        'Dim point As PointI = Building_List(Crew.Value.WorkBuilding).PickupPoint
        'If Not tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit For


        Dim tile As PointI = Crew.Value.find_tile

        'path_find.Standard_Pathfind(tile, point)

        If path_find.Found_State = Pathfind.Pathfind.PFState.Found Then
            Dim list As LinkedList(Of PointI)
            list = path_find.Path

            'Need to add wait for finish                                
            For Each dest In list
                Crew.Value.command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
            Next
            'Crew.Value.command_queue.Enqueue(New Crew.Command_Trans_Start())


        End If
        'End If


        'End If
        'End If

    End Sub


    Sub Send_Crew_Home(ByVal crew As KeyValuePair(Of Integer, Crew))
        Dim Home_Point As PointI
        Dim Home_rect As Rectangle


        'If crew.Value.HomeBuilding > -1 Then

        'Send workers home after errands
        'If crew.Value.Worker_Type = Worker_Type_Enum.Worker Then

        'Home_rect = Building_List(crew.Value.HomeBuilding).BuildingRect(crew.Value.HomeSpace)
        Home_Point.x = CInt(Home_rect.X + Home_rect.Width / 2)
        Home_Point.y = CInt(Home_rect.Y + Home_rect.Height / 2)
        'Find AP in building                    
        'Pathfind

        If Not tile_map(crew.Value.find_tile.x, crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Sub
        Dim tile As PointI = crew.Value.find_tile

        crew.Value.command_queue.Clear()
        Dim End_Point As PointI
        End_Point = Send_Crew_Errands(crew.Value, crew.Key)

        'If Building_List(crew.Value.WorkBuilding).Working_crew_list.Contains(crew.Key) Then Building_List(crew.Value.WorkBuilding).Working_crew_list.Remove(crew.Key)
        'If Building_List(crew.Value.WorkBuilding).access_point.ContainsKey(tile) Then Building_List(crew.Value.WorkBuilding).access_point(tile).Used = False
        MoveCrewTo(End_Point, Home_Point, crew.Value)

        'End If

        'Send transporters home after Errands
        'If crew.Value.Worker_Type = Worker_Type_Enum.Transporter Then

        'Home_rect = Building_List(crew.Value.HomeBuilding).BuildingRect(crew.Value.HomeSpace)
        Home_Point.x = CInt(Home_rect.X + Home_rect.Width / 2)
        Home_Point.y = CInt(Home_rect.Y + Home_rect.Height / 2)
        'Find AP in building                    
        'Pathfind
        'crew.Value.RemoveWhenDone = True
        'If Not tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit For
        'crew.Value.command_queue.Enqueue(New Crew.Command_Trans_Start())


        'Dim End_Point As PointI
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
        'End If




        'End If


    End Sub


    Function Send_Crew_Errands(ByVal Crew As Crew, ByVal ID As Integer) As PointI

        If Not tile_map(Crew.find_tile.x, Crew.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Function
        Dim tile As PointI
        'If Crew.Worker_Type = Worker_Type_Enum.Worker Then tile = Crew.find_tile
        'If Crew.Worker_Type = Worker_Type_Enum.Transporter Then tile = Building_List(Crew.WorkBuilding).PickupPoint

        Dim GotoBank As Boolean
        Dim Pub_Time As Integer
        'If Crew.Wealth > 500 Then GotoBank = True

        Dim Found_Bank_AP As Boolean
        Dim Bank_Point As PointI

        Dim Home_Point As PointI
        Dim Home_rect As Rectangle

        'SetHomeBuildings
        'If Crew.BankBuilding = -1 Then
        'Home_rect = Building_List(Crew.HomeBuilding).BuildingRect(Crew.HomeSpace)
        Home_Point.x = CInt(Home_rect.X + Home_rect.Width / 2)
        Home_Point.y = CInt(Home_rect.Y + Home_rect.Height / 2)
        'Crew.BankBuilding = GetNearistBuilding(building_type_enum.Bank, Home_Point)
        'End If

        'If Crew.PubBuilding = -1 Then
        'Home_rect = Building_List(Crew.HomeBuilding).BuildingRect(Crew.HomeSpace)
        Home_Point.x = CInt(Home_rect.X + Home_rect.Width / 2)
        Home_Point.y = CInt(Home_rect.Y + Home_rect.Height / 2)
        'Crew.PubBuilding = GetNearistBuilding(building_type_enum.Pub, Home_Point)
        'End If





        If GotoBank = True Then
            'Need to add find new bank code
            'If Crew.BankBuilding > -1 Then
            'If crew has a bank
            'For Each AP In Building_List(Crew.BankBuilding).access_point
            'If AP.Value.Type = BAP_Type.Customer AndAlso AP.Value.Used = False Then Bank_Point = AP.Key : Found_Bank_AP = True : Exit For
            'Next

            If Found_Bank_AP = True Then
                Found_Bank_AP = False
                'Building_List(Crew.BankBuilding).access_point(Bank_Point).Used = True

                MoveCrewTo(tile, Bank_Point, Crew)
                'Add Bank Script
                'Crew.command_queue.Enqueue(New Crew.Command_Pub_Start(Bank_Point, Pub_Time))
                tile = Bank_Point
            End If
        End If
        'End If

        'Need to add find new pub code
        'If Crew.PubBuilding > -1 Then
        Pub_Time = random(100, 200)
        Dim Found_AP As Boolean
        Dim Pub_Point As PointI
        'For Each AP In Building_List(Crew.PubBuilding).access_point
        'If AP.Value.Type = BAP_Type.Customer AndAlso AP.Value.Used = False Then Pub_Point = AP.Key : Found_AP = True : Exit For
        'Next

        If Found_AP = True Then
            Found_AP = False
            'Set NextUp to work to true
            'Building_List(Crew.PubBuilding).access_point(Pub_Point).Used = True
            'Pathfind
            MoveCrewTo(tile, Pub_Point, Crew)
            'Crew.command_queue.Enqueue(New Crew.Command_Pub_Start(Pub_Point, Pub_Time))
            tile = Pub_Point

        End If
        'End If

        Return tile


    End Function

    Sub Send_Transporter_Buy(ByVal Crew As Crew, ByVal PickupCash As Integer, ByVal BuyType As Item_Enum, ByVal BuyAmount As Integer, ByVal Safe_Point As PointI, ByVal Exchange_Point As PointI, ByVal Building_Point As PointI, ByVal BuildingID As Integer)


        'Goto Safe Point
        If Not tile_map(Crew.find_tile.x, Crew.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Sub
        Dim tile As PointI = Crew.find_tile

        Crew.command_queue.Clear()

        MoveCrewTo(tile, Safe_Point, Crew)

        'Pickup Money
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup(PickupCash))

        'Send To Buy Point
        'MoveCrewTo(Safe_Point, Exchange_Point, Crew)

        'Buy Goods
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Buy(BuyAmount, BuyType))

        'Pickup Any Exchange money
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Exchange(BuildingID))

        'Send To Drop Off Point
        'MoveCrewTo(Exchange_Point, Building_Point, Crew)

        'Drop Off Goods
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff(BuildingID, BuyAmount, BuyType))

        'Send To Safe Point
        'MoveCrewTo(Building_Point, Safe_Point, Crew)

        'Dropoff Money
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff_Money(BuildingID))

        'Add to workers
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Start())
    End Sub



    Sub Send_Transporter_Sell(ByVal Crew As Crew, ByVal SellType As Item_Enum, ByVal SellAmount As Integer, ByVal Exchange_Point As PointI, ByVal Safe_Point As PointI, ByVal Building_Point As PointI, ByVal BuildingID As Integer)


        'Goto Safe Point
        If Not tile_map(Crew.find_tile.x, Crew.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Sub
        Dim tile As PointI = Crew.find_tile

        Crew.command_queue.Clear()

        'Send To Point to pickup
        'MoveCrewTo(tile, Building_Point, Crew)

        'Pickup Goods
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Goods(BuildingID, SellAmount, SellType))

        'Send To Buy Point
        'MoveCrewTo(Building_Point, Exchange_Point, Crew)

        'Sell Goods
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Sell(BuildingID, SellAmount, SellType))

        'Pickup Any Exchange money
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Exchange(BuildingID))

        'Send To Safe Point
        'MoveCrewTo(Exchange_Point, Safe_Point, Crew)

        'Dropoff Money
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff_Money(BuildingID))

        'Send To Drop Off Point
        'MoveCrewTo(Safe_Point, Building_Point, Crew)

        'Add to workers
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Start())
    End Sub


    Sub Send_Transporter_BuySell(ByVal Crew As Crew, ByVal SellType As Item_Enum, ByVal SellAmount As Integer, ByVal PickupCash As Integer, ByVal BuyType As Item_Enum, ByVal BuyAmount As Integer, ByVal Safe_Point As PointI, ByVal Exchange_Point As PointI, ByVal Building_Point As PointI, ByVal BuildingID As Integer)


        'Goto Safe Point
        If Not tile_map(Crew.find_tile.x, Crew.find_tile.y).walkable = walkable_type_enum.Walkable Then Exit Sub
        Dim tile As PointI = Crew.find_tile

        Crew.command_queue.Clear()

        MoveCrewTo(tile, Safe_Point, Crew)

        'Pickup Money
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup(PickupCash))


        'Send To Point to pickup
        MoveCrewTo(Safe_Point, Building_Point, Crew)

        'Pickup Goods
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Goods(BuildingID, SellAmount, SellType))

        'Send To Buy Point
        MoveCrewTo(Building_Point, Exchange_Point, Crew)

        'Buy Goods
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Buy(BuyAmount, BuyType))

        'Sell Goods
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Sell(BuildingID, SellAmount, SellType))

        'Pickup Any Exchange money
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Pickup_Exchange(BuildingID))

        'Send To Drop Off Point
        MoveCrewTo(Exchange_Point, Building_Point, Crew)

        'Drop Off Goods
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff(BuildingID, BuyAmount, BuyType))

        'Send To Safe Point
        MoveCrewTo(Building_Point, Safe_Point, Crew)

        'Dropoff Money
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Dropoff_Money(BuildingID))

        'Send To Drop Off Point
        MoveCrewTo(Safe_Point, Building_Point, Crew)

        'Add to workers
        'Crew.command_queue.Enqueue(New Crew.Command_Trans_Start())
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
                'If building.Value.Working_crew_list.Contains(Crew) Then building.Value.Working_crew_list.Remove(Crew)
                'If building.Value.Assigned_crew_list.Contains(Crew) Then building.Value.Assigned_crew_list.Remove(Crew)
                'If building.Value.Available_Transporters.Contains(Crew) Then building.Value.Available_Transporters.Remove(Crew)
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