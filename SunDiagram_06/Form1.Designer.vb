<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.SunPathDiagram = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SunRays = New System.Windows.Forms.Button()
        Me.Tabs = New System.Windows.Forms.TabControl()
        Me.Data = New System.Windows.Forms.TabPage()
        Me.SetHourCB = New System.Windows.Forms.CheckBox()
        Me.SetDateCB = New System.Windows.Forms.CheckBox()
        Me.HourTB = New System.Windows.Forms.NumericUpDown()
        Me.NorthOffset = New System.Windows.Forms.Label()
        Me.NorthOffTB = New System.Windows.Forms.TextBox()
        Me.TimeZone = New System.Windows.Forms.Label()
        Me.Longitude = New System.Windows.Forms.Label()
        Me.Latitude = New System.Windows.Forms.Label()
        Me.CityLocationLabel = New System.Windows.Forms.Label()
        Me.TimeZoneTB = New System.Windows.Forms.TextBox()
        Me.LongitudeTB = New System.Windows.Forms.TextBox()
        Me.LatitudeTB = New System.Windows.Forms.TextBox()
        Me.MonthCalendar1 = New System.Windows.Forms.MonthCalendar()
        Me.SunDiagramTab = New System.Windows.Forms.TabPage()
        Me.SunRaysGBox = New System.Windows.Forms.GroupBox()
        Me.ShadowRaysCheckBox1 = New System.Windows.Forms.CheckBox()
        Me.SunRaysPickPoint = New System.Windows.Forms.CheckBox()
        Me.SunPath = New System.Windows.Forms.GroupBox()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.SunPathShowAng = New System.Windows.Forms.CheckBox()
        Me.SunPathPickObj = New System.Windows.Forms.CheckBox()
        Me.SunPath3D = New System.Windows.Forms.CheckBox()
        Me.SunAnalysisTab = New System.Windows.Forms.TabPage()
        Me.SunExpGB1 = New System.Windows.Forms.GroupBox()
        Me.SEA_Reset = New System.Windows.Forms.Button()
        Me.SEAProgressBar = New System.Windows.Forms.ProgressBar()
        Me.SEA_TimeEnd_Min = New System.Windows.Forms.NumericUpDown()
        Me.SEA_TimeStart_Min = New System.Windows.Forms.NumericUpDown()
        Me.SEA_AngleMax = New System.Windows.Forms.NumericUpDown()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.SEA_AngleMin = New System.Windows.Forms.NumericUpDown()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.GradientOption_CB = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.SEA_AnalyseType = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.SEA_TimeSegCB = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.TimeUnitCBox1 = New System.Windows.Forms.ComboBox()
        Me.AEA_NegativeButton = New System.Windows.Forms.Button()
        Me.AEA_PositiveButton = New System.Windows.Forms.Button()
        Me.TimeExposureNumUD = New System.Windows.Forms.NumericUpDown()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.ReceivingGeomButton = New System.Windows.Forms.Button()
        Me.OccludingGeomButton = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.SEA_TimeEnd = New System.Windows.Forms.NumericUpDown()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.SEA_TimeStart = New System.Windows.Forms.NumericUpDown()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SEA_Lab1 = New System.Windows.Forms.Label()
        Me.SEAButton = New System.Windows.Forms.Button()
        Me.DropShadowBox = New System.Windows.Forms.GroupBox()
        Me.DropShadowButton = New System.Windows.Forms.Button()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.SunPathDiagram2 = New System.Windows.Forms.Button()
        Me.Tabs.SuspendLayout()
        Me.Data.SuspendLayout()
        CType(Me.HourTB, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SunDiagramTab.SuspendLayout()
        Me.SunRaysGBox.SuspendLayout()
        Me.SunPath.SuspendLayout()
        Me.SunAnalysisTab.SuspendLayout()
        Me.SunExpGB1.SuspendLayout()
        CType(Me.SEA_TimeEnd_Min, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SEA_TimeStart_Min, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SEA_AngleMax, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SEA_AngleMin, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.TimeExposureNumUD, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SEA_TimeEnd, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.SEA_TimeStart, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.DropShadowBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'SunPathDiagram
        '
        resources.ApplyResources(Me.SunPathDiagram, "SunPathDiagram")
        Me.SunPathDiagram.Name = "SunPathDiagram"
        Me.SunPathDiagram.UseVisualStyleBackColor = True
        '
        'Label1
        '
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.Name = "Label1"
        '
        'SunRays
        '
        resources.ApplyResources(Me.SunRays, "SunRays")
        Me.SunRays.Name = "SunRays"
        Me.SunRays.UseVisualStyleBackColor = True
        '
        'Tabs
        '
        resources.ApplyResources(Me.Tabs, "Tabs")
        Me.Tabs.Controls.Add(Me.Data)
        Me.Tabs.Controls.Add(Me.SunDiagramTab)
        Me.Tabs.Controls.Add(Me.SunAnalysisTab)
        Me.Tabs.Name = "Tabs"
        Me.Tabs.SelectedIndex = 0
        '
        'Data
        '
        Me.Data.Controls.Add(Me.SetHourCB)
        Me.Data.Controls.Add(Me.SetDateCB)
        Me.Data.Controls.Add(Me.HourTB)
        Me.Data.Controls.Add(Me.NorthOffset)
        Me.Data.Controls.Add(Me.NorthOffTB)
        Me.Data.Controls.Add(Me.TimeZone)
        Me.Data.Controls.Add(Me.Longitude)
        Me.Data.Controls.Add(Me.Latitude)
        Me.Data.Controls.Add(Me.CityLocationLabel)
        Me.Data.Controls.Add(Me.TimeZoneTB)
        Me.Data.Controls.Add(Me.LongitudeTB)
        Me.Data.Controls.Add(Me.LatitudeTB)
        Me.Data.Controls.Add(Me.MonthCalendar1)
        resources.ApplyResources(Me.Data, "Data")
        Me.Data.Name = "Data"
        Me.Data.UseVisualStyleBackColor = True
        '
        'SetHourCB
        '
        resources.ApplyResources(Me.SetHourCB, "SetHourCB")
        Me.SetHourCB.Name = "SetHourCB"
        Me.SetHourCB.UseVisualStyleBackColor = True
        '
        'SetDateCB
        '
        resources.ApplyResources(Me.SetDateCB, "SetDateCB")
        Me.SetDateCB.Name = "SetDateCB"
        Me.SetDateCB.UseVisualStyleBackColor = True
        '
        'HourTB
        '
        Me.HourTB.BackColor = System.Drawing.SystemColors.ControlLightLight
        resources.ApplyResources(Me.HourTB, "HourTB")
        Me.HourTB.ForeColor = System.Drawing.SystemColors.WindowText
        Me.HourTB.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.HourTB.Name = "HourTB"
        Me.HourTB.Value = New Decimal(New Integer() {12, 0, 0, 0})
        '
        'NorthOffset
        '
        resources.ApplyResources(Me.NorthOffset, "NorthOffset")
        Me.NorthOffset.Name = "NorthOffset"
        '
        'NorthOffTB
        '
        resources.ApplyResources(Me.NorthOffTB, "NorthOffTB")
        Me.NorthOffTB.Name = "NorthOffTB"
        '
        'TimeZone
        '
        resources.ApplyResources(Me.TimeZone, "TimeZone")
        Me.TimeZone.Name = "TimeZone"
        '
        'Longitude
        '
        resources.ApplyResources(Me.Longitude, "Longitude")
        Me.Longitude.Name = "Longitude"
        '
        'Latitude
        '
        resources.ApplyResources(Me.Latitude, "Latitude")
        Me.Latitude.Name = "Latitude"
        '
        'CityLocationLabel
        '
        resources.ApplyResources(Me.CityLocationLabel, "CityLocationLabel")
        Me.CityLocationLabel.Name = "CityLocationLabel"
        '
        'TimeZoneTB
        '
        resources.ApplyResources(Me.TimeZoneTB, "TimeZoneTB")
        Me.TimeZoneTB.Name = "TimeZoneTB"
        '
        'LongitudeTB
        '
        resources.ApplyResources(Me.LongitudeTB, "LongitudeTB")
        Me.LongitudeTB.Name = "LongitudeTB"
        '
        'LatitudeTB
        '
        resources.ApplyResources(Me.LatitudeTB, "LatitudeTB")
        Me.LatitudeTB.Name = "LatitudeTB"
        '
        'MonthCalendar1
        '
        resources.ApplyResources(Me.MonthCalendar1, "MonthCalendar1")
        Me.MonthCalendar1.Name = "MonthCalendar1"
        Me.MonthCalendar1.TitleBackColor = System.Drawing.SystemColors.ControlDark
        '
        'SunDiagramTab
        '
        Me.SunDiagramTab.Controls.Add(Me.SunRaysGBox)
        Me.SunDiagramTab.Controls.Add(Me.SunPath)
        resources.ApplyResources(Me.SunDiagramTab, "SunDiagramTab")
        Me.SunDiagramTab.Name = "SunDiagramTab"
        Me.SunDiagramTab.UseVisualStyleBackColor = True
        '
        'SunRaysGBox
        '
        Me.SunRaysGBox.Controls.Add(Me.ShadowRaysCheckBox1)
        Me.SunRaysGBox.Controls.Add(Me.SunRaysPickPoint)
        Me.SunRaysGBox.Controls.Add(Me.SunRays)
        resources.ApplyResources(Me.SunRaysGBox, "SunRaysGBox")
        Me.SunRaysGBox.Name = "SunRaysGBox"
        Me.SunRaysGBox.TabStop = False
        '
        'ShadowRaysCheckBox1
        '
        resources.ApplyResources(Me.ShadowRaysCheckBox1, "ShadowRaysCheckBox1")
        Me.ShadowRaysCheckBox1.Name = "ShadowRaysCheckBox1"
        Me.ShadowRaysCheckBox1.UseVisualStyleBackColor = True
        '
        'SunRaysPickPoint
        '
        resources.ApplyResources(Me.SunRaysPickPoint, "SunRaysPickPoint")
        Me.SunRaysPickPoint.Name = "SunRaysPickPoint"
        Me.SunRaysPickPoint.UseVisualStyleBackColor = True
        '
        'SunPath
        '
        Me.SunPath.Controls.Add(Me.SunPathDiagram2)
        Me.SunPath.Controls.Add(Me.CheckBox2)
        Me.SunPath.Controls.Add(Me.CheckBox1)
        Me.SunPath.Controls.Add(Me.SunPathShowAng)
        Me.SunPath.Controls.Add(Me.SunPathPickObj)
        Me.SunPath.Controls.Add(Me.SunPath3D)
        Me.SunPath.Controls.Add(Me.SunPathDiagram)
        resources.ApplyResources(Me.SunPath, "SunPath")
        Me.SunPath.Name = "SunPath"
        Me.SunPath.TabStop = False
        '
        'CheckBox2
        '
        resources.ApplyResources(Me.CheckBox2, "CheckBox2")
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'CheckBox1
        '
        resources.ApplyResources(Me.CheckBox1, "CheckBox1")
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'SunPathShowAng
        '
        resources.ApplyResources(Me.SunPathShowAng, "SunPathShowAng")
        Me.SunPathShowAng.Name = "SunPathShowAng"
        Me.SunPathShowAng.UseVisualStyleBackColor = True
        '
        'SunPathPickObj
        '
        resources.ApplyResources(Me.SunPathPickObj, "SunPathPickObj")
        Me.SunPathPickObj.Name = "SunPathPickObj"
        Me.SunPathPickObj.UseVisualStyleBackColor = True
        '
        'SunPath3D
        '
        resources.ApplyResources(Me.SunPath3D, "SunPath3D")
        Me.SunPath3D.Name = "SunPath3D"
        Me.SunPath3D.UseVisualStyleBackColor = True
        '
        'SunAnalysisTab
        '
        Me.SunAnalysisTab.Controls.Add(Me.SunExpGB1)
        Me.SunAnalysisTab.Controls.Add(Me.DropShadowBox)
        resources.ApplyResources(Me.SunAnalysisTab, "SunAnalysisTab")
        Me.SunAnalysisTab.Name = "SunAnalysisTab"
        Me.SunAnalysisTab.UseVisualStyleBackColor = True
        '
        'SunExpGB1
        '
        Me.SunExpGB1.Controls.Add(Me.SEA_Reset)
        Me.SunExpGB1.Controls.Add(Me.SEAProgressBar)
        Me.SunExpGB1.Controls.Add(Me.SEA_TimeEnd_Min)
        Me.SunExpGB1.Controls.Add(Me.SEA_TimeStart_Min)
        Me.SunExpGB1.Controls.Add(Me.SEA_AngleMax)
        Me.SunExpGB1.Controls.Add(Me.Label11)
        Me.SunExpGB1.Controls.Add(Me.SEA_AngleMin)
        Me.SunExpGB1.Controls.Add(Me.Label12)
        Me.SunExpGB1.Controls.Add(Me.Label13)
        Me.SunExpGB1.Controls.Add(Me.GradientOption_CB)
        Me.SunExpGB1.Controls.Add(Me.Label8)
        Me.SunExpGB1.Controls.Add(Me.SEA_AnalyseType)
        Me.SunExpGB1.Controls.Add(Me.Label10)
        Me.SunExpGB1.Controls.Add(Me.SEA_TimeSegCB)
        Me.SunExpGB1.Controls.Add(Me.Label9)
        Me.SunExpGB1.Controls.Add(Me.TimeUnitCBox1)
        Me.SunExpGB1.Controls.Add(Me.AEA_NegativeButton)
        Me.SunExpGB1.Controls.Add(Me.AEA_PositiveButton)
        Me.SunExpGB1.Controls.Add(Me.TimeExposureNumUD)
        Me.SunExpGB1.Controls.Add(Me.Label7)
        Me.SunExpGB1.Controls.Add(Me.Label6)
        Me.SunExpGB1.Controls.Add(Me.Label5)
        Me.SunExpGB1.Controls.Add(Me.ReceivingGeomButton)
        Me.SunExpGB1.Controls.Add(Me.OccludingGeomButton)
        Me.SunExpGB1.Controls.Add(Me.Label4)
        Me.SunExpGB1.Controls.Add(Me.SEA_TimeEnd)
        Me.SunExpGB1.Controls.Add(Me.Label3)
        Me.SunExpGB1.Controls.Add(Me.SEA_TimeStart)
        Me.SunExpGB1.Controls.Add(Me.Label2)
        Me.SunExpGB1.Controls.Add(Me.SEA_Lab1)
        Me.SunExpGB1.Controls.Add(Me.SEAButton)
        resources.ApplyResources(Me.SunExpGB1, "SunExpGB1")
        Me.SunExpGB1.Name = "SunExpGB1"
        Me.SunExpGB1.TabStop = False
        '
        'SEA_Reset
        '
        resources.ApplyResources(Me.SEA_Reset, "SEA_Reset")
        Me.SEA_Reset.Name = "SEA_Reset"
        Me.SEA_Reset.UseVisualStyleBackColor = True
        '
        'SEAProgressBar
        '
        resources.ApplyResources(Me.SEAProgressBar, "SEAProgressBar")
        Me.SEAProgressBar.Name = "SEAProgressBar"
        Me.SEAProgressBar.Tag = "Text"
        '
        'SEA_TimeEnd_Min
        '
        Me.SEA_TimeEnd_Min.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.SEA_TimeEnd_Min.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.SEA_TimeEnd_Min, "SEA_TimeEnd_Min")
        Me.SEA_TimeEnd_Min.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.SEA_TimeEnd_Min.Name = "SEA_TimeEnd_Min"
        '
        'SEA_TimeStart_Min
        '
        Me.SEA_TimeStart_Min.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.SEA_TimeStart_Min.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.SEA_TimeStart_Min, "SEA_TimeStart_Min")
        Me.SEA_TimeStart_Min.Maximum = New Decimal(New Integer() {59, 0, 0, 0})
        Me.SEA_TimeStart_Min.Name = "SEA_TimeStart_Min"
        '
        'SEA_AngleMax
        '
        Me.SEA_AngleMax.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.SEA_AngleMax.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.SEA_AngleMax, "SEA_AngleMax")
        Me.SEA_AngleMax.Maximum = New Decimal(New Integer() {90, 0, 0, 0})
        Me.SEA_AngleMax.Name = "SEA_AngleMax"
        Me.SEA_AngleMax.Value = New Decimal(New Integer() {90, 0, 0, 0})
        '
        'Label11
        '
        resources.ApplyResources(Me.Label11, "Label11")
        Me.Label11.Name = "Label11"
        '
        'SEA_AngleMin
        '
        Me.SEA_AngleMin.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.SEA_AngleMin.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.SEA_AngleMin, "SEA_AngleMin")
        Me.SEA_AngleMin.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.SEA_AngleMin.Name = "SEA_AngleMin"
        '
        'Label12
        '
        resources.ApplyResources(Me.Label12, "Label12")
        Me.Label12.Name = "Label12"
        '
        'Label13
        '
        resources.ApplyResources(Me.Label13, "Label13")
        Me.Label13.Name = "Label13"
        '
        'GradientOption_CB
        '
        Me.GradientOption_CB.FormattingEnabled = True
        Me.GradientOption_CB.Items.AddRange(New Object() {resources.GetString("GradientOption_CB.Items"), resources.GetString("GradientOption_CB.Items1"), resources.GetString("GradientOption_CB.Items2")})
        resources.ApplyResources(Me.GradientOption_CB, "GradientOption_CB")
        Me.GradientOption_CB.Name = "GradientOption_CB"
        '
        'Label8
        '
        resources.ApplyResources(Me.Label8, "Label8")
        Me.Label8.Name = "Label8"
        '
        'SEA_AnalyseType
        '
        Me.SEA_AnalyseType.FormattingEnabled = True
        Me.SEA_AnalyseType.Items.AddRange(New Object() {resources.GetString("SEA_AnalyseType.Items"), resources.GetString("SEA_AnalyseType.Items1")})
        resources.ApplyResources(Me.SEA_AnalyseType, "SEA_AnalyseType")
        Me.SEA_AnalyseType.Name = "SEA_AnalyseType"
        '
        'Label10
        '
        resources.ApplyResources(Me.Label10, "Label10")
        Me.Label10.Name = "Label10"
        '
        'SEA_TimeSegCB
        '
        Me.SEA_TimeSegCB.FormattingEnabled = True
        Me.SEA_TimeSegCB.Items.AddRange(New Object() {resources.GetString("SEA_TimeSegCB.Items"), resources.GetString("SEA_TimeSegCB.Items1"), resources.GetString("SEA_TimeSegCB.Items2")})
        resources.ApplyResources(Me.SEA_TimeSegCB, "SEA_TimeSegCB")
        Me.SEA_TimeSegCB.Name = "SEA_TimeSegCB"
        '
        'Label9
        '
        resources.ApplyResources(Me.Label9, "Label9")
        Me.Label9.Name = "Label9"
        '
        'TimeUnitCBox1
        '
        Me.TimeUnitCBox1.FormattingEnabled = True
        Me.TimeUnitCBox1.Items.AddRange(New Object() {resources.GetString("TimeUnitCBox1.Items"), resources.GetString("TimeUnitCBox1.Items1"), resources.GetString("TimeUnitCBox1.Items2"), resources.GetString("TimeUnitCBox1.Items3")})
        resources.ApplyResources(Me.TimeUnitCBox1, "TimeUnitCBox1")
        Me.TimeUnitCBox1.Name = "TimeUnitCBox1"
        '
        'AEA_NegativeButton
        '
        Me.AEA_NegativeButton.BackColor = System.Drawing.SystemColors.ActiveBorder
        Me.AEA_NegativeButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        resources.ApplyResources(Me.AEA_NegativeButton, "AEA_NegativeButton")
        Me.AEA_NegativeButton.Name = "AEA_NegativeButton"
        Me.AEA_NegativeButton.UseVisualStyleBackColor = False
        '
        'AEA_PositiveButton
        '
        Me.AEA_PositiveButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        resources.ApplyResources(Me.AEA_PositiveButton, "AEA_PositiveButton")
        Me.AEA_PositiveButton.Name = "AEA_PositiveButton"
        Me.AEA_PositiveButton.UseVisualStyleBackColor = True
        '
        'TimeExposureNumUD
        '
        Me.TimeExposureNumUD.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.TimeExposureNumUD.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.TimeExposureNumUD, "TimeExposureNumUD")
        Me.TimeExposureNumUD.Maximum = New Decimal(New Integer() {14, 0, 0, 0})
        Me.TimeExposureNumUD.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.TimeExposureNumUD.Name = "TimeExposureNumUD"
        Me.TimeExposureNumUD.Value = New Decimal(New Integer() {2, 0, 0, 0})
        '
        'Label7
        '
        resources.ApplyResources(Me.Label7, "Label7")
        Me.Label7.Name = "Label7"
        '
        'Label6
        '
        resources.ApplyResources(Me.Label6, "Label6")
        Me.Label6.Name = "Label6"
        '
        'Label5
        '
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.Name = "Label5"
        '
        'ReceivingGeomButton
        '
        Me.ReceivingGeomButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        resources.ApplyResources(Me.ReceivingGeomButton, "ReceivingGeomButton")
        Me.ReceivingGeomButton.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.ReceivingGeomButton.Name = "ReceivingGeomButton"
        Me.ReceivingGeomButton.UseVisualStyleBackColor = True
        '
        'OccludingGeomButton
        '
        Me.OccludingGeomButton.FlatAppearance.BorderColor = System.Drawing.Color.Gray
        resources.ApplyResources(Me.OccludingGeomButton, "OccludingGeomButton")
        Me.OccludingGeomButton.ForeColor = System.Drawing.SystemColors.AppWorkspace
        Me.OccludingGeomButton.Name = "OccludingGeomButton"
        Me.OccludingGeomButton.UseVisualStyleBackColor = True
        '
        'Label4
        '
        resources.ApplyResources(Me.Label4, "Label4")
        Me.Label4.Name = "Label4"
        '
        'SEA_TimeEnd
        '
        Me.SEA_TimeEnd.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.SEA_TimeEnd.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.SEA_TimeEnd, "SEA_TimeEnd")
        Me.SEA_TimeEnd.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.SEA_TimeEnd.Name = "SEA_TimeEnd"
        Me.SEA_TimeEnd.Value = New Decimal(New Integer() {18, 0, 0, 0})
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'SEA_TimeStart
        '
        Me.SEA_TimeStart.BackColor = System.Drawing.SystemColors.ControlLightLight
        Me.SEA_TimeStart.ForeColor = System.Drawing.SystemColors.WindowText
        resources.ApplyResources(Me.SEA_TimeStart, "SEA_TimeStart")
        Me.SEA_TimeStart.Maximum = New Decimal(New Integer() {23, 0, 0, 0})
        Me.SEA_TimeStart.Name = "SEA_TimeStart"
        Me.SEA_TimeStart.Value = New Decimal(New Integer() {7, 0, 0, 0})
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'SEA_Lab1
        '
        resources.ApplyResources(Me.SEA_Lab1, "SEA_Lab1")
        Me.SEA_Lab1.Name = "SEA_Lab1"
        '
        'SEAButton
        '
        resources.ApplyResources(Me.SEAButton, "SEAButton")
        Me.SEAButton.Name = "SEAButton"
        Me.SEAButton.UseVisualStyleBackColor = True
        '
        'DropShadowBox
        '
        Me.DropShadowBox.Controls.Add(Me.DropShadowButton)
        resources.ApplyResources(Me.DropShadowBox, "DropShadowBox")
        Me.DropShadowBox.Name = "DropShadowBox"
        Me.DropShadowBox.TabStop = False
        '
        'DropShadowButton
        '
        resources.ApplyResources(Me.DropShadowButton, "DropShadowButton")
        Me.DropShadowButton.Name = "DropShadowButton"
        Me.DropShadowButton.UseVisualStyleBackColor = True
        '
        'ColorDialog1
        '
        Me.ColorDialog1.Color = System.Drawing.Color.DarkRed
        '
        'SunPathDiagram2
        '
        resources.ApplyResources(Me.SunPathDiagram2, "SunPathDiagram2")
        Me.SunPathDiagram2.Name = "SunPathDiagram2"
        Me.SunPathDiagram2.UseVisualStyleBackColor = True
        '
        'Form1
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.Tabs)
        Me.Name = "Form1"
        Me.Tabs.ResumeLayout(False)
        Me.Data.ResumeLayout(False)
        Me.Data.PerformLayout()
        CType(Me.HourTB, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SunDiagramTab.ResumeLayout(False)
        Me.SunRaysGBox.ResumeLayout(False)
        Me.SunRaysGBox.PerformLayout()
        Me.SunPath.ResumeLayout(False)
        Me.SunPath.PerformLayout()
        Me.SunAnalysisTab.ResumeLayout(False)
        Me.SunExpGB1.ResumeLayout(False)
        Me.SunExpGB1.PerformLayout()
        CType(Me.SEA_TimeEnd_Min, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SEA_TimeStart_Min, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SEA_AngleMax, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SEA_AngleMin, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.TimeExposureNumUD, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SEA_TimeEnd, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.SEA_TimeStart, System.ComponentModel.ISupportInitialize).EndInit()
        Me.DropShadowBox.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents SunPathDiagram As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SunRays As System.Windows.Forms.Button
    Friend WithEvents Tabs As System.Windows.Forms.TabControl
    Friend WithEvents SunDiagramTab As System.Windows.Forms.TabPage
    Friend WithEvents SunAnalysisTab As System.Windows.Forms.TabPage
    Friend WithEvents Data As System.Windows.Forms.TabPage
    Friend WithEvents CityLocationLabel As System.Windows.Forms.Label
    Friend WithEvents Longitude As System.Windows.Forms.Label
    Friend WithEvents Latitude As System.Windows.Forms.Label
    Friend WithEvents SEAButton As System.Windows.Forms.Button
    Public WithEvents LongitudeTB As System.Windows.Forms.TextBox
    Friend WithEvents TimeZone As System.Windows.Forms.Label
    Public WithEvents LatitudeTB As System.Windows.Forms.TextBox
    Public WithEvents TimeZoneTB As System.Windows.Forms.TextBox
    Friend WithEvents NorthOffset As System.Windows.Forms.Label
    Public WithEvents NorthOffTB As System.Windows.Forms.TextBox
    Friend WithEvents SetHourCB As System.Windows.Forms.CheckBox
    Friend WithEvents SetDateCB As System.Windows.Forms.CheckBox
    Public WithEvents HourTB As System.Windows.Forms.NumericUpDown
    Friend WithEvents SunPath As System.Windows.Forms.GroupBox
    Friend WithEvents SunPathPickObj As System.Windows.Forms.CheckBox
    Friend WithEvents SunPath3D As System.Windows.Forms.CheckBox
    Friend WithEvents SunRaysGBox As System.Windows.Forms.GroupBox
    Friend WithEvents SunRaysPickPoint As System.Windows.Forms.CheckBox
    Friend WithEvents ShadowRaysCheckBox1 As System.Windows.Forms.CheckBox
    Public WithEvents MonthCalendar1 As System.Windows.Forms.MonthCalendar
    Friend WithEvents DropShadowBox As System.Windows.Forms.GroupBox
    Friend WithEvents DropShadowButton As System.Windows.Forms.Button
    Friend WithEvents SunExpGB1 As System.Windows.Forms.GroupBox
    Friend WithEvents SEA_Lab1 As System.Windows.Forms.Label
    Friend WithEvents TimeUnitCBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents AEA_NegativeButton As System.Windows.Forms.Button
    Friend WithEvents AEA_PositiveButton As System.Windows.Forms.Button
    Public WithEvents TimeExposureNumUD As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents ReceivingGeomButton As System.Windows.Forms.Button
    Friend WithEvents OccludingGeomButton As System.Windows.Forms.Button
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents SEA_TimeEnd As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents SEA_TimeStart As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents ColorDialog1 As System.Windows.Forms.ColorDialog
    Friend WithEvents SEA_TimeSegCB As System.Windows.Forms.ComboBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents GradientOption_CB As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents SEA_AnalyseType As System.Windows.Forms.ComboBox
    Public WithEvents SEA_AngleMax As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Public WithEvents SEA_AngleMin As System.Windows.Forms.NumericUpDown
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents SunPathShowAng As System.Windows.Forms.CheckBox
    Public WithEvents SEA_TimeEnd_Min As System.Windows.Forms.NumericUpDown
    Public WithEvents SEA_TimeStart_Min As System.Windows.Forms.NumericUpDown
    Friend WithEvents SEAProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents SEA_Reset As System.Windows.Forms.Button
    Friend WithEvents CheckBox2 As Windows.Forms.CheckBox
    Friend WithEvents CheckBox1 As Windows.Forms.CheckBox
    Friend WithEvents SunPathDiagram2 As Windows.Forms.Button
End Class
