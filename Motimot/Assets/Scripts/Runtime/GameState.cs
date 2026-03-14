using System.Collections.Generic;

namespace Motimot
{
    /// <summary>
    /// Full game state for one session: hidden word, submitted attempts (rows with feedback),
    /// current row in progress, and win/lose/in-progress phase.
    /// </summary>
    public sealed class GameState
    {
        /// <summary>The word the player must guess (one per session).</summary>
        public string HiddenWord { get; }

        /// <summary>Submitted guesses, each a row of tiles with feedback.</summary>
        public IReadOnlyList<Row> Attempts { get; }

        /// <summary>Letters typed so far for the current attempt (length 0 to <see cref="GameConstants.WordLength"/>). No feedback until submitted.</summary>
        public string CurrentRowLetters { get; }

        /// <summary>Whether the session is in progress, won, or lost.</summary>
        public GamePhase Phase { get; }

        public GameState(
            string hiddenWord,
            IReadOnlyList<Row> attempts,
            string currentRowLetters,
            GamePhase phase)
        {
            HiddenWord = hiddenWord ?? string.Empty;
            Attempts = attempts ?? new List<Row>();
            CurrentRowLetters = currentRowLetters ?? string.Empty;
            Phase = phase;
        }

        /// <summary>Number of submitted attempts so far.</summary>
        public int AttemptsCount => Attempts.Count;
    }
}
