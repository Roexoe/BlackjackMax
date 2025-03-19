using Blackjack.Models;
using System;
using System.Windows.Forms;

namespace Blackjack
{
    public partial class Form1 : Form
    {
        private Deck deck;

        public Form1()
        {
            InitializeComponent();
            deck = new Deck();
        }

        private void shuffleButton_Click(object sender, EventArgs e)
        {
            deck.Shuffle();
            cardLabel.Text = "Deck shuffled!";
        }

        private void drawButton_Click(object sender, EventArgs e)
        {
            try
            {
                Card drawnCard = deck.DealCard();
                cardLabel.Text = $"Card: {drawnCard}";
            }
            catch (InvalidOperationException ex)
            {
                cardLabel.Text = ex.Message;
            }
        }
    }
}
