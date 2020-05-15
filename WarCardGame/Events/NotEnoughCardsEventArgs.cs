using System;

namespace WarCardGame.Events
{
    public class NotEnoughCardsEventArgs : EventArgs
    {
        public int Turn { get; set; }
        public string Loser { get; set; }

        public NotEnoughCardsEventArgs(int turn, string loser)
        {
            Loser = loser;
            Turn = turn;
        }
    }
}