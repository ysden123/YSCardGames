namespace YSCardGamesLibraryTest
{
    [TestClass]
    public sealed class CardDeskTest
    {
        [TestMethod]
        public void TestShuffle()
        {
            var desk = new YSCardGamesLibrary.CardDesk();
            var originalOrder = desk.Cards.ToArray();
            desk.Shuffle();
            var shuffledOrder = desk.Cards.ToArray();
            // Check that the order has changed
            bool isDifferent = false;
            for (int i = 0; i < originalOrder.Length; i++)
            {
                if (originalOrder[i] != shuffledOrder[i])
                {
                    isDifferent = true;
                    break;
                }
            }
            Assert.IsTrue(isDifferent, "The card order should be different after shuffling.");

            // Check that all cards are still present
            CollectionAssert.AreEquivalent(originalOrder, shuffledOrder, "All cards should be present after shuffling.");

/*            foreach (var card in desk.Cards)
            {
                Console.WriteLine($"{card}");
            }
*/        }

        [TestMethod]
        public void TestNext()
        {
            var desk = new YSCardGamesLibrary.CardDesk();
            int initialCount = desk.Cards.Count;
            var card = desk.Next();
            Assert.IsNotNull(card, "Next() should return a card.");
            Assert.AreEqual(initialCount - 1, desk.Cards.Count, "The card count should decrease by one after calling Next().");

            while (desk.Next() != null)
            {
            }

            Assert.IsNull(desk.Next(), "Next() should return null when there are no cards left.");
            Assert.AreEqual(0, desk.Cards.Count, "The card count should be zero when all cards have been drawn.");
        }
    }
}
