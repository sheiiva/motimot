using System;
using System.Collections.Generic;

namespace Motimot
{
    /// <summary>
    /// One full guess: an ordered line of tiles (length = <see cref="GameConstants.WordLength"/>).
    /// </summary>
    public sealed class Row
    {
        private readonly Tile[] _tiles;

        /// <summary>Ordered tiles for this guess (read-only).</summary>
        public IReadOnlyList<Tile> Tiles => _tiles;

        /// <summary>Creates a row from exactly <see cref="GameConstants.WordLength"/> tiles.</summary>
        /// <exception cref="ArgumentException">Thrown when <paramref name="tiles"/> length is not <see cref="GameConstants.WordLength"/>.</exception>
        public Row(Tile[] tiles)
        {
            if (tiles == null || tiles.Length != GameConstants.WordLength)
            {
                throw new ArgumentException(
                    $"Row must have exactly {GameConstants.WordLength} tiles.",
                    nameof(tiles));
            }

            _tiles = new Tile[GameConstants.WordLength];
            for (int i = 0; i < GameConstants.WordLength; i++)
            {
                _tiles[i] = new Tile(tiles[i].Letter, tiles[i].Feedback, i);
            }
        }
    }
}
