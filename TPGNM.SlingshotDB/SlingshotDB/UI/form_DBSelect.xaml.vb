Public Class form_DBSelect
  'Public Properties
  Public Property Connector As String

  ''' <summary>
  ''' RDBMS String Settings
  ''' </summary>
  ''' <param name="c">Connector</param>
  ''' <remarks></remarks>
  Public Sub New(ByRef c As String)
    ' This call is required by the designer.
    InitializeComponent()

    'Widen Scope
    Connector = c

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

    Catch ex As Exception
      MsgBox(ex.ToString, MsgBoxStyle.Exclamation, "Oops")
    End Try

  End Sub

  Private Sub Button_SaveSettings_Click(sender As Object, e As Windows.RoutedEventArgs) Handles Button_SaveSettings.Click

    'Family
    Connector = ComboBox_Connector.SelectedItem

    'Close
    Me.Close()

  End Sub
End Class
