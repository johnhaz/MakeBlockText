using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Text;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MakeBlockText
{
  
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void DrawAsciiFromFont(string stIn)
        {
            int fsize = Int32.Parse((string)((ComboBoxItem)cboFontSize.SelectedItem).Content);
            int lt3, lt1;
            byte[] bt2;
            int pixelwidth = fsize * stIn.Length;
            byte[,] bt6 = new byte[fsize, pixelwidth];
            string[] st4 = new string[fsize / 2];
            string[] stChars = { " ", "▄", "▀", "█" };
            string startText = txtDelim1.Text;

            if (startText == "")
            {
                startText = ".";
            }
            using (CanvasRenderTarget cRT = new CanvasRenderTarget(CanvasDevice.GetSharedDevice(), pixelwidth, fsize, 96))
            {
                using (CanvasDrawingSession cRTx = cRT.CreateDrawingSession())
                {
                    var ctf = new CanvasTextFormat
                    {
                        HorizontalAlignment = CanvasHorizontalAlignment.Center,
                        VerticalAlignment = CanvasVerticalAlignment.Center,
                        FontSize = fsize
                    };
                    for (lt1 = 0; lt1 < stIn.Length; lt1++)
                    {
                        cRTx.DrawText(stIn[lt1].ToString(), lt1 * fsize + fsize / 2, fsize / 2 - 1, Colors.White, ctf);
                    }
                }
                bt2 = cRT.GetPixelBytes();
            }

            for (lt3 = 0; lt3 < st4.Length; lt3++)
            {
                st4[lt3] = startText;
            }
            for (lt1 = 3; lt1 < bt2.Length; lt1 += 4)
            {
                if (bt2[lt1] > 100)
                {
                    bt6[(int)Math.Floor((double)(lt1 - 3) / (pixelwidth * 4)), ((lt1 - 3) % (pixelwidth * 4)) / 4] = 1;
                }
            }

            for (lt1 = 0; lt1 < pixelwidth - 3; lt1++)
            {
                if (lt1 < (pixelwidth - 3))
                {
                    for (lt3 = 0; lt3 < fsize; lt3++)
                    {
                        if ((bt6[lt3, lt1] != 0) || (bt6[lt3, lt1 + 1] != 0) || (bt6[lt3, lt1 + 2] != 0))
                        {
                            lt3 = fsize;
                        }
                    }
                }
                if (lt3 == (fsize + 1))
                {
                    for (lt3 = 0; lt3 < (fsize / 2); lt3++)
                    {
                        st4[lt3] += stChars[bt6[lt3 * 2 + 1, lt1] + bt6[lt3 * 2, lt1] * 2];
                    }
                }
            }

            txtBlock0.Text = "";
            for (lt1 = 0; lt1 < (fsize / 2); lt1++)
            {
                txtBlock0.Text += st4[lt1] + txtDelim2.Text + '\xD' + '\xA';
            }
        }

        private void cmdGo_Click(object sender, RoutedEventArgs e)
        {
            DrawAsciiFromFont(txt1.Text);
        }
    }
}
