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

    'Read Bit
    Public Function ReadBit(addr As Integer, bit As Integer) As Integer
        Dim address_val() As Integer
        address_val = modbus_Client.ReadHoldingRegisters(addr, 1)

        Dim binaryString As String = Convert.ToString(address_val(0), 2).PadLeft(16, "0"c)
        binaryString = binaryString.Reverse.ToArray
        For i As Integer = 0 To binaryString.Length - 1
            If i = bit Then
                Return Val(binaryString(i))
            End If
        Next
    End Function

    'Write Bit
    Public Sub WriteBit(addr As Integer, bit As Integer, val As Integer)
        Dim address_val() As Integer
        address_val = modbus_Client.ReadHoldingRegisters(addr, 1)

        Dim binaryString As String = Convert.ToString(address_val(0), 2).PadLeft(16, "0"c)
        binaryString = binaryString.Reverse.ToArray
        Dim temp(16)
        Dim temp_str As String
        For i As Integer = 0 To binaryString.Length - 1
            If i = bit Then
                temp(i) = val
            Else
                If binaryString(i) = "1" Then
                    temp(i) = 1
                Else
                    temp(i) = 0
                End If
            End If
            temp_str = temp_str + temp(i).ToString
        Next
        temp_str = temp_str.Reverse.ToArray
        Dim integer_val As Integer = Convert.ToInt16(temp_str.ToString, 2)

        modbus_Client.WriteSingleRegister(addr, integer_val)
    End Sub

    'Read Double Integer

    Public Function ReadModbusDInt(addr As Integer, array As Integer) As Integer()
        Try
            If Connected() Then
                Return modbus_Client.ReadHoldingRegisters(addr, array)
            Else
                Console.WriteLine("Not connected to PLC.")
                Return New Integer() {}
            End If
        Catch ex As Exception
            Console.WriteLine("Error reading from Modbus: " & ex.Message)
            Return New Integer() {}
        End Try
    End Function
End Module
