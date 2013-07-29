Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System

Imports System.Data.SQLite

Public Class GHSQLite_Query
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQLite Query", "LiteQuery", "Query a SQLite database file (*.s3db)", "Slingshot!", "RDBMS Connection")
  End Sub

  'Exposure parameter (line dividers)
  Public Overrides ReadOnly Property Exposure As Grasshopper.Kernel.GH_Exposure
    Get
      Return GH_Exposure.quarternary
    End Get
  End Property

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{062d223b-74e1-431c-b532-60891a31995b}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHSQLite_Query
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddBooleanParameter("Create Toggle", "CToggle", "Set to 'True' to create the *.s3db database file.", GH_ParamAccess.item, False)
    pManager.AddTextParameter("Directory Path", "Directory", "The directory for the SQLite database file.", GH_ParamAccess.item)
    pManager.AddTextParameter("Database", "Database", "The name of the database file.", GH_ParamAccess.item)
    pManager.AddTextParameter("Query", "Query", "A SQLite Query.", GH_ParamAccess.item)
    pManager.AddIntegerParameter("Column Number", "Column", "The column number to output.", GH_ParamAccess.item, 0)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Exceptions", "out", "Displays errors.")
    pManager.Register_GenericParam("Column Query Result", "CResult", "Results in a specific column")
    pManager.Register_GenericParam("Query Result", "QResult", "Full result of a query.  Columns separated by commas.")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try

      Dim ctoggle As Boolean = False
      Dim path As String = Nothing
      Dim database As String = Nothing
      Dim query As String = Nothing
      Dim column As Integer = Nothing

      DA.GetData(Of Boolean)(0, ctoggle)
      DA.GetData(Of String)(1, path)
      DA.GetData(Of String)(2, database)
      DA.GetData(Of String)(3, query)
      DA.GetData(Of Integer)(4, column)

      If ctoggle = True Then
        Dim filepath As String = path + "\" + database + ".s3db"

        'Connect to SQLite
        Dim SQLConnect As New SQLite.SQLiteConnection()
        Dim SQLCommand As SQLiteCommand

        Dim DataList As New List(Of String)
        Dim ColumnSel As New List(Of Object)

        SQLConnect.ConnectionString = "Data Source=" & filepath
        SQLConnect.Open()
        SQLCommand = SQLConnect.CreateCommand
        SQLCommand.CommandText = query

        Dim SQLReader As SQLiteDataReader = SQLCommand.ExecuteReader()

        While SQLReader.Read()
          Dim RowString As String = SQLReader(0)
          For i As Integer = 1 To SQLReader.FieldCount - 1
            RowString = RowString & "," & SQLReader(i)
          Next
          DataList.Add(RowString)
          ColumnSel.Add(SQLReader(column))
        End While

        SQLCommand.Dispose()
        SQLConnect.Close()

        'Set lists to outputs
        DA.SetDataList(1, ColumnSel)
        DA.SetDataList(2, DataList)

      End If

    Catch ex As Exception

      DA.SetData(0, ex.ToString)

    End Try
  End Sub
#End Region

End Class