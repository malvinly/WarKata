using System;
using System.Text;

namespace WarCardGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // Setting the output to support Unicode characters for card suits.
            Console.OutputEncoding = Encoding.UTF8;
            int numberOfGames;
            int totalNumberOfGames = 0;

            while (true)
            {
                Console.WriteLine("If you play 1 game, detailed turn-by-turn messages will be displayed.");
                Console.Write("How many games do you want to play? ");
                string input = Console.ReadLine();

                if (!Int32.TryParse(input, out numberOfGames))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\n\"{input}\" isn't a number.");
                    Console.ResetColor();
                    continue;
                }

                if (numberOfGames <= 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nNumber must be greater than 0.");
                    Console.ResetColor();
                    continue;
                }

                break;
            }

            bool verboseMode = numberOfGames == 1;

            do
            {
                Console.Clear();

                for (int i = 0; i < numberOfGames; i++)
                {
                    Console.Write($"Game #{totalNumberOfGames++}: ");

                    InitGame(verboseMode)
                        .Play();
                }

                Console.WriteLine("\nPress any key to play again. Press {ENTER} to quit.");
            }
            while (Console.ReadKey().Key != ConsoleKey.Enter);
        }

        private static Game InitGame(bool verboseMode)
        {
            var game = new Game();

            if (verboseMode)
            {
                game.OnTurnStart += (sender, eventArgs) =>
                {
                    Console.WriteLine($"Turn #{eventArgs.Turn}. Player 1 has {game.Player1CardCount + 1} cards and player 2 has {game.Player2CardCount + 1} cards.");
                    Console.WriteLine($"\tPlayer 1 ({eventArgs.Player1Card}) vs Player 2 ({eventArgs.Player2Card})");
                };

                game.OnTurnFinish += (sender, eventArgs) =>
                {
                    Console.WriteLine($"\t{eventArgs.Winner} wins.\n");
                };

                game.OnWarStart += (sender, eventArgs) =>
                {
                    Console.Write("\tWar! ");
                };

                game.OnWarNotEnoughCards += (sender, eventArgs) =>
                {
                    Console.WriteLine($"{eventArgs.Loser} ran out of cards.\n");
                };

                game.OnWarDealt += (sender, eventArgs) =>
                {
                    Console.WriteLine($"Player 1 ({eventArgs.Player1Card}) vs Player 2 ({eventArgs.Player2Card})");
                };

                game.OnWarFinish += (sender, eventArgs) =>
                {
                    Console.WriteLine($"\t{eventArgs.Winner} wins and took {eventArgs.WinningCardCount} cards.\n");
                };
            }

            game.OnGameFinish += (sender, eventArgs) =>
            {
                if (eventArgs.Winner == "Tie")
                    Console.WriteLine($"We'll call it a tie after {eventArgs.Turn} turns.");
                else
                    Console.WriteLine($"Winner is {eventArgs.Winner} after {eventArgs.Turn} turns.");
            };

            return game;
        }
    }
}