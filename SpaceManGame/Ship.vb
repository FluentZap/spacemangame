<Serializable()> Public Class ship_room_type
    Inherits room_type
    Public Priority As Priority_enum = Priority_enum.Medium
    Sub New(ByVal type As tech_list_enum)
        Me.type = type
    End Sub

End Class

<Serializable()> Public Class ship_pipeline_type
    Public Type As Pipeline_type_enum
    Public Tile_Type As device_tile_type_enum
    Public Name As String
    Public Color As Color    
    Public Tiles As New Dictionary(Of PointI, Pipeline_sprite_enum)()

    'Public Connected_Devices As HashSet(Of Integer) = New HashSet(Of Integer)

    Public Supply As Decimal
    Public Supply_Limit As Decimal
    Public Supply_Drain As Decimal
    Public Efficiency As Decimal

    Sub New(ByVal Type As Pipeline_type_enum, ByVal Supply_Limit As Decimal, ByVal color As Color, ByVal Tile_Type As device_tile_type_enum, Optional ByVal Name As String = "")
        Me.Type = Type
        Me.Supply_Limit = Supply_Limit
        Me.Color = color
        Me.Tile_Type = Tile_Type
        Me.Name = Name
    End Sub


    Sub Calculate_efficiency()
        If Supply_Drain <= 0 Then Me.Efficiency = 1 : Exit Sub
        If Supply <= 0 Then Me.Efficiency = 0 : Exit Sub
        Me.Efficiency = Supply / Supply_Drain
        If Me.Efficiency > 1 Then Me.Efficiency = 1
    End Sub




End Class




Public Class Ship

    <Serializable()> Public MustInherit Class Ship_Command_script
        Public status As script_status_enum = script_status_enum.uninvoked
        Public type As ship_script_enum
    End Class

    <Serializable()> Public Class Command_move
        Inherits Ship_Command_script
        Public dest As PointD
        Public dx As Double
        Public dy As Double
        Public residual_movement As Double

        Sub New(ByRef dest As PointD)
            type = ship_script_enum.work_in_room
            Me.dest = dest
            residual_movement = Nothing
        End Sub

    End Class

    'Structure Capabilities_type




    'End Structure


    'connectivity layers
    'Dim id As Integer
    Public type As ship_type_enum 'Bio/Carbon fiber/Energy
    Public shipclass As ship_class_enum 'Frigate/Capital

    Public device_list As New Dictionary(Of Integer, Ship_device)()
    Public Crew_list As New Dictionary(Of Integer, Crew)()
    Public Officer_List As New Dictionary(Of Integer, Officer)()
    Public weapon_list As New Dictionary(Of Integer, Integer)()
    Public room_list As New Dictionary(Of Integer, ship_room_type)()

    Public pipeline_list As New Dictionary(Of Integer, ship_pipeline_type)()

    'Public 

    'Public Capabilities As Capabilities_type

    Public vector_velocity As PointD
    Public orbiting As Integer = -1    
    Public center_point As PointI
    Public Mass As Decimal
    Public rotation As Decimal
    Public angular_velocity As Decimal
    Public location As PointD

    Public Nav_Angle As Double
    Public Turn_Left As Boolean
    Public TurnPassZero As Boolean
    Public Stop_Rotation As Boolean
    Public NavControl As Boolean


    Public target_position As PointD
    Public target_rotation As Double


    'Dim destination As PointI
    Public Ship_ID As Integer
    Private path_find As A_star
    Public tile_map(,) As Ship_tile
    Public Build_ship As Ship
    Public shipsize As PointI
    Public ambient__light As Color
    Public Faction As Integer

    Public Weapon_control_groups As New Dictionary(Of Integer, Weapon_control_group)()
    Public Engine_Coltrol_Group As New Dictionary(Of Direction_Enum, Dictionary(Of Integer, VectorD))()

    Public Engine_BalanceX As Decimal
    Public Engine_BalanceY As Decimal
    Public Engine_BalanceR As Decimal

    Public device_animations As New Dictionary(Of Integer, Device_Animation_class)()

    Public ship_animations As New Dictionary(Of Integer, Basic_Animation)()

    Public open_doors As New HashSet(Of Integer)()

    Public Projectiles As New HashSet(Of Projectile)()

    Public Landed As Boolean
    Public LandedPlanet As Integer

    'Temps
    Public StartCy, StopCy As Double
    Public force As Decimal
    Public cycles As Integer = -2
    Public BurnTime As Integer


    Sub Load_Pathfinding()
        path_find = New A_star
        path_find.set_map(tile_map, shipsize)
    End Sub


    Sub New(ByVal id As Integer, ByVal location As PointD, ByVal type As ship_type_enum, ByVal shipclass As ship_class_enum, ByRef shipsize As PointI, ByVal Faction As Integer)
        ReDim Me.tile_map(shipsize.x, shipsize.y)
        Me.type = type
        Me.shipclass = shipclass
        Me.shipsize = shipsize
        Me.Ship_ID = id
    End Sub

    Sub New(ByVal s As Ship_Save)
        type = s.type
        shipclass = s.shipclass
        device_list = s.device_list
        Crew_list = s.Crew_list
        Officer_List = s.Officer_List
        weapon_list = s.weapon_list
        room_list = s.room_list
        pipeline_list = s.pipeline_list
        vector_velocity = s.vector_velocity
        center_point = s.center_point
        Mass = s.Mass
        rotation = s.rotation
        angular_velocity = s.angular_velocity
        location = s.location
        target_position = s.target_position
        target_rotation = s.target_rotation
        tile_map = s.tile_map
        shipsize = s.shipsize
        ambient__light = s.ambient__light
        Weapon_control_groups = s.Weapon_control_groups
        Engine_Coltrol_Group = s.Engine_Coltrol_Group

        Load_Pathfinding()
    End Sub


    Public Sub DoEvents()
        Run_animations()
        Populate_Rooms()
        run_crew_scrips(Crew_list)
        Update_Officers()

        close_doors()
        Process_Devices()
        Calculate_Engine_Ratios()
        Calculate_Engines()

        If NavControl = True Then Nav_Computer_Next()

        Process_Engines()

        cycles += 1
        If cycles < 50 Then
            For Each Device In u.Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.RotateR)
                'u.Ship_List(current_selected_ship_view).Set_Engine_Throttle(Device.Key, 1)
            Next
        End If

        If cycles = 50 Then
            cycles = 100
            For Each Device In u.Ship_List(current_selected_ship_view).Engine_Coltrol_Group(Direction_Enum.RotateR)
                'u.Ship_List(current_selected_ship_view).Set_Engine_Throttle(Device.Key, 0)
            Next
        End If

        If Landed = False Then
            Update_Locataion()
        End If
        'Update_Orbiting()

        Handle_Projectiles()
    End Sub


    Sub Update_Orbiting()
        If orbiting > -1 Then
            Dim planetPos As PointD
            Dim planetPosNext As PointD
            planetPos = Get_Planet_Location(orbiting)
            planetPosNext = Get_Planet_Location(orbiting, 0.05)
            location.x += planetPosNext.x - planetPos.x
            location.y += planetPosNext.y - planetPos.y
        End If
    End Sub

    Public Sub Refresh()
        Calculate_Weapon_Angles()
        Load_Pathfinding()
    End Sub


