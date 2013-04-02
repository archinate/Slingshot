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


Public Class UTIL_Deserialize
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"

  'Methods
  Public Sub New()
    MyBase.New("Deserialize Object", "DSerial", "Deserialize objects such as Points, Curves, Surfaces, BReps, and Meshes from serialized XML strings", "Slingshot!", "Utility")
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
      Return New Guid("{a74bb9ac-a31c-4014-b368-ca02f40918a3}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLUtility_DeSerialize

    End Get
  End Property

#End Region

#Region "Inputs/Outputs"

  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)

    pManager.AddTextParameter("XML String", "XML", "Serialized XML strings representing Points, Curves, Surfaces, BReps, and Meshes).", GH_ParamAccess.list)

  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_PointParam("Points", "Points", "Deserialized Point objects.")
    pManager.Register_CurveParam("Curves", "Curves", "Deserialized Curve objects.")
    pManager.Register_SurfaceParam("Surfaces", "Surfaces", "Deserialized Surface objects.")
    pManager.Register_BRepParam("BReps", "BRep", "Deserialized Boundary representation objects.")
    pManager.Register_MeshParam("Meshes", "Meshes", "Deserialized Mesh objects.")
  End Sub

#End Region

#Region "Solution"

  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)


    Dim xml As New List(Of String)

    DA.GetDataList(Of String)(0, xml)

    Dim myPts As New List(Of Point3d)
    Dim myCrvs As New List(Of Curve)
    Dim mySrfs As New List(Of Brep)
    Dim myBrps As New List(Of Brep)
    Dim myMshs As New List(Of Mesh)

    If xml Is Nothing Then Return

    For i As Integer = 0 To xml.Count - 1

      Dim chunk As New GH_IO.Serialization.GH_LooseChunk("xml")
      chunk.Deserialize_Xml(xml(i))

      Dim chunkItems As List(Of GH_IO.Types.GH_Item) = chunk.Items
      Dim chunkItem As GH_IO.Types.GH_Item = chunkItems(chunkItems.Count - 1)

      'Convert points
      If chunk.Name = "Point" Then
        Dim ghpt As GH_IO.Types.GH_Point3D = chunkItem._point3d
        Dim myPoint As Point3d = New Point3d(ghpt.x, ghpt.y, ghpt.z)
        myPts.Add(myPoint)

        'Convert curves
      ElseIf chunk.Name = "Curve" Then
        Dim myBytes() As Byte = chunkItem._bytearray
        Dim myCurve As Object = Kernel.GH_Convert.ByteArrayToCommonObject(Of Curve)(myBytes)
        myCrvs.Add(myCurve)

        'Convert surfaces
      ElseIf chunk.Name = "Surface" Then
        Dim myBytes() As Byte = chunkItem._bytearray
        Dim mySurface As Object = Kernel.GH_Convert.ByteArrayToCommonObject(Of Brep)(myBytes)
        mySrfs.Add(mySurface)

        'Convert Breps
      ElseIf chunk.Name = "Brep" Then
        Dim myBytes() As Byte = chunkItem._bytearray
        Dim myBrep As Object = Kernel.GH_Convert.ByteArrayToCommonObject(Of Brep)(myBytes)
        myBrps.Add(myBrep)

        'Convert Meshes
      ElseIf chunk.Name = "Mesh" Then
        Dim myBytes() As Byte = chunkItem._bytearray
        Dim myMesh As Object = Kernel.GH_Convert.ByteArrayToCommonObject(Of Mesh)(myBytes)
        myMshs.Add(myMesh)
      End If
    Next

    DA.SetDataList(0, myPts)
    DA.SetDataList(1, myCrvs)
    DA.SetDataList(2, mySrfs)
    DA.SetDataList(3, myBrps)
    DA.SetDataList(4, myMshs)
  End Sub

#End Region

End Class