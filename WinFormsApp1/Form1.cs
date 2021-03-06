using System;
using System.Linq;
using System.Windows.Forms;
using Contracts;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private TreeNode<Category> rootLevel;
        private int Depth=1;
        private bool isAnnual = true;

        private bool isEU = false;

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
            rootLevel = SaveLoad.LoadOrDownload(isEU);
        }

        private void SaveData()
        {
            SaveLoad.SaveBackfilledData(rootLevel.Flatten().ToList(),isEU);
        }

        private void DeleteSavedData()
        {
            SaveLoad.DeleteData(isEU);
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
                var annualInflation = inflationNumbers.AnnualObservations.Observations;
                x = annualInflation.Select(x => new DateTime(year: int.Parse(x.Value._year), month: 1, 1).ToOADate()).ToArray();

                y = annualInflation.Select(x => (double)x.Value._percentChange).ToArray();
            }
            else
            {
                var mensualInflation = inflationNumbers.MensualObservations.Observations;
                x = mensualInflation.Select(x => new DateTime(year: int.Parse( x.Value._year), month: int.Parse(x.Value._month.Replace("M","")), 1).ToOADate()).ToArray();
                y = mensualInflation.Select(x => (double)x.Value._percentChange).ToArray();
            }

            this.formsPlot1.Plot.XAxis.DateTimeFormat(true);
            this.formsPlot1.Plot.Clear();
            this.formsPlot1.Plot.AddScatter(x, y);
           // this.labelAvg.Text = "" + (isAnnual ? 1 : 12 )* y.Average();

           // this.labelValueOfDollar.Text = "" +  1 / Math.Pow(1 + y.Average() / 100, y.Length);
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
            label2.Text = "Value of a " + (isEU ? "Euro" : "Dollar");
        }

        private void buttonSwitch_Click(object sender, EventArgs e)
        {
            if (isAnnual)
            {
                isAnnual = false;
                this.buttonSwitch.Text = "Switch to Annual";
                
            }
            else
            {
                isAnnual = true;
                this.buttonSwitch.Text = "Switch to Mensual";
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            rootLevel.PrintNodesWeightRecursive(rootLevel);
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            isEU = !isEU;
            LoadData();
            Refresh();
        }

        private void buttonSaveWeights_Click(object sender, EventArgs e)
        {
            SaveLoad.SaveWeights(rootLevel, isEU);
        }

        private void buttonLoadWeight_Click_1(object sender, EventArgs e)
        {
            SaveLoad.OverrideWeights(rootLevel, isEU);
            Refresh();
        }
    }
}
