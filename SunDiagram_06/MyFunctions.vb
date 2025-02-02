﻿Imports System.Drawing

Module MyFunction
    'Enum SEA_GOptionValues
    '    Threshold
    '    Gradient
    '    Precised
    'End Enum

    'Enum SEA_AnTypeValues
    '    SunExposure
    '    ShadowCast
    'End Enum


    'Public variables:
    Public lat As Double
    Public lon As Double
    Public TZone As Double
    Public nOffset As Double
    Public setDate As Boolean
    Public setHour As Boolean
    Public day As Double
    Public month As Double
    Public hour As Double
    Public SPD_is3D As Boolean = False
    Public SPD_pickBld As Boolean = False
    Public SPD_showAngle As Boolean = False
    Public SPD_Shadow As Boolean = False
    Public SR_pickPt As Boolean = False
    Public SR_ShadowRays As Boolean = False
    Public SEA_colorNeg As Drawing.Color = Drawing.Color.Red
    Public SEA_colorPos As Drawing.Color = Drawing.Color.White
    Public SEA_CastObj As New List(Of Guid)()
    Public SEA_CastObjMesh As New List(Of Rhino.Geometry.Mesh)()
    Public SEA_RecObj As New List(Of Guid)()
    Public SEA_StartH As Double
    Public SEA_EndH As Double
    Public SEA_StartHVal As Double
    Public Sea_EndHVal As Double
    Public SEA_StartMinVal, SEA_EndMinVal As Double
    Public SEA_MinAngle, SEA_MaxAngle As Double
    Public SEA_MinAngleRad As Double
    Public SEA_MaxAngleRad As Double

    Public SEA_Threshold As Double
    Public SEA_TimeUnits As Integer
    Public SEA_TimeSeg As Double

    Public SEA_AnType As SEA_AnTypeValues

    Public SEA_GOption As SEA_GOptionValues

    Public text_height As Double = 0.8
    Public text_font As String = "Verdana"
    Public text_plane As New Rhino.Geometry.Plane

    Public WriteFile As Boolean
    Public CreateLegend As Boolean
    Public CreateDots As Boolean

    ''' <summary>
    ''' create array with default values for the sun calculations
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <param name="lon"></param>
    ''' <param name="lat"></param>
    ''' <param name="tZone"></param>
    ''' <param name="nOffset"></param>
    ''' <returns></returns>
    Public Function GetData(ByVal lon As Double, ByVal lat As Double, ByVal tZone As Double, ByVal nOffset As Double) As Double()

        'Dim arrVal As New ArrayList
        'With arrVal
        '    .Add(11)            'iTimemonth
        '    .Add(1)             'iTimeDay
        '    .Add(13)            'iTimeHours
        '    .Add(0)             'iTimeMinutes
        '    .Add(30.4)          'fLatitude
        '    .Add(-3.6)          'fLongitude
        '    .Add(1.0)           'fTimeZone
        '    .Add(84)            'fRadius
        '    .Add(28.7752)       'fOffset
        'End With

        Dim arr(8) As Double
        arr(0) = 11
        arr(1) = 1
        arr(2) = 13
        arr(3) = 0
        arr(4) = lat
        arr(5) = lon
        arr(6) = tZone
        arr(7) = 84
        arr(8) = nOffset

        Return arr
    End Function

    ''' <summary>
    ''' returns unitized sun vector
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <param name="fAltitude"></param>
    ''' <param name="fAzimuth"></param>
    ''' <returns></returns>
    Public Function GetSunVector(ByVal fAltitude As Double, ByVal fAzimuth As Double) As Rhino.Geometry.Vector3d

        Dim vec As Rhino.Geometry.Vector3d
        Dim ax As New Rhino.Geometry.Vector3d(0, 0, 1)

        vec = New Rhino.Geometry.Vector3d(0, 1, 0)
        vec.Rotate(Rhino.RhinoMath.ToRadians(fAltitude), New Rhino.Geometry.Vector3d(1, 0, 0))
        vec.Rotate(Rhino.RhinoMath.ToRadians(-fAzimuth), ax)
        Dim sunPt = New Rhino.Geometry.Point3d(vec)

        Return vec
    End Function

    ''' <summary>
    ''' add layer
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <param name="layer_name"></param>
    ''' <returns></returns>
    Public Function AddLayer(ByVal doc As Rhino.RhinoDoc, ByVal layer_name As String) As Rhino.Commands.Result

        ' Does a layer with the same name already exist?
        'Dim layer_index As Integer = doc.Layers.Find(layer_name, True)

        Dim currentLayer As Rhino.DocObjects.Layer = doc.Layers.FindName("layer_name")

        'If layer_index >= 0 Then
        If currentLayer Is Nothing Then
            'Rhino.RhinoApp.WriteLine("A layer with the name {0} already exists.", layer_name)
            Return Rhino.Commands.Result.Cancel
        End If

        ' Add a new layer to the document
        Dim layer_index As Integer = doc.Layers.Add(layer_name, System.Drawing.Color.Black)
        If layer_index < 0 Then
            'Rhino.RhinoApp.WriteLine("Unable to add {0} layer.", layer_name)
            Return Rhino.Commands.Result.Failure
        End If
        Return Rhino.Commands.Result.Success
    End Function

    ''' <summary>
    ''' add child layers
    ''' </summary>
    ''' <param name="doc"></param>
    ''' <param name="parent_name"></param>
    ''' <param name="child_name"></param>
    ''' <param name="color"></param>
    ''' <returns></returns>
    Public Function AddChildLayer(ByVal doc As Rhino.RhinoDoc, ByVal parent_name As String, ByVal child_name As String, ByVal color As String) As Rhino.Commands.Result

        ' Was a layer named entered?
        'Dim index As Integer = doc.Layers.Find(parent_name, True)

        'If index < 0 Then
        '    Return Rhino.Commands.Result.Cancel
        'End If

        Dim parent_layer As Rhino.DocObjects.Layer = doc.Layers.FindName(parent_name)

        'If layer_index >= 0 Then
        If parent_layer IsNot Nothing Then
            Return Rhino.Commands.Result.Cancel
        End If


        'Dim parent_layer As Rhino.DocObjects.Layer = doc.Layers(index)

        ' Create a child layer
        Dim name As String = parent_name + "_" + child_name
        Dim childlayer As New Rhino.DocObjects.Layer With {
            .ParentLayerId = parent_layer.Id,
            .Name = name
        }

        If color = "red" Then
            childlayer.Color = System.Drawing.Color.Red
        ElseIf color = "blue" Then
            childlayer.Color = System.Drawing.Color.Blue
        ElseIf color = "green" Then
            childlayer.Color = System.Drawing.Color.Green
        ElseIf color = "gray" Then
            childlayer.Color = System.Drawing.Color.Gray
        ElseIf color = "yellow" Then
            childlayer.Color = System.Drawing.Color.Gold
        Else
            childlayer.Color = System.Drawing.Color.Black
        End If

        Dim index = doc.Layers.Add(childlayer)
        If index < 0 Then
            'Rhino.RhinoApp.WriteLine("Unable to add {0} layer.", name)
            Return Rhino.Commands.Result.Failure
        End If

        Return Rhino.Commands.Result.Success

    End Function

    Public Function Map(ByVal a As Double, ByVal min As Double, ByVal max As Double,
                        ByVal refMin As Byte, ByVal refMax As Byte) As Integer

        Dim res As Double

        'res = refMin + (refMax - refMin) / ((min + max) / a)

        res = (a - min) * (refMax - refMin) / (max - min) + refMin
        'Rhino.RhinoApp.WriteLine(CStr(a) & "_" & CStr(min) & "_" & CStr(max) & "_" & CStr(refMin) & "_" & CStr(refMax))

        Return CType(res, Integer)

    End Function
    ''' <summary>
    ''' returns a list of 9 colors - Indigo, Blue, DeepSkyBlue, Teal, LawnGreen, Yellow, Orange, OrangeRed, Red
    ''' </summary>
    ''' <returns></returns>
    Public Function GetColorSteps() As List(Of System.Drawing.Color)
        Dim arr_col As New List(Of System.Drawing.Color)
        Dim col_Temp As System.Drawing.Color

        '0h
        col_Temp = Drawing.Color.Indigo
        arr_col.Add(col_Temp)

        '1h
        col_Temp = Drawing.Color.Blue
        arr_col.Add(col_Temp)

        '2h
        col_Temp = Drawing.Color.DeepSkyBlue
        arr_col.Add(col_Temp)

        '3h
        col_Temp = Drawing.Color.Teal
        arr_col.Add(col_Temp)

        '4h
        col_Temp = Drawing.Color.LawnGreen
        arr_col.Add(col_Temp)

        '5h
        col_Temp = Drawing.Color.Yellow
        arr_col.Add(col_Temp)

        '6h
        col_Temp = Drawing.Color.Orange
        arr_col.Add(col_Temp)

        '7h
        col_Temp = Drawing.Color.OrangeRed
        arr_col.Add(col_Temp)

        '8h +
        col_Temp = Drawing.Color.Red
        arr_col.Add(col_Temp)

        Return arr_col
    End Function


    'Add Text in WorldXY plane with origin in given point
    Public Function AddText(ByVal doc As Rhino.RhinoDoc, ByVal pt_O As Rhino.Geometry.Point3d, ByVal t As String, ByVal h_t As Double) As Guid

        Dim plane_t As Rhino.Geometry.Plane = Rhino.Geometry.Plane.WorldXY
        Dim height As Double = h_t
        Dim font As String = text_font

        plane_t.Origin = pt_O

        Dim myT As Guid = doc.Objects.AddText(t, plane_t, height, font, False, False)

        Return myT
    End Function

    'get bounding box from the array of mesh ids
    Public Function GetBBox(doc As Rhino.RhinoDoc, ByVal ids_Mesh As List(Of Guid)) As Rhino.Geometry.BoundingBox

        Dim objRef As Rhino.DocObjects.ObjRef
        Dim arr_pts_t As New List(Of Rhino.Geometry.Point3d)
        Dim bbox As New Rhino.Geometry.BoundingBox

        For Each obj In ids_Mesh
            Dim pts() As Rhino.Geometry.Point3d
            'objRef = New Rhino.DocObjects.ObjRef(obj)
            objRef = New Rhino.DocObjects.ObjRef(doc, obj)
            If Not IsNothing(objRef.Mesh) Then
                pts = objRef.Mesh.GetBoundingBox(True).GetCorners

                If Not IsNothing(pts) Then
                    For Each p In pts
                        arr_pts_t.Add(p)
                    Next
                End If

            End If

        Next

        If Not IsNothing(arr_pts_t) Then
            bbox = New Rhino.Geometry.BoundingBox(arr_pts_t)
        End If

        Return bbox
    End Function


    'get sunray angle
    Public Function GetVecAngle(ByVal vec As Rhino.Geometry.Vector3d) As Double
        Dim z, len, angle As Double


        z = vec.Z
        len = vec.Length

        If len = 0 Then Return 0

        angle = Math.Asin(z / len)

        angle = Rhino.RhinoMath.ToDegrees(angle)

        Return angle

    End Function


End Module
