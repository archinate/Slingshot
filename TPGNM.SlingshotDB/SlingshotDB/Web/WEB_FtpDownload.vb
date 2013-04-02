Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types

Imports GH_IO
Imports GH_IO.Serialization

Imports System
Imports System.IO

Public Class WEB_FtpDownload
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("FTP Download", "FtpDown", "Download a file from a FTP server.", "Slingshot!", "Web Tools")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{ef60f06a-1fd1-4614-bf43-ada623006918}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.WEB_FtpDownload
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("FTP Address", "Address", "The FTP Address of the file to download.", GH_ParamAccess.item)
    pManager.AddTextParameter("File Path", "Path", "Save location for the file to download.", GH_ParamAccess.item)
    pManager.AddTextParameter("FTP Username", "Username", "Username for the FTP server.", GH_ParamAccess.item)
    pManager.AddTextParameter("FTP Password", "Password", "Password for the FTP server.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", GH_ParamAccess.item, False)

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

          'delete existing file
          Dim FileToDelete As String
          FileToDelete = path
          If System.IO.File.Exists(FileToDelete) = True Then
            System.IO.File.Delete(FileToDelete)
          End If

          'download file

          Dim ftpClient As New System.Net.WebClient
          ftpClient.Credentials = New System.Net.NetworkCredential(user, pass)
          My.Computer.FileSystem.WriteAllBytes(path, _
            ftpClient.DownloadData(address), True)

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