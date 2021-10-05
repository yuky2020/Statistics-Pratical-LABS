using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        bool allowResize = false;
        bool allowMove = false;
        private void resizaileBox_MouseUp(object sender, MouseEventArgs e)
        {
            allowResize = false;
            allowMove = false;
        }

        private void resizaileBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (allowResize)
            {
                this.panel1.Height = resizaileBox.Top + e.Y;
                this.panel1.Width = resizaileBox.Left + e.X;
            }
            if (allowMove)
            {
                this.panel1.Location = new Point(this.panel1.Location.X + e.X, this.panel1.Location.Y + e.Y);
                

                
            }
        }

        private void resizaileBox_Click(object sender, EventArgs e)
        {

        }

        private void resizaileBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) allowResize = true;
            else allowMove = true;
        }

       

    }
}
