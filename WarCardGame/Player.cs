using System.Collections.Generic;

namespace WarCardGame
{
    public class Player
    {
        private readonly Queue<Card> _deck = new Queue<Card>();

        public virtual int CardsLeft => _deck.Count;

        public virtual void AddCards(params Card[] cards)
        {
            foreach (Card card in cards)
            {
                _deck.Enqueue(card);
            }
        }

        public virtual Card PlayCard()
        {
            if (_deck.Count > 0)
                return _deck.Dequeue();

            return null;
        }

        public virtual List<Card> PlayWar()
        {
            if (_deck.Count >= 2)
            {
                List<Card> cards = new List<Card>
                {
                    PlayCard(),
                    PlayCard()
                };

                return cards;
            }

            return null;
        }

        public virtual void RemoveAllCards()
        {
            _deck.Clear();
        }
    }
}