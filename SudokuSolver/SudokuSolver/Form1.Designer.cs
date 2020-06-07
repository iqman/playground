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
            this.buttonInit = new System.Windows.Forms.Button();
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
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard)).BeginInit();
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
            // buttonInit
            // 
            this.buttonInit.Location = new System.Drawing.Point(518, 12);
            this.buttonInit.Name = "buttonInit";
            this.buttonInit.Size = new System.Drawing.Size(75, 23);
            this.buttonInit.TabIndex = 1;
            this.buttonInit.Text = "SolveStep";
            this.buttonInit.UseVisualStyleBackColor = true;
            this.buttonInit.Click += new System.EventHandler(this.buttonInit_Click);
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
            this.buttonAutoSolve.Location = new System.Drawing.Point(13, 535);
            this.buttonAutoSolve.Name = "buttonAutoSolve";
            this.buttonAutoSolve.Size = new System.Drawing.Size(75, 23);
            this.buttonAutoSolve.TabIndex = 10;
            this.buttonAutoSolve.Text = "Autosolve";
            this.buttonAutoSolve.UseVisualStyleBackColor = true;
            this.buttonAutoSolve.Click += new System.EventHandler(this.buttonAutoSolve_Click);
            // 
            // buttonAbortAutoSolve
            // 
            this.buttonAbortAutoSolve.Location = new System.Drawing.Point(13, 564);
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
            this.listBoxGuessStack.Size = new System.Drawing.Size(146, 69);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(817, 614);
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
            this.Controls.Add(this.buttonInit);
            this.Controls.Add(this.pictureBoxBoard);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBoard)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxBoard;
        private System.Windows.Forms.Button buttonInit;
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
    }
}

