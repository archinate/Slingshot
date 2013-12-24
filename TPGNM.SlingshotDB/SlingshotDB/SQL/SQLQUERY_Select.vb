Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System
Imports System.IO

Public Class SQLQUERY_Select
  Inherits Grasshopper.Kernel.GH_Component

  Private _connector As String = "MySQL"

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL SELECT String", "QuerySFW", "Formats a SQL SELECT FROM WHERE query string.", "Slingshot!", "SQL")
  End Sub

  'Exposure parameter (line dividers)
  Public Overrides ReadOnly Property Exposure As Grasshopper.Kernel.GH_Exposure
    Get
      Return GH_Exposure.secondary
    End Get
  End Property

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{4427fece-5ba5-4f97-af4d-32724260845a}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLQUERY_Select
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Select", "Select", "Select Columns", GH_ParamAccess.item, "")
    pManager.AddTextParameter("From", "From", "From Table", GH_ParamAccess.item, "")
    pManager.AddTextParameter("Where", "Where", "Where some condition (optional)", GH_ParamAccess.item, "")
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("Query String", "QString", "A SQL SELECT FROM WHERE query string.")
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
    Try
      Dim _select As String = Nothing
      Dim _from As String = Nothing
      Dim _where As String = Nothing

      DA.GetData(Of String)(0, _select)
      DA.GetData(Of String)(1, _from)
      DA.GetData(Of String)(2, _where)

      Dim QString As String = Nothing

      If _where = "" Then
        QString = "SELECT " & _select & " FROM " & _from
      Else
        QString = "SELECT " & _select & " FROM " & _from & " WHERE " & _where
      End If

      DA.SetData(0, QString)

    Catch ex As Exception

    End Try
  End Sub
#End Region

End Class
