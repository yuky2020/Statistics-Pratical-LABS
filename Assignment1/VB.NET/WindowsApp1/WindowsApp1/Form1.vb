Public Class Form1
    Public Sub New()
        InitializeComponent()
        Timer1.Interval = 3

    End Sub

    Private r As Rectangle
    Private g As Graphics
    Private x As Integer = 0
    Private y As Integer = 0


    Private Sub MyFristGUI_Load(ByVal sender As Object, ByVal e As EventArgs)
    End Sub




    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim rand = New Random()
        x = rand.[Next](280, 660)
        y = rand.[Next](220, 330)
        g.Clear(BackColor)
        g.DrawRectangle(New Pen(Brushes.Cornsilk, 6), r)
        g.DrawCircle(New Pen(Brushes.Cornsilk, 2), x, y, 20)
        g.FillCircle(Brushes.Cornsilk, x, y, 20)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.RichTextBox1.Text = "button pressed"
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        r = New Rectangle(260, 200, 400, 150)
        g = Me.CreateGraphics()
        Timer1.Interval = 3



        Timer1.Start()
            Me.Button2.Visible = False
            Me.Button3.Text = "STOP"
        Me.Button3.Visible = True
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Timer1.Stop()
        g.Clear(BackColor)
        Me.Button3.Visible = False
        Me.Button2.Visible = True
    End Sub
End Class
