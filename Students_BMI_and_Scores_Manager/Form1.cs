using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Students_BMI_and_Scores_Manager
{
    public partial class Form1 : Form
    {
        int index = 1;
        double height;
        double weight;
        string name;
        double bmi;
        public Form1()
        {
            InitializeComponent();
            idLabel.Text = index.ToString();
        }
        
        private void addDataBtn_Click(object sender, EventArgs e)
        {
            try
            {
                height = double.Parse(heightTextbox.Text);
                weight = double.Parse(weightTextbox.Text);
                name = nameTextbox.Text;
                if (height >= 3 || weight <= 10)
                {
                    DialogResult d = MessageBox.Show("請您檢查輸入的資料是否正確!\n" +
                        "您輸入的身高可能高於樓層高度\n您輸入的體重可能輕於中型犬的體重" +
                        "\n\n若您確定資料輸入無誤請按Yes儲存\n否則請按No返回編輯", "注意!"
                        , MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (d == DialogResult.Yes)
                    {
                        bmi = weight / (height * height);
                        dataGridView1.Rows.Add(new object[] { index, name, height, weight, bmi });
                        index += 1;
                        idLabel.Text = index.ToString();
                    }
                    else
                    {
                        //donothing
                    }
                }
                else
                {
                    bmi = weight / (height * height);
                    dataGridView1.Rows.Add(new object[] { index, name, height, weight, bmi });
                    index += 1;
                    idLabel.Text = index.ToString();
                }
            }
            catch
            {
                MessageBox.Show("輸入有誤!請檢查!", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void heightTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void weightTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void double_click(object sender, DataGridViewCellMouseEventArgs e)
            //雙擊資料表的三角標籤可修改該列資料
        {
            DataGridViewCellCollection selRowData = dataGridView1.SelectedRows[0].Cells;

            nameTextbox.Text = Convert.ToString(selRowData[1].Value);
            heightTextbox.Text = Convert.ToString(selRowData[2].Value);
            weightTextbox.Text = Convert.ToString(selRowData[3].Value);
            idLabel.Text = Convert.ToString(selRowData[0].Value);
        }

        //下面的函式是用來畫統計圖表的
        public void setBar(Dictionary<string, int> i_weight_bar)
        {
            this.chart1.Series["weight"].Points.Clear();
            foreach (var OneItem in i_weight_bar)
            {
                this.chart1.Series["weight"].Points.AddXY(OneItem.Key, OneItem.Value);
            }
        }
        //下面的函式，把表格上的體重分群，分成~40,40~50,50~60,60~70,70~80,80~
        public void updateChart()
        {
            Dictionary<string, int> _weight_bar = new Dictionary<string, int>();

            _weight_bar.Add("~40", 0);
            _weight_bar.Add("40~50", 0);
            _weight_bar.Add("50~60", 0);
            _weight_bar.Add("60~70", 0);
            _weight_bar.Add("70~80", 0);
            _weight_bar.Add("80~", 0);

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                if (Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) < 40)
                {
                    _weight_bar["~40"] = _weight_bar["~40"] + 1;
                }
                else if (Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) >= 40 && Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) < 50)
                {
                    _weight_bar["40~50"] = _weight_bar["40~50"] + 1;
                }
                else if (Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) >= 50 && Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) < 60)
                {
                    _weight_bar["50~60"] = _weight_bar["50~60"] + 1;
                }
                else if (Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) >= 60 && Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) < 70)
                {
                    _weight_bar["60~70"] = _weight_bar["60~70"] + 1;
                }
                else if (Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) >= 70 && Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value) < 80)
                {
                    _weight_bar["70~80"] = _weight_bar["70~80"] + 1;
                }
                else
                {
                    _weight_bar["80~"] = _weight_bar["80~"] + 1;
                }
            }
            setBar(_weight_bar);

        }

        private void editDataBtn_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
                {
                    if (dataGridView1.Rows[i].Cells[0].Value.ToString() == this.idLabel.Text)
                    {
                        dataGridView1.Rows[i].Cells[1].Value = nameTextbox.Text;
                        dataGridView1.Rows[i].Cells[2].Value = heightTextbox.Text;
                        dataGridView1.Rows[i].Cells[3].Value = weightTextbox.Text;
                        // 計算BMI
                        dataGridView1.Rows[i].Cells[4].Value = double.Parse(weightTextbox.Text) / (double.Parse(heightTextbox.Text) * double.Parse(heightTextbox.Text));
                        break;
                    }

                }
                //updateChart();
            }
            catch
            {
                MessageBox.Show("輸入有誤!請檢查!", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
