Public Class SPD2
    Public Shared ids As New List(Of Guid)()

    'main function, that draws the diagram
    Public Shared Function Draw(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result
        Rhino.RhinoApp.WriteLine("Draw Sun Path Diagram")

        Dim arr() As Double
        arr = GetData(lon, lat, TZone, nOffset)
        AddLayers_SDP(doc)
        ids = New List(Of Guid)()

        DrawStereoscopicTemplate(doc, arr(7), arr(8))

        DrawStereoscopicSunPathMonthlyDiagram(doc, arr, SPD_is3D)

        DrawStereoscopicSunPathHourlyDiagram(doc, arr, SPD_is3D)

        doc.Groups.Add(ids)

        doc.Views.Redraw()

        Return Rhino.Commands.Result.Success

    End Function

    '# draw Stereoscopic Template
    Private Shared Function DrawStereoscopicTemplate(ByVal doc As Rhino.RhinoDoc, ByVal fRad As Double, ByVal fOffset As Double) As Rhino.Commands.Result

        'Define source plane
        Dim source_plane As Rhino.Geometry.Plane = Rhino.Geometry.Plane.WorldXY

        'Define main functions variables
        Dim center As New Rhino.Geometry.Point3d(0, 0, 0)
        Dim arrObj As New System.Collections.Generic.List(Of Guid)()

        Dim xform As Rhino.Geometry.Transform = Rhino.Geometry.Transform.Rotation(Rhino.RhinoMath.ToRadians(-fOffset), center)
        Dim delete_original As Boolean = True

        'set current layer
        'Dim index As Integer = doc.Layers.Find("SPD_template", True)
        'doc.Layers.SetCurrentLayerIndex(index, True)
        Dim currentLayer As Rhino.DocObjects.Layer = doc.Layers.FindName("SPD_template")
        doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

        'Draw outer ring and main axes
        Dim c As New Rhino.Geometry.Circle(center, fRad + 1)
        Dim l1 As New Rhino.Geometry.Line(New Rhino.Geometry.Point3d(-fRad, 0, 0), New Rhino.Geometry.Point3d(fRad, 0, 0))
        Dim l2 As New Rhino.Geometry.Line(New Rhino.Geometry.Point3d(0, -fRad, 0), New Rhino.Geometry.Point3d(0, fRad, 0))
        arrObj.Add(doc.Objects.Transform(doc.Objects.AddCircle(c), xform, delete_original))
        arrObj.Add(doc.Objects.Transform(doc.Objects.AddLine(l1), xform, delete_original))
        arrObj.Add(doc.Objects.Transform(doc.Objects.AddLine(l2), xform, delete_original))

        'draw concentric circles
        Dim tRad As Double
        For i As Integer = 0 To 80 Step 10
            tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(i))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(i)))
            c.Center = center
            c.Radius = tRad
            arrObj.Add(doc.Objects.Transform(doc.Objects.AddCircle(c), xform, delete_original))
        Next

        Dim t As String
        Dim height As Double = 1.5
        Dim font As String = "Arial"
        Dim plane As Rhino.Geometry.Plane

        'draw small lines every 15 degrees
        Dim pt As New Rhino.Geometry.Point3d(0, fRad, 0)
        Dim ax As New Rhino.Geometry.Vector3d(0, 0, 1)
        Dim vecS As Rhino.Geometry.Vector3d
        Dim vecE As Rhino.Geometry.Vector3d

        For i As Integer = 0 To 355 Step 5
            vecS = New Rhino.Geometry.Vector3d(pt)
            vecS.Rotate(Rhino.RhinoMath.ToRadians(-i), ax)
            vecE = vecS
            vecE.Unitize()
            Dim l As New Rhino.Geometry.Line(New Rhino.Geometry.Point3d(vecS), vecE)
            arrObj.Add(doc.Objects.Transform(doc.Objects.AddLine(l), xform, delete_original))

            'draw azimuth angles as text
            Dim j As Double = i Mod 15
            If j = 0 And Not i = 355 Then
                t = i & Chr(176)
                plane = source_plane
                vecE = vecS + 4 * vecE + New Rhino.Geometry.Vector3d(-1.5, -height / 2, 0)
                plane.Origin = New Rhino.Geometry.Point3d(vecE)
                arrObj.Add(doc.Objects.Transform(doc.Objects.AddText(t, plane, height, font, False, False), xform, delete_original))
            End If

        Next

        'draw altitude angles as text
        For i As Integer = 10 To 80 Step 10
            tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(i))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(i))) + 0.5
            t = i & Chr(176)
            plane = source_plane
            plane.Origin = New Rhino.Geometry.Point3d(0.5, tRad, 0)
            arrObj.Add(doc.Objects.Transform(doc.Objects.AddText(t, plane, height, font, False, False), xform, delete_original))
        Next

        For i As Integer = 0 To arrObj.Count - 1
            ids.Add(arrObj(i))
        Next

        Return Rhino.Commands.Result.Success

    End Function

    '# draw Stereoscopic Template - monthly
    Private Shared Function DrawStereoscopicSunPathMonthlyDiagram(ByVal doc As Rhino.RhinoDoc,
                                                                    ByVal arrVal() As Double,
                                                                    ByVal Is3D As Boolean) As Rhino.Commands.Result

        Dim tMonth As Double = arrVal(0)
        Dim tDay As Double = arrVal(1)
        Dim tHours As Double = arrVal(2)
        Dim tMin As Double = arrVal(3)
        Dim lat As Double = arrVal(4)
        Dim lon As Double = arrVal(5)
        Dim tZone As Double = arrVal(6)
        Dim fRad As Double = arrVal(7)
        Dim fOff As Double = arrVal(8)

        Dim arrMonth() As String = {"", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
        Dim arrSunValues(3) As Double
        Dim tRad As Double
        Dim vec As Rhino.Geometry.Vector3d
        Dim ax As New Rhino.Geometry.Vector3d(0, 0, 1)
        Dim arrPtSunRise, arrPtSunSet As Rhino.Geometry.Point3d
        Dim temp1, temp2 As Integer
        Dim crv As Rhino.Geometry.Curve
        Dim attribs = doc.CreateDefaultAttributes()
        Dim t As String
        Dim plane As Rhino.Geometry.Plane
        Dim source_plane As Rhino.Geometry.Plane = Rhino.Geometry.Plane.WorldXY
        Dim height As Double = 1.5
        Dim font As String = "Arial"
        Dim vecT As Rhino.Geometry.Vector3d
        'Dim index As Integer
        Dim currentLayer As Rhino.DocObjects.Layer
        Dim arrObj As New System.Collections.Generic.List(Of Guid)()

        'calculate and draw monhly path
        If setDate = False Then
            'Option without precised date
            For i As Integer = 1 To 12
                Dim arrPts As New Rhino.Collections.Point3dList
                tMonth = i
                arrSunValues = SunAngle.Calculate(0, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
                If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel

                tHours = Math.Floor(arrSunValues(2))
                tMin = (arrSunValues(2) - tHours) * 60
                arrSunValues = SunAngle.Calculate(0, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
                If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel

                If Is3D Then

                    tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))

                Else

                    tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))

                End If

                vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
                If Is3D Then
                    vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))
                End If
                vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
                arrPtSunRise = New Rhino.Geometry.Point3d(vec)
                arrPts.Add(arrPtSunRise)

                temp1 = CType(arrSunValues(2), Integer) + 1
                temp2 = CType(arrSunValues(3), Integer) - 1
                For j As Integer = temp1 To temp2
                    tHours = j
                    arrSunValues = SunAngle.Calculate(0, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
                    If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel
                    If Is3D Then
                        tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                    Else
                        tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                    End If
                    vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
                    If Is3D Then
                        vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))
                    End If
                    vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
                    Dim arrPt As New Rhino.Geometry.Point3d(vec)
                    arrPts.Add(arrPt)

                Next

                tHours = Math.Floor(arrSunValues(3))
                tMin = (arrSunValues(3) - tHours) * 60
                arrSunValues = SunAngle.Calculate(0, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
                If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel
                If Is3D Then
                    tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                Else
                    tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                End If
                vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
                If Is3D Then
                    vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))
                End If
                vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
                arrPtSunSet = New Rhino.Geometry.Point3d(vec)
                arrPts.Add(arrPtSunSet)

                crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPts, 3)

                If i <= 6 Then
                    t = "1. " & arrMonth(i)
                    plane = source_plane
                    vecT = New Rhino.Geometry.Vector3d(arrPtSunRise) + New Rhino.Geometry.Vector3d(1.5, 0, 0)
                    plane.Origin = New Rhino.Geometry.Point3d(vecT)

                    'set current layer
                    'index = doc.Layers.Find("SPD_monthPath_1-6", True)
                    'doc.Layers.SetCurrentLayerIndex(index, True)
                    currentLayer = doc.Layers.FindName("SPD_monthPath_1-6")
                    doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                    arrObj.Add(doc.Objects.AddText(t, plane, height, font, False, False))
                Else
                    attribs.ObjectColor = Drawing.Color.Blue

                    t = "1. " & arrMonth(i)
                    plane = source_plane
                    vecT = New Rhino.Geometry.Vector3d(arrPtSunSet) + New Rhino.Geometry.Vector3d(-9, 0, 0)
                    plane.Origin = New Rhino.Geometry.Point3d(vecT)

                    'set current layer
                    'index = doc.Layers.Find("SPD_monthPath_7-12", True)
                    'doc.Layers.SetCurrentLayerIndex(index, True)
                    currentLayer = doc.Layers.FindName("SPD_monthPath_7-12")
                    doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                    arrObj.Add(doc.Objects.AddText(t, plane, height, font, False, False))
                End If

                arrObj.Add(doc.Objects.AddCurve(crv))

            Next
        Else
            'Option with precised date
            Dim arrPts As New Rhino.Collections.Point3dList
            tMonth = month
            tDay = day

            arrSunValues = SunAngle.Calculate(0, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
            If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel

            tHours = Math.Floor(arrSunValues(2))
            tMin = (arrSunValues(2) - tHours) * 60
            arrSunValues = SunAngle.Calculate(0, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
            If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel
            If Is3D Then
                tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
            Else
                tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
            End If

            vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
            If Is3D Then
                vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))

            End If
            vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
            arrPtSunRise = New Rhino.Geometry.Point3d(vec)
            arrPts.Add(arrPtSunRise)

            temp1 = CType(arrSunValues(2), Integer) + 1
            temp2 = CType(arrSunValues(3), Integer) - 1
            For j As Integer = temp1 To temp2
                tHours = j
                arrSunValues = SunAngle.Calculate(0, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
                If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel
                If Is3D Then
                    tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                Else
                    tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                End If
                vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
                If Is3D Then
                    vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))

                End If
                vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
                Dim arrPt As New Rhino.Geometry.Point3d(vec)
                arrPts.Add(arrPt)
            Next

            tHours = Math.Floor(arrSunValues(3))
            tMin = (arrSunValues(3) - tHours) * 60
            arrSunValues = SunAngle.Calculate(0, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
            If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel
            If Is3D Then
                tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
            Else
                tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
            End If
            vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
            If Is3D Then
                vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))
            End If
            vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
            arrPtSunSet = New Rhino.Geometry.Point3d(vec)
            arrPts.Add(arrPtSunSet)

            crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPts, 3)

            If month <= 6 Then

                t = tDay & ". " & arrMonth(CInt(month))
                plane = source_plane
                vecT = New Rhino.Geometry.Vector3d(arrPtSunRise) + New Rhino.Geometry.Vector3d(1.5, 0, 0)
                plane.Origin = New Rhino.Geometry.Point3d(vecT)

                'set current layer
                'index = doc.Layers.Find("SPD_monthPath_1-6", True)
                'doc.Layers.SetCurrentLayerIndex(index, True)
                currentLayer = doc.Layers.FindName("SPD_monthPath_1-6")
                doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                arrObj.Add(doc.Objects.AddText(t, plane, height, font, False, False))

            Else
                t = tDay & ". " & arrMonth(CInt(month))
                plane = source_plane
                vecT = New Rhino.Geometry.Vector3d(arrPtSunSet) + New Rhino.Geometry.Vector3d(-9, 0, 0)
                plane.Origin = New Rhino.Geometry.Point3d(vecT)

                'set current layer
                'index = doc.Layers.Find("SPD_monthPath_7-12", True)
                'doc.Layers.SetCurrentLayerIndex(index, True)
                currentLayer = doc.Layers.FindName("SPD_monthPath_7-12")
                doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                arrObj.Add(doc.Objects.AddText(t, plane, height, font, False, False))
            End If
            arrObj.Add(doc.Objects.AddCurve(crv))
        End If

        For i As Integer = 0 To arrObj.Count - 1
            ids.Add(arrObj(i))
        Next

        Return Rhino.Commands.Result.Success
    End Function

    '# draw stereoscopic sunpath diagram - hourly
    Private Shared Function DrawStereoscopicSunPathHourlyDiagram(ByVal doc As Rhino.RhinoDoc,
                                                                   ByVal arrVal() As Double,
                                                                   ByVal Is3D As Boolean) As Rhino.Commands.Result

        Dim tMonth As Double = arrVal(0)
        Dim tDay As Double = arrVal(1)
