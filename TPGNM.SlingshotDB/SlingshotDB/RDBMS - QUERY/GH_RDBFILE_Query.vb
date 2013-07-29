Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System

Public Class GHRDBFILE_Query
  Inherits Grasshopper.Kernel.GH_Component

  Private _rdbms As String = "SQLite"
  Private _path As String = ""

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("RDB File Query", "Query", "Send a query to a Relational Database File", "Slingshot!", "RDBMS")
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
      Return New Guid("{a1b19301-1df9-4e12-8ead-712eee4f33c6}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHOLEDB_Query
    End Get
  End Property
#End Region

#Region "Menu Items"
  'Append Component menues.
  Public Overrides Function AppendMenuItems(menu As Windows.Forms.ToolStripDropDown) As Boolean

    Menu_AppendItem(menu, "Database Settings...", AddressOf Menu_Settings)

    Return True
  End Function

  'On menu item click...
  Private Sub Menu_Settings(ByVal sender As Object, ByVal e As EventArgs)

    'Open Settings dialogue
    Dim m_settingsdialogue As New form_FileSelect(_rdbms, _path)
    m_settingsdialogue.ShowDialog()
    _rdbms = m_settingsdialogue.DatabaseType
    _path = m_settingsdialogue.FilePath

    ExpireSolution(True)

  End Sub

  'GH Writer
  Public Overrides Function Write(writer As GH_IWriter) As Boolean
    writer.SetString("Database", _rdbms)
    writer.SetString("Path", _path)
    Return MyBase.Write(writer)
  End Function

  'GH Reader
  Public Overrides Function Read(reader As GH_IReader) As Boolean
    reader.TryGetString("Database", _rdbms)
    reader.TryGetString("Path", _path)
    Return MyBase.Read(reader)
  End Function
#End Region

#Region "Inputs/Outputs"

  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", False, GH_ParamAccess.item)
    pManager.AddTextParameter("SQL Query", "Query", "A SQL query.", GH_ParamAccess.item)
  End Sub

#End Region

#Region "Solution"

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Exceptions", "out", "Displays errors.")
    pManager.Register_GenericParam("Query Data Set", "DataSet", "A DataSet of Results")
  End Sub

  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim RDBMS As String = _rdbms
      Dim filepath As String = _path
      Dim connect As Boolean = False
      Dim query As String = Nothing

      DA.GetData(Of Boolean)(0, connect)
      DA.GetData(Of String)(1, query)

      If connect = True Then
        Dim sqlDataSet As DataSet = Nothing

        Dim dbcommand As New clsRDBMS()
        If RDBMS = "Access" Then
          sqlDataSet = dbcommand.AccessQuery(filepath, query)
        ElseIf RDBMS = "Excel" Then
          sqlDataSet = dbcommand.ExcelQuery(filepath, query)
        ElseIf RDBMS = "SQLite" Then
          sqlDataSet = dbcommand.SQLiteQuery(filepath, query)
        End If

        'Set Data lists to outputs
        DA.SetData(1, sqlDataSet)
      End If

    Catch ex As Exception
      DA.SetData(0, ex.ToString)
    End Try

  End Sub

#End Region

End Class
