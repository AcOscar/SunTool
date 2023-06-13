'sun exposure
Imports System.Drawing
Imports System.IO

''' <summary>
''' 
''' </summary>
Public Class SunExposure2
    Public Shared Function Analyse(ByVal doc As Rhino.RhinoDoc,
                                   ByRef SEAProgressBar As System.Windows.Forms.ProgressBar,
                                   Optional ByVal startdate As Date? = Nothing,
                                   Optional ByVal enddate As Date? = Nothing
                                   ) As Rhino.Commands.Result

        RhStopWatch.Start()

        'variables
        Dim RecObjRef As Rhino.DocObjects.ObjRef

        Dim threshold As Double
        Dim exp_Units As Integer
        '''TimeUnits
        Dim tu As Double = SEA_TimeUnits

        Dim vertex_rec As Rhino.Geometry.Point3d()
        Dim vecNorms As Rhino.Geometry.Vector3d()
        Dim exp_Units_array As Integer()
        Dim newGuid, oldGuid As Guid
        Dim GuidsTemp As New List(Of Guid)
        Dim colTemp As Drawing.Color

        Dim numberTotal As Integer = 0
        Dim mesh As New Rhino.Geometry.Mesh
        Dim pts As Rhino.Geometry.Collections.MeshVertexList

        Dim str_startMin, str_endMin As String

        Dim isTimespan As Boolean = False

        Dim TimespanSunAngles As New List(Of SunAngle2)
        Dim TimespanstartT As New List(Of Double)
        Dim TimespanendT As New List(Of Double)
        If startdate IsNot Nothing Then
            If enddate IsNot Nothing Then
                isTimespan = True
            End If
        End If
        Rhino.RhinoApp.WriteLine(" isTimespan : " & isTimespan.ToString)
        'number of days that we have to calculate, at least one day
        Dim days As Integer = 1

        If isTimespan Then
            Dim timeSpan As TimeSpan = enddate.Value.Subtract(startdate.Value)
            days = CInt(timeSpan.TotalDays)
        End If

        Rhino.RhinoApp.WriteLine(" days : " & days.ToString)

        If SEA_CastObj Is Nothing Then
            Rhino.RhinoApp.WriteLine("Please define shadow cast objets first.")
            Return Rhino.Commands.Result.Failure
        End If

        If SEA_RecObj Is Nothing Then
            Rhino.RhinoApp.WriteLine("Please define shadow receiving objets first.")
            Return Rhino.Commands.Result.Failure
        End If

        'for the progress bar - calculate how many vertices we have
        For i = 0 To SEA_RecObj.Count - 1
            'RecObjRef = New Rhino.DocObjects.ObjRef(SEA_RecObj(i))
            RecObjRef = New Rhino.DocObjects.ObjRef(doc, SEA_RecObj(i))
            mesh = RecObjRef.Mesh
            pts = mesh.Vertices
            numberTotal += (pts.Count)
        Next

        SEAProgressBar.Maximum = numberTotal * days
        Dim mySunAngle As SunAngle2
        'For day = 1 To days
        Dim exp_Max As Double = 0

        'If isTimespan Then
        '    mySunAngle = New SunAngle2(day, 0, 0, TZone, lat, lon, nOffset)
        'Else
        Dim startT, endT As Double
        'End If
        If isTimespan Then

            Dim myDays As List(Of Date) = GenerateDateList(startdate, enddate)
            For Each myDate In myDays

                mySunAngle = New SunAngle2(0, myDate.Month, myDate.Day, TZone, lat, lon, nOffset)
                TimespanSunAngles.Add(mySunAngle)

                'adjust the start and end time that it is always during daylight time
                If SEA_StartH >= mySunAngle.Sunrise And SEA_StartH < mySunAngle.Sunset Then
                    startT = SEA_StartH
                ElseIf SEA_StartH > mySunAngle.Sunset Then
                    startT = mySunAngle.Sunset
                Else
                    startT = mySunAngle.Sunrise
                End If

                If SEA_EndH < mySunAngle.Sunset And SEA_EndH > mySunAngle.Sunrise Then
                    endT = SEA_EndH
                ElseIf SEA_EndH < startT Then
                    endT = startT
                Else
                    endT = mySunAngle.Sunset
                End If


                TimespanstartT.Add(startT)
                TimespanendT.Add(endT)

                exp_Max += (endT - startT) * tu '(sunset - sunrise) * tUnits

                Rhino.RhinoApp.WriteLine(" day : " & myDate & " " & startT & "" & endT)
            Next
        Else

            mySunAngle = New SunAngle2(0, month, day, TZone, lat, lon, nOffset)

            'adjust the start and end time that it is always during daylight time
            If SEA_StartH >= mySunAngle.Sunrise And SEA_StartH < mySunAngle.Sunset Then
                startT = SEA_StartH

            ElseIf SEA_StartH > mySunAngle.Sunset Then
                startT = mySunAngle.Sunset
            Else

                startT = mySunAngle.Sunrise
            End If

            If SEA_EndH < mySunAngle.Sunset And SEA_EndH > mySunAngle.Sunrise Then
                endT = SEA_EndH
            ElseIf SEA_EndH < startT Then
                endT = startT
            Else
                endT = mySunAngle.Sunset
            End If

            'the maximum possible exposure time
            exp_Max = (endT - startT) * tu '(sunset - sunrise) * tUnits


        End If






        Dim startH, startMin, endH, endMin As Double

        startH = Math.Floor(startT)
        startMin = Math.Floor((startT - startH) * 60)
        endH = Math.Floor(endT)
        endMin = Math.Floor((endT - endH) * 60)

        str_startMin = CStr(startMin)
        str_endMin = CStr(endMin)
        Dim r, g, b As Integer
        Dim arr_col As List(Of System.Drawing.Color)
        Dim comp_Val As Integer

        If Len(str_startMin) = 1 Then
            str_startMin = "0" & str_startMin
        End If


        If Len(str_endMin) = 1 Then
            str_endMin = "0" & str_endMin
        End If

        If isTimespan Then
            Rhino.RhinoApp.WriteLine(" day : " & day & " month : " & month)
        End If

        Rhino.RhinoApp.WriteLine(" start time : " & CStr(startH) & ":" & str_startMin & " ;end time :" & CStr(endH) & ":" & str_endMin)

        Rhino.RhinoApp.WriteLine(" objects : " & SEA_RecObj.Count)

        For i = 0 To SEA_RecObj.Count - 1

            'Finding Vertex of mesh
            'RecObjRef = New Rhino.DocObjects.ObjRef(SEA_RecObj(i))
            RecObjRef = New Rhino.DocObjects.ObjRef(doc, SEA_RecObj(i))

            'debugger
            Try
                mesh = RecObjRef.Mesh
                pts = mesh.Vertices

                'arr of vertexes
                vertex_rec = Array.ConvertAll(Of Rhino.Geometry.Point3f, Rhino.Geometry.Point3d)(pts.ToArray, Function(p3) New Rhino.Geometry.Point3d(p3))

                'arr of normals in vertexes
                vecNorms = (Array.ConvertAll(Of Rhino.Geometry.Vector3f, Rhino.Geometry.Vector3d)(mesh.Normals.ToArray, Function(vec3) New Rhino.Geometry.Vector3d(vec3)))

                exp_Units_array = New Integer(vertex_rec.GetLength(0)) {}

                'getting the threshold exposure value
                threshold = SEA_Threshold * tu

                Dim cols As New Rhino.Collections.RhinoList(Of Drawing.Color)

                'coloring mesh vertexes
                For j = 0 To vertex_rec.Count - 1

                    If isTimespan Then
                        Dim tmp_exp_Units = 0
                        For d = 0 To days - 1
                            ' Rhino.RhinoApp.WriteLine(" day : " & d)
                            tmp_exp_Units = GenerateArrayfRays(doc.ModelAbsoluteTolerance, tu, vertex_rec(j), vecNorms(j), TimespanstartT(d), TimespanendT(d), SEA_RecObj(i), TimespanSunAngles(d))
                            'Rhino.RhinoApp.WriteLine(" exp_Units : " & tmp_exp_Units)
                            exp_Units += tmp_exp_Units
                        Next

                    Else
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
                        exp_Units = GenerateArrayfRays(doc.ModelAbsoluteTolerance, tu, vertex_rec(j), vecNorms(j), startT, endT, SEA_RecObj(i), mySunAngle)
