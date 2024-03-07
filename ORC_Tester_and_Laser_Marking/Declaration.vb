Module Declaration

    Public MODBUS_ERR

    'For Cylinder  ----------------------------------------------
    Public FORWARD As Integer = 1
    Public BACKWARD As Integer = 2
    Public IDLE As Integer = 0


    Public ADDR_STN1_CYL1 As Integer = 5101

    Public STN1_CYL1 As Integer = 0
    Public LAST_STN1_CYL1 As Integer = 0

    Public ADDR_STN3_CYL1 As Integer = 5301
    Public ADDR_STN3_CYL2 As Integer = 5302
    Public ADDR_STN3_CYL3 As Integer = 5303
    Public ADDR_STN3_CYL4 As Integer = 5304

    Public STN3_CYL1 As Integer = 0
    Public STN3_CYL2 As Integer = 0
    Public STN3_CYL3 As Integer = 0
    Public STN3_CYL4 As Integer = 0
    Public LAST_STN3_CYL1 As Integer = 0
    Public LAST_STN3_CYL2 As Integer = 0
    Public LAST_STN3_CYL3 As Integer = 0
    Public LAST_STN3_CYL4 As Integer = 0

    Public ADDR_STN4_CYL1 As Integer = 5401

    Public STN4_CYL1 As Integer = 0
    Public LAST_STN4_CYL1 As Integer = 0

    Public ADDR_STN5_CYL1 As Integer = 5501
    Public ADDR_STN5_CYL2 As Integer = 5502
    Public ADDR_STN5_CYL3 As Integer = 5503

    Public STN5_CYL1 As Integer = 0
    Public STN5_CYL2 As Integer = 0
    Public STN5_CYL3 As Integer = 0
    Public LAST_STN5_CYL1 As Integer = 0
    Public LAST_STN5_CYL2 As Integer = 0
    Public LAST_STN5_CYL3 As Integer = 0

    Public ADDR_STN6_CYL1 As Integer = 5601
    Public ADDR_STN6_CYL2 As Integer = 5602
    Public ADDR_STN6_CYL3 As Integer = 5603

    Public STN6_CYL1 As Integer = 0
    Public STN6_CYL2 As Integer = 0
    Public STN6_CYL3 As Integer = 0
    Public LAST_STN6_CYL1 As Integer = 0
    Public LAST_STN6_CYL2 As Integer = 0
    Public LAST_STN6_CYL3 As Integer = 0

    Public ADDR_STN1_SEN1 As Integer = 6101

    Public ADDR_STN3_SEN1 As Integer = 6301
    Public ADDR_STN3_SEN2 As Integer = 6302
    Public ADDR_STN3_SEN3 As Integer = 6303
    Public ADDR_STN3_SEN4 As Integer = 6304

    Public ADDR_STN4_SEN1 As Integer = 6401

    Public ADDR_STN5_SEN1 As Integer = 6501
    Public ADDR_STN5_SEN2 As Integer = 6502
    Public ADDR_STN5_SEN3 As Integer = 6503

    Public ADDR_STN6_SEN1 As Integer = 6601
    Public ADDR_STN6_SEN2 As Integer = 6602
    Public ADDR_STN6_SEN3 As Integer = 6603

    'For Festo  ----------------------------------------------
    Public UP As Integer = 1
    Public DOWN As Integer = 2

    'Indicator
    Public ADDR_STN3_TRIG_LFESTO = 370
    Public ADDR_STN3_IND_LFESTO = 371

    Public ADDR_STN3_TRIG_RFESTO = 380
    Public ADDR_STN3_IND_RFESTO = 381

    'Data
    Public ADDR_STN3_PSTN_LFESTO = 374 '(Punya 2 address,startnya dari 374)
    Public ADDR_STN3_SPD_LFESTO = 376
    Public ADDR_STN3_ALM_LFESTO = 378

    Public ADDR_STN3_PSTN_RFESTO = 384 '(Punya 2 address,startnya dari 384)
    Public ADDR_STN3_SPD_RFESTO = 386
    Public ADDR_STN3_ALM_RFESTO = 388


End Module
