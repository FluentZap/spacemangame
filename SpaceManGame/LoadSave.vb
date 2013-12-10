Module LoadSave

    Function load_ship_schematic() As Ship
        Dim binary_formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim stream As New IO.FileStream(Application.StartupPath + "/myfile.bin", IO.FileMode.Open, IO.FileAccess.Read, IO.FileShare.Read)
        Dim schematic As Ship_Save = DirectCast(binary_formatter.Deserialize(stream), Ship_Save)
        stream.Close()
        Return New Ship(schematic)
    End Function


    Sub save_ship_schematic(ByVal save_ship As Ship)

        Dim binary_formatter As New Runtime.Serialization.Formatters.Binary.BinaryFormatter
        Dim stream As New IO.FileStream(Application.StartupPath + "/myfile.bin", IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None)
        binary_formatter.Serialize(stream, New Ship_Save(save_ship))
        stream.Close()
    End Sub



    Sub save_tech_tree()
        'Dim binary_formatter As New Xml.Serialization.XmlSerializer(GetType(tech_item))
        'Dim stream As New IO.FileStream(Application.StartupPath + "/tech.xml", IO.FileMode.Create, IO.FileAccess.Write, IO.FileShare.None)
        'Dim tech As New tech_item()
        'tech.FieldsOS.Add(FieldOS_list_enum.Optics)
        'binary_formatter.Serialize(stream, tech)
        'Dim schematic As tech = DirectCast(binary_formatter.Deserialize(stream), tech_item)
        'stream.Close()
    End Sub

    Sub init_resources()
        Dim Texture_Format As Direct3D.Format = Format.A8R8G8B8

        Dim Filter As Direct3D.Filter = Direct3D.Filter.Point
        Dim MipFilter As Direct3D.Filter = Direct3D.Filter.Point
        'Menu buttons
        button_texture(button_texture_enum.main_menu__button) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Main_Menu_button.png", 250, 40, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        
        'Ship build menu buttons
        button_texture(button_texture_enum.ship_build__menu_button) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Menu_button.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__menu_item) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Menu_item.png", 160, 40, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        button_texture(button_texture_enum.ship_build__room_button) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\room_button.png", 80, 32, 0, Direct3D.Usage.None, Direct3D.Format.A8R8G8B8, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__ship_info) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\class_panel.png", 256, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__device_panel) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Device_panel.png", 220, 480, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__options_button) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Options_button.png", 250, 40, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        button_texture(button_texture_enum.ship_build__minimap) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Minimap_panel.png", 224, 256, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)


        button_texture(button_texture_enum.ship_build__flip_up) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Flip_left.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__flip_right) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Flip_right.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__rotate_left) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Flip_left.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__rotate_right) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Flip_right.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__remove) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\remove_selection.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__remove_selection) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\remove_selection.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__info_bars) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Info_bars.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__tile_grid) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Tile_grid.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__split) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\split_room.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        button_texture(button_texture_enum.ship_build__Pipeline_panel) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Pipeline_panel.png", 128, 20, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__Pipeline_edit) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Pipeline_edit.png", 20, 20, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__Pipeline_delete) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Pipeline_delete.png", 20, 20, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        button_texture(button_texture_enum.ship_build__Pipeline_Dialog_Panel) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Pipeline_dialog_panel.png", 600, 300, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_build__description_panel) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Description_panel.png", 300, 500, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        'Ship External
        button_texture(button_texture_enum.ship_external__Pipeline_Display) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Pipeline_Display.png", 160, 16, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.ship_external__Engine_Display) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Engine_Display.png", 8, 64, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)



        'Player Level Up
        button_texture(button_texture_enum.PLV__Officer_Background) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Character.png", 24, 36, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)        

        button_texture(button_texture_enum.PLV__Officer_ScrollUp) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_ScrollUP.png", 72, 16, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.PLV__Officer_ScrollDown) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_ScrollDown.png", 72, 16, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        button_texture(button_texture_enum.PLV__Officer_ScrollLeft) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_ScrollLeft.png", 16, 108, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        button_texture(button_texture_enum.PLV__Officer_ScrollRight) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_ScrollRight.png", 16, 108, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)


        button_texture(button_texture_enum.PLV__Experience) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Experance.png", 72, 8, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        button_texture(button_texture_enum.PLV__Class_List) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\characters\Class_List.png", 256, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        button_texture(button_texture_enum.PLV__Base_Class) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Base_Class.png", 96, 16, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        button_texture(button_texture_enum.PLV__Base_Class_Frame) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Base_Class_Frame.png", 100, 20, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        button_texture(button_texture_enum.PLV__Class_List_Frame) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Class_Frame.png", 92, 167, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        button_texture(button_texture_enum.PLV__Class_List_HalfFrame1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Class_HalfFrame1.png", 92, 167, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        button_texture(button_texture_enum.PLV__Class_List_HalfFrame2) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Class_HalfFrame2.png", 92, 167, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        button_texture(button_texture_enum.PLV__Skill_Point) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Skill.png", 320, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        button_texture(button_texture_enum.PLV__Skill_Bar) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Skill_Bar.png", 8, 64, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        button_texture(button_texture_enum.PLV__Level_Up) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\PLV_Level.png", 24, 16, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)



        'Personal view
        button_texture(button_texture_enum.Personal__HealthOverlay) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\menu\Personal_HealthOverlay.png", 896, 96, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)


        'Room sprites
        tile_texture(tile_type_enum.Airlock_1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Airlock1.png", 512, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        tile_texture(tile_type_enum.Bridge_1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Bridge1.png", 512, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        tile_texture(tile_type_enum.Corridor_1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Corridor1.png", 512, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        tile_texture(tile_type_enum.Engineering_1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Engineering1.png", 512, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        tile_texture(tile_type_enum.Crew_Quarters_1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\CrewQuarters1.png", 512, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        tile_texture(tile_type_enum.Armor_1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Armor2.png", 416, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        'tile_texture(tile_type_enum.Armor_1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Armor2-2.png", 416, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        tile_texture(tile_type_enum.Armor_2) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Armor3.png", 416, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        tile_texture(tile_type_enum.Hull_1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Hull1.png", 256, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)


        test_texture = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\1.png", 0, 0, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        'tile_texture(9) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\image1.png", 0, 0, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)


        'Planet_Textures
        planet_tile_texture(planet_tile_type_enum.Forest_Planet) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\planet1.png", 640, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        planet_tile_texture(planet_tile_type_enum.Forest_Planet) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\planet1.png", 640, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        planet_tile_texture(planet_tile_type_enum.House) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Building_House_Small.png", 576, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        planet_tile_texture(planet_tile_type_enum.House_Inside) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Building_House_Small_inside.png", 288, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)


        'planet_tile_texture(planet_tile_type_enum.Forest_Planet) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\planet1.png", 96, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        planet_tile_texture(planet_tile_type_enum.Shipyard) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Shipyard1.png", 512, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        'planet_tile_texture(planet_tile_type_enum.Forest_Planet) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\planet1.png", 96, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        'planet_tile_texture(planet_tile_type_enum.Forest_Planet) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\planet1.png", 96, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)


        'Projectile Sprites
        projectile_tile_texture(Projectile_Tile_Type_Enum.Energy1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Projectiles\Energy1.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)


        effect_texture(0) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Cloud1.png", 512, 512, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        effect_texture(1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Cloud2.png", 512, 512, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        effect_texture(2) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Tiles\Cloud3.png", 512, 512, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        'Device sprites
        device_tile_texture(device_tile_type_enum.Generator) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Generator.png", 128, 64, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        device_tile_texture(device_tile_type_enum.Combustion_engine_MK1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Combustion_MK1-1.png", 384, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        device_tile_texture(device_tile_type_enum.Projectile_MK1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Projectile_MK1.png", 160, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        device_tile_texture(device_tile_type_enum.Door_MK1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Door_MK1.png", 128, 416, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        device_tile_texture(device_tile_type_enum.Door_MK2) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Door_MK2.png", 64, 320, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        device_tile_texture(device_tile_type_enum.Airlock_MK1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Airlock_MK1.png", 64, 640, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)


        

        device_tile_texture(device_tile_type_enum.Bridge_Control_Panel) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Control_Panel.png", 160, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        device_tile_texture(device_tile_type_enum.Computer_MK1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Computer_MK1.png", 64, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)


        device_tile_texture(device_tile_type_enum.Thruster_MK1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Thruster_MK1.png", 64, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)



        device_tile_texture(device_tile_type_enum.Pipeline_energy_Small) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Pipeline_energy.png", 480, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        device_tile_texture(device_tile_type_enum.Pipeline_energy_Medium) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Pipeline_energy.png", 480, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        device_tile_texture(device_tile_type_enum.Pipeline_energy_Large) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Pipeline_energy.png", 480, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        device_tile_texture(device_tile_type_enum.Pipeline_data_Small) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Pipeline_data.png", 480, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        device_tile_texture(device_tile_type_enum.Pipeline_data_Medium) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Pipeline_data.png", 480, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        device_tile_texture(device_tile_type_enum.Pipeline_data_Large) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Devices\Pipeline_data.png", 480, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        'Characters
        character_texture(character_sprite_set_enum.Human_Renagade_1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Characters\Renagade1.png", 640, 192, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        character_texture(character_sprite_set_enum.Human_Renagade_2) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Characters\SpellSword.png", 512, 192, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        character_texture(character_sprite_set_enum.Human_Renagade_3) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\Textures\Characters\Renagade3.png", 256, 192, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)


        panel_texture(panel_texture_enum.sbm1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\textures\panels\800 x 180.png", 800, 180, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        panel_texture(panel_texture_enum.minimap) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\textures\panels\minimap.png", 200, 210, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
        panel_texture(panel_texture_enum.white_square) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\textures\panels\select_tile.png", 32, 32, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        panel_texture(panel_texture_enum.palette_button) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\textures\panels\palette button.png", 80, 35, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)
        panel_texture(panel_texture_enum.palette_tab) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\Data\textures\panels\palette tab.png", 100, 35, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, 0)

        Cursor1 = d3d_device.CreateOffscreenPlainSurface(32, 32, Format.A8R8G8B8, Pool.Default)
        SurfaceLoader.FromFile(Cursor1, Application.StartupPath + "\data\textures\cursor\cur1.png", Filter.None, 0)


        icon_texture(0) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\data\textures\icons\star.png", 0, 0, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        icon_texture(1) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\data\textures\icons\moon.png", 0, 0, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)

        icon_texture(2) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\data\textures\icons\planet.png", 0, 0, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)



        star_map_vertex = New VertexBuffer(GetType(point_sprite_vertex_format), 200, d3d_device, Usage.WriteOnly, VertexFormats.Position, Pool.Managed)


        Dim di As New IO.DirectoryInfo(Application.StartupPath + "\data\textures\decals\")
        Dim diar1 As IO.FileInfo() = di.GetFiles("*.png")
        Dim count As Integer
        For Each dra In diar1
            Ship_Decals.Add(count, dra.Name)
        Next

        If Ship_Decals.Count > 0 Then
            For Each decal In Ship_Decals
                Ship_Decal_Texture(decal.Key) = Direct3D.TextureLoader.FromFile(d3d_device, Application.StartupPath + "\data\textures\decals\" + decal.Value + ".png", 0, 0, 0, Direct3D.Usage.None, Texture_Format, Direct3D.Pool.Managed, Filter, MipFilter, Color.FromArgb(255, 0, 255, 0).ToArgb)
            Next
        End If
    End Sub

    Sub load_config_file()
        'Dim file1 As New System.IO.FileStream(Application.StartupPath + "config.txt", IO.FileMode.Open)

        If IO.File.Exists(Application.StartupPath + "\config.txt") Then
            Dim file1 As IO.TextReader = New IO.StreamReader(Application.StartupPath + "\config.txt")

            screen_size.x = Convert.ToInt32(file1.ReadLine)
            screen_size.y = Convert.ToInt32(file1.ReadLine)
            monitor = Convert.ToInt32(file1.ReadLine)
            windowed = Convert.ToBoolean(file1.ReadLine)
            file1.Close()
        Else
            Dim file1 As IO.TextWriter = New IO.StreamWriter(Application.StartupPath + "\config.txt")
            file1.WriteLine(screen_size.x)
            file1.WriteLine(screen_size.y)
            file1.WriteLine(monitor)
            file1.WriteLine(windowed)
            file1.Close()
        End If

    End Sub

End Module
