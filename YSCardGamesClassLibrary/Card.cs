using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YSCardGamesClassLibrary
{
    public record Card
    {
        public required string Name { get; init; }
    }
}
