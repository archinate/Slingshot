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


Public Class UTIL_RhinoUnpack
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"

  'Methods
  Public Sub New()
    MyBase.New("Rhino Unpack", "UnPack", "Unpacks serialized Rhino geometry from a SQLite database file.", "Slingshot!", "Utility")
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
      Return New Guid("{bc963eb7-895f-468b-b987-277b60f1456c}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.UTIL_RhinoUnpack
    End Get
  End Property

#End Region

#Region "Inputs/Outputs"

  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)

    pManager.AddTextParameter("Directory Path", "Directory", "The directory for the SQLite database file.", GH_ParamAccess.item)
    pManager.AddTextParameter("Database", "Database", "The name of the database file.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Deserialize Points", "Point", "Set to 'True' to deserialize points.", GH_ParamAccess.item, False)
    pManager.AddBooleanParameter("Deserialize Curves", "Curve", "Set to 'True' to deserialize curves.", GH_ParamAccess.item, False)
    pManager.AddBooleanParameter("Deserialize Surfaces", "Surface", "Set to 'True' to deserialize surfaces.", GH_ParamAccess.item, False)
    pManager.AddBooleanParameter("Deserialize BReps", "BRep", "Set to 'True' to deserialize breps.", GH_ParamAccess.item, False)
    pManager.AddBooleanParameter("Deserialize Meshes", "Mesh", "Set to 'True' to deserialize meshes.", GH_ParamAccess.item, False)

  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)

    pManager.Register_GenericParam("Errors and Exceptions", "out", "Displays messages, errors, and exceptions.")
    pManager.Register_PointParam("Points", "Points", "Deserialized Point objects.")
    pManager.Register_CurveParam("Curves", "Curves", "Deserialized Curve objects.")
    pManager.Register_SurfaceParam("Surfaces", "Surfaces", "Deserialized Surface objects.")
    pManager.Register_BRepParam("BReps", "BRep", "Deserialized Boundary representation objects.")
    pManager.Register_MeshParam("Meshes", "Meshes", "Deserialized Mesh objects.")
  End Sub

#End Region

