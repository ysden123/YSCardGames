namespace YSCardGamesLibrary
{
    public class CardDesk
    {
        public Stack<Card> Cards { get; init; } = [];

        public CardDesk()
        {
            string[] suits = { Suit.Hearts, Suit.Spades, Suit.Clubs, Suit.Diamonds };
            var ranks = new[] { "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
            var scoress = new[] {6, 7, 8, 9, 10, 2, 3, 4, 11 };
            foreach (var suit in suits)
            {
                for (int i = 0; i < ranks.Length; i++)
                {
                    Cards.Push(new Card
                    {
                        Name = $"{ranks[i]} {suit}",
                        Suit = suit,
                        Rank = scoress[i]
                    });
                }
            }

            Shuffle();
        }

        public void Shuffle()
        {
            var rand = new Random();
            var cardArray = Cards.ToArray();
            Cards.Clear();
            foreach (var card in cardArray.OrderBy(x => rand.Next()))
            {
                Cards.Push(card);
            }

            cardArray = [.. Cards];
            Cards.Clear();
            foreach (var card in cardArray.OrderBy(x => rand.Next()))
            {
                Cards.Push(card);
            }
        }

        public Card? Next()
        {
            if (Cards.Count == 0)
            {
                return null;
            }
            else
            {
                return Cards.Pop();
            }
        }
    }
}
