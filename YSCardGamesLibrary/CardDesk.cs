using System;
using System.Collections.Generic;
using System.Text;

namespace YSCardGamesLibrary
{
    public class CardDesk
    {
        public Stack<Card> Cards { get; init; } = [];

        public CardDesk()
        {
            var suits = new[] { "\u2665", "\u2666", "\u2663", "\u2660" };
            var ranks = new[] { "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };
            var scoress = new[] {6, 7, 8, 9, 10, 2, 3, 4, 11 };
            foreach (var suit in suits)
            {
                for (int i = 0; i < ranks.Length; i++)
                {
                    Cards.Push(new Card
                    {
                        Name = $"{ranks[i]} of {suit}",
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
