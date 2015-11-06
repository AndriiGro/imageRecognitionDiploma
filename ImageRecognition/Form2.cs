using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageRecognition
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        public void Form2_Load(object sender, EventArgs e)
        {

        }

        private Form1 form1; 
        public Form2(Form1 form1)
            : this()
        {
            // TODO: Complete member initialization
            this.form1 = form1;
        }

        public void showForm()
        {
            this.Enabled = true;
            this.Show();
        }

        public void setImage(Bitmap img)
        {
            pictureBoxFinal.Image = img;
        }
    }
}
