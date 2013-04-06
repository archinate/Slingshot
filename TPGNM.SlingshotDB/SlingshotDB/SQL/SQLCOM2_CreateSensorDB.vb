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

Public Class SQLCOM2_CreateSensorDB
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Create Sensor Database", "SensorDB", "Create a special database for storing node sensor data.", "Slingshot!", "SQL")
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
      Return New Guid("{c5dc8574-e8eb-47ce-967b-00bc48416234}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLUTIL_CreateSensorDb
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Database Name", "Database", "Name of the sensor database.", GH_ParamAccess.item)
    pManager.AddTextParameter("Sensor Table Name", "SensName", "Name of the sensor table.", GH_ParamAccess.item)
    pManager.AddPointParameter("Sensor Location", "Location", "A list of sensor locations (points).", GH_ParamAccess.list)
    pManager.AddVectorParameter("Sensor Direction", "Direction", "A list of sensor directions (vectors).", GH_ParamAccess.list)
    pManager.AddNumberParameter("Sensor Values", "Values", "A list of sensor values.", GH_ParamAccess.list)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("SQL Code", "SQL", "SQL Code")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Dim database As String = Nothing
    Dim name As String = Nothing
    Dim loc As New List(Of Point3d)
    Dim dir As New List(Of Vector3d)
    Dim val As New List(Of Double)

    DA.GetData(Of String)(0, database)
    DA.GetData(Of String)(1, name)
    DA.GetDataList(Of Point3d)(2, loc)
    DA.GetDataList(Of Vector3d)(3, dir)
    DA.GetDataList(Of Double)(4, val)

    Dim dbcommand As New List(Of String)
    Dim createDb As String = "CREATE DATABASE IF NOT EXISTS " & database & ";"
    Dim createSnsTb As String = "CREATE TABLE IF NOT EXISTS " & database & "." & name & "_sensors (id Int NOT NULL AUTO_INCREMENT,PRIMARY KEY(id),location Char(32), direction Char(32), value Float);"
    Dim truncSens As String = "TRUNCATE " & database & "." & name & "_sensors;"

    dbcommand.Add(createDb)
    dbcommand.Add(createSnsTb)
    dbcommand.Add(truncSens)

    'insert points

    For i As Integer = 0 To loc.Count - 1

      Dim x As Double = loc(i).X
      Dim y As Double = loc(i).Y
      Dim z As Double = loc(i).Z
      Dim dx As Double = dir(i).X
      Dim dy As Double = dir(i).Y
      Dim dz As Double = dir(i).Z

      Dim location As String = "'" & x & "," & y & "," & z & "'"
      Dim direction As String = "'" & dx & "," & dy & "," & dz & "'"

      Dim dbcom As String = "INSERT INTO " & database & "." & name & "_sensors (location,direction,value) VALUES (" & location & "," & direction & "," & val(i) & ");"
      dbcommand.Add(dbcom)

    Next


    DA.SetDataList(0, dbcommand)

  End Sub
#End Region

End Class
