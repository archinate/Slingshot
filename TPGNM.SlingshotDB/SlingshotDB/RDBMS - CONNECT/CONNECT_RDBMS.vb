Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System

Public Class CONNECT_RDBMS
  Inherits Grasshopper.Kernel.GH_Component

  Private _rdbms As String = "MySQL"
  Private _server As String = "localhost"
  Private _port As String = "3306"
  Private _user As String = "root"
  Private _pass As String = "password"
  Private _database As String = ""
  Private _cmdtimeout As String = "20"
  Private _contimeout As String = "5"

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("Connection String", "CString", "Formats a connection string. (MySQL, Oracle, PostgreSQL, SQL Server 2012)", "Slingshot!", "RDBMS")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{b1edb7f0-a224-4d0e-b543-d9296d1fa936}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHMYSQL_MySQLConnect

    End Get
  End Property
#End Region

#Region "Menu Items"
  'Append Component menues.
  Public Overrides Function AppendMenuItems(menu As Windows.Forms.ToolStripDropDown) As Boolean

    Menu_AppendItem(menu, "Connection Settings...", AddressOf Menu_Settings)

    Return True
  End Function

  'On menu item click...
  Private Sub Menu_Settings(ByVal sender As Object, ByVal e As EventArgs)

    'Open Settings dialogue
    Dim m_settingsdialogue As New form_Command(_rdbms, _server, _port, _user, _pass, _database, _cmdtimeout, _contimeout)
    m_settingsdialogue.ShowDialog()
    _rdbms = m_settingsdialogue.Connector
    _server = m_settingsdialogue.Server
    _port = m_settingsdialogue.Port
    _user = m_settingsdialogue.UserID
    _pass = m_settingsdialogue.Password
    _database = m_settingsdialogue.Database
    _cmdtimeout = m_settingsdialogue.CommandTimeout
    _contimeout = m_settingsdialogue.ConnectionTimeout

    ExpireSolution(True)

  End Sub

  'GH Writer
  Public Overrides Function Write(writer As GH_IWriter) As Boolean
    writer.SetString("Connector", _rdbms)
    writer.SetString("Server", _server)
    writer.SetString("Port", _port)
    writer.SetString("UserID", _user)
    writer.SetString("Password", _pass)
    writer.SetString("Database", _database)
    writer.SetString("CommandTimeout", _cmdtimeout)
    writer.SetString("ConnectionTimeout", _contimeout)
    Return MyBase.Write(writer)
  End Function

  'GH Reader
  Public Overrides Function Read(reader As GH_IReader) As Boolean
    reader.TryGetString("Connector", _rdbms)
    reader.TryGetString("Server", _server)
    reader.TryGetString("Port", _port)
    reader.TryGetString("UserID", _user)
    reader.TryGetString("Password", _pass)
    reader.TryGetString("Database", _database)
    reader.TryGetString("CommandTimeout", _cmdtimeout)
    reader.TryGetString("ConnectionTimeout", _contimeout)
    Return MyBase.Read(reader)
  End Function
#End Region

#Region "Inputs/Outputs"

  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)

  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("Connection String", "CString", "A MySQL connection string.")
  End Sub

#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim connector As String = _rdbms
      Dim server As String = _server
      Dim port As String = _port
      Dim uid As String = _user
      Dim pass As String = _pass
      Dim db As String = _database
      Dim cmdtime As String = _cmdtimeout
      Dim contime As String = _contimeout

      Dim connectionstring As String = Nothing

      If connector = "MySQL" Then
        If db = "" Then
          connectionstring = "Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Pwd=" & pass & "; default command timeout=" & cmdtime & "; Connection Timeout=" & contime
        Else
          connectionstring = "Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Pwd=" & pass & "; Database=" & db & "; default command timeout=" & cmdtime & "; Connection Timeout=" & contime
        End If

      ElseIf connector = "Oracle" Then
        If db = "" Then
          connectionstring = "Driver={Microsoft ODBC for Oracle};Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Pwd=" & pass & "; commandtimeout=" & cmdtime & "; Timeout=" & contime
        Else
          connectionstring = "Driver={Microsoft ODBC for Oracle};Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Database=" & db & "; Pwd=" & pass & "; commandtimeout=" & cmdtime & "; Timeout=" & contime
        End If

      ElseIf connector = "PostgreSQL" Then
        If db = "" Then
          connectionstring = "Driver={PostgreSQL}; Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Pwd=" & pass & "; commandtimeout=" & cmdtime & "; Timeout=" & contime
        Else
          connectionstring = "Driver={PostgreSQL}; Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Database=" & db & "; Pwd=" & pass & "; commandtimeout=" & cmdtime & "; Timeout=" & contime
        End If

      ElseIf connector = "SQL Server 2012" Then
        If db = "" Then
          connectionstring = "Driver={SQL Server Native Client 11.0};Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Pwd=" & pass & "; commandtimeout=" & cmdtime & "; Timeout=" & contime
        Else
          connectionstring = "Driver={SQL Server Native Client 11.0};Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Database=" & db & "; Pwd=" & pass & "; commandtimeout=" & cmdtime & "; Timeout=" & contime
        End If
      End If

      DA.SetData(0, connectionstring)

    Catch ex As Exception

    End Try
  End Sub
#End Region

End Class
