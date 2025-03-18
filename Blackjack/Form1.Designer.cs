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
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Text = "Form1";

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
            this.drawButton.Location = new System.Drawing.Point(50, 100);
            this.drawButton.Name = "drawButton";
            this.drawButton.Size = new System.Drawing.Size(100, 30);
            this.drawButton.Text = "Draw Card";
            this.drawButton.UseVisualStyleBackColor = true;
            this.drawButton.Click += new System.EventHandler(this.drawButton_Click);

            // 
            // cardLabel
            // 
            this.cardLabel.Location = new System.Drawing.Point(50, 150);
            this.cardLabel.Name = "cardLabel";
            this.cardLabel.Size = new System.Drawing.Size(200, 30);
            this.cardLabel.Text = "Card: ";

            // 
            // Form1
            // 
            this.Controls.Add(this.shuffleButton);
            this.Controls.Add(this.drawButton);
            this.Controls.Add(this.cardLabel);
        }

        #endregion
    }
}
