Public Module Enums
    <Serializable()> Public Structure PointD
        Public x As Double
        Public y As Double

        Sub New(ByVal X As Double, ByVal Y As Double)
            Me.x = X
            Me.y = Y
        End Sub

        Sub New(ByVal X As Integer, ByVal Y As Integer)
            Me.x = X
            Me.y = Y
        End Sub

        Function intX() As Integer            
            Return CInt(x) 'CInt(Fix(x))
        End Function

        Function intY() As Integer
            Return CInt(y) 'CInt(Fix(y))
        End Function

        Function sngX() As Single
            Return CSng(x - 0.005)
        End Function

        Function sngY() As Single
            Return CSng(y - 0.005)
        End Function


        Function PointI() As PointI

            'Return New PointI(CInt(x), CInt(y))
            Return New PointI(CInt(x), CInt(y))
        End Function

    End Structure
    <Serializable()> Public Structure PointI
        Public x As Integer
        Public y As Integer

        Sub New(ByVal X As Integer, ByVal Y As Integer)
            Me.x = X
            Me.y = Y
        End Sub

        Sub New(ByVal X As Double, ByVal Y As Double)
            Me.x = Convert.ToInt32(X)
            Me.y = Convert.ToInt32(Y)
        End Sub


        Function ToPoint() As Point
            Return New Point(x, y)
        End Function

        Function ToPointD() As PointD
            Return New PointD(x, y)
        End Function

        Function ToPointF() As PointF
            Return New PointF(x, y)
        End Function

        Public Shared Operator <>(ByVal point1 As PointI, ByVal point2 As PointI) As Boolean
            If point1.x <> point2.x AndAlso point1.y <> point2.y Then Return True            
            Return False
        End Operator

        Public Shared Operator =(ByVal point1 As PointI, ByVal point2 As PointI) As Boolean
            If point1.x = point2.x AndAlso point1.y = point2.y Then Return True
            Return False
        End Operator


    End Structure


    <Serializable()> Public Structure VectorD
        Public x As Double
        Public y As Double
        Public r As Double
        Sub New(ByVal X As Double, ByVal Y As Double, ByVal R As Double)
            Me.x = X
            Me.y = Y
            Me.r = R
        End Sub
    End Structure

    Function ToPointI(ByVal x As Integer, ByVal y As Integer) As PointI
        Return New PointI(x, y)
    End Function
    Enum tile_accesspoint_enum As Byte
        available
        used
        unavailable
    End Enum
    Enum armor_sprite_enum As Byte
        Tile
        CornerTL
        CornerTR
        CornerBL
        CornerBR
        WallT
        WallB
        WallL
        WallR
        AngleTL
        AngleTR
        AngleBL
        AngleBR
    End Enum
    Enum buffs_enum As Byte
        tired
        very_tired




    End Enum
    Enum character_sprite_enum As Byte
        Head
        Turso
        Left_Arm
        Right_Arm
        Left_Leg
        Right_Leg
    End Enum
    Enum character_sprite_set_enum As Byte
        Human_Renagade_1
        Human_Renagade_2
        Human_Renagade_3
        Human_Civilian_1
        Human_Pilot_1
    End Enum
    Enum current_view_enum As Byte
        personal
        ship_internal
        ship_external
        planet
        ship_build
        new_ship_build
        star_map

        personal_level_screen



        Alert_setup
        Squad_setup
        Weapon_control
        Special_abilities
        Fighter_Control        

    End Enum
    Enum device_type_enum As Byte
        thruster
        inter_planet_engine
        life_support
        shield_gen
        transporter
        tractor_beam
        replicator
        generator
        weapon
        nav_comp
        door
        elevator
        light
        launch_bay
        comm
        ai_comp
        control_panel
        sensor
        engine
        main_computer
    End Enum
    Enum pf_status As Byte
        no_map
        no_goal
        pending
        searching
        path_found
        no_path
    End Enum
    Enum room_sprite_enum As Byte
        CornerTL
        CornerTR
        CornerBL
        CornerBR
        WallT
        WallB
        WallL
        WallR
        AngleTL
        AngleTR
        AngleBL
        AngleBR
        Floor
        blank = 254
        empty = 255
    End Enum
    Enum hull_sprite_enum As Byte
        Build_cross

    End Enum
    Enum crew_script_enum As Byte
        center_on_tile
        move_to_tile
        open_door
        use
        hold
        'Ship
        set_room
        remove_room
        'Planet



    End Enum
    Enum ship_class_enum As Byte
        fighterW
        fighterT
        corvette
        frigate
        destroyer
        cruiser
        battle_cruiser
        battle_ship
        carrier
        space_station
    End Enum
    Enum ship_script_enum As Byte
        work_in_room
    End Enum
    Enum ship_room_type_enum As Byte
        bridge
        cargo_bay
        weapons_room
        corridor
        airlock
        medical
        engineering
        crew_quarters
    End Enum
    Enum ship_type_enum As Byte
        biological
        energy
        carbon_fiber
    End Enum
    Enum script_status_enum As Byte
        uninvoked
        running
        complete
    End Enum
    
    Enum tile_type_enum As Byte
        Hull_1
        Armor_1
        Armor_2
        Pipeline_1
        Airlock_1
        Armory_1
        Bridge_1
        Brigg_1
        Cargo_Bay_1
        Corridor_1
        Crew_Quarters_1
        Engineering_1
        Fighter_Bay_1
        Hydroponics_1
        MessDeck_1
        Science_Room_1
        Shuttle_Bay_1
        Sick_Bay_1
        Transporter_Room_1
        Device_Base = 253
        Restricted = 254
        empty = 255
    End Enum


    Enum planet_tile_type_enum As Byte
        Forest_Planet = 0
        Desert_Planet = 1
        Desert_Mine = 2
        Road
        Shipyard
        House
        House_Inside

        empty = 255
    End Enum


    Enum Projectile_Tile_Type_Enum
        Energy1
        Fire1
    End Enum


    Enum device_tile_type_enum As Byte
        Empty
        Bridge_Control_Panel
        Computer_MK1

        Generator
        Combustion_engine_MK1        
        Projectile_MK1
        Door_MK1
        Door_MK2
        Airlock_MK1
        Pipeline_energy_Small
        Pipeline_energy_Medium
        Pipeline_energy_Large
        Pipeline_data_Small
        Pipeline_data_Medium
        Pipeline_data_Large

        Thruster_MK1
    End Enum
    Enum walkable_type_enum As Byte
        Impassable
        Occupied
        HasDevice
        Walkable
        Door
        OpenDoor
    End Enum
    Enum button_texture_enum As Integer
        Blank = 0
        main_menu__button

        ship_build__contex_menu_item
        ship_build__menu_item
        ship_build__ship_info
        ship_build__resource_cost
        ship_build__undo
        ship_build__redo
        ship_build__mouse_position
        ship_build__minimap        
        ship_build__stats_panel
        ship_build__stats_button
        ship_build__menu_button
        ship_build__room_button
        ship_build__device_panel

        ship_build__options_button
        ship_build__description_panel

        ship_build__flip_up

        ship_build__flip_right
        ship_build__rotate_left
        ship_build__rotate_right
        ship_build__remove
        ship_build__remove_selection
        ship_build__info_bars
        ship_build__tile_grid
        ship_build__split

        ship_build__Pipeline_panel
        ship_build__Pipeline_delete
        ship_build__Pipeline_edit
        ship_build__Pipeline_Dialog_Panel


        ship_external__Pipeline_Display
        ship_external__Engine_Display


        PLV__Officer_Background
        PLV__Officer_ScrollUp
        PLV__Officer_ScrollDown
        PLV__Officer_ScrollLeft
        PLV__Officer_ScrollRight
        PLV__Experience
        PLV__Class_List
        PLV__Class_List_Frame

        PLV__Class_List_HalfFrame1
        PLV__Class_List_HalfFrame2
        PLV__Skill_Bar
        PLV__Skill_Point
        PLV__Level_Up
        PLV__Base_Class
        PLV__Base_Class_Frame






        Personal__HealthOverlay




    End Enum
    
    Enum panel_texture_enum
        sbm1
        minimap
        white_square
        palette_button
        palette_tab
    End Enum
    Enum ship_internal_component_enum
        tile
        piping
        device
    End Enum

    Public Enum Pipeline_type_enum
        Data
        Energy
    End Enum


    Public Enum Pipeline_Tile_type_enum
        Small_Energy
        Medium_Energy
        Large_Energy
        Small_Data
        Medium_Data
        Large_Data
    End Enum



    Enum planet_type_enum As Byte
        Inferno
        Desert
        Tropical
        Forest
        Swamp
        Ocean
        Ice
        Vacuum
    End Enum
    Public Enum tile_view_level As Byte
        Unexplored
        Explored
        ViewedDark
        ViewedDim
        Viewed
    End Enum
    Public Enum nebula_type_enum
        Gas
        Rock
        Dust
    End Enum

    Enum Planet_special_tech_enum
        Mathmatics
        Chemistry
        Biology
        Genetics
        MicroBiology
        Astrophysics
        Meteorology
        Optics
        Horology
        Nucleonics
        Acoustics
    End Enum


    Enum desert_planet_sprite_enum As Byte
        Rough
        Pond
        Crystal
        Barren
        PathV
        PathH
        PathTDown
        PathTUp
        PathSquare
        PathTLeft
        PathTRight
        PathELeft
        PathERight
        PathEUp
        PathEDown
        WallTop
        WallLeft
        WallRight
        WallCornerTL
        WallCornerTR
        WallCornerBL
        WallCornerBR
        WallEndB
        WallEndT
        WallEndR
        WallEndL
        MetalPad
        CrystalDrill1
        CrystalDrill2
        CrystalDrill3


    End Enum

    Enum building_type_enum
        'Industrial
        Manufacturer
        Refinery
        Mine
        Laboratory
        'Commercial
        Shop
        Pub
        Faction_Office
        Bank
        'Residential
        House
        Apartment
    End Enum

    Enum d3d_font_enum
        Big_Button
        SB_small


    End Enum

    Enum rotate_enum
        Zero = 0
        Ninty = 90
        OneEighty = 180
        TwoSeventy = 270


    End Enum

    Enum flip_enum
        None
        Flip_X
        Flip_Y
        Both
    End Enum


    Enum Worker_Type_Enum
        Worker
        Transporter
        Gaurd        
    End Enum


    Enum Pipeline_sprite_enum As Byte
        vertical
        horizontal
        TL
        TR
        BL
        BR
        SplitL
        SplitR
        SplitT
        SplitB
        cross
        EndT
        EndB
        EndL
        EndR
    End Enum


    Enum Ship_weapon_enum
        Laser_MK1
        Projectile_MK1


    End Enum

    Enum Weapon_fire_mode_enum
        Beam_Sustaned
        Projectile_Single
    End Enum



    'Menu Items
    Enum External_menu_items_Enum As Byte
        Menu
        Menu_alert_setup
        Menu_squad_setup
        Menu_weapon_control
        Menu_special_abilities
        Menu_Fighter_Control
        Menu_star_map



        Change_Alert
        Alert1
        Alert2
        Alert3
        Alert4
        Alert5

        Special1
        Special2
        Special3
        Special4
        Special5
        Special6
        Special7
        Special8
        Special9
        Special10

    End Enum

    Enum External_Weapon_Control_Enums
        New_Control_Group
        Control_Group_Panel
        Save
        Cancel
    End Enum


    Enum Personal_level_enums
        Officer_Background
        Officer_ScrollUp
        Officer_ScrollDown
        Officer_1
        Officer_2
        Officer_3
        Officer_4
        Officer_5
        Class_Background
        Class_Engineer
        Class_Security
        Class_Scientist
        Class_Aviator
        Class_1
        Class_2
        Class_3
        Class_4
        Class_5
        Class_6
        Class_Level
        Base_Class
        Class_Scroll_Left
        Class_Scroll_Right
    End Enum


    Enum Move_Direction As Byte
        None = 0
        Left = 1
        Right = 2
        Up = 3
        Down = 4
    End Enum
    


    Enum Internal_menu_items_Enum As Byte
        Neww


    End Enum

    Enum Direction_Enum As Byte
        Top
        Bottom
        Left
        Right
        RotateL
        RotateR
    End Enum



    Enum Officer_location_enum
        Planet
        Ship
        Enemy_Ship
    End Enum

    Enum Priority_enum As Byte
        Low = 0
        Medium = 1
        High = 2
        VeryHigh = 3
    End Enum


    Enum Ship_Build_Info_enum
        Info
        Devices
        Rooms
    End Enum





    Enum Effects_Texture_Enum

        Spot_256

    End Enum






    Enum Chest_Equipment_Enum
        Cloth


    End Enum



End Module