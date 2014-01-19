Imports System.Windows.Forms

Public Class form_DataGrid

  Private _dt As DataTable

  Public Sub New(ByRef dt As DataTable)
    InitializeComponent()

    'widen scope
    _dt = dt

  End Sub

  ''' <summary>
  ''' Loads the Form
  ''' </summary>
  ''' <param name="sender"></param>
  ''' <param name="e"></param>
  ''' <remarks></remarks>
  Private Sub form_DataGrid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    Try
      DataGridView_Grasshopper.Visible = True

      DataGridView_Grasshopper.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText

      'Set Chart Type
      Dim m_gridtable = _dt
      DataGridView_Grasshopper.DataSource = m_gridtable
    Catch ex As Exception

    End Try
  End Sub

  Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
    Me.Close()
  End Sub
End Class