#Region "Solution"

  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)

    Dim path As String = Nothing
    Dim database As String = Nothing
    Dim dePts As Boolean = False
    Dim deCrv As Boolean = False
    Dim deSrf As Boolean = False
    Dim deBrp As Boolean = False
    Dim deMsh As Boolean = False



    DA.GetData(Of String)(0, path)
    DA.GetData(Of String)(1, database)
    DA.GetData(Of Boolean)(2, dePts)
    DA.GetData(Of Boolean)(3, deCrv)
    DA.GetData(Of Boolean)(4, deSrf)
    DA.GetData(Of Boolean)(5, deBrp)
    DA.GetData(Of Boolean)(6, deMsh)


    Try

      'Query database and get serialized XML
      Dim xml As New List(Of String)
      Dim ghpaths As New List(Of String)
      Dim filepath As String = path + "\" + database + ".s3db"

      Dim SQLConnect As New SQLite.SQLiteConnection()
      Dim SQLCommand As SQLiteCommand

      Dim DataList As New List(Of String)
      Dim ColumnSel As New List(Of Object)

      SQLConnect.ConnectionString = "Data Source=" & filepath
      SQLConnect.Open()
      SQLCommand = SQLConnect.CreateCommand

      'get serialized XML strings for objects with 'true' toggles
      If dePts = True Then
        SQLCommand.CommandText = "SELECT ghpath,object FROM points;"
        Dim SQLReader As SQLiteDataReader = SQLCommand.ExecuteReader()
        While SQLReader.Read()
          Dim PathString As String = SQLReader(0)
          Dim RowString As String = SQLReader(1)
          xml.Add(RowString)
          ghpaths.Add(PathString)
        End While
        SQLReader.Close()
      End If


      If deCrv = True Then
        SQLCommand.CommandText = "SELECT ghpath,object FROM curves;"
        Dim SQLReader As SQLiteDataReader = SQLCommand.ExecuteReader()
        While SQLReader.Read()
          Dim PathString As String = SQLReader(0)
          Dim RowString As String = SQLReader(1)
          xml.Add(RowString)
          ghpaths.Add(PathString)
        End While
        SQLReader.Close()
      End If

      If deSrf = True Then
        SQLCommand.CommandText = "SELECT ghpath,object FROM surfaces;"
        Dim SQLReader As SQLiteDataReader = SQLCommand.ExecuteReader()
        While SQLReader.Read()
          Dim PathString As String = SQLReader(0)
          Dim RowString As String = SQLReader(1)
          xml.Add(RowString)
          ghpaths.Add(PathString)
        End While
        SQLReader.Close()
      End If

      If deBrp = True Then
        SQLCommand.CommandText = "SELECT ghpath,object FROM breps;"
        Dim SQLReader As SQLiteDataReader = SQLCommand.ExecuteReader()
        While SQLReader.Read()
          Dim PathString As String = SQLReader(0)
          Dim RowString As String = SQLReader(1)
          xml.Add(RowString)
          ghpaths.Add(PathString)
        End While
        SQLReader.Close()
      End If

      If deMsh = True Then
        SQLCommand.CommandText = "SELECT ghpath,object FROM meshes;"
        Dim SQLReader As SQLiteDataReader = SQLCommand.ExecuteReader()
        While SQLReader.Read()
          Dim PathString As String = SQLReader(0)
          Dim RowString As String = SQLReader(1)
          xml.Add(RowString)
          ghpaths.Add(PathString)
        End While
        SQLReader.Close()

      End If

      SQLCommand.Dispose()
      SQLConnect.Close()

      Dim myPts As New DataTree(Of Point3d)
      Dim myCrvs As New DataTree(Of Curve)
      Dim mySrfs As New DataTree(Of Brep)
      Dim myBrps As New DataTree(Of Brep)
      Dim myMshs As New DataTree(Of Mesh)

      'Deserialize XML
      For i As Integer = 0 To xml.Count - 1
        Dim ghpath As String = ghpaths(i)
        Dim ghpatharr As String() = ghpath.Split("-")
        Dim ghpathintarr As New List(Of Integer)
        For Each gh As String In ghpatharr
          ghpathintarr.Add(Convert.ToInt32(gh))
        Next
        Dim m_path As New GH_Path(ghpathintarr.ToArray())

        Dim chunk As New GH_IO.Serialization.GH_LooseChunk("xml")
        chunk.Deserialize_Xml(xml(i))

        'Convert points
        If chunk.Name = "Point" Then
          Dim chunkItems As List(Of GH_IO.Types.GH_Item) = chunk.Items
          Dim chunkItem As GH_IO.Types.GH_Item = chunkItems(0)
          Dim ghpt As GH_IO.Types.GH_Point3D = chunkItem._point3d
          Dim myPoint As Point3d = New Point3d(ghpt.x, ghpt.y, ghpt.z)
          myPts.Add(myPoint, m_path)

          'Convert curves
        ElseIf chunk.Name = "Curve" Then
          Dim chunkItems As List(Of GH_IO.Types.GH_Item) = chunk.Items
          Dim chunkItem As GH_IO.Types.GH_Item = chunkItems(1)
          Dim myBytes() As Byte = chunkItem._bytearray
          Dim myCurve As Object = Kernel.GH_Convert.ByteArrayToCommonObject(Of Curve)(myBytes)
          myCrvs.Add(myCurve, m_path)

          'Convert surfaces
        ElseIf chunk.Name = "Surface" Then
          Dim chunkItems As List(Of GH_IO.Types.GH_Item) = chunk.Items
          Dim chunkItem As GH_IO.Types.GH_Item = chunkItems(0)
          Dim myBytes() As Byte = chunkItem._bytearray
          Dim mySurface As Object = Kernel.GH_Convert.ByteArrayToCommonObject(Of Brep)(myBytes)
          mySrfs.Add(mySurface, m_path)

          'Convert Breps
        ElseIf chunk.Name = "Brep" Then
          Dim chunkItems As List(Of GH_IO.Types.GH_Item) = chunk.Items
          Dim chunkItem As GH_IO.Types.GH_Item = chunkItems(0)
          Dim myBytes() As Byte = chunkItem._bytearray
          Dim myBrep As Object = Kernel.GH_Convert.ByteArrayToCommonObject(Of Brep)(myBytes)
          myBrps.Add(myBrep, m_path)

          'Convert Meshes
        ElseIf chunk.Name = "Mesh" Then
          Dim chunkItems As List(Of GH_IO.Types.GH_Item) = chunk.Items
          Dim chunkItem As GH_IO.Types.GH_Item = chunkItems(0)
          Dim myBytes() As Byte = chunkItem._bytearray
          Dim myMesh As Object = Kernel.GH_Convert.ByteArrayToCommonObject(Of Mesh)(myBytes)
          myMshs.Add(myMesh, m_path)
        End If
      Next

      DA.SetDataTree(1, myPts)
      DA.SetDataTree(2, myCrvs)
      DA.SetDataTree(3, mySrfs)
      DA.SetDataTree(4, myBrps)
      DA.SetDataTree(5, myMshs)

    Catch ex As Exception
      DA.SetData(0, ex.ToString)
    End Try

  End Sub

#End Region
End Class