Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System

Imports MySql

Public Class GHMYSQL_Command
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("MySQL Command", "MyCommand", "Send a command to a MySQL database", "Slingshot!", "RDBMS Connection")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{c635841d-ee80-4336-adf8-0742cde7aa09}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHMYSQL_Command

    End Get
  End Property
#End Region

#Region "Inputs/Outputs"

  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Connect String", "CString", "A MySQL connection string.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", False, GH_ParamAccess.item)
    pManager.AddTextParameter("Command", "Command", "A MySQL command.", GH_ParamAccess.item)

  End Sub

#End Region

#Region "Solution"

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Exceptions", "out", "Displays errors.")
  End Sub

  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim cstring As String = Nothing
      Dim connect As Boolean = False
      Dim command As String = Nothing

      DA.GetData(Of String)(0, cstring)
      DA.GetData(Of Boolean)(1, connect)
      DA.GetData(Of String)(2, command)

      If connect = True Then
        'Establish MySQL Database Connection
        Dim ConnectionString As String = cstring
        Dim mysqlConnect As MySql.Data.MySqlClient.MySqlConnection = New MySql.Data.MySqlClient.MySqlConnection(ConnectionString)
        Dim mysqldata As New MySql.Data.MySqlClient.MySqlDataAdapter
        mysqlConnect.Open()

        'Insert Command
        mysqldata.InsertCommand = New MySql.Data.MySqlClient.MySqlCommand(command, mysqlConnect)
        mysqldata.InsertCommand.ExecuteNonQuery()

        'Close the connection
        mysqlConnect.Close()

        'Display success
        DA.SetData(0, "Command executed!")
      End If

    Catch ex As Exception
      DA.SetData(0, ex.ToString)
    End Try

  End Sub

#End Region

End Class