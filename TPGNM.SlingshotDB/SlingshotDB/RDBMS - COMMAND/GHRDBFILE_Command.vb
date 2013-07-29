Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System

Public Class GHRDBFILE_Command
  Inherits Grasshopper.Kernel.GH_Component

  Private _connector As String = "MySQL"
  Private _path As String = ""

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("RDB File Command", "Command", "Send a command to a Relational Database File", "Slingshot!", "RDBMS")
  End Sub

  'Exposure parameter (line dividers)
  Public Overrides ReadOnly Property Exposure As Grasshopper.Kernel.GH_Exposure
    Get
      Return GH_Exposure.tertiary
    End Get
  End Property

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{77c736f0-415f-4ab1-b735-4dcc9e7a2180}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHOLEDB_Command
    End Get
  End Property
#End Region

#Region "Menu Items"
  'Append Component menues.
  Public Overrides Function AppendMenuItems(menu As Windows.Forms.ToolStripDropDown) As Boolean

    Menu_AppendItem(menu, "Connector Settings...", AddressOf Menu_Settings)

    Return True
  End Function

  'On menu item click...
  Private Sub Menu_Settings(ByVal sender As Object, ByVal e As EventArgs)

    'Open Settings dialogue
    Dim m_settingsdialogue As New form_FileSelect(_connector, _path)
    m_settingsdialogue.ShowDialog()
    _connector = m_settingsdialogue.DatabaseType
    _path = m_settingsdialogue.FilePath

    ExpireSolution(True)

  End Sub

  'GH Writer
  Public Overrides Function Write(writer As GH_IWriter) As Boolean
    writer.SetString("Connector", _connector)
    writer.SetString("Path", _path)
    Return MyBase.Write(writer)
  End Function

  'GH Reader
  Public Overrides Function Read(reader As GH_IReader) As Boolean
    reader.TryGetString("Connector", _connector)
    reader.TryGetString("Path", _path)
    Return MyBase.Read(reader)
  End Function
#End Region

#Region "Inputs/Outputs"

  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", False, GH_ParamAccess.item)
    pManager.AddTextParameter("SQL Command", "Command", "A SQL command.", GH_ParamAccess.item)

  End Sub

#End Region

#Region "Solution"

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Exceptions", "out", "Displays errors.")
  End Sub

  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try

      Dim connector As String = _connector
      Dim filepath As String = _path
      Dim connect As Boolean = False
      Dim command As String = Nothing

      DA.GetData(Of Boolean)(0, connect)
      DA.GetData(Of String)(1, command)

      Dim bool As Boolean = False
      If connect = True Then
        Dim dbcommand As New clsRDBMS()
        If connector = "Access" Then
          bool = dbcommand.MySQLCommand(filepath, command)
        ElseIf connector = "Excel" Then
          bool = dbcommand.ODBCCommand(filepath, command)
        ElseIf connector = "SQLite" Then
          bool = dbcommand.OLEDBCommand(filepath, command)
        End If

        'Display success
        If bool = True Then
          DA.SetData(0, "Command executed!")
        Else
          DA.SetData(0, "No command executed...")
        End If

      End If

    Catch ex As Exception
      DA.SetData(0, ex.ToString)
    End Try

  End Sub

#End Region

End Class
