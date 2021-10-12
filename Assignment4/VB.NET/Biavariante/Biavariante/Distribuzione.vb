

Imports System.Runtime.InteropServices
Imports Biavariante.ElementoDisribuzione


Public Class Distribuzione
    Private distrN As Dictionary(Of String, SortedDictionary(Of Tuple(Of Double, Double), Integer))
    Private distrS As Dictionary(Of String, SortedDictionary(Of String, Integer))
    Private intervall As Double = 10

    Public Sub New()
        distrN = New Dictionary(Of String, SortedDictionary(Of Tuple(Of Double, Double), Integer))()
        distrS = New Dictionary(Of String, SortedDictionary(Of String, Integer))()
    End Sub

    Public Sub addAttributNDef(ByVal s As String, ByVal i As Double)
        addAttributeN(s, i, intervall)
    End Sub

    Public Sub addAttributeN(ByVal s As String, ByVal value As Double, ByVal inte As Double)
        Dim actualdistr As SortedDictionary(Of Tuple(Of Double, Double), Integer)
        Dim min, max As Double
        Dim i As Integer = 1
        min = value - (value Mod inte)
        max = value + (inte - (value Mod inte))
        Dim tmp As Tuple(Of Double, Double) = New Tuple(Of Double, Double)(min, max)

        If Not distrN.TryGetValue(s, actualdistr) Then
            actualdistr = New SortedDictionary(Of Tuple(Of Double, Double), Integer)()
            actualdistr.Add(tmp, 1)
            distrN.Add(s, actualdistr)
        Else

            If Not actualdistr.TryGetValue(tmp, i) Then
                actualdistr.Add(tmp, 1)
            Else
                i += 1
                actualdistr.Remove(tmp)
                actualdistr.Add(tmp, i)
            End If

            distrN.Remove(s)
            distrN.Add(s, actualdistr)
        End If
    End Sub

    Private Sub addAttributeS(ByVal key As String, ByVal value As String)
        Dim actualdistrS As SortedDictionary(Of String, Integer)
        Dim i As Integer = 1

        If Not distrS.TryGetValue(key, actualdistrS) Then
            actualdistrS = New SortedDictionary(Of String, Integer)()
            actualdistrS.Add(value, 1)
            distrS.Add(key, actualdistrS)
        Else

            If Not actualdistrS.TryGetValue(value, i) Then
                actualdistrS.Add(value, 1)
            Else
                i += 1
                actualdistrS.Remove(value)
                actualdistrS.Add(value, i)
            End If

            distrS.Remove(key)
            distrS.Add(key, actualdistrS)
        End If
    End Sub

    Public Sub addElemento(ByVal e As ElementoDisribuzione, ByVal inter As Double)
        For Each item In e.getVariabili()

            If item.Value.Item2 = GetType(String) Then
                Me.addAttributeS(item.Key, CType(item.Value.Item1, String))
            Else
                Me.addAttributeN(item.Key, CDbl(item.Value.Item1), inter)
            End If
        Next
    End Sub

    Public Sub addElementoDef(ByVal e As ElementoDisribuzione)
        addElemento(e, intervall)
    End Sub

    Public Function getdistribuzioneN(ByVal s As String, <Out> ByRef req As SortedDictionary(Of Tuple(Of Double, Double), Integer)) As Boolean
        If distrN.TryGetValue(s, req) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function getdistribuzioneS(ByVal s As String, <Out> ByRef req As SortedDictionary(Of String, Integer)) As Boolean
        If distrS.TryGetValue(s, req) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function getbivariantmatrix(ByVal bivariante As List(Of String), ByVal values As Dictionary(Of Integer, ElementoDisribuzione).ValueCollection, <Out> ByRef numeroRighe As Integer, <Out> ByRef numeroColonnne As Integer) As String(,)
        Dim variabili As String() = bivariante.ToArray()
        Dim columns As SortedDictionary(Of Tuple(Of Double, Double), Integer)
        Dim rows As SortedDictionary(Of Tuple(Of Double, Double), Integer)
        Dim i As Integer, j As Integer = 0
        distrN.TryGetValue(variabili(0), columns)
        distrN.TryGetValue(variabili(1), rows)
        Dim intervalloRow As Double = 0

        While intervalloRow = 0
            intervalloRow = rows.ElementAt(j).Key.Item2 - rows.ElementAt(j).Key.Item1
            j += 1
        End While

        j = 0
        Dim intervalloCol As Double = 0

        While intervalloCol = 0
            intervalloCol = columns.ElementAt(j).Key.Item2 - columns.ElementAt(j).Key.Item1
            j += 1
        End While

        numeroColonnne = columns.Count()
        numeroRighe = rows.Count()
        Dim outputM As String(,) = New String(numeroRighe + 1 - 1, numeroColonnne + 1 - 1) {}
        Dim outputMV As Integer(,) = New Integer(numeroRighe - 1, numeroColonnne - 1) {}

        For i = 0 To numeroRighe - 1

            For j = 0 To numeroColonnne - 1
                outputMV(i, j) = 0
            Next
        Next

        i = 1
        j = 1
        outputM(0, 0) = " "

        For Each column In columns.Keys
            outputM(0, i) = column.ToString()
            i += 1
        Next

        For Each row In rows.Keys
            outputM(j, 0) = row.ToString()
            j += 1
        Next

        For Each elm In values
            Dim trov0 As Boolean = False
            Dim trov1 As Boolean = False
            Dim tmpv0 As Tuple(Of Object, Type)
            Dim tmpv1 As Tuple(Of Object, Type)
            elm.getVariable(variabili(0), tmpv0)
            elm.getVariable(variabili(1), tmpv1)
            i = 0
            j = 0

            While Not trov0 AndAlso i < numeroColonnne

                If (CDbl(tmpv0.Item1) - columns.Keys.ElementAt(i).Item1) <= intervalloCol Then
                    trov0 = True
                Else
                    i += 1
                End If
            End While

            While Not trov1 AndAlso j < numeroRighe

                If CDbl(tmpv1.Item1) - rows.Keys.ElementAt(j).Item1 <= intervalloCol Then
                    trov1 = True
                Else
                    j += 1
                End If
            End While

            If trov0 AndAlso trov1 Then outputMV(j, i) += 1
        Next

        For i = 1 To numeroColonnne

            For j = 1 To numeroRighe
                outputM(j, i) = outputMV(j - 1, i - 1).ToString()
            Next
        Next

        Return outputM
    End Function
End Class

