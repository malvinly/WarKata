using System;

namespace WarCardGame.Events
{
    public class FinishEventArgs : EventArgs
    {
        public string Winner { get; set; }
        
        public FinishEventArgs(string winner)
        {
            Winner = winner;
        }
    }
}