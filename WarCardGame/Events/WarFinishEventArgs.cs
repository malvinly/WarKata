using System;

namespace WarCardGame.Events
{
    public class WarFinishEventArgs : EventArgs
    {
        public string Winner { get; set; }
        public int WinningCardCount { get; set; }

        public WarFinishEventArgs(string winner, int winningCardCount)
        {
            Winner = winner;
            WinningCardCount = winningCardCount;
        }
    }
}