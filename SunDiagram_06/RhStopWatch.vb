Public Class RhStopWatch

    Private Shared myStopWatch As Stopwatch

    Public Shared Sub Start()
        myStopWatch = New Stopwatch()
        myStopWatch.Start()
    End Sub

    Public Shared Sub [Stop]()
        If myStopWatch IsNot Nothing Then
            myStopWatch.Stop()
            Show("RunTime")
        End If

    End Sub

    Public Shared Sub Show(Optional ByVal Message As String = "Split Time")
        Dim ts As TimeSpan = myStopWatch.Elapsed
        Dim elapsedTime As String = String.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10)
        Rhino.RhinoApp.WriteLine(vbCrLf & elapsedTime & " " & Message & vbCrLf)

    End Sub

End Class
