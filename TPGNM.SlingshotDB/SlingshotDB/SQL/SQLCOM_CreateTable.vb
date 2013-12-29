Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System

Public Class SQLCOM_CreateTable
  Inherits Grasshopper.Kernel.GH_Component

  Private _connector As String = "MySQL"

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Create Table", "CreateTB", "A SQL command string used to create a table.", "Slingshot!", "SQL")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{2c2e9920-6e2e-40de-8f7f-308068b97e7d}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLCOM_CreateTable
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Table Name", "Table", "Name of the table to create.", GH_ParamAccess.item)
    pManager.AddTextParameter("Columns", "Columns", "A list of columns.", GH_ParamAccess.list, "id INT NOT NULL AUTO_INCREMENT, PRIMARY KEY(id)")
    pManager.AddBooleanParameter("Drop Table", "Drop?", "Set to 'True' to drop the table.", GH_ParamAccess.item, False)
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
    Dim columns As New List(Of String)
    Dim drop As Boolean = False

    DA.GetData(Of String)(0, table)
    DA.GetDataList(Of String)(1, columns)
    DA.GetData(Of Boolean)(2, drop)

    Dim columnstring As String = columns(0)
    If columns.Count > 1 Then
      For i As Integer = 1 To columns.Count - 1
        columnstring = columnstring & "," & columns(i)
      Next
    End If

    Dim query As String
    If drop = True Then
      query = "DROP TABLE IF EXISTS " & table & ";"
    Else
      query = "CREATE TABLE IF NOT EXISTS " & table & "(" & columnstring & ");"
    End If
    DA.SetData(0, query)

  End Sub
#End Region

End Class