#Disable Warning IDE0059 ' Unnecessary assignment of a value
        Dim tHours As Double = arrVal(2)
        Dim tMin As Double = arrVal(3)
        Dim lat As Double = arrVal(4)
        Dim lon As Double = arrVal(5)
        Dim tZone As Double = arrVal(6)
        Dim fRad As Double = arrVal(7)
        Dim fOff As Double = arrVal(8)

        Dim arrSunValues(3) As Double
        Dim tRad As Double
        Dim vec As Rhino.Geometry.Vector3d
        Dim arrVec As Rhino.Geometry.Vector3d
        Dim ax As New Rhino.Geometry.Vector3d(0, 0, 1)
        Dim arrCut(1) As Rhino.Geometry.Point3d
        Dim t As String
        Dim height As Double = 1.5
        Dim font As String = "Arial"
        Dim plane As Rhino.Geometry.Plane
        Dim source_plane As Rhino.Geometry.Plane = Rhino.Geometry.Plane.WorldXY
        Dim crv As Rhino.Geometry.Curve
        Dim j As Integer
        Dim arrObj As New System.Collections.Generic.List(Of Guid)()
        'Dim index As Integer
        Dim currentLayer As Rhino.DocObjects.Layer
        Dim sphere As Rhino.Geometry.Sphere
        Dim vec_sun As Rhino.Geometry.Vector3d
        Dim angle As Double
