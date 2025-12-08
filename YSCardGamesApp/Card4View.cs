using System.Windows.Media;

namespace YSCardGamesApp
{
    internal record Card4View
    {
        public required string Name { get; init; }
        public required Brush TextColor { get; init; }
    }
}
