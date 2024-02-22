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

    'Main Button

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
    'Tab Manual
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


    'Tab Setting
    Private Sub btn_connect_plc_Click(sender As Object, e As EventArgs) Handles btn_connect_plc.Click
        Try
            If btn_connect_plc.Text = "Connect" Then

                modbusClient.IPAddress = txt_ip_plc.Text
                modbusClient.Port = Convert.ToInt32(txt_port_plc.Text)
                modbusClient.Connect()

                btn_connect_plc.Text = "Disconnect"
                ind_connect_plc.BackColor = Color.LawnGreen
                ind_plc_status.BackColor = Color.LawnGreen
                ModbusRW.Enabled = True
                MODBUS_ERR = False

            ElseIf btn_connect_plc.Text = "Disconnect" Then

                modbusClient.Disconnect()

                btn_connect_plc.Text = "Connect"
                ind_connect_plc.BackColor = Color.Green
                ind_plc_status.BackColor = Color.Green
                ModbusRW.Enabled = False

            End If

        Catch ex As Exception
            MsgBox("Failed to connect/disconnect from PLC: " & ex.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    'Button Manual Setup

    'Button Manual Station 1

    Private Sub btn_stn1_cyl1_fw_Click(sender As Object, e As EventArgs) Handles btn_stn1_cyl1_fw.Click
        If btn_stn1_cyl1_bw.Text = "Is Backward" Then
            btn_stn1_cyl1_bw.PerformClick()
        End If

        If btn_stn1_cyl1_fw.Text = "Forward" Then
            STN1_CYL1 = FORWARD
            btn_stn1_cyl1_fw.Image = My.Resources.button_white_trnsprnt
            btn_stn1_cyl1_fw.Text = "Is Forward"
        ElseIf btn_stn1_cyl1_fw.Text = "Is Forward" Then
            STN1_CYL1 = IDLE
            btn_stn1_cyl1_fw.Image = My.Resources.button_silver_trnsprnt
            btn_stn1_cyl1_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn1_cyl1_bw_Click(sender As Object, e As EventArgs) Handles btn_stn1_cyl1_bw.Click
        If btn_stn1_cyl1_fw.Text = "Is Forward" Then
            btn_stn1_cyl1_fw.PerformClick()
        End If

        If btn_stn1_cyl1_bw.Text = "Backward" Then
            STN1_CYL1 = BACKWARD
            btn_stn1_cyl1_bw.Image = My.Resources.button_white_trnsprnt
            btn_stn1_cyl1_bw.Text = "Is Backward"
        ElseIf btn_stn1_cyl1_bw.Text = "Is Backward" Then
            STN1_CYL1 = IDLE
            btn_stn1_cyl1_bw.Image = My.Resources.button_silver_trnsprnt
            btn_stn1_cyl1_bw.Text = "Backward"
        End If
    End Sub

    'Button Manual Station3

    Private Sub btn_stn3_cyl1_fw_Click(sender As Object, e As EventArgs) Handles btn_stn3_cyl1_fw.Click
        If btn_stn3_cyl1_bw.Text = "Is Backward" Then
            btn_stn3_cyl1_bw.PerformClick()
        End If

        If btn_stn3_cyl1_fw.Text = "Forward" Then
            STN3_CYL1 = FORWARD
            btn_stn3_cyl1_fw.Image = My.Resources.button_white
            btn_stn3_cyl1_fw.Text = "Is Forward"
        ElseIf btn_stn3_cyl1_fw.Text = "Is Forward" Then
            STN3_CYL1 = IDLE
            btn_stn3_cyl1_fw.Image = My.Resources.button_silver
            btn_stn3_cyl1_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn3_cyl1_bw_Click(sender As Object, e As EventArgs) Handles btn_stn3_cyl1_bw.Click
        If btn_stn3_cyl1_fw.Text = "Is Forward" Then
            btn_stn3_cyl1_fw.PerformClick()
        End If

        If btn_stn3_cyl1_bw.Text = "Backward" Then
            STN3_CYL1 = BACKWARD
            btn_stn3_cyl1_bw.Image = My.Resources.button_white
            btn_stn3_cyl1_bw.Text = "Is Backward"
        ElseIf btn_stn3_cyl1_bw.Text = "Is Backward" Then
            STN3_CYL1 = IDLE
            btn_stn3_cyl1_bw.Image = My.Resources.button_silver
            btn_stn3_cyl1_bw.Text = "Backward"
        End If
    End Sub

    Private Sub btn_stn3_cyl2_fw_Click(sender As Object, e As EventArgs) Handles btn_stn3_cyl2_fw.Click
        If btn_stn3_cyl2_bw.Text = "Is Backward" Then
            btn_stn3_cyl2_bw.PerformClick()
        End If

        If btn_stn3_cyl2_fw.Text = "Forward" Then
            STN3_CYL2 = FORWARD
            btn_stn3_cyl2_fw.Image = My.Resources.button_white
            btn_stn3_cyl2_fw.Text = "Is Forward"
        ElseIf btn_stn3_cyl2_fw.Text = "Is Forward" Then
            STN3_CYL2 = IDLE
            btn_stn3_cyl2_fw.Image = My.Resources.button_silver
            btn_stn3_cyl2_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn3_cyl2_bw_Click(sender As Object, e As EventArgs) Handles btn_stn3_cyl2_bw.Click
        If btn_stn3_cyl3_bw.Text = "Is Backward" Then
            btn_stn3_cyl3_bw.PerformClick()
        End If

        If btn_stn3_cyl3_fw.Text = "Forward" Then
            STN3_CYL3 = FORWARD
            btn_stn3_cyl3_fw.Image = My.Resources.button_white
            btn_stn3_cyl3_fw.Text = "Is Forward"
        ElseIf btn_stn3_cyl3_fw.Text = "Is Forward" Then
            STN3_CYL3 = IDLE
            btn_stn3_cyl3_fw.Image = My.Resources.button_silver
            btn_stn3_cyl3_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn3_cyl3_fw_Click(sender As Object, e As EventArgs) Handles btn_stn3_cyl3_fw.Click
        If btn_stn3_cyl3_bw.Text = "Is Backward" Then
            btn_stn3_cyl3_bw.PerformClick()
        End If

        If btn_stn3_cyl3_fw.Text = "Forward" Then
            STN3_CYL3 = FORWARD
            btn_stn3_cyl3_fw.Image = My.Resources.button_white
            btn_stn3_cyl3_fw.Text = "Is Forward"
        ElseIf btn_stn3_cyl3_fw.Text = "Is Forward" Then
            STN3_CYL3 = IDLE
            btn_stn3_cyl3_fw.Image = My.Resources.button_silver
            btn_stn3_cyl3_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn3_cyl3_bw_Click(sender As Object, e As EventArgs) Handles btn_stn3_cyl3_bw.Click
        If btn_stn3_cyl3_fw.Text = "Is Forward" Then
            btn_stn3_cyl3_fw.PerformClick()
        End If

        If btn_stn3_cyl3_bw.Text = "Backward" Then
            STN3_CYL3 = BACKWARD
            btn_stn3_cyl3_bw.Image = My.Resources.button_white
            btn_stn3_cyl3_bw.Text = "Is Backward"
        ElseIf btn_stn3_cyl3_bw.Text = "Is Backward" Then
            STN3_CYL3 = IDLE
            btn_stn3_cyl3_bw.Image = My.Resources.button_silver
            btn_stn3_cyl3_bw.Text = "Backward"
        End If
    End Sub

    Private Sub btn_stn3_cyl4_fw_Click(sender As Object, e As EventArgs) Handles btn_stn3_cyl4_fw.Click
        If btn_stn3_cyl4_bw.Text = "Is Backward" Then
            btn_stn3_cyl4_bw.PerformClick()
        End If

        If btn_stn3_cyl4_fw.Text = "Forward" Then
            STN3_CYL4 = FORWARD
            btn_stn3_cyl4_fw.Image = My.Resources.button_white
            btn_stn3_cyl4_fw.Text = "Is Forward"
        ElseIf btn_stn3_cyl4_fw.Text = "Is Forward" Then
            STN3_CYL4 = IDLE
            btn_stn3_cyl4_fw.Image = My.Resources.button_silver
            btn_stn3_cyl4_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn3_cyl4_bw_Click(sender As Object, e As EventArgs) Handles btn_stn3_cyl4_bw.Click
        If btn_stn3_cyl4_fw.Text = "Is Forward" Then
            btn_stn3_cyl4_fw.PerformClick()
        End If

        If btn_stn3_cyl4_bw.Text = "Backward" Then
            STN3_CYL4 = BACKWARD
            btn_stn3_cyl4_bw.Image = My.Resources.button_white
            btn_stn3_cyl4_bw.Text = "Is Backward"
        ElseIf btn_stn3_cyl4_bw.Text = "Is Backward" Then
            STN3_CYL4 = IDLE
            btn_stn3_cyl4_bw.Image = My.Resources.button_silver
            btn_stn3_cyl4_bw.Text = "Backward"
        End If
    End Sub


    'Button Manual Station 4

    Private Sub btn_stn4_cyl1_fw_Click(sender As Object, e As EventArgs) Handles btn_stn4_cyl1_fw.Click
        If btn_stn4_cyl1_bw.Text = "Is Backward" Then
            btn_stn4_cyl1_bw.PerformClick()
        End If

        If btn_stn4_cyl1_fw.Text = "Forward" Then
            STN4_CYL1 = FORWARD
            btn_stn4_cyl1_fw.Image = My.Resources.button_white_trnsprnt
            btn_stn4_cyl1_fw.Text = "Is Forward"
        ElseIf btn_stn4_cyl1_fw.Text = "Is Forward" Then
            STN4_CYL1 = IDLE
            btn_stn4_cyl1_fw.Image = My.Resources.button_silver_trnsprnt
            btn_stn4_cyl1_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn4_cyl1_bw_Click(sender As Object, e As EventArgs) Handles btn_stn4_cyl1_bw.Click
        If btn_stn4_cyl1_fw.Text = "Is Forward" Then
            btn_stn4_cyl1_fw.PerformClick()
        End If

        If btn_stn4_cyl1_bw.Text = "Backward" Then
            STN4_CYL1 = BACKWARD
            btn_stn4_cyl1_bw.Image = My.Resources.button_white
            btn_stn4_cyl1_bw.Text = "Is Backward"
        ElseIf btn_stn4_cyl1_bw.Text = "Is Backward" Then
            STN4_CYL1 = IDLE
            btn_stn4_cyl1_bw.Image = My.Resources.button_silver
            btn_stn4_cyl1_bw.Text = "Backward"
        End If
    End Sub


    'Button Manual Station 5

    Private Sub btn_stn5_cyl1_fw_Click(sender As Object, e As EventArgs) Handles btn_stn5_cyl1_fw.Click
        If btn_stn5_cyl1_bw.Text = "Is Backward" Then
            btn_stn5_cyl1_bw.PerformClick()
        End If

        If btn_stn5_cyl1_fw.Text = "Forward" Then
            STN5_CYL1 = FORWARD
            btn_stn5_cyl1_fw.Image = My.Resources.button_white
            btn_stn5_cyl1_fw.Text = "Is Forward"
        ElseIf btn_stn5_cyl1_fw.Text = "Is Forward" Then
            STN5_CYL1 = IDLE
            btn_stn5_cyl1_fw.Image = My.Resources.button_silver
            btn_stn5_cyl1_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn5_cyl1_bw_Click(sender As Object, e As EventArgs) Handles btn_stn5_cyl1_bw.Click
        If btn_stn5_cyl1_fw.Text = "Is Forward" Then
            btn_stn5_cyl1_fw.PerformClick()
        End If

        If btn_stn5_cyl1_bw.Text = "Backward" Then
            STN5_CYL1 = BACKWARD
            btn_stn5_cyl1_bw.Image = My.Resources.button_white
            btn_stn5_cyl1_bw.Text = "Is Backward"
        ElseIf btn_stn5_cyl1_bw.Text = "Is Backward" Then
            STN5_CYL1 = IDLE
            btn_stn5_cyl1_bw.Image = My.Resources.button_silver
            btn_stn5_cyl1_bw.Text = "Backward"
        End If
    End Sub

    Private Sub btn_stn5_cyl2_fw_Click(sender As Object, e As EventArgs) Handles btn_stn5_cyl2_fw.Click
        If btn_stn5_cyl2_bw.Text = "Is Backward" Then
            btn_stn5_cyl2_bw.PerformClick()
        End If

        If btn_stn5_cyl2_fw.Text = "Forward" Then
            STN5_CYL2 = FORWARD
            btn_stn5_cyl2_fw.Image = My.Resources.button_white
            btn_stn5_cyl2_fw.Text = "Is Forward"
        ElseIf btn_stn5_cyl2_fw.Text = "Is Forward" Then
            STN5_CYL2 = IDLE
            btn_stn5_cyl2_fw.Image = My.Resources.button_silver
            btn_stn5_cyl2_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn5_cyl2_bw_Click(sender As Object, e As EventArgs) Handles btn_stn5_cyl2_bw.Click
        If btn_stn5_cyl2_fw.Text = "Is Forward" Then
            btn_stn5_cyl2_fw.PerformClick()
        End If

        If btn_stn5_cyl2_bw.Text = "Backward" Then
            STN5_CYL2 = BACKWARD
            btn_stn5_cyl2_bw.Image = My.Resources.button_white
            btn_stn5_cyl2_bw.Text = "Is Backward"
        ElseIf btn_stn5_cyl2_bw.Text = "Is Backward" Then
            STN5_CYL2 = IDLE
            btn_stn5_cyl2_bw.Image = My.Resources.button_silver
            btn_stn5_cyl2_bw.Text = "Backward"
        End If
    End Sub

    Private Sub btn_stn5_cyl3_fw_Click(sender As Object, e As EventArgs) Handles btn_stn5_cyl3_fw.Click
        If btn_stn5_cyl3_bw.Text = "Is Backward" Then
            btn_stn5_cyl3_bw.PerformClick()
        End If

        If btn_stn5_cyl3_fw.Text = "Forward" Then
            STN5_CYL3 = FORWARD
            btn_stn5_cyl3_fw.Image = My.Resources.button_white
            btn_stn5_cyl3_fw.Text = "Is Forward"
        ElseIf btn_stn5_cyl3_fw.Text = "Is Forward" Then
            STN5_CYL3 = IDLE
            btn_stn5_cyl3_fw.Image = My.Resources.button_silver
            btn_stn5_cyl3_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn5_cyl3_bw_Click(sender As Object, e As EventArgs) Handles btn_stn5_cyl3_bw.Click
        If btn_stn5_cyl3_fw.Text = "Is Forward" Then
            btn_stn5_cyl3_fw.PerformClick()
        End If

        If btn_stn5_cyl3_bw.Text = "Backward" Then
            STN5_CYL3 = BACKWARD
            btn_stn5_cyl3_bw.Image = My.Resources.button_white
            btn_stn5_cyl3_bw.Text = "Is Backward"
        ElseIf btn_stn5_cyl3_bw.Text = "Is Backward" Then
            STN5_CYL3 = IDLE
            btn_stn5_cyl3_bw.Image = My.Resources.button_silver
            btn_stn5_cyl3_bw.Text = "Backward"
        End If
    End Sub

    Private Sub btn_stn6_cyl1_fw_Click(sender As Object, e As EventArgs) Handles btn_stn6_cyl1_fw.Click
        If btn_stn6_cyl1_bw.Text = "Is Backward" Then
            btn_stn6_cyl1_bw.PerformClick()
        End If

        If btn_stn6_cyl1_fw.Text = "Forward" Then
            STN6_CYL1 = FORWARD
            btn_stn6_cyl1_fw.Image = My.Resources.button_white
            btn_stn6_cyl1_fw.Text = "Is Forward"
        ElseIf btn_stn6_cyl1_fw.Text = "Is Forward" Then
            STN6_CYL1 = IDLE
            btn_stn6_cyl1_fw.Image = My.Resources.button_silver
            btn_stn6_cyl1_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn6_cyl1_bw_Click(sender As Object, e As EventArgs) Handles btn_stn6_cyl1_bw.Click
        If btn_stn6_cyl1_fw.Text = "Is Forward" Then
            btn_stn6_cyl1_fw.PerformClick()
        End If

        If btn_stn6_cyl1_bw.Text = "Backward" Then
            STN6_CYL1 = BACKWARD
            btn_stn6_cyl1_bw.Image = My.Resources.button_white
            btn_stn6_cyl1_bw.Text = "Is Backward"
        ElseIf btn_stn6_cyl1_bw.Text = "Is Backward" Then
            STN6_CYL1 = IDLE
            btn_stn6_cyl1_bw.Image = My.Resources.button_silver
            btn_stn6_cyl1_bw.Text = "Backward"
        End If
    End Sub

    Private Sub btn_stn6_cyl2_fw_Click(sender As Object, e As EventArgs) Handles btn_stn6_cyl2_fw.Click
        If btn_stn6_cyl2_bw.Text = "Is Backward" Then
            btn_stn6_cyl2_bw.PerformClick()
        End If

        If btn_stn6_cyl2_fw.Text = "Forward" Then
            STN6_CYL2 = FORWARD
            btn_stn6_cyl2_fw.Image = My.Resources.button_white
            btn_stn6_cyl2_fw.Text = "Is Forward"
        ElseIf btn_stn6_cyl2_fw.Text = "Is Forward" Then
            STN6_CYL2 = IDLE
            btn_stn6_cyl2_fw.Image = My.Resources.button_silver
            btn_stn6_cyl2_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn6_cyl2_bw_Click(sender As Object, e As EventArgs) Handles btn_stn6_cyl2_bw.Click
        If btn_stn6_cyl2_fw.Text = "Is Forward" Then
            btn_stn6_cyl2_fw.PerformClick()
        End If

        If btn_stn6_cyl2_bw.Text = "Backward" Then
            STN6_CYL2 = BACKWARD
            btn_stn6_cyl2_bw.Image = My.Resources.button_white
            btn_stn6_cyl2_bw.Text = "Is Backward"
        ElseIf btn_stn6_cyl2_bw.Text = "Is Backward" Then
            STN6_CYL2 = IDLE
            btn_stn6_cyl2_bw.Image = My.Resources.button_silver
            btn_stn6_cyl2_bw.Text = "Backward"
        End If
    End Sub

    Private Sub btn_stn6_cyl3_fw_Click(sender As Object, e As EventArgs) Handles btn_stn6_cyl3_fw.Click
        If btn_stn6_cyl3_bw.Text = "Is Backward" Then
            btn_stn6_cyl3_bw.PerformClick()
        End If

        If btn_stn6_cyl3_fw.Text = "Forward" Then
            STN6_CYL3 = FORWARD
            btn_stn6_cyl3_fw.Image = My.Resources.button_white
            btn_stn6_cyl3_fw.Text = "Is Forward"
        ElseIf btn_stn6_cyl3_fw.Text = "Is Forward" Then
            STN6_CYL3 = IDLE
            btn_stn6_cyl3_fw.Image = My.Resources.button_silver
            btn_stn6_cyl3_fw.Text = "Forward"
        End If
    End Sub

    Private Sub btn_stn6_cyl3_bw_Click(sender As Object, e As EventArgs) Handles btn_stn6_cyl3_bw.Click
        If btn_stn6_cyl3_fw.Text = "Is Forward" Then
            btn_stn6_cyl3_fw.PerformClick()
        End If

        If btn_stn6_cyl3_bw.Text = "Backward" Then
            STN6_CYL3 = BACKWARD
            btn_stn6_cyl3_bw.Image = My.Resources.button_white
            btn_stn6_cyl3_bw.Text = "Is Backward"
        ElseIf btn_stn6_cyl3_bw.Text = "Is Backward" Then
            STN6_CYL3 = IDLE
            btn_stn6_cyl3_bw.Image = My.Resources.button_silver
            btn_stn6_cyl3_bw.Text = "Backward"
        End If
    End Sub
End Class
