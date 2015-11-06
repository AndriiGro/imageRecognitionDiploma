using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;

namespace ImageRecognition
{
    public class neuralNetworkClass
    {
        double[,] weights = new double [100, 1500];
                
        public neuralNetworkClass(){  }

        private Form1 form1; 
        public neuralNetworkClass(Form1 form1)
            : this()
        {
            // TODO: Complete member initialization
            this.form1 = form1;
        }

        private Form3 form3;
        public neuralNetworkClass(Form3 form3)
            : this()
        {
            // TODO: Complete member initialization
            this.form3 = form3;
        }

        public void SetWeightsArray(double[,] arr)
        {
            weights = arr;
        }

        public double[,] GetWeightsArray()
        {
            return weights;
        }

        public void SerializeWeightsArray()
        {
            using (var savefile = new SaveFileDialog())
            {                
                if (savefile.ShowDialog() == DialogResult.OK)
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    FileStream fs = new FileStream(savefile.FileName, FileMode.Create, FileAccess.Write);
                    bf.Serialize(fs, weights);
                    fs.Close();
                }
            }            
        }

        public void DeserializeWeightsArray()
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Load weights array";
                BinaryFormatter bf = new BinaryFormatter();
                
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(dlg.FileName, FileMode.Open);
                    weights = (double[,]) bf.Deserialize(fs);
                    fs.Close();
                }
            }
        }

        public void CheckArray()
        {
            MessageBox.Show("Populated 10, 15:" + weights[0, 0].ToString());
        }

        public void PopulateArrayWithRandom()
        {
            Random rnd = new Random();
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    weights[i, j] = rnd.NextDouble();
                }
            }            
        }


        //public void CreateWeightsArray(int x, int y)
        //{
        //    weights = new double[x,y * 10]; 
        //}
    }
}
