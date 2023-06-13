Imports Rhino

Public Class SR
    Public Shared ids As New List(Of Guid)()

    'main - create List of objects, layers, calling the DrawRaysToPoint Function
    Public Shared Function Draw(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result

        'clean list of objects
        ids = New List(Of Guid)()
        'get array of values
        Dim arr() As Double
        arr = GetData(lon, lat, TZone, nOffset)
        'add layer
        SR.AddLayers_SR(doc)
        'draw rays
        SR.DrawRaysFromToPoint(doc, arr)
        Return Rhino.Commands.Result.Success

    End Function

    'draws rays from points according to the given parameters
    Private Shared Function DrawRaysFromToPoint(ByVal doc As Rhino.RhinoDoc, ByVal arrVal() As Double) As Rhino.Commands.Result

        Dim i, j As Integer
        Dim tMonth As Double = arrVal(0)
        Dim tDay As Double = arrVal(1)
        Dim tHours As Double = arrVal(2)
        Dim tMin As Double = arrVal(3)
        Dim lat As Double = arrVal(4)
        Dim lon As Double = arrVal(5)
        Dim tZone As Double = arrVal(6)
        Dim fRad As Double = arrVal(7)
        Dim fOff As Double = arrVal(8)

        Dim refPt As New Rhino.Geometry.Point3d(0, 0, 0)

        If SR_pickPt Then
            Dim gp As New Rhino.Input.Custom.GetPoint()
            gp.SetCommandPrompt("Pick point")
            gp.[Get]()

            If gp.CommandResult() <> Rhino.Commands.Result.Success Then
                Return gp.CommandResult()
            End If
            refPt = gp.Point
        End If

        doc.Views.RedrawEnabled = False

        Dim dir As Integer

        If SR_ShadowRays Then
            dir = -1
        Else
            dir = 1
        End If

        If setDate = False Then
            For i = 1 To 365 Step 3
                If setHour = False Then
                    'setDate = False 
                    'setHour = False
                    For j = 0 To 23
                        SR.DrawRay(doc, i, tMonth, i, j, tMin, tZone, lat, lon, fOff, dir)
                    Next
                Else
                    'setDate = False 
                    'setHour = True
                    SR.DrawRay(doc, i, tMonth, i, hour, tMin, tZone, lat, lon, fOff, dir)
                End If
            Next
        Else
            If setHour = False Then
                'setDate = True 
                'setHour = False
                For j = 0 To 23
                    SR.DrawRay(doc, 0, month, day, j, tMin, tZone, lat, lon, fOff, dir)
                Next
            Else
                'setDate = True 
                'setHour = True
                SR.DrawRay(doc, 0, month, day, hour, tMin, tZone, lat, lon, fOff, dir)
            End If
        End If


        If SR_pickPt Then
            Dim xform As Rhino.Geometry.Transform = Rhino.Geometry.Transform.Translation(New Rhino.Geometry.Vector3d(refPt))
            For i = 0 To ids.Count - 1
                doc.Objects.Transform(ids(i), xform, True)
            Next
        End If

        Dim ind As Integer = doc.Groups.Add(ids)

        doc.Views.RedrawEnabled = True
        doc.Views.Redraw()
        Return Rhino.Commands.Result.Success
    End Function

    'draws single Ray on the given data
    Public Shared Function DrawRay(ByVal doc As Rhino.RhinoDoc, ByVal iJulianDate As Double, ByVal iTimeMonth As Double, ByVal iTimeDay As Double, ByVal iTimeHours As Double, ByVal iTimeMinutes As Double,
     ByVal FTimeZone As Double, ByVal fLatitude As Double, ByVal fLongitude As Double, ByVal fOffset As Double, ByVal dir As Integer) As Rhino.Commands.Result


        Dim j, k, flag As Integer
        Dim arrMonth() As String = {"", "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec"}
        Dim arrIntMonth() As Integer = {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334}
        Dim tMin, arrSunValues(3) As Double
        Dim vec As Rhino.Geometry.Vector3d
        Dim sunPt As Rhino.Geometry.Point3d
        Dim ax As New Rhino.Geometry.Vector3d(0, 0, 1)
        Dim ray As Rhino.Geometry.Line
        Dim name As String
        'Dim index As Integer
        Dim currentLayer As Rhino.DocObjects.Layer
        tMin = 0

        arrSunValues = SunAngle.Calculate(iJulianDate, iTimeMonth, iTimeDay, iTimeHours, tMin, FTimeZone, fLatitude, fLongitude, fOffset)
        If arrSunValues Is Nothing Then
            ' Rhino.RhinoApp.WriteLine(CStr(iTimeMonth) & " _ " & CStr(iTimeDay) & " _ " & CStr(iTimeHours) & " _ " & CStr(tMin) & " _ " & CStr(TZone) & " _ " & CStr(fLatitude) & " _ " & CStr(fLongitude) & " _ " & CStr(fOffset))
            Return Rhino.Commands.Result.Cancel
        End If

        flag = 0

        If iTimeHours >= arrSunValues(2) And iTimeHours <= arrSunValues(3) Then

            vec = GetSunVector(arrSunValues(0), arrSunValues(1))
            vec *= 5
            vec *= dir
            sunPt = New Rhino.Geometry.Point3d(vec)

            ray = New Rhino.Geometry.Line(New Rhino.Geometry.Point3d(0, 0, 0), sunPt)
            For k = UBound(arrIntMonth) To 0 Step -1
                If iTimeMonth > k And flag = 0 Then
                    name = iTimeDay & ". " & arrMonth(k + 1) & ", " & j & ":00, Al:" & Math.Round(arrSunValues(0) * 10) / 10 & ", Az:" & Math.Round(arrSunValues(1) * 10) / 10
                    ' I don't know how to change the name of the object
                    If iTimeDay < 183 Then
                        'index = doc.Layers.Find("SR_rays_1-6", True)
                        'doc.Layers.SetCurrentLayerIndex(index, True)
                        currentLayer = doc.Layers.FindName("SR_rays_1-6")
                        doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                    Else
                        'index = doc.Layers.Find("SR_rays_7-12", True)
                        'doc.Layers.SetCurrentLayerIndex(index, True)
                        currentLayer = doc.Layers.FindName("SR_rays_7-12")
                        doc.Layers.SetCurrentLayerIndex(currentLayer.Index, True)

                    End If
                    ids.Add(doc.Objects.AddLine(ray))
                    ids.Add(doc.Objects.AddPoint(sunPt))
                    flag = 1
                End If
            Next
        End If

        Return Rhino.Commands.Result.Success

    End Function

    'add SunDiagram Layers
    Public Shared Function AddLayers_SR(ByVal doc As Rhino.RhinoDoc) As Rhino.Commands.Result
        Dim layer_name As String = "SR"
        AddLayer(doc, layer_name)

        'child layers:
        AddChildLayer(doc, layer_name, "rays_1-6", "red") 'rays 1-6 layer
        AddChildLayer(doc, layer_name, "rays_7-12", "blue") 'rays 7-12 layer

        Return Rhino.Commands.Result.Success
    End Function


End Class
