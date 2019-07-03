Public Class SunAngle2

    'source code:
    'http://wiki.naturalfrequency.com/wiki/Solar_Position_Calculator#Manual




    Private fLongitude As Double
    Private FTimeZone As Double
    Private fLatitude As Double
    Private fDifference As Double
    Private fDeclination As Double
    Private fEquation As Double


    Private fLocalTime As Double
    Private fSolarTime As Double
    Private fAltitude As Double
    Private fAzimuth As Double
    Private fSunrise As Double
    Private fSunset As Double

    Private iJulianDate As Double

    Private t As Double


    Private fOffset As Double
    Private _Altitude As Double
    Private _Azimuth As Double
    'Private _Sunset As Double

    Private sin1 As Double
    Private cos2 As Double

    Private fHourAngle As Double


    Private SinfDeclination As Double
    Private CosfDeclination As Double
    Private SinfLatitude As Double
    Private CosfLatitude As Double
    Private CosfHourAngle As Double
    Private prodcfhacfd As Double

    Public ReadOnly Property Altitude As Double
        Get
            Return _Altitude
        End Get
    End Property

    Public ReadOnly Property Azimuth As Double
        Get
            Return _Azimuth
        End Get
    End Property

    Public ReadOnly Property Sunrise As Double

    Public ReadOnly Property Sunset As Double
    'Get
    '    Return _Sunset
    'End Get


    Private ReadOnly arrMonth() As Integer = {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334}

    Private Const pihalf = Math.PI / 2


    Public Sub New(ByVal JulianDate As Double,
                   ByVal TimeMonth As Double,
                   ByVal TimeDay As Double,
                   ByVal TimeZone As Double,
                   ByVal Latitude As Double,
                   ByVal Longitude As Double,
                   ByVal Offset As Double)



        'debugger
        'If iTimeMonth < 1 Or iTimeMonth > 12 Or iTimeHours < 0 Or iTimeHours > 24 Or iTimeMinutes < 0 Or iTimeMinutes > 60 Or fLatitude > 90 Or fLongitude > 90 Then Return Nothin
        If TimeMonth < 1 Or TimeMonth > 12 Or
            fLatitude > 90 Or fLatitude < -90 Or
            fLongitude > 180 Or fLongitude < -180 Then Return
        fOffset = Offset
        'Dim arrMonth() As Integer = {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334}

        ' Calculated data.
        'Dim fDifference As Double
        'Dim fDeclination As Double
        'Dim fEquation As Double

        ' Print Flag
        'Dim intPrint As Integer = 0

        ' Solar information.
        'Dim fLocalTime As Double
        'Dim fSolarTime As Double
        'Dim fAltitude As Double
        'Dim fAzimuth As Double
        'Dim fSunrise As Double
        'Dim fSunset As Double

        ' Temp data.
        'Dim t, local_noon, test, fHourAngle, sin1, cos2, arrReturn(3)

        ' calculate location data.
        fLatitude = Latitude * (Math.PI / 180.0)
        FTimeZone = TimeZone * 15 * (Math.PI / 180.0)
        fLongitude = Longitude * (Math.PI / 180.0)

        ' Calculate julian date.
        Dim a As Integer = CType(TimeMonth, Integer)

        If JulianDate = 0 Then
            iJulianDate = arrMonth(a - 1) + TimeDay
        Else
            iJulianDate = JulianDate
        End If

        ' Calculate solar declination as per Carruthers et al.
        t = 2 * Math.PI * ((iJulianDate - 1) / 365.0)

        fDeclination = (0.322003 _
         - 22.971 * Math.Cos(t) _
         - 0.357898 * Math.Cos(2 * t) _
         - 0.14398 * Math.Cos(3 * t) _
         + 3.94638 * Math.Sin(t) _
         + 0.019334 * Math.Sin(2 * t) _
         + 0.05928 * Math.Sin(3 * t)
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
         + 18.25 * Math.Cos(3 * t)
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

        _Sunrise = fSunrise
        _Sunset = fSunset

    End Sub
    Public Sub Calculate(ByVal iTimeHours As Double,
                         ByVal iTimeMinutes As Double)

        fLocalTime = iTimeHours + (iTimeMinutes / 60.0)

        ' Check validity of local time.
        If (fLocalTime > Sunset) Then fLocalTime = Sunset
        If (fLocalTime < Sunrise) Then fLocalTime = Sunrise
        If (fLocalTime > 24.0) Then fLocalTime = 24.0
        If (fLocalTime < 0.0) Then fLocalTime = 0.0

        ' Caculate solar time.
        fSolarTime = fLocalTime + fEquation + fDifference

        ' Calculate hour angle.
        fHourAngle = (15 * (fSolarTime - 12)) * (Math.PI / 180.0)

        SinfDeclination = Math.Sin(fDeclination)
        CosfDeclination = Math.Cos(fDeclination)
        SinfLatitude = Math.Sin(fLatitude)
        CosfLatitude = Math.Cos(fLatitude)
        CosfHourAngle = Math.Cos(fHourAngle)
        prodcfhacfd = CosfHourAngle * CosfDeclination


        ' Calculate current altitude.
        t = (SinfDeclination * SinfLatitude) +
            (prodcfhacfd * CosfLatitude)
        fAltitude = Math.Asin(t)

        ' Calculate current azimuth.
        t = (SinfDeclination * CosfLatitude) -
        (prodcfhacfd * SinfLatitude)

        'Avoid division by zero error.
        If (fAltitude < (Math.PI / 2.0)) Then
            sin1 = (-CosfDeclination * Math.Sin(fHourAngle)) / Math.Cos(fAltitude)
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
        'If (sin1 < -0.99999) Then
        '    fAzimuth = Math.Asin(sin1)
        If ((sin1 > 0.0) And (cos2 < 0.0)) Then
            If (sin1 = 1.0) Then
                fAzimuth = -(pihalf)
            Else
                fAzimuth = (pihalf) + ((pihalf) - Math.Asin(sin1))
                fAzimuth = (pihalf) + ((pihalf) - Math.Asin(sin1))
            End If
        ElseIf ((sin1 < 0.0) And (cos2 < 0.0)) Then
            If (sin1 = -1.0) Then
                fAzimuth = (pihalf)
            Else
                fAzimuth = -(pihalf) - ((pihalf) + Math.Asin(sin1))
            End If
        Else
            fAzimuth = Math.Asin(sin1)
        End If

        ' A little last-ditch range check.
        If ((fAzimuth < 0.0) And (fLocalTime < 10.0)) Then
            fAzimuth = -fAzimuth
        End If

        _Altitude = fAltitude * 180 / Math.PI
        _Azimuth = fAzimuth * 180 / Math.PI + fOffset

    End Sub

End Class
