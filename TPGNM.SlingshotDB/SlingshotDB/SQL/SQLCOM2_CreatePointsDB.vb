Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports Rhino
Imports Rhino.Geometry
Imports Rhino.Geometry.Collections

Imports System
Imports System.IO

Public Class SQLCOM2_CreatePointsDB
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Create Point Database", "PtsDB", "Create a special database for storing point data.", "Slingshot!", "SQL")
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
      Return New Guid("{e67b01a1-d79d-4267-b1d6-835b44d96fd4}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLUTIL_CreatePtsDB
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Database Name", "Database", "Name of the mesh database.", GH_ParamAccess.item)
    pManager.AddTextParameter("Points Table Name", "PtsName", "Name of the points table.", GH_ParamAccess.item)
    pManager.AddPointParameter("Points", "Points", "A list of points.", GH_ParamAccess.list)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("SQL Code", "SQL", "SQL Code")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Dim database As String = Nothing
    Dim name As String = Nothing
    Dim pts As New List(Of Point3d)

    DA.GetData(Of String)(0, database)
    DA.GetData(Of String)(1, name)
    DA.GetDataList(Of Point3d)(2, pts)

    Dim dbcommand As New List(Of String)
    Dim createDb As String = "CREATE DATABASE IF NOT EXISTS " & database & ";"
    Dim createPtTb As String = "CREATE TABLE IF NOT EXISTS " & database & "." & name & "_points (id Int NOT NULL AUTO_INCREMENT,PRIMARY KEY(id),x Float,y Float,z Float);"
    Dim truncVerts As String = "TRUNCATE " & database & "." & name & "_points;"

    dbcommand.Add(createDb)
    dbcommand.Add(createPtTb)
    dbcommand.Add(truncVerts)

    'insert points

    For i As Integer = 0 To pts.Count - 1

      Dim x As Double = pts(i).X
      Dim y As Double = pts(i).Y
      Dim z As Double = pts(i).Z
      Dim dbcom As String = "INSERT INTO " & database & "." & name & "_points(x,y,z) VALUES (" & x & "," & y & "," & z & ");"
      dbcommand.Add(dbcom)

    Next


    DA.SetDataList(0, dbcommand)

  End Sub
#End Region

End Class