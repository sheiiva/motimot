namespace Motimot
{
    /// <summary>
    /// One cell in the grid: a letter plus its feedback state (and optional position in row).
    /// </summary>
    public readonly struct Tile
    {
        /// <summary>The letter shown in this tile.</summary>
        public char Letter { get; }

        /// <summary>Feedback for this letter (correct position / wrong position / absent).</summary>
        public LetterFeedback Feedback { get; }

        /// <summary>Zero-based index of this tile within its row (optional).</summary>
        public int IndexInRow { get; }

        public Tile(char letter, LetterFeedback feedback, int indexInRow = 0)
        {
            Letter = letter;
            Feedback = feedback;
            IndexInRow = indexInRow;
        }
    }
}
