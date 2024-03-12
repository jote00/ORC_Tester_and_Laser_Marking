Imports System.Threading
Imports System.Data.SqlClient
Imports System.IO
Imports ControlBPM

Public Class MainForm

    Dim statioWrite As Thread
    Dim stationRead As Thread
    Dim ManualState As Boolean

    Dim fullPath As String = System.AppDomain.CurrentDomain.BaseDirectory
    Dim projectFolder As String = fullPath.Replace("\ORC_Tester_and_Laser_Marking\bin\Debug\", "").Replace("\ORC_Tester_and_Laser_Marking\bin\Release\", "")
    Dim iniPath As String = projectFolder + "\Config\Config.INI"
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

        With Config
            .dbHostName = ReadINI(iniPath, "DATABASE", "Hostname")
            .dbUsername = ReadINI(iniPath, "DATABASE", "Username")
            .dbPassword = ReadINI(iniPath, "DATABASE", "Password")
            .dbDatabase = ReadINI(iniPath, "DATABASE", "Database")

        End With

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

    'Private Sub btnMonitoring_Click(sender As Object, e As EventArgs) Handles btnMonitoring.Click
    '    ShowTabControl("monitoring")
    'End Sub
    Private Sub btnReferences_Click(sender As Object, e As EventArgs) Handles btnReferences.Click
        ShowTabControl("references")
        LoadTbRef()
        dgv_ref.ColumnHeadersDefaultCellStyle.Font = New Font("Arial", 10)
        dgv_ref.DefaultCellStyle.Font = New Font("Arial", 10)
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

        If mode = "manual" Then
            ShowButtonSTN(1)
            If mode = "manual" Then
                ManualState = True
            Else
                ManualState = False
            End If
        Else
            ShowButtonSTN(0)
        End If

        If mode = "references" Then
            tabReferences.Visible = True
        Else
            tabReferences.Visible = False
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
        If btn_stn3_cyl2_fw.Text = "Is Forward" Then
            btn_stn3_cyl2_fw.PerformClick()
        End If

        If btn_stn3_cyl2_bw.Text = "Backward" Then
            STN3_CYL2 = BACKWARD
            btn_stn3_cyl2_bw.Image = My.Resources.button_white
            btn_stn3_cyl2_bw.Text = "Is Backward"
        ElseIf btn_stn3_cyl2_bw.Text = "Is Backward" Then
            STN3_CYL2 = IDLE
            btn_stn3_cyl2_bw.Image = My.Resources.button_silver
            btn_stn3_cyl2_bw.Text = "Backward"
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

    'Button Manual Station 3.2 Festo



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
            btn_stn4_cyl1_bw.Image = My.Resources.button_white_trnsprnt
            btn_stn4_cyl1_bw.Text = "Is Backward"
        ElseIf btn_stn4_cyl1_bw.Text = "Is Backward" Then
            STN4_CYL1 = IDLE
            btn_stn4_cyl1_bw.Image = My.Resources.button_silver_trnsprnt
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
                ' Dim readA11 = ReadModbus(ADDR_STN1_CYL1, 1)  'STN1

                Dim readS31 = ReadModbus(ADDR_STN3_SEN1, 1)
                Dim readS32 = ReadModbus(ADDR_STN3_SEN2, 1)
                Dim readS33 = ReadModbus(ADDR_STN3_SEN3, 1)
                Dim readS34 = ReadModbus(ADDR_STN3_SEN4, 1)
                'Dim readA31 = ReadModbus(ADDR_STN3_CYL1, 1)
                'Dim readA32 = ReadModbus(ADDR_STN3_CYL2, 1)   'STN3
                'Dim readA33 = ReadModbus(ADDR_STN3_CYL3, 1)
                'Dim readA34 = ReadModbus(ADDR_STN3_CYL4, 1)

                Dim readS41 = ReadModbus(ADDR_STN4_SEN1, 1)
                'Dim readA41 = ReadModbus(ADDR_STN4_CYL1, 1)   'STN4

                Dim readS51 = ReadModbus(ADDR_STN5_SEN1, 1)
                Dim readS52 = ReadModbus(ADDR_STN5_SEN2, 1)
                Dim readS53 = ReadModbus(ADDR_STN5_SEN3, 1)    'STN5
                'Dim readA51 = ReadModbus(ADDR_STN5_CYL1, 1)
                'Dim readA52 = ReadModbus(ADDR_STN5_CYL2, 1)
                'Dim readA53 = ReadModbus(ADDR_STN5_CYL3, 1)

                Dim readS61 = ReadModbus(ADDR_STN6_SEN1, 1)
                Dim readS62 = ReadModbus(ADDR_STN6_SEN2, 1)
                Dim readS63 = ReadModbus(ADDR_STN6_SEN3, 1)    'STN6
                'Dim readA61 = ReadModbus(ADDR_STN6_CYL1, 1)
                'Dim readA62 = ReadModbus(ADDR_STN6_CYL2, 1)
                'Dim readA63 = ReadModbus(ADDR_STN6_CYL3, 1)

                Dim readLALM As Integer = ReadBit(ADDR_STN3_IND_LFESTO, 0)
                Dim readLPEND As Integer = ReadBit(ADDR_STN3_IND_LFESTO, 1)
                Dim readLHEND As Integer = ReadBit(ADDR_STN3_IND_LFESTO, 2)   'Indicator LFesto
                Dim readLSVON As Integer = ReadBit(ADDR_STN3_IND_LFESTO, 3)
                Dim readLEMG As Integer = ReadBit(ADDR_STN3_IND_LFESTO, 4)

                Dim readRALM As Integer = ReadBit(ADDR_STN3_IND_RFESTO, 0)
                Dim readRPEND As Integer = ReadBit(ADDR_STN3_IND_RFESTO, 1)
                Dim readRHEND As Integer = ReadBit(ADDR_STN3_IND_RFESTO, 2)   'Indicator RFesto
                Dim readRSVON As Integer = ReadBit(ADDR_STN3_IND_RFESTO, 3)
                Dim readREMG As Integer = ReadBit(ADDR_STN3_IND_RFESTO, 4)



                'Text Box Festo read ----------------------------------------------
                Me.Invoke(Sub()

                              tbx_Lfesto_position.Text = ReadDoubleAddrees(ADDR_STN3_PSTN_LFESTO)
                              tbx_Lfesto_speed.Text = ReadModbus(ADDR_STN3_SPD_LFESTO, 1)(0)
                              tbx_Lfesto_alarm.Text = ReadModbus(ADDR_STN3_ALM_LFESTO, 1)(0)

                              tbx_Rfesto_position.Text = ReadDoubleAddrees(ADDR_STN3_PSTN_RFESTO)
                              tbx_Rfesto_speed.Text = ReadModbus(ADDR_STN3_SPD_RFESTO, 1)(0)
                              tbx_Rfesto_alarm.Text = ReadModbus(ADDR_STN3_ALM_RFESTO, 1)(0)
                          End Sub)






                'Station 1 ----------------------------------------------
                'CylSen 1
                If readS11.Length > 0 AndAlso readS11(0) = FORWARD Then
                    man_stn1_cyl1_max.Image = My.Resources.led_green_on
                    'mon_stn1_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn1_cyl1_max.Image = My.Resources.led_red_on
                    'mon_stn1_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS11.Length > 0 AndAlso readS11(0) = BACKWARD Then
                    man_stn1_cyl1_min.Image = My.Resources.led_green_on
                    'mon_stn1_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn1_cyl1_min.Image = My.Resources.led_red_on
                    'mon_stn1_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylAct 1

                'If readA11(0) = FORWARD Then
                '    ind_stn1_cyl1_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn1_cyl1_fw.Image = My.Resources.led_red_off
                'End If

                'If readA11(0) = BACKWARD Then
                '    ind_stn1_cyl1_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn1_cyl1_bw.Image = My.Resources.led_red_off
                'End If

                'Station 3 ----------------------------------------------
                'CylSen 1
                If readS31.Length > 0 AndAlso readS31(0) = FORWARD Then
                    man_stn3_cyl1_max.Image = My.Resources.led_green_on
                    'mon_stn3_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn3_cyl1_max.Image = My.Resources.led_red_on
                    'mon_stn3_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS31.Length > 0 AndAlso readS31(0) = BACKWARD Then
                    man_stn3_cyl1_min.Image = My.Resources.led_green_on
                    'mon_stn3_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn3_cyl1_min.Image = My.Resources.led_red_on
                    'mon_stn3_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylSen 2
                If readS32.Length > 0 AndAlso readS32(0) = FORWARD Then
                    man_stn3_cyl2_max.Image = My.Resources.led_green_on
                    ' mon_stn3_cyl2_max.Image = My.Resources.led_red_on

                Else
                    man_stn3_cyl2_max.Image = My.Resources.led_red_on
                    ' mon_stn3_cyl2_max.Image = My.Resources.led_red_off
                End If


                If readS32.Length > 0 AndAlso readS32(0) = BACKWARD Then
                    man_stn3_cyl2_min.Image = My.Resources.led_green_on
                    'mon_stn3_cyl2_min.Image = My.Resources.led_red_on
                Else
                    man_stn3_cyl2_min.Image = My.Resources.led_red_on
                    'mon_stn3_cyl2_min.Image = My.Resources.led_red_off
                End If

                'CylSen 3
                If readS33.Length > 0 AndAlso readS33(0) = FORWARD Then
                    man_stn3_cyl3_max.Image = My.Resources.led_green_on
                    'mon_stn3_cyl3_max.Image = My.Resources.led_red_on

                Else
                    man_stn3_cyl3_max.Image = My.Resources.led_red_on
                    'mon_stn3_cyl3_max.Image = My.Resources.led_red_off
                End If


                If readS33.Length > 0 AndAlso readS33(0) = BACKWARD Then
                    man_stn3_cyl3_min.Image = My.Resources.led_green_on
                    'mon_stn3_cyl3_min.Image = My.Resources.led_red_on
                Else
                    man_stn3_cyl3_min.Image = My.Resources.led_red_on
                    'mon_stn3_cyl3_min.Image = My.Resources.led_red_off
                End If

                'CylSen 4
                If readS34.Length > 0 AndAlso readS34(0) = FORWARD Then
                    man_stn3_cyl4_max.Image = My.Resources.led_green_on
                    'mon_stn3_cyl4_max.Image = My.Resources.led_red_on

                Else
                    man_stn3_cyl4_max.Image = My.Resources.led_red_on
                    'mon_stn3_cyl4_max.Image = My.Resources.led_red_off
                End If


                If readS34.Length > 0 AndAlso readS34(0) = BACKWARD Then
                    man_stn3_cyl4_min.Image = My.Resources.led_green_on
                    'mon_stn3_cyl4_min.Image = My.Resources.led_red_on
                Else
                    man_stn3_cyl4_min.Image = My.Resources.led_red_on
                    'mon_stn3_cyl4_min.Image = My.Resources.led_red_off
                End If

                'Festo Left
                If readLALM = 1 Then
                    ind_stn3_Lfesto_alm.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Lfesto_alm.Image = My.Resources.led_red_on
                End If

                If readLPEND = 1 Then
                    ind_stn3_Lfesto_pend.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Lfesto_pend.Image = My.Resources.led_red_on
                End If

                If readLHEND = 1 Then
                    ind_stn3_Lfesto_hend.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Lfesto_hend.Image = My.Resources.led_red_on
                End If

                If readLSVON = 1 Then
                    ind_stn3_Lfesto_svon.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Lfesto_svon.Image = My.Resources.led_red_on
                End If

                If readLEMG = 1 Then
                    ind_stn3_Lfesto_emg.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Lfesto_emg.Image = My.Resources.led_red_on
                End If

                'Festo Right
                If readRALM = 1 Then
                    ind_stn3_Rfesto_alm.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Rfesto_alm.Image = My.Resources.led_red_on
                End If

                If readRPEND = 1 Then
                    ind_stn3_Rfesto_pend.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Rfesto_pend.Image = My.Resources.led_red_on
                End If

                If readRHEND = 1 Then
                    ind_stn3_Rfesto_hend.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Rfesto_hend.Image = My.Resources.led_red_on
                End If

                If readRSVON = 1 Then
                    ind_stn3_Rfesto_svon.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Rfesto_svon.Image = My.Resources.led_red_on
                End If

                If readREMG = 1 Then
                    ind_stn3_Rfesto_emg.Image = My.Resources.led_green_on
                Else
                    ind_stn3_Rfesto_emg.Image = My.Resources.led_red_on
                End If


                'CylAct 1

                'If readA31(0) = FORWARD Then
                '    ind_stn3_cyl1_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn3_cyl1_fw.Image = My.Resources.led_red_off
                'End If

                'If readA31(0) = BACKWARD Then
                '    ind_stn3_cyl1_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn3_cyl1_bw.Image = My.Resources.led_red_off
                'End If

                'CylAct 2

                'If readA32(0) = FORWARD Then
                '    ind_stn3_cyl2_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn3_cyl2_fw.Image = My.Resources.led_red_off
                'End If

                'If readA32(0) = BACKWARD Then
                '    ind_stn3_cyl2_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn3_cyl2_bw.Image = My.Resources.led_red_off
                'End If

                'CylAct 3

                'If readA33(0) = FORWARD Then
                '    ind_stn3_cyl3_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn3_cyl3_fw.Image = My.Resources.led_red_off
                'End If

                'If readA33(0) = BACKWARD Then
                '    ind_stn3_cyl3_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn3_cyl3_bw.Image = My.Resources.led_red_off
                'End If

                'CylAct 4

                'If readA34(0) = FORWARD Then
                '    ind_stn3_cyl4_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn3_cyl4_fw.Image = My.Resources.led_red_off
                'End If

                'If readA34(0) = BACKWARD Then
                '    ind_stn3_cyl4_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn3_cyl4_bw.Image = My.Resources.led_red_off
                'End If

                'Station 4 ----------------------------------------------
                'CylSen 1
                If readS41.Length > 0 AndAlso readS41(0) = FORWARD Then
                    man_stn4_cyl1_max.Image = My.Resources.led_green_on
                    'mon_stn4_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn4_cyl1_max.Image = My.Resources.led_red_on
                    'mon_stn4_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS41.Length > 0 AndAlso readS41(0) = BACKWARD Then
                    man_stn4_cyl1_min.Image = My.Resources.led_green_on
                    'mon_stn4_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn4_cyl1_min.Image = My.Resources.led_red_on
                    'mon_stn4_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylAct 1

                'If readA41(0) = FORWARD Then
                '    ind_stn4_cyl1_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn4_cyl1_fw.Image = My.Resources.led_red_off
                'End If

                'If readA41(0) = BACKWARD Then
                '    ind_stn4_cyl1_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn4_cyl1_bw.Image = My.Resources.led_red_off
                'End If

                'Station 5 ----------------------------------------------
                'CylSen 1
                If readS51.Length > 0 AndAlso readS51(0) = FORWARD Then
                    man_stn5_cyl1_max.Image = My.Resources.led_green_on
                    'mon_stn5_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn5_cyl1_max.Image = My.Resources.led_red_on
                    'mon_stn5_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS51.Length > 0 AndAlso readS51(0) = BACKWARD Then
                    man_stn5_cyl1_min.Image = My.Resources.led_green_on
                    'mon_stn5_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn5_cyl1_min.Image = My.Resources.led_red_on
                    'mon_stn5_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylSen 2
                If readS52.Length > 0 AndAlso readS52(0) = FORWARD Then
                    man_stn5_cyl2_max.Image = My.Resources.led_green_on
                    'mon_stn3_cyl2_max.Image = My.Resources.led_red_on

                Else
                    man_stn5_cyl2_max.Image = My.Resources.led_red_on
                    'mon_stn5_cyl2_max.Image = My.Resources.led_red_off
                End If


                If readS52.Length > 0 AndAlso readS52(0) = BACKWARD Then
                    man_stn5_cyl2_min.Image = My.Resources.led_green_on
                    'mon_stn5_cyl2_min.Image = My.Resources.led_red_on
                Else
                    man_stn5_cyl2_min.Image = My.Resources.led_red_on
                    'mon_stn5_cyl2_min.Image = My.Resources.led_red_off
                End If

                'CylSen 3
                If readS53.Length > 0 AndAlso readS53(0) = FORWARD Then
                    man_stn5_cyl3_max.Image = My.Resources.led_green_on
                    'mon_stn5_cyl3_max.Image = My.Resources.led_red_on

                Else
                    man_stn5_cyl3_max.Image = My.Resources.led_red_on
                    'mon_stn5_cyl3_max.Image = My.Resources.led_red_off
                End If


                If readS53.Length > 0 AndAlso readS53(0) = BACKWARD Then
                    man_stn5_cyl3_min.Image = My.Resources.led_green_on
                    'mon_stn5_cyl3_min.Image = My.Resources.led_red_on
                Else
                    man_stn5_cyl3_min.Image = My.Resources.led_red_on
                    'mon_stn5_cyl3_min.Image = My.Resources.led_red_off
                End If

                'CylAct 1

                'If readA51(0) = FORWARD Then
                '    ind_stn5_cyl1_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn5_cyl1_fw.Image = My.Resources.led_red_off
                'End If

                'If readA51(0) = BACKWARD Then
                '    ind_stn5_cyl1_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn5_cyl1_bw.Image = My.Resources.led_red_off
                'End If

                'CylAct 2

                'If readA52(0) = FORWARD Then
                '    ind_stn5_cyl2_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn5_cyl2_fw.Image = My.Resources.led_red_off
                'End If

                'If readA52(0) = BACKWARD Then
                '    ind_stn5_cyl2_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn5_cyl2_bw.Image = My.Resources.led_red_off
                'End If

                'CylAct 3

                'If readA53(0) = FORWARD Then
                '    ind_stn5_cyl3_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn5_cyl3_fw.Image = My.Resources.led_red_off
                'End If

                'If readA53(0) = BACKWARD Then
                '    ind_stn5_cyl3_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn5_cyl3_bw.Image = My.Resources.led_red_off
                'End If

                'Station 6 ----------------------------------------------
                'CylSen 1
                If readS61.Length > 0 AndAlso readS61(0) = FORWARD Then
                    man_stn6_cyl1_max.Image = My.Resources.led_green_on
                    ' mon_stn6_cyl1_max.Image = My.Resources.led_red_on

                Else
                    man_stn6_cyl1_max.Image = My.Resources.led_red_on
                    ' mon_stn6_cyl1_max.Image = My.Resources.led_red_off
                End If


                If readS61.Length > 0 AndAlso readS61(0) = BACKWARD Then
                    man_stn6_cyl1_min.Image = My.Resources.led_green_on
                    'mon_stn6_cyl1_min.Image = My.Resources.led_red_on
                Else
                    man_stn6_cyl1_min.Image = My.Resources.led_red_on
                    'mon_stn6_cyl1_min.Image = My.Resources.led_red_off
                End If

                'CylSen 2
                If readS62.Length > 0 AndAlso readS62(0) = FORWARD Then
                    man_stn6_cyl2_max.Image = My.Resources.led_green_on
                    'mon_stn6_cyl2_max.Image = My.Resources.led_red_on

                Else
                    man_stn6_cyl2_max.Image = My.Resources.led_red_on
                    'mon_stn6_cyl2_max.Image = My.Resources.led_red_off
                End If


                If readS62.Length > 0 AndAlso readS62(0) = BACKWARD Then
                    man_stn6_cyl2_min.Image = My.Resources.led_green_on
                    'mon_stn6_cyl2_min.Image = My.Resources.led_red_on
                Else
                    man_stn6_cyl2_min.Image = My.Resources.led_red_on
                    'mon_stn6_cyl2_min.Image = My.Resources.led_red_off
                End If

                'CylSen 3
                If readS63.Length > 0 AndAlso readS63(0) = FORWARD Then
                    man_stn6_cyl3_max.Image = My.Resources.led_green_on
                    'mon_stn6_cyl3_max.Image = My.Resources.led_red_on

                Else
                    man_stn6_cyl3_max.Image = My.Resources.led_red_on
                    'mon_stn6_cyl3_max.Image = My.Resources.led_red_off
                End If


                If readS63.Length > 0 AndAlso readS63(0) = BACKWARD Then
                    man_stn6_cyl3_min.Image = My.Resources.led_green_on
                    'mon_stn6_cyl3_min.Image = My.Resources.led_red_on
                Else
                    man_stn6_cyl3_min.Image = My.Resources.led_red_on
                    'mon_stn6_cyl3_min.Image = My.Resources.led_red_off
                End If

                'CylAct 1

                'If readA61(0) = FORWARD Then
                '    ind_stn6_cyl1_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn6_cyl1_fw.Image = My.Resources.led_red_off
                'End If

                'If readA61(0) = BACKWARD Then
                '    ind_stn6_cyl1_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn6_cyl1_bw.Image = My.Resources.led_red_off
                'End If

                'CylAct 2

                'If readA62(0) = FORWARD Then
                '    ind_stn6_cyl2_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn6_cyl2_fw.Image = My.Resources.led_red_off
                'End If

                'If readA62(0) = BACKWARD Then
                '    ind_stn6_cyl2_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn6_cyl2_bw.Image = My.Resources.led_red_off
                'End If

                'CylAct 3

                'If readA63(0) = FORWARD Then
                '    ind_stn6_cyl3_fw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn6_cyl3_fw.Image = My.Resources.led_red_off
                'End If

                'If readA63(0) = BACKWARD Then
                '    ind_stn6_cyl3_bw.Image = My.Resources.led_red_on
                'Else
                '    ind_stn6_cyl3_bw.Image = My.Resources.led_red_off
                'End If
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
                    ModbusWriter(STN3_CYL4, ADDR_STN3_CYL4)
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


    '###############################################################################################################################################################################################
    'Festo Controller Button

    'Festo Left ----------------------------------------------
    'Alarm Reset 
    Private Sub btn_Lfesto_alarm_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_alarm.MouseDown
        WriteBit(370, 0, 1)
        btn_Lfesto_alarm.Tag = New Tuple(Of Color, Color)(btn_Lfesto_alarm.BackColor, btn_Lfesto_alarm.ForeColor)
        btn_Lfesto_alarm.BackColor = SystemColors.GradientInactiveCaption
        btn_Lfesto_alarm.ForeColor = Color.DarkBlue

    End Sub

    Private Sub btn_Lfesto_alarm_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_alarm.MouseUp
        WriteBit(370, 0, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Lfesto_alarm.Tag, Tuple(Of Color, Color))
        btn_Lfesto_alarm.BackColor = originalColors.Item1
        btn_Lfesto_alarm.ForeColor = originalColors.Item2
    End Sub


    'Servo On
    Private Sub btn_Lfesto_servo_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_servo.MouseDown
        WriteBit(370, 1, 1)
        btn_Lfesto_servo.Tag = New Tuple(Of Color, Color)(btn_Lfesto_servo.BackColor, btn_Lfesto_servo.ForeColor)
        btn_Lfesto_servo.BackColor = SystemColors.GradientInactiveCaption
        btn_Lfesto_servo.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Lfesto_servo_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_servo.MouseUp
        WriteBit(370, 1, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Lfesto_servo.Tag, Tuple(Of Color, Color))
        btn_Lfesto_servo.BackColor = originalColors.Item1
        btn_Lfesto_servo.ForeColor = originalColors.Item2
    End Sub

    'Jog Min
    Private Sub btn_Lfesto_jog_min_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_jog_min.MouseDown
        WriteBit(370, 2, 1)
        btn_Lfesto_jog_min.Tag = New Tuple(Of Color, Color)(btn_Lfesto_jog_min.BackColor, btn_Lfesto_jog_min.ForeColor)
        btn_Lfesto_jog_min.BackColor = SystemColors.GradientInactiveCaption
        btn_Lfesto_jog_min.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Lfesto_jog_min_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_jog_min.MouseUp
        WriteBit(370, 2, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Lfesto_jog_min.Tag, Tuple(Of Color, Color))
        btn_Lfesto_jog_min.BackColor = originalColors.Item1
        btn_Lfesto_jog_min.ForeColor = originalColors.Item2
    End Sub

    'Jog Plus
    Private Sub btn_Lfesto_jog_plus_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_jog_plus.MouseDown
        WriteBit(370, 3, 1)
        btn_Lfesto_jog_plus.Tag = New Tuple(Of Color, Color)(btn_Lfesto_jog_plus.BackColor, btn_Lfesto_jog_plus.ForeColor)
        btn_Lfesto_jog_plus.BackColor = SystemColors.GradientInactiveCaption
        btn_Lfesto_jog_plus.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Lfesto_jog_plus_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_jog_plus.MouseUp
        WriteBit(370, 3, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Lfesto_jog_plus.Tag, Tuple(Of Color, Color))
        btn_Lfesto_jog_plus.BackColor = originalColors.Item1
        btn_Lfesto_jog_plus.ForeColor = originalColors.Item2
    End Sub

    'Homing
    Private Sub btn_Lfesto_homing_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_homing.MouseDown
        WriteBit(370, 4, 1)
        btn_Lfesto_homing.Tag = New Tuple(Of Color, Color)(btn_Lfesto_homing.BackColor, btn_Lfesto_homing.ForeColor)
        btn_Lfesto_homing.BackColor = SystemColors.GradientInactiveCaption
        btn_Lfesto_homing.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Lfesto_homing_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_homing.MouseUp
        WriteBit(370, 4, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Lfesto_homing.Tag, Tuple(Of Color, Color))
        btn_Lfesto_homing.BackColor = originalColors.Item1
        btn_Lfesto_homing.ForeColor = originalColors.Item2
    End Sub

    'Jisl
    Private Sub btn_Lfesto_jisl_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_jisl.MouseDown
        WriteBit(370, 5, 1)
        btn_Lfesto_jisl.Tag = New Tuple(Of Color, Color)(btn_Lfesto_jisl.BackColor, btn_Lfesto_jisl.ForeColor)
        btn_Lfesto_jisl.BackColor = SystemColors.GradientInactiveCaption
        btn_Lfesto_jisl.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Lfesto_jisl_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_jisl.MouseUp
        WriteBit(370, 5, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Lfesto_jisl.Tag, Tuple(Of Color, Color))
        btn_Lfesto_jisl.BackColor = originalColors.Item1
        btn_Lfesto_jisl.ForeColor = originalColors.Item2
    End Sub

    'Power Reset
    Private Sub btn_Lfesto_power_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_power.MouseDown
        WriteBit(370, 6, 1)
        btn_Lfesto_power.Tag = New Tuple(Of Color, Color)(btn_Lfesto_power.BackColor, btn_Lfesto_power.ForeColor)
        btn_Lfesto_power.BackColor = SystemColors.GradientInactiveCaption
        btn_Lfesto_power.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Lfesto_power_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Lfesto_power.MouseUp
        WriteBit(370, 6, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Lfesto_power.Tag, Tuple(Of Color, Color))
        btn_Lfesto_power.BackColor = originalColors.Item1
        btn_Lfesto_power.ForeColor = originalColors.Item2
    End Sub

    'Festo Right ----------------------------------------------
    'Alarm Reset 
    Private Sub btn_Rfesto_alarm_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_alarm.MouseDown
        WriteBit(380, 0, 1)
        btn_Rfesto_alarm.Tag = New Tuple(Of Color, Color)(btn_Rfesto_alarm.BackColor, btn_Rfesto_alarm.ForeColor)
        btn_Rfesto_alarm.BackColor = SystemColors.GradientInactiveCaption
        btn_Rfesto_alarm.ForeColor = Color.DarkBlue

    End Sub

    Private Sub btn_Rfesto_alarm_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_alarm.MouseUp
        WriteBit(380, 0, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Rfesto_alarm.Tag, Tuple(Of Color, Color))
        btn_Rfesto_alarm.BackColor = originalColors.Item1
        btn_Rfesto_alarm.ForeColor = originalColors.Item2
    End Sub


    'Servo On
    Private Sub btn_Rfesto_servo_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_servo.MouseDown
        WriteBit(380, 1, 1)
        btn_Rfesto_servo.Tag = New Tuple(Of Color, Color)(btn_Rfesto_servo.BackColor, btn_Rfesto_servo.ForeColor)
        btn_Rfesto_servo.BackColor = SystemColors.GradientInactiveCaption
        btn_Rfesto_servo.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Rfesto_servo_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_servo.MouseUp
        WriteBit(380, 1, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Rfesto_servo.Tag, Tuple(Of Color, Color))
        btn_Rfesto_servo.BackColor = originalColors.Item1
        btn_Rfesto_servo.ForeColor = originalColors.Item2
    End Sub

    'Jog Min
    Private Sub btn_Rfesto_jog_min_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_jog_min.MouseDown
        WriteBit(380, 2, 1)
        btn_Rfesto_jog_min.Tag = New Tuple(Of Color, Color)(btn_Rfesto_jog_min.BackColor, btn_Rfesto_jog_min.ForeColor)
        btn_Rfesto_jog_min.BackColor = SystemColors.GradientInactiveCaption
        btn_Rfesto_jog_min.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Rfesto_jog_min_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_jog_min.MouseUp
        WriteBit(380, 2, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Rfesto_jog_min.Tag, Tuple(Of Color, Color))
        btn_Rfesto_jog_min.BackColor = originalColors.Item1
        btn_Rfesto_jog_min.ForeColor = originalColors.Item2
    End Sub

    'Jog Plus
    Private Sub btn_Rfesto_jog_plus_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_jog_plus.MouseDown
        WriteBit(380, 3, 1)
        btn_Rfesto_jog_plus.Tag = New Tuple(Of Color, Color)(btn_Rfesto_jog_plus.BackColor, btn_Rfesto_jog_plus.ForeColor)
        btn_Rfesto_jog_plus.BackColor = SystemColors.GradientInactiveCaption
        btn_Rfesto_jog_plus.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Rfesto_jog_plus_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_jog_plus.MouseUp
        WriteBit(380, 3, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Rfesto_jog_plus.Tag, Tuple(Of Color, Color))
        btn_Rfesto_jog_plus.BackColor = originalColors.Item1
        btn_Rfesto_jog_plus.ForeColor = originalColors.Item2
    End Sub

    'Homing
    Private Sub btn_Rfesto_homing_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_homing.MouseDown
        WriteBit(380, 4, 1)
        btn_Rfesto_homing.Tag = New Tuple(Of Color, Color)(btn_Rfesto_homing.BackColor, btn_Rfesto_homing.ForeColor)
        btn_Rfesto_homing.BackColor = SystemColors.GradientInactiveCaption
        btn_Rfesto_homing.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Rfesto_homing_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_homing.MouseUp
        WriteBit(380, 4, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Rfesto_homing.Tag, Tuple(Of Color, Color))
        btn_Rfesto_homing.BackColor = originalColors.Item1
        btn_Rfesto_homing.ForeColor = originalColors.Item2
    End Sub

    'Jisl
    Private Sub btn_Rfesto_jisl_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_jisl.MouseDown
        WriteBit(380, 5, 1)
        btn_Rfesto_jisl.Tag = New Tuple(Of Color, Color)(btn_Rfesto_jisl.BackColor, btn_Rfesto_jisl.ForeColor)
        btn_Rfesto_jisl.BackColor = SystemColors.GradientInactiveCaption
        btn_Rfesto_jisl.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Rfesto_jisl_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_jisl.MouseUp
        WriteBit(380, 5, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Rfesto_jisl.Tag, Tuple(Of Color, Color))
        btn_Rfesto_jisl.BackColor = originalColors.Item1
        btn_Rfesto_jisl.ForeColor = originalColors.Item2
    End Sub

    'Power Reset
    Private Sub btn_Rfesto_power_MouseDown(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_power.MouseDown
        WriteBit(380, 6, 1)
        btn_Rfesto_power.Tag = New Tuple(Of Color, Color)(btn_Rfesto_power.BackColor, btn_Rfesto_power.ForeColor)
        btn_Rfesto_power.BackColor = SystemColors.GradientInactiveCaption
        btn_Rfesto_power.ForeColor = Color.DarkBlue
    End Sub

    Private Sub btn_Rfesto_power_MouseUp(sender As Object, e As MouseEventArgs) Handles btn_Rfesto_power.MouseUp
        WriteBit(380, 6, 0)
        Dim originalColors As Tuple(Of Color, Color) = DirectCast(btn_Rfesto_power.Tag, Tuple(Of Color, Color))
        btn_Rfesto_power.BackColor = originalColors.Item1
        btn_Rfesto_power.ForeColor = originalColors.Item2
    End Sub


    '###############################################################################################################################################################################################
    'Tab References

    Private Sub LoadTbRef()
        Call ConnectionDB.connection_db()
        Try
            Dim sc As New SqlCommand("SELECT * FROM tbl_References order by [References] asc", ConnectionDB.Connection)
            Dim adapter As New SqlDataAdapter(sc)
            Dim ds As New DataSet

            adapter.Fill(ds)
            dgv_ref.DataSource = ds.Tables(0)
        Catch ex As Exception
            MsgBox(Date.Now.ToString("dd/MM/yyyy - hh:mm:ss => ") + "Error: Database not found!")
        End Try
    End Sub

    Private Sub btn_add_Click(sender As Object, e As EventArgs) Handles btn_add.Click
        If tbx_ref.Text = "" And tbx_punching.Text = "" And tbx_lvl_dist.Text = "" And tbx_lvl_toler.Text = "" And tbx_oring.Text = "" And tbx_Ldist.Text = "" And tbx_Rdist.Text = "" And tbx_Lspeed.Text = "" And tbx_Rspeed.Text = "" And tbx_laser_template.Text = "" And tbx_camera_program.Text = "" Then
            MsgBox("Please Fill All Field!")
            Exit Sub

        Else
            Call ConnectionDB.connection_db()
            Dim sc As New SqlCommand("INSERT INTO tb_reference ([References], [Punching Mode], [Level Distance], [Level Tolerance], [Oring Check], [Festo LEFT Distance], [Festo Right Distance], [Festo LEFT Speed], [Fest RIGHT Speed], [Laser Template], [Camera Program]) VALUES('" & tbx_ref.Text & "', '" & tbx_punching.Text & "', '" & tbx_lvl_dist.Text & "', '" & tbx_lvl_toler.Text & "', '" & tbx_oring.Text & "', '" & tbx_Ldist.Text & "', '" & tbx_Rdist.Text & "', '" & tbx_Lspeed.Text & "', '" & tbx_Rspeed.Text & "', '" & tbx_laser_template.Text & "', '" & tbx_camera_program.Text & "')", ConnectionDB.Connection)
            Dim adapter As New SqlDataAdapter(sc)
            adapter.SelectCommand.ExecuteNonQuery()
            LoadTbRef()
        End If
    End Sub

    Private Sub btn_update_Click(sender As Object, e As EventArgs) Handles btn_update.Click
        If tbx_ref.Text = "" Then
            MsgBox("Please Fill Product References")
            Exit Sub
        Else
            Call ConnectionDB.connection_db()
            Dim sc As New SqlCommand("UPDATE tbl_References SET [Punching Mode] = '" & tbx_punching.Text & "', [Level Distance] = '" & tbx_lvl_dist.Text & "', [Level Tolerance] = '" & tbx_lvl_toler.Text & "', [Oring Check] = '" & tbx_oring.Text & "', [Festo LEFT Distance] = '" & tbx_Ldist.Text & "', [Festo Right Distance] = '" & tbx_Rdist.Text & "', [Festo Left Speed] = '" & tbx_Lspeed.Text & "', [Festo RIGHT Speed] = '" & tbx_Rspeed.Text & "', [Laser Template] = '" & tbx_laser_template.Text & "', [Camera Program] = '" & tbx_camera_program.Text & "' WHERE [References] = '" & tbx_ref.Text & "'", ConnectionDB.Connection)
            Dim adapter As New SqlDataAdapter(sc)
            adapter.SelectCommand.ExecuteNonQuery()
            LoadTbRef()
        End If
    End Sub

    Private Sub btn_delete_Click(sender As Object, e As EventArgs) Handles btn_delete.Click
        If tbx_ref.Text = "" Then
            MsgBox("Please Fill Product References")
            Exit Sub
        Else
            Call ConnectionDB.connection_db()
            Dim sc As New SqlCommand("DELETE from tbl_References where [References] = '" & tbx_ref.Text & "'", ConnectionDB.Connection)
            Dim adapter As New SqlDataAdapter(sc)
            adapter.SelectCommand.ExecuteNonQuery()
            LoadTbRef()
        End If
    End Sub

    Private Sub dgv_ref_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgv_ref.CellDoubleClick
        Call ConnectionDB.connection_db()
        Dim sc As New SqlCommand("SELECT * FROM tbl_References WHERE [References]='" & dgv_ref.CurrentCell.Value & "'", ConnectionDB.Connection)
        Dim rd As SqlDataReader = sc.ExecuteReader
        rd.Read()

        tbx_ref.Text = dgv_ref.CurrentCell.Value
        tbx_punching.Text = rd.Item("Punching Mode")
        tbx_lvl_dist.Text = rd.Item("Level Distance")
        tbx_lvl_toler.Text = rd.Item("Level Tolerance")
        tbx_oring.Text = rd.Item("Oring Check")
        tbx_Ldist.Text = rd.Item("Festo LEFT Distance")
        tbx_Rdist.Text = rd.Item("Festo RIGHT Distance")
        tbx_Lspeed.Text = rd.Item("Festo LEFT Speed")
        tbx_Rspeed.Text = rd.Item("Festo RIGHT Speed")
        tbx_laser_template.Text = rd.Item("Laser Template")
        tbx_camera_program.Text = rd.Item("Camera Program")
    End Sub
End Class

