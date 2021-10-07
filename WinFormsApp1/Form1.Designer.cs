using Contracts;
using System.Linq;

namespace WinFormsApp1
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.buttonNormalize = new System.Windows.Forms.Button();
            this.buttonSetTo0 = new System.Windows.Forms.Button();
            this.formsPlot1 = new ScottPlot.FormsPlot();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelAvg = new System.Windows.Forms.Label();
            this.buttonSwitch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.labelValueOfDollar = new System.Windows.Forms.Label();
            this.buttonReset = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonSaveWeights = new System.Windows.Forms.Button();
            this.buttonLoadWeights = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(1);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Location = new System.Drawing.Point(12, 400);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 82;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(927, 147);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            // 
            // buttonNormalize
            // 
            this.buttonNormalize.Location = new System.Drawing.Point(12, 374);
            this.buttonNormalize.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonNormalize.Name = "buttonNormalize";
            this.buttonNormalize.Size = new System.Drawing.Size(81, 22);
            this.buttonNormalize.TabIndex = 1;
            this.buttonNormalize.Text = "Normalize";
            this.buttonNormalize.UseVisualStyleBackColor = true;
            this.buttonNormalize.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonSetTo0
            // 
            this.buttonSetTo0.Location = new System.Drawing.Point(94, 374);
            this.buttonSetTo0.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonSetTo0.Name = "buttonSetTo0";
            this.buttonSetTo0.Size = new System.Drawing.Size(81, 22);
            this.buttonSetTo0.TabIndex = 2;
            this.buttonSetTo0.Text = "Set to 0";
            this.buttonSetTo0.UseVisualStyleBackColor = true;
            this.buttonSetTo0.Click += new System.EventHandler(this.button2_Click);
            // 
            // formsPlot1
            // 
            this.formsPlot1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.formsPlot1.BackColor = System.Drawing.Color.Transparent;
            this.formsPlot1.Location = new System.Drawing.Point(12, 12);
            this.formsPlot1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.formsPlot1.Name = "formsPlot1";
            this.formsPlot1.Size = new System.Drawing.Size(927, 345);
            this.formsPlot1.TabIndex = 3;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 15;
            this.listBox1.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.listBox1.Location = new System.Drawing.Point(180, 374);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(121, 19);
            this.listBox1.TabIndex = 4;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(307, 378);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Average annual Inflation";
            // 
            // labelAvg
            // 
            this.labelAvg.AutoSize = true;
            this.labelAvg.Location = new System.Drawing.Point(449, 378);
            this.labelAvg.Name = "labelAvg";
            this.labelAvg.Size = new System.Drawing.Size(13, 15);
            this.labelAvg.TabIndex = 6;
            this.labelAvg.Text = "0";
            // 
            // buttonSwitch
            // 
            this.buttonSwitch.Location = new System.Drawing.Point(12, 347);
            this.buttonSwitch.Name = "buttonSwitch";
            this.buttonSwitch.Size = new System.Drawing.Size(163, 23);
            this.buttonSwitch.TabIndex = 7;
            this.buttonSwitch.Text = "Switch to Mensual";
            this.buttonSwitch.UseVisualStyleBackColor = true;
            this.buttonSwitch.Click += new System.EventHandler(this.buttonSwitch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(546, 378);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 15);
            this.label2.TabIndex = 8;
            this.label2.Text = "Value of a Dollar";
            // 
            // labelValueOfDollar
            // 
            this.labelValueOfDollar.AutoSize = true;
            this.labelValueOfDollar.Location = new System.Drawing.Point(644, 378);
            this.labelValueOfDollar.Name = "labelValueOfDollar";
            this.labelValueOfDollar.Size = new System.Drawing.Size(13, 15);
            this.labelValueOfDollar.TabIndex = 9;
            this.labelValueOfDollar.Text = "0";
            // 
            // buttonReset
            // 
            this.buttonReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonReset.Location = new System.Drawing.Point(842, 378);
            this.buttonReset.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(98, 22);
            this.buttonReset.TabIndex = 12;
            this.buttonReset.Text = "ResetData";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(739, 378);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 22);
            this.button1.TabIndex = 13;
            this.button1.Text = "Switch Country";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // buttonSaveWeights
            // 
            this.buttonSaveWeights.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSaveWeights.Location = new System.Drawing.Point(842, 354);
            this.buttonSaveWeights.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonSaveWeights.Name = "buttonSaveWeights";
            this.buttonSaveWeights.Size = new System.Drawing.Size(43, 22);
            this.buttonSaveWeights.TabIndex = 14;
            this.buttonSaveWeights.Text = "Save";
            this.buttonSaveWeights.UseVisualStyleBackColor = true;
            this.buttonSaveWeights.Click += new System.EventHandler(this.buttonSaveWeights_Click);
            // 
            // buttonLoadWeights
            // 
            this.buttonLoadWeights.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoadWeights.Location = new System.Drawing.Point(896, 354);
            this.buttonLoadWeights.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.buttonLoadWeights.Name = "buttonLoadWeights";
            this.buttonLoadWeights.Size = new System.Drawing.Size(43, 22);
            this.buttonLoadWeights.TabIndex = 15;
            this.buttonLoadWeights.Text = "Load";
            this.buttonLoadWeights.UseVisualStyleBackColor = true;
            this.buttonLoadWeights.Click += new System.EventHandler(this.buttonLoadWeight_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(740, 358);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(101, 15);
            this.label3.TabIndex = 16;
            this.label3.Text = "Custom Weights :";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(951, 554);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonLoadWeights);
            this.Controls.Add(this.buttonSaveWeights);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.labelValueOfDollar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonSwitch);
            this.Controls.Add(this.labelAvg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.formsPlot1);
            this.Controls.Add(this.buttonSetTo0);
            this.Controls.Add(this.buttonNormalize);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Personalized Inflation";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button buttonNormalize;
        private System.Windows.Forms.Button buttonSetTo0;
        private ScottPlot.FormsPlot formsPlot1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelAvg;
        private System.Windows.Forms.Button buttonSwitch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelValueOfDollar;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonSaveWeights;
        private System.Windows.Forms.Button buttonLoadWeights;
        private System.Windows.Forms.Label label3;
    }


}

