Public Class SunAngle

    'source code:
    'http://wiki.naturalfrequency.com/wiki/Solar_Position_Calculator#Manual


    '# get sun angle 
    Public Shared Function Calculate(ByVal iJulianDate As Double, ByVal iTimeMonth As Double, ByVal iTimeDay As Double, ByVal iTimeHours As Double, ByVal iTimeMinutes As Double, _
     ByVal FTimeZone As Double, ByVal fLatitude As Double, ByVal fLongitude As Double, ByVal fOffset As Double) As Double()

        'debugger
        'If iTimeMonth < 1 Or iTimeMonth > 12 Or iTimeHours < 0 Or iTimeHours > 24 Or iTimeMinutes < 0 Or iTimeMinutes > 60 Or fLatitude > 90 Or fLongitude > 90 Then Return Nothin
        If iTimeMonth < 1 Or iTimeMonth > 12 Or iTimeHours < 0 Or iTimeHours > 24 Or fLatitude > 90 Or fLatitude < -90 Or fLongitude > 180 Or fLongitude < -180 Then Return Nothing

        Dim arrMonth() As Integer = {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334}

        ' Calculated data.
        Dim fDifference As Double
        Dim fDeclination As Double
        Dim fEquation As Double

        ' Print Flag
        Dim intPrint As Integer = 0

        ' Solar information.
        Dim fLocalTime As Double
        Dim fSolarTime As Double
        Dim fAltitude As Double
        Dim fAzimuth As Double
        Dim fSunrise As Double
        Dim fSunset As Double

        ' Temp data.
        'Dim t, local_noon, test, fHourAngle, sin1, cos2, arrReturn(3)

        ' calculate location data.
        fLatitude = fLatitude * (Math.PI / 180.0)
        FTimeZone = FTimeZone * 15 * (Math.PI / 180.0)
        fLongitude = fLongitude * (Math.PI / 180.0)
        fLocalTime = iTimeHours + (iTimeMinutes / 60.0)

        ' Calculatejulian date.
        Dim a As Integer = CType(iTimeMonth, Integer)

        If iJulianDate = 0 Then iJulianDate = arrMonth(a - 1) + iTimeDay

        ' Calculate solar declination as per Carruthers et al.
        Dim t As Double = 2 * Math.PI * ((iJulianDate - 1) / 365.0)

        fDeclination = (0.322003 _
         - 22.971 * Math.Cos(t) _
         - 0.357898 * Math.Cos(2 * t) _
         - 0.14398 * Math.Cos(3 * t) _
         + 3.94638 * Math.Sin(t) _
         + 0.019334 * Math.Sin(2 * t) _
         + 0.05928 * Math.Sin(3 * t) _
         )

        ' Convert to radians.
        fDeclination = fDeclination * (Math.PI / 180.0)

        ' Calculate the equation of time as per Carruthers et al.
        t = (279.134 + 0.985647 * iJulianDate) * (Math.PI / 180.0)

        fEquation = (5.0323 _
         - 100.976 * Math.Sin(t) _
         + 595.275 * Math.Sin(2 * t) _
         + 3.6858 * Math.Sin(3 * t) _
         - 12.47 * Math.Sin(4 * t) _
         - 430.847 * Math.Cos(t) _
         + 12.5024 * Math.Cos(2 * t) _
         + 18.25 * Math.Cos(3 * t) _
         )

        ' Convert seconds to hours.
        fEquation = fEquation / 3600.0

        ' Calculate difference (in minutes) from reference longitude.
        fDifference = (((fLongitude - FTimeZone) * 180 / Math.PI) * 4) / 60.0

        ' Convert solar noon to local noon.
        Dim local_noon As Double = 12.0 - fEquation - fDifference

        ' Calculate angle normal to meridian plane.
        If (fLatitude > (0.99 * (Math.PI / 2.0))) Then fLatitude = (0.99 * (Math.PI / 2.0))
        If (fLatitude < -(0.99 * (Math.PI / 2.0))) Then fLatitude = -(0.99 * (Math.PI / 2.0))

        Dim test As Double = -Math.Tan(fLatitude) * Math.Tan(fDeclination)

        If (test < -1) Then
            t = Math.Acos(-1.0) / (15 * (Math.PI / 180.0))
        ElseIf (test > 1) Then
            t = Math.Acos(1.0) / (15 * (Math.PI / 180.0))
        Else
            t = Math.Acos(-Math.Tan(fLatitude) * Math.Tan(fDeclination)) / (15 * (Math.PI / 180.0))
        End If

        ' Sunrise and sunset.
        fSunrise = local_noon - t
        fSunset = local_noon + t

        ' Check validity of local time.
        If (fLocalTime > fSunset) Then fLocalTime = fSunset
        If (fLocalTime < fSunrise) Then fLocalTime = fSunrise
        If (fLocalTime > 24.0) Then fLocalTime = 24.0
        If (fLocalTime < 0.0) Then fLocalTime = 0.0

        ' Caculate solar time.
        fSolarTime = fLocalTime + fEquation + fDifference

        ' Calculate hour angle.
        Dim fHourAngle As Double = (15 * (fSolarTime - 12)) * (Math.PI / 180.0)

        ' Calculate current altitude.
        t = (Math.Sin(fDeclination) * Math.Sin(fLatitude)) + (Math.Cos(fDeclination) * Math.Cos(fLatitude) * Math.Cos(fHourAngle))
        fAltitude = Math.Asin(t)

        ' Calculate current azimuth.
        t = (Math.Sin(fDeclination) * Math.Cos(fLatitude)) _
         - (Math.Cos(fDeclination) * Math.Sin(fLatitude) _
         * Math.Cos(fHourAngle))

        ' Avoid division by zero error.
        Dim sin1, cos2 As Double
        If (fAltitude < (Math.PI / 2.0)) Then
            sin1 = (-Math.Cos(fDeclination) * Math.Sin(fHourAngle)) / Math.Cos(fAltitude)
            cos2 = t / Math.Cos(fAltitude)
        Else
            sin1 = 0.0
            cos2 = 0.0
        End If

        ' Some range checking.
        If (sin1 > 1.0) Then sin1 = 1.0
        If (sin1 < -1.0) Then sin1 = -1.0
        If (cos2 < -1.0) Then cos2 = -1.0
        If (cos2 > 1.0) Then cos2 = 1.0

        ' Calculate azimuth subject to quadrant.
        If (sin1 < -0.99999) Then
            fAzimuth = Math.Asin(sin1)
        ElseIf ((sin1 > 0.0) And (cos2 < 0.0)) Then
            If (sin1 >= 1.0) Then
                fAzimuth = -(Math.PI / 2.0)
            Else
                fAzimuth = (Math.PI / 2.0) + ((Math.PI / 2.0) - Math.Asin(sin1))
            End If
        ElseIf ((sin1 < 0.0) And (cos2 < 0.0)) Then
            If (sin1 <= -1.0) Then
                fAzimuth = (Math.PI / 2.0)
            Else
                fAzimuth = -(Math.PI / 2.0) - ((Math.PI / 2.0) + Math.Asin(sin1))
            End If
        Else
            fAzimuth = Math.Asin(sin1)
        End If

        ' A little last-ditch range check.
        If ((fAzimuth < 0.0) And (fLocalTime < 10.0)) Then
            fAzimuth = -fAzimuth
        End If

        fAltitude = fAltitude * 180 / Math.PI
        fAzimuth = fAzimuth * 180 / Math.PI

        'print out the calculated values
        If intPrint = 1 Then
            Rhino.RhinoApp.WriteLine("Julian Date : " & iJulianDate & _
             ", Date : " & iTimeDay & "." & iTimeMonth & _
             ", Time : " & iTimeHours & ":" & Math.Round(iTimeMinutes) & _
             ", Altitude : " & Math.Round(fAltitude * 100) / 100 & _
             ", fAzimuth : " & 180 - Math.Round(fAzimuth * 100) / 100 & _
             ", fsunrise : " & Math.Round(fSunrise * 100) / 100 & _
             ", fsunset : " & Math.Round(fSunset * 100) / 100)
        End If

        Dim arrReturn(3) As Double

        arrReturn(0) = fAltitude
        arrReturn(1) = fAzimuth + fOffset
        arrReturn(2) = fSunrise
        arrReturn(3) = fSunset

        Calculate = arrReturn

    End Function

End Class
