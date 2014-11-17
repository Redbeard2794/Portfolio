namespace Joint_project_1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnStartGame = new System.Windows.Forms.Button();
            this.lblStartMessage = new System.Windows.Forms.Label();
            this.picBxGameOver = new System.Windows.Forms.PictureBox();
            this.btnRestart = new System.Windows.Forms.Button();
            this.picBxLives1 = new System.Windows.Forms.PictureBox();
            this.picBxLives2 = new System.Windows.Forms.PictureBox();
            this.picBxLives3 = new System.Windows.Forms.PictureBox();
            this.lblLivesIs = new System.Windows.Forms.Label();
            this.lblScoreIs = new System.Windows.Forms.Label();
            this.lblScore = new System.Windows.Forms.Label();
            this.lblLivesLeft = new System.Windows.Forms.Label();
            this.btnDifficulty = new System.Windows.Forms.Button();
            this.lblTutorial = new System.Windows.Forms.Label();
            this.lblWarning = new System.Windows.Forms.Label();
            this.lblWin = new System.Windows.Forms.Label();
            this.lblDifficulty = new System.Windows.Forms.Label();
            this.lblDiffIs = new System.Windows.Forms.Label();
            this.lblHostagesLostIs = new System.Windows.Forms.Label();
            this.lblHostagesLost = new System.Windows.Forms.Label();
            this.btnRestartGame = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picBxGameOver)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxLives1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxLives2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxLives3)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartGame
            // 
            this.btnStartGame.BackColor = System.Drawing.Color.Lime;
            this.btnStartGame.ForeColor = System.Drawing.Color.Black;
            this.btnStartGame.Location = new System.Drawing.Point(550, 434);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(96, 39);
            this.btnStartGame.TabIndex = 0;
            this.btnStartGame.Text = "Press to start the game";
            this.btnStartGame.UseVisualStyleBackColor = false;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // lblStartMessage
            // 
            this.lblStartMessage.AutoSize = true;
            this.lblStartMessage.BackColor = System.Drawing.Color.White;
            this.lblStartMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblStartMessage.Location = new System.Drawing.Point(12, 367);
            this.lblStartMessage.Name = "lblStartMessage";
            this.lblStartMessage.Size = new System.Drawing.Size(430, 93);
            this.lblStartMessage.TabIndex = 1;
            this.lblStartMessage.Text = resources.GetString("lblStartMessage.Text");
            // 
            // picBxGameOver
            // 
            this.picBxGameOver.Image = ((System.Drawing.Image)(resources.GetObject("picBxGameOver.Image")));
            this.picBxGameOver.Location = new System.Drawing.Point(204, 52);
            this.picBxGameOver.Name = "picBxGameOver";
            this.picBxGameOver.Size = new System.Drawing.Size(253, 103);
            this.picBxGameOver.TabIndex = 2;
            this.picBxGameOver.TabStop = false;
            this.picBxGameOver.Visible = false;
            // 
            // btnRestart
            // 
            this.btnRestart.BackColor = System.Drawing.Color.Lime;
            this.btnRestart.Location = new System.Drawing.Point(294, 87);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(75, 19);
            this.btnRestart.TabIndex = 3;
            this.btnRestart.Text = "Restart";
            this.btnRestart.UseVisualStyleBackColor = false;
            this.btnRestart.Visible = false;
            this.btnRestart.Click += new System.EventHandler(this.btnRestart_Click);
            // 
            // picBxLives1
            // 
            this.picBxLives1.BackColor = System.Drawing.Color.White;
            this.picBxLives1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBxLives1.Image = ((System.Drawing.Image)(resources.GetObject("picBxLives1.Image")));
            this.picBxLives1.Location = new System.Drawing.Point(569, 12);
            this.picBxLives1.Name = "picBxLives1";
            this.picBxLives1.Size = new System.Drawing.Size(10, 13);
            this.picBxLives1.TabIndex = 7;
            this.picBxLives1.TabStop = false;
            // 
            // picBxLives2
            // 
            this.picBxLives2.BackColor = System.Drawing.Color.White;
            this.picBxLives2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBxLives2.Image = ((System.Drawing.Image)(resources.GetObject("picBxLives2.Image")));
            this.picBxLives2.Location = new System.Drawing.Point(585, 12);
            this.picBxLives2.Name = "picBxLives2";
            this.picBxLives2.Size = new System.Drawing.Size(10, 13);
            this.picBxLives2.TabIndex = 8;
            this.picBxLives2.TabStop = false;
            // 
            // picBxLives3
            // 
            this.picBxLives3.BackColor = System.Drawing.Color.White;
            this.picBxLives3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picBxLives3.Image = ((System.Drawing.Image)(resources.GetObject("picBxLives3.Image")));
            this.picBxLives3.Location = new System.Drawing.Point(601, 12);
            this.picBxLives3.Name = "picBxLives3";
            this.picBxLives3.Size = new System.Drawing.Size(11, 13);
            this.picBxLives3.TabIndex = 9;
            this.picBxLives3.TabStop = false;
            // 
            // lblLivesIs
            // 
            this.lblLivesIs.AutoSize = true;
            this.lblLivesIs.BackColor = System.Drawing.Color.White;
            this.lblLivesIs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLivesIs.Location = new System.Drawing.Point(524, 9);
            this.lblLivesIs.Name = "lblLivesIs";
            this.lblLivesIs.Size = new System.Drawing.Size(40, 15);
            this.lblLivesIs.TabIndex = 10;
            this.lblLivesIs.Text = "Lives :";
            // 
            // lblScoreIs
            // 
            this.lblScoreIs.AutoSize = true;
            this.lblScoreIs.BackColor = System.Drawing.Color.White;
            this.lblScoreIs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblScoreIs.Location = new System.Drawing.Point(521, 31);
            this.lblScoreIs.Name = "lblScoreIs";
            this.lblScoreIs.Size = new System.Drawing.Size(43, 15);
            this.lblScoreIs.TabIndex = 11;
            this.lblScoreIs.Text = "Score :";
            // 
            // lblScore
            // 
            this.lblScore.AutoSize = true;
            this.lblScore.BackColor = System.Drawing.Color.White;
            this.lblScore.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblScore.Location = new System.Drawing.Point(566, 31);
            this.lblScore.Name = "lblScore";
            this.lblScore.Size = new System.Drawing.Size(15, 15);
            this.lblScore.TabIndex = 12;
            this.lblScore.Text = "0";
            // 
            // lblLivesLeft
            // 
            this.lblLivesLeft.AutoSize = true;
            this.lblLivesLeft.BackColor = System.Drawing.Color.White;
            this.lblLivesLeft.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblLivesLeft.Location = new System.Drawing.Point(618, 13);
            this.lblLivesLeft.Name = "lblLivesLeft";
            this.lblLivesLeft.Size = new System.Drawing.Size(15, 15);
            this.lblLivesLeft.TabIndex = 13;
            this.lblLivesLeft.Text = "3";
            this.lblLivesLeft.Visible = false;
            // 
            // btnDifficulty
            // 
            this.btnDifficulty.BackColor = System.Drawing.Color.Red;
            this.btnDifficulty.Location = new System.Drawing.Point(425, 434);
            this.btnDifficulty.Name = "btnDifficulty";
            this.btnDifficulty.Size = new System.Drawing.Size(119, 38);
            this.btnDifficulty.TabIndex = 14;
            this.btnDifficulty.Text = "Press to make the game harder";
            this.btnDifficulty.UseVisualStyleBackColor = false;
            this.btnDifficulty.Click += new System.EventHandler(this.btnDifficulty_Click);
            // 
            // lblTutorial
            // 
            this.lblTutorial.AutoSize = true;
            this.lblTutorial.BackColor = System.Drawing.Color.White;
            this.lblTutorial.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblTutorial.Location = new System.Drawing.Point(201, 174);
            this.lblTutorial.Name = "lblTutorial";
            this.lblTutorial.Size = new System.Drawing.Size(178, 132);
            this.lblTutorial.TabIndex = 15;
            this.lblTutorial.Text = resources.GetString("lblTutorial.Text");
            // 
            // lblWarning
            // 
            this.lblWarning.AutoSize = true;
            this.lblWarning.BackColor = System.Drawing.Color.Red;
            this.lblWarning.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWarning.Location = new System.Drawing.Point(425, 416);
            this.lblWarning.Name = "lblWarning";
            this.lblWarning.Size = new System.Drawing.Size(118, 15);
            this.lblWarning.TabIndex = 16;
            this.lblWarning.Text = "What have you done!!!";
            this.lblWarning.Visible = false;
            // 
            // lblWin
            // 
            this.lblWin.AutoSize = true;
            this.lblWin.BackColor = System.Drawing.Color.White;
            this.lblWin.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblWin.Location = new System.Drawing.Point(201, 306);
            this.lblWin.Name = "lblWin";
            this.lblWin.Size = new System.Drawing.Size(238, 28);
            this.lblWin.TabIndex = 17;
            this.lblWin.Text = "That looks like the last of them,\r\nnow move on with the next phase of the mission" +
    ".";
            this.lblWin.Visible = false;
            // 
            // lblDifficulty
            // 
            this.lblDifficulty.AutoSize = true;
            this.lblDifficulty.BackColor = System.Drawing.Color.White;
            this.lblDifficulty.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDifficulty.Location = new System.Drawing.Point(112, 9);
            this.lblDifficulty.Name = "lblDifficulty";
            this.lblDifficulty.Size = new System.Drawing.Size(40, 15);
            this.lblDifficulty.TabIndex = 18;
            this.lblDifficulty.Text = "normal";
            // 
            // lblDiffIs
            // 
            this.lblDiffIs.AutoSize = true;
            this.lblDiffIs.BackColor = System.Drawing.Color.White;
            this.lblDiffIs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDiffIs.Location = new System.Drawing.Point(9, 9);
            this.lblDiffIs.Name = "lblDiffIs";
            this.lblDiffIs.Size = new System.Drawing.Size(90, 15);
            this.lblDiffIs.TabIndex = 19;
            this.lblDiffIs.Text = "Current difficulty :";
            // 
            // lblHostagesLostIs
            // 
            this.lblHostagesLostIs.AutoSize = true;
            this.lblHostagesLostIs.BackColor = System.Drawing.Color.White;
            this.lblHostagesLostIs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHostagesLostIs.Location = new System.Drawing.Point(521, 52);
            this.lblHostagesLostIs.Name = "lblHostagesLostIs";
            this.lblHostagesLostIs.Size = new System.Drawing.Size(79, 15);
            this.lblHostagesLostIs.TabIndex = 20;
            this.lblHostagesLostIs.Text = "Hostages lost :";
            this.lblHostagesLostIs.Visible = false;
            // 
            // lblHostagesLost
            // 
            this.lblHostagesLost.AutoSize = true;
            this.lblHostagesLost.BackColor = System.Drawing.Color.White;
            this.lblHostagesLost.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHostagesLost.Location = new System.Drawing.Point(605, 52);
            this.lblHostagesLost.Name = "lblHostagesLost";
            this.lblHostagesLost.Size = new System.Drawing.Size(15, 15);
            this.lblHostagesLost.TabIndex = 21;
            this.lblHostagesLost.Text = "0";
            this.lblHostagesLost.Visible = false;
            // 
            // btnRestartGame
            // 
            this.btnRestartGame.BackColor = System.Drawing.Color.Lime;
            this.btnRestartGame.Location = new System.Drawing.Point(255, 337);
            this.btnRestartGame.Name = "btnRestartGame";
            this.btnRestartGame.Size = new System.Drawing.Size(100, 23);
            this.btnRestartGame.TabIndex = 22;
            this.btnRestartGame.Text = "Restart the game";
            this.btnRestartGame.UseVisualStyleBackColor = false;
            this.btnRestartGame.Visible = false;
            this.btnRestartGame.Click += new System.EventHandler(this.btnRestartGame_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(658, 485);
            this.Controls.Add(this.btnRestartGame);
            this.Controls.Add(this.lblHostagesLost);
            this.Controls.Add(this.lblHostagesLostIs);
            this.Controls.Add(this.lblDiffIs);
            this.Controls.Add(this.lblDifficulty);
            this.Controls.Add(this.lblWin);
            this.Controls.Add(this.lblWarning);
            this.Controls.Add(this.lblTutorial);
            this.Controls.Add(this.btnDifficulty);
            this.Controls.Add(this.lblLivesLeft);
            this.Controls.Add(this.lblScore);
            this.Controls.Add(this.lblScoreIs);
            this.Controls.Add(this.lblLivesIs);
            this.Controls.Add(this.picBxLives3);
            this.Controls.Add(this.picBxLives2);
            this.Controls.Add(this.picBxLives1);
            this.Controls.Add(this.btnRestart);
            this.Controls.Add(this.picBxGameOver);
            this.Controls.Add(this.lblStartMessage);
            this.Controls.Add(this.btnStartGame);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.picBxGameOver)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxLives1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxLives2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBxLives3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.Label lblStartMessage;
        private System.Windows.Forms.PictureBox picBxGameOver;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.PictureBox picBxLives1;
        private System.Windows.Forms.PictureBox picBxLives2;
        private System.Windows.Forms.PictureBox picBxLives3;
        private System.Windows.Forms.Label lblLivesIs;
        private System.Windows.Forms.Label lblScoreIs;
        private System.Windows.Forms.Label lblScore;
        private System.Windows.Forms.Label lblLivesLeft;
        private System.Windows.Forms.Button btnDifficulty;
        private System.Windows.Forms.Label lblTutorial;
        private System.Windows.Forms.Label lblWarning;
        private System.Windows.Forms.Label lblWin;
        private System.Windows.Forms.Label lblDifficulty;
        private System.Windows.Forms.Label lblDiffIs;
        private System.Windows.Forms.Label lblHostagesLostIs;
        private System.Windows.Forms.Label lblHostagesLost;
        private System.Windows.Forms.Button btnRestartGame;
    }
}

