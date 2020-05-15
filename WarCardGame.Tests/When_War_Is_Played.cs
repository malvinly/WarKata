using NUnit.Framework;

namespace WarCardGame.Tests
{
    [TestFixture]
    public class When_Game_Is_Played
    {
        [Test]
        [Repeat(100)]
        public void Winner_Should_Be_Determined_By_Higher_Card()
        {
            var game = new Game();

            string expectedWinner = null;
            string actualWinner;

            game.OnTurnStart += (sender, args) =>
            {
                expectedWinner = args.Player1Card.Value > args.Player2Card.Value ? "Player 1" : "Player 2";
            };

            game.OnTurnFinish += (sender, args) =>
            {
                actualWinner = args.Winner;

                Assert.That(expectedWinner == actualWinner);
            };

            game.Play();
        }

        [Test]
        [Repeat(100)]
        public void Winning_Turn_Should_Increase_Card_Count_By_2()
        {
            var game = new Game();

            int player1CardCount = 0;
            int player2CardCount = 0;

            game.OnTurnStart += (sender, args) =>
            {
                player1CardCount = game.Player1CardCount;
                player2CardCount = game.Player2CardCount;
            };

            game.OnTurnFinish += (sender, args) =>
            {
                if (args.Winner == "Player 1")
                    Assert.That(game.Player1CardCount == player1CardCount + 2);
                else
                    Assert.That(game.Player2CardCount == player2CardCount + 2);
            };

            game.Play();
        }

        [Test]
        [Repeat(100)]
        public void War_Should_Occur_When_Cards_Are_Equal()
        {
            var game = new Game();

            bool war = false;
            int turn = 0;

            game.OnTurnStart += (sender, args) =>
            {
                if (args.Player1Card.Value == args.Player2Card.Value)
                {
                    war = true;
                    turn = args.Turn;
                }
            };

            game.OnWarStart += (sender, args) =>
            {
                // War should occur during the same turn when the cards values were equal.
                Assert.That(args.Turn == turn);
                Assert.IsTrue(war);
            };

            game.Play();
        }

        [Test]
        [Repeat(100)]
        public void Winner_Of_War_Should_Be_Determined_By_Higher_Card()
        {
            var game = new Game();

            string expectedWinner = null;
            string actualWinner;

            game.OnWarDealt += (sender, args) =>
            {
                expectedWinner = args.Player1Card.Value > args.Player2Card.Value ? "Player 1" : "Player 2";
            };

            game.OnWarFinish += (sender, args) =>
            {
                actualWinner = args.Winner;

                Assert.That(expectedWinner == actualWinner);
            };

            game.Play();
        }

        [Test]
        [Repeat(100)]
        public void Winning_War_Should_Increase_Card_Count_By_Multiples()
        {
            var game = new Game();

            int player1CardCount = 0;
            int player2CardCount = 0;
            int warCount = 0;

            game.OnTurnStart += (sender, args) =>
            {
                player1CardCount = game.Player1CardCount;
                player2CardCount = game.Player2CardCount;
            };

            game.OnWarStart += (sender, args) =>
            {
                warCount++;
            };

            game.OnWarFinish += (sender, args) =>
            {
                // Depending on how many times War occurs in a row, we need to subtract the 2 cards we place down during each War occurrence.
                // For example, if a player started with 20 cards, they should have 18 after placing down 2 cards. If they win, they'll
                // receive their 2 War cards, the opponent's 2 War cards, and the original turn's 2 cards.
                // The formula for the number of cards gained should be 2 + (WarCount * 4).

                if (args.Winner == "Player 1")
                {
                    player1CardCount = player1CardCount - (warCount * 2);
                    Assert.That(game.Player1CardCount == player1CardCount + args.WinningCardCount);
                }
                else
                {
                    player2CardCount = player2CardCount - (warCount * 2);
                    Assert.That(game.Player2CardCount == player2CardCount + args.WinningCardCount);
                }

                warCount = 0;
            };

            game.Play();
        }

        [Test]
        [Repeat(100)]
        public void Game_Should_End_When_Player_Does_Not_Have_Enough_Cards_To_Finish_War()
        {
            var game = new Game();

            int player1CardCount = 0;
            int player2CardCount = 0;

            game.OnWarStart += (sender, args) =>
            {
                player1CardCount = game.Player1CardCount;
                player2CardCount = game.Player2CardCount;
            };

            game.OnWarNotEnoughCards += (sender, args) =>
            {
                if (args.Loser == "Player 1")
                    Assert.That(player1CardCount < 2);
                else
                    Assert.That(player2CardCount < 2);
            };

            game.Play();
        }

        [Test]
        [Repeat(100)]
        public void Game_Should_End_When_Player_Runs_Out_Of_Cards()
        {
            var game = new Game();

            game.OnGameFinish += (sender, args) =>
            {
                Assert.That(game.Player1CardCount == 0 || game.Player2CardCount == 0);
            };

            game.Play();
        }
    }
}