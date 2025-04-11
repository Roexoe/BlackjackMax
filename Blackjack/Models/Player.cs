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

        public Card Hit(Shoe shoe)
        {
            Card card = shoe.DealCard();
            AddCard(card);
            return card;
        }
        public bool CanSplit()
        {
            return Cards.Count == 2 && Cards[0].Rank == Cards[1].Rank;
        }

        public Player Split(Shoe shoe)
        {
            if (!CanSplit())
                throw new InvalidOperationException("Splitten is niet mogelijk.");

            // Maak een nieuwe hand met de tweede kaart
            Card secondCard = Cards[1];
            Cards.RemoveAt(1);

            Player newHand = new Player($"{Name} (Split)");
            newHand.AddCard(secondCard);

            // Voeg een nieuwe kaart toe aan beide handen
            AddCard(shoe.DealCard());
            newHand.AddCard(shoe.DealCard());

            return newHand;
        }



        public bool CanDoubleDown()
        {
            return Cards.Count == 2; // Double Down is alleen toegestaan bij de eerste twee kaarten
        }
        public void DoubleDown(Shoe shoe)
        {
            // Voeg één kaart toe
            AddCard(shoe.DealCard());
            // Speler moet automatisch stoppen na Double Down
            IsStanding = true;
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

            while (total > 21 && acesCount > 0)
            {
                total -= 10;
                acesCount--;
            }

            return total;
        }

        public bool HasBlackjack()
        {
            return Cards.Count == 2 && CalculateHandValue() == 21;
        }

        public string GetCardsString()
        {
            if (Cards.Count == 0)
                return "Geen kaarten";

            return string.Join(", ", Cards);
        }
    }
}