Imports EasyModbus

Module Modbus
    Dim modbus_Client As ModbusClient = New ModbusClient()
    Dim _Connected As Boolean


    Public Sub ManualConnection()
        modbus_Client.IPAddress = MainForm.txt_ip_plc.Text
        modbus_Client.Port = Convert.ToInt32(MainForm.txt_port_plc.Text)
        modbus_Client.Connect()
        If modbus_Client.Connected Then
            _Connected = True
        End If
    End Sub

    Public Sub AutoConnection()

        Dim ipAddress As String = "127.0.0.1"
        Dim port As Integer = 502

        Try
            modbus_Client.IPAddress = ipAddress
            modbus_Client.Port = port
            modbus_Client.Connect()

            If modbus_Client.Connected Then
                _Connected = True
            End If
        Catch ex As Exception
            Console.WriteLine("Error connecting automatically to PLC: " & ex.Message)
        End Try

    End Sub

    Public Function Connected()
        Return _Connected
    End Function

    Public Sub Disconnect()
        modbus_Client.Disconnect()
        If Not modbus_Client.Connected Then
            _Connected = False

        End If
    End Sub


    Public Function ReadModbus(StartReg As Integer, RegLength As Integer) As Integer()
        Try
            If Connected() Then
                Return modbus_Client.ReadHoldingRegisters(StartReg, RegLength)
            Else
                Console.WriteLine("Not connected to PLC.")
                Return New Integer() {}
            End If
        Catch ex As Exception
            Console.WriteLine("Error reading from Modbus: " & ex.Message)
            Return New Integer() {}
        End Try
    End Function

    Public Sub WriteModbus(RegAddress As Integer, RegValue As Integer)
        Try
            If Connected() Then
                modbus_Client.WriteSingleRegister(RegAddress, RegValue)
            Else
                Console.WriteLine("Not connected to PLC.")
            End If
        Catch ex As Exception
            Console.WriteLine("Error writing to Modbus: " & ex.Message)
        End Try
    End Sub

End Module
