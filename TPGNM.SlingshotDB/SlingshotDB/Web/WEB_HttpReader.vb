Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types

Imports GH_IO
Imports GH_IO.Serialization

Imports System
Imports System.IO
Imports System.Net
Imports System.Linq
Imports System.Data

Public Class WEB_HttpReader
    Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("HTTP Reader", "HTTP", "HTTP web reader.", "Slingshot!", "Web Tools")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{c29e9e6f-7049-40ce-8c05-9c63ced3014e}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.WEB_HTTPReader

    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Web URL", "URL", "A Web URL.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", GH_ParamAccess.item, False)
    pManager.AddNumberParameter("Request Timeout", "Timeout", "Web request timeout.", "-1", GH_ParamAccess.item)
  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    pManager.Register_GenericParam("Exceptions", "out", "Displays errors.")
    pManager.Register_GenericParam("Web Response", "Response", "Results in a specific column")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim url As String = Nothing
      Dim connect As Boolean = False
      Dim timeout As Double = Nothing

      DA.GetData(Of String)(0, url)
      DA.GetData(Of Boolean)(1, connect)
      DA.GetData(Of Double)(2, timeout)

      If connect = True Then
        Dim request As HttpWebRequest = Nothing
        Dim response As HttpWebResponse = Nothing
        Dim sr As StreamReader = Nothing

        Dim mytext As New List(Of String)

        request = CType(WebRequest.Create(url), HttpWebRequest)
        request.Timeout = timeout

        response = CType(request.GetResponse, HttpWebResponse)
        Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252)

        sr = New System.IO.StreamReader(response.GetResponseStream, enc)

        Do While sr.Peek() >= 0
          mytext.Add(sr.ReadLine())
        Loop

        sr.Close()

        DA.SetDataList(1, mytext)

      End If


    Catch ex As Exception

      DA.SetData(0, ex.ToString)

    End Try
  End Sub
#End Region

End Class