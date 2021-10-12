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
        Random r = new Random();
        MediaCalOnline mc=new MediaCalOnline();
        Distribuzione dc = new Distribuzione();
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Interval = 2;
            timer1.Start();
            button3.Visible = true;
            button2.Visible = true;
            button1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer2.Interval = 10;
            timer2.Start();
            button2.Visible = false;
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            double height;
            ElementoDisribuzione studente = new ElementoDisribuzione("Student");
            height =r.NextDouble()+r.Next(140,200);
            studente.setVariable("height", height);
            mc.addElemento(studente);
            dc.addElementoDef(studente);

            richTextBox1.Text += "nuovo Studente altezza: " + height + Environment.NewLine;




        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            double i;
            SortedDictionary<Tuple<double, double>, int> distTest =new  SortedDictionary<Tuple<double, double>, int>();
            mc.getMedia("height", out i);
            if (dc.getdistribuzione("height", out distTest))
            {
                richTextBox3.Text = "";
                foreach (var item in distTest)
                {
                    richTextBox3.Text += item.Key.Item1 + "-" + item.Key.Item2 + " presenta " + item.Value + " entita "+Environment.NewLine;

                }
            }

            richTextBox2.Text = "the A M of the height is" + i + Environment.NewLine;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            timer2.Stop();
            button1.Visible = true;
            button2.Visible = true;
        }

      
    }
}
