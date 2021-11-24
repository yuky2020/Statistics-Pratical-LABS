using System;
using System.Drawing;
using System.Windows.Forms;

namespace BernulliGraphics
/*
Given 2 variables from a csv compute and represent the statistical regression lines (X to Y and viceversa) and the scatterplot.
Optionally, represent also the histograms on the "sides" of the chart (one could be draw vertically and the other one horizontally, in the position that you prefer).
[Remember that all our charts must alway be done within "dynamic viewports" (movable/resizable rectangles). No third party libraries, to ensure ownership of creative process. May choose the language you prefer.].
*/
{
    public partial class BernulliGraphics : Form
    {
        public BernulliGraphics()
        {
            InitializeComponent();
            contb = new Bitmap(755, 681);
            g2 = Graphics.FromImage(contb);
            comboBox1.SelectedIndex = 1;
            comboBox2.SelectedIndex = 0;

        }



        Bitmap contb;
        Graphics g2;

        bool movable = false;
        bool resiable = false;
        // movable view port
        int contgleft = 0;
        int contgtop = 50;
        int contgwid = 400;
        int contgheight = 400;
        int mouseDeltax = 0;
        int mouseDeltay = 0;



        Rectangle viewPortc = new Rectangle(0, 50, 600, 600);






        private void button1_Click(object sender, EventArgs e)


        {
            creaGrafici();


        }








        private void creaGrafici()
        {
            bool is12A = false;
            if (comboBox1.SelectedIndex == 0) is12A = true;

            int n = (int)this.numericUpDown1.Value;
            int m = (int)this.numericUpDown2.Value;

            int j = 40;


            DisegnaGrafici gr = new DisegnaGrafici(m, n, j, g2, viewPortc, contgleft, contgtop, contgwid, contgheight, is12A,comboBox2.SelectedIndex);
            gr.getGrapichs();
            pictureBox2.Image = contb;


        }



        private void pictureBox2_MouseDown(object sender, MouseEventArgs e)
        {
            if ((e.Location.Y >= viewPortc.Top && e.Location.Y <= (viewPortc.Top + viewPortc.Height)) && (e.Location.X >= viewPortc.Left && e.Location.X <= (viewPortc.Left + viewPortc.Width)))
            {
                if (e.Button == MouseButtons.Left)
                    movable = true;
                if (e.Button == MouseButtons.Right)
                    resiable = true;
                mouseDeltax = e.Location.X;
                mouseDeltay = e.Location.Y;

            }


        }

        private void pictureBox2_MouseLeave(object sender, EventArgs e)
        {
            resiable = false;
            movable = false;
        }

        private void pictureBox2_MouseMove(object sender, MouseEventArgs e)
        {
            if (movable == true)
            {

                contgleft = e.Location.X;
                contgtop = e.Location.Y;
                if (contgleft <= 0) contgleft = 0;
                if (contgtop <= 20) contgtop = 20;
                creaGrafici();
            }
            if (resiable == true)
            {
                mouseDeltax = -mouseDeltax + e.Location.X;
                mouseDeltay = -mouseDeltay + e.Location.Y;
                contgwid += mouseDeltax / 40;
                contgheight += mouseDeltay / 40;
                creaGrafici();
            }






        }



        private void pictureBox2_MouseUp(object sender, MouseEventArgs e)
        {
            resiable = false;
            movable = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            creaGrafici();
            if (comboBox1.SelectedIndex == 1)
            {
                comboBox2.Visible = true;
            }
            else
            {
                comboBox2.Visible = false;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            creaGrafici();
        }
    }
}
