Imports System.Collections.Generic
Imports System.Runtime.InteropServices

Namespace WindowsFormsApp1
    Friend Class MediaCalOnline
        Private medieAritmetica As Dictionary(Of String, Double)
        Private numeroElementi As Dictionary(Of String, Integer)

        Public Sub New()
            medieAritmetica = New Dictionary(Of String, Double)()
            numeroElementi = New Dictionary(Of String, Integer)()
        End Sub

        Public Sub addAttribute(ByVal nome As String, ByVal value As Double)
            Dim tmp As Double
            Dim i As Integer

            If medieAritmetica.ContainsKey(nome) Then
                medieAritmetica.TryGetValue(nome, tmp)
                numeroElementi.TryGetValue(nome, i)
                i += 1
                tmp = tmp + (value - tmp) / i
                numeroElementi.Remove(nome)
                medieAritmetica.Remove(nome)
                medieAritmetica.Add(nome, tmp)
                numeroElementi.Add(nome, i)
            Else
                medieAritmetica.Add(nome, value)
                numeroElementi.Add(nome, 1)
            End If
        End Sub

        Public Function getMedia(ByVal name As String, <Out> ByRef i As Double) As Boolean
            If medieAritmetica.TryGetValue(name, i) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub addElemento(ByVal e As ElementoDisribuzione)
            For Each item In e.getVariabili()
                addAttribute(item.Key, item.Value)
            Next
        End Sub
    End Class
End Namespace
