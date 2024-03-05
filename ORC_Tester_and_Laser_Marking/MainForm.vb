Imports System.Threading
Imports System.Data.SqlClient
Imports System.IO
Imports EasyModbus

Public Class MainForm

    Dim statioWrite As Thread
    Dim stationRead As Thread
    Dim ManualState As Boolean
    'Dim EmgState As Integer = 0

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim loadingForm As New LoadingForm()
        loadingForm.ShowDialog()

        stationRead = New Thread(AddressOf plcReading)
        stationRead.Start()
        statioWrite = New Thread(AddressOf plcWriting)
        statioWrite.Start()

        ShowTabControl("home")
        ShowTabManual("None")


        'Fungsi Auto Connect
        AutoConnection()


        If Connected() Then
            btn_connect_plc.Text = "Disconnect"
            ind_connect_plc.BackColor = Color.LawnGreen
            ind_plc_status.BackColor = Color.LawnGreen
            ModbusRW.Enabled = True
            MODBUS_ERR = False
        Else
            ind_connect_plc.BackColor = Color.DarkRed
            ind_plc_status.BackColor = Color.DarkRed
            ModbusRW.Enabled = False
            MODBUS_ERR = True
        End If


    End Sub



    Private Sub DateTime_Tick(sender As Object, e As EventArgs) Handles DateTime.Tick
        lbl_date.Text = Date.Now.ToString("dd-MM-yyyy")
        lbl_curr_time.Text = Date.Now.ToString("hh:mm:ss")
    End Sub


    '###############################################################################################################################################################################################
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
    '###############################################################################################################################################################################################
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

    '###############################################################################################################################################################################################
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

    '###############################################################################################################################################################################################
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
    '###############################################################################################################################################################################################
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

    '###############################################################################################################################################################################################
    'Tab Setting


    'PLC Connection
    Private Sub btn_connect_plc_Click(sender As Object, e As EventArgs) Handles btn_connect_plc.Click
        If btn_connect_plc.Text = "Connect" Then
            Try
                ManualConnection()

                If Connected() Then



                    btn_connect_plc.Text = "Disconnect"
                    ind_connect_plc.BackColor = Color.LawnGreen
                    ind_plc_status.BackColor = Color.LawnGreen
                    ModbusRW.Enabled = True
                    MODBUS_ERR = False


                End If



            Catch ex As Exception
                MsgBox("Failed to connect from PLC: " & ex.Message, MsgBoxStyle.Critical)
            End Try

        ElseIf btn_connect_plc.Text = "Disconnect" Then
            Try
                Disconnect()
                If Not Connected() Then

                    btn_connect_plc.Text = "Connect"
                    ind_connect_plc.BackColor = Color.Green
                    ind_plc_status.BackColor = Color.Green
                    ModbusRW.Enabled = False
                End If

            Catch ex As Exception
                MsgBox("Failed to disconnect from PLC: " & ex.Message, MsgBoxStyle.Critical)
            End Try
        End If

    End Sub

    'Read And Write data Modbus Button

    Private Sub btn_read_Click(sender As Object, e As EventArgs) Handles btn_read.Click
        Try
            Dim address As Integer = Integer.Parse(txtAddress.Text) - 1
            Dim readValue As Integer = Modbus.ReadModbus(address, 1)(0)

            rtbSetting.Text = $"Value at address {address + 1}: {readValue}"
        Catch ex As Exception
            rtbSetting.Text = $"Failed to read data : {ex.Message}"
        End Try
    End Sub

    Private Sub btn_write_Click(sender As Object, e As EventArgs) Handles btn_write.Click
        Try

            Dim address As Integer = Integer.Parse(txtAddress.Text) - 1
            Dim value As Integer = Integer.Parse(txtValue.Text)


            Modbus.WriteModbus(address, value)

            rtbSetting.Text = $"Successfully write value {value} at Address {address + 1}"
        Catch ex As Exception

            rtbSetting.Text = $"Failed to write data: {ex.Message}"
        End Try
    End Sub


    '###############################################################################################################################################################################################
    'Button Manual Setup

    'Button Manual Station 1

    'Private Sub UpdateSTN1_CYL1(value As Integer)
    '    If Me.InvokeRequired Then
    '        Me.Invoke(Sub() UpdateSTN1_CYL1(value))   // Kalo mau pakek Invoke
    '    Else
    '        STN1_CYL1 = value
    '    End If
    'End Sub

    Private Sub btn_stn1_cyl1_fw_Click(sender As Object, e As EventArgs) Handles btn_stn1_cyl1_fw.Click
        If btn_stn1_cyl1_bw.Text = "Is Backward" Then
            btn_stn1_cyl1_bw.PerformClick()
        End If

        If btn_stn1_cyl1_fw.Text = "Forward" Then
            'UpdateSTN1_CYL1(FORWARD)
            STN1_CYL1 = FORWARD
            btn_stn1_cyl1_fw.Image = My.Resources.button_white_trnsprnt
            btn_stn1_cyl1_fw.Text = "Is Forward"
        ElseIf btn_stn1_cyl1_fw.Text = "Is Forward" Then
            'UpdateSTN1_CYL1(IDLE)
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
            'UpdateSTN1_CYL1(BACKWARD)
            STN1_CYL1 = BACKWARD
            btn_stn1_cyl1_bw.Image = My.Resources.button_white_trnsprnt
            btn_stn1_cyl1_bw.Text = "Is Backward"
        ElseIf btn_stn1_cyl1_bw.Text = "Is Backward" Then
            'UpdateSTN1_CYL1(IDLE)
            STN1_CYL1 = IDLE
            btn_stn1_cyl1_bw.Image = My.Resources.button_silver_trnsprnt
            btn_stn1_cyl1_bw.Text = "Backward"
        End If
    End Sub

    'Button Manual Station 3.1

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

    'Button Manual Station 3.2




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





    '###############################################################################################################################################################################################
    'PLC Reading
    Private Sub plcReading()
        Do



            If Connected() Then

                Dim readS11 = ReadModbus(ADDR_STN1_SEN1, 1)
                Dim readA11 = ReadModbus(ADDR_STN1_CYL1, 1)

                Dim readS31 = ReadModbus(ADDR_STN3_SEN1, 1)
                Dim readS32 = ReadModbus(ADDR_STN3_SEN2, 1)
                Dim readS33 = ReadModbus(ADDR_STN3_SEN3, 1)
                Dim readS34 = ReadModbus(ADDR_STN3_SEN4, 1)
                Dim readA31 = ReadModbus(ADDR_STN3_CYL1, 1)
                Dim readA32 = ReadModbus(ADDR_STN3_CYL2, 1)
                Dim readA33 = ReadModbus(ADDR_STN3_CYL3, 1)
                Dim readA34 = ReadModbus(ADDR_STN3_CYL4, 1)

                Dim readS41 = ReadModbus(ADDR_STN4_SEN1, 1)
                Dim readA41 = ReadModbus(ADDR_STN4_CYL1, 1)

                Dim readS51 = ReadModbus(ADDR_STN5_SEN1, 1)
                Dim readS52 = ReadModbus(ADDR_STN5_SEN2, 1)
                Dim readS53 = ReadModbus(ADDR_STN5_SEN3, 1)
                Dim readA51 = ReadModbus(ADDR_STN5_CYL1, 1)
                Dim readA52 = ReadModbus(ADDR_STN5_CYL2, 1)
                Dim readA53 = ReadModbus(ADDR_STN5_CYL3, 1)

                Dim readS61 = ReadModbus(ADDR_STN6_SEN1, 1)
                Dim readS62 = ReadModbus(ADDR_STN6_SEN2, 1)
                Dim readS63 = ReadModbus(ADDR_STN6_SEN3, 1)
                Dim readA61 = ReadModbus(ADDR_STN6_CYL1, 1)
                Dim readA62 = ReadModbus(ADDR_STN6_CYL2, 1)
                Dim readA63 = ReadModbus(ADDR_STN6_CYL3, 1)

                'Station 1 ----------------------------------------------
                'CylSen 1
                If readS11(0) = FORWARD Then
                    man_stn1_cyl1_max.Image = My.Resources.led_red_on
                    mon_stn1_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn1_cyl1_max.Image = My.Resources.led_red_off
                    mon_stn1_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS11(0) = BACKWARD Then
                    man_stn1_cyl1_min.Image = My.Resources.led_red_on
                    mon_stn1_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn1_cyl1_min.Image = My.Resources.led_red_off
                    mon_stn1_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylAct 1

                If readA11(0) = FORWARD Then
                    ind_stn1_cyl1_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn1_cyl1_fw.Image = My.Resources.led_red_off
                End If

                If readA11(0) = BACKWARD Then
                    ind_stn1_cyl1_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn1_cyl1_bw.Image = My.Resources.led_red_off
                End If

                'Station 3 ----------------------------------------------
                'CylSen 1
                If readS31(0) = FORWARD Then
                    man_stn3_cyl1_max.Image = My.Resources.led_red_on
                    mon_stn3_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn3_cyl1_max.Image = My.Resources.led_red_off
                    mon_stn3_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS31(0) = BACKWARD Then
                    man_stn3_cyl1_min.Image = My.Resources.led_red_on
                    mon_stn3_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn3_cyl1_min.Image = My.Resources.led_red_off
                    mon_stn3_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylSen 2
                If readS32(0) = FORWARD Then
                    man_stn3_cyl2_max.Image = My.Resources.led_red_on
                    mon_stn3_cyl2_max.Image = My.Resources.led_red_on

                Else
                    man_stn3_cyl2_max.Image = My.Resources.led_red_off
                    mon_stn3_cyl2_max.Image = My.Resources.led_red_off
                End If


                If readS32(0) = BACKWARD Then
                    man_stn3_cyl2_min.Image = My.Resources.led_red_on
                    mon_stn3_cyl2_min.Image = My.Resources.led_red_on
                Else
                    man_stn3_cyl2_min.Image = My.Resources.led_red_off
                    mon_stn3_cyl2_min.Image = My.Resources.led_red_off
                End If

                'CylSen 3
                If readS33(0) = FORWARD Then
                    man_stn3_cyl3_max.Image = My.Resources.led_red_on
                    mon_stn3_cyl3_max.Image = My.Resources.led_red_on

                Else
                    man_stn3_cyl3_max.Image = My.Resources.led_red_off
                    mon_stn3_cyl3_max.Image = My.Resources.led_red_off
                End If


                If readS33(0) = BACKWARD Then
                    man_stn3_cyl3_min.Image = My.Resources.led_red_on
                    mon_stn3_cyl3_min.Image = My.Resources.led_red_on
                Else
                    man_stn3_cyl3_min.Image = My.Resources.led_red_off
                    mon_stn3_cyl3_min.Image = My.Resources.led_red_off
                End If

                'CylSen 4
                If readS34(0) = FORWARD Then
                    man_stn3_cyl4_max.Image = My.Resources.led_red_on
                    mon_stn3_cyl4_max.Image = My.Resources.led_red_on

                Else
                    man_stn3_cyl4_max.Image = My.Resources.led_red_off
                    mon_stn3_cyl4_max.Image = My.Resources.led_red_off
                End If


                If readS34(0) = BACKWARD Then
                    man_stn3_cyl4_min.Image = My.Resources.led_red_on
                    mon_stn3_cyl4_min.Image = My.Resources.led_red_on
                Else
                    man_stn3_cyl4_min.Image = My.Resources.led_red_off
                    mon_stn3_cyl4_min.Image = My.Resources.led_red_off
                End If

                'CylAct 1

                If readA31(0) = FORWARD Then
                    ind_stn3_cyl1_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn3_cyl1_fw.Image = My.Resources.led_red_off
                End If

                If readA31(0) = BACKWARD Then
                    ind_stn3_cyl1_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn3_cyl1_bw.Image = My.Resources.led_red_off
                End If

                'CylAct 2

                If readA32(0) = FORWARD Then
                    ind_stn3_cyl2_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn3_cyl2_fw.Image = My.Resources.led_red_off
                End If

                If readA32(0) = BACKWARD Then
                    ind_stn3_cyl2_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn3_cyl2_bw.Image = My.Resources.led_red_off
                End If

                'CylAct 3

                If readA33(0) = FORWARD Then
                    ind_stn3_cyl3_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn3_cyl3_fw.Image = My.Resources.led_red_off
                End If

                If readA33(0) = BACKWARD Then
                    ind_stn3_cyl3_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn3_cyl3_bw.Image = My.Resources.led_red_off
                End If

                'CylAct 4

                If readA34(0) = FORWARD Then
                    ind_stn3_cyl4_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn3_cyl4_fw.Image = My.Resources.led_red_off
                End If

                If readA34(0) = BACKWARD Then
                    ind_stn3_cyl4_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn3_cyl4_bw.Image = My.Resources.led_red_off
                End If

                'Station 4 ----------------------------------------------
                'CylSen 1
                If readS41(0) = FORWARD Then
                    man_stn4_cyl1_max.Image = My.Resources.led_red_on
                    mon_stn4_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn4_cyl1_max.Image = My.Resources.led_red_off
                    mon_stn4_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS41(0) = BACKWARD Then
                    man_stn4_cyl1_min.Image = My.Resources.led_red_on
                    mon_stn4_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn4_cyl1_min.Image = My.Resources.led_red_off
                    mon_stn4_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylAct 1

                If readA41(0) = FORWARD Then
                    ind_stn4_cyl1_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn4_cyl1_fw.Image = My.Resources.led_red_off
                End If

                If readA41(0) = BACKWARD Then
                    ind_stn4_cyl1_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn4_cyl1_bw.Image = My.Resources.led_red_off
                End If

                'Station 5 ----------------------------------------------
                'CylSen 1
                If readS51(0) = FORWARD Then
                    man_stn5_cyl1_max.Image = My.Resources.led_red_on
                    mon_stn5_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn5_cyl1_max.Image = My.Resources.led_red_off
                    mon_stn5_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS51(0) = BACKWARD Then
                    man_stn5_cyl1_min.Image = My.Resources.led_red_on
                    mon_stn5_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn5_cyl1_min.Image = My.Resources.led_red_off
                    mon_stn5_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylSen 2
                If readS52(0) = FORWARD Then
                    man_stn5_cyl2_max.Image = My.Resources.led_red_on
                    mon_stn3_cyl2_max.Image = My.Resources.led_red_on

                Else
                    man_stn5_cyl2_max.Image = My.Resources.led_red_off
                    mon_stn5_cyl2_max.Image = My.Resources.led_red_off
                End If


                If readS52(0) = BACKWARD Then
                    man_stn5_cyl2_min.Image = My.Resources.led_red_on
                    mon_stn5_cyl2_min.Image = My.Resources.led_red_on
                Else
                    man_stn5_cyl2_min.Image = My.Resources.led_red_off
                    mon_stn5_cyl2_min.Image = My.Resources.led_red_off
                End If

                'CylSen 3
                If readS53(0) = FORWARD Then
                    man_stn5_cyl3_max.Image = My.Resources.led_red_on
                    mon_stn5_cyl3_max.Image = My.Resources.led_red_on

                Else
                    man_stn5_cyl3_max.Image = My.Resources.led_red_off
                    mon_stn5_cyl3_max.Image = My.Resources.led_red_off
                End If


                If readS53(0) = BACKWARD Then
                    man_stn5_cyl3_min.Image = My.Resources.led_red_on
                    mon_stn5_cyl3_min.Image = My.Resources.led_red_on
                Else
                    man_stn5_cyl3_min.Image = My.Resources.led_red_off
                    mon_stn5_cyl3_min.Image = My.Resources.led_red_off
                End If

                'CylAct 1

                If readA51(0) = FORWARD Then
                    ind_stn5_cyl1_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn5_cyl1_fw.Image = My.Resources.led_red_off
                End If

                If readA51(0) = BACKWARD Then
                    ind_stn5_cyl1_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn5_cyl1_bw.Image = My.Resources.led_red_off
                End If

                'CylAct 2

                If readA52(0) = FORWARD Then
                    ind_stn5_cyl2_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn5_cyl2_fw.Image = My.Resources.led_red_off
                End If

                If readA52(0) = BACKWARD Then
                    ind_stn5_cyl2_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn5_cyl2_bw.Image = My.Resources.led_red_off
                End If

                'CylAct 3

                If readA53(0) = FORWARD Then
                    ind_stn5_cyl3_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn5_cyl3_fw.Image = My.Resources.led_red_off
                End If

                If readA53(0) = BACKWARD Then
                    ind_stn5_cyl3_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn5_cyl3_bw.Image = My.Resources.led_red_off
                End If

                'Station 6 ----------------------------------------------
                'CylSen 1
                If readS61(0) = FORWARD Then
                    man_stn6_cyl1_max.Image = My.Resources.led_red_on
                    mon_stn6_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn6_cyl1_max.Image = My.Resources.led_red_off
                    mon_stn6_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS61(0) = BACKWARD Then
                    man_stn6_cyl1_min.Image = My.Resources.led_red_on
                    mon_stn6_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn6_cyl1_min.Image = My.Resources.led_red_off
                    mon_stn6_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylSen 2
                If readS62(0) = FORWARD Then
                    man_stn6_cyl2_max.Image = My.Resources.led_red_on
                    mon_stn6_cyl2_max.Image = My.Resources.led_red_on

                Else
                    man_stn6_cyl2_max.Image = My.Resources.led_red_off
                    mon_stn6_cyl2_max.Image = My.Resources.led_red_off
                End If


                If readS62(0) = BACKWARD Then
                    man_stn6_cyl2_min.Image = My.Resources.led_red_on
                    mon_stn6_cyl2_min.Image = My.Resources.led_red_on
                Else
                    man_stn6_cyl2_min.Image = My.Resources.led_red_off
                    mon_stn6_cyl2_min.Image = My.Resources.led_red_off
                End If

                'CylSen 3
                If readS63(0) = FORWARD Then
                    man_stn6_cyl3_max.Image = My.Resources.led_red_on
                    mon_stn6_cyl3_max.Image = My.Resources.led_red_on

                Else
                    man_stn6_cyl3_max.Image = My.Resources.led_red_off
                    mon_stn6_cyl3_max.Image = My.Resources.led_red_off
                End If


                If readS63(0) = BACKWARD Then
                    man_stn6_cyl3_min.Image = My.Resources.led_red_on
                    mon_stn6_cyl3_min.Image = My.Resources.led_red_on
                Else
                    man_stn6_cyl3_min.Image = My.Resources.led_red_off
                    mon_stn6_cyl3_min.Image = My.Resources.led_red_off
                End If

                'CylAct 1

                If readA61(0) = FORWARD Then
                    ind_stn6_cyl1_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn6_cyl1_fw.Image = My.Resources.led_red_off
                End If

                If readA61(0) = BACKWARD Then
                    ind_stn6_cyl1_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn6_cyl1_bw.Image = My.Resources.led_red_off
                End If

                'CylAct 2

                If readA62(0) = FORWARD Then
                    ind_stn6_cyl2_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn6_cyl2_fw.Image = My.Resources.led_red_off
                End If

                If readA62(0) = BACKWARD Then
                    ind_stn6_cyl2_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn6_cyl2_bw.Image = My.Resources.led_red_off
                End If

                'CylAct 3

                If readA63(0) = FORWARD Then
                    ind_stn6_cyl3_fw.Image = My.Resources.led_red_on
                Else
                    ind_stn6_cyl3_fw.Image = My.Resources.led_red_off
                End If

                If readA63(0) = BACKWARD Then
                    ind_stn6_cyl3_bw.Image = My.Resources.led_red_on
                Else
                    ind_stn6_cyl3_bw.Image = My.Resources.led_red_off
                End If
            End If


            Thread.Sleep(100)
        Loop
    End Sub

    '###############################################################################################################################################################################################
    'PLC Writing

    Private Sub ModbusWriter(output As Integer, address As Integer)
        If output = FORWARD Then
            Modbus.WriteModbus(address, FORWARD)
        ElseIf output = BACKWARD Then
            Modbus.WriteModbus(address, BACKWARD)
        Else
            Modbus.WriteModbus(address, IDLE)
        End If
    End Sub

    Private Sub plcWriting()
        Do
            If Connected() Then

                'Station 1

                If STN1_CYL1 <> LAST_STN1_CYL1 Then
                    ModbusWriter(STN1_CYL1, ADDR_STN1_CYL1)
                    LAST_STN1_CYL1 = STN1_CYL1
                End If


                'Station 3

                If STN3_CYL1 <> LAST_STN3_CYL1 Then
                    ModbusWriter(STN3_CYL1, ADDR_STN3_CYL1)
                    LAST_STN3_CYL1 = STN3_CYL1
                End If

                If STN3_CYL2 <> LAST_STN3_CYL2 Then
                    ModbusWriter(STN3_CYL2, ADDR_STN3_CYL2)
                    LAST_STN3_CYL2 = STN3_CYL2
                End If

                If STN3_CYL3 <> LAST_STN3_CYL3 Then
                    ModbusWriter(STN3_CYL3, ADDR_STN3_CYL3)
                    LAST_STN3_CYL3 = STN3_CYL3
                End If

                If STN3_CYL4 <> LAST_STN3_CYL4 Then
                    ModbusWriter(STN3_CYL1, ADDR_STN3_CYL4)
                    LAST_STN3_CYL4 = STN3_CYL4
                End If

                'Station 4

                If STN4_CYL1 <> LAST_STN4_CYL1 Then
                    ModbusWriter(STN4_CYL1, ADDR_STN4_CYL1)
                    LAST_STN4_CYL1 = STN4_CYL1
                End If

                'Station 5

                If STN5_CYL1 <> LAST_STN5_CYL1 Then
                    ModbusWriter(STN5_CYL1, ADDR_STN5_CYL1)
                    LAST_STN5_CYL1 = STN5_CYL1
                End If

                If STN5_CYL2 <> LAST_STN5_CYL2 Then
                    ModbusWriter(STN5_CYL2, ADDR_STN5_CYL2)
                    LAST_STN5_CYL2 = STN5_CYL2
                End If

                If STN5_CYL3 <> LAST_STN5_CYL3 Then
                    ModbusWriter(STN5_CYL3, ADDR_STN5_CYL3)
                    LAST_STN5_CYL3 = STN5_CYL3
                End If

                'Station 6

                If STN6_CYL1 <> LAST_STN6_CYL1 Then
                    ModbusWriter(STN6_CYL1, ADDR_STN6_CYL1)
                    LAST_STN6_CYL1 = STN6_CYL1
                End If

                If STN6_CYL2 <> LAST_STN6_CYL2 Then
                    ModbusWriter(STN6_CYL2, ADDR_STN6_CYL2)
                    LAST_STN6_CYL2 = STN6_CYL2
                End If

                If STN6_CYL3 <> LAST_STN6_CYL3 Then
                    ModbusWriter(STN6_CYL3, ADDR_STN6_CYL3)
                    LAST_STN6_CYL3 = STN6_CYL3
                End If

            End If

            Thread.Sleep(100)

        Loop
    End Sub


End Class

