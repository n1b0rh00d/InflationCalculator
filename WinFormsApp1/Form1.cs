using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Contracts;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private TreeNode<Category> rootLevel;
        private int Depth=1;
        private bool isAnnual = true;

        public Form1()
        {
            InitializeComponent();

            this.dataGridView1.AutoGenerateColumns = true;

            LoadData();

            this.dataGridView1.DataSource = rootLevel.ListAll().Where(x => x.Value._depth <= Depth).ToList();

            this.dataGridView1.Columns[2].Visible = false;
            this.dataGridView1.Columns[3].Visible = false;

            this.dataGridView1.Columns[1].ReadOnly = false;
            refreshGraph(true);
        }

        private void LoadData()
        {
            rootLevel = SaveLoad.LoadOrDownload();
        }

        private void SaveData()
        {
            SaveLoad.SaveBackfilledData(rootLevel.Flatten().ToList());
        }

        private void DeleteSavedData()
        {
            SaveLoad.DeleteData();
        }


        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Refresh();
            refreshGraph(isAnnual);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rootLevel.UpdateWeightUpAndDown(100);
            dataGridView1.Refresh();
            refreshGraph(isAnnual);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rootLevel.UpdateWeightUpAndDown(0);
            dataGridView1.Refresh();

        }

        private void refreshGraph(bool isAnnual)
        {
            SerieObservations inflationNumbers = rootLevel.CalculateInflationRecursivelyFromProvidedLevels();
            double[] x;
            double[] y;
            
            if (isAnnual)
            {
                var annualInflation = inflationNumbers.AnnualObservations;
                x = annualInflation.Select(x => new DateTime(year: int.Parse(x.Value._year), month: 1, 1).ToOADate()).ToArray();

                y = annualInflation.Select(x => (double)x.Value._percentChange).ToArray();
            }
            else
            {
                var mensualInflation = inflationNumbers.MensualObservations;
                x = mensualInflation.Select(x => new DateTime(year: int.Parse( x.Value._year), month: int.Parse(x.Value._month.Replace("M","")), 1).ToOADate()).ToArray();
                y = mensualInflation.Select(x => (double)x.Value._percentChange).ToArray();
            }

            this.formsPlot1.Plot.XAxis.DateTimeFormat(true);
            this.formsPlot1.Plot.Clear();
            this.formsPlot1.Plot.AddScatter(x, y);
            this.labelAvg.Text = "" + (isAnnual ? 1 : 12 )* y.Average();

            this.labelValueOfDollar.Text = "" +  1 / Math.Pow(1 + y.Average() / 100, y.Length);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Depth = int.Parse(listBox1.SelectedItem.ToString());
            Refresh();
        }

        private void Refresh()
        {
            var entries = rootLevel.ListAll(Depth);
            this.dataGridView1.Columns.Clear();
            this.dataGridView1.AutoGenerateColumns = true;

            this.dataGridView1.DataSource = entries;

            this.dataGridView1.Columns[2].Visible = false;
            this.dataGridView1.Columns[3].Visible = false;
            this.dataGridView1.Columns[1].ReadOnly = false;
            this.dataGridView1.Columns[4].SortMode = DataGridViewColumnSortMode.Automatic;

            dataGridView1.Refresh();
            refreshGraph(isAnnual);
        }

        private void buttonSwitch_Click(object sender, EventArgs e)
        {
            if (isAnnual)
            {
                isAnnual = false;
                this.label1.Text = "Switch to Annual";
                
            }
            else
            {
                isAnnual = true;
                this.label1.Text = "Switch to Mensual";
            }

            refreshGraph(isAnnual);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            LoadData();
            Refresh();
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            DeleteSavedData();
            LoadData();
            Refresh();
        }
    }
}
