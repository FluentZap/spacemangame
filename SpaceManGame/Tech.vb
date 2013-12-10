Public Module Tech

    Enum tech_list_enum
        Empty = -1
        All_rooms
        Engineering
        Bridge
        Crew_Quarters
        Corridor
        Airlock
        Cargo_Bay
        Messdeck
        Armory
        Brigg
        Shuttle_Bay
        Fighter_Bay
        Science_Room
        Sick_Bay
        Transporter_Room
        Hydroponics

        Armor
        ArmorLV1
        ArmorLV2
        Corvette_Blue_Prints
        Fighter_Blue_Prints
        Frigate_Blue_Prints
        Destroyer_Blue_Prints
        Cruiser_Blue_Prints
        Battle_Cruiser_Blue_Prints
        BattleShip_Blue_Prints
        Carrier_Blue_Prints

        Piping
        Pipe_Energy_100

        Pipe_Plasma_100

        Pipe_Data_100

        'Devices
        Bridge_Control_Panel
        Computer_MK1


        Combustion_Engine_MK1
        Combustion_Engine_MK2
        Combustion_Engine_MK3

        Energy_Generator_MK1

        Projectile_MK1
        Door_MK1
        Door_MK2
        Airlock_MK1
        Thruster_MK1

        Weapon__Projectile_MK1
    End Enum

    Enum planet_tech_list_enum
        Jump_drive
        Combustion_engine
        Thrusters

        Rail_guns
        Missiles

        Wep_Rail_gun_mechine
        Wep_Rail_gun_Pistol
        Wep_Rail_gun_Sniper_Rifle

        Radar_Sensors

        Carbon_fiber_construction
        Corvette_blueprints
        Fighter_blueprints

        Ore_extractor
        Carbon_extractor

    End Enum

    Sub Set_Tech_Level(ByVal Level As Integer, ByVal planet As Integer)

        Select Case Level
            Case 1
                u.planets(planet).tech.Add(planet_tech_list_enum.Jump_drive)
                u.planets(planet).tech.Add(planet_tech_list_enum.Combustion_engine)
                u.planets(planet).tech.Add(planet_tech_list_enum.Thrusters)
                u.planets(planet).tech.Add(planet_tech_list_enum.Rail_guns)
                u.planets(planet).tech.Add(planet_tech_list_enum.Missiles)
                u.planets(planet).tech.Add(planet_tech_list_enum.Wep_Rail_gun_mechine)
                u.planets(planet).tech.Add(planet_tech_list_enum.Wep_Rail_gun_Pistol)
                u.planets(planet).tech.Add(planet_tech_list_enum.Wep_Rail_gun_Sniper_Rifle)
                u.planets(planet).tech.Add(planet_tech_list_enum.Radar_Sensors)
                u.planets(planet).tech.Add(planet_tech_list_enum.Carbon_fiber_construction)
                u.planets(planet).tech.Add(planet_tech_list_enum.Corvette_blueprints)
                u.planets(planet).tech.Add(planet_tech_list_enum.Fighter_blueprints)
                u.planets(planet).tech.Add(planet_tech_list_enum.Ore_extractor)
                u.planets(planet).tech.Add(planet_tech_list_enum.Carbon_extractor)

                If u.planets(planet).special_tech = Planet_special_tech_enum.Biology Then

                End If

            Case 2
            Case 3
            Case 4
            Case 5
            Case 6
            Case 7
            Case 8
            Case 9
            Case 10
        End Select





    End Sub

    Enum Tech_type_enum
        Weapon
        Equipment
        Room
        Pipeline
        Device
        Planet_tech
        Menu_item
        Blueprint
        Armor
    End Enum

    Class Tech_type
        Public Base_type As Tech_type_enum
        Public Name As String
        Public Description As String
        Public Device_room_type As tech_list_enum
        Public tile_type As Integer
        Public weight As Double
        Public device_map(,) As Ship_tile
        Public device_data As device_data


        Sub New(ByVal Name As String, ByVal base_type As Tech_type_enum)
            Me.Name = Name
            Me.Base_type = base_type
        End Sub

        'Room constructor
        Sub New(ByVal Name As String, ByVal base_type As Tech_type_enum, ByVal tile_type As tile_type_enum, ByVal Weight As Double)
            Me.Name = Name
            Me.Base_type = base_type
            Me.tile_type = tile_type
            Me.weight = Weight
        End Sub


        Sub New(ByVal Name As String, ByVal base_type As Tech_type_enum, ByVal device_tile_type As device_tile_type_enum)
            Me.Name = Name
            Me.Base_type = base_type
            Me.tile_type = device_tile_type
        End Sub

        Sub New(ByVal Name As String, ByVal base_type As Tech_type_enum, ByVal device_room_type As tech_list_enum)
            Me.Name = Name
            Me.Base_type = base_type
            Me.Device_room_type = device_room_type
        End Sub

        Sub New(ByVal Name As String, ByVal base_type As Tech_type_enum, ByVal tile_type As tile_type_enum, ByVal device_room_type As tech_list_enum)
            Me.Name = Name
            Me.Base_type = base_type
            Me.tile_type = tile_type
            Me.Device_room_type = device_room_type
        End Sub
    End Class



    Sub Load_tech_list()
        'Tech_list.Add(tech_list_enum.Pipeline, New Tech_type("Pipeline", Tech_type_enum.Room, tile_type_enum.Pipeline_1))
        Tech_list.Add(tech_list_enum.Empty, New Tech_type("", Tech_type_enum.Weapon, tile_type_enum.empty, 0))

        Tech_list.Add(tech_list_enum.All_rooms, New Tech_type("All Rooms", Tech_type_enum.Room, tile_type_enum.empty, 0))

        'Tech_list.Add(tech_list_enum.Armor, New Tech_type("Armor", Tech_type_enum.Room, tile_type_enum.empty, 1))

        'Rooms
        Tech_list.Add(tech_list_enum.Airlock, New Tech_type("Airlock", Tech_type_enum.Room, tile_type_enum.Airlock_1, 2))
        Tech_list.Add(tech_list_enum.Armory, New Tech_type("Armory", Tech_type_enum.Room, tile_type_enum.Armory_1, 2))
        Tech_list.Add(tech_list_enum.Bridge, New Tech_type("Bridge", Tech_type_enum.Room, tile_type_enum.Bridge_1, 3))
        Tech_list.Add(tech_list_enum.Brigg, New Tech_type("Brigg", Tech_type_enum.Room, tile_type_enum.Brigg_1, 2))
        Tech_list.Add(tech_list_enum.Cargo_Bay, New Tech_type("Cargo Bay", Tech_type_enum.Room, tile_type_enum.Cargo_Bay_1, 2))
        Tech_list.Add(tech_list_enum.Corridor, New Tech_type("Corridor", Tech_type_enum.Room, tile_type_enum.Corridor_1, 2))
        Tech_list.Add(tech_list_enum.Crew_Quarters, New Tech_type("Crew Quarters", Tech_type_enum.Room, tile_type_enum.Crew_Quarters_1, 2))
        Tech_list.Add(tech_list_enum.Engineering, New Tech_type("Engineering", Tech_type_enum.Room, tile_type_enum.Engineering_1, 3))
        Tech_list.Add(tech_list_enum.Fighter_Bay, New Tech_type("Fighter Bay", Tech_type_enum.Room, tile_type_enum.Fighter_Bay_1, 2))
        Tech_list.Add(tech_list_enum.Hydroponics, New Tech_type("Hydroponics", Tech_type_enum.Room, tile_type_enum.Hydroponics_1, 2))
        Tech_list.Add(tech_list_enum.Messdeck, New Tech_type("Mess Deck", Tech_type_enum.Room, tile_type_enum.MessDeck_1, 2))
        Tech_list.Add(tech_list_enum.Science_Room, New Tech_type("Science Room", Tech_type_enum.Room, tile_type_enum.Science_Room_1, 2))
        Tech_list.Add(tech_list_enum.Shuttle_Bay, New Tech_type("Shuttle Bay", Tech_type_enum.Room, tile_type_enum.Shuttle_Bay_1, 2))
        Tech_list.Add(tech_list_enum.Sick_Bay, New Tech_type("Sick Bay", Tech_type_enum.Room, tile_type_enum.Sick_Bay_1, 2))
        Tech_list.Add(tech_list_enum.Transporter_Room, New Tech_type("Transporter Room", Tech_type_enum.Room, tile_type_enum.Transporter_Room_1, 2))

        'Blueprints
        Tech_list.Add(tech_list_enum.Corvette_Blue_Prints, New Tech_type("Corvette", Tech_type_enum.Blueprint))
        Tech_list.Add(tech_list_enum.Fighter_Blue_Prints, New Tech_type("Fighter", Tech_type_enum.Blueprint))
        Tech_list.Add(tech_list_enum.Frigate_Blue_Prints, New Tech_type("Frigate", Tech_type_enum.Blueprint))
        Tech_list.Add(tech_list_enum.Destroyer_Blue_Prints, New Tech_type("Destroyer", Tech_type_enum.Blueprint))
        Tech_list.Add(tech_list_enum.Cruiser_Blue_Prints, New Tech_type("Cruiser", Tech_type_enum.Blueprint))
        Tech_list.Add(tech_list_enum.Battle_Cruiser_Blue_Prints, New Tech_type("Battle Cruiser", Tech_type_enum.Blueprint))
        Tech_list.Add(tech_list_enum.BattleShip_Blue_Prints, New Tech_type("Battleship", Tech_type_enum.Blueprint))
        Tech_list.Add(tech_list_enum.Carrier_Blue_Prints, New Tech_type("Carrier", Tech_type_enum.Blueprint))

        'Pipelines
        Tech_list.Add(tech_list_enum.Pipe_Energy_100, New Tech_type("Energy 100", Tech_type_enum.Pipeline, device_tile_type_enum.Pipeline_energy_Small))
        Tech_list.Add(tech_list_enum.Pipe_Data_100, New Tech_type("Data 100", Tech_type_enum.Pipeline, device_tile_type_enum.Pipeline_data_Small))


        'devices
        Tech_list.Add(tech_list_enum.Combustion_Engine_MK1, New Tech_type("Combustion Engine MK1", Tech_type_enum.Device, tech_list_enum.Engineering))
        Tech_list.Add(tech_list_enum.Combustion_Engine_MK2, New Tech_type("Combustion Engine MK2", Tech_type_enum.Device, tech_list_enum.Engineering))
        Tech_list.Add(tech_list_enum.Combustion_Engine_MK3, New Tech_type("Combustion Engine MK3", Tech_type_enum.Device, tech_list_enum.Engineering))
        Tech_list.Add(tech_list_enum.Thruster_MK1, New Tech_type("Thruster MK1", Tech_type_enum.Device, tech_list_enum.All_rooms))
        Tech_list.Add(tech_list_enum.Energy_Generator_MK1, New Tech_type("Energy Generator MK1", Tech_type_enum.Device, tech_list_enum.Engineering))
        Tech_list.Add(tech_list_enum.Door_MK1, New Tech_type("Basic Door", Tech_type_enum.Device, tech_list_enum.All_rooms))
        Tech_list.Add(tech_list_enum.Door_MK2, New Tech_type("Basic Energy Door", Tech_type_enum.Device, tech_list_enum.All_rooms))

        Tech_list.Add(tech_list_enum.Airlock_MK1, New Tech_type("Airlock", Tech_type_enum.Device, tech_list_enum.Airlock))


        Tech_list.Add(tech_list_enum.Bridge_Control_Panel, New Tech_type("Basic Energy Door", Tech_type_enum.Device, tech_list_enum.Bridge))

        Tech_list.Add(tech_list_enum.Computer_MK1, New Tech_type("Basic Main Computer", Tech_type_enum.Device, tech_list_enum.Bridge))


        'Armor
        Tech_list.Add(tech_list_enum.ArmorLV1, New Tech_type("ArmorLV1", Tech_type_enum.Armor, tile_type_enum.Armor_1, 6))
        Tech_list.Add(tech_list_enum.ArmorLV2, New Tech_type("ArmorLV2", Tech_type_enum.Armor, tile_type_enum.Armor_2, 6))


        'Device weapons
        Tech_list.Add(tech_list_enum.Projectile_MK1, New Tech_type("Energy Beam MK1", Tech_type_enum.Device, tech_list_enum.All_rooms))
        Load_Description()
    End Sub



    Sub Load_Description()
        Tech_list(tech_list_enum.Airlock).Description = "The Airlock is used to allow the passage into and out of the vessel."        

    End Sub



    Class device_data
        Public cmap()() As Byte
        Public type As device_type_enum
        Public tile_type As device_tile_type_enum
        Public center As PointI
        Public integrity As Integer
        Public required_Points As crew_resource_type
        Public pipeline As HashSet(Of Device_Pipeline) = New HashSet(Of Device_Pipeline)
        Public IsDoor As Boolean
        Public Rotatable As Boolean
        Public Flipable As flip_enum
        Public Ship_Weapon As Ship_weapon_enum
        'Engine
        Public Acceleration As Double
        Public Deceleration As Double
        Public Thrust_Power As Double        
        Public Active_Point As PointI


        Sub New(ByVal type As device_type_enum, ByVal integrity As Integer, ByVal required_points As crew_resource_type, ByVal center As PointI, ByVal cmap()() As Byte, ByVal pipeline As HashSet(Of Device_Pipeline), ByVal tile_type As device_tile_type_enum, ByVal Rotatable As Boolean, ByVal Flipable As flip_enum, Optional ByVal IsDoor As Boolean = False)
            Me.type = type
            Me.integrity = integrity
            Me.required_Points = required_points
            Me.cmap = cmap
            Me.center = center
            Me.pipeline = pipeline
            Me.tile_type = tile_type
            Me.IsDoor = IsDoor
            Me.Rotatable = Rotatable
            Me.Flipable = Flipable
        End Sub

        Sub New(ByVal type As device_type_enum, ByVal integrity As Integer, ByVal required_points As crew_resource_type, ByVal center As PointI, ByVal cmap()() As Byte, ByVal pipeline As HashSet(Of Device_Pipeline), ByVal tile_type As device_tile_type_enum, ByVal Rotatable As Boolean, ByVal Flipable As flip_enum, ByVal Ship_Weapon As Ship_weapon_enum)
            Me.type = type
            Me.integrity = integrity
            Me.required_Points = required_points
            Me.cmap = cmap
            Me.center = center
            Me.pipeline = pipeline
            Me.tile_type = tile_type
            Me.IsDoor = IsDoor
            Me.Rotatable = Rotatable
            Me.Flipable = Flipable
            Me.Ship_Weapon = Ship_Weapon
        End Sub


        Function Contains_Pipeline(ByVal Pipeline_Type As Pipeline_type_enum) As Boolean
            For Each pipe In Me.pipeline
                If pipe.Type = Pipeline_Type Then Return True
            Next
            Return False
        End Function



    End Class







    Sub set_device_map()
        'C_map Legend        
        Const B As Byte = 0
        Const R As Byte = 1
        Const W As Byte = 2
        Const E As Byte = 3

        'Const AE As Byte = 4
        Const RE As Byte = 5
        Const WE As Byte = 6
        Const EE As Byte = 7

        Const AP As Byte = 8



        '0, Blank / Does not set base to room
        '1, On room tile
        '2, On wall tile
        '3, On empty tile

        '4, On anything / Does not set sprite / set's device
        '5, On room tile / Does not set sprite / set's device
        '6, On wall tile / Does not set sprite / set's device
        '7, On empty tile / Does not set sprite / set's device
        '8, Access point

        'S_map verticle width
        'Dim s_map() As Byte = New Byte() {3, 3, 3, 3}
        'Thrust in 1k Newton * 60

        Dim Pipe As HashSet(Of Device_Pipeline) = New HashSet(Of Device_Pipeline)


        Dim c_map()() As Byte = New Byte(12)() {}
        c_map(0) = New Byte() {B, B, AP, B, B}
        c_map(1) = New Byte() {B, R, R, R, B}
        c_map(2) = New Byte() {B, R, R, R, B}
        c_map(3) = New Byte() {B, W, W, W, B}
        c_map(4) = New Byte() {B, E, E, E, B}
        c_map(5) = New Byte() {B, EE, EE, EE, B}
        c_map(6) = New Byte() {B, EE, EE, EE, B}
        c_map(7) = New Byte() {B, EE, EE, EE, B}
        c_map(8) = New Byte() {B, EE, EE, EE, B}
        c_map(9) = New Byte() {B, EE, EE, EE, B}
        c_map(10) = New Byte() {B, EE, EE, EE, B}
        c_map(11) = New Byte() {B, EE, EE, EE, B}
        c_map(12) = New Byte() {B, EE, EE, EE, B}

        Pipe = New HashSet(Of Device_Pipeline)
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Energy, -25))
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Data, -10))
        Device_tech_list.Add(tech_list_enum.Combustion_Engine_MK1, New device_data(device_type_enum.engine, 100, New crew_resource_type(20, 0), New PointI(2, 2), c_map, Pipe, device_tile_type_enum.Combustion_engine_MK1, True, flip_enum.Both))
        Device_tech_list(tech_list_enum.Combustion_Engine_MK1).Thrust_Power = 10
        Device_tech_list(tech_list_enum.Combustion_Engine_MK1).Acceleration = 0.01
        Device_tech_list(tech_list_enum.Combustion_Engine_MK1).Deceleration = 0.01
        Device_tech_list(tech_list_enum.Combustion_Engine_MK1).Active_Point = New PointI(2, 4)



        c_map = New Byte(2)() {}
        c_map(0) = New Byte() {B, E, B}
        c_map(1) = New Byte() {WE, W, WE}
        c_map(2) = New Byte() {R, R, R}
        Pipe = New HashSet(Of Device_Pipeline)
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Energy, -10))
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Data, -10))
        Device_tech_list.Add(tech_list_enum.Projectile_MK1, New device_data(device_type_enum.weapon, 100, New crew_resource_type(0, 0), New PointI(1, 1), c_map, Pipe, device_tile_type_enum.Projectile_MK1, True, flip_enum.None, Ship_weapon_enum.Projectile_MK1))
        Device_tech_list(tech_list_enum.Projectile_MK1).Active_Point = New PointI(1, 0)


        c_map = New Byte(3)() {}
        c_map(0) = New Byte() {RE, RE}
        c_map(1) = New Byte() {W, W}
        c_map(2) = New Byte() {W, W}
        c_map(3) = New Byte() {RE, RE}
        Pipe = New HashSet(Of Device_Pipeline)
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Data, -5))
        Device_tech_list.Add(tech_list_enum.Door_MK1, New device_data(device_type_enum.door, 100, New crew_resource_type(0, 0), New PointI(0, 2), c_map, Pipe, device_tile_type_enum.Door_MK1, True, flip_enum.None, True))


        c_map = New Byte(3)() {}
        c_map(0) = New Byte() {RE}
        c_map(1) = New Byte() {W}
        c_map(2) = New Byte() {W}
        c_map(3) = New Byte() {RE}
        Pipe = New HashSet(Of Device_Pipeline)
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Data, -5))
        Device_tech_list.Add(tech_list_enum.Door_MK2, New device_data(device_type_enum.door, 100, New crew_resource_type(0, 0), New PointI(0, 2), c_map, Pipe, device_tile_type_enum.Door_MK2, True, flip_enum.None, True))



        c_map = New Byte(2)() {}
        c_map(0) = New Byte() {RE, R, R, RE}
        c_map(1) = New Byte() {RE, R, R, RE}
        c_map(2) = New Byte() {RE, AP, AP, RE}
        Pipe = New HashSet(Of Device_Pipeline)
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Energy, 50))        
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Data, -10))
        Device_tech_list.Add(tech_list_enum.Energy_Generator_MK1, New device_data(device_type_enum.generator, 100, New crew_resource_type(10, 0), New PointI(1, 1), c_map, Pipe, device_tile_type_enum.Generator, True, flip_enum.Both))



        c_map = New Byte(0)() {}
        c_map(0) = New Byte() {EE, EE, EE, E, W}
        Pipe = New HashSet(Of Device_Pipeline)
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Energy, -5))
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Data, -10))
        Device_tech_list.Add(tech_list_enum.Thruster_MK1, New device_data(device_type_enum.thruster, 100, New crew_resource_type(0, 0), New PointI(4, 0), c_map, Pipe, device_tile_type_enum.Thruster_MK1, True, flip_enum.Flip_X))
        Device_tech_list(tech_list_enum.Thruster_MK1).Thrust_Power = 1
        Device_tech_list(tech_list_enum.Thruster_MK1).Acceleration = 1
        Device_tech_list(tech_list_enum.Thruster_MK1).Deceleration = 1
        Device_tech_list(tech_list_enum.Thruster_MK1).Active_Point = New PointI(3, 0)



        c_map = New Byte(1)() {}
        c_map(0) = New Byte() {R, R, R}
        c_map(1) = New Byte() {R, AP, R}
        Pipe = New HashSet(Of Device_Pipeline)
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Data, 10))        
        Device_tech_list.Add(tech_list_enum.Bridge_Control_Panel, New device_data(device_type_enum.generator, 100, New crew_resource_type(0, 0), New PointI(1, 1), c_map, Pipe, device_tile_type_enum.Bridge_Control_Panel, True, flip_enum.Both))



        c_map = New Byte(1)() {}
        c_map(0) = New Byte() {R}
        c_map(1) = New Byte() {R}
        Pipe = New HashSet(Of Device_Pipeline)
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Data, 50))
        Device_tech_list.Add(tech_list_enum.Computer_MK1, New device_data(device_type_enum.generator, 100, New crew_resource_type(0, 0), New PointI(0, 1), c_map, Pipe, device_tile_type_enum.Computer_MK1, False, flip_enum.None))


        c_map = New Byte(0)() {}
        c_map(0) = New Byte() {W, W}

        Pipe = New HashSet(Of Device_Pipeline)
        Pipe.Add(New Device_Pipeline(Pipeline_type_enum.Data, 50))
        Device_tech_list.Add(tech_list_enum.Airlock_MK1, New device_data(device_type_enum.door, 100, New crew_resource_type(0, 0), New PointI(0, 1), c_map, Pipe, device_tile_type_enum.Airlock_MK1, True, flip_enum.Both, True))



    End Sub









    Sub Set_Weapon_Tech()
        Weapon_tech_list.Add(Ship_weapon_enum.Projectile_MK1, New Ship_Weapon(Weapon_fire_mode_enum.Projectile_Single, 100, 1, 100, 1, 1))

    End Sub



    Enum Buff_List_Enum As Byte
        Buffs


    End Enum


    Enum Ability_List_Enum As Byte
        Mage__Fireball


    End Enum

    Enum Class_List_Enum As Integer
        Empty = -1
        'Base Classes
        Engineer
        Security
        Scientist
        Aviator

        'Extended Classes
        Mage
        Spellsword
        Thief
        Inventor
        Shadow
        PlaceHolder
        Eye_Of_The_Placeholder
    End Enum


    Class Officer_Class
        Public ClassID As Class_List_Enum        
        Public Base_Class As Class_List_Enum = Class_List_Enum.Empty
        Public Base_Class_2 As Class_List_Enum = Class_List_Enum.Empty
        Public Experance As Integer
        Public Skill_Points As Byte
        Public Level As Byte
        Public LevelCap As Byte


        Sub Load_Lists(ByVal ClassID As Class_List_Enum)
            Select Case ClassID
                Case Is = Class_List_Enum.Mage
                    LevelCap = 20
                    Base_Class = Class_List_Enum.Scientist
                    Base_Class_2 = Class_List_Enum.Engineer

                Case Is = Class_List_Enum.Spellsword
                    LevelCap = 20
                    Base_Class = Class_List_Enum.Scientist
                    Base_Class_2 = Class_List_Enum.Security

                Case Is = Class_List_Enum.Shadow
                    LevelCap = 20
                    Base_Class = Class_List_Enum.Security
            End Select
        End Sub

        Sub New(ByVal ClassID As Class_List_Enum, ByVal Experance As Integer, ByVal Level As Byte)
            Me.ClassID = ClassID
            Me.Experance = Experance
            Me.Level = Level
            Me.LevelCap = LevelCap
            Load_Lists(ClassID)
        End Sub

    End Class



    Enum Skill_List_Enum As Integer
        None = -1
        Engineer__A
        Engineer__B
        Engineer__C

        Security__A
        Security__B
        Security__C

        Mage__Base

    End Enum


    'Enum
    Class Skill_Item
        Public Buff_List As HashSet(Of Buff_List_Enum) = New HashSet(Of Buff_List_Enum)
        Public Ability_List As HashSet(Of Ability_List_Enum) = New HashSet(Of Ability_List_Enum)
        Public Position As PointI
        Public Parent As Skill_List_Enum
        Public Inherited As Boolean
        Public Req_Level As Integer
        Public Sprite As Integer
        Public Cost As Byte


        Sub New(ByVal Buff_List As HashSet(Of Buff_List_Enum), ByVal Ability_List As HashSet(Of Ability_List_Enum), ByVal Position As PointI, ByVal Sprite As Integer, ByVal Parent As Skill_List_Enum, ByVal Req_Level As Integer, ByVal Cost As Byte, Optional ByVal Inherited As Boolean = False)
            Me.Buff_List = Buff_List
            Me.Ability_List = Ability_List
            Me.Position = Position
            Me.Sprite = Sprite
            Me.Parent = Parent
            Me.Req_Level = Req_Level
            Me.Inherited = Inherited
            Me.Cost = Cost
        End Sub

    End Class




    Class Class_Tech_Tree
        Public Skills As Dictionary(Of Skill_List_Enum, Skill_Item) = New Dictionary(Of Skill_List_Enum, Skill_Item)


        Sub New(ByVal ClassID As Class_List_Enum)
            Load_Lists(ClassID)
        End Sub


        Sub Load_Lists(ByVal ClassId As Class_List_Enum)

            Dim B As New HashSet(Of Buff_List_Enum)
            Dim A As New HashSet(Of Ability_List_Enum)
            Dim P As PointI

            Select Case ClassId
                'Set class tech tree
                Case Is = Class_List_Enum.Engineer
                    B.Add(Buff_List_Enum.Buffs)
                    A.Add(Ability_List_Enum.Mage__Fireball)
                    P = New PointI(0, 0)
                    Skills.Add(Skill_List_Enum.Engineer__A, New Skill_Item(B, A, P, 0, Skill_List_Enum.None, 0, 1))
                    Reset_List(A, B)

                    B.Add(Buff_List_Enum.Buffs)
                    A.Add(Ability_List_Enum.Mage__Fireball)
                    P = New PointI(0, 1)
                    Skills.Add(Skill_List_Enum.Engineer__B, New Skill_Item(B, A, P, 1, Skill_List_Enum.Engineer__A, 5, 1))
                    Reset_List(A, B)

                    B.Add(Buff_List_Enum.Buffs)
                    A.Add(Ability_List_Enum.Mage__Fireball)
                    P = New PointI(0, 2)
                    Skills.Add(Skill_List_Enum.Engineer__C, New Skill_Item(B, A, P, 2, Skill_List_Enum.Engineer__B, 10, 1))
                    Reset_List(A, B)



                Case Is = Class_List_Enum.Mage
                    B.Add(Buff_List_Enum.Buffs)
                    A.Add(Ability_List_Enum.Mage__Fireball)
                    P = New PointI(0, 0)
                    Skills.Add(Skill_List_Enum.Engineer__A, New Skill_Item(B, A, P, 3, Skill_List_Enum.None, 0, 0, True))
                    Reset_List(A, B)

            End Select
        End Sub


        Private Sub Reset_List(ByVal A As HashSet(Of Ability_List_Enum), ByVal B As HashSet(Of Buff_List_Enum))
            A = New HashSet(Of Ability_List_Enum)
            B = New HashSet(Of Buff_List_Enum)
        End Sub



    End Class



End Module
