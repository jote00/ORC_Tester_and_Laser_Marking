Imports System.Data.SqlClient
Module ConnectionDB

    Public Connection As SqlConnection

    Public Sub connection_db()

        Try
            Dim database As String
            With Config

                database = "Data Source=" & .dbHostName & "\SQLEXPRESS01;
                initial catalog=" & .dbDatabase & ";
                integrated security=true"
            End With
            Connection = New SqlConnection(database)
            If Connection.State = ConnectionState.Closed Then Connection.Open() Else Connection.Close()
        Catch ex As Exception
            MsgBox("Database Connection Error -> " + ex.Message)
        End Try
    End Sub



End Module
