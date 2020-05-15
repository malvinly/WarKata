using System;
using System.Collections;
using System.Collections.Generic;

namespace WarCardGame
{
    public class Deck : IEnumerable<Card>
    {
        private static readonly Random Random = new Random();
        private readonly Queue<Card> _deck;

        public Deck()
        {
            List<Card> cards = GenerateCards();

            _deck = Shuffle(cards);
        }

        public Card GetCard()
        {
            return _deck.Dequeue();
        }

        private List<Card> GenerateCards()
        {
            var cards = new List<Card>();

            // Jacks have a value of 11
            // Queens have a value of 12
            // Kings have a value of 13
            // Aces have a value of 14. In the game of War, Aces are treated as the high card.
            for (int i = 2; i <= 14; i++)
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    cards.Add(new Card(i, suit));
                }
            }

            return cards;
        }

        private Queue<Card> Shuffle(List<Card> cards)
        {
            var shuffledCards = new Queue<Card>();

            // Randomly pick a card from our unshuffled deck and add it to our shuffled deck.
            while (cards.Count > 0)
            {
                int index = Random.Next(0, cards.Count);
                shuffledCards.Enqueue(cards[index]);
                cards.RemoveAt(index);
            }

            return shuffledCards;
        }

        public IEnumerator<Card> GetEnumerator()
        {
            return _deck.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _deck.GetEnumerator();
        }
    }
}