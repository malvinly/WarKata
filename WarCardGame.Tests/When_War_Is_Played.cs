using System.Runtime.Remoting;
using Moq;
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
            var player1 = CreateMockPlayer(new Card(4, Suit.Diamonds), new Card(8, Suit.Diamonds));
            var player2 = CreateMockPlayer(new Card(3, Suit.Diamonds), new Card(7, Suit.Diamonds));
            
            var game = new Game(player1.Object, player2.Object);

            game.PlayTurn();
            
            player1.Verify(x => x.AddCards(It.Is<Card[]>(c => c.Length == 2)), Times.Once);
            Assert.That(player1.Object.CardsLeft == 3);
            Assert.That(player2.Object.CardsLeft == 1);

            game.PlayTurn();

            player1.Verify(x => x.AddCards(It.Is<Card[]>(c => c.Length == 2)), Times.Exactly(2));
            Assert.That(player1.Object.CardsLeft == 4);
            Assert.That(player2.Object.CardsLeft == 0);
        }

        [Test]
        [Repeat(100)]
        public void Game_Should_End_When_Player_Runs_Out_Of_Cards()
        {
            var player1 = CreateMockPlayer(new Card(4, Suit.Diamonds));
            var player2 = CreateMockPlayer(new Card(3, Suit.Diamonds));
            bool gameFinished = false;

            var game = new Game(player1.Object, player2.Object);
            game.OnGameFinish += (sender, args) => { gameFinished = true; };
            game.PlayTurn();
            
            Assert.IsTrue(gameFinished);
        }

        [Test]
        [Repeat(100)]
        public void War_Should_Occur_When_Cards_Are_Equal()
        {
            var player1 = CreateMockPlayer(new Card(7, Suit.Diamonds), new Card(2, Suit.Clubs), new Card(3, Suit.Clubs));
            var player2 = CreateMockPlayer(new Card(7, Suit.Hearts), new Card(5, Suit.Hearts), new Card(6, Suit.Hearts));

            var game = new Game(player1.Object, player2.Object);
            game.PlayTurn();

            player1.Verify(x => x.PlayWar(), Times.Once);
            player2.Verify(x => x.PlayWar(), Times.Once);
        }

        [Test]
        [Repeat(100)]
        public void Winner_Of_War_Should_Be_Determined_By_Higher_Card()
        {
            var player1 = CreateMockPlayer(new Card(7, Suit.Diamonds), new Card(2, Suit.Clubs), new Card(3, Suit.Clubs));
            var player2 = CreateMockPlayer(new Card(7, Suit.Hearts), new Card(5, Suit.Hearts), new Card(6, Suit.Hearts));
            bool warFinished = false;

            var game = new Game(player1.Object, player2.Object);

            game.OnWarFinish += (sender, args) =>
            {
                warFinished = true;
                Assert.That(args.Winner == "Player 2");
                Assert.That(args.WinningCardCount == 6);
            };

            game.PlayTurn();
            
            Assert.IsTrue(warFinished);
        }

        [Test]
        [Repeat(100)]
        public void When_Multiple_Wars_Happen_Then_Winner_Should_Take_More_Cards()
        {
            var player1 = CreateMockPlayer(new Card(7, Suit.Diamonds), new Card(3, Suit.Diamonds), new Card(8, Suit.Diamonds), new Card(9, Suit.Clubs), new Card(5, Suit.Clubs));
            var player2 = CreateMockPlayer(new Card(7, Suit.Hearts), new Card(2, Suit.Hearts), new Card(8, Suit.Hearts), new Card(8, Suit.Clubs), new Card(6, Suit.Clubs));
            bool warFinished = false;

            var game = new Game(player1.Object, player2.Object);

            game.OnWarFinish += (sender, args) =>
            {
                warFinished = true;
                Assert.That(args.Winner == "Player 2");
                Assert.That(args.WinningCardCount == 10);
            };

            game.PlayTurn();

            player1.Verify(x => x.PlayWar(), Times.Exactly(2));
            player2.Verify(x => x.PlayWar(), Times.Exactly(2));
            Assert.IsTrue(warFinished);
        }

        [Test]
        [Repeat(100)]
        public void Game_Should_End_When_Player_Does_Not_Have_Enough_Cards_To_Finish_War()
        {
            var player1 = CreateMockPlayer(new Card(7, Suit.Diamonds), new Card(2, Suit.Clubs), new Card(3, Suit.Clubs));
            var player2 = CreateMockPlayer(new Card(7, Suit.Hearts), new Card(5, Suit.Hearts));
            bool notEnoughCards = false;
            bool gameFinished = false;

            var game = new Game(player1.Object, player2.Object);
            
            game.OnWarNotEnoughCards += (sender, args) =>
            {
                notEnoughCards = true;
                Assert.That(args.Loser == "Player 2");
            };

            game.OnGameFinish += (sender, args) =>
            {
                gameFinished = true;
                Assert.That(args.Winner == "Player 1");
            };

            game.PlayTurn();

            Assert.IsTrue(notEnoughCards);
            Assert.IsTrue(gameFinished);
        }

        private Mock<Player> CreateMockPlayer(params Card[] cards)
        {
            var mock = new Mock<Player>();

            mock.CallBase = true;
            mock.Object.AddCards(cards);

            // Reset the call invocation count so the previous AddCards won't impact our tests.
            mock.Reset();
            mock.CallBase = true;

            return mock;
        }
    }
}