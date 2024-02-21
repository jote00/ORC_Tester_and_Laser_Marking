Imports System.Threading
Imports System.Data.SqlClient
Imports System.IO
Imports EasyModbus

Public Class MainForm

    Dim modbusClient As ModbusClient = New ModbusClient()
    Dim ManualState As Boolean

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Dim loadingForm As New LoadingForm()
        'loadingForm.ShowDialog()


        'ShowTabControl("home")

    End Sub


End Class
