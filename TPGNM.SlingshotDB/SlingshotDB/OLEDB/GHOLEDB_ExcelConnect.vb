Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types

Imports GH_IO
Imports GH_IO.Serialization

Imports System

Public Class GHMYSQL_ExcelConnect
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("Excel Connection String", "ExcelString", "Formats an Excel connection string.", "Slingshot!", "RDBMS Connection")
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
      Return New Guid("{e7d8d513-4b48-49c6-80e4-ec4c03eb581b}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHOLEDB_ExcelConnect
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("File Path", "Path", "Location of the Excel File", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Table Headers", "Header", "Use first row of sheet is a table header.", GH_ParamAccess.item, False)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("Connection String", "CString", "An Excel connection string.")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim xlpath As String = Nothing
      Dim xlhdr As Boolean = Nothing

      DA.GetData(Of String)(0, xlpath)
      DA.GetData(Of Boolean)(1, xlhdr)

      If xlhdr = True Then
        Dim connectionstring As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & xlpath & ";Extended Properties=""Excel 12.0;HDR=Yes"""
        DA.SetData(0, connectionstring)
      Else
        Dim connectionstring As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & xlpath & ";Extended Properties=""Excel 12.0;HDR=No"""
        DA.SetData(0, connectionstring)
      End If

    Catch ex As Exception

    End Try
  End Sub
#End Region

End Class