Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types
Imports GH_IO
Imports GH_IO.Serialization

Imports System
Imports System.IO

Public Class WEB_FtpUpload
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("FTP Upload", "FtpUp", "Upload a file to a FTP server.", "Slingshot!", "Web Tools")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{7b0f7751-9aa9-4b3c-8e39-c03c3e32a380}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.WEB_FtpUpload
    End Get
  End Property
#End Region


#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("FTP Address", "Address", "The FTP Address to upload to.", GH_ParamAccess.item)
    pManager.AddTextParameter("File Path", "Path", "Location of the file to upload.", GH_ParamAccess.item)
    pManager.AddTextParameter("FTP Username", "Username", "Username for the FTP server.", GH_ParamAccess.item)
    pManager.AddTextParameter("FTP Password", "Password", "Password for the FTP server.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", False, GH_ParamAccess.item)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Exceptions", "out", "Displays errors.")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim address As String = Nothing
      Dim path As String = Nothing
      Dim user As String = Nothing
      Dim pass As String = Nothing
      Dim connect As Boolean = False

      DA.GetData(Of String)(0, address)
      DA.GetData(Of String)(1, path)
      DA.GetData(Of String)(2, user)
      DA.GetData(Of String)(2, pass)
      DA.GetData(Of Boolean)(2, connect)

      If connect = True Then

        Try
          ' Request access to FTP
          Dim ftpClient As New System.Net.WebClient
          ftpClient.Credentials = New System.Net.NetworkCredential(user, pass)
          ftpClient.UploadFile(address, path)

        Catch e As Exception
          DA.SetData(0, e.ToString)
        End Try

      End If


    Catch ex As Exception

      DA.SetData(0, ex.ToString)

    End Try
  End Sub
#End Region

End Class