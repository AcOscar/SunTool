
'User Interface
<System.Runtime.InteropServices.Guid("52C942F9-3338-4155-9389-B4C9A5354933")>
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
        'Dim arr() As Double
        'arr =
        GetData()
        'month = arr(1)
        'day = arr(0)

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
        Dim ci As New System.Globalization.CultureInfo("en-gb")
        Dim instance As New System.Data.DataSet With {
            .Locale = ci
        }

        SEA_TimeSeg = 100.0
    End Sub

    '###################################################################
    '###SET DATA
    '###################################################################
    'set Latitude
    Private Sub LatitudeTB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LatitudeTB.TextChanged
        Try
            lat = Double.Parse(LatitudeTB.Text)

        Catch ex As FormatException
            LatitudeTB.Text = "47"
            lat = 47
        End Try

    End Sub

    'Set Longitude
    Private Sub LongitudeTB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LongitudeTB.TextChanged
        Try
            lon = Double.Parse(LongitudeTB.Text)
        Catch ex As FormatException
            LongitudeTB.Text = "8"
            lon = 8
        End Try
    End Sub

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
        'Dim arr() As Double
        'arr = 
        GetData()
        'month = arr(1)
        'day = arr(0)

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
            HourTB.Enabled = True
        Else
            HourTB.Enabled = False
        End If

    End Sub

    'calendar
    Private Sub MonthCalendar1_DateChanged(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles MonthCalendar1.DateChanged
        If setDate Then

            'MonthCalendar1.ImeMode = Windows.Forms.ImeMode.On

            'Dim temp As String = CStr(MonthCalendar1.SelectionStart.Date)

            'Dim arr() As Double

            ' arr = 
            GetData()
            'month = arr(1)
            'day = arr(0)

            'Else

            '    MonthCalendar1.ImeMode = Windows.Forms.ImeMode.Off

        End If

    End Sub

    'get data from the calendar
    Private Sub GetData() 'As Double()

        ' Dim temp As String = CStr(MonthCalendar1.SelectionStart.Date)
        Dim myDate As Date = MonthCalendar1.SelectionStart.Date

        'Dim data(1) As Double
        'data(1) = myDate.Month
        'data(0) = myDate.Day
        month = myDate.Month
        day = myDate.Day

        ' Return data

    End Sub

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
    End Sub
    Private Sub SunPathDiagram2_Click(sender As Object, e As EventArgs) Handles SunPathDiagram2.Click
        SPD2.Draw(Rhino.RhinoDoc.ActiveDoc)

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
    '###SUN EXPOSURE ANALYSIS
    '######################################################################

    'proceed sun analysis
    Private Sub SEAButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEAButton.Click
        SE.Analyse(Rhino.RhinoDoc.ActiveDoc, SEAProgressBar)
        SEAProgressBar.Value = 0
    End Sub

    'reset mesh
    Private Sub SEA_Reset_Click(sender As System.Object, e As System.EventArgs) Handles SEA_Reset.Click
        SE.ResetMesh(Rhino.RhinoDoc.ActiveDoc)
    End Sub

    'set start hour of anaylis
    Private Sub SEA_TimeStart_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_TimeStart.ValueChanged

        SEA_StartHVal = SEA_TimeStart.Value
        SEA_StartH = SEA_StartHVal + SEA_StartMinVal

        If SEA_EndH < SEA_StartH Then
            SEA_EndH = SEA_StartH
        End If

    End Sub

    'set start minutes of anaylis
    Private Sub SEA_TimeStart_Min_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SEA_TimeStart_Min.ValueChanged
        Dim tempVal As Double
        tempVal = SEA_TimeStart_Min.Value

        SEA_StartMinVal = tempVal / 60

        SEA_StartH = SEA_StartHVal + SEA_StartMinVal

        If SEA_EndH < SEA_StartH Then
            SEA_EndH = SEA_StartH
        End If

    End Sub


    'set end hour of analysis
    Private Sub SEA_TimeEnd_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_TimeEnd.ValueChanged
        Sea_EndHVal = SEA_TimeEnd.Value

        SEA_EndH = Sea_EndHVal + SEA_EndMinVal

        If SEA_EndH < SEA_StartH Then
            SEA_EndH = SEA_StartH
        End If
    End Sub


    'set end minutes of anaylis
    Private Sub SEA_TimeEnd_Min_ValueChanged(sender As System.Object, e As System.EventArgs) Handles SEA_TimeEnd_Min.ValueChanged

        Dim tempVal As Double
        tempVal = SEA_TimeEnd_Min.Value

        SEA_EndMinVal = tempVal / 60

        SEA_EndH = Sea_EndHVal + SEA_EndMinVal

        If SEA_EndH < SEA_StartH Then
            SEA_EndH = SEA_StartH
        End If

    End Sub


    'min Angle to analyse
    Private Sub SEA_AngleMin_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_AngleMin.ValueChanged
        SEA_MinAngle = SEA_AngleMin.Value
        SEA_MinAngleRad = SEA_MinAngle / (180 / Math.PI)
        If SEA_MaxAngle < SEA_MinAngle Then
            SEA_MaxAngle = SEA_MinAngle

            SEA_MaxAngleRad = SEA_MaxAngle / (180 / Math.PI)

        End If
    End Sub

    'max Angle to analyse
    Private Sub SEA_AngleMax_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_AngleMax.ValueChanged
        SEA_MaxAngle = SEA_AngleMax.Value
        SEA_MaxAngleRad = SEA_MaxAngle / (180 / Math.PI)

        If SEA_MaxAngle < SEA_MinAngle Then
            SEA_MaxAngle = SEA_MinAngle

            SEA_MaxAngleRad = SEA_MaxAngle / (180 / Math.PI)

        End If
    End Sub

    'pick occluding geometry
    Private Sub OccludingGeomButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OccludingGeomButton.Click

        SEA_CastObj = New List(Of Guid)()
        SEA_CastObjMesh = New List(Of Rhino.Geometry.Mesh)

        Dim go As New Rhino.Input.Custom.GetObject()
        go.SetCommandPrompt("pick shadow casting objects As mesh")
        go.GeometryAttributeFilter = Rhino.Input.Custom.GeometryAttributeFilter.ClosedMesh Or Rhino.Input.Custom.GeometryAttributeFilter.OpenMesh
        go.SubObjectSelect = False
        go.GroupSelect = True
        go.GetMultiple(1, 0)
        If go.CommandResult() <> Rhino.Commands.Result.Success Then
            Exit Sub
        End If

        For i As Integer = 0 To go.ObjectCount - 1
            Dim objref As New Rhino.DocObjects.ObjRef(go.[Object](i).ObjectId)
            If Not IsNothing(objref.Mesh) Then
                SEA_CastObj.Add(go.[Object](i).ObjectId)
                'https://frasergreenroyd.com/rhino-error-this-object-cannot-be-modified-because-it-is-controlled-by-a-document/
                SEA_CastObjMesh.Add(objref.Mesh.DuplicateMesh)
            End If
        Next

        If SEA_CastObj IsNot Nothing Then
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
            Dim objref As New Rhino.DocObjects.ObjRef(go.[Object](i).ObjectId)
            If Not IsNothing(objref.Mesh) Then SEA_RecObj.Add(go.[Object](i).ObjectId)
        Next

        If SEA_RecObj IsNot Nothing Then
            SEAButton.Enabled = True
        Else
            SEAButton.Enabled = False
        End If

        If SEA_RecObj IsNot Nothing Then
            ReceivingGeomButton.ForeColor = Drawing.Color.Black
        Else
            ReceivingGeomButton.ForeColor = Drawing.Color.Gray
        End If

    End Sub

    'pick negativeColor
    Private Sub AEA_NegativeButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEA_NegativeButton.Click
        ColorDialog1.ShowDialog()
        AEA_NegativeButton.BackColor = ColorDialog1.Color
        SEA_colorNeg = ColorDialog1.Color

    End Sub

    'pick positive color
    Private Sub AEA_PositiveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AEA_PositiveButton.Click
        ColorDialog1.ShowDialog()
        AEA_PositiveButton.BackColor = ColorDialog1.Color
        SEA_colorPos = ColorDialog1.Color
    End Sub

    'choose Analyse Type
    Private Sub GradientOption_CB_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GradientOption_CB.SelectedIndexChanged

        Select Case GradientOption_CB.SelectedItem.ToString
            Case "ShowThreshold"
                SEA_GOption = SEA_GOptionValues.Threshold

                TimeExposureNumUD.Enabled = True

                SEA_TimeSegCB.Enabled = True

                AEA_NegativeButton.Enabled = True
                AEA_PositiveButton.Enabled = True

            Case "ShowGradient"
                SEA_GOption = SEA_GOptionValues.Gradient

                TimeExposureNumUD.Enabled = False

                SEA_TimeSegCB.Enabled = False

                AEA_NegativeButton.Enabled = True
                AEA_PositiveButton.Enabled = True

            Case Else 'HoursOfExposure
                SEA_GOption = SEA_GOptionValues.Precised

                TimeExposureNumUD.Enabled = False

                SEA_TimeSegCB.Enabled = False

                AEA_NegativeButton.Enabled = False
                AEA_PositiveButton.Enabled = False

        End Select

    End Sub

    'set time threshold
    Private Sub TimeExposureNumUD_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeExposureNumUD.ValueChanged
        SEA_Threshold = TimeExposureNumUD.Value
    End Sub

    'choose time units
    Private Sub TimeUnitCBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimeUnitCBox1.SelectedIndexChanged

        Select Case TimeUnitCBox1.SelectedItem.ToString
            Case "60"
                SEA_TimeUnits = 1
            Case "30"
                SEA_TimeUnits = 2
            Case "15"
                SEA_TimeUnits = 4
            Case Else
                SEA_TimeUnits = 60
        End Select

    End Sub

    'choose type of analyse - sun exposure or shadow analyse
    Private Sub SEA_AnalyseType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SEA_AnalyseType.SelectedIndexChanged, SEA_AnalyseType.SelectedIndexChanged

        Select Case SEA_AnalyseType.SelectedItem.ToString
            Case "Sun Exposure"
                SEA_AnType = SEA_AnTypeValues.SunExposure

            Case Else
                SEA_AnType = SEA_AnTypeValues.ShadowCast

        End Select

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

    End Sub

    '######################################################################
    '###SHADOW ANALYSIS
    '######################################################################
    'drop shadow
    Private Sub DropShadowButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DropShadowButton.Click

        DropShadow.Draw(Rhino.RhinoDoc.ActiveDoc)

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        SunExposure2.Analyse(Rhino.RhinoDoc.ActiveDoc, SEAProgressBar)
        SEAProgressBar.Value = 0

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Dim myStudy As New SunStudy With {
            .Sun = New Rhino.Render.Sun
        }

        myStudy.Sun.SetPosition(MonthCalendar1.SelectionStart.Date, lat, lon)

        myStudy.ReceivingGeometry = SEA_RecObj

        myStudy.CastingGeometry = SEA_CastObj

        myStudy.StartTime = New DateTime(MonthCalendar1.SelectionStart.Date.Date.Year, MonthCalendar1.SelectionStart.Date.Date.Month, MonthCalendar1.SelectionStart.Date.Date.Day, SEA_TimeStart.Value, SEA_TimeStart_Min.Value, 0)

        myStudy.EndTime = New DateTime(MonthCalendar1.SelectionStart.Date.Date.Year, MonthCalendar1.SelectionStart.Date.Date.Month, MonthCalendar1.SelectionStart.Date.Date.Day, SEA_TimeEnd.Value, SEA_TimeEnd_Min.Value, 0)

        myStudy.AnalizedAngleMin = SEA_MinAngle

        myStudy.AnalizedAngleMax = SEA_MaxAngle

        myStudy.AnalyseType = SEA_AnType

        myStudy.DiagramType = SEA_GOption

        myStudy.TimeThreshold = SEA_Threshold

        myStudy.TimeUnit = SEA_TimeUnits

        myStudy.TimeSegments = SEA_TimeSeg

        myStudy.Analyse()

    End Sub

    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles Button2.Click
        SunExposure2.Analyse(Rhino.RhinoDoc.ActiveDoc, SEAProgressBar, New Date(2023, 1, 1), New Date(2023, 12, 31))
        SEAProgressBar.Value = 0
    End Sub

    Private Sub CreateTextCB_CheckedChanged(sender As Object, e As EventArgs) Handles CreateDotsCB.CheckedChanged
        CreateDots = CreateDotsCB.Checked
    End Sub

    Private Sub WriteFileCB_CheckedChanged(sender As Object, e As EventArgs) Handles WriteFileCB.CheckedChanged
        WriteFile = WriteFileCB.Checked
    End Sub

    Private Sub CreateLegendCB_CheckedChanged(sender As Object, e As EventArgs) Handles CreateLegendCB.CheckedChanged
        CreateLegend = CreateLegendCB.Checked
    End Sub

End Class

