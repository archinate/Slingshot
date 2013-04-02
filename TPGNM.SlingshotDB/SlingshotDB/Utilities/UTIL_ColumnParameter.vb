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

Public Class UTIL_ColumnParameter
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"

  'Methods
  Public Sub New()
    MyBase.New("SQL Column Parameters", "ColumnParam", "Formats a SQL column parameter string.", "Slingshot!", "Utility")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{8de09236-53b4-4b52-bde7-e623ff2690b2}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLUTIL_ColumnParameter

    End Get
  End Property

#End Region

#Region "Inputs/Outputs"

  'Input parameters
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Column Name", "Name", "The name of the column", GH_ParamAccess.item, "id")
    pManager.AddTextParameter("Data Type", "Data", "Type of column data (Int, Char, etc.)", GH_ParamAccess.item, "Int")
    pManager.AddBooleanParameter("Primary Key", "PK", "Sets the column as a 'Primary Key'.", GH_ParamAccess.item, False)
    pManager.AddBooleanParameter("Not Null", "NN", "Sets column as 'Not Null'.", GH_ParamAccess.item, False)
    pManager.AddBooleanParameter("Auto Increment", "AI", "Sets column to 'Auto Increment'.", GH_ParamAccess.item, False)
  End Sub

  'Output parameters
  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("Column Parameter String", "CParam", "A string for setting column parameters.")
  End Sub

#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim name As String = Nothing
      Dim data As String = Nothing
      Dim pk As Boolean = False
      Dim nn As Boolean = False
      Dim ai As Boolean = False

      DA.GetData(Of String)(0, name)
      DA.GetData(Of String)(1, data)
      DA.GetData(Of Boolean)(2, pk)
      DA.GetData(Of Boolean)(3, nn)
      DA.GetData(Of Boolean)(4, ai)

      Dim sqlColumn As String = name & " " & data

      If nn = True Then
        sqlColumn = sqlColumn & " NOT NULL"
      End If

      If ai = True Then
        sqlColumn = sqlColumn & " AUTO_INCREMENT"
      End If

      If pk = True Then
        sqlColumn = sqlColumn & ", PRIMARY KEY(" & name & ")"
      End If

      DA.SetData(0, sqlColumn)

    Catch ex As Exception

    End Try
  End Sub

#End Region

End Class