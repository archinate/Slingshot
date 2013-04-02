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

Imports System.Data.SQLite



Public Class GHSQLite_CreateDB
    Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("SQLite Create Database File", "LiteCreate", "Creates a SQLite Database file *.s3db if it doesn't already exist", "Slingshot!", "RDBMS Connection")
  End Sub

  'Exposure parameter (line dividers)
  Public Overrides ReadOnly Property Exposure As Grasshopper.Kernel.GH_Exposure
    Get
      Return GH_Exposure.quarternary
    End Get
  End Property

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx

  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{a2dfdff6-5add-4487-9ac9-3bae2cd12ec2}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHSQLite_CreateDB

    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddBooleanParameter("Create Toggle", "CToggle", "Set to 'True' to create the *.s3db database file.", GH_ParamAccess.item, False)
    pManager.AddTextParameter("Directory Path", "Directory", "The directory for the SQLite database file.", GH_ParamAccess.item)
    pManager.AddTextParameter("Database", "Database", "The name of the database file.", GH_ParamAccess.item)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Exceptions", "out", "Prints error or success streams.")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try

      Dim ctoggle As Boolean = False
      Dim path As String = Nothing
      Dim database As String = Nothing

      DA.GetData(Of Boolean)(0, ctoggle)
      DA.GetData(Of String)(1, path)
      DA.GetData(Of String)(2, database)

      If ctoggle = True Then
        Dim filepath As String = path + "\" + database + ".s3db"
        Dim SQLConnect As New SQLite.SQLiteConnection()
        SQLConnect.ConnectionString = "Data Source=" & filepath
        SQLConnect.Open()
        SQLConnect.Close()

        DA.SetData(0, "Database Created!")
      End If

    Catch ex As Exception

      DA.SetData(0, ex.ToString)

    End Try
  End Sub
#End Region

End Class