using System;

namespace WarCardGame.Events
{
    public class WarFinishEventArgs : EventArgs
    {
        public int Turn { get; }
        public string Winner { get; set; }
        public int WinningCardCount { get; set; }

        public WarFinishEventArgs(int turn, string winner, int winningCardCount)
        {
            Turn = turn;
            Winner = winner;
            WinningCardCount = winningCardCount;
        }
    }
}