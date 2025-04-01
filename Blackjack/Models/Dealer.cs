using System;
using System.Collections.Generic;

namespace Blackjack.Models
{
    public class Dealer
    {
        private Card? hiddenCard;
        private List<Card> visibleCards;
        private Deck deck;

        public Dealer(Deck deck)
        {
            this.deck = deck;
            visibleCards = new List<Card>();
            ResetHand();
        }

        // Reset de hand van de dealer
        public void ResetHand()
        {
            hiddenCard = null;
            visibleCards.Clear();
        }

        // Pakt de eerste (verborgen) kaart voor de dealer
        public void TakeHiddenCard()
        {
            hiddenCard = deck.DealCard();
        }

        // Pakt een zichtbare kaart voor de dealer
        public Card TakeVisibleCard()
        {
            Card card = deck.DealCard();
            visibleCards.Add(card);
            return card;
        }

        // Maakt de verborgen kaart zichtbaar
        public void RevealHiddenCard()
        {
            if (hiddenCard == null)
                throw new InvalidOperationException("Er is geen verborgen kaart!");

            visibleCards.Insert(0, hiddenCard);
            hiddenCard = null;
        }

        // Controleert of de dealer blackjack heeft (21 punten met 2 kaarten)
        public bool HasBlackjack()
        {
            if (hiddenCard == null || visibleCards.Count != 1)
                return false;

            return CalculateHandValue() == 21;
        }

        // Controleert of de dealer potentieel blackjack kan hebben (10/face of Aas als zichtbare kaart)
        public bool MightHaveBlackjack()
        {
            if (visibleCards.Count != 1)
                return false;

            // Als de zichtbare kaart een 10, J, Q, K (waarde 10) of een Aas is
            return visibleCards[0].Value == 10 || visibleCards[0].Value == 11;
        }

        // Controleert of de verborgen kaart samen met de zichtbare kaart blackjack maakt
        public bool HiddenCardMakesBlackjack()
        {
            if (hiddenCard == null || visibleCards.Count != 1)
                return false;

            return (hiddenCard.Value + visibleCards[0].Value) == 21;
        }

        // Berekent de waarde van de hand van de dealer
        public int CalculateHandValue()
        {
            int total = 0;
            int acesCount = 0;

            // Tel de waarde van de verborgen kaart
            if (hiddenCard != null)
            {
                total += hiddenCard.Value;
                if (hiddenCard.Rank == "A")
                    acesCount++;
            }

            // Tel de waarde van de zichtbare kaarten
            foreach (var card in visibleCards)
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

        // Geeft een string representatie van de zichtbare kaarten
        public string GetVisibleCardsString()
        {
            if (visibleCards.Count == 0)
                return "Geen zichtbare kaarten";

            return string.Join(", ", visibleCards);
        }

        // Dealer pakt tot hij 17 of hoger heeft.
        public bool ShouldHit()
        {
            return CalculateHandValue() < 17;
        }

        public void DealerTurn()
        {
            // Make sure to call RevealHiddenCard if there is a hidden card
            if (hiddenCard != null)
            {
                RevealHiddenCard();  // Use this method instead of directly manipulating the collections
            }

            // Blijf kaarten trekken tot de dealer 17 >= heeft
            while (ShouldHit())
            {
                TakeVisibleCard();
            }
        }
        // Add this new method to check if the dealer has a hidden card
        public bool HasHiddenCard()
        {
            return hiddenCard != null;
        }
    }
}
