Imports RMA.Rhino
'Imports RMA.OpenNURBS
'Imports RMA.Rhino.RhUtil

'''<summary>
''' A Rhino.NET plug-in can contain as many MRhinoCommand derived classes as it wants.
''' DO NOT create an instance of this class (this is the responsibility of Rhino.NET.)
'''</summary>
Public Class RCOM4Command
  Inherits RMA.Rhino.MRhinoCommand

  '''<summary>
  ''' Rhino tracks commands by their unique ID. Every command must have a unique id.
  ''' The Guid created by the project wizard is unique. You can create more Guids using
  ''' the "Create Guid" tool in the Tools menu.
  '''</summary>
  '''<returns>The id for this command</returns>
  Public Overrides Function CommandUUID() As System.Guid
        Return New Guid("{4792387B-7CFE-487e-B706-BA2A05BCE8BA}")
  End Function

  '''<returns>The command name as it appears on the Rhino command line</returns>
  Public Overrides Function EnglishCommandName() As String
        Return "HdeM_SunDiagram"
  End Function

  '''<summary> This gets called when when the user runs this command.</summary>
  Public Overrides Function RunCommand(ByVal context As RMA.Rhino.IRhinoCommandContext) As RMA.Rhino.IRhinoCommand.result

        RMA.UI.MRhinoDockBarManager.ShowDockBar(Form1.BarID(), True, False)

        Return IRhinoCommand.result.success

    End Function
End Class
