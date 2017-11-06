namespace _10x10Solver
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
            this.pictureBoxBoard = new System.Windows.Forms.PictureBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonStep = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.buttonWeighting = new System.Windows.Forms.Button();
            this.listBoxWeighting = new System.Windows.Forms.ListBox();
            this.progressBarWeigting = new System.Windows.Forms.ProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxBoard
            // 
            this.pictureBoxBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxBoard.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxBoard.Name = "pictureBoxBoard";
            this.pictureBoxBoard.Size = new System.Drawing.Size(380, 510);
            this.pictureBoxBoard.TabIndex = 0;
            this.pictureBoxBoard.TabStop = false;
            this.pictureBoxBoard.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxBoard_MouseDown);
            this.pictureBoxBoard.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxBoard_MouseMove);
            this.pictureBoxBoard.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxBoard_MouseUp);
            // 
            // buttonRun
            // 
            this.buttonRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRun.Location = new System.Drawing.Point(564, 12);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 1;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonStep
            // 
            this.buttonStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStep.Location = new System.Drawing.Point(564, 41);
            this.buttonStep.Name = "buttonStep";
            this.buttonStep.Size = new System.Drawing.Size(75, 23);
            this.buttonStep.TabIndex = 2;
            this.buttonStep.Text = "Step";
            this.buttonStep.UseVisualStyleBackColor = true;
            this.buttonStep.Click += new System.EventHandler(this.buttonStep_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClear.Location = new System.Drawing.Point(564, 70);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(75, 23);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // buttonWeighting
            // 
            this.buttonWeighting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonWeighting.Location = new System.Drawing.Point(564, 99);
            this.buttonWeighting.Name = "buttonWeighting";
            this.buttonWeighting.Size = new System.Drawing.Size(75, 23);
            this.buttonWeighting.TabIndex = 4;
            this.buttonWeighting.Text = "Analyze";
            this.buttonWeighting.UseVisualStyleBackColor = true;
            this.buttonWeighting.Click += new System.EventHandler(this.buttonWeighting_Click);
            // 
            // listBoxWeighting
            // 
            this.listBoxWeighting.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listBoxWeighting.FormattingEnabled = true;
            this.listBoxWeighting.Location = new System.Drawing.Point(398, 168);
            this.listBoxWeighting.Name = "listBoxWeighting";
            this.listBoxWeighting.Size = new System.Drawing.Size(241, 355);
            this.listBoxWeighting.TabIndex = 5;
            // 
            // progressBarWeigting
            // 
            this.progressBarWeigting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBarWeigting.Location = new System.Drawing.Point(398, 139);
            this.progressBarWeigting.Name = "progressBarWeigting";
            this.progressBarWeigting.Size = new System.Drawing.Size(241, 23);
            this.progressBarWeigting.TabIndex = 6;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 532);
            this.Controls.Add(this.progressBarWeigting);
            this.Controls.Add(this.listBoxWeighting);
            this.Controls.Add(this.buttonWeighting);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonStep);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.pictureBoxBoard);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxBoard;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonStep;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.Button buttonWeighting;
        private System.Windows.Forms.ListBox listBoxWeighting;
        private System.Windows.Forms.ProgressBar progressBarWeigting;
    }
}

