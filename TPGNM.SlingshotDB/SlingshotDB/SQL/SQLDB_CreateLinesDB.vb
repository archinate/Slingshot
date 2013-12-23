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

Public Class SQLDB_CreateLinesDB
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Create Line Database", "LineDB", "Create a special database for storing line data.", "Slingshot!", "SQL")
  End Sub

  'Exposure
  Public Overrides ReadOnly Property Exposure As Grasshopper.Kernel.GH_Exposure
    Get
      Return GH_Exposure.tertiary
    End Get
  End Property

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{c5d0ef32-55df-4788-b86c-71b808f9482c}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLUTIL_CreateLineDb
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Database Name", "Database", "Name of the lines database.", GH_ParamAccess.item)
    pManager.AddTextParameter("Line Table Name", "LnName", "Name of the lines table.", GH_ParamAccess.item)
    pManager.AddLineParameter("Lines", "Lines", "A list of lines.", GH_ParamAccess.list)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("SQL Code", "SQL", "SQL Code")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Dim database As String = Nothing
    Dim name As String = Nothing
    Dim lines As New List(Of Line)

    DA.GetData(Of String)(0, database)
    DA.GetData(Of String)(1, name)
    DA.GetDataList(Of Line)(2, lines)

    Dim dbcommand As New List(Of String)
    Dim createDb As String = "CREATE DATABASE IF NOT EXISTS " & database & ";"
    Dim createLnTb As String = "CREATE TABLE IF NOT EXISTS " & database & "." & name & "_lines (id Int NOT NULL AUTO_INCREMENT,PRIMARY KEY(id),x1 Float,y1 Float,z1 Float,x2 Float,y2 Float,z2 Float;"
    Dim truncLns As String = "TRUNCATE " & database & "." & name & "_lines;"

    dbcommand.Add(createDb)
    dbcommand.Add(createLnTb)
    dbcommand.Add(truncLns)

    'insert points

    For i As Integer = 0 To lines.Count - 1
      Dim stpt As Point3d = lines(i).From
      Dim enpt As Point3d = lines(i).To

      Dim x1 As Double = stpt.X
      Dim y1 As Double = stpt.Y
      Dim z1 As Double = stpt.Z
      Dim x2 As Double = enpt.X
      Dim y2 As Double = enpt.Y
      Dim z2 As Double = enpt.Z
      Dim dbcom As String = "INSERT INTO " & database & "." & name & "_lines(x1,y1,z1,x2,y2,z2) VALUES (" & x1 & "," & y1 & "," & z1 & "," & x2 & "," & y2 & "," & z2 & ");"
      dbcommand.Add(dbcom)

    Next

    DA.SetDataList(0, dbcommand)

  End Sub
#End Region

End Class
