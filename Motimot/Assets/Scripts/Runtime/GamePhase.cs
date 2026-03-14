namespace Motimot
{
    /// <summary>
    /// Current phase of the session: still playing, won, or lost.
    /// </summary>
    public enum GamePhase
    {
        /// <summary>Session in progress; player can submit more guesses.</summary>
        InProgress,

        /// <summary>Player guessed the hidden word.</summary>
        Won,

        /// <summary>Player used all attempts without guessing the word.</summary>
        Lost
    }
}
