using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Models
{
    public class Deck
    {
        private List<Card> cards; // Een lijst van kaarten in het deck
        private static readonly string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" }; // De vier soorten kaarten
        private static readonly string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" }; // De waarden van de kaarten

        // Dit is de constructor. Hij maakt een nieuw deck aan door ResetDeck aan te roepen.
        public Deck()
        {
            ResetDeck();
        }

        // Deze methode maakt het deck opnieuw door alle 52 kaarten toe te voegen
        public void ResetDeck()
        {
            cards = new List<Card>();
            foreach (var suit in suits)
            {
                foreach (var rank in ranks)
                {
                    cards.Add(new Card(suit, rank));
                }
            }
        }

        // Deze methode print alle kaarten in het deck naar de console
        public void PrintDeck()
        {
            foreach (var card in cards)
            {
                Console.WriteLine(card);
            }
        }

        // Deze methode schudt het deck met behulp van het Fisher-Yates algoritme
        public void Shuffle()
        {
            Random rng = new Random();
            int n = cards.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]); // Wissel de kaarten om
            }
        }

        // Deze methode deelt een kaart van de bovenkant van het deck en verwijdert deze uit het deck
        public Card DealCard()
        {
            if (cards.Count == 0)
            {
                throw new InvalidOperationException("Geen kaarten meer in het deck!");
            }

            Card dealtCard = cards[0];
            cards.RemoveAt(0);
            return dealtCard;
        }
    }
}

