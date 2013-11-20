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
    Public Tiles As Dictionary(Of PointI, Pipeline_sprite_enum) = New Dictionary(Of PointI, Pipeline_sprite_enum)

    'Public Connected_Devices As HashSet(Of Integer) = New HashSet(Of Integer)

    Public Supply As Integer
    Public Supply_Limit As Integer
    Public Supply_Drain As Integer
    Public Efficiency As Integer

    Sub New(ByVal Type As Pipeline_type_enum, ByVal Supply_Limit As Integer, ByVal color As Color, ByVal Tile_Type As device_tile_type_enum, Optional ByVal Name As String = "")
        Me.Type = Type
        Me.Supply_Limit = Supply_Limit
        Me.Color = color
        Me.Tile_Type = Tile_Type
        Me.Name = Name
    End Sub


    Function Pipeline_efficiency() As Integer
        If Supply_Drain <= 0 Then Return Supply
        If Supply <= 0 Then Return 0
        Me.Efficiency = Convert.ToInt32(Supply / Supply_Drain * 100)
        Return Efficiency
    End Function

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

    Public device_list As Dictionary(Of Integer, Ship_device) = New Dictionary(Of Integer, Ship_device)
    Public Crew_list As Dictionary(Of Integer, Crew) = New Dictionary(Of Integer, Crew)
    Public Officer_List As Dictionary(Of Integer, Officer) = New Dictionary(Of Integer, Officer)
    Public weapon_list As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)
    Public room_list As Dictionary(Of Integer, ship_room_type) = New Dictionary(Of Integer, ship_room_type)

    Public pipeline_list As Dictionary(Of Integer, ship_pipeline_type) = New Dictionary(Of Integer, ship_pipeline_type)

    'Public 

    'Public Capabilities As Capabilities_type

    Public vector_velocity As PointD
    Public orbiting As Integer = -1    
    Public center_point As PointI
    Public Mass As Double
    Public rotation As Double
    Public angular_velocity As Double
    Public location As PointD

    Public Turn_Point As Double
    Public Turn_Left As Boolean
    Public Stop_Rotation As Boolean



    Public target_position As PointD
    Public target_rotation As Double


    'Dim destination As PointI
    Private path_find As A_star
    Public tile_map(,) As Ship_tile
    Public Build_ship As Ship
    Public shipsize As PointI
    Public ambient__light As Color
    Public Faction As Integer

    Public Weapon_control_groups As Dictionary(Of Integer, Weapon_control_group) = New Dictionary(Of Integer, Weapon_control_group)
    Public Engine_Coltrol_Group As Dictionary(Of Direction_Enum, Dictionary(Of Integer, VectorD)) = New Dictionary(Of Direction_Enum, Dictionary(Of Integer, VectorD))

    Public device_animations As Dictionary(Of Integer, Device_Animation_class) = New Dictionary(Of Integer, Device_Animation_class)

    Public ship_animations As Dictionary(Of Integer, Basic_Animation) = New Dictionary(Of Integer, Basic_Animation)

    Public open_doors As HashSet(Of Integer) = New HashSet(Of Integer)

    Public Projectiles As HashSet(Of Projectile) = New HashSet(Of Projectile)

    Sub Load_Pathfinding()
        path_find = New A_star
        path_find.set_map(tile_map, shipsize)
    End Sub


    Sub New(ByVal id As Integer, ByVal location As PointD, ByVal type As ship_type_enum, ByVal shipclass As ship_class_enum, ByRef shipsize As PointI, ByVal Faction As Integer)
        ReDim Me.tile_map(shipsize.x, shipsize.y)
        Me.type = type
        Me.shipclass = shipclass
        Me.shipsize = shipsize
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
        run_crew_scrips()
        Update_Officers()

        close_doors()
        Process_Devices()
        Calculate_Engines()

        Update_Locataion()
        Handle_Projectiles()
    End Sub

    Public Sub Refresh()
        Calculate_Weapon_Angles()
        Load_Pathfinding()
    End Sub


