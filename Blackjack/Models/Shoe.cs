// In Blackjack/Models/Shoe.cs
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Blackjack.Models
{
    public class Shoe
    {
        private readonly List<Deck> decks;
        private Deck currentDeck;
        private int totalCards;
        private int initialTotalCards;

        public int TotalDecks { get; }
        public int RemainingDecks => decks.Count + (currentDeck.RemainingCards > 0 ? 1 : 0);
        public int RemainingCards => currentDeck.RemainingCards + decks.Count * 52;
        public double RemainingPercentage => (double)RemainingCards / initialTotalCards * 100;

        public Shoe(int numberOfDecks)
        {
            if (numberOfDecks <= 0)
                throw new ArgumentException("Aantal decks moet groter zijn dan nul.");

            TotalDecks = numberOfDecks;
            decks = new List<Deck>();

            for (int i = 0; i < numberOfDecks; i++)
            {
                var deck = new Deck();
                deck.Shuffle();
                decks.Add(deck);
            }

            currentDeck = decks[0];
            decks.RemoveAt(0);

            totalCards = (numberOfDecks * 52);
            initialTotalCards = totalCards;
        }

        public Card DealCard()
        {
            // Check of we een nieuw deck nodig hebben
            if (currentDeck.RemainingCards == 0)
            {
                if (decks.Count > 0)
                {
                    currentDeck = decks[0];
                    decks.RemoveAt(0);
                }
                else
                {
                    throw new InvalidOperationException("Geen decks meer in de shoe.");
                }
            }

            // Check of het huidige deck bijna leeg is (25% of minder)
            if (currentDeck.IsLowOnCards() && decks.Count > 0)
            {
                MessageBox.Show($"Waarschuwing: Nog maar {currentDeck.RemainingPercentage:F1}% van het huidige deck over!",
                               "Deck bijna leeg", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            totalCards--;
            return currentDeck.DealCard();
        }

        public string GetStatus()
        {
            return $"Shoe status: {RemainingCards}/{initialTotalCards} kaarten over ({RemainingPercentage:F1}%), " +
                   $"nog {RemainingDecks} van {TotalDecks} decks over";
        }
    }
}
