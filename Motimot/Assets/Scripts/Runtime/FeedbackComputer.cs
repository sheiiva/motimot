using System;

namespace Motimot
{
    /// <summary>
    /// Computes per-letter feedback (CorrectPosition / WrongPosition / Absent) for a guess against the hidden word.
    /// Handles repeated letters: green takes priority over yellow; each occurrence in the hidden word is consumed at most once.
    /// </summary>
    public static class FeedbackComputer
    {
        /// <summary>
        /// Computes feedback for each letter of <paramref name="guess"/> against <paramref name="hiddenWord"/>.
        /// Both must have length <see cref="GameConstants.WordLength"/>.
        /// </summary>
        public static Tile[] Compute(string guess, string hiddenWord)
        {
            if (guess == null || hiddenWord == null ||
                guess.Length != GameConstants.WordLength ||
                hiddenWord.Length != GameConstants.WordLength)
            {
                throw new ArgumentException("Guess and hiddenWord must have length WordLength.");
            }

            string g = guess.ToLowerInvariant();
            string h = hiddenWord.ToLowerInvariant();

            var feedback = new LetterFeedback[GameConstants.WordLength];
            var hiddenUsed = new bool[GameConstants.WordLength];

            // First pass: correct position (green)
            for (int i = 0; i < GameConstants.WordLength; i++)
            {
                if (g[i] == h[i])
                {
                    feedback[i] = LetterFeedback.CorrectPosition;
                    hiddenUsed[i] = true;
                }
            }

            // Second pass: wrong position (yellow) or absent (gray)
            for (int i = 0; i < GameConstants.WordLength; i++)
            {
                if (feedback[i] == LetterFeedback.CorrectPosition)
                {
                    continue;
                }

                bool foundYellow = false;
                for (int j = 0; j < GameConstants.WordLength && !foundYellow; j++)
                {
                    if (!hiddenUsed[j] && h[j] == g[i])
                    {
                        feedback[i] = LetterFeedback.WrongPosition;
                        hiddenUsed[j] = true;
                        foundYellow = true;
                    }
                }

                if (!foundYellow)
                {
                    feedback[i] = LetterFeedback.Absent;
                }
            }

            var tiles = new Tile[GameConstants.WordLength];
            for (int i = 0; i < GameConstants.WordLength; i++)
            {
                tiles[i] = new Tile(g[i], feedback[i], i);
            }

            return tiles;
        }
    }
}
