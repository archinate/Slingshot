Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System
Imports System.IO


Public Class SQLQUERY_ColumnList
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Column List", "ColumnList", "A SQL query string to get a list of available columns.", "Slingshot!", "SQL")
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
      Return New Guid("{dbe20b6c-6a5b-45b6-9089-f7eb7f7aaafc}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLQUERY_ColumnList
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Database Table", "Table", "Name of a SQL table.", GH_ParamAccess.item)

  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("SQL Code", "SQL", "SQL Code")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)

    Dim table As String = Nothing

    DA.GetData(Of String)(0, table)

    Dim query As String = "SHOW columns FROM " & table

    DA.SetData(0, query)

  End Sub
#End Region

End Class