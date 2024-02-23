Imports System.Threading


Public Class LoadingForm
    Private Sub LoadingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Task.Run(Sub()
                     For i As Integer = 1 To 100
                         UpdateProgressBar(i)

                         Thread.Sleep(20)
                     Next

                     Me.Invoke(Sub() Me.Close())

                 End Sub)

    End Sub

    Private Sub UpdateProgressBar(value As Integer)
        Me.Invoke(Sub() barLoading.Value = value)

    End Sub
End Class