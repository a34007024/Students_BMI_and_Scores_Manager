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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        //form1與form2之間傳值參考自:
        //http://b20259isgood.blogspot.com/2014/03/cform.html
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)this.Owner;//把Form2的父窗口指針賦予給f1
            f1.selectedFunction = 1;//使用父窗口指針賦予值
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)this.Owner;//把Form2的父窗口指針賦予給f1
            f1.selectedFunction = 2;//使用父窗口指針賦予值
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)this.Owner;//把Form2的父窗口指針賦予給f1
            f1.selectedFunction = 3;//使用父窗口指針賦予值
            this.Close();
            //export CSV function has not finished yet
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form1 f1 = (Form1)this.Owner;//把Form2的父窗口指針賦予給f1
            f1.selectedFunction = 4;//使用父窗口指針賦予值
            this.Close();
        }

    }
}
