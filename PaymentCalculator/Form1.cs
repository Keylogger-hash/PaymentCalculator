using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;


namespace PaymentCalculator
{
    public partial class Form1 : Form
    {
        DataTable dtShedule = new DataTable();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            double euro = 90.30;
            double dollar = 76.26;
            double creditRate1 = 0.0;
            double creditSum1 = Convert.ToDouble(textBox1.Text);
            int creditPeriod1 = Convert.ToInt32(textBox3.Text);
            bool creditTypePeriod = radioButton1.Checked ? true : false;
            bool creditType = radioButton3.Checked ? true : false;
            bool moneyTypeEuro = radioButton5.Checked ? true : false;
            bool moneyTypeDollars = radioButton6.Checked ? true : false;
            bool moneyTypeRussian = radioButton7.Checked ? true : false;
            if (moneyTypeEuro == true)
            {
                creditSum1 = creditSum1 / euro;
                creditRate1 = 0.04;
            }
            if (moneyTypeDollars == true)
            {
                creditSum1 = creditSum1 / dollar;
                creditRate1 = 0.04;
            }
            if (moneyTypeRussian == true)
            {
                creditSum1 = creditSum1 / 1.0;
                creditRate1 = 0.125;
            }
            if (creditTypePeriod == false)
            {
                creditPeriod1 = creditPeriod1 * 1;
            }
            else if (creditTypePeriod == true)
            {
                creditPeriod1 = creditPeriod1 * 12;
            }
            Calculator calc = new Calculator(creditSum1,creditRate1,creditPeriod1,creditType);
            dtShedule = calc.GetShedule();
            dataGridView1.DataSource = dtShedule;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
            for (int i = 1; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].DefaultCellStyle.Format = String.Format("### ### ### ##0.#0");
                dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                dataGridView1.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
           
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "CSV (*.csv)|*.csv";
                sfd.FileName = "Output.csv";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("It wasn't possible to write the data to the disk." + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            int columnCount = dataGridView1.Columns.Count;
                            string columnNames = "";
                            string[] outputCsv = new string[dataGridView1.Rows.Count + 1];
                            for (int i = 0; i < columnCount; i++)
                            {
                                columnNames += dataGridView1.Columns[i].HeaderText.ToString() + ",";
                            }
                            outputCsv[0] += columnNames;

                            for (int i = 1; (i - 1) < dataGridView1.Rows.Count; i++)
                            {
                                for (int j = 0; j < columnCount; j++)
                                {
                                    outputCsv[i] += dataGridView1.Rows[i - 1].Cells[j].Value+ ",";
                                }
                            }

                            File.WriteAllLines(sfd.FileName, outputCsv, Encoding.UTF8);
                            MessageBox.Show("При импортировании игнорируйте 3 и 5 колонку\nОстаток по долгу отображается в 4 колонке");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No Record To Export !!!", "Info");
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && !Char.IsControl(e.KeyChar) && (e.KeyChar != ','))
            {
                e.Handled = true;
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && !Char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char ch = e.KeyChar;
            if (!Char.IsDigit(ch) && !Char.IsControl(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
    }
}