#Enable Warning BC42104 ' Variable is used before it has been assigned a value

                    End If

                    'Dim doc As RhinoDoc = Rhino.RhinoDoc.ActiveDoc
                    'Dim text As String = "1234"
                    'Dim location As Rhino.Geometry.Point3d = New Rhino.Geometry.Point3d(0, 0, 0) ' Anpassen nach Bedarf

                    'doc.Objects.AddTextDot(exp_Units, vertex_rec(j))
                    'doc.Views.Redraw()


                    exp_Units_array(j) = exp_Units
                    Dim check_Val As Double
                    check_Val = exp_Units
                    exp_Units = 0
                    Select Case SEA_GOption
                        Case SEA_GOptionValues.Threshold

                            If check_Val >= threshold Then
                                colTemp = SEA_colorPos
                            Else
                                colTemp = SEA_colorNeg
                            End If

                        Case SEA_GOptionValues.Gradient

                            r = check_Val * (SEA_colorPos.R - SEA_colorNeg.R) / exp_Max + SEA_colorNeg.R
                            g = check_Val * (SEA_colorPos.G - SEA_colorNeg.G) / exp_Max + SEA_colorNeg.G
                            b = check_Val * (SEA_colorPos.B - SEA_colorNeg.B) / exp_Max + SEA_colorNeg.B

                            colTemp = Drawing.Color.FromArgb(255, r, g, b)

                        Case SEA_GOptionValues.Precised
                            arr_col = GetColorSteps()
                            If isTimespan Then
                                'comp_Val = exp_Max / check_Val / tu

                                ''Rhino.RhinoApp.WriteLine(" exp_Units : " & exp_Max)
                                'If comp_Val > (arr_col.Count - 1) Then comp_Val = arr_col.Count - 1

                                'colTemp = arr_col(comp_Val)

                                Select Case check_Val
                                    Case > 2700
                                        colTemp = arr_col(8)
                                    Case > 2400
                                        colTemp = arr_col(7)
                                    Case > 2100
                                        colTemp = arr_col(6)
                                    Case > 1800
                                        colTemp = arr_col(5)
                                    Case > 1500
                                        colTemp = arr_col(4)
                                    Case > 1200
                                        colTemp = arr_col(3)
                                    Case > 900
                                        colTemp = arr_col(2)
                                    Case > 600
                                        colTemp = arr_col(1)
                                    Case > 300
                                        colTemp = arr_col(0)
                                End Select

                            Else
                                comp_Val = CInt(Math.Floor(check_Val / tu))
                                If comp_Val > (arr_col.Count - 1) Then comp_Val = arr_col.Count - 1
                                colTemp = arr_col(comp_Val)
                            End If

                            If CreateDots Then
                                doc.Objects.AddTextDot(check_Val, vertex_rec(j))
                            End If

                    End Select

                    cols.Add(colTemp)

                    'update progress bar
                    SEAProgressBar.Value += 1

                Next

                For j = 0 To vertex_rec.Count - 1

                    mesh.VertexColors.SetColor(j, cols(j))

                Next
                Dim attribs As Rhino.DocObjects.ObjectAttributes = RecObjRef.[Object]().Attributes

                oldGuid = RecObjRef.ObjectId
                doc.Objects.Delete(RecObjRef, True)
                newGuid = doc.Objects.AddMesh(mesh, attribs)

                For j = 0 To SEA_CastObj.Count - 1
                    If SEA_CastObj(j) = oldGuid Then SEA_CastObj(j) = newGuid
                Next

                If Not newGuid = Nothing Then GuidsTemp.Add(newGuid)


            Catch ex As Exception
                Rhino.RhinoApp.WriteLine("Redefine receiving geometry")
