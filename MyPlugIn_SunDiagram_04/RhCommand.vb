Imports Rhino.DocObjects
Imports Rhino
Imports Rhino.Geometry

Public Class RhCommand
    Inherits Rhino.Commands.Command

    Shared _instance As RhCommand

    Public Sub New()
        _instance = Me
    End Sub

    Public Shared ReadOnly Property Instance() As RhCommand
        Get
            Return _instance
        End Get
    End Property

    Public Overrides ReadOnly Property EnglishName() As String
        Get
            Return "HdM_SunDiagram"
        End Get
    End Property

    Protected Overrides Function RunCommand(ByVal doc As Rhino.RhinoDoc, ByVal mode As Rhino.Commands.RunMode) As Rhino.Commands.Result

        RhPlugin.Instance.OpenPanel()

        Return Rhino.Commands.Result.Success

    End Function

End Class