#Region "Devices"


    Sub Process_Devices()
        'Crew Efficiency calculation Start
        Dim Room_sci_points As Decimal
        Dim Room_eng_points As Decimal

        For Each room In room_list
            Room_eng_points = 0
            Room_sci_points = 0
            'calculate Room Supply/Demand
            For Each crew In room.Value.working_crew_list
                Room_sci_points += Crew_list(crew).Get_points.science
                Room_eng_points += Crew_list(crew).Get_points.engineering
            Next

            room.Value.required_crew_resources.engineering = 0
            room.Value.required_crew_resources.science = 0
            For Each dev In room.Value.device_list
                room.Value.required_crew_resources.engineering += Device_tech_list(device_list(dev).tech_ID).required_Points.engineering
                room.Value.required_crew_resources.science += Device_tech_list(device_list(dev).tech_ID).required_Points.science
            Next

            If room.Value.required_crew_resources.engineering > 0 Then room.Value.efficiency.engineering = Room_eng_points / room.Value.required_crew_resources.engineering
            If room.Value.required_crew_resources.science > 0 Then room.Value.efficiency.science = Room_sci_points / room.Value.required_crew_resources.science
            If room.Value.efficiency.engineering > 1 Then room.Value.efficiency.engineering = 1
            If room.Value.efficiency.science > 1 Then room.Value.efficiency.science = 1

            For Each dev In room.Value.device_list
                Dim Tech As device_data = Device_tech_list(device_list(dev).tech_ID)

                'Crew Efficiency calculation
                If Tech.required_Points.engineering > 0 AndAlso Tech.required_Points.science > 0 Then
                    'Both
                    If room.Value.efficiency.engineering > room.Value.efficiency.science Then
                        device_list(dev).crew_efficiency = room.Value.efficiency.science
                    Else
                        device_list(dev).crew_efficiency = room.Value.efficiency.engineering
                    End If

                ElseIf Tech.required_Points.engineering > 0 Then
                    'eng
                    device_list(dev).crew_efficiency = room.Value.efficiency.engineering
                ElseIf Tech.required_Points.science > 0 Then
                    'sci
                    device_list(dev).crew_efficiency = room.Value.efficiency.science
                Else
                    'No requirements
                    device_list(dev).crew_efficiency = 1
                End If

            Next
        Next

        'Crew Efficiency calculation End



        For Each pipeline In pipeline_list
            pipeline.Value.Supply = 0
            pipeline.Value.Supply_Drain = 0
        Next

        'Pipeline Efficiency
        Dim Pipeline_Drain As Decimal = 0
        Dim Pipeline_Supply As Decimal = 0

        'Do Device Logic

        'Add supply and drain for efficiency
        For Each Device In device_list
            If Device.Value.pipeline.Count > 0 Then
                For Each pipeline In Device.Value.pipeline
                    If pipeline.Pipeline_Connection > -1 Then

                        Select Case Device.Value.type
                            'Device calculations
                            Case device_type_enum.generator
                                If pipeline.Amount > 0 Then
                                    'Add to pipeline
                                    Device.Value.supply_efficiency = 1
                                    Dim crew_ef As Decimal = (Device.Value.crew_efficiency * 0.8D + 0.2D)
                                    Dim supply_ef As Decimal = Device.Value.supply_efficiency
                                    Dim device_max = pipeline.Amount
                                    Pipeline_Supply += (crew_ef * device_max * supply_ef)
                                Else
                                    'Add to drain
                                    Pipeline_Drain += -(Device.Value.crew_efficiency * pipeline.Amount)
                                End If

                            Case device_type_enum.engine
                                Dim crew_ef As Decimal = Device.Value.crew_efficiency * 0.8D + 0.2D
                                Dim supply_ef As Decimal = pipeline_list(pipeline.Pipeline_Connection).Efficiency
                                Dim device_max = pipeline.Amount
                                'Add to drain
                                'Throttled_Engine                                
                                Device.Value.device_efficiency = crew_ef * supply_ef
                                Pipeline_Drain += -device_max

                            Case device_type_enum.thruster
                                Dim supply_ef As Decimal = pipeline_list(pipeline.Pipeline_Connection).Efficiency
                                'Add to drain
                                Device.Value.device_efficiency = supply_ef
                                Pipeline_Drain += -pipeline.Amount
                        End Select



                        pipeline_list(pipeline.Pipeline_Connection).Supply += Pipeline_Supply
                        pipeline_list(pipeline.Pipeline_Connection).Supply_Drain += Pipeline_Drain
                        If pipeline_list(pipeline.Pipeline_Connection).Supply > pipeline_list(pipeline.Pipeline_Connection).Supply_Limit Then pipeline_list(pipeline.Pipeline_Connection).Supply = pipeline_list(pipeline.Pipeline_Connection).Supply_Limit
                        If pipeline_list(pipeline.Pipeline_Connection).Supply_Drain > pipeline_list(pipeline.Pipeline_Connection).Supply_Limit Then pipeline_list(pipeline.Pipeline_Connection).Supply_Drain = pipeline_list(pipeline.Pipeline_Connection).Supply_Limit


                        'If devices recieve from pipeline, set efficiency
                        'If pipeline.Amount < 0 Then Device.Value.supply_efficiency = pipeline_list(pipeline.Pipeline_Connection).Efficiency
                        Pipeline_Drain = 0
                        Pipeline_Supply = 0

                    End If
                Next
            End If
        Next


        For Each pipeline In pipeline_list
            'Calculate efficiency
            pipeline.Value.Calculate_efficiency()
        Next

    End Sub


    Sub Add_Device(ByVal Id As Integer, ByVal device As Ship_device, ByVal tiles As Dictionary(Of PointI, Device_tile), ByVal room As Integer)
        device_list.Add(Id, device)
        room_list(room).device_list.Add(Id)
        For Each pos In tiles.Keys
            tile_map(pos.x, pos.y).device_tile = tiles(pos)
            tile_map(pos.x, pos.y).walkable = walkable_type_enum.HasDevice
        Next
    End Sub

#End Region

