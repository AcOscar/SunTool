﻿Public Class SunAngle2

    'source code:
    'http://wiki.naturalfrequency.com/wiki/Solar_Position_Calculator#Manual


    Private ReadOnly fLongitude As Double
    Private ReadOnly FTimeZone As Double
    Private ReadOnly fLatitude As Double
    Private ReadOnly fDifference As Double
    Private ReadOnly fDeclination As Double
    Private ReadOnly fEquation As Double


    Private fLocalTime As Double
    Private fSolarTime As Double
    Private fAltitude As Double
    Private fAzimuth As Double
    Private ReadOnly fSunrise As Double
    Private ReadOnly fSunset As Double

    Private ReadOnly iJulianDate As Double

    Private t As Double

    ''' <summary>
    ''' offset to north
    ''' </summary>
    Private ReadOnly fOffset As Double
    'Private _Altitude As Double
    'Private _Azimuth As Double
    Private _AltitudeRad As Double
    Private _AzimuthRad As Double
    'Private _Sunset As Double

    'helpers
    Private sin1 As Double
    Private cos2 As Double

    Private fHourAngle As Double


    Private SinfDeclination As Double
    Private CosfDeclination As Double
    Private SinfLatitude As Double
    Private CosfLatitude As Double
    Private CosfHourAngle As Double
    Private prod_cfha_cfd As Double
    Private CosfAltitude As Double
    'Public ReadOnly Property Altitude As Double
    '    Get
    '        Return _Altitude
    '    End Get
    'End Property

    'Public ReadOnly Property Azimuth As Double
    '    Get
    '        Return _Azimuth
    '    End Get
    'End Property
    Public ReadOnly Property AzimuthRad As Double
        Get
            Return _AzimuthRad
        End Get
    End Property
    Public ReadOnly Property AltitudeRad As Double
        Get
            Return _AltitudeRad
        End Get
    End Property

    Public ReadOnly Property Sunrise As Double

    Public ReadOnly Property Sunset As Double


    Private ReadOnly arrMonth() As Integer = {0, 31, 59, 90, 120, 151, 181, 212, 243, 273, 304, 334}

    Private Const pihalf = Math.PI / 2


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="JulianDate"></param>
    ''' <param name="TimeMonth"></param>
    ''' <param name="TimeDay"></param>
    ''' <param name="TimeZone"></param>
    ''' <param name="Latitude"></param>
    ''' <param name="Longitude"></param>
    ''' <param name="Offset"></param>
    Public Sub New(ByVal JulianDate As Double,
                   ByVal TimeMonth As Double,
                   ByVal TimeDay As Double,
                   ByVal TimeZone As Double,
                   ByVal Latitude As Double,
                   ByVal Longitude As Double,
                   ByVal Offset As Double)

        If TimeMonth < 1 Or TimeMonth > 12 Or
            Latitude > 90 Or Latitude < -90 Or
            Longitude > 180 Or Longitude < -180 Then Return

        fOffset = Offset

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
        fDeclination *= (Math.PI / 180.0)

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
        fEquation /= 3600.0

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

    Public Sub SetAngle(ByVal h As Integer,
                         ByVal mins As Integer)

        _AltitudeRad = arrAltitude(h, mins)
        _AzimuthRad = arrAzimuth(h, mins)



    End Sub
    Private arrAltitude(,) As Double
    Private arrAzimuth(,) As Double

    Public Sub PreCalculate(ByVal startT As Double,
                            ByVal endT As Double,
                            ByVal TUnits As Double)
        'Dim exposureU As Integer
        'Dim arrExposure As New List(Of Integer)
        Dim minsS As Double

        'Dim ray As Rhino.Geometry.Ray3d

        'Dim tMonth As Double = month
        'Dim tDay As Double = day

        'Dim angle As Double

        'Dim vec As Rhino.Geometry.Vector3d
        Dim iter As Integer = CType((endT - startT) * TUnits, Integer)
        Dim iMin As Integer = CType(Math.Floor(startT), Integer)
        minsS = startT - iMin
        Dim iMax As Integer = CType(endT, Integer)
        Dim h, mins As Double
        Dim minCount As Integer
        Dim jMax As Integer

        Select Case TUnits
            Case 1
                'Unit = 60min
                minCount = 60
                jMax = 0
            Case 2
                'Unit = 30min
                minCount = 30
                jMax = 1
            Case 4
                'Unit = 15min
                minCount = 15
                jMax = 3
            Case Else
                'Unit = 1min
                minCount = 1
                jMax = 59
        End Select
        ReDim arrAltitude(iMax - iMin, jMax) 'As Double
        ReDim arrAzimuth(iMax - iMin, jMax) 'As Double


        For i = iMin To iMax

            For j = 0 To jMax
                mins = minsS * 60 + j * minCount

                h = i + Math.Floor(mins / 60)

                mins = mins Mod 60

                Calculate(h, mins)

                arrAltitude(i - iMin, j) = AltitudeRad
                arrAzimuth(i - iMin, j) = AzimuthRad


            Next j
        Next i





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
        prod_cfha_cfd = CosfHourAngle * CosfDeclination


        ' Calculate current altitude.
        t = (SinfDeclination * SinfLatitude) +
            (prod_cfha_cfd * CosfLatitude)
        fAltitude = Math.Asin(t)

        ' Calculate current azimuth.
        t = (SinfDeclination * CosfLatitude) -
        (prod_cfha_cfd * SinfLatitude)

        CosfAltitude = Math.Cos(fAltitude)

        'Avoid division by zero error.
        If (fAltitude < pihalf) Then
            sin1 = (-CosfDeclination * Math.Sin(fHourAngle)) / CosfAltitude
            cos2 = t / CosfAltitude
        Else
            sin1 = 0.0
            cos2 = 0.0
        End If

        ' Some range checking.
        If (sin1 > 1.0) Then
            sin1 = 1.0
        End If
        If (sin1 < -1.0) Then
            sin1 = -1.0
        End If
        If (cos2 < -1.0) Then
            cos2 = -1.0
        End If
        If (cos2 > 1.0) Then
            cos2 = 1.0
        End If

        ' Calculate azimuth subject to quadrant.
        'If (sin1 < -0.99999) Then
        '    fAzimuth = Math.Asin(sin1)
        If ((sin1 > 0.0) AndAlso (cos2 < 0.0)) Then
            If (sin1 = 1.0) Then
                fAzimuth = -(pihalf)
            Else
                'fAzimuth = (pihalf) + ((pihalf) - Math.Asin(sin1))
                fAzimuth = Math.PI - Math.Asin(sin1)

            End If
        ElseIf ((sin1 < 0.0) AndAlso (cos2 < 0.0)) Then
            If (sin1 = -1.0) Then
                fAzimuth = (pihalf)
            Else
                'fAzimuth = -(pihalf) - ((pihalf) + Math.Asin(sin1))
                fAzimuth = -Math.PI - Math.Asin(sin1)
            End If
        Else
            fAzimuth = Math.Asin(sin1)
        End If

        ' A little last-ditch range check.
        If ((fAzimuth < 0.0) AndAlso (fLocalTime < 10.0)) Then
            fAzimuth = -fAzimuth
        End If

        _AltitudeRad = fAltitude
        _AzimuthRad = fAzimuth + (fOffset * Math.PI / 180)
        '_Altitude = fAltitude * 180 / Math.PI
        '_Azimuth = (fAzimuth * 180 / Math.PI) + fOffset

    End Sub

End Class
