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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        Dictionary<int, ElementoDisribuzione> csvContent = new Dictionary<int, ElementoDisribuzione>();
        Distribuzione distr = new Distribuzione();
        MediaCalOnline medie = new MediaCalOnline();
        List<String> attributename = new List<string>();
        List<String> bivariante = new List<string>();//per ora poi diventera n variante;
        private void button1_Click(object sender, EventArgs e)


        {

            var filePath = string.Empty;
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                filePath = openFileDialog1.FileName;

                //Read the contents of the file into a stream
                using (TextFieldParser csvParser = new TextFieldParser(filePath))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Save the row with the column names
                    string[] fieldsNames = csvParser.ReadFields();
                    attributename.AddRange(fieldsNames);
                    int i = 0;
                    while (!csvParser.EndOfData)
                    {
                        // Read current line fields, pointer moves to the next line.

                        string[] fields = csvParser.ReadFields();
                        ElementoDisribuzione elem = new ElementoDisribuzione(fields[0]);
                        int j = 0;
                        foreach (String field in fields)
                        {
                            if (!String.IsNullOrEmpty(field))
                            {
                                double tmp;
                                if (Double.TryParse(field, out tmp)) { elem.setVariable(fieldsNames[j], new Tuple<Object, Type>(tmp, tmp.GetType())); medie.addAttribute(fieldsNames[j], tmp); }
                                else elem.setVariable(fieldsNames[j], new Tuple<Object, Type>(field, field.GetType()));
                            }
                            j++;
                        }
                        csvContent.Add(i, elem);
                        distr.addElementoDef(elem);
                        i++;
                    }
                }
                button2.Visible = true;
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ;

            foreach (var elem in attributename)
                contextMenuStrip1.Items.Add(elem);
            contextMenuStrip1.Visible = true;

        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            richTextBox1.Text += "-------------------------------------------------------------" + Environment.NewLine;
            richTextBox1.Text += e.ClickedItem.Text + Environment.NewLine;

            double media = 0;
            double stdVariation;
            List<Double> values=new List<double>(0);
            SortedDictionary<Tuple<double, double>, int> firstdistr;
            medie.getMedia(e.ClickedItem.Text, out media);
            //se la media è diversa da zero sono sicuro che è double
            if (media != 0)
            {
                Tuple<Object, Type> tmp;
                foreach (var elm in csvContent.Values)
                {
                    elm.getVariable(e.ClickedItem.Text, out tmp);

                    values.Add((double)tmp.Item1);

                }
                medie.getStandardDeviation(e.ClickedItem.Text, values,out stdVariation);
                distr.getdistribuzioneN(e.ClickedItem.Text, out firstdistr);
                foreach (var el in firstdistr)
                {
                    richTextBox1.Text += el.Key.ToString() + " : " + el.Value + Environment.NewLine;
                }
            }
            else stdVariation = 0;


            richTextBox1.Text += e.ClickedItem + " with average: " + media + " and standard variation: " + stdVariation+ Environment.NewLine;
            bivariante.Add(e.ClickedItem.Text);
          
            button2.Visible = false;
            foreach (var elem in attributename)
                contextMenuStrip2.Items.Add(elem);
            contextMenuStrip2.Visible = true;
        }

        private void contextMenuStrip2_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            richTextBox1.Text += "-------------------------------------------------------------" + Environment.NewLine;
            richTextBox1.Text += e.ClickedItem.Text + Environment.NewLine;
            double media2 = 0;
            double stdVariation;
            List<Double> values = new List<double>(0);
            SortedDictionary<Tuple<double,double>,int> seconddistr;
            bool isDouble = false;
            medie.getMedia(e.ClickedItem.Text, out media2);
            if (media2 != 0)
            {
                isDouble = true;
                Tuple<Object, Type> tmp;
                foreach (var elm in csvContent.Values)
                {
                    elm.getVariable(e.ClickedItem.Text, out tmp);

                    values.Add((double)tmp.Item1);

                }
                medie.getStandardDeviation(e.ClickedItem.Text, values, out stdVariation);
                distr.getdistribuzioneN(e.ClickedItem.Text, out seconddistr);
                foreach (var el in seconddistr)
                {
                    richTextBox1.Text += el.Key.ToString() +" : " + el.Value +Environment.NewLine;
                }
            }
            else stdVariation = 0;
            richTextBox1.Text += e.ClickedItem + " with average: " + media2 + " and standard variation: " + stdVariation +  Environment.NewLine;
            bivariante.Add(e.ClickedItem.Text);
           
            richTextBox1.Text += Environment.NewLine;
            int nr, nc;
            String[,] bivariantMatrix = distr.getbivariantmatrix(bivariante, csvContent.Values, out nr, out nc);

            for (int i = 0; i <= nr; i++)
            {
                richTextBox1.Text += Environment.NewLine;
                for (int j = 0; j <= nc; j++)
                {
                    richTextBox1.Text += bivariantMatrix[i, j].PadLeft(6);
                    
                }
            }

        }



    }
    }
