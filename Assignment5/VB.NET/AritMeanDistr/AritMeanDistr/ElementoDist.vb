Imports System.Collections.Generic
Imports System.Runtime.InteropServices

Namespace WindowsFormsApp1
    Friend Class ElementoDisribuzione
        Private name As String
        Private variabili As Dictionary(Of String, Double)

        Public Sub New(ByVal nome As String)
            name = nome
            variabili = New Dictionary(Of String, Double)()
        End Sub

        Public Sub setVariable(ByVal name As String, ByVal value As Double)
            variabili.Add(name, value)
        End Sub

        Public Function getVariable(ByVal name As String, <Out> ByRef ret As Double) As Boolean
            If variabili.TryGetValue(name, ret) Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Function getVariabili() As Dictionary(Of String, Double)
            Return variabili
        End Function
    End Class
End Namespace
