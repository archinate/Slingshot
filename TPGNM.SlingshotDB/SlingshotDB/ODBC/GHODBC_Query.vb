Imports Rhino
Imports Rhino.Geometry
Imports Rhino.Collections

Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types

Imports GH_IO
Imports GH_IO.Serialization

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

Imports System.Data.Odbc

Public Class GHODBC_QUERY
    Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("ODBC Query", "ODBC Query", "Query a database using ODBC.", "Slingshot!", "RDBMS Connection")
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
      Return New Guid("{2a953e09-5178-449d-b49d-ebd6135499dc}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHODBC_Query

    End Get
  End Property
#End Region


#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Connect String", "CString", "A database connection string.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", GH_ParamAccess.item, False)
    pManager.AddTextParameter("Query", "Query", "A database query.", GH_ParamAccess.item)
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

        Dim dbConnect As OdbcConnection = New OdbcConnection(cstring)
        dbConnect.Open()

        Dim dbdata As New OdbcDataAdapter(query, dbConnect)

        dbdata.Fill(sqlDataSet, "result")
        dbConnect.Close()


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