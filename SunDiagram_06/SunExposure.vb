'sun exposure
Public Class SE

    Public Shared Function Analyse(ByVal doc As Rhino.RhinoDoc, ByRef SEAProgressBar As System.Windows.Forms.ProgressBar) As Rhino.Commands.Result
        RhStopWatch.Start()

        'add a "SEA_mesh" layer
        'AddLayers_SEA(doc)

        'get array of values
        Dim arr() As Double
        arr = GetData(doc, lon, lat, TZone, nOffset)

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
        Dim SEA_Progress As Integer

        SEA_Progress = 0

        If SEA_OccObj Is Nothing Or SEA_RecObj Is Nothing Then Return Rhino.Commands.Result.Failure

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

        If Len(str_startMin) = 1 Then
            str_startMin = "0" & str_startMin
        End If

        If Len(str_endMin) = 1 Then
            str_endMin = "0" & str_endMin
        End If

        Rhino.RhinoApp.WriteLine(" start time : " & CStr(startH) & ":" & str_startMin & " ;end time :" & CStr(endH) & ":" & str_endMin)
        'Rhino.RhinoApp.WriteLine(CStr(exp_Max))

        'for the progress bar - calculate how many vertexes we already have
        For i = 0 To SEA_RecObj.Count - 1
            objRef = New Rhino.DocObjects.ObjRef(SEA_RecObj(i))
            mesh = objRef.Mesh
            pts = mesh.Vertices
            numberTotal = numberTotal + (pts.Count - 1)
        Next

        For i = 0 To SEA_RecObj.Count - 1
            'Finding Vertex of mesh
            vertex_rec = New Rhino.Collections.Point3dList
            vecNorms = New Rhino.Collections.RhinoList(Of Rhino.Geometry.Vector3d)

            'Dim myObj = doc.Objects.Find(SEA_RecObj(i))
            objRef = New Rhino.DocObjects.ObjRef(SEA_RecObj(i))

            'debugger
            Try
                'If Not objRef Is Nothing Then
                mesh = objRef.Mesh
                pts = mesh.Vertices
                faceNormals = mesh.Normals
                faces = mesh.Faces

                'arr of vertexes
                If Not pts Is Nothing Then
                    For j = 0 To pts.Count - 1
                        ptTemp = New Rhino.Geometry.Point3d(pts(j))
                        vertex_rec.Add(ptTemp)
                        'doc.Objects.AddPoint(pts(j))
                    Next
                End If

                'arr of CenterptsOf a face and normals
                'If Not faces Is Nothing Then
                '    For j = 0 To faces.Count - 1
                '        ptTemp = faces.GetFaceCenter(j)
                '        vertex_rec.Add(ptTemp)
                '        'doc.Objects.AddPoint(ptTemp)
                '    Next
                'End If

                'Dim faceNormals = mesh.FaceNormals


                'arr of normals in vertexes
                If Not faceNormals Is Nothing Then
                    For j = 0 To faceNormals.Count - 1
                        vecNtemp = faceNormals(j)
                        vecNorms.Add(vecNtemp)
                    Next
                End If

                'Rhino.RhinoApp.WriteLine("nbr of vertxs : " & vertex_rec.Count & "nbr of normals: " & vecNorms.Count)

                'For i = 0 To vertex_rec.Count - 1
                '    Dim line As New Rhino.Geometry.Line(vertex_rec(i), New Rhino.Geometry.Point3d(vertex_rec(i) + vecNorms(i)))
                '    doc.Objects.AddLine(line)
                'Next

                'getting the threshold exposure value
                threshold = SEA_Threshold * tu

                Dim cols As New Rhino.Collections.RhinoList(Of Drawing.Color)

                'coloring mesh vertexes
                For j = 0 To vertex_rec.Count - 1
                    exp_Units = GenerateArrayfRays(doc, arrVal, tu, vertex_rec(j), vecNorms(j), startT, endT, SEA_RecObj(i))
                    Dim check_Val As Double
                    check_Val = exp_Units
                    'if AnalyseType = Shadow Casting then 
                    'If SEA_AnType = 1 Then check_Val = exp_Max - exp_Units

                    Select Case SEA_GOption
                        Case SEA_GOptionValues.Threshold

                            If check_Val >= threshold Then
                                colTemp = SEA_colorPos
                            Else
                                colTemp = SEA_colorNeg
                            End If

                        Case SEA_GOptionValues.Gradient

                            Dim r = Map(check_Val, 0, exp_Max, SEA_colorNeg.R, SEA_colorPos.R)
                            If r < 0 Then
                                r = 0
                            ElseIf r > 255 Then
                                r = 255
                            End If
                            Dim g = Map(check_Val, 0, exp_Max, SEA_colorNeg.G, SEA_colorPos.G)
                            If g < 0 Then
                                g = 0
                            ElseIf g > 255 Then
                                g = 255
                            End If
                            Dim b = Map(check_Val, 0, exp_Max, SEA_colorNeg.B, SEA_colorPos.B)
                            If b < 0 Then
                                b = 0
                            ElseIf b > 255 Then
                                b = 255
                            End If

                            colTemp = Drawing.Color.FromArgb(255, r, g, b)

                        Case SEA_GOptionValues.Precised

                            Dim arr_col As New List(Of System.Drawing.Color)
                            Dim comp_Val As Integer

                            comp_Val = CInt(Math.Floor(check_Val / tu))

                            arr_col = GetColorSteps()

                            If comp_Val > (arr_col.Count - 1) Then comp_Val = arr_col.Count - 1

                            colTemp = arr_col(comp_Val)

                    End Select

                    cols.Add(colTemp)

                    'progress bar
                    counter += 1
                    SEA_Progress = CInt((counter / numberTotal) * 100)
                    If SEA_Progress > 100 Then SEA_Progress = 100
                    'Rhino.RhinoApp.WriteLine(counter & " _ " & numberTotal & " _ " & SEA_Progress)
                    SEAProgressBar.Value = SEA_Progress
                Next

                For j = 0 To vertex_rec.Count - 1

                    mesh.VertexColors.SetColor(j, cols(j))
                    'mesh.VertexColors.SetColor(mesh.Faces.GetFace(j), cols(j))

                Next
                Dim attribs As Rhino.DocObjects.ObjectAttributes = objRef.[Object]().Attributes
                'Dim index = doc.Layers.Find("SEA_mesh", True)
                'attribs.LayerIndex = index

                oldGuid = objRef.ObjectId
                doc.Objects.Delete(objRef, True)
                newGuid = doc.Objects.AddMesh(mesh, attribs)

                For j = 0 To SEA_OccObj.Count - 1
                    If SEA_OccObj(j) = oldGuid Then SEA_OccObj(j) = newGuid
                Next
                If Not newGuid = Nothing Then GuidsTemp.Add(newGuid)
                'End If
            Catch ex As Exception
                Rhino.RhinoApp.WriteLine("Redefine receiving geometry")
                'Return Rhino.Commands.Result.Failure
            End Try
        Next

        SEA_RecObj = GuidsTemp

        If SEA_GOption = 2 Then AddColorDiagram(doc)

        doc.Views.Redraw()

        RhStopWatch.Stop()

        Return Rhino.Commands.Result.Success
    End Function

    'it returns how many time units ( in the given set of time) the given mesh is exposed to the sun
    Public Shared Function GenerateArrayfRays(ByVal doc As Rhino.RhinoDoc, ByVal arrVal() As Double, ByVal TUnits As Double, ByVal vertx As Rhino.Geometry.Point3d, ByVal vecN As Rhino.Geometry.Vector3d, ByVal startT As Double, ByVal endT As Double, ByVal id As Guid) As Integer

        Dim exposureU As Integer = 0
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

        Dim arrSunValues(3) As Double

        Dim vec As Rhino.Geometry.Vector3d
        'Rhino.RhinoApp.WriteLine(CStr(startT) & " _ " & CStr(endT))
        Dim iter As Integer = CType((endT - startT) * TUnits, Integer)
        'Dim iMin As Integer = CType(startT, Integer)
        Dim iMin As Integer = CType(Math.Floor(startT), Integer)
        minsS = startT - iMin
        'Rhino.RhinoApp.WriteLine(CStr(iMin) & " _ " & CStr(minsS))
        Dim iMax As Integer = CType(endT, Integer)
        'Dim iter As Integer = CType((SEA_EndH - SEA_StartH) * TUnits, Integer)
        'Dim iMin As Integer = CType(SEA_StartH, Integer)
        'Dim iMax As Integer = CType(SEA_EndH, Integer)
        Dim h, mins As Double
        'Dim quartCount As Integer
        Dim minCount As Integer
        Dim jMax As Integer
        If TUnits = 1 Then
            'Unit = 60min
            'quartCount = 4
            minCount = 60
            jMax = 0
        ElseIf TUnits = 2 Then
            'Unit = 30min
            'quartCount = 2
            minCount = 30
            jMax = 1
        ElseIf TUnits = 4 Then
            'Unit = 15min
            'quartCount = 1
            minCount = 15
            jMax = 3
        Else
            'Unit = 1min
            'quartCount = 1 / 15
            minCount = 1
            jMax = 59
        End If
        Dim param As Double
        Dim tempEx As Integer
        Dim objRef As Rhino.DocObjects.ObjRef
        Dim oi As Integer

        'Rhino.RhinoApp.WriteLine("iMin : " & iMin & " iMax : " & iMax & " minsS : " & minsS)

        For i = iMin To iMax
            'h = i
            For j = 0 To jMax
                'mins = minsS * 60 + j * quartCount * 15
                mins = minsS * 60 + j * minCount

                h = i + Math.Floor(mins / 60)

                mins = mins Mod 60
                'Rhino.RhinoApp.WriteLine(CStr(i) & " _ " & CStr(mins) & " _ " & CStr(h))
                arrSunValues = SunAngle.Calculate(0, tMonth, tDay, h, mins, TZone, lat, lon, fOff)

                'tempEx = 1
                'Rhino.RhinoApp.WriteLine(CStr(Math.Round((h + mins / 60), 2)))
                If Math.Round((h + mins / 60), 2) >= Math.Round(arrSunValues(2), 2) And Math.Round((h + mins / 60), 2) <= Math.Round(arrSunValues(3), 2) And Math.Round((h + mins / 60), 2) <= Math.Round(endT, 2) Then
                    'Rhino.RhinoApp.WriteLine(CStr(Math.Round((h + mins / 60), 2)) & " _ " & CStr(Math.Round(arrSunValues(2), 2)) & " _ " & CStr(Math.Round(arrSunValues(3), 2)) & " _ " & CStr(Math.Round(endT, 2)))
                    vec = GetSunVector(doc, arrSunValues(0), arrSunValues(1))
                    angle = GetVecAngle(vec)


                    'ray = New Rhino.Geometry.Ray3d(vertx, vec)
                    'we want the ray to be offsetted from the mesh in order to make sure that the calculations are not corrupted by the selfcasting
                    ray = New Rhino.Geometry.Ray3d(vertx + doc.ModelAbsoluteTolerance * vec, vec)

                    'check if ray crosses the receiving geometry
                    Dim angleVec As Double = Rhino.Geometry.Vector3d.VectorAngle(vec, vecN)

                    If Math.Abs(Rhino.RhinoMath.ToDegrees(angleVec)) <= 90 And (angle >= SEA_MinAngle And angle <= SEA_MaxAngle) Then
                        'check if ray crosses the occluding geometry
                        oi = 0
                        If SEA_AnType = 0 Then
                            'sun analysis
                            tempEx = 1
                            'Analyse Type = Exposure analysys
                            Do Until oi = SEA_OccObj.Count Or tempEx = 0
                                objRef = New Rhino.DocObjects.ObjRef(SEA_OccObj(oi))
                                Try

                                    param = Rhino.Geometry.Intersect.Intersection.MeshRay(objRef.Mesh, ray)
                                    'If param > 0.1 Then tempEx = 0
                                    If (param >= doc.ModelAbsoluteTolerance And id <> SEA_OccObj(oi)) Or (param > doc.ModelAbsoluteTolerance) Then tempEx = 0

                                Catch ex As Exception
                                    'Rhino.RhinoApp.WriteLine("Redefine occluding geometry")
                                    tempEx = 1
                                    'Return Rhino.Commands.Result.Failure
                                End Try
                                oi = oi + 1
                            Loop
                        Else
                            'shadow casting
                            tempEx = 0
                            'Analyse Type = Shadow casting
                            Do Until oi = SEA_OccObj.Count Or tempEx = 1
                                objRef = New Rhino.DocObjects.ObjRef(SEA_OccObj(oi))
                                Try
                                    param = Rhino.Geometry.Intersect.Intersection.MeshRay(objRef.Mesh, ray)
                                    'If param > 0.1 Then tempEx = 1
                                    'Dim idd As Guid
                                    'Dim attribs = doc.CreateDefaultAttributes()
                                    'attribs.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject
                                    'attribs.Name = CStr(h & ":" & mins)
                                    'idd = doc.Objects.AddLine(New Rhino.Geometry.Line(vertx, ray.PointAt(1)))
                                    If (param >= doc.ModelAbsoluteTolerance And id <> SEA_OccObj(oi)) Or (param > doc.ModelAbsoluteTolerance) Then
                                        tempEx = 1
                                        'attribs.ObjectColor = Drawing.Color.Blue
                                    Else
                                        'attribs.ObjectColor = Drawing.Color.Red
                                    End If
                                    'doc.Objects.ModifyAttributes(idd, attribs, True)

                                Catch ex As Exception
                                    'Rhino.RhinoApp.WriteLine("Redefine occluding geometry")
                                    tempEx = 0
                                    'Return Rhino.Commands.Result.Failure
                                End Try
                                oi = oi + 1
                            Loop
                        End If

                    Else
                        tempEx = 0
                        'if AnalyseType = Shadow Casting - AVOID SELFCASTING 
                        'If SEA_AnType = 1 Then tempEx = 1
                    End If

                    ' exposureUTemp = exposureUTemp + tempEx
                    arrExposure.Add(tempEx)

                    'if notShaded then show line
                    'If tempEx = 1 Then
                    'Dim attribs = doc.CreateDefaultAttributes()
                    'attribs.Name = CStr(h) & "_" & CStr(mins)
                    'Dim line As New Rhino.Geometry.Line(vertx, New Rhino.Geometry.Point3d(vertx + vec))
                    'doc.Objects.AddLine(line, attribs)
                    'End If
                End If
            Next

        Next
        exposureU = GetExposureUnits(arrExposure)
        'Rhino.RhinoApp.WriteLine(CStr(exposureU))
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

        GetExposureUnits = ex_Val

    End Function

    'add SunDiagram Layers
    Public Shared Function AddLayers_SEA(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result
        Dim layer_name As String = "SEA_mesh"
        AddLayer(doc, layer_name)

        Return Rhino.Commands.Result.Success
    End Function

  
    'add color diagram
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

    'add legend Layers
    Public Shared Function AddLayers_legend(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result
        Dim layer_name As String = "SE_legend"

        AddLayer(doc, layer_name)

        Return Rhino.Commands.Result.Success
    End Function

    Public Shared Function ResetMesh(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result

        Dim mesh As Rhino.Geometry.Mesh

        Dim go As New Rhino.Input.Custom.GetObject()
        go.SetCommandPrompt("pick mesh to reset")
        go.GeometryAttributeFilter = Rhino.Input.Custom.GeometryAttributeFilter.ClosedMesh Or Rhino.Input.Custom.GeometryAttributeFilter.OpenMesh
        go.SubObjectSelect = False
        go.GroupSelect = True
        go.GetMultiple(1, 0)
        If go.CommandResult() <> Rhino.Commands.Result.Success Then
            Return go.CommandResult

            'Exit Function
        End If
        For i As Integer = 0 To go.ObjectCount - 1
            Dim objref As Rhino.DocObjects.ObjRef = New Rhino.DocObjects.ObjRef(go.[Object](i).ObjectId)
            If Not IsNothing(objref.Mesh) Then
                mesh = objref.Mesh

                Dim attribs As Rhino.DocObjects.ObjectAttributes = objref.[Object]().Attributes

                'doc.Objects.ModifyAttributes(objref, attribs, True)
                mesh.VertexColors.Clear()
             
                Try
                    doc.Objects.Delete(objref, True)
                    doc.Objects.AddMesh(mesh, attribs)
                Catch ex As Exception

                End Try

            End If
        Next

        Return Rhino.Commands.Result.Success
    End Function

End Class
