Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System
Imports System.Data.OleDb


Public Class GHOLEDB_Command
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("OLE DB Command", "OLEDBCommand", "Send a command to a database using OLE DB", "Slingshot!", "RDBMS Connection")
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
      Return New Guid("{4904b8d4-8f52-4a92-9488-0edc861b3ab3}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHOLEDB_Command
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Connection String", "CString", "A database connection string.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", GH_ParamAccess.item, False)
    pManager.AddTextParameter("Command", "Command", "A database command.", GH_ParamAccess.item)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Exceptions", "out", "Prints error or success streams.")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim cstring As String = Nothing
      Dim connect As Boolean = False
      Dim command As String = Nothing

      DA.GetData(Of String)(0, cstring)
      DA.GetData(Of Boolean)(1, connect)
      DA.GetData(Of String)(2, command)

      If connect = True Then

        'Connect to OLEDB
        Dim dbConnect As OleDbConnection = New OleDbConnection(cstring)
        dbConnect.Open()

        'Execute OLEDB command
        Dim dbCommand As OleDbCommand = New OleDbCommand(command, dbConnect)
        dbCommand.ExecuteNonQuery()
        dbConnect.Close()

        'Return success
        DA.SetData(0, "Executed database command: " & command)
      End If

    Catch ex As Exception

      DA.SetData(0, ex.ToString)

    End Try
  End Sub
#End Region

End Class