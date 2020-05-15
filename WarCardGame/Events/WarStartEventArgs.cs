using System;

namespace WarCardGame.Events
{
    public class WarStartEventArgs : EventArgs
    {
        public int Turn { get; set; }

        public WarStartEventArgs(int turn)
        {
            Turn = turn;
        }
    }
}