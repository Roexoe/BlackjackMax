using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blackjack.Models
{
    public class Card
    {
        public string Suit { get; }
        public string Rank { get; }
        public int Value { get; } // De numerieke waarde in blackjack

        public Card(string suit, string rank)
        {
            Suit = suit;
            Rank = rank;
            Value = DetermineCardValue(rank);
        }

        private int DetermineCardValue(string rank)
        {
            return rank switch
            {
                "A" => 11, // Aas kan 1 of 11 zijn (logica kan later in de game worden toegevoegd)
                "K" or "Q" or "J" => 10,
                _ => int.Parse(rank)
            };
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit} (Value: {Value})";
        }
    }
}
