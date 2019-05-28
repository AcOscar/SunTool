'''<summary>
''' The following class is required for all Rhino.NET plug-ins.
''' These are used to display plug-in information in the plug-in manager.
''' Any string will work for these attributes, so if you don't have a fax
''' number it is OK to enter something like "none"
'''</summary>
Public Class RCOM4PlugInAttributes
  Inherits RMA.Rhino.MRhinoPlugInAttributes

  Public Overrides Function Address() As String
        Return "Basel, Rheinschanze"
  End Function

  Public Overrides Function Country() As String
        Return "CH"
  End Function

  Public Overrides Function Email() As String
        Return "e.augustynowicz@herzogdemeuron.com"
  End Function

  Public Overrides Function Fax() As String
        Return "----"
  End Function

  Public Overrides Function Organization() As String
        Return "Digital Tech. Group / Herzog de Meuron Architekten"
  End Function

  Public Overrides Function Phone() As String
        Return "----"
  End Function

  Public Overrides Function UpdateURL() As String
        Return "http://www.herzogdemeuron.com"
  End Function

  Public Overrides Function Website() As String
        Return "http://www.herzogdemeuron.com"
  End Function
End Class