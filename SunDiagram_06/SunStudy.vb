Imports Rhino


Public Class SunStudy


    Public Property Sun As Rhino.Render.Sun
    Public Property ReceivingGeometry As List(Of Guid)
    Public Property CastingGeometry As List(Of Guid)
    Public Property StartTime As DateTime
    Public Property EndTime As DateTime
    Public Property AnalizedAngleMin As Double
    Public Property AnalizedAngleMax As Double
    Public Property AnalyseType As SEA_GOptionValues
    Public Property DiagramType As SEA_AnTypeValues
    Public Property TimeThreshold As Double
    Public Property TimeUnit As Integer
    Public Property TimeSegments As Integer

    Friend Sub Analyse()

        Select Case AnalyseType
            Case SEA_AnTypeValues.ShadowCast

            Case SEA_AnTypeValues.SunExposure

        End Select

    End Sub
End Class
