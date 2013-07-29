Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System
Imports System.IO

Public Class SQLCOM2_CreateGenomeDB

  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQL Create Genome Database", "GeneDB", "Create a special database for storing design genomes", "Slingshot!", "SQL")
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
      Return New Guid("{3ff77621-37e7-428b-9ae4-09994d831fae}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.SQLUTIL_CreateGenomeDb
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Database Name", "Database", "Name of the genome database.", GH_ParamAccess.item)
    pManager.AddTextParameter("Genome Table Name", "GeneName", "Name of the genome.", GH_ParamAccess.item)
    pManager.AddIntegerParameter("Number of Genes", "GeneNum", "Number of genes to store", GH_ParamAccess.item)

  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("SQL Code", "SQL", "SQL Code")
  End Sub

  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Dim database As String = Nothing
    Dim name As String = Nothing
    Dim genenum As Integer = Nothing

    DA.GetData(Of String)(0, database)
    DA.GetData(Of String)(1, name)
    DA.GetData(Of Integer)(2, genenum)

    Dim mygenes As String = "gene0 Float"
    For i As Integer = 1 To genenum - 1
      Dim newgene As String = ",gene" & i & " Float"
      mygenes = mygenes + newgene
    Next

    Dim dbcommand As New List(Of String)
    Dim createDb As String = "CREATE DATABASE IF NOT EXISTS " & database & ";"
    Dim createGnTb As String = "CREATE TABLE IF NOT EXISTS " & database & "." & name & "_genome (id Int NOT NULL AUTO_INCREMENT,PRIMARY KEY(id),generation Int,fitness Float," & mygenes & ");"

    dbcommand.Add(createDb)
    dbcommand.Add(createGnTb)

    DA.SetDataList(0, dbcommand)

  End Sub
#End Region

End Class
