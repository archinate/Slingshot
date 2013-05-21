Public Class form_Command

  'Public Properties
  Public Property Connector As String
  Public Property Server As String
  Public Property Port As String
  Public Property UserID As String
  Public Property Password As String
  Public Property Database As String
  Public Property CommandTimeout As String
  Public Property ConnectionTimeout As String

  ''' <summary>
  ''' Connection String Settings
  ''' </summary>
  ''' <param name="c">Connector</param>
  ''' <param name="s">server</param>
  ''' <param name="p">Port</param>
  ''' <param name="u">UserID</param>
  ''' <param name="pw">Password</param>
  ''' <param name="db">Database</param>
  ''' <param name="CmT">CommandTimeout</param>
  ''' <param name="CoT">ConnectionTimeout</param>
  ''' <remarks></remarks>
  Public Sub New(ByRef c As String, ByRef s As String, ByRef p As String, ByRef u As String, ByRef pw As String, ByRef db As String, ByRef CmT As String, ByRef CoT As String)
    ' This call is required by the designer.
    InitializeComponent()

    'Widen Scope
    Connector = c
    Server = s
    Port = p
    UserID = u
    Password = pw
    Database = db
    CommandTimeout = CmT
    ConnectionTimeout = CoT

  End Sub

  Private Sub form_FamilyExportSettings_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

    Try

      'Add ComboBox Items
      ComboBox_Connector.Items.Clear()
      ComboBox_Connector.Items.Add("MySQL")
      ComboBox_Connector.Items.Add("Oracle")
      ComboBox_Connector.Items.Add("PostgreSQL")
      ComboBox_Connector.Items.Add("SQL Server 2012")

      'Set field values
      ComboBox_Connector.SelectedItem = Connector
      Text_Server.Text = Server
      Text_Port.Text = Port
      Text_User.Text = UserID
      Text_Password.Password = Password
      Text_Database.Text = Database
      Text_CmdTimeout.Text = CommandTimeout
      Text_ConTimeout.Text = ConnectionTimeout


    Catch ex As Exception
      MsgBox(ex.ToString, MsgBoxStyle.Exclamation, "Oops")
    End Try

  End Sub

  Private Sub Button_SaveSettings_Click(sender As Object, e As Windows.RoutedEventArgs) Handles Button_SaveSettings.Click

    'Family
    Connector = ComboBox_Connector.SelectedItem
    Server = Text_Server.Text
    Port = Text_Port.Text
    UserID = Text_User.Text
    Password = Text_Password.Password
    Database = Text_Database.Text
    CommandTimeout = Text_CmdTimeout.Text
    ConnectionTimeout = Text_ConTimeout.Text

    'Close
    Me.Close()

  End Sub
End Class