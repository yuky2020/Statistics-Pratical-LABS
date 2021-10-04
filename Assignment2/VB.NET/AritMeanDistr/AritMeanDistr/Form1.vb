
Imports AritMeanDistr.WindowsFormsApp1

Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load


    End Sub

    Private Sub RichTextBox2_TextChanged(sender As Object, e As EventArgs) Handles RichTextBox2.TextChanged

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Timer1.Interval = 2
        Timer1.Start()
        Button3.Visible = True
        Button2.Visible = True
        Button1.Visible = False
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Timer2.Interval = 10
        Timer2.Start()
        Button2.Visible = False
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Timer1.[Stop]()
        Timer2.[Stop]()
        Button1.Visible = True
        Button2.Visible = True
    End Sub

    Private r As Random = New Random()
    Private mc As MediaCalOnline = New MediaCalOnline()
    Private dc As Distribuzione = New Distribuzione()


    Private Sub Timer1_Tick_1(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim height As Double
        Dim studente As ElementoDisribuzione = New ElementoDisribuzione("Student")
        height = r.NextDouble() + r.Next(140, 200)
        studente.setVariable("height", height)
        mc.addElemento(studente)
        dc.addElementoDef(studente)
        RichTextBox1.Text += "nuovo Studente altezza: " & height & Environment.NewLine
    End Sub

    Private Sub Timer2_Tick_1(sender As Object, e As EventArgs) Handles Timer2.Tick
        Dim i As Double
        Dim distTest As SortedDictionary(Of Tuple(Of Double, Double), Integer) = New SortedDictionary(Of Tuple(Of Double, Double), Integer)()
        mc.getMedia("height", i)

        If dc.getdistribuzione("height", distTest) Then
            RichTextBox3.Text = ""

            For Each item In distTest
                RichTextBox3.Text += item.Key.Item1 & "-" & item.Key.Item2 & " presenta " & item.Value & " entita " & Environment.NewLine
            Next
        End If

        RichTextBox2.Text = "the A M of the height is" & i & Environment.NewLine
    End Sub
End Class
