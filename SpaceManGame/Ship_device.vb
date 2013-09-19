<Serializable()> Public Class Device_Pipeline
    Public Pipeline_Connection As Integer
    Public Amount As Integer
    Public Type As Pipeline_type_enum

    Sub New(ByVal Type As Pipeline_type_enum, ByVal Amount As Integer, Optional ByVal Pipeline_Connection As Integer = -1)
        Me.Type = Type
        Me.Amount = Amount
        Me.Pipeline_Connection = Pipeline_Connection
    End Sub
End Class




<Serializable()> Public Class Ship_device

    Public tile_list As HashSet(Of PointI) = New HashSet(Of PointI)

    Public type As device_type_enum
    Public tech_ID As tech_list_enum
    Public integrity As Integer
    Public integrityMax As Integer
    Public temperature As Byte
    Public required_Points As crew_resource_type
    Public crew_efficiency As Integer
    Public supply_efficiency As Integer

    'Thrusters    
    Public Thrust_Power As Double
    Public Thrust_Max As Double
    Public Throttle As Double
    Public Throttled_Engine As Boolean

    Public Active_Point As PointI
    Public Thrust_Direction As Direction_Enum

    'Public Weapon_Point As PointI
    Public Center_Angle As Double
    Public Center_Distance As Double    
    Public Device_Face As rotate_enum

    Public Sprite_Animation_Key As Integer

    Public pipeline As HashSet(Of Device_Pipeline) = New HashSet(Of Device_Pipeline)


    Sub New(ByVal type As device_type_enum, ByVal integrity As Integer, ByVal integrityMax As Integer, ByVal temperature As Byte, ByVal required_Points As crew_resource_type, ByVal pipeline As HashSet(Of Device_Pipeline), ByVal Tech_id As tech_list_enum, ByVal tile_list As HashSet(Of PointI))
        Me.type = type
        Me.integrity = integrity
        Me.integrityMax = integrityMax
        Me.temperature = temperature
        Me.required_Points = required_Points
        Me.tech_ID = Tech_id
        Me.tile_list = tile_list

        For Each pipe In pipeline
            Me.pipeline.Add(New Device_Pipeline(pipe.Type, pipe.Amount))
        Next
    End Sub





    Function Connected_To_Pipeline(ByVal Pipeline As Integer) As Boolean
        For Each Pipe In Me.pipeline
            If Pipe.Pipeline_Connection = Pipeline Then Return True
        Next
        Return False
    End Function

    Sub Connect_Pipeline(ByVal Pipeline As Integer, ByVal type As Pipeline_type_enum)
        For Each pipe In Me.pipeline
            If pipe.Pipeline_Connection = -1 AndAlso pipe.Type = type Then pipe.Pipeline_Connection = Pipeline : Exit For
        Next
    End Sub


    Sub Connect_Pipeline(ByVal Pipeline As Integer, ByVal type As Pipeline_type_enum, ByVal IsOutput As Boolean)
        If IsOutput = True Then
            For Each pipe In Me.pipeline
                If pipe.Pipeline_Connection = -1 AndAlso pipe.Type = type AndAlso pipe.Amount > 0 Then pipe.Pipeline_Connection = Pipeline : Exit For
            Next
        Else
            For Each pipe In Me.pipeline
                If pipe.Pipeline_Connection = -1 AndAlso pipe.Type = type AndAlso pipe.Amount < 0 Then pipe.Pipeline_Connection = Pipeline : Exit For
            Next
        End If

    End Sub



    Sub Disconnect_Pipeline(ByVal Pipeline As Integer)
        For Each pipe In Me.pipeline
            If pipe.Pipeline_Connection = Pipeline Then pipe.Pipeline_Connection = -1 : Exit For
        Next
    End Sub


    Function Contains_Pipeline(ByVal Pipeline_Type As Pipeline_type_enum) As Boolean
        For Each pipe In Me.pipeline
            If pipe.Type = Pipeline_Type Then Return True
        Next
        Return False
    End Function


    Function Contains_Open_Pipeline(ByVal Pipeline_Type As Pipeline_type_enum) As Boolean
        For Each pipe In Me.pipeline
            If pipe.Type = Pipeline_Type AndAlso pipe.Pipeline_Connection = -1 Then Return True
        Next
        Return False
    End Function



End Class
