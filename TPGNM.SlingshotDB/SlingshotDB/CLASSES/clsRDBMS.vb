Imports System

Imports MySql
Imports System.Data.Odbc
Imports System.Data.OleDb

Public Class clsRDBMS

#Region "Commands"
  ''' <summary>
  ''' MySQL Command
  ''' </summary>
  ''' <param name="cs">Connection String</param>
  ''' <param name="com">Command</param>
  ''' <returns>Boolean for Success or Failure</returns>
  ''' <remarks></remarks>
  Public Function MySQLCommand(ByRef cs As String, ByRef com As String) As Boolean
    Try
      'Establish MySQL Database Connection
      Dim mysqlConnect As MySql.Data.MySqlClient.MySqlConnection = New MySql.Data.MySqlClient.MySqlConnection(cs)
      Dim mysqldata As New MySql.Data.MySqlClient.MySqlDataAdapter
      mysqlConnect.Open()

      'Insert Command
      mysqldata.InsertCommand = New MySql.Data.MySqlClient.MySqlCommand(com, mysqlConnect)
      mysqldata.InsertCommand.ExecuteNonQuery()

      'Close the connection
      mysqlConnect.Close()

      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function

  ''' <summary>
  ''' ODBC Command
  ''' </summary>
  ''' <param name="cs">Connection String</param>
  ''' <param name="com">Command</param>
  ''' <returns>Boolean for Success or Failure</returns>
  ''' <remarks></remarks>
  Public Function ODBCCommand(ByRef cs As String, ByRef com As String) As Boolean
    Try
      'Connect to ODBC
      Dim dbConnect As OdbcConnection = New OdbcConnection(cs)
      dbConnect.Open()

      'Execute command
      Dim dbCommand As OdbcCommand = New OdbcCommand(com, dbConnect)
      dbCommand.ExecuteNonQuery()
      dbConnect.Close()

      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function

  ''' <summary>
  ''' OLEDB Command
  ''' </summary>
  ''' <param name="cs">Connection String</param>
  ''' <param name="com">Command</param>
  ''' <returns>Boolean for Success or Failure</returns>
  ''' <remarks></remarks>
  Public Function OLEDBCommand(ByRef cs As String, ByRef com As String) As Boolean
    Try
      'Connect to OLEDB
      Dim dbConnect As OleDbConnection = New OleDbConnection(cs)
      dbConnect.Open()

      'Execute OLEDB command
      Dim dbCommand As OleDbCommand = New OleDbCommand(Command, dbConnect)
      dbCommand.ExecuteNonQuery()
      dbConnect.Close()

      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function

#End Region

#Region "Queries"
  ''' <summary>
  ''' MySQL Query
  ''' </summary>
  ''' <param name="cs">Connection String</param>
  ''' <param name="q">Query</param>
  ''' <returns>DataSet</returns>
  ''' <remarks></remarks>
  Public Function MySQLQuery(ByRef cs As String, ByRef q As String) As DataSet
    Try
      Dim sqlDataSet As New DataSet()

      'Establish MySQL Database Connection
      Dim mysqlConnect As MySql.Data.MySqlClient.MySqlConnection = New MySql.Data.MySqlClient.MySqlConnection(cs)
      mysqlConnect.Open()

      Dim mysqldata As New MySql.Data.MySqlClient.MySqlDataAdapter(q, mysqlConnect)

      'Fill the data set
      mysqldata.Fill(sqlDataSet, "result")
      mysqlConnect.Close()

      Return sqlDataSet
    Catch ex As Exception
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' ODBC Query
  ''' </summary>
  ''' <param name="cs">Connection String</param>
  ''' <param name="q">Query</param>
  ''' <returns>DataSet</returns>
  ''' <remarks></remarks>
  Public Function ODBCQuery(ByRef cs As String, ByRef q As String) As DataSet
    Try
      Dim sqlDataSet As New DataSet()

      'Establish ODBC Database Connection
      Dim dbConnect As OdbcConnection = New OdbcConnection(cs)
      dbConnect.Open()

      Dim dbdata As New OdbcDataAdapter(q, dbConnect)

      'Fill dataset
      dbdata.Fill(sqlDataSet, "result")
      dbConnect.Close()

      Return sqlDataSet
    Catch ex As Exception
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' OLEDB Query
  ''' </summary>
  ''' <param name="cs">Connection String</param>
  ''' <param name="q">Query</param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Public Function OLEDBQuery(ByRef cs As String, ByRef q As String) As DataSet
    Try
      Dim sqlDataSet As New DataSet()

      'Establish ODBC Database Connection
      Dim dbConnect As OdbcConnection = New OdbcConnection(cs)
      dbConnect.Open()

      Dim dbdata As New OdbcDataAdapter(q, dbConnect)

      'Fill dataset
      dbdata.Fill(sqlDataSet, "result")
      dbConnect.Close()

      Return sqlDataSet
    Catch ex As Exception
      Return Nothing
    End Try
  End Function


#End Region

End Class
