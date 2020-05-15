using System;
using System.Collections.Generic;
using System.Linq;
using WarCardGame.Events;

namespace WarCardGame
{
    public class Game
    {
        private readonly Player _player1;
        private readonly Player _player2;

        public int TurnCount { get; private set; }
        private int _maxTurnCount = 10000;

        public event EventHandler<StartEventArgs> OnTurnStart;
        public event EventHandler<FinishEventArgs> OnTurnFinish;
        public event EventHandler OnWarStart;
        public event EventHandler<StartEventArgs> OnWarDealt;
        public event EventHandler<NotEnoughCardsEventArgs> OnWarNotEnoughCards;
        public event EventHandler<WarFinishEventArgs> OnWarFinish;
        public event EventHandler<FinishEventArgs> OnGameFinish;

        private static readonly Random Random = new Random();
        
        public Game(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;
        }

        public void PlayTurn()
        {
            TurnCount++;

            if (!GameCanContinue())
                return;

            Card player1Card = _player1.PlayCard();
            Card player2Card = _player2.PlayCard();

            OnTurnStart?.Invoke(this, new StartEventArgs(player1Card, player2Card));

            if (player1Card.Value > player2Card.Value)
            {
                _player1.AddCards(player1Card, player2Card);
                OnTurnFinish?.Invoke(this, new FinishEventArgs("Player 1"));
            }
            else if (player2Card.Value > player1Card.Value)
            {
                _player2.AddCards(player2Card, player1Card);
                OnTurnFinish?.Invoke(this, new FinishEventArgs("Player 2"));
            }
            else
            {
                // Both cards have the same value, so they'll go to war.
                War(new List<Card> { player1Card }, new List<Card> { player2Card });
            }

            if (TurnCount >= _maxTurnCount)
                OnGameFinish?.Invoke(this, new FinishEventArgs("Tie"));

            if (_player1.CardsLeft <= 0)
                OnGameFinish?.Invoke(this, new FinishEventArgs("Player 2"));

            if (_player2.CardsLeft <= 0)
                OnGameFinish?.Invoke(this, new FinishEventArgs("Player 1"));
        }

        public bool GameCanContinue()
        {
            if (TurnCount >= _maxTurnCount || _player1.CardsLeft <= 0 || _player2.CardsLeft <= 0)
                return false;

            return true;
        }

        private void War(List<Card> player1PendingCards, List<Card> player2PendingCards)
        {
            OnWarStart?.Invoke(this, null);

            // Players must have enough cards to place 1 card face down and 1 card face up during war.
            if (_player1.CardsLeft < 2)
            {
                _player1.RemoveAllCards();
                OnWarNotEnoughCards?.Invoke(this, new NotEnoughCardsEventArgs("Player 1"));
                return;
            }

            if (_player2.CardsLeft < 2)
            {
                _player2.RemoveAllCards();
                OnWarNotEnoughCards?.Invoke(this, new NotEnoughCardsEventArgs("Player 2"));
                return;
            }

            List<Card> player1Cards = _player1.PlayWar();
            List<Card> player2Cards = _player2.PlayWar();
            
            player1PendingCards.AddRange(player1Cards);
            player2PendingCards.AddRange(player2Cards);

            Card player1UpCard = player1Cards.Last();
            Card player2UpCard = player2Cards.Last();

            OnWarDealt?.Invoke(this, new StartEventArgs(player1UpCard, player2UpCard));

            if (player1UpCard.Value > player2UpCard.Value)
            {
                // To keep things consistent, the winning player always takes their cards first.
                player1PendingCards.AddRange(player2PendingCards);

                foreach (var card in player1PendingCards)
                    _player1.AddCards(card);

                OnWarFinish?.Invoke(this, new WarFinishEventArgs("Player 1", player1PendingCards.Count));
            }
            else if (player2UpCard.Value > player1UpCard.Value)
            {
                // To keep things consistent, the winning player always takes their cards first.
                player2PendingCards.AddRange(player1PendingCards);

                foreach (var card in player2PendingCards)
                    _player2.AddCards(card);

                OnWarFinish?.Invoke(this, new WarFinishEventArgs("Player 2", player2PendingCards.Count));
            }
            else
            {
                // Both cards have the same value, so they'll go to war again.
                War(player1PendingCards, player2PendingCards);
            }
        }
    }
}