#Region "ship scripts"

    Sub Activate_LandingBay(ByVal ID As Integer)
        If Landed = True Then
            Dim device As Ship_device = device_list(ID)
            If device.Activated = False Then
                
                For Each Tile In device.tile_list
                    Dim IdList(Officer_List.Count - 1) As Integer
                    Officer_List.Keys.CopyTo(IdList, 0)
                    For Each OID In IdList
                        If Officer_List(OID).find_tile = Tile Then
                            Officer_List.Remove(OID)
                            u.Planet_List(Loaded_planet).officer_list.Add(OID, u.Officer_List(OID))
                            u.Officer_List(OID).region = Officer_location_enum.Planet
                            u.Officer_List(OID).Location_ID = Loaded_planet
                            Dim newPos As PointD
                            newPos.x = u.Planet_List(Loaded_planet).landed_ships(current_selected_ship_view).x * 32
                            newPos.y = u.Planet_List(Loaded_planet).landed_ships(current_selected_ship_view).y * 32
                            newPos.x += u.Officer_List(OID).location.x
                            newPos.y += u.Officer_List(OID).location.y
                            u.Officer_List(OID).location = newPos
                        End If
                    Next

                Next
                device.Activated = True
            Else
                Dim P As Planet = u.Planet_List(LandedPlanet)

                For Each Tile In device.tile_list
                    Dim IdList(P.officer_list.Count - 1) As Integer
                    P.officer_list.Keys.CopyTo(IdList, 0)
                    For Each OID In IdList
                        Dim TilePos As PointI = P.officer_list(OID).find_tile
                        Dim ShipPos As PointI = P.landed_ships(Ship_ID)
                        If TilePos - ShipPos = Tile Then
                            P.officer_list.Remove(OID)
                            Officer_List.Add(OID, u.Officer_List(OID))
                            u.Officer_List(OID).region = Officer_location_enum.Ship
                            u.Officer_List(OID).Location_ID = Ship_ID
                            Dim newPos As PointD

                            'newPos.x = (TilePos.x - ShipPos.x) * 32
                            'newPos.y = (TilePos.y - ShipPos.y) * 32
                            newPos.x = u.Officer_List(OID).location.x - (ShipPos.x) * 32
                            newPos.y = u.Officer_List(OID).location.y - (ShipPos.y) * 32
                            u.Officer_List(OID).location = newPos
                        End If
                    Next
                Next

                'Dim IdList(u.Planet_List(Loaded_planet).officer_list.Count - 1) As Integer
                'u.Planet_List(Loaded_planet).officer_list.Keys.CopyTo(IdList, 0)
                'For Each OID In IdList
                'For Each Tile In device.tile_list
                'If Officer_List(OID).find_tile = Tile Then
                'Officer_List.Remove(OID)
                'u.Planet_List(Loaded_planet).officer_list.Add(OID, u.Officer_List(OID))
                'u.Officer_List(OID).region = Officer_location_enum.Planet
                'u.Officer_List(OID).Location_ID = Loaded_planet
                'Dim newPos As PointD
                'newPos.x = u.Planet_List(Loaded_planet).landed_ships(current_selected_ship_view).x * 32
                'newPos.y = u.Planet_List(Loaded_planet).landed_ships(current_selected_ship_view).y * 32
                'newPos.x += u.Officer_List(OID).location.x
                'newPos.y += u.Officer_List(OID).location.y
                'u.Officer_List(OID).location = newPos
                'End If
                'Next
                'Next


                device.Activated = False
            End If
        End If
    End Sub


    Function open_door(ByRef point As PointI) As Boolean
        If tile_map(point.x, point.y).device_tile IsNot Nothing Then
            If Not device_animations.ContainsKey(tile_map(point.x, point.y).device_tile.device_ID) AndAlso Not open_doors.Contains(tile_map(point.x, point.y).device_tile.device_ID) Then
                Dim Aniset As Animation_set_Enum
                Select Case device_list(tile_map(point.x, point.y).device_tile.device_ID).tech_ID
                    Case Is = tech_list_enum.Door_MK1
                        Aniset = Animation_set_Enum.Door_MK1_open
                    Case Is = tech_list_enum.Door_MK2
                        Aniset = Animation_set_Enum.Door_MK2_open
                    Case Is = tech_list_enum.Airlock_MK1
                        Aniset = Animation_set_Enum.Airlock_MK1_open
                End Select
                device_animations.Add(tile_map(point.x, point.y).device_tile.device_ID, New Device_Animation_class(Aniset, Special_animation_action_enum.Open_door))
                Return True
            End If
        End If
        Return False
    End Function


    Sub close_doors()
        Dim can_close As Boolean
        Dim closed_doors As HashSet(Of Integer) = New HashSet(Of Integer)
        For Each item In open_doors
            can_close = True
            For Each Tile In device_list(item).tile_list
                For Each crew In Crew_list
                    Dim rect As New Rectangle(Tile.x * 32, Tile.y * 32, 32, 32)
                    Dim rect2 As Rectangle = crew.Value.find_rect
                    If rect.IntersectsWith(rect2) Then can_close = False
                Next
                For Each Crew In Officer_List
                    Dim rect As New Rectangle(Tile.x * 32, Tile.y * 32, 32, 32)
                    If rect.IntersectsWith(Crew.Value.find_rect) Then can_close = False
                Next
            Next
            If Not device_animations.ContainsKey(item) Then
                If can_close = True Then
                    closed_doors.Add(item)
                    For Each Tile In device_list(item).tile_list
                        tile_map(Tile.x, Tile.y).walkable = walkable_type_enum.Door
                    Next
                    Dim Aniset As Animation_set_Enum
                    Select Case device_list(item).tech_ID
                        Case Is = tech_list_enum.Door_MK1
                            Aniset = Animation_set_Enum.Door_MK1_close
                        Case Is = tech_list_enum.Door_MK2
                            Aniset = Animation_set_Enum.Door_MK2_close
                        Case Is = tech_list_enum.Airlock_MK1
                            Aniset = Animation_set_Enum.Airlock_MK1_close
                    End Select
                    device_animations.Add(item, New Device_Animation_class(Aniset))
                End If

            End If
        Next

        For Each item In closed_doors
            open_doors.Remove(item)
        Next
    End Sub


    Sub Run_animations()

        Dim remove_list As HashSet(Of Integer) = New HashSet(Of Integer)

        For Each item In device_animations
            device_list(item.Key).Sprite_Animation_Key = item.Value.advance()
            If item.Value.Finished = True Then remove_list.Add(item.Key)
        Next

        Dim remove As Boolean = True
        For Each item In remove_list
            remove = True

            If device_animations(item).special_action = Special_animation_action_enum.Open_door Then
                For Each Tile In device_list(item).tile_list
                    tile_map(Tile.x, Tile.y).walkable = walkable_type_enum.OpenDoor
                Next
                'Load_Pathfinding()
                device_animations.Remove(item)
                remove = False

                Dim Aniset As Animation_set_Enum
                Select Case device_list(item).tech_ID
                    Case Is = tech_list_enum.Door_MK1
                        Aniset = Animation_set_Enum.Door_MK1_hold
                    Case Is = tech_list_enum.Door_MK2
                        Aniset = Animation_set_Enum.Door_MK2_hold
                    Case Is = tech_list_enum.Airlock_MK1
                        Aniset = Animation_set_Enum.Airlock_MK1_hold
                End Select

                device_animations.Add(item, New Device_Animation_class(Aniset, Special_animation_action_enum.Hold_door))
            End If


            If device_animations(item).special_action = Special_animation_action_enum.Hold_door Then
                open_doors.Add(item)
            End If



            If device_animations(item).special_action = Special_animation_action_enum.Close_door Then
                For Each Tile In device_list(item).tile_list
                    tile_map(Tile.x, Tile.y).walkable = walkable_type_enum.Door
                Next
                open_doors.Remove(item)
                'Load_Pathfinding()
            End If

            If remove = True Then device_animations.Remove(item)
        Next

        For Each item In Officer_List
            item.Value.Update_Sprite()
        Next

        For Each item In Crew_list
            item.Value.Update_Sprite()
        Next

    End Sub


    Sub Check_Fatigue()
        For Each Crew In Crew_list

        Next
    End Sub


    Sub Populate_Rooms()
        Dim idle_list As List(Of Integer) = New List(Of Integer)
        Dim working_list As New Dictionary(Of Integer, Priority_enum)()

        For Each Crew In Crew_list
            If Crew.Value.working = False Then
                If Crew.Value.Faction = Me.Faction Then idle_list.Add(Crew.Key)
            Else
                If tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).roomID > 1 Then
                    If Crew.Value.Faction = Me.Faction Then working_list.Add(Crew.Key, room_list(tile_map(Crew.Value.find_tile.x, Crew.Value.find_tile.y).roomID).Priority)
                End If
            End If
        Next

        If Crew_list.Count > 0 Then

        End If

        'Dim required_points As crew_resource_type
        For Each room In room_list
            Dim open_ap As Integer = 0
            'check for open access points
            For Each ap In room.Value.access_point
                If ap.Value.Manned = False Then open_ap += 1
            Next
            If open_ap > 0 Then
                Dim points As New crew_resource_type
                'calculate needed points
                For Each Crew In room.Value.working_crew_list
                    points.engineering += Crew_list(Crew).Get_points.engineering
                    points.science += Crew_list(Crew).Get_points.science
                Next
                For Each Crew In room.Value.assigned_crew_list
                    points.engineering += Crew_list(Crew).Get_points.engineering
                    points.science += Crew_list(Crew).Get_points.science
                Next

                If room.Value.required_crew_resources.engineering > points.engineering Then
                    For Each Crew In idle_list
                        If Crew_list(Crew).Get_points.engineering > 0 AndAlso Me.Faction = Crew_list(Crew).Faction Then
                            send_crew_towork(Crew, room.Key)
                            idle_list.Remove(Crew)
                            'Need to make remove list
                            Exit For
                        End If
                    Next
                End If

                If room.Value.required_crew_resources.science > points.science Then
                    For Each Crew In idle_list
                        If Crew_list(Crew).Get_points.science > 0 AndAlso Me.Faction = Crew_list(Crew).Faction Then
                            send_crew_towork(Crew, room.Key)
                            idle_list.Remove(Crew)
                            Exit For
                        End If
                    Next
                End If


                If idle_list.Count > 0 Then
                    'idle_list.Dequeue()
                End If
            End If
        Next


    End Sub


    Sub send_crew_towork(ByVal crew As Integer, ByVal room As Integer)
        Dim point As PointI
        For Each ap In room_list(room).access_point
            If ap.Value.Manned = False Then point = ap.Key
        Next

        path_find.set_start_end(Crew_list(crew).find_tile, point)
        path_find.find_path()
        If path_find.get_status = pf_status.path_found Then
            If Crew_list(crew).command_queue.Any Then
                Crew_list(crew).command_queue.Clear()
            End If
            room_list(room).access_point(point).Manned = True
            room_list(room).assigned_crew_list.Add(crew)
            Crew_list(crew).working = True
            Dim list As LinkedList(Of PointI)
            list = path_find.get_path()

            For Each dest In list
                If tile_map(dest.x, dest.y).walkable = walkable_type_enum.Door OrElse tile_map(dest.x, dest.y).walkable = walkable_type_enum.OpenDoor Then
                    Crew_list(crew).command_queue.Enqueue(New Crew.Command_Open_Door(dest))
                End If
                Crew_list(crew).command_queue.Enqueue(New Crew.Command_move(New PointD(dest.x * 32, dest.y * 32)))
            Next
            Crew_list(crew).command_queue.Enqueue(New Crew.Command_set_room())
        End If

    End Sub

