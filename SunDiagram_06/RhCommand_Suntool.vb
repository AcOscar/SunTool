Imports Rhino

Public Class RhCommand_Suntool
    Inherits Commands.Command

    Shared _instance As RhCommand_Suntool

    Public Sub New()
        _instance = Me
    End Sub

    Public Shared ReadOnly Property Instance() As RhCommand_Suntool
        Get
            Return _instance
        End Get
    End Property

    Public Overrides ReadOnly Property EnglishName() As String
        Get
            Return "SunTool"
        End Get
    End Property

    Protected Overrides Function RunCommand(ByVal doc As RhinoDoc, ByVal mode As Commands.RunMode) As Commands.Result

        RhPlugin.Instance.OpenPanel()

        Return Commands.Result.Success

    End Function

End Class

