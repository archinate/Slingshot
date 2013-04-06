Imports System
Imports System.Data.Odbc

Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types

Imports GH_IO
Imports GH_IO.Serialization

Public Class GHODBC_Command
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("ODBC Command", "ODBCCommand", "Send a command to a database using ODBC", "Slingshot!", "RDBMS Connection")
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
      Return New Guid("{d5293b9f-184b-4ac1-9ec3-fc57bf4711af}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.GHODBC_Command

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

        'Connect to ODBC
        Dim dbConnect As OdbcConnection = New OdbcConnection(cstring)
        dbConnect.Open()

        'Execute command
        Dim dbCommand As OdbcCommand = New OdbcCommand(command, dbConnect)
        dbCommand.ExecuteNonQuery()
        dbConnect.Close()

        'Return success message
        DA.SetData(0, "Executed database command: " & command)
      End If

    Catch ex As Exception
      DA.SetData(0, ex.ToString)
    End Try

  End Sub
#End Region

End Class