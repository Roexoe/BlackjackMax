using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Models
{
    public class Deck
    {
        private List<Card> cards;
        private static readonly string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
        private static readonly string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };

        private const int FullDeckSize = 52;

        public int RemainingCards => cards.Count;
        public double RemainingPercentage => (double)cards.Count / FullDeckSize * 100;

        public Deck()
        {
            ResetDeck();
        }

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

        public void PrintDeck()
        {
            foreach (var card in cards)
            {
                Console.WriteLine(card);
            }
        }

        public void Shuffle()
        {
            Random rng = new Random();
            int n = cards.Count;

            for (int i = n - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (cards[i], cards[j]) = (cards[j], cards[i]);
            }
        }

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

        public bool IsLowOnCards()
        {
            return RemainingPercentage <= 25.0;
        }
    }
}