#End Region

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
            If tile.x >= 0 AndAlso tile.x <= shipsize.x AndAlso tile.y >= 0 AndAlso tile.y <= shipsize.y Then
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
        tile_map(contact_Point.x, contact_Point.y).color = Color.Red

        For y = contact_Point.y - 2 To contact_Point.y + 2
            For x = contact_Point.x - 2 To contact_Point.x + 2
                If x >= 0 AndAlso x <= shipsize.x AndAlso y >= 0 AndAlso y <= shipsize.y Then
                    tile_map(x, y).color = Color.Red

                    For Each Crew In Crew_list
                        If Crew.Value.find_tile = New PointI(x, y) Then
                            Crew.Value.Health.Damage_All_Limbs(3)
                        End If

                    Next

                End If
            Next
        Next

    End Sub

    Sub Update_Locataion()        
        'Move Ship

        location.x += vector_velocity.x
        location.y += vector_velocity.y
        rotation += angular_velocity

        If rotation < 0 Then rotation += PI * 2D
        If rotation > PI * 2 Then rotation -= PI * 2D
        angular_velocity = 0
        vector_velocity.x = 0
        vector_velocity.y = 0

        If Math.Abs(vector_velocity.x) < Engine_BalanceX Then
            vector_velocity.x = 0
        Else
            'If vector_velocity.x > 0 Then vector_velocity.x -= Engine_BalanceX Else vector_velocity.x += Engine_BalanceX
        End If

        If Math.Abs(vector_velocity.y) < Engine_BalanceY Then
            vector_velocity.y = 0
        Else
            'If vector_velocity.y > 0 Then vector_velocity.y -= Engine_BalanceY Else vector_velocity.y += Engine_BalanceY
        End If


        If Math.Abs(angular_velocity) < Engine_BalanceR Then
            angular_velocity = 0
        Else
            'If angular_velocity > 0 Then angular_velocity -= Engine_BalanceR Else angular_velocity += Engine_BalanceR
        End If




        'vector_velocity.x = 0
        'vector_velocity.y = 0
        'angular_velocity = 0
        
        'FRICTION
        'vector_velocity.x -= vector_velocity.x * Drag
        'vector_velocity.y -= vector_velocity.y * Drag
        'angular_velocity -= angular_velocity * Drag
        'Dim limit As Integer = 1


        'Exit Sub

        If vector_velocity.x < 0 Then
            'If vector_velocity.x + Friction > 0 Then vector_velocity.x = 0 Else vector_velocity.x += Friction
        Else
            'If vector_velocity.x - Friction < 0 Then vector_velocity.x = 0 Else vector_velocity.x -= Friction
        End If

        If vector_velocity.y < 0 Then
            'If vector_velocity.y + Friction > 0 Then vector_velocity.y = 0 Else vector_velocity.y += Friction
        Else
            'If vector_velocity.y - Friction < 0 Then vector_velocity.y = 0 Else vector_velocity.y -= Friction
        End If


        If angular_velocity < 0 Then
            'If angular_velocity + Friction > 0 Then angular_velocity = 0 Else angular_velocity += Friction
        Else
            'If angular_velocity - Friction < 0 Then angular_velocity = 0 Else angular_velocity -= Friction
        End If


    End Sub

    Sub Calculate_Engines()
        'Finds engine ratio at full power
        Engine_Coltrol_Group.Clear()
        Engine_Coltrol_Group.Add(Direction_Enum.Top, New Dictionary(Of Integer, VectorD)())
        Engine_Coltrol_Group.Add(Direction_Enum.Bottom, New Dictionary(Of Integer, VectorD)())
        Engine_Coltrol_Group.Add(Direction_Enum.Left, New Dictionary(Of Integer, VectorD)())
        Engine_Coltrol_Group.Add(Direction_Enum.Right, New Dictionary(Of Integer, VectorD)())
        Engine_Coltrol_Group.Add(Direction_Enum.RotateL, New Dictionary(Of Integer, VectorD)())
        Engine_Coltrol_Group.Add(Direction_Enum.RotateR, New Dictionary(Of Integer, VectorD)())
        Engine_BalanceX = 0
        Engine_BalanceY = 0
        Engine_BalanceR = 0
        For Each Device In device_list
            If Device.Value.type = device_type_enum.engine OrElse Device.Value.type = device_type_enum.thruster Then

                'Dim accelerate As Decimal = Device.Value.Thrust_Max / Mass
                Dim accelerate As Decimal = Device_tech_list(Device.Value.tech_ID).Thrust_Power / Mass
                Dim Distance As Decimal
                Dim x As Decimal
                Dim y As Decimal
                Dim r As Decimal
                Select Case Device.Value.Thrust_Direction
                    Case Is = Direction_Enum.Top
                        x = 0
                        y = 1
                        Distance = -(center_point.x - Device.Value.Active_Point.x)
                    Case Is = Direction_Enum.Bottom
                        x = 0
                        y = -1
                        Distance = (center_point.x - Device.Value.Active_Point.x)
                    Case Is = Direction_Enum.Left
                        x = 1
                        y = 0
                        Distance = (center_point.y - Device.Value.Active_Point.y)
                    Case Is = Direction_Enum.Right
                        x = -1
                        y = 0
                        Distance = -(center_point.y - Device.Value.Active_Point.y)
                End Select

                'Distance = Distance
                'accelerate = accelerate - (Distance * accelerate)

                x = x * accelerate
                y = y * accelerate
                r = accelerate * (Distance / 32)

                'Need to add per engine stabilisers
                'Change location of check
                Engine_BalanceX += x
                Engine_BalanceX += y
                Engine_BalanceX += r

                If x > 0 Then
                    Engine_Coltrol_Group(Direction_Enum.Left).Add(Device.Key, New VectorD(x, y, r))
                ElseIf x < 0 Then
                    Engine_Coltrol_Group(Direction_Enum.Right).Add(Device.Key, New VectorD(x, y, r))
                End If

                If y > 0 Then
                    Engine_Coltrol_Group(Direction_Enum.Top).Add(Device.Key, New VectorD(x, y, r))
                ElseIf y < 0 Then
                    Engine_Coltrol_Group(Direction_Enum.Bottom).Add(Device.Key, New VectorD(x, y, r))
                End If

                If r > 0 Then
                    Engine_Coltrol_Group(Direction_Enum.RotateR).Add(Device.Key, New VectorD(x, y, r))
                ElseIf r < 0 Then
                    Engine_Coltrol_Group(Direction_Enum.RotateL).Add(Device.Key, New VectorD(x, y, r))
                End If

            End If


        Next
    End Sub


    Sub Calculate_Engine_Ratios()

        For Each Device In device_list
            If Device.Value.type = device_type_enum.engine OrElse Device.Value.type = device_type_enum.thruster Then
                'Dim accelerate As Decimal = Device_tech_list(Device.Value.tech_ID).Thrust_Power / Mass
                'Dim accelerateR As Decimal = Device_tech_list(Device.Value.tech_ID).Thrust_Power / (Mass * Mass)

                Dim Distance As Decimal
                Dim ActivePoint As PointI = Device.Value.Active_Point                
                Select Case Device.Value.Thrust_Direction
                    Case Is = Direction_Enum.Top
                        Distance = -(center_point.x - Device.Value.Active_Point.x)
                    Case Is = Direction_Enum.Bottom
                        Distance = (center_point.x - Device.Value.Active_Point.x)
                    Case Is = Direction_Enum.Left
                        Distance = (center_point.y - Device.Value.Active_Point.y)
                    Case Is = Direction_Enum.Right
                        Distance = -(center_point.y - Device.Value.Active_Point.y)
                End Select

                'Device.Value.Engine_Ratio.Rotation = accelerateR * Distance   '* 0.03125D
                'Device.Value.Engine_Ratio.Thrust = accelerate                

                Device.Value.Engine_Ratio.Rotation = Distance / (Mass * Mass)
                Device.Value.Engine_Ratio.Thrust = 1 / Mass

            End If
        Next

    End Sub



    Sub Nav_Computer_Next()

        If BurnTime > 0 Then
            SetAllEngines(Direction_Enum.RotateR, 1, thrust_type_enum.Thruster)
        Else
            SetAllEngines(Direction_Enum.RotateR, 0, thrust_type_enum.Thruster)
            NavControl = False
        End If
        BurnTime -= 1
    End Sub


    Sub Nav_Computer()

        Dim LeftR As Double
        Dim RightR As Double
        Dim RotNext As Double

        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateL)
            If device_list(engine.Key).type = device_type_enum.thruster Then
                LeftR += engine.Value.r
            End If
        Next

        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateR)
            If device_list(engine.Key).type = device_type_enum.thruster Then
                RightR += engine.Value.r
            End If
        Next

        'Check rotaion direction
        'target_rotation(-rotation > PI * 2 + rotation - target_rotation)

        'Dim tar As Double = If(target_rotation > 180, target_rotation - 360, target_rotation)
        'Dim rot As Double = If(rotation > 180, rotation - 360, rotation)

        'To zero_speed (angular_velocity / RightR)
        'To point (distance / angular_velocity)
        Dim ratio As Decimal
        If Stop_Rotation = False Then            

            If Turn_Left = True Then

                RotNext = rotation + angular_velocity + LeftR

                If rotation - RotNext > Nav_Angle Then
                    StartCy += 1                    
                    SetAllEngines(Direction_Enum.RotateL, 1, thrust_type_enum.Thruster)
                    Nav_Angle += rotation - RotNext

                ElseIf rotation - RotNext < Nav_Angle Then

                    Stop_Rotation = True
                    SetAllEngines(0)
                    Nav_Angle = 0
                End If

            Else
                RotNext = rotation + angular_velocity + RightR

                If RotNext - rotation < Nav_Angle Then
                    StartCy += 1

                    SetAllEngines(Direction_Enum.RotateR, 1, thrust_type_enum.Thruster)
                    Nav_Angle -= RotNext - rotation

                ElseIf RotNext - rotation > Nav_Angle Then
                    Stop_Rotation = True
                    ratio = CDec(Math.Abs((Nav_Angle - angular_velocity) / RightR))
                    StartCy += ratio
                    SetAllEngines(Direction_Enum.RotateR, ratio, thrust_type_enum.Thruster)
                    Nav_Angle = 0
                    'Nav_Angle -= RightR * ratio
                End If


                End If
            'End If
        Else

            'If Stop_Rotation = True Then

            If angular_velocity > 0 Then
                If angular_velocity + LeftR < 0 Then
                    NavControl = False
                    ratio = CDec(Math.Abs(angular_velocity / LeftR))
                    StopCy += ratio
                    SetAllEngines(Direction_Enum.RotateL, ratio, thrust_type_enum.Thruster)
                    SetAllEngines(Direction_Enum.RotateR, 0, thrust_type_enum.Thruster)
                Else
                    StopCy += 1
                    SetAllEngines(Direction_Enum.RotateL, 1, thrust_type_enum.Thruster)
                    SetAllEngines(Direction_Enum.RotateR, 0, thrust_type_enum.Thruster)
                End If
            End If

            If angular_velocity < 0 Then
                If angular_velocity + RightR > 0 Then
                    NavControl = False
                    ratio = CDec(Math.Abs(angular_velocity / RightR))
                    SetAllEngines(Direction_Enum.RotateL, 0, thrust_type_enum.Thruster)
                    SetAllEngines(Direction_Enum.RotateR, ratio, thrust_type_enum.Thruster)
                Else
                    SetAllEngines(Direction_Enum.RotateL, 0, thrust_type_enum.Thruster)
                    SetAllEngines(Direction_Enum.RotateR, 1, thrust_type_enum.Thruster)
                End If
            End If
        End If

        'rotation - t_rotation
        'Me.rotation = Me.target_rotation


    End Sub

    Function Check_Engine_Distance(ByVal Engine As Ship_device, ByVal TargetDistance As Decimal) As Integer
        Dim CoastTime As Integer
        Dim AccelDistance As Decimal
        Dim DecelDistance As Decimal
        Dim AccelTime As Integer
        Dim DecelTime As Integer

        For a = Engine.Engine_Power To Device_tech_list(Engine.tech_ID).Thrust_Power Step Device_tech_list(Engine.tech_ID).Acceleration
            AccelDistance += Engine.Engine_Ratio.Rotation * a
            AccelTime += 1
        Next

        For a = Device_tech_list(Engine.tech_ID).Thrust_Power To 0 Step -Device_tech_list(Engine.tech_ID).Deceleration
            DecelDistance += Engine.Engine_Ratio.Rotation * a
            DecelTime += 1
        Next

        'Check if target is higher then Accel + Decel
        If TargetDistance > AccelDistance + DecelDistance Then
            Dim RemainingDistance As Decimal = TargetDistance - (AccelDistance + DecelDistance)

            For a = 0 To RemainingDistance Step Device_tech_list(Engine.tech_ID).Thrust_Power * Engine.Engine_Ratio.Rotation
                CoastTime += 1
            Next
        Else
            'Turn shorter then Accel + Decel            
            AccelTime = 0
            DecelTime = 0
            AccelDistance = 0
            DecelDistance = 0

            Dim TAD, TDD As Decimal
            TAD = TargetDistance * (Device_tech_list(Engine.tech_ID).Acceleration / Device_tech_list(Engine.tech_ID).Deceleration) / 2
            TDD = TargetDistance * (Device_tech_list(Engine.tech_ID).Deceleration / Device_tech_list(Engine.tech_ID).Acceleration) / 2

            For a = Engine.Engine_Power To Device_tech_list(Engine.tech_ID).Thrust_Power Step Device_tech_list(Engine.tech_ID).Acceleration
                AccelDistance += Engine.Engine_Ratio.Rotation * a
                AccelTime += 1
                If AccelDistance >= TAD Then Exit For
            Next

            For a = Device_tech_list(Engine.tech_ID).Thrust_Power To 0 Step -Device_tech_list(Engine.tech_ID).Deceleration
                DecelDistance += Engine.Engine_Ratio.Rotation * a
                DecelTime += 1
                If DecelDistance >= TDD Then Exit For
            Next

        End If

        Return AccelTime + CoastTime
    End Function





    Sub SetFullTurn(ByVal theta As Decimal)
        target_rotation = theta

        Stop_Rotation = False

        Dim LeftR As Double
        Dim RightR As Double
        Dim Time As Integer

        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateL)
            If device_list(engine.Key).type = device_type_enum.thruster Then LeftR += engine.Value.r
            'Check_Engine_Distance(device_list(engine.Key))
        Next

        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateR)
            If device_list(engine.Key).type = device_type_enum.thruster Then RightR += engine.Value.r
            'Check_Engine_Distance(device_list(engine.Key))
            Time = Check_Engine_Distance(device_list(engine.Key), theta)
        Next

        BurnTime = Time

        'Cycles to stop at max velocity
        'Cycles to get to max velocity







        Dim distance As Double = target_rotation - rotation

        'Vel Final = initial(1-drag)^time

        'Check rotaion direction
        Dim tar As Double = target_rotation
        Dim rot As Double = rotation
        Dim left As Double
        Dim right As Double
        If tar < rot Then right = (tar + PI * 2) - rot
        If tar > rot Then right = tar - rot

        If tar > rot Then left = (PI * 2 + rot) - tar
        If tar < rot Then left = rot - tar

        'Turn Right
        If left > right OrElse left = right Then
            Turn_Left = False

            Dim ratio As Double = Math.Abs(LeftR) / Math.Abs(RightR)
            Nav_Angle = right * 0.5 '* ratio






            '+ If(LeftR <> 0, angular_velocity / LeftR, 0)
            'Nav_Angle = ratio * 0.5 * right(rotation + right * (ratio * 0.5)) + If(LeftR <> 0, angular_velocity / LeftR, 0)
            'Check if turn point is past zero
            'If Nav_Angle > PI * 2 Then Nav_Angle -= PI * 2

        End If

        'Turn Left
        If left < right Then
            Turn_Left = True

            Dim ratio As Double = Math.Abs(LeftR) / Math.Abs(RightR)
            'Turn_Point = tar + ((rotation - tar) * (ratio * 0.5)) '+ If(RightR <> 0, angular_velocity / RightR, 0)
            Nav_Angle = -left * ratio * 0.5 ' + If(RightR <> 0, angular_velocity / RightR, 0)
        End If

    End Sub


    Sub SetAllEngines(ByVal Direction As Direction_Enum, ByVal Percent As Decimal, Optional ByVal Type As thrust_type_enum = thrust_type_enum.All)
        If Percent < 0 Then Percent = 0
        If Percent > 1 Then Percent = 1
        For Each engine In Engine_Coltrol_Group(Direction)
            Select Case Type
                Case Is = thrust_type_enum.All
                    Set_Engine_Throttle(engine.Key, Percent)
                Case Is = thrust_type_enum.Thruster
                    If device_list(engine.Key).type = device_type_enum.thruster Then Set_Engine_Throttle(engine.Key, Percent)
                Case Is = thrust_type_enum.Engine
                    If device_list(engine.Key).type = device_type_enum.engine Then Set_Engine_Throttle(engine.Key, Percent)
            End Select
        Next
    End Sub

    Sub SetAllEngines(ByVal Percent As Decimal, Optional ByVal Type As thrust_type_enum = thrust_type_enum.All)
        If Percent < 0 Then Percent = 0
        If Percent > 1 Then Percent = 1
        For Each group In Engine_Coltrol_Group
            For Each engine In group.Value
                Select Case Type
                    Case Is = thrust_type_enum.All
                        Set_Engine_Throttle(engine.Key, Percent)
                    Case Is = thrust_type_enum.Thruster
                        If device_list(engine.Key).type = device_type_enum.thruster Then Set_Engine_Throttle(engine.Key, Percent)
                    Case Is = thrust_type_enum.Engine
                        If device_list(engine.Key).type = device_type_enum.engine Then Set_Engine_Throttle(engine.Key, Percent)
                End Select
            Next
        Next
    End Sub


    Sub accelerate(ByVal speed As Double)
        speed = speed / Mass
        Dim x = speed * Math.Cos((rotation)) ' * 0.017453292519943295)
        Dim y = speed * Math.Sin((rotation)) ' * 0.017453292519943295)
        vector_velocity.x -= y
        vector_velocity.y += x
    End Sub

    Sub Set_Engine_Throttle(ByVal DeviceID As Integer, ByVal percent As Decimal, Optional ByVal Add As Boolean = False)
        If Add = True Then
            Me.device_list(DeviceID).Throttle += percent
        Else
            Me.device_list(DeviceID).Throttle = percent
        End If

        If Me.device_list(DeviceID).Throttle > 1 Then Me.device_list(DeviceID).Throttle = 1
        If Me.device_list(DeviceID).Throttle < 0 Then Me.device_list(DeviceID).Throttle = 0
    End Sub


    Sub Fire_Weapon(ByVal DeviceID As Integer)
        Dim Fire_Point As PointD
        Dim Vector As PointD
        Dim rotate As Double

        Dim weapon As Ship_Weapon = Weapon_tech_list(Device_tech_list(device_list(DeviceID).tech_ID).Ship_Weapon)

        Fire_Point.x = device_list(DeviceID).Center_Distance * Math.Cos(rotation + device_list(DeviceID).Center_Angle) + location.x + center_point.x * 32 + 16
        Fire_Point.y = device_list(DeviceID).Center_Distance * Math.Sin(rotation + device_list(DeviceID).Center_Angle) + location.y + center_point.y * 32 + 16

        Select Case device_list(DeviceID).Device_Face
            Case Is = rotate_enum.OneEighty
                Vector.x = Math.Cos(rotation - 1.57079633)
                Vector.y = Math.Sin(rotation - 1.57079633)
                rotate = rotation + 3.14159265

            Case Is = rotate_enum.TwoSeventy
                Vector.x = Math.Cos(rotation)
                Vector.y = Math.Sin(rotation)
                rotate = rotation - 1.57079633

            Case Is = rotate_enum.Ninty
                Vector.x = Math.Cos(rotation + 3.14159265)
                Vector.y = Math.Sin(rotation + 3.14159265)
                rotate = rotation + 1.57079633

            Case Is = rotate_enum.Zero
                Vector.x = Math.Cos(rotation + 1.57079633)
                Vector.y = Math.Sin(rotation + 1.57079633)
                rotate = rotation
        End Select
        Dim rnd As Double = random(90, 10) / 100
        Vector.x *= -weapon.Projectile_Speed * rnd
        Vector.y *= -weapon.Projectile_Speed * rnd
        Vector.x += vector_velocity.x
        Vector.y += vector_velocity.y


        rnd = random(50, 100) / 100
        'u.Projectiles.Add(New Projectile(Fire_Point, Vector, rotate, CInt(100 * rnd), 1000))
        u.Projectiles.Add(New Projectile(Fire_Point, Vector, rotate, 1000))

    End Sub


    Sub Calculate_Weapon_Angles()
        Dim center As PointD
        Dim distance As Double
        For Each Device In device_list
            If Device.Value.type = device_type_enum.weapon Then
                center.x = (center_point.x * 32 - Device.Value.Active_Point.x * 32) ^ 2
                center.y = (center_point.y * 32 - Device.Value.Active_Point.y * 32) ^ 2
                distance = Math.Sqrt(center.x + center.y)
                Device.Value.Center_Angle = Math.Atan2((center_point.y - Device.Value.Active_Point.y), (center_point.x - Device.Value.Active_Point.x)) + 3.14159265358979
                Device.Value.Center_Distance = distance
            End If
        Next
    End Sub


    Sub Process_Engines()
        'Dim Thrust As Double
        'Dim ThrustMax As Double
        For Each Device In Me.device_list
            If Device.Value.type = device_type_enum.engine OrElse Device.Value.type = device_type_enum.thruster Then
                'Dim Thrust_Point As New PointD(Device.Value.Active_Point.ToPointD.x + 16, Device.Value.Active_Point.ToPointD.y + 16)                
                Dim T As Decimal = Device.Value.Throttle
                Dim Acc As Decimal = Device_tech_list(Device.Value.tech_ID).Acceleration
                Dim Dec As Decimal = Device_tech_list(Device.Value.tech_ID).Deceleration
                Dim TMax As Decimal = Device_tech_list(Device.Value.tech_ID).Thrust_Power
                Dim ATMax As Decimal = Device_tech_list(Device.Value.tech_ID).Thrust_Power * Device.Value.device_efficiency



                'Add acceleration To throttled engines                
                If Device.Value.Engine_Power < ATMax * T Then                    
                    Device.Value.Engine_Power += Acc
                    If Device.Value.Engine_Power > ATMax Then Device.Value.Engine_Power = ATMax
                End If

                'Dim min As Decimal = ATMax * T
                'If Device.Value.Engine_Power < ATMax * T Then
                'min = ThrustMax
                'End If


                If Device.Value.Engine_Power > ATMax * T Then
                    Device.Value.Engine_Power -= Dec
                    If Device.Value.Engine_Power < 0 Then Device.Value.Engine_Power = 0
                End If

                Apply_Engine(Device.Value)
                'Me.Apply_Force(Device.Value.Engine_Power, Device.Value.Active_Point.ToPointD, Device.Value.Thrust_Direction)
            End If

        Next
    End Sub

    Function Get_Direction_Theta(ByVal Direction As Direction_Enum) As Decimal
        Select Case Direction
            Case Is = Direction_Enum.Top
                Return PI
            Case Is = Direction_Enum.Bottom
                Return 0
            Case Is = Direction_Enum.Left
                Return -PI / 2
            Case Is = Direction_Enum.Right
                Return PI / 2
        End Select
        Return 0
    End Function


    Sub Apply_Force(ByVal force As Decimal, ByVal point As PointD, ByVal direction As Direction_Enum)
        Dim accelerate As Decimal = force / Mass
        Dim Distance As Decimal
        Dim x As Double
        Dim y As Double

        Select Case direction
            Case Is = Direction_Enum.Top
                x = Math.Cos(rotation + 3.14159265)
                y = Math.Sin(rotation + 3.14159265)
                Distance = CInt(center_point.x - point.x)
            Case Is = Direction_Enum.Bottom
                x = Math.Cos(rotation)
                y = Math.Sin(rotation)
                Distance = CInt(-(center_point.x - point.x))
            Case Is = Direction_Enum.Left
                x = Math.Cos(rotation - 1.57079633)
                y = Math.Sin(rotation - 1.57079633)
                Distance = CInt(center_point.y - point.y)
            Case Is = Direction_Enum.Right
                x = Math.Cos(rotation + 1.57079633)
                y = Math.Sin(rotation + 1.57079633)
                Distance = CInt(-(center_point.y - point.y))
        End Select

        angular_velocity += accelerate * Distance * 0.03125D ' / 32

        'Distance = -Math.Abs(Distance)
        'accelerate = accelerate - (Distance * accelerate)
        vector_velocity.x += y * accelerate
        vector_velocity.y -= x * accelerate
    End Sub


    Sub Apply_Engine(ByVal Engine As Ship_device)        
        Dim Ratio As Decimal = Engine.Engine_Power '/ Device_tech_list(Engine.tech_ID).Thrust_Power
        angular_velocity += Engine.Engine_Ratio.Rotation * Ratio
        Dim X As Double = Math.Sin(rotation + Get_Direction_Theta(Engine.Thrust_Direction))
        Dim Y As Double = Math.Cos(rotation + Get_Direction_Theta(Engine.Thrust_Direction))

        vector_velocity.x += Engine.Engine_Ratio.Thrust * Ratio * X
        vector_velocity.y += Engine.Engine_Ratio.Thrust * Ratio * -Y
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

        Dim IdList(Officer_List.Count - 1) As Integer
        Officer_List.Keys.CopyTo(IdList, 0)
        For Each id In IdList
            If Landed = True AndAlso tile_map(Officer_List(id).find_tile.x, Officer_List(id).find_tile.y).type = tile_type_enum.empty Then
                Officer_List.Remove(id)
                u.Planet_List(Loaded_planet).officer_list.Add(id, u.Officer_List(id))
                u.Officer_List(id).region = Officer_location_enum.Planet
                u.Officer_List(id).Location_ID = Loaded_planet
                Dim newPos As PointD
                newPos.x = u.Planet_List(Loaded_planet).landed_ships(current_selected_ship_view).x * 32
                newPos.y = u.Planet_List(Loaded_planet).landed_ships(current_selected_ship_view).y * 32
                newPos.x += u.Officer_List(id).location.x
                newPos.y += u.Officer_List(id).location.y
                u.Officer_List(id).location = newPos
            End If
        Next
    End Sub

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
        Dim Left, Right, Top, Bottom As Integer
        Left = CInt(Math.Floor(R.Left / 32))
        Right = CInt(Math.Floor((R.Right - 0.1) / 32))
        Top = CInt(Math.Floor(R.Top / 32))
        Bottom = CInt(Math.Floor((R.Bottom - 0.1) / 32))

        PLeft = Left : PRight = Right : PTop = Top : PBottom = Bottom

        If Left >= 0 AndAlso Right <= shipsize.y AndAlso Top >= 0 AndAlso Bottom <= shipsize.x Then
            Select Case Direction
                Case Is = Move_Direction.Up
                    If Not tile_map(Left, Top).walkable = walkable_type_enum.Walkable AndAlso Not tile_map(Left, Top).walkable = walkable_type_enum.OpenDoor Then CantMove = True
                    If Not tile_map(Right, Top).walkable = walkable_type_enum.Walkable AndAlso Not tile_map(Right, Top).walkable = walkable_type_enum.OpenDoor Then CantMove = True
                    If tile_map(Left, Top).walkable = walkable_type_enum.Door Then open_door(New PointI(Left, Top))
                    If tile_map(Right, Top).walkable = walkable_type_enum.Door Then open_door(New PointI(Right, Top))
                Case Is = Move_Direction.Down
                    If Not tile_map(Left, Bottom).walkable = walkable_type_enum.Walkable AndAlso Not tile_map(Left, Bottom).walkable = walkable_type_enum.OpenDoor Then CantMove = True
                    If Not tile_map(Right, Bottom).walkable = walkable_type_enum.Walkable AndAlso Not tile_map(Right, Bottom).walkable = walkable_type_enum.OpenDoor Then CantMove = True
                    If tile_map(Left, Bottom).walkable = walkable_type_enum.Door Then open_door(New PointI(Left, Bottom))
                    If tile_map(Right, Bottom).walkable = walkable_type_enum.Door Then open_door(New PointI(Right, Bottom))
                Case Is = Move_Direction.Left
                    If Not tile_map(Left, Top).walkable = walkable_type_enum.Walkable AndAlso Not tile_map(Left, Top).walkable = walkable_type_enum.OpenDoor Then CantMove = True
                    If Not tile_map(Left, Bottom).walkable = walkable_type_enum.Walkable AndAlso Not tile_map(Left, Bottom).walkable = walkable_type_enum.OpenDoor Then CantMove = True
                    If tile_map(Left, Top).walkable = walkable_type_enum.Door Then open_door(New PointI(Left, Top))
                    If tile_map(Left, Bottom).walkable = walkable_type_enum.Door Then open_door(New PointI(Left, Bottom))
                Case Is = Move_Direction.Right
                    If Not tile_map(Right, Top).walkable = walkable_type_enum.Walkable AndAlso Not tile_map(Right, Top).walkable = walkable_type_enum.OpenDoor Then CantMove = True
                    If Not tile_map(Right, Bottom).walkable = walkable_type_enum.Walkable AndAlso Not tile_map(Right, Bottom).walkable = walkable_type_enum.OpenDoor Then CantMove = True
                    If tile_map(Right, Top).walkable = walkable_type_enum.Door Then open_door(New PointI(Right, Top))
                    If tile_map(Right, Bottom).walkable = walkable_type_enum.Door Then open_door(New PointI(Right, Bottom))
            End Select
        Else
            CantMove = True
        End If

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

    Sub clear_fov()
        Dim x0, y0, radius As Integer
        'x0 = (current_player.GetLocation.intX + 16) \ 32
        'y0 = (current_player.GetLocation.intY + 16) \ 32
        'radius = current_player.view_range

        For x = -radius To radius
            For y = -radius To radius
                If x + x0 >= 0 AndAlso x + x0 <= shipsize.x AndAlso y + y0 >= 0 AndAlso y + y0 <= shipsize.y Then
                    If tile_map(x + x0, y + y0).viewed > tile_view_level.Unexplored Then
                        tile_map(x + x0, y + y0).viewed = tile_view_level.Explored
                    End If
                End If
            Next
        Next

    End Sub

    Sub update_fov()
        Dim x0, y0, radius As Integer
        'x0 = (current_player.GetLocation.intX + 16) \ 32
        'y0 = (current_player.GetLocation.intY + 16) \ 32
        'radius = current_player.view_range
        'r = Player.view_range
        Dim dim_range As Integer = 2
        Dim f As Integer = 1 - radius
        Dim ddF_x As Integer = 1
        Dim ddF_y As Integer = -2 * radius
        Dim x As Integer = 0
        Dim y As Integer = radius


        For y1 = y0 - radius To y0 + radius
            set_tile_view(x0, y1, 0)
        Next

        For x1 = x0 - radius To x0 + radius
            set_tile_view(x1, y0, 0)
        Next

        For x = 0 To y

            If f >= 0 Then
                y = y - 1
                ddF_y += 2
                f += ddF_y
            End If

            ddF_x += 2
            f += ddF_x

            For x1 = x0 - x To x0 + x
                set_tile_view(x1, y0 + y, 0)
            Next

            For x1 = x0 - x To x0 + x
                set_tile_view(x1, y0 - y, 0)
            Next

            For x1 = x0 - y To x0 + y
                set_tile_view(x1, y0 + x, 0)
            Next

            For x1 = x0 - y To x0 + y
                set_tile_view(x1, y0 - x, 0)
            Next
        Next x

        'If dim_range > 0 Then dim_range = dim_range - 1
        ' Next


    End Sub

    Sub set_tile_view(ByVal x As Integer, ByVal y As Integer, ByVal dim_range As Integer)
        If x >= 0 AndAlso x <= shipsize.x AndAlso y >= 0 AndAlso y <= shipsize.y Then
            Select Case dim_range
                Case 2
                    tile_map(x, y).viewed = tile_view_level.ViewedDark
                Case 1
                    tile_map(x, y).viewed = tile_view_level.ViewedDim
                Case 0
                    tile_map(x, y).viewed = tile_view_level.Viewed
            End Select
        End If

    End Sub

    Function GetOfficer() As Dictionary(Of Integer, Officer)
        Return Me.Officer_List
    End Function

    Sub AddOfficer(ByVal Id As Integer, ByRef officer As Officer)
        Me.Officer_List.Add(Id, officer)
    End Sub

    Sub SetTileMap(ByRef tile_map As Ship_tile(,))
        ReDim tile_map(tile_map.GetUpperBound(0), tile_map.GetUpperBound(1))
        Me.tile_map = tile_map
    End Sub

    Sub SetTile(ByRef point As PointI, ByRef Ship_tile As Ship_tile)
        Me.tile_map(point.x, point.y) = Ship_tile
    End Sub

    Sub SetTile(ByVal X As Integer, ByVal Y As Integer, ByRef Ship_tile As Ship_tile)
        Me.tile_map(X, Y) = Ship_tile
    End Sub

    Function GetTile(ByVal point As PointI) As Ship_tile
        Return tile_map(point.x, point.y)
    End Function

    Function GetShipSize() As PointI
        Return Me.shipsize
    End Function


    Function Get_Relative_Pos() As PointD
        Return New PointD(location.x + center_point.x * 32 + 16, location.y + center_point.y * 32 + 16)
    End Function


    Function Get_Center_Point() As PointD
        Return New PointD(center_point.x * 32 + 16, center_point.y * 32 + 16)
    End Function


    Function Clone() As Ship
        Dim cship As New Ship(0, New PointD(0, 0), ship_type_enum.carbon_fiber, shipclass, shipsize, Faction)

        cship.device_list = New Dictionary(Of Integer, Ship_device)()
        cship.Crew_list = New Dictionary(Of Integer, Crew)()
        cship.Officer_List = New Dictionary(Of Integer, Officer)()
        cship.weapon_list = New Dictionary(Of Integer, Integer)()
        cship.room_list = New Dictionary(Of Integer, ship_room_type)()
        cship.pipeline_list = New Dictionary(Of Integer, ship_pipeline_type)()
        For Each item In device_list
            cship.device_list.Add(item.Key, item.Value)
        Next
        For Each item In Crew_list
            cship.Crew_list.Add(item.Key, item.Value)
        Next
        For Each item In Officer_List
            cship.Officer_List.Add(item.Key, item.Value)
        Next
        For Each item In weapon_list
            cship.weapon_list.Add(item.Key, item.Value)
        Next
        For Each item In room_list
            cship.room_list.Add(item.Key, item.Value)
        Next
        For Each item In pipeline_list
            cship.pipeline_list.Add(item.Key, item.Value)
        Next

        cship.tile_map = DirectCast(tile_map.Clone, Ship_tile(,))

        Return cship
    End Function

    Function Get_Ship_Save() As Ship_Save
        Return New Ship_Save(Me)
    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class


