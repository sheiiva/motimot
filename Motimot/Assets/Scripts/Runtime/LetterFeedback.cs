namespace Motimot
{
    /// <summary>
    /// Feedback state for a single letter in a guess (green / yellow / gray).
    /// </summary>
    public enum LetterFeedback
    {
        /// <summary>Letter is correct and in the right position (green).</summary>
        CorrectPosition,

        /// <summary>Letter is in the hidden word but in the wrong position (yellow).</summary>
        WrongPosition,

        /// <summary>Letter is not in the hidden word (gray).</summary>
        Absent
    }
}
