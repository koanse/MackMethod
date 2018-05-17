using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MackMethod
{
    public partial class MainForm : Form
    {
        int n = 3;
        string protocol;
        public MainForm()
        {
            InitializeComponent();
            toolStripTextBox1_TextChanged(this, null);
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void toolStripTextBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                n = int.Parse(toolStripTextBox1.Text);
                dataGridView1.Rows.Clear();
                dataGridView1.Columns.Clear();
                for (int j = 0; j < n; j++)
                {
                    DataGridViewColumn c = new DataGridViewColumn();
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.ValueType = typeof(float);
                    c.CellTemplate = cell;
                    dataGridView1.Columns.Add(c);
                }
                for (int i = 0; i < n; i++)
                    dataGridView1.Rows.Add();
            }
            catch
            {
                n = 3;
                toolStripTextBox1.Text = "3";
                dataGridView1.Columns.Clear();
                for (int j = 0; j < n; j++)
                    dataGridView1.Columns.Add(j.ToString(), j.ToString());
                for (int i = 0; i < n; i++)
                    dataGridView1.Rows.Add();
            }
        }
        private void calculateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }        
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                float[,] x = new float[n, n];
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        x[i, j] = (float)dataGridView1.Rows[i].Cells[j].Value;
                MackMethod m = new MackMethod(x);
                protocol = "";
                int iterNum = 1;
                while (m.DoIteration())
                {
                    protocol += "Итерация номер " +
                        iterNum.ToString() + "\r\n";
                    protocol += m.iterationResult + "\r\n\r\n";
                    iterNum++;
                }
                protocol += "Итерация номер " +
                        iterNum.ToString() + "\r\n";
                protocol += m.iterationResult + "\r\n\r\n";
                protocol += m.GetResult();
            }
            catch
            {
                MessageBox.Show("Вычисления прерваны", "Прерывание",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }        
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

        }
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ProtocolForm pf = new ProtocolForm(protocol);
            pf.ShowDialog();
        }
        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Неверный формат данных. При вводе дробей используйте запятую",
                "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox ab = new AboutBox();
            ab.ShowDialog();
        }
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HelpForm hf = new HelpForm();
            hf.ShowDialog();
        }
    }
}
