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

Public Class UTIL_Serialize
    Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
    'Methods
    Public Sub New()
        MyBase.New("Serialize Object", "Serial", "Serialize Points, Curves, Surfaces, BReps, and Meshes as XML strings.", "Slingshot!", "Utility")
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
            Return New Guid("{cddc8907-9a81-4fa9-b1e5-828222876cef}")
        End Get
    End Property

    'Icon 24x24
    Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
        Get
            Return My.Resources.SQLUtility_Serialize
        End Get
  End Property

#End Region

#Region "Inputs/Outputs"

    Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)

    pManager.AddGenericParameter("Geometry Objects", "Objects", "Geometry to serialize. Supports Points, Curves, Surfaces, BReps, and Meshes).", GH_ParamAccess.item)

    End Sub

    Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
        pManager.Register_GenericParam("XML String", "XML", "Serialized objects as XML strings")
  End Sub
#End Region

#Region "Solution"

    Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)


        Dim obj As Object = Nothing

        DA.GetData(Of Object)(0, obj)

        Dim xml As String

        'check geometry type
        If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Point" Then

            Dim myobj As Kernel.Types.GH_Point = obj
            Dim mypoint As Point3d = myobj.Value
            Dim pointObj As New Kernel.Types.GH_Point(mypoint)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Point")
            pointObj.Write(chunk)

            xml = chunk.Serialize_Xml()
            DA.SetData(0, xml)
        End If

        If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Brep" Then

            Dim myobj As Kernel.Types.GH_Brep = obj
            Dim mybrep As Brep = myobj.Value
            Dim brepObj As New Kernel.Types.GH_Brep(mybrep)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Brep")
            brepObj.Write(chunk)

            xml = chunk.Serialize_Xml()
            DA.SetData(0, xml)

        End If

        If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Curve" Then

            Dim myobj As Kernel.Types.GH_Curve = obj
            Dim mycurve As Curve = myobj.Value
            Dim curveObj As New Kernel.Types.GH_Curve(mycurve)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Curve")
            curveObj.Write(chunk)

            xml = chunk.Serialize_Xml()
            DA.SetData(0, xml)
        End If

        If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Surface" Then

            Dim myobj As Kernel.Types.GH_Surface = obj
            Dim mysurf As Brep = myobj.Value
            Dim surfaceObj As New Kernel.Types.GH_Brep(mysurf)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Surface")
            surfaceObj.Write(chunk)

            xml = chunk.Serialize_Xml()
            DA.SetData(0, xml)
        End If

        If obj.GetType.FullName = "Grasshopper.Kernel.Types.GH_Mesh" Then

            Dim myobj As Kernel.Types.GH_Mesh = obj
            Dim mymesh As Mesh = myobj.Value
            Dim meshObj As New Kernel.Types.GH_Mesh(mymesh)
            Dim chunk As New GH_IO.Serialization.GH_LooseChunk("Mesh")
            meshobj.Write(chunk)

            xml = chunk.Serialize_Xml()
            DA.SetData(0, xml)
        End If
  End Sub

#End Region

End Class