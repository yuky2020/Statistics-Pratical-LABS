Imports Microsoft.VisualBasic.FileIO

Public Class Form1

    Private csvContent As Dictionary(Of Integer, ElementoDisribuzione) = New Dictionary(Of Integer, ElementoDisribuzione)()
    Private distr As Distribuzione = New Distribuzione()
    Private medie As MediaCalOnline = New MediaCalOnline()
    Private attributename As List(Of String) = New List(Of String)()
    Private bivariante As List(Of String) = New List(Of String)()

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim filePath = String.Empty
        OpenFileDialog1.InitialDirectory = "c:\"
        OpenFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
        OpenFileDialog1.FilterIndex = 2
        OpenFileDialog1.RestoreDirectory = True

        If OpenFileDialog1.ShowDialog() = DialogResult.OK Then
            filePath = OpenFileDialog1.FileName

            Using csvParser As TextFieldParser = New TextFieldParser(filePath)
                csvParser.CommentTokens = New String() {"#"}
                csvParser.SetDelimiters(New String() {","})
                csvParser.HasFieldsEnclosedInQuotes = True
                Dim fieldsNames As String() = csvParser.ReadFields()
                attributename.AddRange(fieldsNames)
                Dim i As Integer = 0

                While Not csvParser.EndOfData
                    Dim fields As String() = csvParser.ReadFields()
                    Dim elem As ElementoDisribuzione = New ElementoDisribuzione(fields(0))
                    Dim j As Integer = 0

                    For Each field As String In fields

                        If Not String.IsNullOrEmpty(field) Then
                            Dim tmp As Double

                            If Double.TryParse(field, tmp) Then
                                elem.setVariable(fieldsNames(j), New Tuple(Of Object, Type)(tmp, tmp.[GetType]()))
                                medie.addAttribute(fieldsNames(j), tmp)
                            Else
                                elem.setVariable(fieldsNames(j), New Tuple(Of Object, Type)(field, field.[GetType]()))
                            End If
                        End If

                        j += 1
                    Next

                    csvContent.Add(i, elem)
                    distr.addElementoDef(elem)
                    i += 1
                End While
            End Using

            Button2.Visible = True
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        For Each elem In attributename
            ContextMenuStrip1.Items.Add(elem)
        Next

        ContextMenuStrip1.Visible = True
    End Sub


    Private Sub contextMenuStrip1_ItemClicked(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs)

    End Sub



    Private Sub ContextMenuStrip1_ItemClicked_1(sender As Object, e As ToolStripItemClickedEventArgs) Handles ContextMenuStrip1.ItemClicked
        RichTextBox1.Text += "-------------------------------------------------------------" & Environment.NewLine
        RichTextBox1.Text += e.ClickedItem.Text + Environment.NewLine
        Dim media As Double = 0
        Dim stdVariation As Double
        Dim values As List(Of Double) = New List(Of Double)(0)
        Dim firstdistr As SortedDictionary(Of Tuple(Of Double, Double), Integer)
        medie.getMedia(e.ClickedItem.Text, media)

        If media <> 0 Then
            Dim tmp As Tuple(Of Object, Type)

            For Each elm In csvContent.Values
                elm.getVariable(e.ClickedItem.Text, tmp)
                values.Add(CDbl(tmp.Item1))
            Next

            medie.getStandardDeviation(e.ClickedItem.Text, values, stdVariation)
            distr.getdistribuzioneN(e.ClickedItem.Text, firstdistr)

            For Each el In firstdistr
                RichTextBox1.Text += el.Key.ToString() & " : " & el.Value & Environment.NewLine
            Next
        Else
            stdVariation = 0
        End If

        RichTextBox1.Text += e.ClickedItem.Text & " with average: " & media & " and standard variation: " & stdVariation & Environment.NewLine
        bivariante.Add(e.ClickedItem.Text)
        Button2.Visible = False

        For Each elem In attributename
            ContextMenuStrip2.Items.Add(elem)
        Next

        ContextMenuStrip2.Visible = True
    End Sub

    Private Sub ContextMenuStrip2_ItemClicked_1(sender As Object, e As ToolStripItemClickedEventArgs) Handles ContextMenuStrip2.ItemClicked
        RichTextBox1.Text += "-------------------------------------------------------------" & Environment.NewLine
        RichTextBox1.Text += e.ClickedItem.Text & Environment.NewLine
        Dim media2 As Double = 0
        Dim stdVariation As Double
        Dim values As List(Of Double) = New List(Of Double)(0)
        Dim seconddistr As SortedDictionary(Of Tuple(Of Double, Double), Integer)
        Dim isDouble As Boolean = False
        medie.getMedia(e.ClickedItem.Text, media2)

        If media2 <> 0 Then
            isDouble = True
            Dim tmp As Tuple(Of Object, Type)

            For Each elm In csvContent.Values
                elm.getVariable(e.ClickedItem.Text, tmp)
                values.Add(CDbl(tmp.Item1))
            Next

            medie.getStandardDeviation(e.ClickedItem.Text, values, stdVariation)
            distr.getdistribuzioneN(e.ClickedItem.Text, seconddistr)

            For Each el In seconddistr
                RichTextBox1.Text += el.Key.ToString() & " : " & el.Value & Environment.NewLine
            Next
        Else
            stdVariation = 0
        End If

        RichTextBox1.Text += e.ClickedItem.Text & " with average: " & media2 & " and standard variation: " & stdVariation & Environment.NewLine
        bivariante.Add(e.ClickedItem.Text)
        RichTextBox1.Text += Environment.NewLine
        Dim nr, nc As Integer
        Dim bivariantMatrix As String(,) = distr.getbivariantmatrix(bivariante, csvContent.Values, nr, nc)

        For i As Integer = 0 To nr
            RichTextBox1.Text += Environment.NewLine

            For j As Integer = 0 To nc
                RichTextBox1.Text += bivariantMatrix(i, j).PadLeft(6)
            Next
        Next
    End Sub
End Class
