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


        ShowTabControl("home")
        ShowTabManual("None")




    End Sub


    Private Sub btnHome_Click(sender As Object, e As EventArgs) Handles btnHome.Click
        ShowTabControl("home")
    End Sub

    Private Sub btnManual_Click(sender As Object, e As EventArgs) Handles btnManual.Click
        ShowTabControl("manual")
    End Sub

    Private Sub btnMonitoring_Click(sender As Object, e As EventArgs) Handles btnMonitoring.Click
        ShowTabControl("monitoring")
    End Sub

    Private Sub btnSetting_Click(sender As Object, e As EventArgs) Handles btnSetting.Click
        ShowTabControl("setting")
    End Sub

    'Show Tab Control
    Private Sub ShowTabControl(mode As String)

        ShowTabManual("None")

        If mode = "home" Then
            tabHome.Visible = True
        Else
            tabHome.Visible = False
        End If

        If mode = "monitoring" Or mode = "manual" Then
            ShowButtonSTN(1)
            If mode = "manual" Then
                ManualState = True
            Else
                ManualState = False
            End If
        Else
            ShowButtonSTN(0)
        End If

        If mode = "setting" Then
            tabSetting.Visible = True
        Else
            tabSetting.Visible = False
        End If
    End Sub


    'Show Button Station

    Private Sub ShowButtonSTN(mode As Integer)
        If mode = 1 Then
            btnSTN1.Enabled = True
            btnSTN2.Enabled = True
            btnSTN3.Enabled = True
            btnSTN4.Enabled = True
            btnSTN5.Enabled = True
            btnSTN6.Enabled = True
        Else
            btnSTN1.Enabled = False
            btnSTN2.Enabled = False
            btnSTN3.Enabled = False
            btnSTN4.Enabled = False
            btnSTN5.Enabled = False
            btnSTN6.Enabled = False
        End If
    End Sub


    'Show Tab Manual
    Private Sub ShowTabManual(mode As String)
        If mode = "STN1" Then
            If ManualState Then
                tab_man_stn1.Visible = True
            Else
                tab_mon_stn1.Visible = True
            End If
        Else
            tab_man_stn1.Visible = False
            tab_mon_stn1.Visible = False
        End If

        If mode = "STN2" Then
            If ManualState Then
                tab_man_stn2.Visible = True
            Else
                tab_mon_stn2.Visible = True
            End If
        Else
            tab_man_stn2.Visible = False
            tab_mon_stn2.Visible = False
        End If

        If mode = "STN3" Then
            If ManualState Then
                tab_man_stn3.Visible = True
            Else
                tab_mon_stn3.Visible = True
            End If
        Else
            tab_man_stn3.Visible = False
            tab_mon_stn3.Visible = False
        End If

        If mode = "STN4" Then
            If ManualState Then
                tab_man_stn4.Visible = True
            Else
                tab_mon_stn4.Visible = True
            End If
        Else
            tab_man_stn4.Visible = False
            tab_mon_stn4.Visible = False
        End If

        If mode = "STN5" Then
            If ManualState Then
                tab_man_stn5.Visible = True
            Else
                tab_mon_stn5.Visible = True
            End If
        Else
            tab_man_stn5.Visible = False
            tab_mon_stn5.Visible = False
        End If

        If mode = "STN6" Then
            If ManualState Then
                tab_man_stn6.Visible = True
            Else
                tab_mon_stn6.Visible = True
            End If
        Else
            tab_man_stn6.Visible = False
            tab_mon_stn6.Visible = False
        End If

    End Sub

    Private Sub btnSTN1_Click(sender As Object, e As EventArgs) Handles btnSTN1.Click
        ShowTabManual("STN1")
    End Sub

    Private Sub btnSTN2_Click(sender As Object, e As EventArgs) Handles btnSTN2.Click
        ShowTabManual("STN2")
    End Sub

    Private Sub btnSTN3_Click(sender As Object, e As EventArgs) Handles btnSTN3.Click
        ShowTabManual("STN3")
    End Sub

    Private Sub btnSTN4_Click(sender As Object, e As EventArgs) Handles btnSTN4.Click
        ShowTabManual("STN4")
    End Sub

    Private Sub btnSTN5_Click(sender As Object, e As EventArgs) Handles btnSTN5.Click
        ShowTabManual("STN5")
    End Sub

    Private Sub btnSTN6_Click(sender As Object, e As EventArgs) Handles btnSTN6.Click
        ShowTabManual("STN6")
    End Sub

End Class
