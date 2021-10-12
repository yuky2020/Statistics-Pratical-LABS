Imports System
Imports System.Collections.Generic
Imports System.Runtime.InteropServices

Namespace WindowsFormsApp1
    Friend Class Distribuzione
        Private distr As Dictionary(Of String, SortedDictionary(Of Tuple(Of Double, Double), Integer))
        Private intervall As Double = 10

        Public Sub New()
            distr = New Dictionary(Of String, SortedDictionary(Of Tuple(Of Double, Double), Integer))()
        End Sub

        'intervall standard 10
        Public Sub addAttributDef(ByVal s As String, ByVal i As Double)
            addAttribute(s, i, intervall)
        End Sub

        Public Sub addAttribute(ByVal s As String, ByVal value As Double, ByVal inte As Double)
            Dim actualdistr As SortedDictionary(Of Tuple(Of Double, Double), Integer)
            Dim min, max As Double
            Dim i As Integer = 1
            min = value - value Mod inte
            max = value + (inte - value Mod inte)
            Dim tmp As Tuple(Of Double, Double) = New Tuple(Of Double, Double)(min, max)

            If Not distr.TryGetValue(s, actualdistr) Then
                actualdistr = New SortedDictionary(Of Tuple(Of Double, Double), Integer)()
                actualdistr.Add(tmp, 1)
                distr.Add(s, actualdistr)
            Else

                If Not actualdistr.TryGetValue(tmp, i) Then
                    actualdistr.Add(tmp, 1)
                Else
                    i += 1
                    actualdistr.Remove(tmp)
                    actualdistr.Add(tmp, i)
                End If

                distr.Remove(s)
                distr.Add(s, actualdistr)
            End If
        End Sub

        Public Sub addElemento(ByVal e As ElementoDisribuzione, ByVal inter As Double)
            For Each item In e.getVariabili()
                addAttribute(item.Key, item.Value, inter)
            Next
        End Sub

        Public Sub addElementoDef(ByVal e As ElementoDisribuzione)
            Me.addElemento(e, intervall)
        End Sub

        Public Function getdistribuzione(ByVal s As String, <Out> ByRef req As SortedDictionary(Of Tuple(Of Double, Double), Integer)) As Boolean
            If distr.TryGetValue(s, req) Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class
End Namespace

