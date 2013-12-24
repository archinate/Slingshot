Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System


Public Class SQLCOM_CreateDatabase
  Inherits Grasshopper.Kernel.GH_Component

  Private _connector As String = "MySQL"

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Create Database", "CreateDB", "A SQL command string used to create a database.", "Slingshot!", "SQL")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{2554d790-eec8-4ff7-9634-aaabf769265d}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLCOM_CreateDatabase
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Database Name", "Database", "Name of the database to create.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Drop Database", "Drop?", "Set to 'True' to drop the database.", GH_ParamAccess.item, False)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("SQL Code", "SQL", "SQL Code")
  End Sub
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
    Dim m_settingsdialogue As New form_DBSelect(_connector)
    m_settingsdialogue.ShowDialog()
    _connector = m_settingsdialogue.Connector

    ExpireSolution(True)

  End Sub

  'GH Writer
  Public Overrides Function Write(writer As GH_IWriter) As Boolean
    writer.SetString("Connector", _connector)
    Return MyBase.Write(writer)
  End Function

  'GH Reader
  Public Overrides Function Read(reader As GH_IReader) As Boolean
    reader.TryGetString("Connector", _connector)
    Return MyBase.Read(reader)
  End Function
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Dim database As String = Nothing
    Dim drop As Boolean = False

    DA.GetData(Of String)(0, database)
    DA.GetData(Of Boolean)(1, drop)

    Dim query As String = Nothing

    If _connector = "MySQL" Then
      If drop = True Then
        query = "DROP DATABASE IF EXISTS " & database & ";"
      Else
        query = "CREATE DATABASE IF NOT EXISTS " & database & ";"
      End If
    ElseIf _connector = "Oracle" Then
      If drop = True Then
        query = "DROP DATABASE IF EXISTS " & database & ";"
      Else
        query = "CREATE DATABASE IF NOT EXISTS " & database & ";"
      End If
    ElseIf _connector = "PostgreSQL" Then
        If drop = True Then
          query = "DROP DATABASE IF EXISTS " & database & ";"
        Else
          query = "CREATE DATABASE IF NOT EXISTS " & database & ";"
        End If
    ElseIf _connector = "SQL Server 2012" Then
        If drop = True Then
          query = "IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'" & database & "') DROP DATABASE " & database & ";"
        Else
          query = "IF NOT EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'" & database & "') CREATE DATABASE " & database & ";"
        End If
      End If


      DA.SetData(0, query)

  End Sub
#End Region

End Class