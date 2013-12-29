Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System

Public Class SQLCOM_Insert
  Inherits Grasshopper.Kernel.GH_Component

  Private _connector As String = "MySQL"

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Insert Command", "Insert", "A SQL command string to Insert (or Update) data into a SQL table.", "Slingshot!", "SQL")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{2c108841-28b4-4734-93c4-857361a36bb8}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLCOM_Insert
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)

    pManager.AddTextParameter("Table", "Table", "A database table.", GH_ParamAccess.item, "")
    pManager.AddTextParameter("Columns", "Columns", "A list of columns to write data to in a table.", GH_ParamAccess.list, "")
    pManager.AddTextParameter("Data", "Data", "A list of data to write.", GH_ParamAccess.list, "")
    pManager.AddBooleanParameter("Update", "Update", "Default is set to 'False' to use INSERT INTO method.  'True' uses UPDATE SET WHERE method to update columns based on a condition.  ", GH_ParamAccess.item, False)
    pManager.AddTextParameter("Where", "Where", "A list of conditions for which rows to update. Conditions should match the list of data to update.", GH_ParamAccess.list, "")

  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("SQL Code", "SQL", "Mesh SQL Code")
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

      Dim table As String = Nothing
      Dim column As New List(Of String)
      Dim data As New List(Of String)
      Dim update As Boolean = False
      Dim where As New List(Of String)

      DA.GetData(Of String)(0, table)
      DA.GetDataList(Of String)(1, column)
      DA.GetDataList(Of String)(2, data)
      DA.GetData(Of Boolean)(3, update)
      DA.GetDataList(Of String)(4, where)

      'create a column string
      Dim columnstring As String = column(0)
      If column.Count > 0 Then
        For i As Integer = 1 To column.Count - 1
          columnstring = columnstring & "," & column(i)
        Next
      End If

      Dim command As String
      Dim commandlist As New List(Of String)

      If update = True Then
        For i As Integer = 0 To data.Count - 1
          command = "UPDATE " & table & " SET " & "(" & columnstring & ")" & " = " & data(i) & " WHERE " & where(i)
          commandlist.Add(command)
        Next

      Else

        For i As Integer = 0 To data.Count - 1
          command = "INSERT INTO " & table & "(" & columnstring & ") VALUES (" & data(i) & ")"
          commandlist.Add(command)
        Next

      End If

      DA.SetDataList(0, commandlist)

    Catch ex As Exception

      DA.SetData(0, ex.ToString)

    End Try
  End Sub
#End Region

End Class