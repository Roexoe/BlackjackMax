using System;
using System.Collections.Generic;

namespace Blackjack.Models
{
    public class Dealer
    {
        private Card? hiddenCard;
        private List<Card> visibleCards;
        private Shoe shoe;
        private int score;

        public int Score
        {
            get => score;
            private set => score = value;
        }

        public Dealer(Shoe shoe)
        {
            this.shoe = shoe;
            score = 0; // Start met een score van 0
            visibleCards = new List<Card>();
        }

        public void AddPoints(int points)
        {
            Score += points;
        }

        public void SubtractPoints(int points)
        {
            Score -= points;
        }

        public void ResetHand()
        {
            hiddenCard = null;
            visibleCards.Clear();
            AddPoints(5); // Correcte reset, geef 5 punten
        }

        public void TakeHiddenCard()
        {
            if (hiddenCard == null)
            {
                hiddenCard = shoe.DealCard();
                AddPoints(10); // Correcte actie, geef 10 punten
            }
            else
            {
                SubtractPoints(5); // Onjuiste actie, trek 5 punten af
                throw new InvalidOperationException("Dealer heeft al een verborgen kaart.");
            }
        }

        public Card TakeVisibleCard()
        {
            var card = shoe.DealCard();
            visibleCards.Add(card);
            AddPoints(10); // Correcte actie, geef 10 punten
            return card;
        }

        public void RevealHiddenCard()
        {
            if (hiddenCard != null)
            {
                visibleCards.Add(hiddenCard);
                hiddenCard = null;
                AddPoints(15); // Correcte actie, geef 15 punten
            }
            else
            {
                SubtractPoints(10); // Onjuiste actie, trek 10 punten af
                throw new InvalidOperationException("Er is geen verborgen kaart om te onthullen.");
            }
        }

        public bool HasBlackjack()
        {
            bool blackjack = CalculateHandValue() == 21 && visibleCards.Count == 2;
            if (blackjack)
                AddPoints(20); // Blackjack, geef 20 punten
            return blackjack;
        }

        public bool MightHaveBlackjack()
        {
            return visibleCards.Count == 1 && (visibleCards[0].Value == 10 || visibleCards[0].Value == 11);
        }

        public bool HiddenCardMakesBlackjack()
        {
            if (hiddenCard == null)
                return false;

            bool blackjack = CalculateHandValue() == 21;
            if (blackjack)
                AddPoints(20); // Blackjack, geef 20 punten
            return blackjack;
        }

        public int CalculateHandValue()
        {
            int total = 0;
            int acesCount = 0;

            foreach (var card in visibleCards)
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

        public string GetVisibleCardsString()
        {
            return string.Join(", ", visibleCards);
        }

        public bool ShouldHit(bool userDecision)
        {
            int handValue = CalculateHandValue();
            if (handValue < 17)
            {
                if (!userDecision) // Als de dealer ervoor kiest om niet te hitten
                {
                    SubtractPoints(10); // Strafpunten voor een onjuiste beslissing
                    return false;
                }
                return true; // Dealer moet hitten
            }
            return false; // Dealer hoeft niet te hitten
        }


        public void DealerTurn()
        {
            AddPoints(10); // Correcte beurt, geef 10 punten
        }

        public bool HasHiddenCard()
        {
            return hiddenCard != null;
        }
    }
}