<System.Serializable()> Public Class Ship_Save

    Public type As ship_type_enum
    Public shipclass As ship_class_enum

    Public device_list As New Dictionary(Of Integer, Ship_device)()
    Public Crew_list As New Dictionary(Of Integer, Crew)()
    Public Officer_List As New Dictionary(Of Integer, Officer)()
    Public weapon_list As New Dictionary(Of Integer, Integer)()
    Public room_list As New Dictionary(Of Integer, ship_room_type)()

    Public pipeline_list As New Dictionary(Of Integer, ship_pipeline_type)()

    Public vector_velocity As PointD
    Public center_point As PointI
    Public Mass As Decimal
    Public rotation As Decimal
    Public angular_velocity As Decimal
    Public location As PointD

    Public Turn_Point As Double
    Public Turn_Left As Boolean
    Public Stop_Rotation As Boolean



    Public target_position As PointD
    Public target_rotation As Double

    Public tile_map(,) As Ship_tile
    Public shipsize As PointI
    Public ambient__light As Color

    Public Weapon_control_groups As New Dictionary(Of Integer, Weapon_control_group)()
    Public Engine_Coltrol_Group As New Dictionary(Of Direction_Enum, Dictionary(Of Integer, VectorD))()

    Sub New(ByVal s As Ship)
        type = s.type
        shipclass = s.shipclass
        device_list = s.device_list
        Crew_list = s.Crew_list
        Officer_List = s.Officer_List
        weapon_list = s.weapon_list
        room_list = s.room_list
        pipeline_list = s.pipeline_list
        vector_velocity = s.vector_velocity
        center_point = s.center_point
        Mass = s.Mass
        rotation = s.rotation
        angular_velocity = s.angular_velocity
        location = s.location
        target_position = s.target_position
        target_rotation = s.target_rotation
        tile_map = s.tile_map
        shipsize = s.shipsize
        ambient__light = s.ambient__light
        Weapon_control_groups = s.Weapon_control_groups
        Engine_Coltrol_Group = s.Engine_Coltrol_Group
    End Sub




End Class