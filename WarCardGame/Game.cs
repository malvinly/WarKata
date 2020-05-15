using System;
using System.Collections.Generic;
using WarCardGame.Events;

namespace WarCardGame
{
    public class Game
    {
        public event EventHandler<StartEventArgs> OnTurnStart;
        public event EventHandler<FinishEventArgs> OnTurnFinish;
        public event EventHandler<WarStartEventArgs> OnWarStart;
        public event EventHandler<StartEventArgs> OnWarDealt;
        public event EventHandler<NotEnoughCardsEventArgs> OnWarNotEnoughCards;
        public event EventHandler<WarFinishEventArgs> OnWarFinish;
        public event EventHandler<FinishEventArgs> OnGameFinish;

        private readonly Queue<Card> _player1 = new Queue<Card>();
        private readonly Queue<Card> _player2 = new Queue<Card>();
        private static readonly Random Random = new Random();

        public int Player1CardCount => _player1.Count;
        public int Player2CardCount => _player2.Count;

        public Game()
        {
            // Randomly pick who gets the first card.
            var dealerTarget = Random.Next(2) == 0 ? _player1 : _player2;

            var deck = new Deck();

            foreach (Card card in deck)
            {
                dealerTarget.Enqueue(card);

                // Next card dealt will go to the other person.
                dealerTarget = dealerTarget == _player1 ? _player2 : _player1;
            }
        }

        public void Play()
        {
            int turnCount = 0;

            // As long as someone still has cards in their deck, the game will continue.
            // I also set a limit of 10,000 turns because some games will go on forever.
            while (_player1.Count > 0 && _player2.Count > 0 && turnCount < 10000)
            {
                turnCount++;
                
                Card player1Card = _player1.Dequeue();
                Card player2Card = _player2.Dequeue();

                OnTurnStart?.Invoke(this, new StartEventArgs(turnCount, player1Card, player2Card));

                if (player1Card.Value > player2Card.Value)
                {
                    _player1.Enqueue(player1Card);
                    _player1.Enqueue(player2Card);

                    OnTurnFinish?.Invoke(this, new FinishEventArgs(turnCount, "Player 1"));
                }
                else if (player2Card.Value > player1Card.Value)
                {
                    _player2.Enqueue(player2Card);
                    _player2.Enqueue(player1Card);

                    OnTurnFinish?.Invoke(this, new FinishEventArgs(turnCount, "Player 2"));
                }
                else
                {
                    // Both cards have the same value, so they'll go to war.
                    War(turnCount, new List<Card> { player1Card }, new List<Card> { player2Card });
                }
            }

            if (turnCount >= 10000)
            {
                OnGameFinish?.Invoke(this, new FinishEventArgs(turnCount, "Tie"));
            }
            else
            {
                var winner = _player1.Count == 0 ? "Player 2" : "Player 1";
                OnGameFinish?.Invoke(this, new FinishEventArgs(turnCount, winner));
            }
        }

        private void War(int turnCount, List<Card> player1PendingCards, List<Card> player2PendingCards)
        {
            OnWarStart?.Invoke(this, new WarStartEventArgs(turnCount));

            // Players must have enough cards to place 1 card face down and 1 card face up during war.
            if (_player1.Count < 2)
            {
                 _player1.Clear();
                OnWarNotEnoughCards?.Invoke(this, new NotEnoughCardsEventArgs(turnCount, "Player 1"));
                return;
            }

            if (_player2.Count < 2)
            {
                _player2.Clear();
                OnWarNotEnoughCards?.Invoke(this, new NotEnoughCardsEventArgs(turnCount, "Player 2"));
                return;
            }

            var player1DownCard = _player1.Dequeue();
            var player1UpCard = _player1.Dequeue();

            var player2DownCard = _player2.Dequeue();
            var player2UpCard = _player2.Dequeue();

            player1PendingCards.Add(player1DownCard);
            player1PendingCards.Add(player1UpCard);

            player2PendingCards.Add(player2DownCard);
            player2PendingCards.Add(player2UpCard);

            OnWarDealt?.Invoke(this, new StartEventArgs(turnCount, player1UpCard, player2UpCard));

            if (player1UpCard.Value > player2UpCard.Value)
            {
                // To keep things consistent, the winning player always takes their cards first.
                player1PendingCards.AddRange(player2PendingCards);

                foreach (var card in player1PendingCards)
                    _player1.Enqueue(card);

                OnWarFinish?.Invoke(this, new WarFinishEventArgs(turnCount, "Player 1", player1PendingCards.Count));
            }
            else if (player2UpCard.Value > player1UpCard.Value)
            {
                // To keep things consistent, the winning player always takes their cards first.
                player2PendingCards.AddRange(player1PendingCards);

                foreach (var card in player2PendingCards)
                    _player2.Enqueue(card);

                OnWarFinish?.Invoke(this, new WarFinishEventArgs(turnCount, "Player 2", player2PendingCards.Count));
            }
            else
            {
                // Both cards have the same value, so they'll go to war again.
                War(turnCount, player1PendingCards, player2PendingCards);
            }
        }
    }
}