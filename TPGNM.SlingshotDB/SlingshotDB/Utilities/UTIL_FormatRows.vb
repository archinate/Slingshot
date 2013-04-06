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

Public Class UTIL_FormatRows
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"

  'Methods
  Public Sub New()
    MyBase.New("SQL Format Rows", "SQLFormatRows", "Formats data into rows for a SQL table.", "Slingshot!", "Utility")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{65691bd7-37b8-4694-bd21-9f4601063309}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLUTIL_FormatRows

    End Get
  End Property

#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Row Data List", "RowData", "A list of data to be formated into a row string", GH_ParamAccess.list, "")
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("Row Strings", "RowStrings", "A list of row strings.")
  End Sub

#End Region

#Region "Solution"

  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim rowdata As New List(Of String)
      DA.GetDataList(Of String)(0, rowdata)

      Dim mystring As String = "'" & rowdata(0) & "'"

      For i As Integer = 1 To rowdata.Count - 1
        mystring = mystring & "," & "'" & rowdata(i) & "'"
      Next

      DA.SetData(0, mystring)
    Catch ex As Exception

    End Try
  End Sub

#End Region

End Class