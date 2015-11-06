namespace ImageRecognition
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBoxVideo = new System.Windows.Forms.PictureBox();
            this.buttonSnap = new System.Windows.Forms.Button();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.comboBoxDevices = new System.Windows.Forms.ComboBox();
            this.buttonRecognize = new System.Windows.Forms.Button();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.checkBoxBinarize = new System.Windows.Forms.CheckBox();
            this.buttonNeural1 = new System.Windows.Forms.Button();
            this.buttonNeural2 = new System.Windows.Forms.Button();
            this.buttonNeural3 = new System.Windows.Forms.Button();
            this.textBoxRecognized = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxVideo
            // 
            this.pictureBoxVideo.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.pictureBoxVideo.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxVideo.Name = "pictureBoxVideo";
            this.pictureBoxVideo.Size = new System.Drawing.Size(462, 337);
            this.pictureBoxVideo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxVideo.TabIndex = 0;
            this.pictureBoxVideo.TabStop = false;
            this.pictureBoxVideo.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxVideo_Paint);
            this.pictureBoxVideo.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxVideo_MouseDown);
            this.pictureBoxVideo.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxVideo_MouseMove);
            // 
            // buttonSnap
            // 
            this.buttonSnap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonSnap.Location = new System.Drawing.Point(263, 357);
            this.buttonSnap.Name = "buttonSnap";
            this.buttonSnap.Size = new System.Drawing.Size(75, 23);
            this.buttonSnap.TabIndex = 1;
            this.buttonSnap.Text = "Snap picture";
            this.buttonSnap.UseVisualStyleBackColor = true;
            this.buttonSnap.Click += new System.EventHandler(this.buttonSnap_Click);
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonConnect.Location = new System.Drawing.Point(172, 357);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // comboBoxDevices
            // 
            this.comboBoxDevices.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.comboBoxDevices.FormattingEnabled = true;
            this.comboBoxDevices.Location = new System.Drawing.Point(45, 359);
            this.comboBoxDevices.Name = "comboBoxDevices";
            this.comboBoxDevices.Size = new System.Drawing.Size(121, 21);
            this.comboBoxDevices.TabIndex = 3;
            // 
            // buttonRecognize
            // 
            this.buttonRecognize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonRecognize.Location = new System.Drawing.Point(355, 357);
            this.buttonRecognize.Name = "buttonRecognize";
            this.buttonRecognize.Size = new System.Drawing.Size(92, 23);
            this.buttonRecognize.TabIndex = 4;
            this.buttonRecognize.Text = "Recognize";
            this.buttonRecognize.UseVisualStyleBackColor = true;
            this.buttonRecognize.Click += new System.EventHandler(this.buttonRecognize_Click);
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonLoadImage.Location = new System.Drawing.Point(453, 357);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadImage.TabIndex = 5;
            this.buttonLoadImage.Text = "Load image";
            this.buttonLoadImage.UseVisualStyleBackColor = true;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // checkBoxBinarize
            // 
            this.checkBoxBinarize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.checkBoxBinarize.AutoSize = true;
            this.checkBoxBinarize.Location = new System.Drawing.Point(12, 363);
            this.checkBoxBinarize.Name = "checkBoxBinarize";
            this.checkBoxBinarize.Size = new System.Drawing.Size(15, 14);
            this.checkBoxBinarize.TabIndex = 6;
            this.checkBoxBinarize.UseVisualStyleBackColor = true;
            // 
            // buttonNeural1
            // 
            this.buttonNeural1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNeural1.Enabled = false;
            this.buttonNeural1.Location = new System.Drawing.Point(480, 33);
            this.buttonNeural1.Name = "buttonNeural1";
            this.buttonNeural1.Size = new System.Drawing.Size(75, 35);
            this.buttonNeural1.TabIndex = 7;
            this.buttonNeural1.Text = "Load weights";
            this.buttonNeural1.UseVisualStyleBackColor = true;
            this.buttonNeural1.Click += new System.EventHandler(this.buttonNeural1_Click);
            // 
            // buttonNeural2
            // 
            this.buttonNeural2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNeural2.Location = new System.Drawing.Point(480, 74);
            this.buttonNeural2.Name = "buttonNeural2";
            this.buttonNeural2.Size = new System.Drawing.Size(75, 23);
            this.buttonNeural2.TabIndex = 8;
            this.buttonNeural2.Text = "Teach ";
            this.buttonNeural2.UseVisualStyleBackColor = true;
            this.buttonNeural2.Click += new System.EventHandler(this.buttonNeural2_Click);
            // 
            // buttonNeural3
            // 
            this.buttonNeural3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonNeural3.Location = new System.Drawing.Point(480, 103);
            this.buttonNeural3.Name = "buttonNeural3";
            this.buttonNeural3.Size = new System.Drawing.Size(75, 23);
            this.buttonNeural3.TabIndex = 9;
            this.buttonNeural3.Text = "Don\'t touch";
            this.buttonNeural3.UseVisualStyleBackColor = true;
            this.buttonNeural3.Click += new System.EventHandler(this.buttonNeural3_Click);
            // 
            // textBoxRecognized
            // 
            this.textBoxRecognized.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRecognized.Location = new System.Drawing.Point(480, 329);
            this.textBoxRecognized.Name = "textBoxRecognized";
            this.textBoxRecognized.Size = new System.Drawing.Size(75, 20);
            this.textBoxRecognized.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(480, 298);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 28);
            this.label1.TabIndex = 11;
            this.label1.Text = "Recognized numbers:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 392);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxRecognized);
            this.Controls.Add(this.buttonNeural3);
            this.Controls.Add(this.buttonNeural2);
            this.Controls.Add(this.buttonNeural1);
            this.Controls.Add(this.checkBoxBinarize);
            this.Controls.Add(this.buttonLoadImage);
            this.Controls.Add(this.buttonRecognize);
            this.Controls.Add(this.comboBoxDevices);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.buttonSnap);
            this.Controls.Add(this.pictureBoxVideo);
            this.Name = "Form1";
            this.Text = "Image reconnition by Grondzal";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxVideo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxVideo;
        private System.Windows.Forms.Button buttonSnap;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.ComboBox comboBoxDevices;
        private System.Windows.Forms.Button buttonRecognize;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.CheckBox checkBoxBinarize;
        private System.Windows.Forms.Button buttonNeural1;
        private System.Windows.Forms.Button buttonNeural2;
        private System.Windows.Forms.Button buttonNeural3;
        private System.Windows.Forms.TextBox textBoxRecognized;
        private System.Windows.Forms.Label label1;
    }
}

