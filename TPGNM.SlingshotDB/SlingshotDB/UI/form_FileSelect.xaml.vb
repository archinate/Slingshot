Public Class form_FileSelect

  'Public Properties
  Public Property DatabaseType As String
  Public Property FilePath As String

  ''' <summary>
  ''' RDBMS String Settings
  ''' </summary>
  ''' <param name="d">Database</param>
  ''' <param name="f">File</param>
  ''' <remarks></remarks>
  Public Sub New(ByRef d As String, ByRef f As String)
    ' This call is required by the designer.
    InitializeComponent()

    'Widen Scope
    DatabaseType = d
    FilePath = f

  End Sub

  Private Sub form_FamilyExportSettings_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

    Try

      'Add ComboBox Items
      ComboBox_DatabaseType.Items.Clear()
      ComboBox_DatabaseType.Items.Add("Access")
      ComboBox_DatabaseType.Items.Add("Excel")
      ComboBox_DatabaseType.Items.Add("SQLite")

      'Set field values
      ComboBox_DatabaseType.SelectedItem = DatabaseType
      TextBox_File.Text = FilePath

    Catch ex As Exception
      MsgBox(ex.ToString, MsgBoxStyle.Exclamation, "Oops")
    End Try

  End Sub

  Private Sub Button_SaveSettings_Click(sender As Object, e As Windows.RoutedEventArgs) Handles Button_SaveSettings.Click

    'Family
    DatabaseType = ComboBox_DatabaseType.SelectedItem
    FilePath = TextBox_File.Text

    'Close
    Me.Close()

  End Sub

  Private Sub Button_File_Click(sender As Object, e As Windows.RoutedEventArgs) Handles Button_File.Click
    Dim m_d As New clsDialogs()
    'Open document dialog and select file.
    If ComboBox_DatabaseType.SelectedItem = "SQLite" Then
      FilePath = m_d.OpenFile("Document", "s3db", "SQLite Database | *.s3db")
      TextBox_File.Text = FilePath
      ComboBox_DatabaseType.SelectedItem = "SQLite"
    ElseIf ComboBox_DatabaseType.SelectedItem = "Access" Then
      FilePath = m_d.OpenFile("Document", "accdb", "Access Database | *.accdb")
      TextBox_File.Text = FilePath
      ComboBox_DatabaseType.SelectedItem = "Access"
    ElseIf ComboBox_DatabaseType.SelectedItem = "Excel" Then
      FilePath = m_d.OpenFile("Document", "xlsx", "Excel Database | *.xlsx")
      TextBox_File.Text = FilePath
      ComboBox_DatabaseType.SelectedItem = "Excel"
    End If
  End Sub
End Class
