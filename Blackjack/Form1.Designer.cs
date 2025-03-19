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
        // Nieuwe controls om de dealer en spelers beter weer te geven
        private Label dealerLabel;
        private Label playersInfoLabel;

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
            this.ClientSize = new System.Drawing.Size(800, 600); // Vergroot het formulier
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

            // NumericUpDown voor aantal spelers
            this.playersNumericUpDown = new NumericUpDown();
            this.playersNumericUpDown.Location = new System.Drawing.Point(50, 350);
            this.playersNumericUpDown.Name = "playersNumericUpDown";
            this.playersNumericUpDown.Size = new System.Drawing.Size(100, 30);
            this.playersNumericUpDown.Minimum = 1;
            this.playersNumericUpDown.Maximum = 7;
            this.playersNumericUpDown.Value = 1;

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

            // 
            // Form1 - Voeg alle controls toe
            // 
            this.Controls.Add(this.shuffleButton);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.dealerLabel);
            this.Controls.Add(this.cardLabel);
            this.Controls.Add(this.playersInfoLabel);
            this.Controls.Add(this.playersNumericUpDown);
            this.Controls.Add(this.playersLabel);
            this.Controls.Add(this.startGameButton);
            this.Controls.Add(this.statusLabel);
        }

        #endregion
    }
}
