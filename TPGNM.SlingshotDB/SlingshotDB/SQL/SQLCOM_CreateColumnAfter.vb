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

Public Class SQLCOM_CreateColumnAfter
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Create Column After", "ColumnAfter", "A SQL command used to create a column after a specified column in a table.", "Slingshot!", "SQL")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{814d862e-3274-46c4-8ffa-9dd98606b7de}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLCOM_CreateColumnAfter

    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Table Name", "Table", "Name of the table.", GH_ParamAccess.item)
    pManager.AddTextParameter("Column Name", "Column", "Name of the database to create.", GH_ParamAccess.item)
    pManager.AddTextParameter("Column Params", "Params", "The parameters of the column.", GH_ParamAccess.item)
    pManager.AddTextParameter("After Column", "After", "The new column will be placed after this column", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Drop Database", "Drop?", "Set to 'True' to drop the database.", GH_ParamAccess.item, False)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("SQL Code", "SQL", "SQL Code")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Dim table As String = Nothing
    Dim column As String = Nothing
    Dim param As String = Nothing
    Dim after As String = Nothing
    Dim drop As Boolean = False

    DA.GetData(Of String)(0, table)
    DA.GetData(Of String)(1, column)
    DA.GetData(Of String)(2, param)
    DA.GetData(Of String)(3, after)
    DA.GetData(Of Boolean)(4, drop)

    Dim query As String
    If drop = True Then
      query = "ALTER TABLE " & table & " DROP " & column & ";"
    Else
      query = "ALTER TABLE " & table & " ADD " & param & " AFTER " & after & ";"
    End If

    DA.SetData(0, query)

  End Sub
#End Region

End Class