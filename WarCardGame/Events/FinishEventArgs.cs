using System;

namespace WarCardGame.Events
{
    public class FinishEventArgs : EventArgs
    {
        public int Turn { get; set; }
        public string Winner { get; set; }
        
        public FinishEventArgs(int turn, string winner)
        {
            Winner = winner;
            Turn = turn;
        }
    }
}