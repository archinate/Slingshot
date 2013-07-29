Imports System

Imports MySql
Imports System.Data.Odbc
Imports System.Data.OleDb
Imports System.Data.SQLite

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
      Dim dbCommand As OleDbCommand = New OleDbCommand(com, dbConnect)
      dbCommand.ExecuteNonQuery()
      dbConnect.Close()

      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function

  ''' <summary>
  ''' Excel Command
  ''' </summary>
  ''' <param name="p">File Path</param>
  ''' <param name="c">Command</param>
  ''' <returns>Boolean for Success or Failure</returns>
  ''' <remarks></remarks>
  Public Function ExcelCommand(ByRef p As String, ByRef c As String) As Boolean
    Try
      Dim cs As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & p & ";Extended Properties=""Excel 12.0;HDR=Yes"""

      'Connect to OLEDB
      Dim dbConnect As OleDbConnection = New OleDbConnection(cs)
      dbConnect.Open()

      'Execute OLEDB command
      Dim dbCommand As OleDbCommand = New OleDbCommand(c, dbConnect)
      dbCommand.ExecuteNonQuery()
      dbConnect.Close()

      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function

  ''' <summary>
  ''' SQLite Command
  ''' </summary>
  ''' <param name="p">File Path</param>
  ''' <param name="c">Command</param>
  ''' <returns>Boolean for Success or Failure</returns>
  ''' <remarks></remarks>
  Public Function SQLiteCommand(ByRef p As String, ByRef c As String) As Boolean
    Try
      Dim filepath As String = p

      'Connect to SQLite
      Dim SQLConnect As New SQLite.SQLiteConnection()
      Dim SQLCommand As SQLiteCommand

      SQLConnect.ConnectionString = "Data Source=" & filepath
      SQLConnect.Open()
      SQLCommand = SQLConnect.CreateCommand
      SQLCommand.CommandText = c
      SQLCommand.ExecuteNonQuery()
      SQLCommand.Dispose()
      SQLConnect.Close()

      Return True
    Catch ex As Exception
      Return False
    End Try
  End Function

  ''' <summary>
  ''' Access Command
  ''' </summary>
  ''' <param name="p">File Path</param>
  ''' <param name="c">Command</param>
  ''' <returns>Boolean for Success or Failure</returns>
  ''' <remarks></remarks>
  Public Function AccessCommand(ByRef p As String, ByRef c As String) As Boolean
    Try

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

      'Establish OLEDB Database Connection
      Dim dbConnect As OleDbConnection = New OleDbConnection(cs)
      dbConnect.Open()

      Dim dbdata As New OleDbDataAdapter(q, dbConnect)

      'Fill dataset
      dbdata.Fill(sqlDataSet, "result")
      dbConnect.Close()

      Return sqlDataSet
    Catch ex As Exception
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' SQLite Query
  ''' </summary>
  ''' <param name="p">File Path</param>
  ''' <param name="q">Query String</param>
  ''' <returns>Dataset</returns>
  ''' <remarks></remarks>
  Public Function SQLiteQuery(ByRef p As String, ByRef q As String) As DataSet
    Try
      Dim sqlDataSet As New DataSet()

      'Connect to SQLite
      Dim SQLConnect As New SQLite.SQLiteConnection()
      Dim SQLCommand As SQLiteCommand

      SQLConnect.ConnectionString = "Data Source=" & p
      SQLConnect.Open()
      SQLCommand = SQLConnect.CreateCommand
      SQLCommand.CommandText = q

      Dim SQLdb As New SQLiteDataAdapter(q, SQLConnect)

      SQLdb.Fill(sqlDataSet)

      Return sqlDataSet
    Catch ex As Exception
      Return Nothing
    End Try

  End Function

  ''' <summary>
  ''' Excel Query
  ''' </summary>
  ''' <param name="p">File Path</param>
  ''' <param name="q">Query String</param>
  ''' <returns>Dataset</returns>
  ''' <remarks></remarks>
  Public Function ExcelQuery(ByRef p As String, ByRef q As String) As DataSet
    Try
      Dim cs As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & p & ";Extended Properties=""Excel 12.0;HDR=Yes"""

      Dim sqlDataSet As New DataSet()

      'Establish OLEDB Database Connection
      Dim dbConnect As OleDbConnection = New OleDbConnection(cs)
      dbConnect.Open()

      Dim dbdata As New OleDbDataAdapter(q, dbConnect)

      'Fill dataset
      dbdata.Fill(sqlDataSet, "result")
      dbConnect.Close()

      Return sqlDataSet
    Catch ex As Exception
      Return Nothing
    End Try
  End Function

  ''' <summary>
  ''' Access Query
  ''' </summary>
  ''' <param name="p">File Path</param>
  ''' <param name="q">Query String</param>
  ''' <returns>Dataset</returns>
  ''' <remarks></remarks>
  Public Function AccessQuery(ByRef p As String, ByRef q As String) As DataSet
    Try

    Catch ex As Exception
      Return Nothing
    End Try
  End Function

#End Region

End Class