#Enable Warning IDE0059 ' Unnecessary assignment of a value

        ' calculate and hourly path
        Dim flag As Integer
        If setHour = False Then
            For i = 0 To 23
                flag = 0
                tHours = i
                tMin = 0
                Dim arrPts As New Rhino.Collections.Point3dList
                Dim arrPtsB, arrPtsR As New Rhino.Collections.Point3dList

                If setDate = False Then
                    For j = 1 To 365 Step 3
                        arrSunValues = SunAngle.Calculate(j, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
                        If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel

                        If i >= arrSunValues(2) And i <= arrSunValues(3) Then
                            If Is3D Then
                                tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                            Else
                                tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                            End If
                            vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
                            If Is3D Then
                                vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))
                            End If
                            vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
                            Dim arrPt As New Rhino.Geometry.Point3d(vec)

                            arrPts.Add(arrPt)

                            If j <= 172 Then
                                arrPtsR.Add(arrPt)
                            End If

                            If j = 355 Then
                                arrPtsR.Insert(0, arrPt)
                            End If

                            If j >= 172 And j <= 355 Then
                                arrPtsB.Add(arrPt)
                            End If

                            If j = 184 Then
                                arrVec = vec
                                arrVec.Unitize()
                                arrVec *= (-3)
                                arrVec += New Rhino.Geometry.Vector3d(-2, 0, 0)
                                arrVec += vec

                                t = i & ":00"
                                plane = source_plane
                                plane.Origin = New Rhino.Geometry.Point3d(arrVec)
                                arrObj.Add(doc.Objects.AddText(t, plane, height, font, False, False))
                            End If
                        Else
                            flag = 1
                        End If
                    Next


                    If arrPts.Count > 0 And flag = 0 Then
                        'set current layer
                        'index = doc.Layers.Find("SPD_hourPath_1-6", True)
                        'doc.Layers.SetCurrentLayerIndex(Index, True)
                        currentLayer = doc.Layers.FindName("SPD_hourPath_1-6")
                        doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                        'add Crv
                        crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPtsR, 3)
                        arrObj.Add(doc.Objects.AddCurve(crv))

                        'set current layer
                        'index = doc.Layers.Find("SPD_hourPath_7-12", True)
                        'doc.Layers.SetCurrentLayerIndex(Index, True)
                        currentLayer = doc.Layers.FindName("SPD_hourPath_7-12")
                        doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                        'add Crv
                        crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPtsB, 3)
                        arrObj.Add(doc.Objects.AddCurve(crv))

                    ElseIf arrPts.Count > 0 And flag = 1 Then
                        'set current layer
                        'index = doc.Layers.Find("SPD_hourPath_1-6", True)
                        'doc.Layers.SetCurrentLayerIndex(Index, True)
                        currentLayer = doc.Layers.FindName("SPD_hourPath_1-6")
                        doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                        'add Crv
                        crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPtsR, 3)
                        arrObj.Add(doc.Objects.AddCurve(crv))

                        'set current layer
                        'index = doc.Layers.Find("SPD_hourPath_7-12", True)
                        'doc.Layers.SetCurrentLayerIndex(Index, True)
                        currentLayer = doc.Layers.FindName("SPD_hourPath_7-12")
                        doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                        'add Crv
                        crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPtsB, 3)
                        arrObj.Add(doc.Objects.AddCurve(crv))

                    End If

                Else
                    j = 0
                    tMonth = month
                    tDay = day
                    arrSunValues = SunAngle.Calculate(j, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
                    If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel

                    If i >= arrSunValues(2) And i <= arrSunValues(3) Then
                        If Is3D Then
                            tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                        Else
                            tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                        End If
                        vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
                        If Is3D Then
                            vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))
                        End If
                        vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
                        Dim arrPt As New Rhino.Geometry.Point3d(vec)
                        If Is3D Then
                            If tMonth <= 6 Then
                                'index = doc.Layers.Find("SPD_hourPath_1-6", True)
                                currentLayer = doc.Layers.FindName("SPD_hourPath_1-6")
                            Else
                                'Index = doc.Layers.Find("SPD_hourPath_7-12", True)
                                currentLayer = doc.Layers.FindName("SPD_hourPath_7-12")
                            End If

                            'doc.Layers.SetCurrentLayerIndex(index, True)
                            doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                        End If

                        arrPts.Add(arrPt)
                        arrObj.Add(doc.Objects.AddPoint(arrPt))

                        'adding an hour
                        arrVec = vec
                        arrVec.Unitize()
                        arrVec *= (-3)
                        arrVec += New Rhino.Geometry.Vector3d(-2, 0, 0)
                        arrVec += vec

                        If SPD_showAngle Then
                            vec_sun = GetSunVector(arrSunValues(0), arrSunValues(1))
                            angle = GetVecAngle(vec_sun)
                            t = i & ":00" & ", " & CStr(Math.Round(angle, 1)) & "°"
                        Else
                            t = i & ":00"
                        End If

                        plane = source_plane
                        plane.Origin = New Rhino.Geometry.Point3d(arrVec)
                        arrObj.Add(doc.Objects.AddText(t, plane, height, font, False, False))
                        If Is3D Then
                            'index = doc.Layers.Find("SPD_sunDirection", True)
                            'doc.Layers.SetCurrentLayerIndex(index, True)
                            currentLayer = doc.Layers.FindName("SPD_sunDirection")
                            doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                            'sun Sphere
                            plane.Origin = New Rhino.Geometry.Point3d(vec)
                            sphere = New Rhino.Geometry.Sphere(plane, 1.5)
                            Dim arrow As New Rhino.Geometry.Line(arrPt, New Rhino.Geometry.Point3d(0, 0, 0))
                            Dim attribs = doc.CreateDefaultAttributes()
                            attribs.ObjectDecoration = Rhino.DocObjects.ObjectDecoration.None
                            attribs.ObjectColor = Drawing.Color.Aqua

                            'Index = doc.Layers.Find("SPD_sunDirection", True)
                            'doc.Layers.SetCurrentLayerIndex(Index, True)

                            currentLayer = doc.Layers.FindName("SPD_sunDirection")
                            doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                            arrObj.Add(doc.Objects.AddSphere(sphere))
                            arrObj.Add(doc.Objects.AddLine(arrow, attribs))

                        End If

                    Else
                        flag = 1
                    End If
                End If
            Next
        Else
            flag = 0
            tHours = hour
            tMin = 0
            Dim arrPts As New Rhino.Collections.Point3dList
            Dim arrPtsB, arrPtsR As New Rhino.Collections.Point3dList

            If setDate = False Then
                For j = 1 To 365 Step 3
                    arrSunValues = SunAngle.Calculate(j, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
                    If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel

                    If tHours >= arrSunValues(2) And tHours <= arrSunValues(3) Then
                        If Is3D Then
                            tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                        Else
                            tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                        End If
                        vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
                        vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))
                        vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
                        Dim arrPt As New Rhino.Geometry.Point3d(vec)
                        arrPts.Add(arrPt)

                        If j <= 172 Then
                            arrPtsR.Add(arrPt)
                        End If

                        If j = 355 Then
                            arrPtsR.Insert(0, arrPt)
                        End If

                        If j >= 172 And j <= 355 Then
                            arrPtsB.Add(arrPt)
                        End If

                        If j = 184 Then
                            arrVec = vec
                            arrVec.Unitize()
                            arrVec *= (-3)
                            arrVec += New Rhino.Geometry.Vector3d(-2, 0, 0)
                            arrVec += vec

                            t = tHours & ":00"
                            plane = source_plane
                            plane.Origin = New Rhino.Geometry.Point3d(arrVec)
                            arrObj.Add(doc.Objects.AddText(t, plane, height, font, False, False))
                        End If
                    Else
                        flag = 1
                    End If
                Next

                If arrPts.Count > 0 And flag = 0 Then
                    'set current layer
                    'index = doc.Layers.Find("SPD_hourPath_1-6", True)
                    'doc.Layers.SetCurrentLayerIndex(Index, True)
                    currentLayer = doc.Layers.FindName("SPD_hourPath_1-6")
                    doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                    'add Crv
                    crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPtsR, 3)
                    arrObj.Add(doc.Objects.AddCurve(crv))

                    'set current layer
                    'index = doc.Layers.Find("SPD_hourPath_7-12", True)
                    'doc.Layers.SetCurrentLayerIndex(Index, True)
                    currentLayer = doc.Layers.FindName("SPD_hourPath_7-12")
                    doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                    'add Crv
                    crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPtsB, 3)
                    arrObj.Add(doc.Objects.AddCurve(crv))

                ElseIf arrPts.Count > 0 And flag = 1 Then
                    'set current layer
                    'index = doc.Layers.Find("SPD_hourPath_1-6", True)
                    'doc.Layers.SetCurrentLayerIndex(Index, True)
                    currentLayer = doc.Layers.FindName("SPD_hourPath_1-6")
                    doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                    'add Crv
                    crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPtsR, 3)
                    arrObj.Add(doc.Objects.AddCurve(crv))

                    'set current layer
                    'index = doc.Layers.Find("SPD_hourPath_7-12", True)
                    'doc.Layers.SetCurrentLayerIndex(Index, True)
                    currentLayer = doc.Layers.FindName("SPD_hourPath_7-12")
                    doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                    'add Crv
                    crv = Rhino.Geometry.Curve.CreateInterpolatedCurve(arrPtsB, 3)
                    arrObj.Add(doc.Objects.AddCurve(crv))

                End If

            Else

                j = 0
                tMonth = month
                tDay = day
                tHours = hour
                arrSunValues = SunAngle.Calculate(j, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)
                If arrSunValues Is Nothing Then Return Rhino.Commands.Result.Cancel

                If tHours >= arrSunValues(2) And tHours <= arrSunValues(3) Then
                    If Is3D Then
                        tRad = (fRad) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                    Else
                        tRad = (fRad * Math.Cos(Rhino.RhinoMath.ToRadians(arrSunValues(0)))) / (1 + Math.Sin(Rhino.RhinoMath.ToRadians(arrSunValues(0))))
                    End If

                    vec = New Rhino.Geometry.Vector3d(0, tRad, 0)
                    If Is3D Then
                        vec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))
                    End If
                    vec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
                    Dim arrPt As New Rhino.Geometry.Point3d(vec)
                    arrPts.Add(arrPt)
                    If Is3D Then
                        If tMonth <= 6 Then
                            'index = doc.Layers.Find("SPD_hourPath_1-6", True)
                            currentLayer = doc.Layers.FindName("SPD_hourPath_1-6")
                        Else
                            'Index = doc.Layers.Find("SPD_hourPath_1-6", True)
                            currentLayer = doc.Layers.FindName("SPD_hourPath_1-6")
                        End If

                        'doc.Layers.SetCurrentLayerIndex(index, True)
                        doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                    End If

                    arrObj.Add(doc.Objects.AddPoint(arrPt))

                    'adding an hour
                    arrVec = vec
                    arrVec.Unitize()
                    arrVec *= -3
                    arrVec += New Rhino.Geometry.Vector3d(-2, 0, 0)
                    arrVec += vec


                    If SPD_showAngle Then
                        vec_sun = GetSunVector(arrSunValues(0), arrSunValues(1))
                        angle = GetVecAngle(vec_sun)
                        t = tHours & ":00" & ", " & CStr(Math.Round(angle, 1)) & "°"
                    Else
                        t = tHours & ":00"
                    End If

                    plane = source_plane
                    plane.Origin = New Rhino.Geometry.Point3d(arrVec)
                    arrObj.Add(doc.Objects.AddText(t, plane, height, font, False, False))

                    If Is3D Then
                        'sun Sphere
                        plane.Origin = New Rhino.Geometry.Point3d(vec)
                        sphere = New Rhino.Geometry.Sphere(plane, 1.5)
                        Dim arrow As New Rhino.Geometry.Line(arrPt, New Rhino.Geometry.Point3d(0, 0, 0))

                        'index = doc.Layers.Find("SPD_sunDirection", True)
                        'doc.Layers.SetCurrentLayerIndex(index, True)
                        currentLayer = doc.Layers.FindName("SPD_sunDirection")
                        doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                        Dim attribs = doc.CreateDefaultAttributes()
                        attribs.ObjectDecoration = Rhino.DocObjects.ObjectDecoration.None
                        attribs.ObjectColor = Drawing.Color.Plum

                        arrObj.Add(doc.Objects.AddSphere(sphere))
                        arrObj.Add(doc.Objects.AddLine(arrow, attribs))
                    End If

                Else
                    flag = 1
                End If
            End If
        End If

        For i As Integer = 0 To arrObj.Count - 1
            ids.Add(arrObj(i))
        Next

        Return Rhino.Commands.Result.Success

    End Function

    'add SunDiagram Layers
    Public Shared Function AddLayers_SDP(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result
        Dim layer_name As String = "SPD"
        AddLayer(doc, layer_name)

        'child layers:
        AddChildLayer(doc, layer_name, "template", "gray") 'template layer
        'AddChildLayer(doc, layer_name, "text", "gray") 'template description text layer
        AddChildLayer(doc, layer_name, "monthPath_1-6", "red") 'month path 1-6 layer
        AddChildLayer(doc, layer_name, "monthPath_7-12", "blue") 'month path 7-12 layer
        AddChildLayer(doc, layer_name, "hourPath_1-6", "red") 'hour path 1-6 layer
        AddChildLayer(doc, layer_name, "hourPath_7-12", "blue") 'hour path 7-12 layer
        AddChildLayer(doc, layer_name, "sunDirection", "yellow") 'sun sphere and sun direction

        Return Rhino.Commands.Result.Success
    End Function

End Class

