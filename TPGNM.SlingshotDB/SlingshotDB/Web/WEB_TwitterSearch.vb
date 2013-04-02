Imports Grasshopper
Imports Grasshopper.Kernel
Imports Grasshopper.Kernel.Data
Imports Grasshopper.Kernel.Types

Imports GH_IO
Imports GH_IO.Serialization

Imports System
Imports System.IO
Imports System.Net
Imports System.Web
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

Public Class WEB_TwitterSearch
  Inherits Grasshopper.Kernel.GH_Component

#Region "Register"
  'Methods
  Public Sub New()
    MyBase.New("Twitter Search", "Twitter", "Twitter Search API: https://dev.twitter.com/docs/using-search.", "Slingshot!", "Web Tools")
  End Sub

  'GUID generator http://www.guidgenerator.com/online-guid-generator.aspx
  Public Overrides ReadOnly Property ComponentGuid As System.Guid
    Get
      Return New Guid("{27917514-19b5-452a-ab36-5dab08e02149}")
    End Get
  End Property

  'Icon 24x24
  Protected Overrides ReadOnly Property Internal_Icon_24x24 As System.Drawing.Bitmap
    Get
      Return My.Resources.WEB_TwitterSearch
    End Get
  End Property
#End Region

#Region "Inputs/Outputs"
  Protected Overrides Sub RegisterInputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_InputParamManager)
    pManager.AddTextParameter("Twitter Search Query", "Query", "Query for the Twitter Search API.", GH_ParamAccess.item)
    pManager.AddBooleanParameter("Connect Toggle", "CToggle", "Set to 'True' to connect.", GH_ParamAccess.item, False)
    'pManager.Register_DoubleParam("Request Timeout", "Timeout", "Request timeout.", "-1", GH_ParamAccess.item)

  End Sub

  Protected Overrides Sub RegisterOutputParams(ByVal pManager As Grasshopper.Kernel.GH_Component.GH_OutputParamManager)
    'pManager.Register_GenericParam("Exceptions", "out", "Displays errors.")
    pManager.Register_GenericParam("Tweet ID", "ID", "The unique twitter ID of tweet.")
    pManager.Register_GenericParam("Author", "Author", "Author of the tweet.")
    pManager.Register_GenericParam("Date", "Date", "Date tweet was published.")
    pManager.Register_GenericParam("URL", "URL", "Profile URL.")
    pManager.Register_GenericParam("Tweet", "Tweet", "Tweet text.")
    pManager.Register_GenericParam("Atom Feed", "Atom", "The full Atom feed (xml)")
  End Sub
#End Region

#Region "Solution"
  Protected Overrides Sub SolveInstance(ByVal DA As Grasshopper.Kernel.IGH_DataAccess)
    Try
      Dim query As String = Nothing
      Dim connect As Boolean = False
      'Dim timeout As Double = Nothing

      Dim tweetID As New List(Of String)
      Dim author As New List(Of String)
      Dim tweet As New List(Of String)
      Dim published As New List(Of String)
      Dim turl As New List(Of String)

      DA.GetData(Of String)(0, query)
      DA.GetData(Of Boolean)(1, connect)
      'DA.GetData(Of Double)(2, timeout)

      If connect = True Then

        'get atom feed from twitter search
        Dim request As HttpWebRequest = Nothing
        Dim response As HttpWebResponse = Nothing
        Dim sr As StreamReader = Nothing
        Dim twittersearch As String = "http://search.twitter.com/search.atom?q=" & HttpUtility.UrlEncode(query)

        Dim mytext As New List(Of String)


        request = CType(WebRequest.Create(twittersearch), HttpWebRequest)
        request.Timeout = -1

        response = CType(request.GetResponse, HttpWebResponse)
        Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252)

        sr = New System.IO.StreamReader(response.GetResponseStream, enc)

        'twitter search result converted to string
        Do While sr.Peek() >= 0
          mytext.Add(sr.ReadLine())
        Loop

        sr.Close()

        'parse atom feed as xml
        Dim myxmldoc As XmlDocument

        Dim myxmlIDlist As XmlNodeList
        Dim myxmlAuthlist As XmlNodeList
        Dim myxmlTweetlist As XmlNodeList
        Dim myxmlURLlist As XmlNodeList
        Dim myxmlDatelist As XmlNodeList

        Dim myxmlnode As XmlNode
        myxmldoc = New XmlDocument
        myxmldoc.XmlResolver = Nothing

        myxmldoc.Load(twittersearch)

        myxmlIDlist = myxmldoc.GetElementsByTagName("id")
        myxmlAuthlist = myxmldoc.GetElementsByTagName("name")
        myxmlTweetlist = myxmldoc.GetElementsByTagName("title")
        myxmlURLlist = myxmldoc.GetElementsByTagName("uri")
        myxmlDatelist = myxmldoc.GetElementsByTagName("published")

        Dim myxmlreturn As New List(Of String)

        For Each myxmlnode In myxmlIDlist
          Dim idvalue As String = myxmlnode.InnerText
          tweetID.Add(idvalue)
        Next
        tweetID.RemoveAt(0)

        For Each myxmlnode In myxmlAuthlist
          Dim idvalue As String = myxmlnode.InnerText
          author.Add(idvalue)
        Next

        For Each myxmlnode In myxmlTweetlist
          Dim idvalue As String = myxmlnode.InnerText
          tweet.Add(idvalue)
        Next
        tweet.RemoveAt(0)

        For Each myxmlnode In myxmlURLlist
          Dim idvalue As String = myxmlnode.InnerText
          turl.Add(idvalue)
        Next

        For Each myxmlnode In myxmlDatelist
          Dim idvalue As String = myxmlnode.InnerText
          published.Add(idvalue)
        Next

        DA.SetDataList(0, tweetID)
        DA.SetDataList(1, author)
        DA.SetDataList(2, published)
        DA.SetDataList(3, turl)
        DA.SetDataList(4, tweet)
        DA.SetDataList(5, mytext)

      End If


    Catch ex As Exception

      'DA.SetData(0, ex.ToString)

    End Try
  End Sub
#End Region

End Class