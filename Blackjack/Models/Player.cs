using System.Collections.Generic;

namespace Blackjack.Models
{
    public class Player
    {
        public string Name { get; }
        public List<Card> Cards { get; }
        public bool IsBusted => CalculateHandValue() > 21;
        public bool IsStanding { get; private set; }

        public Player(string name)
        {
            Name = name;
            Cards = new List<Card>();
        }
        public void Reset()
        {
            ClearCards();
            IsStanding = false;
        }

        public void Stand()
        {
            IsStanding = true;
        }

        public Card Hit(Deck deck)
        {
            Card card = deck.DealCard();
            AddCard(card);
            return card;
        }
        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public void ClearCards()
        {
            Cards.Clear();
        }

        public int CalculateHandValue()
        {
            int total = 0;
            int acesCount = 0;

            foreach (var card in Cards)
            {
                total += card.Value;
                if (card.Rank == "A")
                    acesCount++;
            }

            // Azen kunnen 1 of 11 punten waard zijn
            // Als de totale waarde > 21 is en we hebben azen, dan maken we de waarde van een aas 1 in plaats van 11
            while (total > 21 && acesCount > 0)
            {
                total -= 10; // Verlaag de totale waarde met 10 (11 - 1)
                acesCount--;
            }

            return total;
        }

        public bool HasBlackjack()
        {
            return Cards.Count == 2 && CalculateHandValue() == 21;
        }

        // Geeft een string representatie van de kaarten in de hand
        public string GetCardsString()
        {
            if (Cards.Count == 0)
                return "Geen kaarten";

            return string.Join(", ", Cards);
        }
        // Geeft de speler de optie om te hitten of te passen

    }
}
