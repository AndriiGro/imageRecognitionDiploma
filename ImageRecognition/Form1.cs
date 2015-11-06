using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using AForge.Imaging.Filters;

namespace ImageRecognition
{
    public partial class Form1 : Form
    {
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        Bitmap snapPicture, video, loadedImage;
        private Point RectStartPoint;
        private Rectangle Rect = new Rectangle();
        private Brush selectionBrush = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
        List<Bitmap> foundImagesArray = new List<Bitmap>();        
        Median median = new Median();

        private enum StepDirection
        {
            None,
            Up,
            Left,
            Down,
            Right
        }
        private StepDirection previousStep;
        private StepDirection nextStep;
        List<Point> contour = new List<Point>();
        int initX = 0, initY = 0;        

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (FinalVideo.IsRunning == true) FinalVideo.Stop();

            pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[comboBoxDevices.SelectedIndex].MonikerString);
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            FinalVideo.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            foreach (FilterInfo VideoCaptureDevice in VideoCaptureDevices)
            {
                comboBoxDevices.Items.Add(VideoCaptureDevice.Name);
            }

            comboBoxDevices.SelectedIndex = 0;
            FinalVideo = new VideoCaptureDevice();
        }

        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            video = (Bitmap)eventArgs.Frame.Clone();
            if (checkBoxBinarize.Checked == true)
            {
                video = ImageBinarization(video);
            }
            pictureBoxVideo.Image = video;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (FinalVideo.IsRunning == true) FinalVideo.Stop();
        }

        private void buttonSnap_Click(object sender, EventArgs e)
        {
            snapPicture = video;
            FinalVideo.Stop();
            pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBoxVideo.Image = snapPicture;
        }

        private void pictureBoxVideo_MouseDown(object sender, MouseEventArgs e)
        {
            RectStartPoint = e.Location;
            Invalidate();
        }

        private void pictureBoxVideo_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
            {
                return;
            }
            Point tempEndPoint = e.Location;
            Rect =
                new Rectangle(
                    Math.Min(RectStartPoint.X, tempEndPoint.X),
                    Math.Min(RectStartPoint.Y, tempEndPoint.Y),
                    Math.Abs(RectStartPoint.X - tempEndPoint.X),
                    Math.Abs(RectStartPoint.Y - tempEndPoint.Y));
            Invalidate(Rect);
            pictureBoxVideo.Refresh();
        }

        private void pictureBoxVideo_Paint(object sender, PaintEventArgs e)
        {
            if (pictureBoxVideo.Image != null)
            {
                Brush brush = new SolidBrush(Color.FromArgb(128, 72, 145, 220));
                e.Graphics.FillRectangle(brush, Rect);
            }
        }

        private void buttonRecognize_Click(object sender, EventArgs e)
        {
            Bitmap inImage = snapPicture;         
            Bitmap outImage = ImageBinarization(inImage); 
            pictureBoxVideo.Image = outImage;
            //Rect.Width = Rect.Height = 0;
            MessageBox.Show("Binarized");

            median.Size = 3;
            median.ApplyInPlace(outImage);

            outImage = adaptBinarizedImage(outImage);
            
            pictureBoxVideo.Image = outImage;
            MessageBox.Show("Filtered");

            snapPicture = outImage;          
            
            //Some magic
            FindObjects();
            int imagesCount = foundImagesArray.Count();
            MessageBox.Show(imagesCount + " objects are found. Ready to recognize");

            //Recognizing for each bitmap number
            Form3 frm3 = new Form3();
            neuralNetworkClass neuralClass = new neuralNetworkClass();
            double[,] weightsArray;
            List<int> foundNumList = new List<int>();
            //Obtaining weights array
            neuralClass.DeserializeWeightsArray();
            weightsArray = neuralClass.GetWeightsArray();
            foreach (Bitmap bmp in foundImagesArray)
            {
                int classNum = 0;
                double outParam = 0.0;
                for (int i = 0; i < 10; i++)
                {
                    double temp = frm3.GetOutParam(i, weightsArray, bmp);
                    if (temp > outParam)
                    {
                        outParam = temp;
                        classNum = i;
                    }
                }
                foundNumList.Add(classNum);
            }
            String nums = "";
            foreach (int number in foundNumList)
            {
                nums += number.ToString() + " ";
            }
            textBoxRecognized.Text = nums;
            
            //Form2 frm2 = new Form2(this);
            //frm2.showForm();

            //foreach (Bitmap bmp in foundImagesArray)
            //{
            //    frm2.setImage(bmp);
            //    MessageBox.Show("Are found ;)");
            //}       
        }

        public void FindObjects()
        {
            bool isThereAnymoreObjects = true;
            do
            {
                contour.Clear();
                isThereAnymoreObjects = FindStartPoint(snapPicture);
                if (isThereAnymoreObjects) break;
                WalkPerimeter(initX, initY);
                if (!IsObjectTouchBounds(contour) && contour.Count > 100)
                {
                    Bitmap tmp = CopyIrregularAreaFromBitmap(snapPicture, contour, Color.White);
                    tmp = cutImage(tmp);
                    tmp = LinearInterpolationScale(tmp, 100, 150);
                    tmp = ImageBinarization(tmp);
                    foundImagesArray.Add(tmp);
                    FillIrregularAreaByColour(snapPicture, contour, Color.White);
                }
                else
                {
                    FillIrregularAreaByColour(snapPicture, contour, Color.White);
                }                
            }while(true);
            MessageBox.Show("Search is finished");
        }

        public bool IsObjectTouchBounds(List<Point> points)
        {
            bool isTouch = false;
            foreach(Point p in points)
            {
                if (p.X == 1 || p.X == snapPicture.Width - 1 || 
                    p.Y == 1 || p.Y == snapPicture.Height - 1)
                {
                    isTouch = true;
                }
            }
            return isTouch;
        }  
        
        private static Bitmap cropImage(Bitmap bmpImage, Rectangle cropArea)
        {
            Bitmap bmpCrop = bmpImage.Clone(cropArea, bmpImage.PixelFormat);
            return bmpCrop;
        }

        public Bitmap ImageBinarization(Bitmap img)
        {
            Bitmap outImage = new Bitmap(img.Width, img.Height);
            Color[,] color = new Color[img.Width, img.Height];

            for (int i = 0; i < img.Width; i++)
            {
                for (int x = 0; x < img.Height; x++)
                {
                    Color oc = img.GetPixel(i, x);
                    Color nc;
                    if ((int)((oc.R + oc.G + oc.B) / 3) > 127)
                    {
                        nc = Color.White;
                    }
                    else
                    {
                        nc = Color.Black;
                    }
                    outImage.SetPixel(i, x, nc);
                }
            }
            return outImage;
        }              

        public List<Point> findContour(Bitmap bmp, Point p)
        {
            List<Point> points = new List<Point>();
            points.Add(p);
            int x = p.X, y = p.Y;
            do
            {

                Color clr = bmp.GetPixel(p.X - 1, p.Y - 1);
                //if( == 0)
                //{

                //}
            }
            while (points[0] == p);
            return points;
        }        

        public Bitmap cutImage(Bitmap bmp)
        {
            int newX = 0, newY = 0, newHeight = bmp.Height, newWidth = bmp.Width;

            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);
                    if (clr.R == 0)
                    {
                        newX = x;
                        goto newYSearch;
                    }
                }
            }

        newYSearch:

            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    Color clr = bmp.GetPixel(x, y);
                    if (clr.R == 0)
                    {
                        newY = y;
                        goto newWidthSearch;
                    }
                }
            }

        newWidthSearch:

            for (int x = bmp.Width - 1; x > 0; x--)
            {
                for (int y = bmp.Height - 1; y > 0; y--)
                {
                    Color clr = bmp.GetPixel(x, y);
                    if (clr.R == 0)
                    {
                        newWidth = x - newX;
                        goto newHeightSearch;
                    }
                }
            }

        newHeightSearch:

            for (int y = bmp.Height - 1; y > 0; y--)
            {
                for (int x = bmp.Width - 1; x > 0; x--)
                {
                    Color clr = bmp.GetPixel(x, y);
                    if (clr.R == 0)
                    {
                        newHeight = y - newY;
                        goto nextStep;
                    }
                }
            }

        nextStep:

            Rect.X = newX;
            Rect.Y = newY;
            Rect.Height = newHeight;
            Rect.Width = newWidth;

            Bitmap outBmp = cropImage(bmp, Rect);
            Rect.Width = Rect.Height = 0;
            return outBmp;
        }
        
        public Bitmap LinearInterpolationScale(Bitmap bmp, int newXSize, int newYSize)
        {
            Bitmap newBMP = new Bitmap(newXSize, newYSize);

            int[] temp = new int[newXSize * newYSize];
            int x, y;
            Color A, B, C, D;
            float x_ratio = ((float)(bmp.Width - 1)) / newXSize;
            float y_ratio = ((float)(bmp.Height - 1)) / newYSize;

            float w, h;

            for (int i = 0; i < newYSize; i++)
            {
                for (int j = 0; j < newXSize; j++)
                {
                    x = (int)(x_ratio * j);
                    y = (int)(y_ratio * i);
                    w = (x_ratio * j) - x;
                    h = (y_ratio * i) - y;

                    A = bmp.GetPixel(x, y);
                    B = bmp.GetPixel(x + 1, y);
                    C = bmp.GetPixel(x, y + 1);
                    D = bmp.GetPixel(x + 1, y + 1);

                    int r = (int)(A.R * (1 - w) * (1 - h) + B.R * (w) * (1 - h) + C.R * (h) * (1 - w) + D.R * (w * h));
                    int g = (int)(A.G * (1 - w) * (1 - h) + B.G * (w) * (1 - h) + C.G * (h) * (1 - w) + D.G * (w * h));
                    int b = (int)(A.B * (1 - w) * (1 - h) + B.B * (w) * (1 - h) + C.B * (h) * (1 - w) + D.B * (w * h));

                    Color col = Color.FromArgb(r, g, b);

                    newBMP.SetPixel(j, i, col);
                }
            }
            return newBMP;
        }

        //Contour tracing functions
        private Bitmap adaptBinarizedImage(Bitmap img)
        {
            img = cutImage(img);
            Bitmap output = new Bitmap(img.Width + 2, img.Height + 2);
            Color[,] colors = new Color[output.Width, output.Height];
                       
                for (int x = 0; x < output.Width; x++)
                {
                    for (int y = 0; y < output.Height; y++)
                    {
                        if (x == 0 | y == 0 | (x == 0 & y == 0) | x == output.Width - 1 |
                            y == output.Height - 1 | (x == output.Width - 1 & y == output.Height - 1))
                        {
                            colors[x, y] = Color.White;
                        }
                        else
                        {
                            colors[x, y] = img.GetPixel(x - 1, y - 1);
                        }
                    }
                }            
            
            for (int y = 0; y < output.Height; y++)
                for (int x = 0; x < output.Width; x++)
                {
                    output.SetPixel(x, y, colors[x, y]);
                }
            
            return output; 
        }

        private void buttonLoadImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    loadedImage = new Bitmap(dlg.FileName);
                    snapPicture = loadedImage;
                    pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
                    pictureBoxVideo.Image = snapPicture;                    
                }
            }
        }        

        private bool IsPixelSolid(int x, int y)
        {
            // Make sure we don't pick a point outside our
            // image boundary!
            if (x < 0 || y < 0 || x >= snapPicture.Width || y >= snapPicture.Height)
                return false;

            int argb = Color.Black.ToArgb();
            // Check the color value of the pixel   
            Color clr = snapPicture.GetPixel(x, y);
            if (clr.ToArgb() == argb)
                return true;
            
            // Otherwise, it's not black
            return false;
        }

        // Determines and sets the state of the 4 pixels that
        // represent our current state, and sets our current and
        // previous directions
        private void Step(int x, int y)
        {
            // Scan our 4 pixel area
            bool upLeft = IsPixelSolid(x - 1, y - 1);
            bool upRight = IsPixelSolid(x, y - 1);
            bool downLeft = IsPixelSolid(x - 1, y);
            bool downRight = IsPixelSolid(x, y);

            // Store our previous step
            previousStep = nextStep;

            // Determine which state we are in
            int state = 0;

            if (upLeft)
                state |= 1;
            if (upRight)
                state |= 2;
            if (downLeft)
                state |= 4;
            if (downRight)
                state |= 8;

            
            switch (state)
            {
                case 1: nextStep = StepDirection.Up; break;
                case 2: nextStep = StepDirection.Right; break;
                case 3: nextStep = StepDirection.Right; break;
                case 4: nextStep = StepDirection.Left; break;
                case 5: nextStep = StepDirection.Up; break;
                case 6:
                    if (previousStep == StepDirection.Up)
                    {
                        nextStep = StepDirection.Left;
                    }
                    else
                    {
                        nextStep = StepDirection.Right;
                    }
                    break;
                case 7: nextStep = StepDirection.Right; break;
                case 8: nextStep = StepDirection.Down; break;
                case 9:
                    if (previousStep == StepDirection.Right)
                    {
                        nextStep = StepDirection.Up;
                    }
                    else
                    {
                        nextStep = StepDirection.Down;
                    }
                    break;
                case 10: nextStep = StepDirection.Down; break;
                case 11: nextStep = StepDirection.Down; break;
                case 12: nextStep = StepDirection.Left; break;
                case 13: nextStep = StepDirection.Up; break;
                case 14: nextStep = StepDirection.Left; break;
                default:
                    nextStep = StepDirection.None;
                    break;
            }
        }

        public bool FindStartPoint(Bitmap bmp)
        {
            bool imageIsClear = false;
            int argb = Color.Black.ToArgb();
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    Color clr = bmp.GetPixel(x, y);
                    
                    if (argb == clr.ToArgb())
                    {                        
                        initX = x;
                        initY = y;
                        return imageIsClear;
                    }                   
                }
            }
            imageIsClear = true;
            return imageIsClear;
        }

        public void WalkPerimeter(int startX, int startY)
        {
            // Do some sanity checking, so we aren't
            // walking outside the image           
            if (startX < 0)
                startX = 0;
            if (startX > snapPicture.Width)
                startX = snapPicture.Width;
            if (startY < 0)
                startY = 0;
            if (startY > snapPicture.Height)
                startY = snapPicture.Height;
            // Set up our return list
            //List<Vector2> pointList = new List<Vector2>();

            // Our current x and y positions, initialized
            // to the init values passed in
            int x = startX;
            int y = startY;

            // The main while loop, continues stepping until
            // we return to our initial points
            do
            {
                // Evaluate our state, and set up our next direction
                Step(x, y);
                //MessageBox.Show("surprise motherfucker! 3");
                // If our current point is within our image
                // add it to the list of points
                if (x >= 0 &&
                    x < snapPicture.Width &&
                    y >= 0 &&
                    y < snapPicture.Height)
                    contour.Add(new Point(x, y));
                    //pointList.Add(new Vector2(x, y));
               // MessageBox.Show("surprise motherfucker! 4" + nextStep.ToString());
                switch (nextStep)
                {
                    case StepDirection.Up: y--; break;
                    case StepDirection.Left: x--; break;
                    case StepDirection.Down: y++; break;
                    case StepDirection.Right: x++; break;
                    default:
                        break;
                }
                //MessageBox.Show("surprise motherfucker! 5 X: " +x + "Y: " + y + "Init:" + initX + initY);

                //bool iterator = (x != initX || y != initY);
                //MessageBox.Show("Iterator = " + iterator.ToString());

            } while (x != initX || y != initY);
            //MessageBox.Show("surprise motherfucker! 6 ;( ");
            //return pointList;
        }

        public Bitmap DrawImageFromPointList(List<Point> points)
        { 
            Bitmap bmp = new Bitmap(snapPicture.Width, snapPicture.Height);
            for (int x = 0; x < bmp.Width; x++)
            {
                for (int y = 0; y < bmp.Height; y++)
                {
                    bmp.SetPixel(x, y, Color.White);
                }
            }

            foreach(Point p in points)
            {
                bmp.SetPixel(p.X, p.Y, Color.Black);
            }

            return bmp;

        }

        //Other methods to copy, color fill or filter found objects

        public Bitmap CopyIrregularAreaFromBitmap(Bitmap source, List<Point> points, Color bg_color)
        { 
            Bitmap copiedImg = new Bitmap(source);
            using (Graphics gr = Graphics.FromImage(copiedImg))
            {
                // Set the background color.
                gr.Clear(bg_color);

                // Make a brush out of the original image.
                using (Brush br = new TextureBrush(source))
                {
                    // Fill the selected area with the brush.
                    gr.FillPolygon(br, points.ToArray());

                    // Find the bounds of the selected area.
                    Rectangle source_rect = GetPointListBounds(points);

                    // Make a bitmap that only holds the selected area.
                    Bitmap result = new Bitmap(
                        source_rect.Width, source_rect.Height);

                    // Copy the selected area to the result bitmap.
                    using (Graphics result_gr = Graphics.FromImage(result))
                    {
                        Rectangle dest_rect = new Rectangle(0, 0,
                            source_rect.Width, source_rect.Height);
                        result_gr.DrawImage(copiedImg, dest_rect,
                            source_rect, GraphicsUnit.Pixel);
                    }

                    // Return the result.
                    return result;
                }
            }
        }

        private Rectangle GetPointListBounds(List<Point> points)
        {
            int xmin = points[0].X;
            int xmax = xmin;
            int ymin = points[0].Y;
            int ymax = ymin;

            for (int i = 1; i < points.Count; i++)
            {
                if (xmin > points[i].X) xmin = points[i].X;
                if (xmax < points[i].X) xmax = points[i].X;
                if (ymin > points[i].Y) ymin = points[i].Y;
                if (ymax < points[i].Y) ymax = points[i].Y;
            }

            return new Rectangle(xmin, ymin, xmax - xmin, ymax - ymin);
        }

        public void FillIrregularAreaByColour(Bitmap source, List<Point> points, Color fillClr)
        {
            Graphics gr = Graphics.FromImage(source);
            Brush fillBrush = new SolidBrush(fillClr);
            gr.FillPolygon(fillBrush, points.ToArray());           
        }

        private void buttonNeural1_Click(object sender, EventArgs e)
        {
            //neuralNetworkClass neuroClass = new neuralNetworkClass();
            //neuroClass.PopulateArrayWithRandom();
            //neuroClass.CheckArray();
            //neuroClass.SerializeWeightsArray();
            //MessageBox.Show("Array saved");
            //neuroClass.DeserializeWeightsArray();
            //MessageBox.Show("Weights loaded");
            //neuroClass.CheckArray();
        }

        private void buttonNeural2_Click(object sender, EventArgs e)
        {
            Form3 frm3 = new Form3();
            frm3.showForm();
        }

        private void buttonNeural3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("You little rebel, I like you.");
        }

        //Some
        //garbage 

        //public Bitmap AdjustImage(Bitmap originalImage)
        //{
        //    //Bitmap origanalImage;
        //    Bitmap adjustedImage = new Bitmap(originalImage.Width, originalImage.Height);
        //    float brightness = 1.0f; // no change in brightness
        //    float contrast = 1.5f; // twice the contrast
        //    //double gamma = 1.0f; // no change in gamma

        //    float adjustedBrightness = brightness - 1.0f;
        //    // create matrix that will brighten and contrast the image
        //    float[][] ptsArray ={
        //            new float[] {contrast, 0, 0, 0, 0}, // scale red
        //            new float[] {0, contrast, 0, 0, 0}, // scale green
        //            new float[] {0, 0, contrast, 0, 0}, // scale blue
        //            new float[] {0, 0, 0, 1.0f, 0}, // don't scale alpha
        //            new float[] {adjustedBrightness, adjustedBrightness, adjustedBrightness, 0, 1}};

        //    ImageAttributes imageAttributes = new ImageAttributes();
        //    imageAttributes.ClearColorMatrix();
        //    imageAttributes.SetColorMatrix(new ColorMatrix(ptsArray), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        //    //imageAttributes.SetGamma(gamma, ColorAdjustType.Bitmap);
        //    Graphics g = Graphics.FromImage(adjustedImage);
        //    g.DrawImage(originalImage, new Rectangle(0, 0, adjustedImage.Width, adjustedImage.Height)
        //        , 0, 0, originalImage.Width, originalImage.Height,
        //        GraphicsUnit.Pixel, imageAttributes);
        //    return adjustedImage;
        //}

        //private Color[,] BitmapToColorArray(Bitmap bmp)
        //{
        //    Color[,] color = new Color[bmp.Width, bmp.Height];
        //    for (int y = 0; y < bmp.Height; y++)
        //    {
        //        for (int x = 0; x < bmp.Width; x++)
        //        {
        //            color[x, y] = bmp.GetPixel(x, y);
        //        }
        //    }
        //    return color;
        //}

        //private Bitmap ColorArrayToBitmap(Color[,] color)
        //{
        //    Bitmap bmp = new Bitmap(color.GetLength(0), color.GetLength(1));
        //    for (int y = 0; y < bmp.Height; y++)
        //    {
        //        for (int x = 0; x < bmp.Width; x++)
        //        {
        //            bmp.SetPixel(x, y, color[x, y]);
        //        }
        //    }
        //    return bmp;
        //}

      //  public static Bitmap MakeGrayscale3(Bitmap original)
      //  {
      //      //create a blank bitmap the same size as original
      //      Bitmap newBitmap = new Bitmap(original.Width, original.Height);

      //      //get a graphics object from the new image
      //      Graphics g = Graphics.FromImage(newBitmap);

      //      //create the grayscale ColorMatrix
      //      ColorMatrix colorMatrix = new ColorMatrix(
      //         new float[][] 
      //{
      //   new float[] {.3f, .3f, .3f, 0, 0},
      //   new float[] {.59f, .59f, .59f, 0, 0},
      //   new float[] {.11f, .11f, .11f, 0, 0},
      //   new float[] {0, 0, 0, 1, 0},
      //   new float[] {0, 0, 0, 0, 1}
      //});

      //      //create some image attributes
      //      ImageAttributes attributes = new ImageAttributes();

      //      //set the color matrix attribute
      //      attributes.SetColorMatrix(colorMatrix);

      //      //draw the original image on the new image
      //      //using the grayscale color matrix
      //      g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
      //         0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

      //      //dispose the Graphics object
      //      g.Dispose();
      //      return newBitmap;
      //  }

        //public Bitmap cutBlackGarbage(Bitmap bmp)
        //{
        //    Point p, pFlip;
        //    p = findGarbage(bmp);
        //    Bitmap fliped180 = bmp;
        //    fliped180.RotateFlip(RotateFlipType.Rotate180FlipNone);
        //    pFlip = findGarbage(fliped180);

        //    Rect.X = p.X;
        //    Rect.Y = p.Y;
        //    MessageBox.Show("pFlip.X= " + pFlip.X);
        //    Rect.Height = bmp.Height - pFlip.X;
        //    Rect.Width = bmp.Width - pFlip.Y;

        //    Bitmap outBmp = cropImage(bmp, Rect);
        //    return outBmp;
        //}

        //public Point findGarbage(Bitmap bmp)
        //{
        //    Point p = new Point(0, 0);
        //    int newX = 0, newY = 0, range = bmp.Width / 3;
        //    bool doNotStopCycle = true;

        //    try
        //    {
        //        do
        //        {

        //            bool findX = false, findY = false;
        //            int x = 0, y = 0;
        //            for (; y <= range; y++)
        //            {
        //                Color clr = bmp.GetPixel(newX, y);
        //                if (clr.R == 0)
        //                {
        //                    break;
        //                }
        //                if (y == range)
        //                {
        //                    findX = true;
        //                }
        //            }

        //            for (; x <= range; x++)
        //            {
        //                Color clr = bmp.GetPixel(x, newY);
        //                if (clr.R == 0)
        //                {
        //                    break;
        //                }
        //                if (x == range)
        //                {
        //                    findY = true;
        //                }
        //            }
        //            if (findX && findY)
        //            {
        //                doNotStopCycle = false;
        //                newX = x;
        //                newY = y;
        //            }
        //            else
        //            {
        //                newX++;
        //                newY++;
        //                if (newY > bmp.Height) { doNotStopCycle = false; }
        //            }
        //        }
        //        while (doNotStopCycle);
        //    }
        //    catch (Exception ex) { MessageBox.Show(ex.Message); }
        //    p.X = newX;
        //    p.Y = newY;
        //    return p;
        //}

        //private Bitmap SobelEdgeDetect(Bitmap original)
        //{
        //    Bitmap b = original;
        //    Bitmap bb = original;
        //    int width = b.Width;
        //    int height = b.Height;
        //    int[,] gx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } };
        //    int[,] gy = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

        //    int[,] allPixR = new int[width, height];
        //    int[,] allPixG = new int[width, height];
        //    int[,] allPixB = new int[width, height];

        //    int limit = 128 * 128;

        //    for (int i = 0; i < width; i++)
        //    {
        //        for (int j = 0; j < height; j++)
        //        {
        //            allPixR[i, j] = b.GetPixel(i, j).R;
        //            allPixG[i, j] = b.GetPixel(i, j).G;
        //            allPixB[i, j] = b.GetPixel(i, j).B;
        //        }
        //    }

        //    int new_rx = 0, new_ry = 0;
        //    int new_gx = 0, new_gy = 0;
        //    int new_bx = 0, new_by = 0;
        //    int rc, gc, bc;
        //    for (int i = 1; i < b.Width - 1; i++)
        //    {
        //        for (int j = 1; j < b.Height - 1; j++)
        //        {

        //            new_rx = 0;
        //            new_ry = 0;
        //            new_gx = 0;
        //            new_gy = 0;
        //            new_bx = 0;
        //            new_by = 0;
        //            rc = 0;
        //            gc = 0;
        //            bc = 0;

        //            for (int wi = -1; wi < 2; wi++)
        //            {
        //                for (int hw = -1; hw < 2; hw++)
        //                {
        //                    rc = allPixR[i + hw, j + wi];
        //                    new_rx += gx[wi + 1, hw + 1] * rc;
        //                    new_ry += gy[wi + 1, hw + 1] * rc;

        //                    gc = allPixG[i + hw, j + wi];
        //                    new_gx += gx[wi + 1, hw + 1] * gc;
        //                    new_gy += gy[wi + 1, hw + 1] * gc;

        //                    bc = allPixB[i + hw, j + wi];
        //                    new_bx += gx[wi + 1, hw + 1] * bc;
        //                    new_by += gy[wi + 1, hw + 1] * bc;
        //                }
        //            }
        //            if (new_rx * new_rx + new_ry * new_ry > limit || new_gx * new_gx + new_gy * new_gy > limit || new_bx * new_bx + new_by * new_by > limit)
        //                bb.SetPixel(i, j, Color.Black);
        //            else
        //                bb.SetPixel(i, j, Color.White);
        //        }
        //    }
        //    return bb;

        //}

    }
}
