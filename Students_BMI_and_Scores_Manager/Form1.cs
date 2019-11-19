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
        public Form1()
        {
            InitializeComponent();
            idLabel.Text = index.ToString();
        }
        
        private void addDataBtn_Click(object sender, EventArgs e)
        {
            height = double.Parse(heightTextbox.Text);
            weight = double.Parse(weightTextbox.Text);
            name = nameTextbox.Text;
            dataGridView1.Rows.Add(new object[] { index, name, height, weight});
        }

        private void heightTextbox_TextChanged(object sender, EventArgs e)
        {

        }

        private void weightTextbox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
