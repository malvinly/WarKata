using System;

namespace WarCardGame.Events
{
    public class NotEnoughCardsEventArgs : EventArgs
    {
        public string Loser { get; }

        public NotEnoughCardsEventArgs(string loser)
        {
            Loser = loser;
        }
    }
}