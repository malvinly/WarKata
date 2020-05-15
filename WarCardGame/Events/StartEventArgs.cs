using System;

namespace WarCardGame.Events
{
    public class StartEventArgs : EventArgs
    {
        public Card Player1Card { get; set; }
        public Card Player2Card { get; set; }

        public StartEventArgs(Card player1Card, Card player2Card)
        {
            Player1Card = player1Card;
            Player2Card = player2Card;
        }
    }
}