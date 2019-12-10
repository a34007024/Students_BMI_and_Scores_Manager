using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;//導入存取SQLite的函式庫

namespace Students_BMI_and_Scores_Manager
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            idLabel.Text = publicVariables.index.ToString();
            change_dataGridView1_font();
            load_DB();//程式初始化, 載入dataBase
        }

        public class publicVariables
        {   //用來暫存資料的全域變數
            //把全域變數包在一個class可以避免該變數變成無人管的變數
            //在未來專案要多人合作時，可避免某一個變數出錯
            //找不到人來負責處理的窘境
            //宣告成static則可以不用new一個Object即可調用該變數
            public static int serial = 0;
            public static int index = 1;
            public static double height;
            public static double weight;
            public static string name;
            public static double bmi;
            public static int chineseScore;
            public static int englishScore;
            public static int mathScore;
        }

        private void change_dataGridView1_font()
        {
            this.dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("微軟正黑體", 10);
            this.dataGridView1.DefaultCellStyle.Font = new Font("微軟正黑體", 12);

            //for test
            //DataGridViewRowCollection rows = dataGridView1.Rows;
            //rows.Add(new Object[] { 1, "Tom", 101, 175.0, 66.0 });
            //rows.Add(new Object[] { 2, "John", 102, 165.0, 56.0 });
            //rows.Add(new Object[] { 3, "Mary", 103, 155.0, 46.0 });
        }

        public class DBConfig//用來設定SQLite的屬性 (如檔案路徑)
        {
            public static string dbFile = Application.StartupPath + @"\owo.db";
            public static string dbPath = "Data source=" + dbFile;

            public static SQLiteConnection sqlite_connect;
            public static SQLiteCommand sqlite_cmd;
            public static SQLiteDataReader sqlite_dataReader;
        }

        private void load_DB()//載入DataBase
        {
            DBConfig.sqlite_connect = new SQLiteConnection(DBConfig.dbPath);
            DBConfig.sqlite_connect.Open();//Open database

            show_DB();
        }

        private void show_DB()
        {
            try
            {
                this.dataGridView1.Rows.Clear();

                string sql = @"SELECT * from record;";
                DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
                DBConfig.sqlite_dataReader = DBConfig.sqlite_cmd.ExecuteReader();

                while (DBConfig.sqlite_dataReader.Read()) //read every data
                {
                    int serial = Convert.ToInt32(DBConfig.sqlite_dataReader["serial"]);
                    string name = Convert.ToString(DBConfig.sqlite_dataReader["name"]);
                    int id = Convert.ToInt32(DBConfig.sqlite_dataReader["id"]);
                    double height = Convert.ToDouble(DBConfig.sqlite_dataReader["height"]);
                    double weight = Convert.ToDouble(DBConfig.sqlite_dataReader["weight"]);
                    double bmi = Convert.ToDouble(DBConfig.sqlite_dataReader["BMI"]);
                    int chineseScore = Convert.ToInt32(DBConfig.sqlite_dataReader["chineseScore"]);
                    int englishScore = Convert.ToInt32(DBConfig.sqlite_dataReader["englishScore"]);
                    int mathScore = Convert.ToInt32(DBConfig.sqlite_dataReader["mathScore"]);

                    publicVariables.serial = serial;
                    DataGridViewRowCollection rows = dataGridView1.Rows;
                    rows.Add(new Object[] { id, name, height, weight, bmi, chineseScore, englishScore, mathScore });
                }
                DBConfig.sqlite_dataReader.Close();
            }
            catch
            {
                MessageBox.Show("無法存取資料庫!\n資料庫檔案可能毀損、遺失\r或被其他應用程式存取之中",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void addDataBtn_Click(object sender, EventArgs e)
        {
            try
            {
                publicVariables.height = double.Parse(heightTextbox.Text);
                publicVariables.weight = double.Parse(weightTextbox.Text);
                publicVariables.name = nameTextbox.Text;

                if (publicVariables.height >= 3 || publicVariables.weight <= 10)
                {
                    DialogResult d = MessageBox.Show("請您檢查輸入的資料是否正確!\n" +
                        "您輸入的身高可能高於樓層高度\n您輸入的體重可能輕於中型犬的體重" +
                        "\n\n若您確定資料輸入無誤請按Yes儲存\n否則請按No返回編輯", "注意!"
                        , MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (d == DialogResult.Yes)
                    {
                        publicVariables.bmi = publicVariables.weight / (publicVariables.height * publicVariables.height);
                        dataGridView1.Rows.Add(new object[] { publicVariables.index, publicVariables.name, publicVariables.height, publicVariables.weight, publicVariables.bmi });
                        publicVariables.index += 1;
                        idLabel.Text = publicVariables.index.ToString();
                        updateChart();//新增資料後，更新圖表
                    }
                    else
                    {
                        //donothing
                    }
                }
                else
                {
                    publicVariables.bmi = publicVariables.weight / (publicVariables.height * publicVariables.height);
                    dataGridView1.Rows.Add(new object[] { publicVariables.index, publicVariables.name, publicVariables.height, publicVariables.weight, publicVariables.bmi });
                    publicVariables.index += 1;
                    idLabel.Text = publicVariables.index.ToString();
                    updateChart();//新增資料後，更新圖表
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
            chineseScoreTextbox.Text = Convert.ToString(selRowData[5].Value);
            englishScoreTextbox.Text = Convert.ToString(selRowData[6].Value);
            mathScoreTextbox.Text = Convert.ToString(selRowData[7].Value);
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
