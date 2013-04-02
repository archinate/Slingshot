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


Public Class GHMYSQL_PostgresConnect
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("PostgreSQL Connection String", "PGString", "Formats a PostgreSQL connection string.", "Slingshot!", "RDBMS Connection")
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
      Return New Guid("{282b429e-44cb-4ea6-8fe8-f122e496f5e2}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHODBC_PostgresConnect

    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Server", "Server", "Address of PostgreSQL Server", GH_ParamAccess.item, "localhost")
    pManager.AddTextParameter("Port", "Port", "PostgreSQL Server Port", GH_ParamAccess.item, "5432")
    pManager.AddTextParameter("User ID", "UID", "User Identification", GH_ParamAccess.item, "root")
    pManager.AddTextParameter("Password", "Pass", "Password", GH_ParamAccess.item, "password")
    pManager.AddTextParameter("Database", "Database", "Optional database parameter", GH_ParamAccess.item, "")
    pManager.AddTextParameter("Command Timeout", "CmdTimeout", "Timeout for the PostgreSQL command.", GH_ParamAccess.item, "20")
    pManager.AddTextParameter("Connnection Timeout", "ConTimeout", "Timeout for the PostgreSQL connection.", GH_ParamAccess.item, "5")
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_StringParam("Connection String", "CString", "A PostgreSQL connection string.")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim server As String = Nothing
      Dim port As String = Nothing
      Dim uid As String = Nothing
      Dim pass As String = Nothing
      Dim db As String = Nothing
      Dim cmdtime As String = Nothing
      Dim contime As String = Nothing

      DA.GetData(Of String)(0, server)
      DA.GetData(Of String)(1, port)
      DA.GetData(Of String)(2, uid)
      DA.GetData(Of String)(3, pass)
      DA.GetData(Of String)(4, db)
      DA.GetData(Of String)(5, cmdtime)
      DA.GetData(Of String)(6, contime)

      Dim connectionstring As String
      If db = "" Then
        connectionstring = "Driver={PostgreSQL}; Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Pwd=" & pass & "; commandtimeout=" & cmdtime & "; Timeout=" & contime

      Else
        connectionstring = "Driver={PostgreSQL}; Server=" & server & "; Port=" & port & "; Uid=" & uid & "; Database=" & db & "; Pwd=" & pass & "; commandtimeout=" & cmdtime & "; Timeout=" & contime

      End If
      DA.SetData(0, connectionstring)

    Catch ex As Exception

    End Try
  End Sub
#End Region

End Class