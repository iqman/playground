namespace SudokuSolver
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
            this.buttonSolveStep = new System.Windows.Forms.Button();
            this.listBoxStatus = new System.Windows.Forms.ListBox();
            this.textBoxNumberToSolve = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonIsValid = new System.Windows.Forms.Button();
            this.buttonFindFiftyFiefties = new System.Windows.Forms.Button();
            this.buttonClearExclusion = new System.Windows.Forms.Button();
            this.buttonGuess = new System.Windows.Forms.Button();
            this.buttonRevertToBeforeGuess = new System.Windows.Forms.Button();
            this.buttonAutoSolve = new System.Windows.Forms.Button();
            this.buttonAbortAutoSolve = new System.Windows.Forms.Button();
            this.listBoxGuessStack = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.labelNumberOfGuesses = new System.Windows.Forms.Label();
            this.checkBoxAnimateAutoSolve = new System.Windows.Forms.CheckBox();
            this.buttonReset = new System.Windows.Forms.Button();
            this.buttonPerfTest = new System.Windows.Forms.Button();
            this.pictureBoxImage = new System.Windows.Forms.PictureBox();
            this.buttonLoadImage = new System.Windows.Forms.Button();
            this.buttonProcessImage = new System.Windows.Forms.Button();
            this.textBoxAnnotationDetails = new System.Windows.Forms.TextBox();
            this.buttonProcess2 = new System.Windows.Forms.Button();
            this.buttonProcess3 = new System.Windows.Forms.Button();
            this.buttonProcess4 = new System.Windows.Forms.Button();
            this.buttonDenoise = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.buttonCloseGaps = new System.Windows.Forms.Button();
            this.buttonTransform = new System.Windows.Forms.Button();
            this.buttonReload = new System.Windows.Forms.Button();
            this.buttonLoadClipboard = new System.Windows.Forms.Button();
            this.buttonShowCells = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxBoard
            // 
            this.pictureBoxBoard.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxBoard.Name = "pictureBoxBoard";
            this.pictureBoxBoard.Size = new System.Drawing.Size(500, 500);
            this.pictureBoxBoard.TabIndex = 0;
            this.pictureBoxBoard.TabStop = false;
            // 
            // buttonSolveStep
            // 
            this.buttonSolveStep.Location = new System.Drawing.Point(518, 12);
            this.buttonSolveStep.Name = "buttonSolveStep";
            this.buttonSolveStep.Size = new System.Drawing.Size(75, 23);
            this.buttonSolveStep.TabIndex = 1;
            this.buttonSolveStep.Text = "SolveStep";
            this.buttonSolveStep.UseVisualStyleBackColor = true;
            this.buttonSolveStep.Click += new System.EventHandler(this.buttonInit_Click);
            // 
            // listBoxStatus
            // 
            this.listBoxStatus.FormattingEnabled = true;
            this.listBoxStatus.Location = new System.Drawing.Point(518, 41);
            this.listBoxStatus.Name = "listBoxStatus";
            this.listBoxStatus.Size = new System.Drawing.Size(277, 472);
            this.listBoxStatus.TabIndex = 2;
            // 
            // textBoxNumberToSolve
            // 
            this.textBoxNumberToSolve.Location = new System.Drawing.Point(695, 14);
            this.textBoxNumberToSolve.Name = "textBoxNumberToSolve";
            this.textBoxNumberToSolve.Size = new System.Drawing.Size(100, 20);
            this.textBoxNumberToSolve.TabIndex = 3;
            this.textBoxNumberToSolve.Text = "1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(599, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Number To Solve";
            // 
            // buttonIsValid
            // 
            this.buttonIsValid.Location = new System.Drawing.Point(518, 519);
            this.buttonIsValid.Name = "buttonIsValid";
            this.buttonIsValid.Size = new System.Drawing.Size(75, 23);
            this.buttonIsValid.TabIndex = 5;
            this.buttonIsValid.Text = "IsValid";
            this.buttonIsValid.UseVisualStyleBackColor = true;
            this.buttonIsValid.Click += new System.EventHandler(this.buttonIsValid_Click);
            // 
            // buttonFindFiftyFiefties
            // 
            this.buttonFindFiftyFiefties.Location = new System.Drawing.Point(614, 519);
            this.buttonFindFiftyFiefties.Name = "buttonFindFiftyFiefties";
            this.buttonFindFiftyFiefties.Size = new System.Drawing.Size(75, 23);
            this.buttonFindFiftyFiefties.TabIndex = 6;
            this.buttonFindFiftyFiefties.Text = "Find 50/50";
            this.buttonFindFiftyFiefties.UseVisualStyleBackColor = true;
            this.buttonFindFiftyFiefties.Click += new System.EventHandler(this.buttonFindFiftyFiefties_Click);
            // 
            // buttonClearExclusion
            // 
            this.buttonClearExclusion.Location = new System.Drawing.Point(720, 519);
            this.buttonClearExclusion.Name = "buttonClearExclusion";
            this.buttonClearExclusion.Size = new System.Drawing.Size(75, 23);
            this.buttonClearExclusion.TabIndex = 7;
            this.buttonClearExclusion.Text = "Clear Ex";
            this.buttonClearExclusion.UseVisualStyleBackColor = true;
            this.buttonClearExclusion.Click += new System.EventHandler(this.buttonClearExclusion_Click);
            // 
            // buttonGuess
            // 
            this.buttonGuess.Location = new System.Drawing.Point(518, 548);
            this.buttonGuess.Name = "buttonGuess";
            this.buttonGuess.Size = new System.Drawing.Size(75, 23);
            this.buttonGuess.TabIndex = 8;
            this.buttonGuess.Text = "Guess";
            this.buttonGuess.UseVisualStyleBackColor = true;
            this.buttonGuess.Click += new System.EventHandler(this.buttonGuess_Click);
            // 
            // buttonRevertToBeforeGuess
            // 
            this.buttonRevertToBeforeGuess.Location = new System.Drawing.Point(518, 577);
            this.buttonRevertToBeforeGuess.Name = "buttonRevertToBeforeGuess";
            this.buttonRevertToBeforeGuess.Size = new System.Drawing.Size(75, 23);
            this.buttonRevertToBeforeGuess.TabIndex = 9;
            this.buttonRevertToBeforeGuess.Text = "Revert";
            this.buttonRevertToBeforeGuess.UseVisualStyleBackColor = true;
            this.buttonRevertToBeforeGuess.Click += new System.EventHandler(this.buttonRevertToBeforeGuess_Click);
            // 
            // buttonAutoSolve
            // 
            this.buttonAutoSolve.Location = new System.Drawing.Point(13, 519);
            this.buttonAutoSolve.Name = "buttonAutoSolve";
            this.buttonAutoSolve.Size = new System.Drawing.Size(75, 23);
            this.buttonAutoSolve.TabIndex = 10;
            this.buttonAutoSolve.Text = "Autosolve";
            this.buttonAutoSolve.UseVisualStyleBackColor = true;
            this.buttonAutoSolve.Click += new System.EventHandler(this.buttonAutoSolve_Click);
            // 
            // buttonAbortAutoSolve
            // 
            this.buttonAbortAutoSolve.Location = new System.Drawing.Point(13, 577);
            this.buttonAbortAutoSolve.Name = "buttonAbortAutoSolve";
            this.buttonAbortAutoSolve.Size = new System.Drawing.Size(75, 23);
            this.buttonAbortAutoSolve.TabIndex = 11;
            this.buttonAbortAutoSolve.Text = "Abort";
            this.buttonAbortAutoSolve.UseVisualStyleBackColor = true;
            this.buttonAbortAutoSolve.Click += new System.EventHandler(this.buttonAbortAutoSolve_Click);
            // 
            // listBoxGuessStack
            // 
            this.listBoxGuessStack.FormattingEnabled = true;
            this.listBoxGuessStack.Location = new System.Drawing.Point(104, 531);
            this.listBoxGuessStack.Name = "listBoxGuessStack";
            this.listBoxGuessStack.Size = new System.Drawing.Size(146, 43);
            this.listBoxGuessStack.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(101, 515);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Number of guesses:";
            // 
            // labelNumberOfGuesses
            // 
            this.labelNumberOfGuesses.AutoSize = true;
            this.labelNumberOfGuesses.Location = new System.Drawing.Point(205, 515);
            this.labelNumberOfGuesses.Name = "labelNumberOfGuesses";
            this.labelNumberOfGuesses.Size = new System.Drawing.Size(13, 13);
            this.labelNumberOfGuesses.TabIndex = 14;
            this.labelNumberOfGuesses.Text = "0";
            // 
            // checkBoxAnimateAutoSolve
            // 
            this.checkBoxAnimateAutoSolve.AutoSize = true;
            this.checkBoxAnimateAutoSolve.Checked = true;
            this.checkBoxAnimateAutoSolve.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAnimateAutoSolve.Location = new System.Drawing.Point(104, 585);
            this.checkBoxAnimateAutoSolve.Name = "checkBoxAnimateAutoSolve";
            this.checkBoxAnimateAutoSolve.Size = new System.Drawing.Size(64, 17);
            this.checkBoxAnimateAutoSolve.TabIndex = 15;
            this.checkBoxAnimateAutoSolve.Text = "Animate";
            this.checkBoxAnimateAutoSolve.UseVisualStyleBackColor = true;
            // 
            // buttonReset
            // 
            this.buttonReset.Location = new System.Drawing.Point(614, 577);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(75, 23);
            this.buttonReset.TabIndex = 16;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = true;
            this.buttonReset.Click += new System.EventHandler(this.buttonReset_Click);
            // 
            // buttonPerfTest
            // 
            this.buttonPerfTest.Location = new System.Drawing.Point(13, 548);
            this.buttonPerfTest.Name = "buttonPerfTest";
            this.buttonPerfTest.Size = new System.Drawing.Size(75, 23);
            this.buttonPerfTest.TabIndex = 17;
            this.buttonPerfTest.Text = "Perf test";
            this.buttonPerfTest.UseVisualStyleBackColor = true;
            this.buttonPerfTest.Click += new System.EventHandler(this.buttonPerfTest_Click);
            // 
            // pictureBoxImage
            // 
            this.pictureBoxImage.Location = new System.Drawing.Point(819, 14);
            this.pictureBoxImage.Name = "pictureBoxImage";
            this.pictureBoxImage.Size = new System.Drawing.Size(500, 500);
            this.pictureBoxImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxImage.TabIndex = 18;
            this.pictureBoxImage.TabStop = false;
            // 
            // buttonLoadImage
            // 
            this.buttonLoadImage.Location = new System.Drawing.Point(819, 520);
            this.buttonLoadImage.Name = "buttonLoadImage";
            this.buttonLoadImage.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadImage.TabIndex = 19;
            this.buttonLoadImage.Text = "Load";
            this.buttonLoadImage.UseVisualStyleBackColor = true;
            this.buttonLoadImage.Click += new System.EventHandler(this.buttonLoadImage_Click);
            // 
            // buttonProcessImage
            // 
            this.buttonProcessImage.Location = new System.Drawing.Point(900, 520);
            this.buttonProcessImage.Name = "buttonProcessImage";
            this.buttonProcessImage.Size = new System.Drawing.Size(75, 23);
            this.buttonProcessImage.TabIndex = 20;
            this.buttonProcessImage.Text = "Process";
            this.buttonProcessImage.UseVisualStyleBackColor = true;
            this.buttonProcessImage.Click += new System.EventHandler(this.buttonProcessImage_Click);
            // 
            // textBoxAnnotationDetails
            // 
            this.textBoxAnnotationDetails.Location = new System.Drawing.Point(1325, 12);
            this.textBoxAnnotationDetails.Multiline = true;
            this.textBoxAnnotationDetails.Name = "textBoxAnnotationDetails";
            this.textBoxAnnotationDetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxAnnotationDetails.Size = new System.Drawing.Size(382, 502);
            this.textBoxAnnotationDetails.TabIndex = 21;
            // 
            // buttonProcess2
            // 
            this.buttonProcess2.Location = new System.Drawing.Point(981, 520);
            this.buttonProcess2.Name = "buttonProcess2";
            this.buttonProcess2.Size = new System.Drawing.Size(75, 23);
            this.buttonProcess2.TabIndex = 22;
            this.buttonProcess2.Text = "Process2";
            this.buttonProcess2.UseVisualStyleBackColor = true;
            this.buttonProcess2.Click += new System.EventHandler(this.buttonProcess2_Click);
            // 
            // buttonProcess3
            // 
            this.buttonProcess3.Location = new System.Drawing.Point(1062, 520);
            this.buttonProcess3.Name = "buttonProcess3";
            this.buttonProcess3.Size = new System.Drawing.Size(75, 23);
            this.buttonProcess3.TabIndex = 23;
            this.buttonProcess3.Text = "FindEdge";
            this.buttonProcess3.UseVisualStyleBackColor = true;
            this.buttonProcess3.Click += new System.EventHandler(this.buttonProcess3_Click);
            // 
            // buttonProcess4
            // 
            this.buttonProcess4.Location = new System.Drawing.Point(1143, 520);
            this.buttonProcess4.Name = "buttonProcess4";
            this.buttonProcess4.Size = new System.Drawing.Size(75, 23);
            this.buttonProcess4.TabIndex = 24;
            this.buttonProcess4.Text = "Cut&Extract";
            this.buttonProcess4.UseVisualStyleBackColor = true;
            this.buttonProcess4.Click += new System.EventHandler(this.buttonProcess4_Click);
            // 
            // buttonDenoise
            // 
            this.buttonDenoise.Location = new System.Drawing.Point(900, 551);
            this.buttonDenoise.Name = "buttonDenoise";
            this.buttonDenoise.Size = new System.Drawing.Size(75, 23);
            this.buttonDenoise.TabIndex = 25;
            this.buttonDenoise.Text = "Denoise";
            this.buttonDenoise.UseVisualStyleBackColor = true;
            this.buttonDenoise.Click += new System.EventHandler(this.buttonDenoise_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(981, 551);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "HL Edge";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // buttonCloseGaps
            // 
            this.buttonCloseGaps.Location = new System.Drawing.Point(981, 581);
            this.buttonCloseGaps.Name = "buttonCloseGaps";
            this.buttonCloseGaps.Size = new System.Drawing.Size(75, 23);
            this.buttonCloseGaps.TabIndex = 27;
            this.buttonCloseGaps.Text = "Close Gaps";
            this.buttonCloseGaps.UseVisualStyleBackColor = true;
            this.buttonCloseGaps.Click += new System.EventHandler(this.buttonCloseGaps_Click);
            // 
            // buttonTransform
            // 
            this.buttonTransform.Location = new System.Drawing.Point(1110, 551);
            this.buttonTransform.Name = "buttonTransform";
            this.buttonTransform.Size = new System.Drawing.Size(75, 23);
            this.buttonTransform.TabIndex = 28;
            this.buttonTransform.Text = "Transform";
            this.buttonTransform.UseVisualStyleBackColor = true;
            this.buttonTransform.Click += new System.EventHandler(this.buttonTransform_Click);
            // 
            // buttonReload
            // 
            this.buttonReload.Location = new System.Drawing.Point(819, 551);
            this.buttonReload.Name = "buttonReload";
            this.buttonReload.Size = new System.Drawing.Size(75, 23);
            this.buttonReload.TabIndex = 29;
            this.buttonReload.Text = "Reload";
            this.buttonReload.UseVisualStyleBackColor = true;
            this.buttonReload.Click += new System.EventHandler(this.buttonReload_Click);
            // 
            // buttonLoadClipboard
            // 
            this.buttonLoadClipboard.Location = new System.Drawing.Point(819, 580);
            this.buttonLoadClipboard.Name = "buttonLoadClipboard";
            this.buttonLoadClipboard.Size = new System.Drawing.Size(75, 23);
            this.buttonLoadClipboard.TabIndex = 30;
            this.buttonLoadClipboard.Text = "Load Clip";
            this.buttonLoadClipboard.UseVisualStyleBackColor = true;
            this.buttonLoadClipboard.Click += new System.EventHandler(this.buttonLoadClipboard_Click);
            // 
            // buttonShowCells
            // 
            this.buttonShowCells.Location = new System.Drawing.Point(1052, 577);
            this.buttonShowCells.Name = "buttonShowCells";
            this.buttonShowCells.Size = new System.Drawing.Size(75, 23);
            this.buttonShowCells.TabIndex = 31;
            this.buttonShowCells.Text = "ShowCells";
            this.buttonShowCells.UseVisualStyleBackColor = true;
            this.buttonShowCells.Click += new System.EventHandler(this.buttonShowCells_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1719, 614);
            this.Controls.Add(this.buttonShowCells);
            this.Controls.Add(this.buttonLoadClipboard);
            this.Controls.Add(this.buttonReload);
            this.Controls.Add(this.buttonTransform);
            this.Controls.Add(this.buttonCloseGaps);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonDenoise);
            this.Controls.Add(this.buttonProcess4);
            this.Controls.Add(this.buttonProcess3);
            this.Controls.Add(this.buttonProcess2);
            this.Controls.Add(this.textBoxAnnotationDetails);
            this.Controls.Add(this.buttonProcessImage);
            this.Controls.Add(this.buttonLoadImage);
            this.Controls.Add(this.pictureBoxImage);
            this.Controls.Add(this.buttonPerfTest);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.checkBoxAnimateAutoSolve);
            this.Controls.Add(this.labelNumberOfGuesses);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.listBoxGuessStack);
            this.Controls.Add(this.buttonAbortAutoSolve);
            this.Controls.Add(this.buttonAutoSolve);
            this.Controls.Add(this.buttonRevertToBeforeGuess);
            this.Controls.Add(this.buttonGuess);
            this.Controls.Add(this.buttonClearExclusion);
            this.Controls.Add(this.buttonFindFiftyFiefties);
            this.Controls.Add(this.buttonIsValid);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxNumberToSolve);
            this.Controls.Add(this.listBoxStatus);
            this.Controls.Add(this.buttonSolveStep);
            this.Controls.Add(this.pictureBoxBoard);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxBoard;
        private System.Windows.Forms.Button buttonSolveStep;
        private System.Windows.Forms.ListBox listBoxStatus;
        private System.Windows.Forms.TextBox textBoxNumberToSolve;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonIsValid;
        private System.Windows.Forms.Button buttonFindFiftyFiefties;
        private System.Windows.Forms.Button buttonClearExclusion;
        private System.Windows.Forms.Button buttonGuess;
        private System.Windows.Forms.Button buttonRevertToBeforeGuess;
        private System.Windows.Forms.Button buttonAutoSolve;
        private System.Windows.Forms.Button buttonAbortAutoSolve;
        private System.Windows.Forms.ListBox listBoxGuessStack;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelNumberOfGuesses;
        private System.Windows.Forms.CheckBox checkBoxAnimateAutoSolve;
        private System.Windows.Forms.Button buttonReset;
        private System.Windows.Forms.Button buttonPerfTest;
        private System.Windows.Forms.PictureBox pictureBoxImage;
        private System.Windows.Forms.Button buttonLoadImage;
        private System.Windows.Forms.Button buttonProcessImage;
        private System.Windows.Forms.TextBox textBoxAnnotationDetails;
        private System.Windows.Forms.Button buttonProcess2;
        private System.Windows.Forms.Button buttonProcess3;
        private System.Windows.Forms.Button buttonProcess4;
        private System.Windows.Forms.Button buttonDenoise;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonCloseGaps;
        private System.Windows.Forms.Button buttonTransform;
        private System.Windows.Forms.Button buttonReload;
        private System.Windows.Forms.Button buttonLoadClipboard;
        private System.Windows.Forms.Button buttonShowCells;
    }
}

