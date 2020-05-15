using System.Linq;
using NUnit.Framework;

namespace WarCardGame.Tests
{
    [TestFixture]
    public class When_A_New_Deck_Is_Shuffled
    {
        [Test]
        public void Number_Of_Cards_Is_52()
        {
            var deck = new Deck();
            Assert.That(deck.Count() == 52);
        }

        [Test]
        public void Should_Not_Match_Another_Deck()
        {
            var deck1 = new Deck();
            var deck2 = new Deck();

            Assert.That(deck1, Is.Not.EquivalentTo(deck2));
        }

        [Test]
        public void Contains_Correct_Number_Of_Suits()
        {
            var deck = new Deck();

            int clubs = deck.Count(x => x.Suit == Suit.Clubs);
            int diamonds = deck.Count(x => x.Suit == Suit.Diamonds);
            int spades = deck.Count(x => x.Suit == Suit.Spades);
            int hearts = deck.Count(x => x.Suit == Suit.Hearts);

            Assert.That(clubs == 13);
            Assert.That(diamonds == 13);
            Assert.That(spades == 13);
            Assert.That(hearts == 13);
        }

        [Test]
        public void Contains_Correct_Number_Of_Cards_Per_Suit()
        {
            var deck = new Deck();

            var clubs = deck
                .Where(x => x.Suit == Suit.Clubs)
                .OrderBy(x => x.Value)
                .Select(x => x.Value);

            var expected = Enumerable.Range(2, 13);

            Assert.That(expected, Is.EquivalentTo(clubs));
        }
    }
}
