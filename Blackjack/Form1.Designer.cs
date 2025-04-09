using System.ComponentModel;
using System.Windows.Forms;

namespace Blackjack
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
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
            this.components = new Container();
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Text = "Blackjack Game";

            this.shuffleButton = new Button();
            this.drawButton = new Button();
            this.cardLabel = new Label();

            // 
            // shuffleButton
            // 
            this.shuffleButton.Location = new System.Drawing.Point(50, 50);
            this.shuffleButton.Name = "shuffleButton";
            this.shuffleButton.Size = new System.Drawing.Size(100, 30);
            this.shuffleButton.Text = "Shuffle";
            this.shuffleButton.UseVisualStyleBackColor = true;
            this.shuffleButton.Click += new System.EventHandler(this.shuffleButton_Click);

            // 
            // drawButton
            // 
            this.drawButton.Location = new System.Drawing.Point(170, 50);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(100, 30);
            this.drawButton.Text = "Deal Card";
            this.drawButton.UseVisualStyleBackColor = true;
            this.drawButton.Click += new System.EventHandler(this.drawButton_Click);

            // Dealer label
            this.dealerLabel = new Label();
            this.dealerLabel.Location = new System.Drawing.Point(50, 100);
            this.dealerLabel.Name = "dealerLabel";
            this.dealerLabel.Size = new System.Drawing.Size(700, 30);
            this.dealerLabel.Text = "Dealer:";
            this.dealerLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            // 
            // cardLabel - Vergroten om meer kaartinformatie weer te geven
            // 
            this.cardLabel.Location = new System.Drawing.Point(50, 140);
            this.cardLabel.Name = "cardLabel";
            this.cardLabel.Size = new System.Drawing.Size(700, 150); // Veel groter maken
            this.cardLabel.Text = "Kaarten:";
            this.cardLabel.AutoSize = false;

            // Players info label
            this.playersInfoLabel = new Label();
            this.playersInfoLabel.Location = new System.Drawing.Point(50, 300);
            this.playersInfoLabel.Name = "playersInfoLabel";
            this.playersInfoLabel.Size = new System.Drawing.Size(700, 30);
            this.playersInfoLabel.Text = "Spelers:";
            this.playersInfoLabel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);

            // Knop om aantal spelers in te stellen
            this.playersNumericUpDown = new NumericUpDown();
            this.playersNumericUpDown.Location = new System.Drawing.Point(50, 350);
            this.playersNumericUpDown.Name = "playersNumericUpDown";
            this.playersNumericUpDown.Size = new System.Drawing.Size(100, 30);
            this.playersNumericUpDown.Minimum = 1;
            this.playersNumericUpDown.Maximum = 4;
            this.playersNumericUpDown.Value = 1;


            this.decksNumericUpDown = new NumericUpDown();
            this.decksNumericUpDown.Location = new System.Drawing.Point(370, 350);
            this.decksNumericUpDown.Name = "decksNumericUpDown";
            this.decksNumericUpDown.Size = new System.Drawing.Size(100, 30);
            this.decksNumericUpDown.Minimum = 1;
            this.decksNumericUpDown.Maximum = 8;
            this.decksNumericUpDown.Value = 1;

            this.decksLabel = new Label();
            this.decksLabel.Location = new System.Drawing.Point(480, 350);
            this.decksLabel.Name = "decksLabel";
            this.decksLabel.Size = new System.Drawing.Size(200, 30);
            this.decksLabel.Text = "Aantal decks in shoe:";

            // Label voor instructie
            this.playersLabel = new Label();
            this.playersLabel.Location = new System.Drawing.Point(160, 350);
            this.playersLabel.Name = "playersLabel";
            this.playersLabel.Size = new System.Drawing.Size(200, 30);
            this.playersLabel.Text = "Stel aantal spelers in:";

            // Dealer start knop
            this.startGameButton = new Button();
            this.startGameButton.Location = new System.Drawing.Point(50, 400);
            this.startGameButton.Name = "startGameButton";
            this.startGameButton.Size = new System.Drawing.Size(150, 30);
            this.startGameButton.Text = "Start Spel (Dealer)";
            this.startGameButton.UseVisualStyleBackColor = true;
            this.startGameButton.Click += new System.EventHandler(this.startGameButton_Click);

            // Status label - Verplaatst naar onderaan en groter gemaakt
            this.statusLabel = new Label();
            this.statusLabel.Location = new System.Drawing.Point(50, 450);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(700, 60); // Groter maken voor meer informatie
            this.statusLabel.Text = "Stel het aantal spelers in en laat de dealer het spel starten.";
            this.statusLabel.AutoSize = false;
            this.statusLabel.BorderStyle = BorderStyle.FixedSingle; // Een rand toevoegen voor zichtbaarheid

            // Hit button
            this.hitButton = new Button();
            this.hitButton.Location = new System.Drawing.Point(290, 50);
            this.hitButton.Name = "hitButton";
            this.hitButton.Size = new System.Drawing.Size(100, 30);
            this.hitButton.Text = "Hit";
            this.hitButton.UseVisualStyleBackColor = true;
            this.hitButton.Enabled = false;
            this.hitButton.Click += new System.EventHandler(this.hitButton_Click);
            // Stand button
            this.standButton = new Button();
            this.standButton.Location = new System.Drawing.Point(410, 50);
            this.standButton.Name = "Standbutton";
            this.standButton.Size = new System.Drawing.Size(100, 30);
            this.standButton.Text = "Stand";
            this.standButton.UseVisualStyleBackColor = true;
            this.standButton.Enabled = false;
            this.standButton.Click += new System.EventHandler(this.standButton_Click);
            // Dealer hit button
            this.dealerHitButton = new Button();
            this.dealerHitButton.Location = new System.Drawing.Point(530, 50);
            this.dealerHitButton.Name = "dealerHitButton";
            this.dealerHitButton.Size = new System.Drawing.Size(100, 30);
            this.dealerHitButton.Text = "Hit (Dealer)";
            this.dealerHitButton.UseVisualStyleBackColor = true;
            this.dealerHitButton.Enabled = false;
            this.dealerHitButton.Click += new System.EventHandler(this.dealerHitButton_Click);


            this.Controls.Add(this.shuffleButton);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.dealerLabel);
            this.Controls.Add(this.cardLabel);
            this.Controls.Add(this.playersInfoLabel);
            this.Controls.Add(this.playersNumericUpDown);
            this.Controls.Add(this.playersLabel);
            this.Controls.Add(this.startGameButton);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.hitButton);
            this.Controls.Add(this.standButton);
            this.Controls.Add(this.dealerHitButton);
            this.Controls.Add(this.decksNumericUpDown);
            this.Controls.Add(this.decksLabel);
        }

        #endregion
    }
}
