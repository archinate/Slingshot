Imports Rhino
Imports Rhino.Geometry
Imports Rhino.Collections

Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types

Imports GH_IO
Imports GH_IO.Serialization

Imports MySql

Imports System
Imports System.IO
Imports System.Xml
Imports System.Xml.Linq
Imports System.Linq
Imports System.Data
Imports System.Drawing
Imports System.Reflection
Imports System.Collections
Imports System.Windows.Forms
Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Runtime.InteropServices

Public Class GHMYSQL_Query
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("MySQL Query", "MyQuery", "Query MySQL.", "Slingshot!", "RDBMS Connection")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{e15c4fa8-263a-46b3-b418-4b39f92ea82b}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHMySQL_Query

    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Connect String", "CString", "A MySQL connection string.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", False, GH_ParamAccess.item)
    pManager.AddTextParameter("MySQL Query", "Query", "A MySQL query.", GH_ParamAccess.item)
    pManager.AddIntegerParameter("Column Number", "Column", "The MySQL column number to output.", 0, GH_ParamAccess.item)
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
      Dim cstring As String = Nothing
      Dim connect As Boolean = False
      Dim query As String = Nothing
      Dim column As Integer = Nothing

      DA.GetData(Of String)(0, cstring)
      DA.GetData(Of Boolean)(1, connect)
      DA.GetData(Of String)(2, query)
      DA.GetData(Of Integer)(3, column)


      If connect = True Then
        Dim sqlDataSet As New DataSet()

        'Establish MySQL Database Connection

        Dim ConnectionString As String = cstring

        Dim mysqlConnect As MySql.Data.MySqlClient.MySqlConnection = New MySql.Data.MySqlClient.MySqlConnection(ConnectionString)
        mysqlConnect.Open()

        Dim mysqldata As New MySql.Data.MySqlClient.MySqlDataAdapter(query, mysqlConnect)

        mysqldata.Fill(sqlDataSet, "result")
        mysqlConnect.Close()


        Dim DataListA As New List(Of Object)
        For i As Integer = 0 To sqlDataSet.Tables(0).Rows.Count - 1
          DataListA.Add(sqlDataSet.Tables(0).Rows(i)(column))
        Next

        Dim DataListB As New List(Of Object)
        For i As Integer = 0 To sqlDataSet.Tables(0).Rows.Count - 1
          Dim rowString As String = sqlDataSet.Tables(0).Rows(i)(0)
          For j As Integer = 1 To sqlDataSet.Tables(0).Columns.Count - 1
            rowString = rowString & "," & sqlDataSet.Tables(0).Rows(i)(j)
          Next
          DataListB.Add(rowString)
        Next


        DA.SetDataList(1, DataListA)
        DA.SetDataList(2, DataListB)

      End If


    Catch ex As Exception

      DA.SetData(0, ex.ToString)

    End Try
  End Sub
#End Region

End Class