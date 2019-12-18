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
            f1.randomBigDataPageReturnStatus = -1;
            this.Close();
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)this.Owner;
            try
            {
                f1.nameStartID = int.Parse(nameStartIDtextBox.Text);
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
