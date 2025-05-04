using System.ComponentModel;
using System.Windows.Forms;

namespace Blackjack
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.Windows.Forms.Label scoreLabel;
        private IContainer components = null;
        private Button shuffleButton;
        private Button drawButton;
        private Label cardLabel;
        private NumericUpDown playersNumericUpDown;
        private Label playersLabel;
        private Button startGameButton;
        private Label statusLabel;
        private Label dealerLabel;
        private Label playersInfoLabel;
        private Button hitButton;
        private Button standButton;
        private NumericUpDown decksNumericUpDown;
        private Label decksLabel;
        private Button dealerHitButton;
        private Button splitButton;
        private Button doubleDownButton;
        private Label dealerScoreLabel;

        //private Button addPlayerButton;
        //private Button removePlayerButton;


        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            shuffleButton = new Button();
            drawButton = new Button();
            cardLabel = new Label();
            dealerLabel = new Label();
            playersInfoLabel = new Label();
            playersNumericUpDown = new NumericUpDown();
            decksNumericUpDown = new NumericUpDown();
            decksLabel = new Label();
            playersLabel = new Label();
            startGameButton = new Button();
            statusLabel = new Label();
            hitButton = new Button();
            standButton = new Button();
            dealerHitButton = new Button();
            splitButton = new Button();
            doubleDownButton = new Button();
            dealerScoreLabel = new Label();
            scoreLabel = new Label();
            ((ISupportInitialize)playersNumericUpDown).BeginInit();
            ((ISupportInitialize)decksNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // shuffleButton
            // 
            shuffleButton.Location = new Point(50, 50);
            shuffleButton.Name = "shuffleButton";
            shuffleButton.Size = new Size(100, 30);
            shuffleButton.TabIndex = 0;
            shuffleButton.Text = "Shuffle";
            shuffleButton.UseVisualStyleBackColor = true;
            shuffleButton.Click += shuffleButton_Click;
            // 
            // drawButton
            // 
            drawButton.Location = new Point(170, 50);
            drawButton.Name = "drawButton";
            drawButton.Size = new Size(100, 30);
            drawButton.TabIndex = 1;
            drawButton.Text = "Deal Card";
            drawButton.UseVisualStyleBackColor = true;
            drawButton.Click += drawButton_Click;
            // 
            // cardLabel
            // 
            cardLabel.Location = new Point(50, 140);
            cardLabel.Name = "cardLabel";
            cardLabel.Size = new Size(700, 150);
            cardLabel.TabIndex = 3;
            cardLabel.Text = "Kaarten:";
            // 
            // dealerLabel
            // 
            dealerLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dealerLabel.Location = new Point(50, 100);
            dealerLabel.Name = "dealerLabel";
            dealerLabel.Size = new Size(700, 30);
            dealerLabel.TabIndex = 2;
            dealerLabel.Text = "Dealer:";
            // 
            // playersInfoLabel
            // 
            playersInfoLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            playersInfoLabel.Location = new Point(50, 300);
            playersInfoLabel.Name = "playersInfoLabel";
            playersInfoLabel.Size = new Size(700, 30);
            playersInfoLabel.TabIndex = 4;
            playersInfoLabel.Text = "Spelers:";
            // 
            // playersNumericUpDown
            // 
            playersNumericUpDown.Location = new Point(50, 350);
            playersNumericUpDown.Maximum = new decimal(new int[] { 4, 0, 0, 0 });
            playersNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            playersNumericUpDown.Name = "playersNumericUpDown";
            playersNumericUpDown.Size = new Size(100, 23);
            playersNumericUpDown.TabIndex = 5;
            playersNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // decksNumericUpDown
            // 
            decksNumericUpDown.Location = new Point(370, 350);
            decksNumericUpDown.Maximum = new decimal(new int[] { 8, 0, 0, 0 });
            decksNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            decksNumericUpDown.Name = "decksNumericUpDown";
            decksNumericUpDown.Size = new Size(100, 23);
            decksNumericUpDown.TabIndex = 12;
            decksNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // decksLabel
            // 
            decksLabel.Location = new Point(480, 350);
            decksLabel.Name = "decksLabel";
            decksLabel.Size = new Size(200, 30);
            decksLabel.TabIndex = 13;
            decksLabel.Text = "Aantal decks in shoe:";
            // 
            // playersLabel
            // 
            playersLabel.Location = new Point(160, 350);
            playersLabel.Name = "playersLabel";
            playersLabel.Size = new Size(200, 30);
            playersLabel.TabIndex = 6;
            playersLabel.Text = "Stel aantal spelers in:";
            // 
            // startGameButton
            // 
            startGameButton.Location = new Point(50, 400);
            startGameButton.Name = "startGameButton";
            startGameButton.Size = new Size(150, 30);
            startGameButton.TabIndex = 7;
            startGameButton.Text = "Start Spel (Dealer)";
            startGameButton.UseVisualStyleBackColor = true;
            startGameButton.Click += startGameButton_Click;
            // 
            // statusLabel
            // 
            statusLabel.BorderStyle = BorderStyle.FixedSingle;
            statusLabel.Location = new Point(50, 450);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(700, 60);
            statusLabel.TabIndex = 8;
            statusLabel.Text = "Stel het aantal spelers en de hoeveelheid decks in de shoe in en laat de dealer het spel starten. Klik op shuffle om het aantal decks in de shoe te bevestigen";
            statusLabel.Click += statusLabel_Click;
            // 
            // hitButton
            // 
            hitButton.Enabled = false;
            hitButton.Location = new Point(290, 50);
            hitButton.Name = "hitButton";
            hitButton.Size = new Size(100, 30);
            hitButton.TabIndex = 9;
            hitButton.Text = "Hit";
            hitButton.UseVisualStyleBackColor = true;
            hitButton.Click += hitButton_Click;
            // 
            // standButton
            // 
            standButton.Enabled = false;
            standButton.Location = new Point(410, 50);
            standButton.Name = "standButton";
            standButton.Size = new Size(100, 30);
            standButton.TabIndex = 10;
            standButton.Text = "Stand";
            standButton.UseVisualStyleBackColor = true;
            standButton.Click += standButton_Click;
            // 
            // dealerHitButton
            // 
            dealerHitButton.Enabled = false;
            dealerHitButton.Location = new Point(530, 50);
            dealerHitButton.Name = "dealerHitButton";
            dealerHitButton.Size = new Size(100, 30);
            dealerHitButton.TabIndex = 11;
            dealerHitButton.Text = "Hit (Dealer)";
            dealerHitButton.UseVisualStyleBackColor = true;
            dealerHitButton.Click += dealerHitButton_Click;
            // 
            // splitButton
            // 
            splitButton.Enabled = false;
            splitButton.Location = new Point(650, 50);
            splitButton.Name = "splitButton";
            splitButton.Size = new Size(100, 30);
            splitButton.TabIndex = 14;
            splitButton.Text = "Split";
            splitButton.UseVisualStyleBackColor = true;
            splitButton.Click += splitButton_Click;
            // 
            // doubleDownButton
            // 
            doubleDownButton.Enabled = false;
            doubleDownButton.Location = new Point(770, 50);
            doubleDownButton.Name = "doubleDownButton";
            doubleDownButton.Size = new Size(100, 30);
            doubleDownButton.TabIndex = 15;
            doubleDownButton.Text = "Double Down";
            doubleDownButton.UseVisualStyleBackColor = true;
            doubleDownButton.Click += doubleDownButton_Click;
            // 
            // dealerScoreLabel
            // 
            dealerScoreLabel.Location = new Point(0, 0);
            dealerScoreLabel.Name = "dealerScoreLabel";
            dealerScoreLabel.Size = new Size(100, 23);
            dealerScoreLabel.TabIndex = 1;
            // 
            // scoreLabel
            // 
            scoreLabel.AutoSize = true;
            scoreLabel.Location = new Point(222, 408);
            scoreLabel.Name = "scoreLabel";
            scoreLabel.Size = new Size(48, 15);
            scoreLabel.TabIndex = 0;
            scoreLabel.Text = "Score: 0";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 800);
            Controls.Add(scoreLabel);
            Controls.Add(dealerScoreLabel);
            Controls.Add(shuffleButton);
            Controls.Add(drawButton);
            Controls.Add(dealerLabel);
            Controls.Add(cardLabel);
            Controls.Add(playersInfoLabel);
            Controls.Add(playersNumericUpDown);
            Controls.Add(playersLabel);
            Controls.Add(startGameButton);
            Controls.Add(statusLabel);
            Controls.Add(hitButton);
            Controls.Add(standButton);
            Controls.Add(dealerHitButton);
            Controls.Add(decksNumericUpDown);
            Controls.Add(decksLabel);
            Controls.Add(splitButton);
            Controls.Add(doubleDownButton);
            Name = "Form1";
            Text = "Blackjack Game";
            ((ISupportInitialize)playersNumericUpDown).EndInit();
            ((ISupportInitialize)decksNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
