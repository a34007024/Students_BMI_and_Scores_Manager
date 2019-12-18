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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)this.Owner;
            f1.randomBigDataPageReturnStatus = 0;
            this.Close();
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)this.Owner;
            try
            {
                f1.nameStartID = int.Parse(nameStartIDtextBox.Text);
                f1.dataCount = int.Parse(dataCountTextBox.Text);
                f1.minCHscore = int.Parse(minCHscoreTextBox.Text);
                f1.minENscore = int.Parse(minENscoreTextBox.Text);
                f1.minMATHscore = int.Parse(minMATHscoreTextBox.Text);
                f1.minHeight = double.Parse(minHeightTextBox.Text);
                f1.minWeight = double.Parse(minWeightTextBox.Text);
                f1.maxCHscore = int.Parse(maxCHscoreTextBox.Text);
                f1.maxENscore = int.Parse(maxENscoreTextBox.Text);
                f1.maxMATHscore = int.Parse(maxMATHscoreTextBox.Text);
                f1.maxHeight = double.Parse(maxHeightTextBox.Text);
                f1.maxWeight = double.Parse(maxWeightTextBox.Text);
                f1.randomBigDataPageReturnStatus = 1;//各項參數成功回傳
            }
            catch
            {
                MessageBox.Show("輸入有誤!請檢查!\n\n成績只能輸入整數喔!", "Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Close();
        }
    }
}