#If DEBUG Then
                Rhino.RhinoApp.WriteLine(ex.ToString)
#End If

            End Try

            If WriteFile Then
                'WriteArrayToCSV(exp_Units_array, 10, RecObjRef.ObjectId.ToString)
                'WriteArrayToCSV(exp_Units_array, 10, RecObjRef.Object.Name)
#Disable Warning BC42104 ' Variable is used before it has been assigned a value
                WriteArrayToCSV2(exp_Units_array, RecObjRef.Object.Name)
#Enable Warning BC42104 ' Variable is used before it has been assigned a value

            End If

        Next 'SEA_RecObj
        'Next day

        SEA_RecObj = GuidsTemp

        If CreateLegend Then
            If isTimespan Then
                If SEA_GOption = SEA_GOptionValues.Precised Then AddColorDiagram2(doc)

            Else
                If SEA_GOption = SEA_GOptionValues.Precised Then AddColorDiagram(doc)

            End If



        End If

        doc.Views.Redraw()

        RhStopWatch.Stop()

        Return Rhino.Commands.Result.Success

    End Function

    Public Shared Function GenerateDateList(startDate As Date, endDate As Date) As List(Of Date)
        Dim alldays As New List(Of Date)

        Dim currentDay As Date = startDate
        While currentDay <= endDate
            alldays.Add(currentDay)
            currentDay = currentDay.AddDays(1)
        End While

        Return alldays
    End Function

    Public Shared Sub WriteArrayToCSV(array As Integer(), valuesPerLine As Integer, Name As String)
        Dim filePath As String = "c:\temp\output" & Name & ".csv" ' Specify the path and filename for the output CSV file

        Using writer As StreamWriter = New StreamWriter(filePath)
            ' Definiere eine Variable für die Anzahl der Werte in einer Zeile
            'Dim valuesPerLine As Integer = 10
            For i As Integer = 0 To array.Length - 1 Step valuesPerLine
                ' Erstelle eine Liste für diese Zeile
                Dim lineValues As New List(Of Integer)

                ' Füge bis zu 'valuesPerLine' Werte in die Liste ein
                For j As Integer = 0 To valuesPerLine - 1
                    If i + j < array.Length Then
                        lineValues.Add(array(i + j))
                    Else
                        Exit For
                    End If
                Next

                ' Schreibe die Werte dieser Zeile in die Datei, getrennt durch Kommas
                writer.WriteLine(String.Join(",", lineValues))
            Next
        End Using
    End Sub

    Public Shared Sub WriteArrayToCSV2(array As Integer(), Name As String)
        Dim filePath As String = "c:\temp\output" & Name & ".csv" ' Specify the path and filename for the output CSV file

        Using writer As StreamWriter = New StreamWriter(filePath)
            ' Schreibe alle Elemente in einer einzigen Zeile, getrennt durch Kommas
            writer.WriteLine(String.Join(",", array))
        End Using

    End Sub


    ''' <summary>
    ''' it returns how many time units ( in the given set of time) the given vertex is exposed to the sun
    ''' </summary>
    ''' <param name="ModelAbsoluteTolerance"></param>
    ''' <param name="TUnits"></param>
    ''' <param name="vertx"></param>
    ''' <param name="vecN"></param>
    ''' <param name="startT"></param>
    ''' <param name="endT"></param>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Shared Function GenerateArrayfRays(ByVal ModelAbsoluteTolerance As Double,
                                              ByVal TUnits As Double,
                                              ByRef vertx As Rhino.Geometry.Point3d,
                                              ByRef vecN As Rhino.Geometry.Vector3d,
                                              ByVal startT As Double,
                                              ByVal endT As Double,
                                              ByVal id As Guid,
                                              mySunAngle As SunAngle2) As Integer

        Dim exposureU As Integer
        Dim arrExposure As New List(Of Integer)
        Dim minsS As Double

        Dim ray As Rhino.Geometry.Ray3d

        Dim tMonth As Double = month
        Dim tDay As Double = day

        Dim angle As Double

        Dim vec As Rhino.Geometry.Vector3d
        Dim iter As Integer = CType((endT - startT) * TUnits, Integer)
        Dim iMin As Integer = CType(Math.Floor(startT), Integer)
        minsS = startT - iMin
        Dim iMax As Integer = CType(endT, Integer)
        Dim h, mins As Double
        Dim minCount As Integer
        Dim jMax As Integer

        Select Case TUnits
            Case 1
                'Unit = 60min
                minCount = 60
                jMax = 0
            Case 2
                'Unit = 30min
                minCount = 30
                jMax = 1
            Case 4
                'Unit = 15min
                minCount = 15
                jMax = 3
            Case Else
                'Unit = 1min
                minCount = 1
                jMax = 59
        End Select

        Dim param As Double
        Dim tempEx As Integer
        Dim oi As Integer
        Dim angleVec As Double
        'Rhino.RhinoApp.WriteLine("iMin : " & iMin & " iMax : " & iMax & " minsS : " & minsS)
        Dim piHalf As Double = Math.PI / 2

        Dim len As Double
        Dim xVec As New Rhino.Geometry.Vector3d(1, 0, 0)
        Dim yVec As New Rhino.Geometry.Vector3d(0, 1, 0)
        Dim zVec As New Rhino.Geometry.Vector3d(0, 0, 1)
        Dim myMesh As Rhino.Geometry.Mesh

        'mySunAngle.PreCalculate(startT, endT, TUnits)

        For i = iMin To iMax

            For j = 0 To jMax
                mins = minsS * 60 + j * minCount

                h = i + Math.Floor(mins / 60)

                mins = mins Mod 60

                mySunAngle.Calculate(h, mins)
                'mySunAngle.SetAngle(i - iMin, j)


                'RoundTime = Math.Round((h + mins / 60), 2)

                'vec = New Rhino.Geometry.Vector3d(0, 1, 0)
                vec = yVec
                'vec.Rotate(Rhino.RhinoMath.ToRadians(mySunAngle.Altitude), New Rhino.Geometry.Vector3d(1, 0, 0))
                'vec.Rotate(Rhino.RhinoMath.ToRadians(-mySunAngle.Azimuth), New Rhino.Geometry.Vector3d(0, 0, 1))
                vec.Rotate(mySunAngle.AltitudeRad, xVec)
                vec.Rotate(-mySunAngle.AzimuthRad, zVec)

                len = vec.Length

                'If len = 0 Then
                '    Rhino.RhinoApp.WriteLine("vec.Length = 0 ")


                '    Return 0
                'End If

                angle = Rhino.RhinoMath.ToDegrees(Math.Asin(vec.Z / len))

                'check if ray crosses the receiving geometry
                angleVec = Rhino.Geometry.Vector3d.VectorAngle(vec, vecN)

                If Math.Abs(angleVec) <= piHalf AndAlso (angle >= SEA_MinAngle AndAlso angle <= SEA_MaxAngle) Then
                    'we want the ray to be offsetted from the mesh in order to make sure that the calculations are not corrupted by the selfcasting
                    ray = New Rhino.Geometry.Ray3d(vertx + ModelAbsoluteTolerance * vec, vec)
                    'check if ray crosses the occluding geometry
                    oi = 0
                    If SEA_AnType = 0 Then
                        'sun analysis
                        tempEx = 1
                        'Analyse Type = Exposure analysys
                        Try

                            Do Until oi = SEA_CastObj.Count
                                myMesh = SEA_CastObjMesh(oi)
                                'param = Rhino.Geometry.Intersect.Intersection.MeshRay(SEA_CastObjMesh(oi), ray)
                                param = Rhino.Geometry.Intersect.Intersection.MeshRay(myMesh, ray)
                                If (param >= ModelAbsoluteTolerance AndAlso id <> SEA_CastObj(oi)) OrElse
                                    (param > ModelAbsoluteTolerance) Then
                                    tempEx = 0
                                    Exit Do
                                End If

                                oi += 1
                            Loop

                        Catch ex As Exception

                            Rhino.RhinoApp.WriteLine("Redefine occluding geometry")
                            tempEx = 1
#If DEBUG Then
                            Rhino.RhinoApp.WriteLine(ex.ToString)
#End If

                        End Try
                    Else
                        'shadow casting
                        tempEx = 0
                        'Analyse Type = Shadow casting
                        Try
                            Do Until oi = SEA_CastObj.Count 'Or tempEx = 1

                                param = Rhino.Geometry.Intersect.Intersection.MeshRay(SEA_CastObjMesh(oi), ray)

                                If (param >= ModelAbsoluteTolerance AndAlso id <> SEA_CastObj(oi)) OrElse
                                    (param > ModelAbsoluteTolerance) Then

                                    tempEx = 1

                                    Exit Do

                                End If

                                oi += 1

                            Loop

                        Catch ex As Exception
                            Rhino.RhinoApp.WriteLine("Redefine occluding geometry")
                            tempEx = 0
#If DEBUG Then
                            Rhino.RhinoApp.WriteLine(ex.ToString)
#End If
                        End Try
                    End If

                Else
                    tempEx = 0
                End If

                arrExposure.Add(tempEx)

            Next

        Next

        exposureU = GetExposureUnits(arrExposure)

        Return exposureU

    End Function

    'Public Shared Function GetExposureUnits(ByRef LstExposure As List(Of Integer)) As Integer
    Public Shared Function GetExposureUnits(ByRef arrExposure As List(Of Integer)) As Integer
        'Dim ex_Val As Integer '= 0
        'Dim i As Integer
        'Dim sum1, sum2, sumTemp, sumAll As Integer
        'Dim val1, val2 As Integer
        'Dim arrExposure As Integer() = LstExposure.ToArray



        ''check if the exposure units are full exposure time
        'For i = 0 To arrExposure.Count - 2
        '    val1 = arrExposure(i)
        '    val2 = arrExposure(i + 1)
        '    If val1 = 1 And val2 = 0 Then arrExposure(i) = 0
        'Next
        'arrExposure(arrExposure.Count - 1) = 0
        'sum1 = 0
        'sum2 = 0
        'sumTemp = 0
        'sumAll = 0

        ''For i = 0 To arrExposure.Count - 1

        ''    sumAll = sumAll + arrExposure(i)

        ''    If arrExposure(i) = 1 And (i < arrExposure.Count - 1) Then
        ''        sumTemp = sumTemp + arrExposure(i)
        ''    ElseIf arrExposure(i) = 1 And (i = arrExposure.Count - 1) Then
        ''        sumTemp = sumTemp + arrExposure(i)
        ''        If sumTemp > sum1 Then
        ''            sum2 = sum1
        ''            sum1 = sumTemp
        ''        ElseIf sumTemp > sum2 Then
        ''            sum2 = sumTemp
        ''        End If
        ''    Else
        ''        If sumTemp > sum1 Then
        ''            sum2 = sum1
        ''            sum1 = sumTemp
        ''        ElseIf sumTemp > sum2 Then
        ''            sum2 = sumTemp
        ''        End If
        ''        sumTemp = 0
        ''    End If
        ''Next

        'If sum1 = 0 Then sum1 = sumTemp

        ''time segments
        'Select Case SEA_TimeSeg
        '    Case 1
        '        'measurement in max 1 time segment

        '        For i = 0 To arrExposure.Count - 1

        '            sumAll += arrExposure(i)

        '            If arrExposure(i) = 1 And (i = arrExposure.Count - 1) Then
        '                sumTemp += arrExposure(i)
        '                If sumTemp > sum1 Then
        '                    sum1 = sumTemp
        '                End If
        '            Else
        '                If sumTemp > sum1 Then
        '                    sum1 = sumTemp
        '                End If
        '                sumTemp = 0
        '            End If
        '        Next

        '        ex_Val = sum1
        '    Case 2
        '        'measurement in max 2 time segments
        '        For i = 0 To arrExposure.Count - 1

        '            sumAll = sumAll + arrExposure(i)

        '            If arrExposure(i) = 1 And (i < arrExposure.Count - 1) Then
        '                sumTemp = sumTemp + arrExposure(i)
        '            ElseIf arrExposure(i) = 1 And (i = arrExposure.Count - 1) Then
        '                sumTemp = sumTemp + arrExposure(i)
        '                If sumTemp > sum1 Then
        '                    sum2 = sum1
        '                    sum1 = sumTemp
        '                ElseIf sumTemp > sum2 Then
        '                    sum2 = sumTemp
        '                End If
        '            Else
        '                If sumTemp > sum1 Then
        '                    sum2 = sum1
        '                    sum1 = sumTemp
        '                ElseIf sumTemp > sum2 Then
        '                    sum2 = sumTemp
        '                End If
        '                sumTemp = 0
        '            End If
        '        Next


        '        ex_Val = sum1 + sum2
        '    Case Else
        '        'no time segment limit

        '        'For i = 0 To arrExposure.Count - 1

        '        '    sumAll += arrExposure(i)
        '        '    arrExposure.Sum
        '        'Next
        '        sumAll = arrExposure.Sum
        '        ex_Val = sumAll

        'End Select
        '---
        Dim ex_Val As Integer = 0
        Dim i As Integer
        Dim sum1, sum2, sumTemp, sumAll As Integer
        Dim val1, val2 As Integer

        'check if the exposure units are full exposure time
        For i = 0 To arrExposure.Count - 2
            val1 = arrExposure(i)
            val2 = arrExposure(i + 1)
            If val1 = 1 And val2 = 0 Then arrExposure(i) = 0
        Next
        arrExposure(arrExposure.Count - 1) = 0
        sum1 = 0
        sum2 = 0
        sumTemp = 0
        sumAll = 0

        For i = 0 To arrExposure.Count - 1

            sumAll += arrExposure(i)

            If arrExposure(i) = 1 And (i < arrExposure.Count - 1) Then
                sumTemp += arrExposure(i)
            ElseIf arrExposure(i) = 1 And (i = arrExposure.Count - 1) Then
                sumTemp += arrExposure(i)
                If sumTemp > sum1 Then
                    sum2 = sum1
                    sum1 = sumTemp
                ElseIf sumTemp > sum2 Then
                    sum2 = sumTemp
                End If
            Else
                If sumTemp > sum1 Then
                    sum2 = sum1
                    sum1 = sumTemp
                ElseIf sumTemp > sum2 Then
                    sum2 = sumTemp
                End If
                sumTemp = 0
            End If
        Next

        If sum1 = 0 Then sum1 = sumTemp

        'time segments
        If SEA_TimeSeg = 1 Then
            'measurement in max 1 time segment
            ex_Val = sum1
        ElseIf SEA_TimeSeg = 2 Then
            'measurement in max 2 time segments
            ex_Val = sum1 + sum2
        Else
            'no time segment limit
            ex_Val = sumAll
        End If

        ' GetExposureUnits = ex_Val



        Return ex_Val

    End Function

    ''' <summary>
    ''' add color diagram
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Public Shared Function AddColorDiagram2(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result
        Dim ids_col As New List(Of Guid)
        Dim rec As New Rhino.Geometry.Rectangle3d
        Dim vecL, vecH, vecO, vecT As Rhino.Geometry.Vector3d
        Dim pRef As Rhino.Geometry.Point3d
        Dim hatch() As Rhino.Geometry.Hatch
        Dim attrib_hatch As New Rhino.DocObjects.ObjectAttributes
        Dim arr_col As List(Of System.Drawing.Color)
        Dim col As System.Drawing.Color
        Dim plane As Rhino.Geometry.Plane = Rhino.Geometry.Plane.WorldXY
        Dim name As String = "Sun_ColorDiagram"
        Dim pt As Rhino.Geometry.Point3d
        Dim id_hatch, id_rec, id_text As Guid
        Dim text_def As String
        Dim index As Integer
        Dim attribs As New Rhino.DocObjects.ObjectAttributes

        Dim bbox As New Rhino.Geometry.BoundingBox

        AddLayers(doc, "SE_legend2")

        ' index = doc.Layers.Find("SE_legend2", True)

        Dim currentLayer As Rhino.DocObjects.Layer = doc.Layers.FindName("SE_legend2")
        'doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)
        If currentLayer Is Nothing Then
            index = 1
        Else
            index = currentLayer.Index
        End If


        'If index < 0 Then index = 1

        attribs.LayerIndex = index

        'get insertion point
        bbox = GetBBox(doc, SEA_RecObj)
        If Not IsNothing(bbox) Then
            pt = bbox.GetCorners()(1)
        Else
            pt = New Rhino.Geometry.Point3d(0, 0, 0)
        End If


        If SEA_AnType = 0 Then
            'sun exposure
            text_def = " hours of sun exposure"
        Else
            'shadow 
            text_def = " hours of shadow"
        End If

        vecL = New Rhino.Geometry.Vector3d(1, 0, 0)
        vecH = New Rhino.Geometry.Vector3d(0, 1, 0)
        vecO = New Rhino.Geometry.Vector3d(0, 2, 0)
        vecT = New Rhino.Geometry.Vector3d(2, 0, 0)

        arr_col = GetColorSteps()

        If IsNothing(arr_col) Then Return Rhino.Commands.Result.Failure

        For i As Integer = 0 To arr_col.Count - 1
            col = arr_col(i)
            pRef = pt + i * vecO
            plane.Origin = pRef
            rec = New Rhino.Geometry.Rectangle3d(plane, pRef + vecL, pRef + vecH)
            If Not IsNothing(rec) Then
                Dim arr_Rec As New List(Of Rhino.Geometry.Curve)
                arr_Rec.Add(rec.ToNurbsCurve)
                attrib_hatch.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject
                attrib_hatch.ObjectColor = col
                attrib_hatch.Name = name
                attrib_hatch.LayerIndex = index
                hatch = Rhino.Geometry.Hatch.Create(arr_Rec, 0, 0, 0, 0)

                For Each ht In hatch
                    id_hatch = doc.Objects.AddHatch(ht)
                    doc.Objects.ModifyAttributes(id_hatch, attrib_hatch, True)
                    ids_col.Add(id_hatch)
                Next

                'addRec
                id_rec = doc.Objects.AddCurve(rec.ToNurbsCurve)
                doc.Objects.ModifyAttributes(id_rec, attrib_hatch, True)
                ids_col.Add(id_rec)


                'text
                'id_text = AddText(doc, pRef + vecT, CStr(i) & text_def, text_height)

                id_text = AddText(doc, pRef + vecT, CStr(i * 300 + 300) & text_def, text_height)




                doc.Objects.ModifyAttributes(id_text, attribs, True)
                ids_col.Add(id_text)

            End If


        Next

        doc.Groups.Add(ids_col)

        Return Rhino.Commands.Result.Success

    End Function    ''' <summary>
    ''' add color diagram
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Public Shared Function AddColorDiagram(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result
        Dim ids_col As New List(Of Guid)
        Dim rec As New Rhino.Geometry.Rectangle3d
        Dim vecL, vecH, vecO, vecT As Rhino.Geometry.Vector3d
        Dim pRef As Rhino.Geometry.Point3d
        Dim hatch() As Rhino.Geometry.Hatch
        Dim attrib_hatch As New Rhino.DocObjects.ObjectAttributes
        Dim arr_col As New List(Of System.Drawing.Color)
        Dim col As System.Drawing.Color
        Dim plane As Rhino.Geometry.Plane = Rhino.Geometry.Plane.WorldXY
        Dim name As String = "Sun_ColorDiagram"
        Dim pt As Rhino.Geometry.Point3d
        Dim id_hatch, id_rec, id_text As Guid
        Dim text_def As String
        Dim index As Integer
        Dim attribs As New Rhino.DocObjects.ObjectAttributes

        Dim bbox As New Rhino.Geometry.BoundingBox

        AddLayers(doc, "SE_legend")

        'index = doc.Layers.Find("SE_legend", True)
        Dim currentLayer As Rhino.DocObjects.Layer = doc.Layers.FindName("SE_legend2")
        'doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)
        If currentLayer Is Nothing Then
            index = 1
        Else
            index = currentLayer.Index
        End If

        'If index < 0 Then index = 1

        attribs.LayerIndex = index

        'get insertion point
        bbox = GetBBox(doc, SEA_RecObj)
        If Not IsNothing(bbox) Then
            pt = bbox.GetCorners()(1)
        Else
            pt = New Rhino.Geometry.Point3d(0, 0, 0)
        End If


        If SEA_AnType = 0 Then
            'sun exposure
            text_def = " hours of sun exposure"
        Else
            'shadow 
            text_def = " hours of shadow"
        End If

        vecL = New Rhino.Geometry.Vector3d(1, 0, 0)
        vecH = New Rhino.Geometry.Vector3d(0, 1, 0)
        vecO = New Rhino.Geometry.Vector3d(0, 2, 0)
        vecT = New Rhino.Geometry.Vector3d(2, 0, 0)

        arr_col = GetColorSteps()

        If IsNothing(arr_col) Then Return Rhino.Commands.Result.Failure

        For i As Integer = 0 To arr_col.Count - 1
            col = arr_col(i)
            pRef = pt + i * vecO
            plane.Origin = pRef
            rec = New Rhino.Geometry.Rectangle3d(plane, pRef + vecL, pRef + vecH)
            If Not IsNothing(rec) Then
                Dim arr_Rec As New List(Of Rhino.Geometry.Curve)
                arr_Rec.Add(rec.ToNurbsCurve)
                attrib_hatch.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject
                attrib_hatch.ObjectColor = col
                attrib_hatch.Name = name
                attrib_hatch.LayerIndex = index
                hatch = Rhino.Geometry.Hatch.Create(arr_Rec, 0, 0, 0, 0)

                For Each ht In hatch
                    id_hatch = doc.Objects.AddHatch(ht)
                    doc.Objects.ModifyAttributes(id_hatch, attrib_hatch, True)
                    ids_col.Add(id_hatch)
                Next

                'addRec
                id_rec = doc.Objects.AddCurve(rec.ToNurbsCurve)
                doc.Objects.ModifyAttributes(id_rec, attrib_hatch, True)
                ids_col.Add(id_rec)


                'text
                id_text = AddText(doc, pRef + vecT, CStr(i) & text_def, text_height)
                doc.Objects.ModifyAttributes(id_text, attribs, True)
                ids_col.Add(id_text)

            End If


        Next

        doc.Groups.Add(ids_col)
        Return Rhino.Commands.Result.Success

    End Function

    ''' <summary>
    ''' add legend Layers
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Public Shared Function AddLayers(ByVal doc As Rhino.RhinoDoc, layer_name As String) As Rhino.Commands.Result
        If layer_name IsNot Nothing Then

            Return AddLayer(doc, layer_name)

        Else
            Return Rhino.Commands.Result.Failure
        End If

    End Function


End Class
