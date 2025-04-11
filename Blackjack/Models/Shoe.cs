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
        public double CurrentDeckRemainingPercentage => currentDeck.RemainingPercentage;

        public int TotalDecks { get; }
        public int RemainingDecks => decks.Count + (currentDeck.RemainingCards > 0 ? 1 : 0);
        public int RemainingCards => currentDeck.RemainingCards + decks.Count * 52;
        public double RemainingPercentage => (double)RemainingCards / initialTotalCards * 100;

        // Voeg een eigenschap toe om te controleren of het laatste deck wordt gebruikt
        public bool IsLastDeck => decks.Count == 0;

        // Voeg een eigenschap toe om te controleren of het laatste deck minder dan 25% kaarten heeft
        public bool IsLastDeckLowOnCards => IsLastDeck && currentDeck.IsLowOnCards();

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
            // Check if the current deck is low on cards (25% or less cards remaining)
            if (currentDeck.IsLowOnCards())
            {
                if (decks.Count > 0)
                {
                    // If we have more decks available, switch to a new deck
                    Console.WriteLine($"Deck at {currentDeck.RemainingPercentage:F1}% - switching to a new deck");
                    currentDeck = decks[0];
                    decks.RemoveAt(0);
                }
                else
                {
                    // Als het het laatste deck is en het heeft minder dan 25% kaarten,
                    // dan blijven we het gebruiken maar de UI moet de waarschuwing tonen
                    if (currentDeck.RemainingCards == 0)
                    {
                        throw new InvalidOperationException("Geen kaarten meer in de shoe.");
                    }

                    // We geven geen waarschuwing hier, omdat we dat in de UI willen doen
                }
            }

            // Deal a card from the current deck
            Card dealtCard = currentDeck.DealCard();
            totalCards--;

            return dealtCard;
        }

        public string GetStatus()
        {
            return $"Shoe status: {RemainingCards}/{initialTotalCards} kaarten over ({RemainingPercentage:F1}%), " +
                   $"nog {RemainingDecks} van {TotalDecks} decks over";
        }
    }
}
