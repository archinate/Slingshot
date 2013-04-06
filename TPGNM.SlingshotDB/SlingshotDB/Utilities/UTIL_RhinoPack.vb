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

Imports System.Data.SQLite

Public Class UTIL_RhinoPack
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"

  'Methods
  Public Sub New()
    MyBase.New("Rhino Pack", "Pack", "Packages serialized Rhino geometry into a SQLite database file.", "Slingshot!", "Utility")
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
      Return New Guid("{6854901a-d5a7-41eb-9625-1cff78438d74}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.UTIL_RhinoPack
    End Get
  End Property

#End Region

#Region "Inputs/Outputs"

  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddBooleanParameter("Create Toggle", "CToggle", "Set to 'True' to create the *.s3db database file.", GH_ParamAccess.item, False)
    pManager.AddTextParameter("Directory Path", "Directory", "The directory for the SQLite database file.", GH_ParamAccess.item)
    pManager.AddTextParameter("Database", "Database", "The name of the database file.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Truncate Tables", "Truncate", "Truncate tables in file before filling?", GH_ParamAccess.item, False)
    pManager.AddGenericParameter("Geometry Objects", "Objects", "Geometry to serialize. Supports Points, Curves, Surfaces, BReps, and Meshes).", GH_ParamAccess.list)

  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Errors and Exceptions", "out", "Displays messages, errors, and exceptions.")
  End Sub

#End Region

#Region "Solution"

  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)

    Dim ctoggle As Boolean = False
    Dim path As String = Nothing
    Dim database As String = Nothing
    Dim truncate As Boolean = False
    Dim objList As New List(Of Object)

    DA.GetData(Of Boolean)(0, ctoggle)
    DA.GetData(Of String)(1, path)
    DA.GetData(Of String)(2, database)
    DA.GetData(Of Boolean)(3, truncate)
    DA.GetDataList(Of Object)(4, objList)

    If ctoggle = True Then

      Try
        'serialized string lists
        Dim pointXml As New List(Of String)
        Dim curveXml As New List(Of String)
        Dim surfaceXml As New List(Of String)
        Dim brepXml As New List(Of String)
        Dim meshXml As New List(Of String)

        Dim obj As Object
        For Each obj In objList

          'check geometry type
          If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Point" Then
            Dim xml As String
            Dim myobj As Kernel.Types.GH_Point = obj
            Dim mypoint As Point3d = myobj.Value
            Dim pointObj As New Kernel.Types.GH_Point(mypoint)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Point")
            pointObj.Write(chunk)

            xml = chunk.Serialize_Xml()
            pointXml.Add(xml)
          End If

          If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Brep" Then
            Dim xml As String
            Dim myobj As Kernel.Types.GH_Brep = obj
            Dim mybrep As Brep = myobj.Value
            Dim brepObj As New Kernel.Types.GH_Brep(mybrep)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Brep")
            brepObj.Write(chunk)

            xml = chunk.Serialize_Xml()
            brepXml.Add(xml)
          End If

          If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Curve" Then
            Dim xml As String
            Dim myobj As Kernel.Types.GH_Curve = obj
            Dim mycurve As Curve = myobj.Value
            Dim curveObj As New Kernel.Types.GH_Curve(mycurve)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Curve")
            curveObj.Write(chunk)

            xml = chunk.Serialize_Xml()
            curveXml.Add(xml)
          End If

          If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Surface" Then
            Dim xml As String
            Dim myobj As Kernel.Types.GH_Surface = obj
            Dim mysurf As Brep = myobj.Value
            Dim surfaceObj As New Kernel.Types.GH_Brep(mysurf)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Surface")
            surfaceObj.Write(chunk)

            xml = chunk.Serialize_Xml()
            surfaceXml.Add(xml)
          End If

          If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Mesh" Then
            Dim xml As String
            Dim myobj As Kernel.Types.GH_Mesh = obj
            Dim mymesh As Mesh = myobj.Value
            Dim meshObj As New Kernel.Types.GH_Mesh(mymesh)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Mesh")
            meshObj.Write(chunk)

            xml = chunk.Serialize_Xml()
            meshXml.Add(xml)
          End If

        Next

        'Write lists of serialized geometry to SQLite File

        'write points
        'Create directory if it does not already exist
        If (Not System.IO.Directory.Exists(path)) Then
          System.IO.Directory.CreateDirectory(path)
        End If

        'Create the database file
        Dim filepath As String = path + "\" + database + ".s3db"
        Dim SQLConnect As New SQLite.SQLiteConnection()
        SQLConnect.ConnectionString = "Data Source=" & filepath
        Dim SQLCommand As SQLiteCommand

        SQLConnect.Open()

        'Table creation strings
        Dim pointsTb As String = "CREATE TABLE IF NOT EXISTS points(id INTEGER PRIMARY KEY, object TEXT);"
        Dim curvesTb As String = "CREATE TABLE IF NOT EXISTS curves(id INTEGER PRIMARY KEY, object TEXT);"
        Dim surfacesTb As String = "CREATE TABLE IF NOT EXISTS surfaces(id INTEGER PRIMARY KEY, object TEXT);"
        Dim brepsTb As String = "CREATE TABLE IF NOT EXISTS breps(id INTEGER PRIMARY KEY, object TEXT);"
        Dim meshesTb As String = "CREATE TABLE IF NOT EXISTS meshes(id INTEGER PRIMARY KEY, object TEXT);"

        SQLCommand = SQLConnect.CreateCommand

        'create points table
        SQLCommand.CommandText = pointsTb
        SQLCommand.ExecuteNonQuery()

        'create curves table
        SQLCommand.CommandText = curvesTb
        SQLCommand.ExecuteNonQuery()

        'create surfaces table
        SQLCommand.CommandText = surfacesTb
        SQLCommand.ExecuteNonQuery()

        'create breps table
        SQLCommand.CommandText = brepsTb
        SQLCommand.ExecuteNonQuery()

        'create meshes table
        SQLCommand.CommandText = meshesTb
        SQLCommand.ExecuteNonQuery()


        'truncate tables (clear data)
        If truncate = True Then
          Dim truncPts As String = "DELETE FROM points"
          Dim truncCrv As String = "DELETE FROM curves"
          Dim truncSrf As String = "DELETE FROM surfaces"
          Dim truncBrep As String = "DELETE FROM breps"
          Dim truncMsh As String = "DELETE FROM meshes"

          'truncate points table
          SQLCommand.CommandText = truncPts
          SQLCommand.ExecuteNonQuery()


          'truncate curves table
          SQLCommand.CommandText = truncCrv
          SQLCommand.ExecuteNonQuery()


          'truncate surfaces table
          SQLCommand.CommandText = truncSrf
          SQLCommand.ExecuteNonQuery()


          'truncate breps table
          SQLCommand.CommandText = truncBrep
          SQLCommand.ExecuteNonQuery()


          'truncate meshes table
          SQLCommand.CommandText = truncMsh
          SQLCommand.ExecuteNonQuery()

        End If


        Dim str As String

        'fill points table
        For Each str In pointXml
          Dim insertStr As String = "INSERT INTO points(object) VALUES(" & "'" & str & "'" & ");"
          SQLCommand.CommandText = insertStr
          SQLCommand.ExecuteNonQuery()

        Next

        'fill curves table
        For Each str In curveXml
          Dim insertStr As String = "INSERT INTO curves(object) VALUES(" & "'" & str & "'" & ");"
          SQLCommand.CommandText = insertStr
          SQLCommand.ExecuteNonQuery()

        Next

        'fill surfaces table
        For Each str In surfaceXml
          Dim insertStr As String = "INSERT INTO surfaces(object) VALUES(" & "'" & str & "'" & ");"
          SQLCommand.CommandText = insertStr
          SQLCommand.ExecuteNonQuery()

        Next

        'fill breps table
        For Each str In brepXml
          Dim insertStr As String = "INSERT INTO breps(object) VALUES(" & "'" & str & "'" & ");"
          SQLCommand.CommandText = insertStr
          SQLCommand.ExecuteNonQuery()

        Next

        'fill meshes table
        For Each str In meshXml
          Dim insertStr As String = "INSERT INTO meshes(object) VALUES(" & "'" & str & "'" & ");"
          SQLCommand.CommandText = insertStr
          SQLCommand.ExecuteNonQuery()

        Next

        SQLCommand.Dispose()
        SQLConnect.Close()

      Catch ex As Exception
        DA.SetData(0, ex.ToString)
      End Try
    End If
  End Sub

#End Region

End Class
