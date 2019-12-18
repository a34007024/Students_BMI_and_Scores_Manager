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
            //f1.
            this.Close();
        }

        private void confirmBtn_Click(object sender, EventArgs e)
        {

            try
            {
                //nameIDtextBox.Text
            }
            catch
            {

            }
        }
    }
}
