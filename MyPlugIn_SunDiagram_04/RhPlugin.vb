Public Class RhPlugin
    Inherits Rhino.PlugIns.PlugIn

    Shared _instance As RhPlugin

    Public Shared WrapperGUID As Guid

    Public Sub New()

        _instance = Me

        WrapperGUID = GetType(Form1).GUID

    End Sub

    Protected Overrides Function OnLoad(ByRef errorMessage As String) As Rhino.PlugIns.LoadReturnCode

        Rhino.UI.Panels.RegisterPanel(Me, GetType(Form1), "HdM Sun Tool", My.Resources.SunIcon)

        OpenPanel()

        Return Rhino.PlugIns.LoadReturnCode.Success

    End Function

    Public Shared ReadOnly Property Instance() As RhPlugin

        Get

            Return _instance

        End Get

    End Property


    Sub OpenPanel()

        If Not Rhino.UI.Panels.IsPanelVisible(WrapperGUID) Then

            Rhino.UI.Panels.OpenPanel(WrapperGUID)

        End If

    End Sub

End Class
