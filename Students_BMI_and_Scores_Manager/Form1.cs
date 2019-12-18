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
using Excel = Microsoft.Office.Interop.Excel;//Import Excel 的關聯函式庫

namespace Students_BMI_and_Scores_Manager
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            change_dataGridView1_font();

            load_DB();//程式初始化, 載入dataBase
            updateChart();//初始化載入資料後更新圖表
            this.TopMost = true;
        }

        public class publicVariables
        {   //用來暫存資料的全域變數
            //把全域變數包在一個class可以避免該變數變成無人管的變數
            //在未來專案要多人合作時，可避免某一個變數出錯
            //找不到人來負責處理的窘境
            //宣告成static則可以不用new一個Object即可調用該變數
            public static int serial = 0;
            public static int id = 1;
            public static double height;
            public static double weight;
            public static string name;
            public static double bmi;
            public static int chineseScore;
            public static int englishScore;
            public static int mathScore;
            public static bool successfulLoadDatabase = false;
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
            if (publicVariables.successfulLoadDatabase == true)
                MessageBox.Show("資料庫連結成功!", "INFO", MessageBoxButtons.OK, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);//設定此彈出視窗為最上層
        }

        private void show_DB()//把資料庫的資料顯示在dataGridView上(更新idLabel的功能也在此)
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

                    publicVariables.id = id;//把讀取到的id存在全域變數中(最後一筆的資料便會停駐於此)
                    publicVariables.serial = serial;
                    DataGridViewRowCollection rows = dataGridView1.Rows;
                    rows.Add(new Object[] { id, name, height, weight, bmi, chineseScore, englishScore, mathScore, serial });
                }
                DBConfig.sqlite_dataReader.Close();
                //資料庫資料讀取完畢
                publicVariables.successfulLoadDatabase = true;
                publicVariables.id += 1;//最後一筆讀入的id再加1即為下一筆要輸入進去的資料id
                idLabel.Text = publicVariables.id.ToString();//改變idLabel的數值
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
                publicVariables.chineseScore = int.Parse(chineseScoreTextbox.Text);
                publicVariables.englishScore = int.Parse(englishScoreTextbox.Text);
                publicVariables.mathScore = int.Parse(mathScoreTextbox.Text);
                publicVariables.name = nameTextbox.Text;

                if (publicVariables.height >= 3 || publicVariables.weight <= 10)
                {
                    DialogResult d = MessageBox.Show("請您檢查輸入的資料是否正確!\n" +
                        "您輸入的身高可能高於樓層高度\n您輸入的體重可能輕於中型犬的體重" +
                        "\n\n若您確定資料輸入無誤請按Yes儲存\n否則請按No返回編輯", "注意!"
                        , MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (d == DialogResult.Yes)
                    {
                        publicVariables.bmi = publicVariables.weight / (publicVariables.height * publicVariables.height);//計算BMI

                        string sql = @"INSERT INTO record(id,name,height,weight,BMI,chineseScore,englishScore,mathScore) VALUES(" + publicVariables.id.ToString() + ",'" + publicVariables.name + "'," + publicVariables.height.ToString() + "," + publicVariables.weight.ToString() + "," + publicVariables.bmi.ToString() + "," + publicVariables.chineseScore.ToString() + "," + publicVariables.englishScore.ToString() + "," + publicVariables.mathScore.ToString() + ");";
                        //宣告一個字串存放要執行的SQL指令
                        DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
                        DBConfig.sqlite_cmd.ExecuteNonQuery();//執行SQL指令(寫入)

                        show_DB();//把資料庫讀取出來並顯示於dataGridView上

                        updateChart();//新增資料後，更新圖表
                        //因為show_DB()內已經有寫自動編號功能，因此此處不必再處理
                        idLabel.Text = publicVariables.id.ToString();
                    }
                    else
                    {
                        //donothing
                    }
                }
                else
                {
                    publicVariables.bmi = publicVariables.weight / (publicVariables.height * publicVariables.height);//計算BMI

                    string sql = @"INSERT INTO record(id,name,height,weight,BMI,chineseScore,englishScore,mathScore) VALUES(" + publicVariables.id.ToString() + ",'" + publicVariables.name + "'," + publicVariables.height.ToString() + "," + publicVariables.weight.ToString() + "," + publicVariables.bmi.ToString() + "," + publicVariables.chineseScore.ToString() + "," + publicVariables.englishScore.ToString() + "," + publicVariables.mathScore.ToString() + ");";
                    //宣告一個字串存放要執行的SQL指令
                    DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
                    DBConfig.sqlite_cmd.ExecuteNonQuery();//執行SQL指令(寫入)

                    show_DB();//把資料庫讀取出來並顯示於dataGridView上

                    updateChart();//新增資料後，更新圖表
                    //因為show_DB()內已經有寫自動編號功能，因此此處不必再處理
                    idLabel.Text = publicVariables.id.ToString();
                }
            }
            catch
            {
                MessageBox.Show("輸入有誤!請檢查!\n\n成績只能輸入整數喔!", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            publicVariables.serial = Convert.ToInt32(selRowData[8].Value);//儲存選定列的serial
            //因為serial於資料庫中是設定為自動增加的，相較於idLabel的值，不用擔心有重複的問題
            //所以用serial來鎖定選定的資料欄位比較合適
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
                publicVariables.id = int.Parse(idLabel.Text);
                publicVariables.height = double.Parse(heightTextbox.Text);
                publicVariables.weight = double.Parse(weightTextbox.Text);
                publicVariables.chineseScore = int.Parse(chineseScoreTextbox.Text);
                publicVariables.englishScore = int.Parse(englishScoreTextbox.Text);
                publicVariables.mathScore = int.Parse(mathScoreTextbox.Text);
                publicVariables.name = nameTextbox.Text;

                publicVariables.bmi = publicVariables.weight / (publicVariables.height * publicVariables.height);//計算BMI

                string sql = @"UPDATE record SET name='" + publicVariables.name + "',height=" + publicVariables.height.ToString() +
                    ",weight=" + publicVariables.weight.ToString() +
                    ",BMI=" + publicVariables.bmi.ToString() +
                    ",chineseScore=" + publicVariables.chineseScore.ToString() +
                    ",englishScore=" + publicVariables.englishScore.ToString() +
                    ",mathScore=" + publicVariables.mathScore.ToString() +
                    " where serial = " + publicVariables.serial.ToString() + ";";//利用資料庫內的serial來決定要更改的資料列
                                                                                 //因為serial於資料庫中是設定為自動增加的，相較於idLabel的值，不用擔心有重複的問題
                                                                                 //所以用serial來鎖定選定的資料欄位比較合適
                                                                                 //宣告一個字串存放要執行的SQL指令
                DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
                DBConfig.sqlite_cmd.ExecuteNonQuery();//執行SQL指令(寫入)

                show_DB();//把資料庫讀取出來並顯示於dataGridView上

            }
            catch
            {
                MessageBox.Show("輸入有誤!請檢查!\n\n成績只能輸入整數喔!", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void delDataBtn_Click(object sender, EventArgs e)
        {
            string sql = @"DELETE from record " +
                                      "  where serial = " + publicVariables.serial.ToString() + ";";
            //因為serial於資料庫中是設定為自動增加的，相較於idLabel的值，不用擔心有重複的問題
            //所以用serial來鎖定選定的資料欄位比較合適

            DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
            DBConfig.sqlite_cmd.ExecuteNonQuery();
            show_DB();
        }

        /* 未寫進class內的全域變數都放在這裡喔~
         * 寫在此區的全域變數是為了與其他form之間進行資料的傳遞
         */
        public int selectedFunction = 0;
        public int nameStartID = 0;
        public int dataCount = 0;
        public int minHeight = 0;
        public int maxHeight = 0;
        public int minWeight = 0;
        public int maxWeight = 0;
        public int minCHscore = 0;
        public int maxCHscore = 0;
        public int minENscore = 0;
        public int maxENscore = 0;
        public int minMATHscore = 0;
        public int maxMATHscore = 0;
        public int randomBigDataPageReturnStatus = 0;
        /* 未寫進class內的全域變數都放在這裡喔~
         * 寫在此區的全域變數是為了與其他form之間進行資料的傳遞
         */

        private void exportBtn_Click(object sender, EventArgs e)
        {
            selectedFunction = 0;
            Form2 f2 = new Form2();
            f2.Owner = this;//重要的一步，使Form2的Owner指針指向Form1
            f2.ShowDialog();
            //MessageBox.Show(selectedFunction.ToString());//debug用，顯示form2回傳的值
            if(selectedFunction == 1)//匯出資料(excel)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);//設定初始存檔資料夾位置
                save.FileName = "Export_xlsx_Data";
                save.Filter = "*.xlsx|*.xlsx";//指定輸出格式
                if (save.ShowDialog() != DialogResult.OK) return;//如果選了取消，就不往後執行

                // Excel 物件
                Excel.Application xls = null;
                try
                {
                    xls = new Excel.Application();
                    // Excel WorkBook
                    Excel.Workbook book = xls.Workbooks.Add();
                    //Excel.Worksheet Sheet = (Excel.Worksheet)book.Worksheets[1];
                    Excel.Worksheet Sheet = xls.ActiveSheet;

                    // 把 DataGridView 資料塞進 Excel 內

                    // 把DataGridView 標題輸入進excel表格
                    for (int k = 0; k < this.dataGridView1.Columns.Count; k++)
                    {
                        Sheet.Cells[1, k + 1] = this.dataGridView1.Columns[k].HeaderText.ToString();
                    }

                    // 把DataGridView 內容輸入進excel表格
                    for (int i = 0; i < this.dataGridView1.Rows.Count - 1; i++)
                    {
                        for (int j = 0; j < this.dataGridView1.Columns.Count; j++)
                        {
                            string value = dataGridView1.Rows[i].Cells[j].Value.ToString();
                            Sheet.Cells[i + 2, j + 1] = value;
                        }
                    }

                    // 儲存檔案
                    book.SaveAs(save.FileName);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    xls.Quit();
                }
            }
            else if(selectedFunction == 2)//匯出圖表(excel)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                save.FileName = "Export_Chart_Data";
                save.Filter = "*.xlsx|*.xlsx";
                if (save.ShowDialog() != DialogResult.OK) return;

                // Excel 物件
                Excel.Application xls = null;
                try
                {
                    xls = new Excel.Application();
                    // Excel WorkBook
                    Excel.Workbook book = xls.Workbooks.Add();
                    //Excel.Worksheet Sheet = (Excel.Worksheet)book.Worksheets[1];
                    Excel.Worksheet Sheet = xls.ActiveSheet;

                    // 把資料塞進 Excel 內

                    // 標題
                    Sheet.Cells[1, 1] = "體重範圍";
                    Sheet.Cells[1, 2] = "人數";

                    // 內容
                    for (int k = 0; k < this.chart1.Series["weight"].Points.Count; k++)
                    {
                        Sheet.Cells[k + 2, 1] = this.chart1.Series["weight"].Points[k].AxisLabel.ToString();
                        Sheet.Cells[k + 2, 2] = this.chart1.Series["weight"].Points[k].YValues[0].ToString();
                    }


                    // 儲存檔案
                    book.SaveAs(save.FileName);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    xls.Quit();
                }

            }
            else if(selectedFunction == 3)//匯出資料(csv)
            {
                //export CSV function has not finished yet
            }
            else if(selectedFunction == 4)//匯出圖表(jpg)
            {
                SaveFileDialog save = new SaveFileDialog();
                save.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                save.FileName = "Export_Chart_JPG";
                save.Filter = "*.jpg|*.jpg";
                if (save.ShowDialog() != DialogResult.OK) return;

                chart1.SaveImage(save.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                //do nothing
            }
        }

        private void calculateAverageBtn_Click(object sender, EventArgs e)
        {
            int count = dataGridView1.Rows.Count;
            double heightTotal = 0;
            double heightAvg = 0;
            double weightTotal = 0;
            double weightAvg = 0;
            double bmiTotal = 0;
            double bmiAvg = 0;
            double chScoreTotal = 0;
            double chScoreAvg = 0;
            double enScoreTotal = 0;
            double enScoreAvg = 0;
            double mathScoreTotal = 0;
            double mathScoreAvg = 0;
            for (int i = 0; i < count; i++)
            {
                heightTotal = heightTotal + Convert.ToDouble(dataGridView1.Rows[i].Cells[2].Value);
                weightTotal = weightTotal + Convert.ToDouble(dataGridView1.Rows[i].Cells[3].Value);
                bmiTotal += Convert.ToDouble(dataGridView1.Rows[i].Cells[4].Value);
                chScoreTotal += Convert.ToDouble(dataGridView1.Rows[i].Cells[5].Value);
                enScoreTotal += Convert.ToDouble(dataGridView1.Rows[i].Cells[6].Value);
                mathScoreTotal += Convert.ToDouble(dataGridView1.Rows[i].Cells[7].Value);
            }
            heightAvg = heightTotal / (count - 1);//count要減一 是因為有一列空白欄位
            weightAvg = weightTotal / (count - 1);
            bmiAvg = bmiTotal / (count - 1);
            chScoreAvg = chScoreTotal / (count - 1);
            enScoreAvg = enScoreTotal / (count - 1);
            mathScoreAvg = mathScoreTotal / (count - 1);

            MessageBox.Show("總人數:" + (count - 1) +
                "\n平均身高為:" + heightAvg.ToString() + "公尺(m)" +
                "\n平均體重為:" + weightAvg.ToString() + "公斤(kg)" +
                "\n平均BMI為:" + bmiAvg.ToString() +
                "\n平均國文成績為:" + chScoreAvg.ToString() +
                "\n平均英文成績為:" + enScoreAvg.ToString() +
                "\n平均數學成績為:" + mathScoreAvg.ToString());
        }

        private void randomBigDataBtn_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.Owner = this;//重要的一步，使Form2的Owner指針指向Form1
            f3.ShowDialog();
            if(randomBigDataPageReturnStatus == 1)
            {
                Random rnd = new Random();
                for (int i = 0; i < dataCount; i++)
                {
                    double randomH = Convert.ToDouble(rnd.Next(minHeight, maxHeight)) / 100;
                    double randomW = Convert.ToDouble(rnd.Next(minWeight, maxWeight)) / 100;
                    publicVariables.height = randomH;
                    publicVariables.weight = randomW;
                    publicVariables.chineseScore = rnd.Next(minCHscore, maxCHscore);
                    publicVariables.englishScore = rnd.Next(minENscore, maxENscore);
                    publicVariables.mathScore = rnd.Next(minMATHscore, maxMATHscore);
                    publicVariables.name = "Random" + nameStartID.ToString();

                    publicVariables.bmi = publicVariables.weight / (publicVariables.height * publicVariables.height);//計算BMI

                    string sql = @"INSERT INTO record(id,name,height,weight,BMI,chineseScore,englishScore,mathScore) VALUES(" + publicVariables.id.ToString() + ",'" + publicVariables.name + "'," + publicVariables.height.ToString() + "," + publicVariables.weight.ToString() + "," + publicVariables.bmi.ToString() + "," + publicVariables.chineseScore.ToString() + "," + publicVariables.englishScore.ToString() + "," + publicVariables.mathScore.ToString() + ");";
                    //宣告一個字串存放要執行的SQL指令
                    DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
                    DBConfig.sqlite_cmd.ExecuteNonQuery();//執行SQL指令(寫入)

                    nameStartID += 1;//更新下一位隨機生成數據的名字
                    publicVariables.id += 1;//更新下一筆隨機生成數據的ID
                }
                //儲存完多筆資料後再更新圖表即顯示於dataGridView上
                show_DB();//把資料庫讀取出來並顯示於dataGridView上
                updateChart();//新增資料後，更新圖表
            }
            else
            {
                //do nothing
            }
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            this.TopMost = false;//滑鼠點擊視窗後取消掉固定在最上層
        }
    }
}
