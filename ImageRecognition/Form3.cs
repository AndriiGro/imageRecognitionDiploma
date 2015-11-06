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
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private Form1 frm1;
        public Form3(Form1 frm1)
            : this()
        {
            this.frm1 = frm1;
        }

        public void showForm()
        {
            this.Enabled = true;
            this.Show();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            neuralClass.DeserializeWeightsArray();
            weightsToTeach = neuralClass.GetWeightsArray();              
        }

        neuralNetworkClass neuralClass = new neuralNetworkClass();
        Bitmap bitmapNumberToTeach = new Bitmap(1, 1);
        Color[,] colorArr = new Color[100, 150];
        double[,] weightsToTeach;

        private void buttonLoad_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    bitmapNumberToTeach = new Bitmap(dlg.FileName);
                    //Adjust image ot required format
                    Form1 frm1 = new Form1();
                    bitmapNumberToTeach = frm1.cutImage(bitmapNumberToTeach);
                    bitmapNumberToTeach = frm1.LinearInterpolationScale(bitmapNumberToTeach, 100, 150);
                    bitmapNumberToTeach = frm1.ImageBinarization(bitmapNumberToTeach);
                    
                    pictureBoxTeach.Image = bitmapNumberToTeach;
                    
                    for (int y = 0; y < bitmapNumberToTeach.Height; y++)
                        for (int x = 0; x < bitmapNumberToTeach.Width; x++)
                        {
                            colorArr[x, y] = bitmapNumberToTeach.GetPixel(x, y);
                        }
                }
            }
        }
        
        private void buttonStartLearning_Click(object sender, EventArgs e)
        {
            int numOfClass = (int)numericUpDownClass.Value;
            //double progress = 0;
            //double net = 0;
            double outj = 0;
            double deltaW = 0;
            double mu = 0.01;
            double delta = 0;

            //labelProgress.Text = "Started...";

            do
            {
                outj = GetOutParam(numOfClass);
                for (int i = 0; i < bitmapNumberToTeach.Width; i++)
                {
                    for (int j = 0; j < bitmapNumberToTeach.Height; j++)
                    {                    
                        delta = 1 - outj;
                        deltaW = delta * outj * mu;
                        Color oc = bitmapNumberToTeach.GetPixel(i, j);
                        int argb = oc.ToArgb();
                        if (argb == Color.Black.ToArgb())
                        {
                            double temp = weightsToTeach[i, j + (numOfClass * 150)] + deltaW;
                            if (temp > 1.0)
                            {
                                weightsToTeach[i, j + (numOfClass * 150)] = 1.0;
                            }
                            else
                            {
                                weightsToTeach[i, j + (numOfClass * 150)] = temp;
                            }
                        }
                        else
                        {
                            double temp = weightsToTeach[i, j + (numOfClass * 150)] - deltaW;
                            if (temp < 0.0)
                            {
                                weightsToTeach[i, j + (numOfClass * 150)] = 0.0;
                            }
                            else
                            {
                                weightsToTeach[i, j + (numOfClass * 150)] = temp;
                            }
                        }                    
                    }                
                }
                labelProgress.Text = outj.ToString();
                labelProgress.Refresh();
            }
            while(outj <= 0.95);                   
        }

        public double GetOutParam(int classNum)
        {
            double outParam = 0;
            double netParam = 0;
            int blackPixels = 0;

            for (int i = 0; i < bitmapNumberToTeach.Width; i++)
            {
                for (int j = 0; j < bitmapNumberToTeach.Height; j++)
                {
                    int argb = colorArr[i, j].ToArgb();
                    if (argb == Color.Black.ToArgb())
                    {
                        blackPixels++;
                        netParam += weightsToTeach[i, j + (classNum * 150)];
                    }
                }
            }
            outParam = netParam / blackPixels;
            return outParam;
        }

        public double GetOutParam(int classNum, double[,] weightsArr, Bitmap bmp)
        {
            double outParam = 0;
            double netParam = 0;
            int blackPixels = 0;

            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    int argb = bmp.GetPixel(i, j).ToArgb();
                    //int argb = colorArr[i, j].ToArgb();
                    if (argb == Color.Black.ToArgb())
                    {
                        blackPixels++;
                        netParam += weightsArr[i, j + (classNum * 150)];
                    }
                }
            }
            outParam = netParam / blackPixels;
            return outParam;
        }

        private void buttonSaveChanges_Click(object sender, EventArgs e)
        {
            neuralClass.SetWeightsArray(weightsToTeach);
            neuralClass.SerializeWeightsArray();
            MessageBox.Show("Saved");
        }

        

        //public Color[,] GetColorArrayFromBitmap(Bitmap bmp)
        //{
        //    Color[,] color = new Color[bmp.Width, bmp.Height];
        //    for (int y = 0; y < bmp.Height; y++)
        //        for (int x = 0; x < bmp.Width; x++)
        //        {
        //            color[x, y] = bmp.GetPixel(x, y);
        //        }
        //    return color;
        //}


    }
}
