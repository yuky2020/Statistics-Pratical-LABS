
Imports System.Runtime.InteropServices

Public Class ElementoDisribuzione
    Private name As String
    Private variabili As Dictionary(Of String, Tuple(Of Object, Type))

    Public Sub New(ByVal nome As String)
        Me.name = nome
        Me.variabili = New Dictionary(Of String, Tuple(Of Object, Type))()
    End Sub

    Public Sub setVariable(ByVal name As String, ByVal s As Tuple(Of Object, Type))
        Me.variabili.Add(name, s)
    End Sub

    Public Function getVariable(ByVal name As String, <Out> ByRef ret As Tuple(Of Object, Type)) As Boolean
        If Me.variabili.TryGetValue(name, ret) Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function getVariabili() As Dictionary(Of String, Tuple(Of Object, Type))
        Return Me.variabili
    End Function
End Class

