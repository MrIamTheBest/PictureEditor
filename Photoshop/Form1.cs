using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;
namespace Photoshop
{
    public partial class Form1 : Form
    {
        private int szer = 0, wys = 0;
        private Bitmap b1, b2, KopiaPicture, OrygPicture;
        Color kolor;
        bool rysuj = false;
        bool wybierz = false;
        int x, y, lx, ly = 0;
        float kontrast = 0;
        Item current;
        public Form1()
        {
            InitializeComponent();
        }
        public enum Item
        {
            Rectangle, Elipse, Line, Brush, Eraser, Picker
        }

        private void otwórzToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Load(openFileDialog1.FileName);
                szer = pictureBox1.Image.Width;
                wys = pictureBox1.Image.Height;
                b1 = (Bitmap)pictureBox1.Image;
                b2 = new Bitmap(b1);

            }
        }

        private void pictureBox3_MouseDown(object sender, MouseEventArgs e)
        {
            wybierz = true;
        }

        private void pictureBox3_MouseMove(object sender, MouseEventArgs e)
        {

            if (wybierz)
            {
                Bitmap bmp = (Bitmap)pictureBox3.Image.Clone();
                kolor = bmp.GetPixel(e.X, e.Y);
                red.Value = kolor.R;
                blue.Value = kolor.B;
                green.Value = kolor.G;
                alpha.Value = kolor.A;
                redval.Text = kolor.R.ToString();
                blueval.Text = kolor.B.ToString();
                greenval.Text = kolor.G.ToString();
                alphaval.Text = kolor.A.ToString();
                pictureBox2.BackColor = kolor;
            }
        }

        private void pictureBox3_MouseUp(object sender, MouseEventArgs e)
        {
            wybierz = false;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            rysuj = true;
            x = e.X;
            y = e.Y;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {

            if (rysuj)
            {
                Graphics g = pictureBox1.CreateGraphics();
                switch (current)
                {
                    case Item.Rectangle:
                        g.FillRectangle(new SolidBrush(kolor), x, y, e.X - x, e.Y - y);
                        break;
                    case Item.Elipse:
                        g.FillEllipse(new SolidBrush(kolor), x, y, e.X - x, e.Y - y);
                        break;
                    case Item.Brush:
                        g.FillEllipse(new SolidBrush(kolor), e.X - x + x, e.Y - y + y, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox1.Text));
                        break;
                    case Item.Eraser:
                        g.FillEllipse(new SolidBrush(pictureBox1.BackColor), e.X - x + x, e.Y - y + y, Convert.ToInt32(textBox1.Text), Convert.ToInt32(textBox1.Text));
                        break;
                }
                g.Dispose();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            rysuj = false;
            lx = e.X;
            ly = e.Y;
            if (current == Item.Line)
            {
                Graphics g = pictureBox1.CreateGraphics();
                g.DrawLine(new Pen(new SolidBrush(kolor)), new Point(x, y), new Point(lx, ly));
                g.Dispose();
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (current == Item.Picker)
            {
                Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                Graphics g = Graphics.FromImage(bmp);
                Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
                g.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
                g.Dispose();
                kolor = bmp.GetPixel(e.X, e.Y);
                pictureBox2.BackColor = kolor;
                red.Value = kolor.R;
                blue.Value = kolor.B;
                green.Value = kolor.G;
                alpha.Value = kolor.A;
                redval.Text = kolor.R.ToString();
                blueval.Text = kolor.B.ToString();
                greenval.Text = kolor.G.ToString();
                alphaval.Text = kolor.A.ToString();
                bmp.Dispose();
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            current = Item.Elipse;
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            current = Item.Brush;
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            current = Item.Eraser;
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

            current = Item.Line;
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            current = Item.Rectangle;
        }

        private void pictureBox5_Click_1(object sender, EventArgs e)
        {
            current = Item.Picker;
        }

        private void czyśćToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }

        private void zapiszToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Graphics g = Graphics.FromImage(bmp);
            Rectangle rect = pictureBox1.RectangleToScreen(pictureBox1.ClientRectangle);
            g.CopyFromScreen(rect.Location, Point.Empty, pictureBox1.Size);
            g.Dispose();
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(saveFileDialog1.FileName);

            }
        }

        private void red_Scroll(object sender, EventArgs e)
        {
            kolor = Color.FromArgb(red.Value, green.Value, blue.Value, alpha.Value);
            pictureBox2.BackColor = kolor;
            redval.Text = "R: " + kolor.R.ToString();
        }

        private void green_Scroll(object sender, EventArgs e)
        {
            kolor = Color.FromArgb(red.Value, green.Value, blue.Value, alpha.Value);
            pictureBox2.BackColor = kolor;
            greenval.Text = "G: " + kolor.G.ToString();
        }

        private void blue_Scroll(object sender, EventArgs e)
        {
            kolor = Color.FromArgb(red.Value, green.Value, blue.Value, alpha.Value);
            pictureBox2.BackColor = kolor;
            blueval.Text = "B: " + kolor.B.ToString();
        }

        private void alpha_Scroll(object sender, EventArgs e)
        {
            kolor = Color.FromArgb(red.Value, green.Value, blue.Value, alpha.Value);
            pictureBox2.BackColor = kolor;
            alphaval.Text = "A: " + kolor.A.ToString();
        }

        private void cofnijToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int srednia;
            for (int i = 0; i < szer; i++)
            {
                for (int j = 0; j < wys; j++)
                {
                    kolor = b1.GetPixel(i, j);
                    srednia = (kolor.R + kolor.G + kolor.B) / 3;
                    kolor = Color.FromArgb(srednia, srednia, srednia);
                    b1.SetPixel(i, j, kolor);
                }
            }
            pictureBox1.Refresh();
        }

        private void powtórzToolStripMenuItem_Click(object sender, EventArgs e)
        {

            for (int x = 0; x < szer; x++)
            {
                for (int y = 0; y < wys; y++)
                {
                    b1.SetPixel(x, y, Color.FromArgb(255 - b1.GetPixel(x, y).R, 255 - b1.GetPixel(x, y).G, 255 - b1.GetPixel(x, y).B));
                }
            }
            pictureBox1.Refresh();
        }

        private void czarnobiałeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int srednia;
            for (int i = 0; i < szer; i++)
            {
                for (int j = 0; j < wys; j++)
                {
                    kolor = b1.GetPixel(i, j);
                    srednia = (kolor.R + kolor.G + kolor.B) / 3;
                    kolor = Color.FromArgb(srednia, srednia, srednia);
                    b1.SetPixel(i, j, kolor);
                }
            }
            pictureBox1.Refresh();
        }

        private void negatywToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < szer; x++)
            {
                for (int y = 0; y < wys; y++)
                {
                    b1.SetPixel(x, y, Color.FromArgb(255 - b1.GetPixel(x, y).R, 255 - b1.GetPixel(x, y).G, 255 - b1.GetPixel(x, y).B));
                }
            }
            pictureBox1.Refresh();

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            int n = trackBar2.Value;
            float r, g, b;
            if (n > 0 && n < 127)
            {
                for (int x = 0; x < szer; x++)
                {
                    for (int y = 0; y < wys; y++)
                    {
                        kolor = b2.GetPixel(x, y);
                        r = (127 * (kolor.R - n)) / (127 - n);
                        if (r > 255)
                        {
                            r = 255;
                        }
                        else if (r < 0)
                        {
                            r = 0;
                        }
                        g = (127 * (kolor.G - n)) / (127 - n);
                        if (g > 255)
                        {
                            g = 255;
                        }
                        else if (g < 0)
                        {
                            g = 0;
                        }
                        b = (127 * (kolor.B - n)) / (127 - n);
                        if (b > 255)
                        {
                            b = 255;
                        }
                        else if (b < 0)
                        {
                            b = 0;
                        }
                        b1.SetPixel(x, y, Color.FromArgb((int)r, (int)g, (int)b));
                    }
                }
            }
            else if (n < 0)
            {
                for (int i = 0; i < szer; i++)
                {
                    for (int j = 0; j < wys; j++)
                    {
                        kolor = b2.GetPixel(i, j);
                        r = ((127 + n) * (kolor.R - n)) / 127;
                        g = ((127 + n) * (kolor.G - n)) / 127;
                        b = ((127 + n) * (kolor.B - n)) / 127;

                        b1.SetPixel(i, j, Color.FromArgb((int)r, (int)g, (int)b));
                    }

                }
            }
            pictureBox1.Refresh();
        }
    }
}
