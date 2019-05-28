'User Interface
<System.Runtime.InteropServices.Guid("FA12592B-3D61-4CE9-ADAE-8167D5774D76")>
Public Class Form1

    Public Sub New()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        AEA_NegativeButton.BackColor = SEA_colorNeg
        AEA_PositiveButton.BackColor = SEA_colorPos
        lat = Double.Parse(LatitudeTB.Text)
        lon = Double.Parse(LongitudeTB.Text)
        TZone = Double.Parse(TimeZoneTB.Text)
        nOffset = Double.Parse(NorthOffTB.Text)
        Dim arr() As Double
        arr = getData()
        month = arr(1)
        day = arr(0)
        hour = HourTB.Value
        SPD_is3D = SunPath3D.Checked
        SPD_pickBld = SunPathPickObj.Checked
        'SPD_Shadow = SunPathShadows.Checked
        SR_pickPt = SunRaysPickPoint.Checked
        SR_ShadowRays = ShadowRaysCheckBox1.Checked
        SEA_StartH = SEA_TimeStart.Value
        SEA_EndH = SEA_TimeEnd.Value
        SEA_Threshold = TimeExposureNumUD.Value
        SEA_TimeUnits = 1
        SEA_AnType = 0 'sunExposure

        'change set locale value
        Dim ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-gb")
        Dim instance As New System.Data.DataSet
        instance.Locale = ci
        'Rhino.RhinoApp.WriteLine(instance.Locale.DisplayName)

        'Dim myTS As New ArrayList()
        'myTS.Add("n.o.")
        'myTS.Add("1")
        'myTS.Add("2")
        'SEA_TimeSegCB.DisplayMember = "LongName"
        'SEA_TimeSegCB.ValueMember = "ShortName"
        'SEA_TimeSegCB.DataSource = myTS

        SEA_TimeSeg = 100.0
    End Sub

    Public Shared Function BarID() As Guid
        'Dim g As New Guid("{6B7943D9-5D57-4981-A521-C0A621FDB846}")
        'new GUID h.rasch 25.01.2018 for RhinoCommon integration
        Dim g As New Guid("{B3DE274E-BC89-415D-A846-7B3DDDDCF705}")
        Return g
    End Function

    '###################################################################
    '###SET DATA
    '###################################################################
    'set Latitude
    Private Sub LatitudeTB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LatitudeTB.TextChanged
        'Dim text As String = LatitudeTB.Text
        'Dim isConv As Boolean = controlDouble(text)
        'If isConv Then
        '    lat = Double.Parse(text)
        'Else
        '    LatitudeTB.Text = "47.57"
        '    lat = 47.57
        'End If
        Try
            lat = Double.Parse(LatitudeTB.Text)

        Catch ex As FormatException
            LatitudeTB.Text = "47"
            lat = 47
        End Try

        'Rhino.RhinoApp.WriteLine("lat : " & CStr(lat))
    End Sub

    ''set Latitude
    'Private Sub LatitudeTB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LatitudeTB.TextChanged
    '    Dim arr1(), arr2() As String
    '    Dim val As Double

    '    Try
    '        val = Double.Parse(LatitudeTB.Text)

    '    Catch ex As FormatException
    '        LatitudeTB.Text = "47.57"
    '        lat = 47.57
    '    End Try

    '    'shitty debugging to make sure that data is read correctly
    '    arr1 = Split(LatitudeTB.Text, ".")
    '    arr2 = Split(LatitudeTB.Text, ",")

    '    ' Rhino.RhinoApp.WriteLine(CStr(arr1.Length) & " _ " & CStr(arr2.Length))

    '    '47
    '    If arr1.Length = 1 And arr2.Length = 1 Then
    '        Try
    '            lat = Double.Parse(LatitudeTB.Text)

    '        Catch ex As FormatException
    '            LatitudeTB.Text = "47.57"
    '            lat = 47.57
    '        End Try

    '        '47.57
    '    ElseIf arr1.Length = 2 And arr2.Length = 1 Then
    '        Try
    '            lat = Double.Parse(arr1(0) & "." & arr1(1))

    '        Catch ex As FormatException
    '            LatitudeTB.Text = "47.57"
    '            lat = 47.57
    '        End Try

    '        If CInt(lat) <> Double.Parse(arr1(0)) Then
    '            Try
    '                lat = Double.Parse(arr1(0) & "," & arr1(1))

    '            Catch ex As FormatException
    '                LatitudeTB.Text = "47.57"
    '                lat = 47.57
    '            End Try
    '        End If

    '        '47,57
    '    ElseIf arr2.Length = 1 Then
    '        Try
    '            lat = Double.Parse(arr2(0) & "." & arr2(1))
    '        Catch ex As FormatException
    '            LatitudeTB.Text = "47.57"
    '            lat = 47.57
    '        End Try

    '        If CInt(lat) <> Double.Parse(arr2(0)) Then
    '            Try
    '                lat = Double.Parse(arr2(0) & "," & arr2(1))

    '            Catch ex As FormatException
    '                LatitudeTB.Text = "47.57"
    '                lat = 47.57
    '            End Try
    '        End If
    '    Else
    '        LatitudeTB.Text = "47.57"
    '        lat = 47.57
    '    End If

    '    Rhino.RhinoApp.WriteLine("lat : " & CStr(lat))
    'End Sub


    'check if text is convertible to Double
    Private Function controlDouble(ByVal text As String) As Boolean
        Dim val As Double
        Dim result As Boolean = True
        Try
            val = Double.Parse(text)
        Catch ex As FormatException
            result = False
        End Try
        Return result
    End Function


    'Set Longitude
    Private Sub LongitudeTB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LongitudeTB.TextChanged
        Try
            lon = Double.Parse(LongitudeTB.Text)
        Catch ex As FormatException
            LongitudeTB.Text = "8"
            lon = 8
        End Try
    End Sub

    'Private Sub LongitudeTB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LatitudeTB.TextChanged
    '    Dim arr1(), arr2() As String
    '    Dim val As Double

    '    Try
    '        val = Double.Parse(LongitudeTB.Text)

    '    Catch ex As FormatException
    '        LongitudeTB.Text = "7.6"
    '        lon = 7.6
    '    End Try

    '    'shitty debugging to make sure that data is read correctly
    '    arr1 = Split(LongitudeTB.Text, ".")
    '    arr2 = Split(LongitudeTB.Text, ",")

    '    Rhino.RhinoApp.WriteLine(CStr(arr1.Length) & " _ " & CStr(arr2.Length))

    '    '47
    '    If arr1.Length = 1 And arr2.Length = 1 Then
    '        Try
    '            lon = Double.Parse(LongitudeTB.Text)

    '        Catch ex As FormatException
    '            LongitudeTB.Text = "7.6"
    '            lon = 7.6
    '        End Try

    '        '47.57
    '    ElseIf arr1.Length = 2 And arr2.Length = 1 Then
    '        Try
    '            lon = Double.Parse(arr1(0) & "." & arr1(1))

    '        Catch ex As FormatException
    '            LongitudeTB.Text = "7.6"
    '            lon = 7.6
    '        End Try

    '        If CInt(lon) <> Double.Parse(arr1(0)) Then
    '            Try
    '                lon = Double.Parse(arr1(0) & "," & arr1(1))

    '            Catch ex As FormatException
    '                LongitudeTB.Text = "7.6"
    '                lon = 7.6
    '            End Try
    '        End If

    '        '47,57
    '    ElseIf arr2.Length = 1 Then
    '        Try
    '            lon = Double.Parse(arr2(0) & "." & arr2(1))
    '        Catch ex As FormatException
    '            LongitudeTB.Text = "7.6"
    '            lon = 7.6
    '        End Try

    '        If CInt(lon) <> Double.Parse(arr2(0)) Then
    '            Try
    '                lon = Double.Parse(arr2(0) & "," & arr2(1))

    '            Catch ex As FormatException
    '                LongitudeTB.Text = "7.6"
    '                lon = 7.6
    '            End Try
    '        End If
    '    Else
    '        LongitudeTB.Text = "7.6"
    '        lon = 7.6
    '    End If

    '    Rhino.RhinoApp.WriteLine("lon : " & CStr(lon))
    'End Sub

    'Set Time Zone
    Private Sub TimeZoneTB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeZoneTB.TextChanged
        Try
            TZone = Double.Parse(TimeZoneTB.Text)
        Catch ex As FormatException
            TimeZoneTB.Text = "1"
            TZone = 1
        End Try

        'Rhino.RhinoApp.WriteLine(CStr(TZone))
    End Sub

    'Set North Offset
    Private Sub NorthOffTB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NorthOffTB.TextChanged
        Try
            nOffset = Double.Parse(NorthOffTB.Text)
        Catch ex As FormatException
            NorthOffTB.Text = "0"
            nOffset = 0
        End Try
    End Sub

    'setDate check bar
    Private Sub SetDateCB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetDateCB.CheckedChanged
        setDate = SetDateCB.Checked
        Dim arr() As Double
        arr = getData()
        month = arr(1)
        day = arr(0)

        If setDate Then
            'MonthCalendar1.TitleBackColor = Drawing.Color.DarkBlue
            MonthCalendar1.Enabled = True
            DropShadowBox.Enabled = True
            SunExpGB1.Enabled = True
            SunPathShowAng.Enabled = True
        Else
            'MonthCalendar1.TitleBackColor = Drawing.Color.LightGray
            MonthCalendar1.Enabled = False
            DropShadowBox.Enabled = False
            SunExpGB1.Enabled = False
            SunPathShowAng.Enabled = False
        End If

        'Rhino.RhinoApp.WriteLine(CStr(setDate))
    End Sub

    'setHour check bar
    Private Sub SetHourCB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetHourCB.CheckedChanged
        setHour = SetHourCB.Checked
        hour = HourTB.Value

        If setHour Then
            'HourTB.BackColor = Drawing.Color.White
            HourTB.Enabled = True
        Else
            'HourTB.BackColor = Drawing.Color.LightGray
            HourTB.Enabled = False
        End If

    End Sub

    'calendar
    Private Sub MonthCalendar1_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles MonthCalendar1.DateChanged
        If setDate Then
            MonthCalendar1.ImeMode = Windows.Forms.ImeMode.On
            Dim temp As String = CStr(MonthCalendar1.SelectionStart.Date)
            Dim arr() As Double
            arr = getData()
            month = arr(1)
            day = arr(0)
            'Rhino.RhinoApp.WriteLine(CStr(day) & " _ " & CStr(month))
        Else
            MonthCalendar1.ImeMode = Windows.Forms.ImeMode.Off
        End If
    End Sub

    'get data from the calendar
    Private Function getData() As Double()

        Dim temp As String = CStr(MonthCalendar1.SelectionStart.Date)
        Dim myDate As Date = MonthCalendar1.SelectionStart.Date
        'Dim germKb As Boolean = True
        'Rhino.RhinoApp.WriteLine(temp)
        'Dim arr() As String
        Dim data(1) As Double
        data(1) = myDate.Month
        data(0) = myDate.Day
        'arr = Split(temp, ".")

        'Try
        '    data(1) = CDbl(arr(0))
        'Catch ex As Exception
        '    arr = Split(temp, "/")
        '    germKb = False
        'End Try

        'If IsArray(arr) And germKb Then
        '    data(0) = CDbl(arr(0)) ' month
        '    data(1) = CDbl(arr(1)) ' day

        'ElseIf IsArray(arr) Then
        '    data(1) = CDbl(arr(0)) ' month
        '    data(0) = CDbl(arr(1)) ' day
        'Else
        '    Rhino.RhinoApp.WriteLine("Change setting of your keybord to English - date tool doesn't work")
        '    data(1) = 1
        '    data(0) = 1
        'End If
        'Rhino.RhinoApp.WriteLine("day : " & data(0) & " month : " & data(1))
        Return data
    End Function

    'set hour
    Private Sub HourTB_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HourTB.ValueChanged
        If setHour Then
            HourTB.ImeMode = Windows.Forms.ImeMode.On
            hour = HourTB.Value
            'Rhino.RhinoApp.WriteLine(CStr(hour))
        Else
            HourTB.ImeMode = Windows.Forms.ImeMode.Off
        End If
    End Sub

    '######################################################################
    '###SUN PATH DIAGRAM
    '######################################################################
    'set 3D
    Private Sub SunPath3D_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SunPath3D.CheckedChanged
        SPD_is3D = SunPath3D.Checked
    End Sub

    'pick object
    Private Sub SunPathPickObj_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SunPathPickObj.CheckedChanged
        SPD_pickBld = SunPathPickObj.Checked

        'If SPD_pickBld Then
        '    SunPathShadows.Enabled = True
        'Else
        '    SunPathShadows.Enabled = False
        'End If
    End Sub

    'show angle in the sun path
    Private Sub SunPathShowAng_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SunPathShowAng.CheckedChanged
        SPD_showAngle = SunPathShowAng.Checked
    End Sub

    'show shadow checkBox
    Private Sub SunPathShadows_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        'SPD_Shadow = SunPathShadows.Checked
    End Sub

    'display sun diagram
    Private Sub SunPathDiagram_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SunPathDiagram.Click
        SPD.Draw(Rhino.RhinoDoc.ActiveDoc)
        'If SPD_Shadow Then DropShadow.Draw(Rhino.RhinoDoc.ActiveDoc, lon, lat, TZone, nOffset, setDate, month, day, setHour, hour)
    End Sub

    '######################################################################
    '###SUN RAYS DIAGRAM
    '######################################################################
    'pick point
    Private Sub SunRaysPickPoint_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SunRaysPickPoint.CheckedChanged
        SR_pickPt = SunRaysPickPoint.Checked
    End Sub

    'show shadow rays ( not sun rays )
    Private Sub ShadowRaysCheckBox1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShadowRaysCheckBox1.CheckedChanged
        SR_ShadowRays = ShadowRaysCheckBox1.Checked
    End Sub

    'display sun rays
    Private Sub SunRays_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SunRays.Click
        SR.Draw(Rhino.RhinoDoc.ActiveDoc)
    End Sub


    '######################################################################
    '###SUN STUDY
    '######################################################################

    'sun study
    Private Sub SunStudy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SunStudy.Click
        Rhino.RhinoApp.WriteLine("Module under construction")
    End Sub
    '######################################################################
    '###SUN EXPOSURE ANALYSIS
    '######################################################################
    'box
    Private Sub SunExpGB1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SunExpGB1.Enter

    End Sub

    'proceed sun analysis
    Private Sub SEAButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEAButton.Click
        SE.Analyse(Rhino.RhinoDoc.ActiveDoc)
    End Sub

    'set start hour of anaylis
    Private Sub SEA_TimeStart_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_TimeStart.ValueChanged
        SEA_StartH = SEA_TimeStart.Value
        If SEA_EndH < SEA_StartH Then
            SEA_EndH = SEA_StartH
            'SEA_TimeEnd.Value = SEA_EndH
        End If

    End Sub

    'set end hour of analysis
    Private Sub SEA_TimeEnd_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_TimeEnd.ValueChanged
        SEA_EndH = SEA_TimeEnd.Value
        If SEA_EndH < SEA_StartH Then
            SEA_EndH = SEA_StartH
            'SEA_TimeEnd.Value = (SEA_EndH)
        End If
    End Sub

    'min Angle to analyse
    Private Sub SEA_AngleMin_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_AngleMin.ValueChanged
        SEA_MinAngle = SEA_AngleMin.Value
        If SEA_MaxAngle < SEA_MinAngle Then
            SEA_MaxAngle = SEA_MinAngle
        End If
    End Sub

    'max Angle to analyse
    Private Sub SEA_AngleMax_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_AngleMax.ValueChanged
        SEA_MaxAngle = SEA_AngleMax.Value
        If SEA_MaxAngle < SEA_MinAngle Then
            SEA_MaxAngle = SEA_MinAngle
        End If
    End Sub

    'pick occluding geometry
    Private Sub OccludingGeomButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OccludingGeomButton.Click

        SEA_OccObj = New List(Of Guid)()

        Dim go As New Rhino.Input.Custom.GetObject()
        go.SetCommandPrompt("pick shadow casting objects As mesh")
        go.GeometryAttributeFilter = Rhino.Input.Custom.GeometryAttributeFilter.ClosedMesh Or Rhino.Input.Custom.GeometryAttributeFilter.OpenMesh
        go.SubObjectSelect = False
        go.GroupSelect = True
        go.GetMultiple(1, 0)
        If go.CommandResult() <> Rhino.Commands.Result.Success Then
            Exit Sub
        End If

        'Dim ids As New List(Of Guid)()
        For i As Integer = 0 To go.ObjectCount - 1
            Dim objref As Rhino.DocObjects.ObjRef = New Rhino.DocObjects.ObjRef(go.[Object](i).ObjectId)
            If Not IsNothing(objref.Mesh) Then SEA_OccObj.Add(go.[Object](i).ObjectId)
        Next

        If Not SEA_OccObj Is Nothing Then
            OccludingGeomButton.ForeColor = Drawing.Color.Black
        Else
            OccludingGeomButton.ForeColor = Drawing.Color.Gray
        End If
    End Sub

    'pick receiving geometry
    Private Sub ReceivingGeomButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReceivingGeomButton.Click

        SEA_RecObj = New List(Of Guid)()

        Dim go As New Rhino.Input.Custom.GetObject()
        go.SetCommandPrompt("pick analysed objects as mesh")
        go.GeometryAttributeFilter = Rhino.Input.Custom.GeometryAttributeFilter.ClosedMesh Or Rhino.Input.Custom.GeometryAttributeFilter.OpenMesh
        go.SubObjectSelect = False
        go.GroupSelect = True
        go.GetMultiple(1, 0)
        If go.CommandResult() <> Rhino.Commands.Result.Success Then
            Exit Sub
        End If

        'Dim ids As New List(Of Guid)()
        For i As Integer = 0 To go.ObjectCount - 1
            Dim objref As Rhino.DocObjects.ObjRef = New Rhino.DocObjects.ObjRef(go.[Object](i).ObjectId)
            If Not IsNothing(objref.Mesh) Then SEA_RecObj.Add(go.[Object](i).ObjectId)
        Next

        If Not SEA_RecObj Is Nothing Then
            SEAButton.Enabled = True
        Else
            SEAButton.Enabled = False
        End If

        If Not SEA_RecObj Is Nothing Then
            ReceivingGeomButton.ForeColor = Drawing.Color.Black
        Else
            ReceivingGeomButton.ForeColor = Drawing.Color.Gray
        End If

    End Sub

    'pick negativeColor
    Private Sub AEA_NegativeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEA_NegativeButton.Click
        'If ColorDialog1.ShowDialog() = DialogResult.OK Then
        ColorDialog1.ShowDialog()
        AEA_NegativeButton.BackColor = ColorDialog1.Color
        SEA_colorNeg = ColorDialog1.Color
        'End If

    End Sub

    'pick positive color
    Private Sub AEA_PositiveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEA_PositiveButton.Click
        ColorDialog1.ShowDialog()
        AEA_PositiveButton.BackColor = ColorDialog1.Color
        SEA_colorPos = ColorDialog1.Color
    End Sub

    'choose Analyse Type
    Private Sub GradientOption_CB_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GradientOption_CB.SelectedIndexChanged

        Dim ind As Integer = GradientOption_CB.SelectedIndex
        Dim obj = GradientOption_CB.Items.Item(ind)

        If CStr(obj) = "ShowThreshold" Then
            SEA_GOption = 0 'MinExposure Time
        ElseIf CStr(obj) = "ShowGradient" Then
            SEA_GOption = 1 'Gradient
        Else
            SEA_GOption = 2 'Precised
        End If

        If SEA_GOption = 0 Then
            'threshold
            TimeExposureNumUD.Enabled = True
            SEA_TimeSegCB.Enabled = True

            AEA_NegativeButton.Enabled = True
            AEA_PositiveButton.Enabled = True

        ElseIf SEA_GOption = 1 Then
            'gradient
            TimeExposureNumUD.Enabled = False

            SEA_TimeSegCB.Enabled = False

            AEA_NegativeButton.Enabled = True
            AEA_PositiveButton.Enabled = True
        Else
            TimeExposureNumUD.Enabled = False
            SEA_TimeSegCB.Enabled = False

            AEA_NegativeButton.Enabled = False
            AEA_PositiveButton.Enabled = False

        End If
        'Rhino.RhinoApp.WriteLine(CStr(SEA_AType))
    End Sub

    'set time threshold
    Private Sub TimeExposureNumUD_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeExposureNumUD.ValueChanged
        SEA_Threshold = TimeExposureNumUD.Value
    End Sub

    'choose time units
    Private Sub TimeUnitCBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeUnitCBox1.SelectedIndexChanged

        Dim ind As Integer = TimeUnitCBox1.SelectedIndex
        Dim obj = TimeUnitCBox1.Items.Item(ind)

        If CStr(obj) = "60" Then
            SEA_TimeUnits = 1
        ElseIf CStr(obj) = "30" Then
            SEA_TimeUnits = 2
        ElseIf CStr(obj) = "15" Then
            SEA_TimeUnits = 4
        Else
            SEA_TimeUnits = 60
        End If

    End Sub

    'choose type of analyse - sun exposure or shadow analyse
    Private Sub SEA_AnalyseType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_AnalyseType.SelectedIndexChanged, SEA_AnalyseType.SelectedIndexChanged
        Dim ind As Integer = SEA_AnalyseType.SelectedIndex
        Dim obj = SEA_AnalyseType.Items.Item(ind)

        If CStr(obj) = "Sun Exposure" Then
            SEA_AnType = 0
        Else
            SEA_AnType = 1
        End If


    End Sub

    'choose number of segments
    Private Sub SEA_TimeSegCB_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_TimeSegCB.SelectedIndexChanged
        Dim ind As Integer = SEA_TimeSegCB.SelectedIndex
        Dim obj = SEA_TimeSegCB.Items.Item(ind)

        If CStr(obj) = "n.o." Then
            SEA_TimeSeg = 100
        ElseIf CStr(obj) = "1" Then
            SEA_TimeSeg = 1
        Else
            SEA_TimeSeg = 2
        End If

        'Rhino.RhinoApp.WriteLine(CStr(SEA_TimeSeg))
    End Sub



    '######################################################################
    '###SHADOW ANALYSIS
    '######################################################################
    'drop shadow
    Private Sub DropShadowButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropShadowButton.Click

        DropShadow.Draw(Rhino.RhinoDoc.ActiveDoc)

    End Sub

End Class

