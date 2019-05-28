Imports RMA.Rhino

'''<summary>
''' Every Rhino.NET Plug-In must have one and only one MRhinoPlugIn derived
''' class. DO NOT create an instance of this class. It is the responsibility
''' of Rhino.NET to create an instance of this class and register it with Rhino.
'''</summary>
Public Class RCOM4PlugIn
  Inherits RMA.Rhino.MRhinoUtilityPlugIn



  '''<summary>
  ''' Rhino tracks plug-ins by their unique ID. Every plug-in must have a unique id.
  ''' The Guid created by the project wizard is unique. You can create more Guids using
  ''' the "Create Guid" tool in the Tools menu.
  '''</summary>
  '''<returns>The id for this plug-in</returns>
  Public Overrides Function PlugInID() As System.Guid
        Return New System.Guid("{BE393D75-8300-4cac-BD99-A0EE947D3865}")
  End Function

  '''<returns>Plug-In name as displayed in the plug-in manager dialog</returns>
  Public Overrides Function PlugInName() As String
        Return "HdeM_PluginName"
  End Function

  '''<returns>Version information for this plug-in</returns>
  Public Overrides Function PlugInVersion() As String
    Return "1.0.0.0"
  End Function

  '''<summary>
  ''' Called after the plug-in is loaded and the constructor has been run.
  ''' This is a good place to perform any significant initialization,
  ''' license checking, and so on.  This function must return 1 for
  ''' the plug-in to continue to load.
  '''</summary>
  '''<returns>
  '''  1 = initialization succeeded, let the plug-in load
  '''  0 = unable to initialize, don't load plug-in and display an error dialog
  ''' -1 = unable to initialize, don't load plug-in and do not display an error
  '''      dialog. Note: OnUnloadPlugIn will not be called
    '''</returns>
    Dim m_docking_dialog As Form1
  Public Overrides Function OnLoadPlugIn() As Integer


        'the following should always be the case, but just to be paranoid
        If (m_docking_dialog Is Nothing) Then
            'create our custom user control
            m_docking_dialog = New Form1()
            'make a dockbar that this user control will be embedded in
            Dim cb As New RMA.UI.MRhinoUiDockBar(Form1.BarID(), "HdeM_Tools", m_docking_dialog)
            'add it to the Rhino user interface (initially hidden)
            RMA.UI.MRhinoDockBarManager.CreateRhinoDockBar(Me, cb, False, RMA.UI.MRhinoUiDockBar.DockLocation.floating, RMA.UI.MRhinoUiDockBar.DockStyle.right)
        End If

        Return 1

  End Function

  '''<summary>
  ''' Called when the plug-in is about to be unloaded.  After this
  ''' function is called, the plug-in will be disposed.
  '''</summary>
  Public Overrides Sub OnUnloadPlugIn()
    ' TODO: Add plug-in cleanup code here.
  End Sub


End Class
