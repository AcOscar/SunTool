'sun exposure
Imports System.Drawing

Public Class SunExposure2
    Public Shared Function Analyse(ByVal doc As Rhino.RhinoDoc,
                                   ByRef SEAProgressBar As System.Windows.Forms.ProgressBar) As Rhino.Commands.Result

        RhStopWatch.Start()

        'get array of values
        'Dim arr() As Double
        'arr = GetData(doc, lon, lat, TZone, nOffset)

        'variables
        Dim objRef As Rhino.DocObjects.ObjRef
        Dim ptTemp As Rhino.Geometry.Point3d
        Dim vecNtemp As Rhino.Geometry.Vector3d

        Dim threshold As Double
        Dim exp_Units As Integer
        Dim arrVal = GetData(doc, lon, lat, TZone, nOffset)
        Dim tu As Double = SEA_TimeUnits
        Dim vertex_rec As New Rhino.Collections.Point3dList
        Dim vecNorms As New Rhino.Collections.RhinoList(Of Rhino.Geometry.Vector3d)
        Dim newGuid, oldGuid As Guid
        Dim GuidsTemp As New List(Of Guid)
        Dim colTemp As Drawing.Color
        Dim counter As Integer = 0


        Dim numberTotal As Integer = 0
        Dim mesh As New Rhino.Geometry.Mesh
        Dim pts As Rhino.Geometry.Collections.MeshVertexList


        Dim faceNormals As Rhino.Geometry.Collections.MeshVertexNormalList
        Dim faces As Rhino.Geometry.Collections.MeshFaceList
        Dim str_startMin, str_endMin As String

        SEAProgressBar.Value = 0

        If SEA_CastObj Is Nothing Or SEA_RecObj Is Nothing Then Return Rhino.Commands.Result.Failure

        'Dim SunAng As New SunAngle2(0, month, day, 12, 0, TZone, lat, lon, nOffset)


        'Setting the ex_Max
        Dim arrSunValues(3) As Double
        arrSunValues = SunAngle.Calculate(0, month, day, 12, 0, TZone, lat, lon, nOffset)



        Dim startT, endT As Double

        If SEA_StartH >= arrSunValues(2) And SEA_StartH < arrSunValues(3) Then
            startT = SEA_StartH
        ElseIf SEA_StartH > arrSunValues(3) Then
            startT = arrSunValues(3)
        Else
            startT = arrSunValues(2)
        End If

        If SEA_EndH < arrSunValues(3) And SEA_EndH > arrSunValues(2) Then
            endT = SEA_EndH
        ElseIf SEA_EndH < startT Then
            endT = startT
        Else
            endT = arrSunValues(3)
        End If

        Dim exp_Max As Double = (endT - startT) * tu '(sunset - sunrise) * tUnits

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

        Rhino.RhinoApp.WriteLine(" start time : " & CStr(startH) & ":" & str_startMin & " ;end time :" & CStr(endH) & ":" & str_endMin)

        'for the progress bar - calculate how many vertices we have
        For i = 0 To SEA_RecObj.Count - 1
            objRef = New Rhino.DocObjects.ObjRef(SEA_RecObj(i))
            mesh = objRef.Mesh
            pts = mesh.Vertices
            numberTotal += (pts.Count)
        Next

        SEAProgressBar.Maximum = numberTotal

        For i = 0 To SEA_RecObj.Count - 1

            'Finding Vertex of mesh
            vertex_rec = New Rhino.Collections.Point3dList
            vecNorms = New Rhino.Collections.RhinoList(Of Rhino.Geometry.Vector3d)

            objRef = New Rhino.DocObjects.ObjRef(SEA_RecObj(i))

            'debugger
            Try
                mesh = objRef.Mesh
                pts = mesh.Vertices
                faceNormals = mesh.Normals
                faces = mesh.Faces

                'arr of vertexes
                If Not pts Is Nothing Then
                    For j = 0 To pts.Count - 1
                        ptTemp = New Rhino.Geometry.Point3d(pts(j))
                        vertex_rec.Add(ptTemp)
                    Next
                End If

                'arr of normals in vertexes
                If Not faceNormals Is Nothing Then
                    For j = 0 To faceNormals.Count - 1
                        vecNtemp = faceNormals(j)
                        vecNorms.Add(vecNtemp)
                    Next
                End If

                'getting the threshold exposure value
                threshold = SEA_Threshold * tu

                Dim cols As New Rhino.Collections.RhinoList(Of Drawing.Color)

                'coloring mesh vertexes
                For j = 0 To vertex_rec.Count - 1
                    exp_Units = GenerateArrayfRays(doc.ModelAbsoluteTolerance, arrVal, tu, vertex_rec(j), vecNorms(j), startT, endT, SEA_RecObj(i))

                    Dim check_Val As Double
                    check_Val = exp_Units

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

                            comp_Val = CInt(Math.Floor(check_Val / tu))

                            arr_col = GetColorSteps()

                            If comp_Val > (arr_col.Count - 1) Then comp_Val = arr_col.Count - 1

                            colTemp = arr_col(comp_Val)

                    End Select

                    cols.Add(colTemp)

                    'update progress bar
                    counter += 1
                    SEAProgressBar.Value = counter

                Next

                ' Dim arr As Double() = New Double(999999) {}
                'Parallel.[For](0, vertex_rec.Count - 1, Sub(j)
                '                                            'arr(j) = Math.Pow(j, 0.333) * Math.Sqrt(Math.Sin(j))
                '                                            mesh.VertexColors.SetColor(j, cols(j))

                '                                        End Sub)

                For j = 0 To vertex_rec.Count - 1

                    mesh.VertexColors.SetColor(j, cols(j))
                    'mesh.VertexColors.SetColor(mesh.Faces.GetFace(j), cols(j))

                Next
                Dim attribs As Rhino.DocObjects.ObjectAttributes = objRef.[Object]().Attributes

                oldGuid = objRef.ObjectId
                doc.Objects.Delete(objRef, True)
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
        Next

        SEA_RecObj = GuidsTemp

        If SEA_GOption = SEA_GOptionValues.Precised Then AddColorDiagram(doc)

        doc.Views.Redraw()

        RhStopWatch.Stop()

        Return Rhino.Commands.Result.Success

    End Function


    ''' <summary>
    ''' it returns how many time units ( in the given set of time) the given mesh is exposed to the sun
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <param name="arrVal"></param>
    ''' <param name="TUnits"></param>
    ''' <param name="vertx"></param>
    ''' <param name="vecN"></param>
    ''' <param name="startT"></param>
    ''' <param name="endT"></param>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Shared Function GenerateArrayfRays(ByVal ModelAbsoluteTolerance As Double,
                                              ByVal arrVal() As Double,
                                              ByVal TUnits As Double,
                                              ByRef vertx As Rhino.Geometry.Point3d,
                                              ByRef vecN As Rhino.Geometry.Vector3d,
                                              ByVal startT As Double,
                                              ByVal endT As Double,
                                              ByVal id As Guid) As Integer

        Dim exposureU As Integer
        Dim arrExposure As New List(Of Integer)
        Dim minsS As Double

        Dim ray As Rhino.Geometry.Ray3d

        Dim tMonth As Double = month
        Dim tDay As Double = day
        Dim tHours As Double = arrVal(2)
        Dim tMin As Double = arrVal(3)
        Dim fRad As Double = arrVal(7)
        Dim fOff As Double = arrVal(8)

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
                'quartCount = 4
                minCount = 60
                jMax = 0
            Case 2
                'Unit = 30min
                'quartCount = 2
                minCount = 30
                jMax = 1
            Case 4
                'Unit = 15min
                'quartCount = 1
                minCount = 15
                jMax = 3
            Case Else
                'Unit = 1min
                'quartCount = 1 / 15
                minCount = 1
                jMax = 59
        End Select

        Dim param As Double
        Dim tempEx As Integer
        'Dim objRef As Rhino.DocObjects.ObjRef
        Dim oi As Integer
        Dim angleVec As Double
        'Rhino.RhinoApp.WriteLine("iMin : " & iMin & " iMax : " & iMax & " minsS : " & minsS)
        Dim piHalf As Double = Math.PI / 2
        Dim mySunAngle As New SunAngle2(0, tMonth, tDay, TZone, lat, lon, fOff)

        Dim RoundTime As Double
        Dim len As Double

        For i = iMin To iMax

            For j = 0 To jMax
                'mins = minsS * 60 + j * quartCount * 15
                mins = minsS * 60 + j * minCount

                h = i + Math.Floor(mins / 60)

                mins = mins Mod 60

                mySunAngle.Calculate(h, mins)

                RoundTime = Math.Round((h + mins / 60), 2)

                If RoundTime >= Math.Round(mySunAngle.Sunrise, 2) AndAlso
                    RoundTime <= Math.Round(mySunAngle.Sunset, 2) AndAlso
                    RoundTime <= Math.Round(endT, 2) Then

                    vec = New Rhino.Geometry.Vector3d(0, 1, 0)
                    vec.Rotate(Rhino.RhinoMath.ToRadians(mySunAngle.Altitude), New Rhino.Geometry.Vector3d(1, 0, 0))
                    vec.Rotate(Rhino.RhinoMath.ToRadians(-mySunAngle.Azimuth), New Rhino.Geometry.Vector3d(0, 0, 1))

                    len = vec.Length

                    If len = 0 Then Return 0

                    angle = Rhino.RhinoMath.ToDegrees(Math.Asin(vec.Z / len))

                    'check if ray crosses the receiving geometry
                    angleVec = Rhino.Geometry.Vector3d.VectorAngle(vec, vecN)
                    Dim myMesh As Rhino.Geometry.Mesh

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

                End If
            Next

        Next

        exposureU = GetExposureUnits(arrExposure)

        Return exposureU

    End Function

    Public Shared Function GetExposureUnits(ByVal arrExposure As List(Of Integer)) As Integer
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

            sumAll = sumAll + arrExposure(i)

            If arrExposure(i) = 1 And (i < arrExposure.Count - 1) Then
                sumTemp = sumTemp + arrExposure(i)
            ElseIf arrExposure(i) = 1 And (i = arrExposure.Count - 1) Then
                sumTemp = sumTemp + arrExposure(i)
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

        Return ex_Val

    End Function

    ''' <summary>
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

        AddLayers_legend(doc)

        index = doc.Layers.Find("SE_legend", True)

        If index < 0 Then index = 1

        attribs.LayerIndex = index

        'get insertion point
        bbox = GetBBox(SEA_RecObj)
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
                hatch = Rhino.Geometry.Hatch.Create(arr_Rec, 0, 0, 0)

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



    End Function

    ''' <summary>
    ''' add legend Layers
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <returns></returns>
    Public Shared Function AddLayers_legend(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result
        Dim layer_name As String = "SE_legend"

        AddLayer(doc, layer_name)

        Return Rhino.Commands.Result.Success
    End Function


End Class
