namespace ImageRecognition
{
    partial class Form3
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
            this.pictureBoxTeach = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelProgress = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numericUpDownClass = new System.Windows.Forms.NumericUpDown();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonStartLearning = new System.Windows.Forms.Button();
            this.buttonSaveChanges = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTeach)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClass)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxTeach
            // 
            this.pictureBoxTeach.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.pictureBoxTeach.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxTeach.Name = "pictureBoxTeach";
            this.pictureBoxTeach.Size = new System.Drawing.Size(100, 150);
            this.pictureBoxTeach.TabIndex = 0;
            this.pictureBoxTeach.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 305);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Progress:";
            // 
            // labelProgress
            // 
            this.labelProgress.AutoSize = true;
            this.labelProgress.Location = new System.Drawing.Point(62, 305);
            this.labelProgress.Name = "labelProgress";
            this.labelProgress.Size = new System.Drawing.Size(28, 13);
            this.labelProgress.TabIndex = 2;
            this.labelProgress.Text = "0.00";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(205, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Class:";
            // 
            // numericUpDownClass
            // 
            this.numericUpDownClass.Location = new System.Drawing.Point(208, 73);
            this.numericUpDownClass.Maximum = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.numericUpDownClass.Name = "numericUpDownClass";
            this.numericUpDownClass.Size = new System.Drawing.Size(75, 20);
            this.numericUpDownClass.TabIndex = 5;
            this.numericUpDownClass.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // buttonLoad
            // 
            this.buttonLoad.Location = new System.Drawing.Point(208, 12);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(75, 23);
            this.buttonLoad.TabIndex = 6;
            this.buttonLoad.Text = "Load image";
            this.buttonLoad.UseVisualStyleBackColor = true;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonStartLearning
            // 
            this.buttonStartLearning.Location = new System.Drawing.Point(208, 121);
            this.buttonStartLearning.Name = "buttonStartLearning";
            this.buttonStartLearning.Size = new System.Drawing.Size(75, 41);
            this.buttonStartLearning.TabIndex = 7;
            this.buttonStartLearning.Text = "Start learning";
            this.buttonStartLearning.UseVisualStyleBackColor = true;
            this.buttonStartLearning.Click += new System.EventHandler(this.buttonStartLearning_Click);
            // 
            // buttonSaveChanges
            // 
            this.buttonSaveChanges.Location = new System.Drawing.Point(208, 177);
            this.buttonSaveChanges.Name = "buttonSaveChanges";
            this.buttonSaveChanges.Size = new System.Drawing.Size(75, 37);
            this.buttonSaveChanges.TabIndex = 8;
            this.buttonSaveChanges.Text = "Save changes";
            this.buttonSaveChanges.UseVisualStyleBackColor = true;
            this.buttonSaveChanges.Click += new System.EventHandler(this.buttonSaveChanges_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(396, 382);
            this.Controls.Add(this.buttonSaveChanges);
            this.Controls.Add(this.buttonStartLearning);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.numericUpDownClass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelProgress);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxTeach);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTeach)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownClass)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxTeach;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelProgress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numericUpDownClass;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonStartLearning;
        private System.Windows.Forms.Button buttonSaveChanges;
    }
}