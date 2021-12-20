Imports Microsoft.Graphics.Canvas
Imports Microsoft.Graphics.Canvas.Text
Imports Windows.UI
Public NotInheritable Class MainPage
    Inherits Page

    Private Sub DrawAsciiBlocksFromText(stIn As String)
        Dim bt2(), bt6(,) As Byte
        Dim lt3, fsize, hsize, pixelWidth As Integer
        Dim st4() As String, sStart As String = txtDelim1.Text
        Dim stDrawChars() As String = {" ", ChrW(&H2584), ChrW(&H2580), ChrW(&H2588)}

        If cboFontSize.SelectedIndex <> -1 Then
            fsize = CInt(cboFontSize.SelectedItem.Content)
        Else
            fsize = 12
        End If
        hsize = fsize / 2 - 1
        If sStart = "" Then sStart = "." 'won't line up right with a blank starting the line
        pixelWidth = stIn.Length * fsize
        ReDim bt6(fsize - 1, pixelWidth - 1)
        ReDim st4(hsize)
        For lt1 = 0 To hsize
            st4(lt1) = sStart
        Next

        Using sx = New CanvasRenderTarget(CanvasDevice.GetSharedDevice, pixelWidth, fsize, 96)
            Using sxx = sx.CreateDrawingSession
                Dim for5 As New CanvasTextFormat With {.HorizontalAlignment = CanvasHorizontalAlignment.Center,
                    .VerticalAlignment = CanvasVerticalAlignment.Center, .FontFamily = "Arial", .FontSize = fsize}
                For lt1 = 0 To stIn.Length - 1
                    sxx.DrawText(stIn(lt1), lt1 * fsize + hsize, hsize, Colors.White, for5)
                Next
            End Using
            bt2 = sx.GetPixelBytes
        End Using

        For lt1 = 3 To bt2.GetUpperBound(0) - 1 Step 4
            If bt2(lt1) > 100 Then bt6(Math.Floor((lt1 - 3) / (pixelWidth * 4)), ((lt1 - 3) Mod (pixelWidth * 4)) / 4) = 1
        Next

        For lt1 = 0 To pixelWidth - 1
            If lt1 < pixelWidth - 3 Then 'cutting the width between letters to two spaces
                For lt3 = 0 To fsize - 1
                    If (bt6(lt3, lt1) <> 0) Or (bt6(lt3, lt1 + 1) <> 0) Or (bt6(lt3, lt1 + 2) <> 0) Then Exit For
                Next
            End If
            If Not (lt3 = fsize) Then 'drawing 
                For lt3 = 0 To hsize
                    st4(lt3) &= stDrawChars(bt6(lt3 * 2, lt1) * 2 + bt6(lt3 * 2 + 1, lt1))
                Next
            End If
        Next

        txtBlock0.Text = ""
        For lt1 = 0 To hsize
            txtBlock0.Text &= st4(lt1) & txtDelim2.Text & vbCrLf
        Next

    End Sub

    Private Sub cmdGo_Click(sender As Object, e As RoutedEventArgs) Handles cmdGo.Click

        If txt1.Text <> "" Then
            DrawAsciiBlocksFromText(txt1.Text)
        End If

    End Sub

End Class
