Public Enum Speech_Enum
    Shopkeeper_Greeting

End Enum
Module AiSpeech



    Function GetAiSpeech(ByVal Crew As Crew) As String
        Dim D = 20
        If Crew.command_queue.Count > 0 Then
            Select Case Crew.command_queue.First.type
                Case Is = crew_script_enum.Customer_EatDrink
                    Select Case random(0, 5)
                        Case Is = 0 : Crew.Speech.Add("Bring me drink!", D)
                        Case Is = 1 : Crew.Speech.Add("Ooo my favorite", D)
                        Case Is = 2 : Crew.Speech.Add("More drink!", D)
                        Case Is = 3 : Crew.Speech.Add("Try the fish", D)
                        Case Is = 4 : Crew.Speech.Add("I'm Hungry...", D)
                        Case Is = 5 : Crew.Speech.Add("Eat at Joes", D)
                    End Select

            End Select
        End If


        Return ""
    End Function


    Sub GetAiSpeech(ByVal C As Crew, ByVal Type As Speech_Enum)
        Dim D = 20
        Dim S As Speech_Type = C.Speech
        Select Case Type
            Case Is = Speech_Enum.Shopkeeper_Greeting
                Select Case random(0, 2)
                    Case Is = 0 : S.Add("Welcome", D)
                    Case Is = 1 : S.Add("Can i help you?", D)
                    Case Is = 2
                        Select Case GST
                            Case 1000 To 1500 'morning
                                S.Add("Good Morning", D)
                            Case 2500 To 3000 'evening
                                S.Add("Good Evening", D)
                            Case 1500 To 2500 'Day
                                S.Add("Good Day", D)
                            Case 0 To 1000 'Night
                                S.Add("Your out late", D)
                        End Select
                End Select
        End Select
    End Sub



End Module
