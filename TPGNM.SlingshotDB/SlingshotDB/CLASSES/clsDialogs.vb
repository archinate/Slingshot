Public Class clsDialogs
  ''' <summary>
  ''' Open File Dialogue
  ''' </summary>
  ''' <param name="fname">File Name</param>
  ''' <param name="ext">Default Extension</param>
  ''' <param name="filt">Filter</param>
  ''' <returns>File path as String</returns>
  ''' <remarks></remarks>
  Public Function OpenFile(ByRef fname As String, ByRef ext As String, ByRef filt As String) As String
    'Configure SaveFileDialogue
    Dim m_OpenFileDialog As New Microsoft.Win32.OpenFileDialog()
    m_OpenFileDialog.FileName = fname 'Default file name
    m_OpenFileDialog.DefaultExt = ext 'Default file extension
    m_OpenFileDialog.Filter = filt 'Filter file

    'Show save file dialog box
    Dim m_result As Boolean = m_OpenFileDialog.ShowDialog()
    If m_result Then
      Return m_OpenFileDialog.FileName.ToString
    Else
      Return Nothing
    End If

  End Function

  ''' <summary>
  ''' Save File Dialogue
  ''' </summary>
  ''' <param name="fname">File Name</param>
  ''' <param name="ext">Default Extension</param>
  ''' <param name="filt">Filter</param>
  ''' <returns>File path as String</returns>
  ''' <remarks></remarks>
  Public Function SaveFile(ByRef fname As String, ByRef ext As String, ByRef filt As String) As String
    'Configure SaveFileDialogue
    Dim m_SaveFileDialog As New Microsoft.Win32.SaveFileDialog()
    m_SaveFileDialog.FileName = fname 'Default file name
    m_SaveFileDialog.DefaultExt = ext 'Default file extension
    m_SaveFileDialog.Filter = filt 'Filter file

    'Show save file dialog box
    Dim m_result As Boolean = m_SaveFileDialog.ShowDialog()
    If m_result Then
      Return m_SaveFileDialog.FileName.ToString
    Else
      Return Nothing
    End If

  End Function
End Class
