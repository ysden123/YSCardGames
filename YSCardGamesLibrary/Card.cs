using System;
using System.Collections.Generic;
using System.Text;

namespace YSCardGamesLibrary
{
    public record Card
    {
        public required string Name { get; init; }
        public required string Suit { get; init; }
        public required int Rank { get; init; }
    }
}
