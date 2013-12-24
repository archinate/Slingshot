Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System
Imports System.IO

Public Class SQLCOM_TruncateTable
  Inherits Grasshopper.Kernel.GH_Component

  Private _connector As String = "MySQL"

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Truncate Table", "TruncTable", "A SQL command string to truncate (delete) data in a SQL database table", "Slingshot!", "SQL")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{9ef16f7e-1814-472a-80a8-a976a0032742}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLCOM_Truncate
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Table Name", "Table", "Name of a table.", GH_ParamAccess.item)

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
    Dim table As String = Nothing

    DA.GetData(Of String)(0, table)

    If _connector = "MySQL" Then
      DA.SetData(0, "TRUNCATE " & table & ";")
    ElseIf _connector = "Oracle" Then
      DA.SetData(0, "TRUNCATE " & table & ";")
    ElseIf _connector = "PostgreSQL" Then
      DA.SetData(0, "TRUNCATE " & table & ";")
    ElseIf _connector = "SQL Server 2012" Then
      DA.SetData(0, "TRUNCATE " & table & ";")
    End If

  End Sub
#End Region

End Class