#Region "Devices"


    Sub Process_Devices()

        Dim Room_sci_points As Integer
        Dim Room_eng_points As Integer

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
                room.Value.required_crew_resources.engineering += device_list(dev).required_Points.engineering
                room.Value.required_crew_resources.science += device_list(dev).required_Points.science
            Next

            If room.Value.required_crew_resources.engineering > 0 Then room.Value.efficiency.engineering = Convert.ToInt32((Room_eng_points / room.Value.required_crew_resources.engineering) * 100)
            If room.Value.required_crew_resources.science > 0 Then room.Value.efficiency.science = Convert.ToInt32((Room_sci_points / room.Value.required_crew_resources.science) * 100)

            For Each dev In room.Value.device_list

                'Crew Efficiency calculation
                If device_list(dev).required_Points.engineering > 0 AndAlso device_list(dev).required_Points.science > 0 Then
                    'Both
                    If room.Value.efficiency.engineering > room.Value.efficiency.science Then
                        device_list(dev).crew_efficiency = room.Value.efficiency.science
                    Else
                        device_list(dev).crew_efficiency = room.Value.efficiency.engineering
                    End If

                ElseIf device_list(dev).required_Points.engineering > 0 Then
                    'eng
                    device_list(dev).crew_efficiency = room.Value.efficiency.engineering
                ElseIf device_list(dev).required_Points.science > 0 Then
                    'sci
                    device_list(dev).crew_efficiency = room.Value.efficiency.science
                Else
                    'No requirements
                    device_list(dev).crew_efficiency = 100
                End If


            Next
        Next


        For Each pipeline In pipeline_list
            pipeline.Value.Supply = 0
            pipeline.Value.Supply_Drain = 0
        Next

        'Pipeline Efficiency
        Dim Pipeline_Drain As Integer = 0
        Dim Pipeline_Supply As Integer = 0

        'Do Device Logic
        For Each Device In device_list
            If Device.Value.pipeline.Count > 0 Then
                For Each pipeline In Device.Value.pipeline
                    If pipeline.Pipeline_Connection > -1 Then

                        Select Case Device.Value.type
                            'Device calculations
                            Case device_type_enum.generator
                                If pipeline.Amount > 0 Then
                                    'Add to pipeline
                                    Device.Value.supply_efficiency = 100
                                    Dim crew_ef = (Device.Value.crew_efficiency * 0.01 * 0.8 + 0.2)
                                    Dim supply_ef = Device.Value.supply_efficiency * 0.01
                                    Dim device_max = pipeline.Amount
                                    Pipeline_Supply += Convert.ToInt32(crew_ef * device_max * supply_ef)
                                Else
                                    'Add to drain
                                    Pipeline_Drain += -Convert.ToInt32(Device.Value.crew_efficiency * 0.01 * pipeline.Amount)
                                End If

                            Case device_type_enum.engine
                                Dim crew_ef = (Device.Value.crew_efficiency * 0.01 * 0.8 + 0.2)
                                Dim supply_ef = Device.Value.supply_efficiency * 0.01
                                Dim device_max = pipeline.Amount
                                'Add to drain
                                'Pipeline_Drain += Convert.ToInt32(device_list(con_device).crew_efficiency * 0.01 * device_list(con_device).pipeline(pipeline.Value.Type))
                                'Throttled_Engine
                                If Device.Value.Throttled_Engine = True Then
                                    Pipeline_Drain += -Convert.ToInt32(crew_ef * device_max)
                                    Device.Value.Thrust_Power = Device_tech_list(Device.Value.tech_ID).Thrust_Power * crew_ef * supply_ef
                                Else 'Instant_Engine
                                    Pipeline_Drain += -Convert.ToInt32(crew_ef * device_max)
                                    Device.Value.Thrust_Power = Device_tech_list(Device.Value.tech_ID).Thrust_Power * crew_ef * supply_ef
                                End If


                            Case device_type_enum.thruster
                                Dim supply_ef = Device.Value.supply_efficiency * 0.01
                                'Add to drain                        
                                Pipeline_Drain += -Convert.ToInt32(pipeline.Amount)
                                Device.Value.Thrust_Power = Device_tech_list(Device.Value.tech_ID).Thrust_Power * supply_ef
                        End Select



                        pipeline_list(pipeline.Pipeline_Connection).Supply += Pipeline_Supply
                        pipeline_list(pipeline.Pipeline_Connection).Supply_Drain += Pipeline_Drain
                        If pipeline_list(pipeline.Pipeline_Connection).Supply > pipeline_list(pipeline.Pipeline_Connection).Supply_Limit Then pipeline_list(pipeline.Pipeline_Connection).Supply = pipeline_list(pipeline.Pipeline_Connection).Supply_Limit
                        If pipeline_list(pipeline.Pipeline_Connection).Supply_Drain > pipeline_list(pipeline.Pipeline_Connection).Supply_Limit Then pipeline_list(pipeline.Pipeline_Connection).Supply_Drain = pipeline_list(pipeline.Pipeline_Connection).Supply_Limit


                        'If devices recieve from pipeline, set efficiency
                        If pipeline.Amount < 0 Then Device.Value.supply_efficiency = pipeline_list(pipeline.Pipeline_Connection).Efficiency
                        Pipeline_Drain = 0
                        Pipeline_Supply = 0

                    End If
                Next
            
            End If
        Next


        For Each pipeline In pipeline_list
            'Calculate efficiency
            pipeline.Value.Pipeline_efficiency()            
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

    Sub run_crew_scrips()

        'Dim crew_temp(crew_list.Keys.Count - 1) As Integer
        ' crew_list.Keys.CopyTo(crew_temp, 0)        
        Dim completed As List(Of Integer) = New List(Of Integer)
        For Each key In Crew_list.Keys
            If Crew_list(key).command_queue.Any Then

                Select Case Crew_list(key).command_queue.First.type
                    Case crew_script_enum.move_to_tile : crew_script_move_to_tile(Crew_list(key))

                    Case crew_script_enum.hold : crew_script_hold(Crew_list(key))

                    Case crew_script_enum.set_room : crew_script_set_room(Crew_list(key), key)

                    Case crew_script_enum.remove_room : crew_script_remove_room(Crew_list(key), key)

                    Case crew_script_enum.open_door : crew_script_open_door(Crew_list(key))

                End Select
                If Crew_list(key).command_queue.First.status = script_status_enum.complete Then Crew_list(key).command_queue.Dequeue()
            End If
        Next

    End Sub

