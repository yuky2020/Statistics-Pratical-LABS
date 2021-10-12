Imports System.IO

Public Class Form1



    Private wDstr As SortedDictionary(Of String, Integer) = New SortedDictionary(Of String, Integer)()


    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        Dim filePath = String.Empty
        OpenFileDialog1.InitialDirectory = "c:\"
        OpenFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
        OpenFileDialog1.FilterIndex = 2
        OpenFileDialog1.RestoreDirectory = True
        OpenFileDialog1.Title = "Select Text File"

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            filePath = OpenFileDialog1.FileName

            Using sr As StreamReader = New StreamReader(filePath)

                While sr.Peek() >= 0
                    Dim line As String = sr.ReadLine()
                    Dim delimiters As Char() = New Char() {" "c, vbCr, vbLf}
                    Dim words As String() = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries)
                    Dim i As Integer = 0

                    For Each word As String In words

                        If Not wDstr.TryGetValue(word, i) Then
                            wDstr.Add(word, 1)
                        Else
                            wDstr.Remove(word)
                            i += 1
                            wDstr.Add(word, i)
                        End If
                    Next
                End While
            End Using

            Dim j As Integer = 0

            For Each w In wDstr
                Chart1.Series("Words").Points.AddXY(w.Key, w.Value)
            Next
        End If
    End Sub
End Class