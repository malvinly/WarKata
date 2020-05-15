using System.Collections.Generic;
using NUnit.Framework;

namespace WarCardGame.Tests
{
    [TestFixture]
    public class When_A_Player
    {
        [Test]
        public void Runs_Out_Of_Cards_Then_They_Cant_Show_A_Card()
        {
            var player = new Player();
            player.AddCards(new Card(5, Suit.Hearts));

            Card first = player.PlayCard();

            Assert.That(first.Value == 5);
            Assert.That(first.Suit == Suit.Hearts);

            Card second = player.PlayCard();

            Assert.IsNull(second);
        }

        [Test]
        public void Can_Go_To_War_With_2_Cards()
        {
            var player = new Player();
            player.AddCards(new Card(5, Suit.Hearts), new Card(6, Suit.Diamonds));

            List<Card> cards = player.PlayWar();

            Assert.That(cards[0].Value == 5);
            Assert.That(cards[0].Suit == Suit.Hearts);

            Assert.That(cards[1].Value == 6);
            Assert.That(cards[1].Suit == Suit.Diamonds);
        }

        [Test]
        public void Cant_Go_To_War_Without_2_Cards()
        {
            var player = new Player();
            player.AddCards(new Card(5, Suit.Hearts));

            List<Card> cards = player.PlayWar();

            Assert.IsNull(cards);
        }
    }
}