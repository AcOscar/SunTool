Public Class DropShadow

    'draws shadow
    Public Shared Function Draw(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result

        'add Main layer
        AddLayer(doc, "Shadow")

        'pick object
        ' Select objects to orient
        Dim go As New Rhino.Input.Custom.GetObject()
        go.SetCommandPrompt("pick shadow casting object")
        go.GeometryAttributeFilter = Rhino.Input.Custom.GeometryAttributeFilter.ClosedMesh Or Rhino.Input.Custom.GeometryAttributeFilter.OpenMesh
        go.SubObjectSelect = False
        go.GroupSelect = True
        go.GetMultiple(1, 0)
        If go.CommandResult() <> Rhino.Commands.Result.Success Then
            Return Rhino.Commands.Result.Failure
            'Exit Function
        End If

        Dim ids As New List(Of Guid)()
        For i As Integer = 0 To go.ObjectCount - 1
            'Dim objref As Rhino.DocObjects.ObjRef = New Rhino.DocObjects.ObjRef(go.[Object](i).ObjectId)
            Dim objref As New Rhino.DocObjects.ObjRef(doc, go.[Object](i).ObjectId)
            If Not IsNothing(objref.Mesh) Then ids.Add(objref.ObjectId)
        Next

        'draw shadow
        If setDate Then
            If setHour Then
                'setHour = True
                'setDate = True
                Try
                    DropShadow.SunStudy(doc, ids, hour)
                Catch ex As FormatException
                    Rhino.RhinoApp.WriteLine("Problem with units")
                End Try

            Else

                'setHour = False
                'setDate = True
                For i = 0 To 23
                    Try
                        DropShadow.SunStudy(doc, ids, i)
                    Catch ex As FormatException
                        Rhino.RhinoApp.WriteLine("Problem with units")
                    End Try
                Next

            End If
        Else
            Rhino.RhinoApp.WriteLine("I'm not going to draw a shadow for the whole year! Select day first")

        End If


        Return Rhino.Commands.Result.Success
    End Function

    Public Shared Function SunStudy(ByVal doc As Rhino.RhinoDoc, ByVal ids As List(Of Guid), ByVal hour As Double) As Rhino.Commands.Result

        Dim plane As Rhino.Geometry.Plane

        plane = New Rhino.Geometry.Plane(New Rhino.Geometry.Point3d(0, 0, 0), New Rhino.Geometry.Vector3d(0, 0, 1))

        Dim planeEq() As Double
        planeEq = plane.GetPlaneEquation()

        Dim arr() As Double
        arr = GetData(lon, lat, TZone, nOffset)

        'Rhino.RhinoApp.WriteLine(CStr(planeEq(0)) & "_" & CStr(planeEq(1)) & "_" & CStr(planeEq(2)) & "_" & CStr(planeEq(3)) & "_id nbr" & CStr(ids.Count) & "_" & CStr(month) & CStr(hour))

        DropShadow.CalcShadows(doc, ids, planeEq, arr, hour)

        Return Rhino.Commands.Result.Success

    End Function


    Public Shared Function CalcShadows(ByVal doc As Rhino.RhinoDoc, ByVal ids As List(Of Guid), ByVal plane() As Double, ByVal arrVal() As Double, ByVal hour As Double) As Rhino.Commands.Result

        Dim tMonth As Double = month
        Dim tDay As Double = day
        Dim tHours As Double = hour
        Dim tMin As Double = arrVal(3)
        Dim lat As Double = arrVal(4)
        Dim lon As Double = arrVal(5)
        Dim tZone As Double = arrVal(6)
        Dim fRad As Double = arrVal(7)
        Dim fOff As Double = arrVal(8)

        Dim sunVec As Rhino.Geometry.Vector3d
        'Dim index As Integer
        Dim CurrentLayer As Rhino.DocObjects.Layer
        Dim matrix As Rhino.Geometry.Transform
        Dim ObjXform As Guid
        Dim objref As Rhino.DocObjects.ObjRef
        Dim arrSunValues() As Double
        Dim ax As New Rhino.Geometry.Vector3d(0, 0, 1)

        arrSunValues = SunAngle.Calculate(0, tMonth, tDay, tHours, tMin, tZone, lat, lon, fOff)

        'proceed only if hour is within sunRise and sunSet
        If tHours >= arrSunValues(2) And tHours <= arrSunValues(3) Then
            sunVec = New Rhino.Geometry.Vector3d(0, 1, 0)
            sunVec.Rotate(Rhino.RhinoMath.ToRadians(arrSunValues(0)), New Rhino.Geometry.Vector3d(1, 0, 0))
            sunVec.Rotate(Rhino.RhinoMath.ToRadians(-arrSunValues(1)), ax)
            sunVec.Reverse()

            matrix = Projectionmatrix(sunVec, plane)

            doc.Views.RedrawEnabled = False

            'add child layer
            Dim layName As String = month & "." & day & "_" & hour & ": 00"
            AddChildLayer(doc, "Shadow", layName, "black")
            'index = doc.Layers.Find("Shadow_" & layName, True)
            'doc.Layers.SetCurrentLayerIndex(index, True)
            CurrentLayer = doc.Layers.FindName("Shadow_" & layName)
            doc.Layers.SetCurrentLayerIndex(CurrentLayer.Index, True)

            For i As Integer = 0 To ids.Count - 1
                ObjXform = doc.Objects.Transform(ids(i), matrix, False)
                'objref = New Rhino.DocObjects.ObjRef(ObjXform)
                objref = New Rhino.DocObjects.ObjRef(doc, ObjXform)
                Dim outline() As Rhino.Geometry.Polyline
                Dim pts As New Rhino.Collections.Point3dList
                outline = objref.Mesh.GetOutlines(Rhino.Geometry.Plane.WorldXY)

                If Not IsNothing(outline) Then

                    For j As Integer = 0 To outline.Length - 1
                        doc.Objects.AddPolyline(outline(j))
                    Next
                End If

                doc.Objects.Delete(ObjXform, True)
            Next

            doc.Views.RedrawEnabled = True
            doc.Views.Redraw()
        End If
        Return Rhino.Commands.Result.Success

    End Function

    Public Shared Function Projectionmatrix(ByVal lightvect As Rhino.Geometry.Vector3d, ByVal ground() As Double) As Rhino.Geometry.Transform

        Dim k As Double

        Dim shadowMat As New Rhino.Geometry.Transform

        'shadowMat = Rhino.XformZero

        k = -1 / (ground(0) * lightvect(0) + ground(1) * lightvect(1) + ground(2) * lightvect(2))

        shadowMat(0, 0) = 1 + k * lightvect(0) * ground(0)
        shadowMat(0, 1) = k * lightvect(0) * ground(1)
        shadowMat(0, 2) = k * lightvect(0) * ground(2)
        shadowMat(0, 3) = k * lightvect(0) * ground(3)

        shadowMat(1, 0) = k * lightvect(1) * ground(0)
        shadowMat(1, 1) = 1 + k * lightvect(1) * ground(1)
        shadowMat(1, 2) = k * lightvect(1) * ground(2)
        shadowMat(1, 3) = k * lightvect(1) * ground(3)

        shadowMat(2, 0) = k * lightvect(2) * ground(0)
        shadowMat(2, 1) = k * lightvect(2) * ground(1)
        shadowMat(2, 2) = 1 + k * lightvect(2) * ground(2)
        shadowMat(2, 3) = k * lightvect(1) * ground(3)

        shadowMat(3, 0) = 0
        shadowMat(3, 1) = 0
        shadowMat(3, 2) = 0
        shadowMat(3, 3) = 1

        Projectionmatrix = shadowMat

    End Function


End Class
