Module Universe
    Private Const Pi As Double = 3.14159265358979
    Private Const e As Double = 2.718281828

    Class region_class
        Public stars As HashSet(Of Integer) = New HashSet(Of Integer)
        Public planets As HashSet(Of Integer) = New HashSet(Of Integer)
    End Class

    Class Universe
        Public stars As New Dictionary(Of Integer, Star)()
        'Public planets As New Dictionary(Of Integer, Planet)()
        Public regions As New Dictionary(Of PointI, region_class)()
        Public nebula As New Dictionary(Of Integer, Nebula)()
        Public Projectiles As New HashSet(Of Projectile)()

        Public Ship_List As New Dictionary(Of Integer, Ship)(500)
        Public Planet_List As New Dictionary(Of Integer, Planet)(150)
        Public Crew_List As New Dictionary(Of Integer, Crew)()
        Public Officer_List As New Dictionary(Of Integer, Officer)()
    End Class

    Sub generate()
        generate_stars()
        generate_planets()
        generate_nebula()
        'generate_intersteller_clouds()
        'generate_astroid_fields()
        'set_regions()


    End Sub

    Sub set_regions()
        Dim regions As New Dictionary(Of PointI, region_class)()
        For y = -500 To 500
            For x = -500 To 500
                regions.Add(New PointI(x, y), New region_class)
            Next
        Next

        For Each star In u.stars
            Dim point As New PointI
            point.x = Convert.ToInt32(star.Value.location.x / 393216)
            point.y = Convert.ToInt32(star.Value.location.y / 393216)
            'regions(point.x + point.y * 100).stars.Add(star.Key)
        Next

        For Each Planet In u.Planet_List
            Dim point As New PointI
            If Planet.Value.orbits_planet = True Then
                point.x = Convert.ToInt32(u.stars(u.Planet_List(Planet.Value.orbit_point).orbit_point).location.x / 393216)
                point.y = Convert.ToInt32(u.stars(u.Planet_List(Planet.Value.orbit_point).orbit_point).location.y / 393216)
            Else
                point.x = Convert.ToInt32(u.stars(Planet.Value.orbit_point).location.x / 393216)
                point.y = Convert.ToInt32(u.stars(Planet.Value.orbit_point).location.y / 393216)
            End If

            'regions(point.x + point.y * 100).planets.Add(Planet.Key)
        Next
        u.regions = regions
    End Sub

    Sub generate_stars()
        Dim points As List(Of PointI) = New List(Of PointI)(150)

        Dim rotate = 0
        'random(50, 25)        
        For r = 50 To 600 Step 50
            For theta = 0 To 359 Step 360 / 2
                Dim x = r * Math.Cos((theta + rotate) * 0.017453292519943295) + random(0, 100)
                Dim y = r * Math.Sin((theta + rotate) * 0.017453292519943295) + random(0, 100)
                points.Add(New PointI(x * 50000, y * 50000))
            Next
            rotate += 32
        Next

        rotate = 0
        For r = 50 To 600 Step 50
            For theta = 0 + 90 To 359 + 90 Step 360 / 2
                Dim x = r * Math.Cos((theta + rotate) * 0.017453292519943295) + random(0, 100)
                Dim y = r * Math.Sin((theta + rotate) * 0.017453292519943295) + random(0, 100)
                points.Add(New PointI(x * 50000, y * 50000))
            Next
            rotate += 32
        Next



        'For a = 1 To 30
        'Dim x, y As Integer
        'Dim redo As Boolean
        'x = random(-600, 1200) * 32768
        'y = random(-600, 1200) * 32768
        '
        'For Each point In points
        'If Math.Abs(x - Point.X) < 50000 OrElse Math.Abs(y - Point.Y) < 50000 Then redo = True
        'Next

        'If redo = True Then
        'a = a - 1
        'redo = False
        'Else
        'points.Add(New PointI(x, y))
        'End If


        'Next

        For p As Integer = 0 To points.Count - 1
            Dim st As New Star()
            st.age = random(1, 1000) + random(1, 1000)
            st.location = points(p)
            st.size = random(1, 10) + random(1, 10)
            st.temperature = random(1, 1000) + random(1, 1000)
            u.stars.Add(p + 1, st)
        Next p

        Dim bh As New Star()
        bh.age = 0
        bh.location = New PointI(0, 0)
        bh.size = 1000
        bh.temperature = 0
        u.stars.Add(0, bh)
    End Sub

    Sub generate_planets()
        'Exit Sub
        Dim systems_to_create As List(Of Integer) = New List(Of Integer)
        Dim r As Integer
        For a = 1 To 40
            Do
                r = random(1, u.stars.Count - 2)
            Loop While systems_to_create.Contains(r)
            systems_to_create.Add(r)
        Next
        systems_to_create.Add(0)

        Dim planetID As Integer = 0
        For Each star In systems_to_create
            Dim last_orbit_distance As Integer = 0
            For planet_number = 1 To random(1, 3)
                Dim orbit_distance As Integer = 163840 + last_orbit_distance + random(0, 163840)
                last_orbit_distance = orbit_distance

                create_planet(planetID, New PointI(512, 512), star, orbit_distance, False, randomD(0.1, 0.9))

                'moon genoration
                If random(0, 0) = 0 Then
                    Dim a As Planet = u.Planet_List(0)
                    Dim moons As Integer = random(1, 1) + random(0, 1)
                    Dim moon_last_orbit_distance As Integer = 0
                    For moon_number = 1 To moons
                        Dim moon_orbit_distance As Integer = 25000 + moon_last_orbit_distance '+ random(0, 10000)
                        moon_last_orbit_distance = moon_orbit_distance
                        create_planet(planetID + moon_number, New PointI(256, 256), planetID, moon_orbit_distance, True, 0) ' randomD(0.1, 0.9))
                    Next
                    planetID += moons
                End If

                planetID += 1
            Next

        Next


    End Sub

    Sub generate_nebula()

        Dim nebula As New Dictionary(Of Integer, Nebula)()

        Dim NebulaID As Integer = 0
        Dim r As Integer
        Dim x1, y1 As Integer

        For a = 0 To random(10, 10)
            Dim points As List(Of PointI) = New List(Of PointI)(150)
            'x1 = random(100000, 1900000)
            'y1 = random(100000, 1900000)

            x1 = random(-19660800, 39321600)
            y1 = random(-19660800, 39321600)
            
            Dim rotate = 0

            For theta = 0 To 359 Step 360 / 16
                r = random(1000000, 1000000)
                'rotate += random(16, 8)
                Dim x = r * Math.Cos((theta) * 0.017453292519943295)
                Dim y = r * Math.Sin((theta) * 0.017453292519943295)
                points.Add(New PointI(x1 + x, y1 + y))
            Next

            'Dim tneb As New Nebula(points, nebula_type_enum.Gas, 10)
            nebula.Add(a, New Nebula(points, nebula_type_enum.Gas, 10))
        Next
        u.nebula = nebula

    End Sub

    Sub create_planet(ByVal planetID As Integer, ByVal size As PointI, ByVal orbit As Integer, ByVal orbit_distance As Integer, ByVal orbits_planet As Boolean, ByVal theta_offset As Double)
        If theta_offset = 0 Then theta_offset = randomD(0.1, 0.1)
        Dim p As Planet
        Select Case random(1, 3) + random(0, 4)
            Case 1 'Vacuum
                p = New Planet(planetID, planet_type_enum.Vacuum, size, orbit, orbit_distance, orbits_planet, theta_offset, Planet_Level_Type.Barren)
            Case 2 'Inferno
                p = New Planet(planetID, planet_type_enum.Inferno, size, orbit, orbit_distance, orbits_planet, theta_offset, Planet_Level_Type.Barren)
            Case 3 'Desert
                p = New Planet(planetID, planet_type_enum.Desert, size, orbit, orbit_distance, orbits_planet, theta_offset, Planet_Level_Type.Barren)
            Case 4 'Tropical
                p = New Planet(planetID, planet_type_enum.Tropical, size, orbit, orbit_distance, orbits_planet, theta_offset, Planet_Level_Type.Barren)
            Case 5 'Forest
                p = New Planet(planetID, planet_type_enum.Forest, size, orbit, orbit_distance, orbits_planet, theta_offset, Planet_Level_Type.Barren)
            Case 6 'Swamp
                p = New Planet(planetID, planet_type_enum.Swamp, size, orbit, orbit_distance, orbits_planet, theta_offset, Planet_Level_Type.Barren)
            Case 7 'Ocean
                p = New Planet(planetID, planet_type_enum.Ocean, size, orbit, orbit_distance, orbits_planet, theta_offset, Planet_Level_Type.Barren)
            Case 8 'Ice
                p = New Planet(planetID, planet_type_enum.Ice, size, orbit, orbit_distance, orbits_planet, theta_offset, Planet_Level_Type.Barren)
            Case Else
                p = New Planet(planetID, planet_type_enum.Vacuum, size, orbit, orbit_distance, orbits_planet, theta_offset, Planet_Level_Type.Barren)
        End Select
        u.Planet_List.Add(planetID, p)
    End Sub

    Function random(ByVal min As Integer, ByVal range As Integer) As Integer
        Return Convert.ToInt32((Rnd() * range) + min)
    End Function
    Function randomD(ByVal min As Double, ByVal range As Double) As Double
        Return (Rnd() * range) + min
    End Function
End Module