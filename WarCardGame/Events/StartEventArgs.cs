using System;

namespace WarCardGame.Events
{
    public class StartEventArgs : EventArgs
    {
        public int Turn { get; set; }
        public Card Player1Card { get; set; }
        public Card Player2Card { get; set; }

        public StartEventArgs(int turn, Card player1Card, Card player2Card)
        {
            Turn = turn;
            Player1Card = player1Card;
            Player2Card = player2Card;
        }
    }
}