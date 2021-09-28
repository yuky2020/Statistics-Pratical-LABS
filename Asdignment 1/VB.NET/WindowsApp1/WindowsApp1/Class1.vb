Imports System.Runtime.CompilerServices

Module GraphicsExtensions
    <Extension()>
    Sub DrawCircle(ByVal g As Graphics, ByVal pen As Pen, ByVal centerX As Single, ByVal centerY As Single, ByVal radius As Single)
        g.DrawEllipse(pen, centerX - radius, centerY - radius, radius + radius, radius + radius)
    End Sub

    <Extension()>
    Sub FillCircle(ByVal g As Graphics, ByVal brush As Brush, ByVal centerX As Single, ByVal centerY As Single, ByVal radius As Single)
        g.FillEllipse(brush, centerX - radius, centerY - radius, radius + radius, radius + radius)
    End Sub
End Module
