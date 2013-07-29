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

Public Class SQLCOM2_CreateMeshDB
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Create Mesh Database", "MeshDB", "Create a special database for storing mesh data.", "Slingshot!", "SQL")
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
      Return New Guid("{ea4067e2-b1bb-42e2-9eaa-172e2b41d37d}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLUTIL_CreateMeshDB
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Database Name", "Database", "Name of the mesh database.", GH_ParamAccess.item)
    pManager.AddTextParameter("Mesh Name", "MshName", "Name of the mesh.", GH_ParamAccess.item)
    pManager.AddMeshParameter("Mesh", "Mesh", "A mesh.", GH_ParamAccess.item)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("SQL Code", "SQL", "SQL Code")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Dim database As String = Nothing
    Dim name As String = Nothing
    Dim mymesh As Mesh = Nothing

    DA.GetData(Of String)(0, database)
    DA.GetData(Of String)(1, name)
    DA.GetData(Of Mesh)(2, mymesh)

    Dim vertices As MeshVertexList
    Dim faces As MeshFaceList


    vertices = mymesh.Vertices
    faces = mymesh.Faces


    Dim dbcommand As New List(Of String)
    Dim createDb As String = "CREATE DATABASE IF NOT EXISTS " & database & ";"
    Dim createVertTb As String = "CREATE TABLE IF NOT EXISTS " & database & "." & name & "_vertices (id Int NOT NULL AUTO_INCREMENT,PRIMARY KEY(id),x Float,y Float,z Float);"
    Dim createFaceTb As String = "CREATE TABLE IF NOT EXISTS " & database & "." & name & "_faces (id Int NOT NULL AUTO_INCREMENT,PRIMARY KEY(id),v_number Int,v1 Int,v2 Int,v3 Int,v4 Int);"
    Dim truncVerts As String = "TRUNCATE " & database & "." & name & "_vertices;"
    Dim truncFaces As String = "TRUNCATE " & database & "." & name & "_faces;"
    dbcommand.Add(createDb)
    dbcommand.Add(createVertTb)
    dbcommand.Add(createFaceTb)
    dbcommand.Add(truncVerts)
    dbcommand.Add(truncFaces)

    'insert vertices

    For i As Integer = 0 To vertices.Count - 1
      Dim x As Double = vertices(i).X
      Dim y As Double = vertices(i).Y
      Dim z As Double = vertices(i).Z
      Dim dbcom As String = "INSERT INTO " & database & "." & name & "_vertices(x,y,z) VALUES (" & x & "," & y & "," & z & ");"
      dbcommand.Add(dbcom)

    Next

    'insert faces

    For j As Integer = 0 To faces.Count - 1
      If faces(j).IsQuad = True Then
        Dim v1 As Integer = faces(j).A + 1
        Dim v2 As Integer = faces(j).B + 1
        Dim v3 As Integer = faces(j).C + 1
        Dim v4 As Integer = faces(j).D + 1
        Dim dbcom As String = "INSERT INTO " & database & "." & name & "_faces(v_number,v1,v2,v3,v4) VALUES (4," & v1 & "," & v2 & "," & v3 & "," & v4 & ");"
        dbcommand.Add(dbcom)
      Else
        Dim v1 As Integer = faces(j).A + 1
        Dim v2 As Integer = faces(j).B + 1
        Dim v3 As Integer = faces(j).C + 1
        Dim dbcom As String = "INSERT INTO " & database & "." & name & "_faces(v_number,v1,v2,v3) VALUES (4," & v1 & "," & v2 & "," & v3 & ");"
        dbcommand.Add(dbcom)

      End If
    Next

    DA.SetDataList(0, dbcommand)

  End Sub
#End Region

End Class