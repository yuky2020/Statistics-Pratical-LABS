using MyVeryfirstCSharpProgram.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyVeryfirstCSharpProgram
{
    public partial class MyFristGUI : Form
    {
        public MyFristGUI()
        {
            InitializeComponent();

            t.Interval = 4;
            flag = new Bitmap(480, 160);
            g = Graphics.FromImage(flag);
            t.Tick += T_Tick;

        }
        Bitmap flag;
        Graphics g;
        int x = 20;
        int y = 20;
        Timer t = new Timer();
       

        private void button1_Click(object sender, EventArgs e)
        {
            this.richTextBox1.Text = "button pressed";
        }

     
        private void T_Tick(object sender, EventArgs e)
        {    
            var rand2 = new Random();
            g = Graphics.FromImage(flag);
            g.Clear(Color.White);
            g.DrawCircle(new Pen(Brushes.Red, 2), x, y, 20);
            g.FillCircle(Brushes.Red, x, y, 20);
           
            x = x+rand2.Next(-10, 10);
            y = y+ rand2.Next(-10,10);
            if (x > 460) x = x - 10;
            if (x < 20) x = x + 10;
            if (y > 140) y = y - 10;
            if (y < 20) y = y + 10;
            pictureBox2.Image = flag;

         
           
        }



        private void MyFristGUI_Load(object sender, EventArgs e)
        {


        }

        private void button2_MouseClick(object sender, MouseEventArgs e)
        {
            
            var rand = new Random();
            x = rand.Next(20, 460);
            y = rand.Next(20, 140);
            t.Start();
            this.button2.Visible = false;
            this.button3.Text = "STOP";
            this.button3.Visible = true;
        }

        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            t.Stop();
            g.Clear(BackColor);
            this.button3.Visible = false;
            this.button2.Visible = true;
            pictureBox2.Image = flag;
        }

       
    }
}
