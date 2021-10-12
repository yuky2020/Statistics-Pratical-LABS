using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace WordCounter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
           
        }

       SortedDictionary<String, int> wDstr = new SortedDictionary<string, int>();
      
        
        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var filePath = string.Empty;
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.Title = "Select Text File";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog1.FileName;

                //Read the contents of the file into a stream
                using (StreamReader sr = new StreamReader(filePath))
                {
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine();
                        char[] delimiters = new char[] { ' ', '\r', '\n' };
                        string[] words = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                        int i = 0;
                        foreach (string word in words)
                        {
                            if (!wDstr.TryGetValue(word, out i)) wDstr.Add(word, 1);
                            else { wDstr.Remove(word); i++; wDstr.Add(word, i); }
                            
                        }
                    }
                }
                int j = 0;
                foreach(var w in wDstr)
                {
                    chart1.Series["Words"].Points.AddXY(w.Key, w.Value);

                }


            }

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }
    }        
    }