#Region "ship scripts"

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
        Dim working_list As Dictionary(Of Integer, Priority_enum) = New Dictionary(Of Integer, Priority_enum)

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

#Region "crew scripts"
    'Must pass byval

    Private Sub crew_script_move_to_tile(ByVal crew_member As Crew)
        Dim command As Crew.Command_move = DirectCast(crew_member.command_queue.First, Crew.Command_move)

        Dim dest As PointD = command.dest
        Dim location As PointD = crew_member.location

        If command.status = script_status_enum.uninvoked Then
            command.status = script_status_enum.running

            If location.x < dest.x Then ' need to go right
                command.dx = 1
            ElseIf location.x > dest.x Then ' need to go left
                command.dx = -1
            Else
                command.dx = 0
            End If
            If location.y < dest.y Then ' need to go down
                command.dy = 1
            ElseIf location.y > dest.y Then ' need to go up
                command.dy = -1
            Else
                command.dy = 0
            End If
            ' go at speed for straight movement
            ' go at sqrt(.5) speed for diagonal
            If command.dx <> 0 AndAlso command.dy <> 0 Then
                command.dx *= DIAGONAL_SPEED_MODIFIER
                command.dy *= DIAGONAL_SPEED_MODIFIER
            End If
        End If

        Dim dx As Double = command.dx * crew_member.speed
        Dim dy As Double = command.dy * crew_member.speed
        Dim x_reached As Boolean = False
        Dim y_reached As Boolean = False


        If command.status <> script_status_enum.complete Then
            If (location.x <= dest.x AndAlso location.x + dx + ACCEPTABLE_MARGIN >= dest.x) _
            OrElse (location.x >= dest.x AndAlso location.x + dx - ACCEPTABLE_MARGIN <= dest.x) Then ' if we have reached the x goal...
                x_reached = True
            End If
            If (location.y <= dest.y AndAlso location.y + dy + ACCEPTABLE_MARGIN >= dest.y) _
            OrElse (location.y >= dest.y AndAlso location.y + dy - ACCEPTABLE_MARGIN <= dest.y) Then ' if we have reached the y goal...
                y_reached = True
            End If

            If x_reached AndAlso y_reached Then

                command.status = script_status_enum.complete
                If dx <> 0.0 AndAlso dy <> 0.0 Then ' moving on a diagonal
                    command.residual_movement = crew_member.speed - Math.Abs(location.x - dest.x) / DIAGONAL_SPEED_MODIFIER
                ElseIf dx = 0.0 AndAlso dy <> 0.0 Then ' moving vertically
                    command.residual_movement = crew_member.speed - Math.Abs(location.y - dest.y)
                ElseIf dx <> 0.0 AndAlso dy = 0.0 Then ' moving horizontally
                    command.residual_movement = crew_member.speed - Math.Abs(location.x - dest.x)
                Else ' not moving? alright then...
                    command.residual_movement = 0.0
                End If
                location.x = dest.x ' ...even it up
                location.y = dest.y
                crew_member.location = location ' write back
                resolve_residual_mvt(crew_member)
                location = crew_member.location ' write in

            Else
                If Not x_reached Then ' if we have not reached the x goal...
                    location.x += dx ' ...add distance
                End If
                If Not y_reached Then ' if we have not reached the y goal...
                    location.y += dy ' ...add distance
                End If
            End If
        End If

        'write back
        crew_member.location = location
        'Console.Write(location.x)
        'Console.Write(", ")
        'Console.Write(location.y)
        'Console.WriteLine()
    End Sub

    Private Sub crew_script_open_door(ByVal crew_member As Crew)
        Dim command As Crew.Command_Open_Door = DirectCast(crew_member.command_queue.First, Crew.Command_Open_Door)

        If command.Set_open = False Then
            If open_door(command.Door_location) = True Then command.Set_open = True
        End If

        If tile_map(command.Door_location.x, command.Door_location.y).walkable = walkable_type_enum.OpenDoor Then
            crew_member.command_queue.First.status = script_status_enum.complete
        End If
    End Sub

    Private Sub resolve_residual_mvt(ByVal crew As Crew)

        If crew.command_queue.Count >= 2 AndAlso crew.command_queue(1).type = crew_script_enum.move_to_tile Then
            Dim first_command As Crew.Command_move = DirectCast(crew.command_queue.First, Crew.Command_move)
            Dim command As Crew.Command_move = DirectCast(crew.command_queue(1), Crew.Command_move)
            Dim dest As PointD = command.dest
            Dim location As PointD = crew.location

            If command.status = script_status_enum.uninvoked Then
                command.status = script_status_enum.running

                If location.x < dest.x Then ' need to go right
                    command.dx = 1
                ElseIf location.x > dest.x Then ' need to go left
                    command.dx = -1
                Else
                    command.dx = 0
                End If
                If location.y < dest.y Then ' need to go down
                    command.dy = 1
                ElseIf location.y > dest.y Then ' need to go up
                    command.dy = -1
                Else
                    command.dy = 0
                End If
                ' go at speed for straight movement
                ' go at sqrt(.5) speed for diagonal
                If command.dx <> 0 AndAlso command.dy <> 0 Then
                    command.dx *= DIAGONAL_SPEED_MODIFIER
                    command.dy *= DIAGONAL_SPEED_MODIFIER
                End If
            End If

            crew.location.x += command.dx * first_command.residual_movement
            crew.location.y += command.dy * first_command.residual_movement

        End If
    End Sub

    Private Sub crew_script_hold(ByRef crew_member As Crew)
        Dim command As Crew.Command_hold = DirectCast(crew_member.command_queue.First, Crew.Command_hold)
        If command.status = script_status_enum.uninvoked Then
            command.status = script_status_enum.running
        End If
        If command.count < command.hold_length Then
            command.count += 1
        Else
            command.status = script_status_enum.complete
        End If
    End Sub

    Private Sub crew_script_set_room(ByVal crew_member As Crew, ByVal id As Integer)
        Dim pos As PointI
        pos.x = crew_member.find_tile.x
        pos.y = crew_member.find_tile.y
        room_list(tile_map(pos.x, pos.y).roomID).working_crew_list.Add(id)
        If room_list(tile_map(pos.x, pos.y).roomID).assigned_crew_list.Contains(id) Then room_list(tile_map(pos.x, pos.y).roomID).assigned_crew_list.Remove(id)
        crew_member.command_queue.First.status = script_status_enum.complete
    End Sub

    Private Sub crew_script_remove_room(ByRef crew_member As Crew, ByVal id As Integer)
        Dim pos As PointI

        pos.x = Convert.ToInt32(crew_member.location.x + 16 * 0.03125)
        pos.y = Convert.ToInt32(crew_member.location.y + 16 * 0.03125)
        room_list(tile_map(pos.x, pos.y).roomID).working_crew_list.Remove(id)

        crew_member.command_queue.First.status = script_status_enum.complete
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
        'Nav_Computer()
        'Move Ship

        location.x += vector_velocity.x * 5
        location.y += vector_velocity.y * 5
        rotation += angular_velocity
        If rotation < 0 Then rotation += PI * 2
        If rotation > PI * 2 Then rotation -= PI * 2

        angular_velocity -= angular_velocity / 100
        vector_velocity.x -= vector_velocity.x / 100
        vector_velocity.y -= vector_velocity.y / 100

        If orbiting > -1 Then            
            Dim planetPos As PointD
            Dim planetPosNext As PointD
            planetPos = Get_Planet_Location(orbiting)
            planetPosNext = Get_Planet_Location(orbiting, 0.05)
            location.x += planetPosNext.x - planetPos.x
            location.y += planetPosNext.y - planetPos.y
        End If

    End Sub

    Sub Calculate_Engines()
        Engine_Coltrol_Group.Clear()
        Engine_Coltrol_Group.Add(Direction_Enum.Top, New Dictionary(Of Integer, VectorD))
        Engine_Coltrol_Group.Add(Direction_Enum.Bottom, New Dictionary(Of Integer, VectorD))
        Engine_Coltrol_Group.Add(Direction_Enum.Left, New Dictionary(Of Integer, VectorD))
        Engine_Coltrol_Group.Add(Direction_Enum.Right, New Dictionary(Of Integer, VectorD))
        Engine_Coltrol_Group.Add(Direction_Enum.RotateL, New Dictionary(Of Integer, VectorD))
        Engine_Coltrol_Group.Add(Direction_Enum.RotateR, New Dictionary(Of Integer, VectorD))

        For Each Device In device_list
            If Device.Value.type = device_type_enum.engine OrElse Device.Value.type = device_type_enum.thruster Then

                Dim accelerate As Double = Device.Value.Thrust_Power / Mass
                Dim Distance As Double
                Dim x As Double
                Dim y As Double
                Dim r As Double
                Select Case Device.Value.Thrust_Direction
                    Case Is = Direction_Enum.Top
                        x = 0
                        y = 1
                        Distance = (center_point.x - Device.Value.Active_Point.x)
                    Case Is = Direction_Enum.Bottom
                        x = 0
                        y = -1
                        Distance = -(center_point.x - Device.Value.Active_Point.x)
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
                r = accelerate * Distance / 32

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

    Sub Nav_Computer()

        Dim LeftR As Double
        Dim RightR As Double

        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateL)
            LeftR += engine.Value.r
        Next
        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateR)
            RightR += engine.Value.r
        Next

        'Check rotaion direction
        'target_rotation(-rotation > PI * 2 + rotation - target_rotation)

        'Dim tar As Double = If(target_rotation > 180, target_rotation - 360, target_rotation)
        'Dim rot As Double = If(rotation > 180, rotation - 360, rotation)

        'To zero_speed (angular_velocity / RightR)
        'To point (distance / angular_velocity)

        If Stop_Rotation = False Then
            If rotation <> target_rotation Then

                If Turn_Left = True Then
                    If rotation > Turn_Point Then
                        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateL)
                            Fire_Engine(engine.Key, 1)
                        Next
                    End If

                    If rotation < Turn_Point Then
                        Stop_Rotation = True
                    End If

                Else
                    If rotation < Turn_Point Then
                        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateR)
                            Fire_Engine(engine.Key, 1)
                        Next
                    End If

                    If rotation > Turn_Point Then
                        Stop_Rotation = True
                    End If

                End If
            End If
        End If

        If Stop_Rotation = True Then
            If angular_velocity > 0 Then
                If angular_velocity + LeftR < 0 Then
                    angular_velocity = 0
                Else
                    For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateL)
                        Fire_Engine(engine.Key, 1)
                    Next
                End If
            ElseIf angular_velocity < 0 Then
                If angular_velocity + RightR > 0 Then
                    angular_velocity = 0
                Else
                    For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateR)
                        Fire_Engine(engine.Key, 1)
                    Next
                End If
            End If
        End If

        'rotation - t_rotation
        'Me.rotation = Me.target_rotation


    End Sub

    Sub SetFullTurn(ByVal theta As Double)
        target_rotation = theta

        Stop_Rotation = False

        Dim LeftR As Double
        Dim RightR As Double

        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateL)
            LeftR += engine.Value.r
        Next
        For Each engine In Engine_Coltrol_Group(Direction_Enum.RotateR)
            RightR += engine.Value.r
        Next

        Dim distance As Double = target_rotation - rotation

        'Check rotaion direction        
        Dim tar As Double = target_rotation
        Dim rot As Double = rotation
        Dim left As Double
        Dim right As Double
        If tar < rot Then right = (tar + PI * 2) - rot
        If tar > rot Then right = tar - rot

        If tar > rot Then left = (PI * 2 + rot) - tar
        If tar < rot Then left = rot - tar

        If left > right Then
            Dim ratio As Double = Math.Abs(LeftR / RightR)
            Turn_Point = (rotation + right * (ratio / 2)) + If(LeftR <> 0, angular_velocity / LeftR, 0)
            Turn_Left = False
        End If


        If left < right Then
            Dim ratio As Double = Math.Abs(LeftR / RightR)
            Turn_Point = (rotation - left * (ratio / 2)) + If(RightR <> 0, angular_velocity / RightR, 0)
            Turn_Left = True
        End If

    End Sub

    Sub accelerate(ByVal speed As Double)
        speed = speed / Mass
        Dim x = speed * Math.Cos((rotation)) ' * 0.017453292519943295)
        Dim y = speed * Math.Sin((rotation)) ' * 0.017453292519943295)
        vector_velocity.x -= y
        vector_velocity.y += x
    End Sub

    Sub Fire_Engine(ByVal DeviceID As Integer, ByVal percent As Double)
        'Me.Apply_Force(device_list(DeviceID).Thrust_Power * percent, device_list(DeviceID).Active_Point.ToPointD, device_list(DeviceID).Thrust_Direction)


        Me.Apply_Force(device_list(DeviceID).Thrust_Power * percent, device_list(DeviceID).Active_Point.ToPointD, device_list(DeviceID).Thrust_Direction)

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
        u.Projectiles.Add(New Projectile(Fire_Point, Vector, rotate, CInt(100 * rnd), 1000))

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






    End Sub


    Sub Apply_Force(ByVal force As Double, ByVal point As PointD, ByVal direction As Direction_Enum)
        Dim accelerate As Double = force / Mass
        Dim Distance As Double
        Dim x As Double
        Dim y As Double

        Select Case direction
            Case Is = Direction_Enum.Top
                x = Math.Cos(rotation + 3.14159265)
                y = Math.Sin(rotation + 3.14159265)
                Distance = CInt(-(center_point.x - point.x))
            Case Is = Direction_Enum.Bottom
                x = Math.Cos(rotation)
                y = Math.Sin(rotation)
                Distance = CInt(center_point.x - point.x)
            Case Is = Direction_Enum.Left
                x = Math.Cos(rotation - 1.57079633)
                y = Math.Sin(rotation - 1.57079633)
                Distance = CInt(center_point.y - point.y)
            Case Is = Direction_Enum.Right
                x = Math.Cos(rotation + 1.57079633)
                y = Math.Sin(rotation + 1.57079633)
                Distance = CInt(-(center_point.y - point.y))
        End Select

        angular_velocity += accelerate * (Distance / 32)

        'Distance = -Math.Abs(Distance)
        'accelerate = accelerate - (Distance * accelerate)
        vector_velocity.x += y * accelerate
        vector_velocity.y -= x * accelerate
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
            If Pos(a).x >= 0 And Pos(a).x <= shipsize.x And Pos(a).y >= 0 And Pos(a).y <= shipsize.y Then
                If tile_map(Convert.ToInt32(Pos(a).x), Convert.ToInt32(Pos(a).y)).walkable = walkable_type_enum.Walkable OrElse tile_map(Convert.ToInt32(Pos(a).x), Convert.ToInt32(Pos(a).y)).walkable = walkable_type_enum.OpenDoor Then
                    b = b + 1
                ElseIf tile_map(Convert.ToInt32(Pos(a).x), Convert.ToInt32(Pos(a).y)).walkable = walkable_type_enum.Door Then
                    open_door(New PointI(Convert.ToInt32(Pos(a).x), Convert.ToInt32(Pos(a).y)))
                End If
            End If
        Next
        'clear_fov()
        If b = 4 Then
            Officer_List(Id).Move(vector)
        ElseIf b <> 0 Then
            If vector.x < 0 Then Officer_List(Id).SetLocation(New PointD(Convert.ToInt32(Pos(0).x * 32 + 32 - 8), Officer_List(Id).GetLocation.y))
            If vector.x > 0 Then Officer_List(Id).SetLocation(New PointD(Convert.ToInt32(Pos(1).x * 32 - 32 + 8), Officer_List(Id).GetLocation.y))
            If vector.y < 0 Then Officer_List(Id).SetLocation(New PointD(Officer_List(Id).GetLocation.x, Convert.ToInt32(Pos(0).y * 32 + 32 - 27)))
            If vector.y > 0 Then Officer_List(Id).SetLocation(New PointD(Officer_List(Id).GetLocation.x, Convert.ToInt32(Pos(2).y * 32 - 32)))
        End If
        'update_fov()
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

        cship.device_list = New Dictionary(Of Integer, Ship_device)
        cship.Crew_list = New Dictionary(Of Integer, Crew)
        cship.Officer_List = New Dictionary(Of Integer, Officer)
        cship.weapon_list = New Dictionary(Of Integer, Integer)
        cship.room_list = New Dictionary(Of Integer, ship_room_type)
        cship.pipeline_list = New Dictionary(Of Integer, ship_pipeline_type)
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

    Public device_list As Dictionary(Of Integer, Ship_device) = New Dictionary(Of Integer, Ship_device)
    Public Crew_list As Dictionary(Of Integer, Crew) = New Dictionary(Of Integer, Crew)
    Public Officer_List As Dictionary(Of Integer, Officer) = New Dictionary(Of Integer, Officer)
    Public weapon_list As Dictionary(Of Integer, Integer) = New Dictionary(Of Integer, Integer)
    Public room_list As Dictionary(Of Integer, ship_room_type) = New Dictionary(Of Integer, ship_room_type)

    Public pipeline_list As Dictionary(Of Integer, ship_pipeline_type) = New Dictionary(Of Integer, ship_pipeline_type)

    Public vector_velocity As PointD
    Public center_point As PointI
    Public Mass As Double
    Public rotation As Double
    Public angular_velocity As Double
    Public location As PointD

    Public Turn_Point As Double
    Public Turn_Left As Boolean
    Public Stop_Rotation As Boolean



    Public target_position As PointD
    Public target_rotation As Double

    Public tile_map(,) As Ship_tile
    Public shipsize As PointI
    Public ambient__light As Color

    Public Weapon_control_groups As Dictionary(Of Integer, Weapon_control_group) = New Dictionary(Of Integer, Weapon_control_group)
    Public Engine_Coltrol_Group As Dictionary(Of Direction_Enum, Dictionary(Of Integer, VectorD)) = New Dictionary(Of Direction_Enum, Dictionary(Of Integer, VectorD))

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