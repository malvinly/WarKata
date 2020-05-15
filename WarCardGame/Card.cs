using System;

namespace WarCardGame
{
    public class Card
    {
        public int Value { get; }
        public Suit Suit { get; }

        public Card(int value, Suit suit)
        {
            if (value <= 0 || value >= 15)
                throw new ArgumentException("A card must have a value between 1 and 14.", nameof(value));

            // Jacks have a value of 11
            // Queens have a value of 12
            // Kings have a value of 13
            // Aces have a value of 14. In the game of War, Aces are treated as the high card.
            if (value == 1)
                value = 14;

            Value = value;
            Suit = suit;
        }

        public override string ToString()
        {
            string cardValue;

            switch (Value)
            {
                case 11:
                    cardValue = "J";
                    break;
                case 12:
                    cardValue = "Q";
                    break;
                case 13:
                    cardValue = "K";
                    break;
                case 14:
                    cardValue = "A";
                    break;
                default:
                    cardValue = Value.ToString();
                    break;
            }
                
            return $"{cardValue} {(char) Suit}";
        }
    }
}